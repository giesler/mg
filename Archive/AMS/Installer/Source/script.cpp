#include <windows.h>
#include <stdio.h>
#include <shlobj.h>
#include "tokens.h"
#include "build.h"
#include "util.h"
#include "exedata.h"

#define MAX_INCLUDEDEPTH 10
#define MAX_LINELENGTH 4096

int CEXEBuild::process_script(FILE *fp, char *curfilename, int *lineptr)
{
  if (has_called_write_output)
  {
    printf("Error (process_script): write_output already called, can't continue\n");
    return PS_ERROR;
  }
  int ret=parseScript(fp,curfilename,lineptr,0);
  if (ret == PS_ENDIF) printf("!endif: stray !endif\n");
  if (IS_PS_ELSE(ret)) printf("!else: stray !else\n");
  if (m_linebuild.getlen())
  {
    printf("Error: invalid script: last line ended with \\\n");
    return PS_ERROR;
  }
  return ret;
}


int CEXEBuild::parseLine(char *str, FILE *fp, char *curfilename, int *lineptr, int ignore)
{
  LineParser line;
  int res;
  
  // remove trailing slash and null

  if (m_linebuild.getlen()>1) m_linebuild.resize(m_linebuild.getlen()-2);

  m_linebuild.add(str,strlen(str)+1);

  if (str[0] && str[strlen(str)-1] == '\\') return PS_OK;

  res=line.parse((char*)m_linebuild.get());

  m_linebuild.resize(0);

  if (res)
  {
    if (res==-2) printf("Error: unterminated string parsing line at %s:%d\n",curfilename,*lineptr);
    else printf("Error: error parsing line (%s:%d)\n",curfilename,*lineptr);
    return PS_ERROR;
  }

parse_again:
  if (line.getnumtokens() < 1) return PS_OK;

  int np,op;
  int tkid=get_commandtoken(line.gettoken_str(0),&np,&op);
  if (tkid == -1)
  {
    char *p=line.gettoken_str(0);
    if (p[0] && p[strlen(p)-1]==':')
    {
      if (!ignore)
      {
        int a=add_label(line.gettoken_str(0));
        if (a) return PS_ERROR;
      }
      line.eattoken();
      goto parse_again;
    }
    printf("Invalid command: %s\n",line.gettoken_str(0));
    return PS_ERROR;
  }

  int v=line.getnumtokens()-(np+1);
  if (v < 0 || (op >= 0 && v > op)) // opt_parms is -1 for unlimited
  {
    printf("%s expects %d",line.gettoken_str(0),np);
    if (op < 0) printf("+");
    if (op > 0) printf("-%d",op);
    printf(" parameters, got %d.\n",line.getnumtokens()-1);
    print_help(line.gettoken_str(0));
    return PS_ERROR;
  }

  int is_elseif=0;

  if (!fp && (tkid == TOK_P_ELSE || tkid == TOK_P_IFNDEF || tkid == TOK_P_IFDEF))
  {
    printf("Error: !if/!else/!ifdef can only be specified in file mode, not in command mode.\n");
    return PS_ERROR;
  }

  if (tkid == TOK_P_ELSE) 
  {
    if (line.getnumtokens() == 1) return PS_ELSE;

    line.eattoken();

    int v=line.gettoken_enum(0,"ifdef\0ifndef\0");
    if (v < 0)
    {
      print_help(line.gettoken_str(0));
      return PS_ERROR;
    }
    if (line.getnumtokens() == 1)
    {
      print_help(line.gettoken_str(0));
      return PS_ERROR;
    }
    if (!v) tkid = TOK_P_IFDEF;
    else tkid = TOK_P_IFNDEF;
    is_elseif=1;
  }

  if (tkid == TOK_P_IFNDEF || tkid == TOK_P_IFDEF)
  {
    int istrue=0;
    if (!ignore || is_elseif)
    {
      int mod=0;
      int p;

       // pure left to right precedence. Not too powerful, but useful.
      for (p = 1; p < line.getnumtokens(); p ++)
      {         
        if (p & 1)
        {
          int new_s=(definedlist.find(line.gettoken_str(p),0) >= 0);
          if (tkid == TOK_P_IFNDEF) new_s=!new_s;

          if (mod == 0) istrue = istrue || new_s;
          else istrue = istrue && new_s;
        }
        else
        {
          mod=line.gettoken_enum(p,"|\0&\0\0");
          if (mod == -1)
          {
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }
        }
      }
      if (is_elseif)
      {
        if (istrue) return PS_ELSE_IF1;
        return PS_ELSE_IF0;
      }
    }

    int r;
    int hasexec=0;
    istrue=!istrue;

    for (;;)
    {
      r=parseScript(fp,curfilename,lineptr, ignore || hasexec || istrue);
      if (!istrue) hasexec=1;
      if (r == PS_ELSE_IF0) istrue=1;
      else if (r == PS_ELSE_IF1) istrue=0;
      else break;
    }

    if (r == PS_ELSE)
    {
      r=parseScript(fp,curfilename,lineptr, ignore || hasexec);
      if (IS_PS_ELSE(r))
      {
        printf("!else: stray !else\n");
        return PS_ERROR;
      }
    }
    if (r == PS_EOF)
    {
      printf("!ifdef: open at EOF - need !endif\n");
      return PS_ERROR;
    }
    if (r == PS_ERROR) return r;
    return PS_OK;
  }
  if (tkid == TOK_P_ENDIF) return PS_ENDIF;
  if (!ignore) 
  {
    int ret=doCommand(tkid,line,fp,curfilename,*lineptr);
    if (ret != PS_OK) return ret;
  }
  return PS_OK;
}

int CEXEBuild::parseScript(FILE *fp, char *curfilename, int *lineptr, int ignore)
{
  char str[MAX_LINELENGTH];

  for (;;)
  {
    (*lineptr)++;
    str[0]=0;
    fgets(str,MAX_LINELENGTH,fp);
    if (feof(fp)&&!str[0]) break;

    // remove trailing whitespace
    {
      char *p=str;
      while (*p) p++;
      if (p > str) p--;
      while (p >= str && (*p == '\r' || *p == '\n' || *p == ' ' || *p == '\t')) p--;
      *++p=0;
    }

    GrowBuf linedata;
    // convert $\r, $\n, $\t to their literals
    {
      char *in=str;
      while (*in)
      {
        char c=*in++;
        if (c == '$' && in[0] == '\\')
        {
          if (in[1] == 'r')
          {
            in+=2;
            c='\r';
          }
          else if (in[1] == 'n')
          {
            in+=2;
            c='\n';
          }
        }
        linedata.add(&c,1);
      }
      linedata.add("",1);
    }
    int ret=parseLine((char*)linedata.get(),fp,curfilename,lineptr,ignore);
    if (ret != PS_OK) return ret;
  }
  return PS_EOF;
}

int CEXEBuild::process_jump(char *s, int *offs)
{
  if (!stricmp(s,"0") || !stricmp(s,"")) *offs=0;
  else
  {
    if ((s[0] >= '0' && s[0] <= '9') || s[0] == '-')
    {
      printf("Error: Goto targets cannot begin with 0-9 or -\n");
      return 1;
    }
    *offs=ns_label.add(s,0);
  }
  return 0;
}

int CEXEBuild::doCommand(int which_token, LineParser &line, FILE *fp, char *curfilename, int linecnt)
{
  static char *usrvars="$0\0$1\0$2\0$3\0$4\0$5\0$6\0$7\0$8\0$9\0$INSTDIR\0$OUTDIR\0";
  static char *rootkeys[2] = {
    "HKCR\0HKLM\0HKCU\0HKU\0HKCC\0HKDD\0HKPD\0",
    "HKEY_CLASSES_ROOT\0HKEY_LOCAL_MACHINE\0HKEY_CURRENT_USER\0HKEY_USERS\0HKEY_CURRENT_CONFIG\0HKEY_DYN_DATA\0HKEY_PERFORMANCE_DATA\0"
  };
  static HKEY rootkey_tab[] = {
    HKEY_CLASSES_ROOT,HKEY_LOCAL_MACHINE,HKEY_CURRENT_USER,HKEY_USERS,HKEY_CURRENT_CONFIG,HKEY_DYN_DATA,HKEY_PERFORMANCE_DATA
  };

  entry ent={0,};
  switch (which_token)
  {
    // header flags
    ///////////////////////////////////////////////////////////////////////////////
    case TOK_NAME:
      if (build_header.name_ptr >= 0)
      {
        warning("Name: specified multiple times, wasting space (%s:%d)",curfilename,linecnt);
      }
      build_header.name_ptr=add_string_main(line.gettoken_str(1));
#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
      build_uninst.name_ptr=add_string_uninst(line.gettoken_str(1));
#endif
      printf("Name: \"%s\"\n",line.gettoken_str(1));
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_CAPTION:
      if (build_header.caption_ptr >= 0)
      {
        warning("Caption: specified multiple times, wasting space (%s:%d)",curfilename,linecnt);
      }
      build_header.caption_ptr=add_string_main(line.gettoken_str(1));
      printf("Caption: \"%s\"\n",line.gettoken_str(1));
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_ICON:
      printf("Icon: \"%s\"\n",line.gettoken_str(1));
      if (replace_icon((char*)header_data_new+icon_offset,line.gettoken_str(1)))
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_UNINSTICON:
#ifndef NSIS_CONFIG_UNINSTALL_SUPPORT
      printf("UninstallIcon: installer not built with NSIS_CONFIG_UNINSTALL_SUPPORT\n");
      return PS_ERROR;
#else
      printf("UninstallIcon: \"%s\"\n",line.gettoken_str(1));
      if (replace_icon((char*)m_unicon_data,line.gettoken_str(1))) 
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
#endif
    case TOK_ENABLEDBITMAP:
      printf("EnabledBitmap: \"%s\"\n",line.gettoken_str(1));
      if (replace_bitmap((char*)header_data_new+enabled_bitmap_offset,line.gettoken_str(1))) 
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_DISABLEDBITMAP:
      printf("DisabledBitmap: \"%s\"\n",line.gettoken_str(1));
      if (replace_bitmap((char*)header_data_new+disabled_bitmap_offset,line.gettoken_str(1))) 
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_DIRTEXT:
      if (build_header.text_ptr >= 0 && line.gettoken_str(1)[0])
      {
        warning("DirText: specified multiple times, wasting space (%s:%d)",curfilename,linecnt);
      }
      build_header.text_ptr=add_string_main(line.gettoken_str(1));
      printf("DirText: \"%s\"\n",line.gettoken_str(1));
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_COMPTEXT:
      if (build_header.componenttext_ptr >= 0 && line.gettoken_str(1)[0])
      {
        warning("ComponentText: specified multiple times, wasting space (%s:%d)",curfilename,linecnt);
      }
      build_header.componenttext_ptr=add_string_main(line.gettoken_str(1));
      printf("ComponentText: \"%s\"\n",line.gettoken_str(1));
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_LICENSETEXT:
      if (build_header.licensetext_ptr >= 0)
      {
        warning("LicenseText: specified multiple times, wasting space (%s:%d)",curfilename,linecnt);
      }
      build_header.licensetext_ptr=add_string_main(line.gettoken_str(1));
      printf("LicenseText: \"%s\"\n",line.gettoken_str(1));
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_LICENSEDATA:
      if (build_header.licensedata_ptr != -1)
      {
        warning("LicenseData: specified multiple times, wasting space (%s:%d)",curfilename,linecnt);
      }
      if (build_header.silent_install)
      {
        warning("LicenseData: SilentInstall enabled, wasting space (%s:%d)",curfilename,linecnt);
      }
      {
        char data[32768];
        FILE *fp;
        int datalen;
        fp=fopen(line.gettoken_str(1),"rb");
        if (!fp)
        {
          printf("LicenseData: open failed \"%s\"\n",line.gettoken_str(1));
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        datalen=fread(data,1,32767,fp);
        if (!feof(fp))
        {
          printf("LicenseData: license must be < 32 kilobytes.\n");
          fclose(fp);
          return PS_ERROR;         
        }
        fclose(fp);
        data[datalen]=0;
        build_header.licensedata_ptr=add_string_main(data);
        printf("LicenseData: \"%s\"      \n",line.gettoken_str(1));
      }
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_UNINSTTEXT:
#ifndef NSIS_CONFIG_UNINSTALL_SUPPORT
      printf("UninstallText: installer not built with NSIS_CONFIG_UNINSTALL_SUPPORT\n");
      return PS_ERROR;
#else
      if (build_uninst.uninstalltext_ptr >= 0)
      {
        warning("UninstallText: specified multiple times, wasting space (%s:%d)",curfilename,linecnt);
      }
      build_uninst.uninstalltext_ptr=add_string_uninst(line.gettoken_str(1));
      printf("UninstallText: \"%s\"\n",line.gettoken_str(1));
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
#endif
    case TOK_SILENTINST:
      build_header.silent_install=line.gettoken_enum(1,"normal\0silent\0silentlog\0");
      if (build_header.silent_install<0)
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
#ifndef NSIS_CONFIG_LOG
      if (build_header.silent_install == 2)
      {
        printf("SilentInstall: silentlog specified, no log support compiled in (use NSIS_CONFIG_LOG)\n");
        return PS_ERROR;
      }
#endif
      printf("SilentInstall: %s\n",line.gettoken_str(1));
      if (build_header.silent_install && build_header.licensedata_ptr != -1)
      {
        warning("SilentInstall: LicenseData already specified. wasting space (%s:%d)",curfilename,linecnt);
      }
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_INSTTYPE:
      {
        int x;
        if (!stricmp(line.gettoken_str(1),"/NOCUSTOM"))
        {
          build_header.no_custom_instmode_flag=1;
          printf("InstType: disabling custom install type\n");
        }
        else if (line.gettoken_str(1)[0]=='/')
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        else
        {
          for (x = 0; x < MAX_INST_TYPES && build_header.install_types_ptr[x]>=0; x ++);
          if (x==MAX_INST_TYPES)
          {
            printf("InstType: no more than %d install types allowed. %d specified\n",MAX_INST_TYPES,MAX_INST_TYPES+1);
            return PS_ERROR;
          }
          else 
          {
            build_header.install_types_ptr[x] = add_string_main(line.gettoken_str(1));
            printf("InstType: %d=\"%s\"\n",x+1,line.gettoken_str(1));
          }
        }
      }
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_OUTFILE:
      strncpy(build_output_filename,line.gettoken_str(1),1024-1);
      printf("OutFile: \"%s\"\n",build_output_filename);
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_INSTDIR:
      if (build_header.install_directory_ptr >= 0)
      {
        warning("InstallDir: specified multiple times. wasting space (%s:%d)",curfilename,linecnt);
      }
      build_header.install_directory_ptr = add_string_main(line.gettoken_str(1));
      printf("InstallDir: \"%s\"\n",line.gettoken_str(1));
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_INSTALLDIRREGKEY: // InstallDirRegKey
      {
        if (build_header.install_reg_key_ptr>= 0)
        {
          warning("InstallRegKey: specified multiple times, wasting space (%s:%d)",curfilename,linecnt);
        }
        int k=line.gettoken_enum(1,rootkeys[0]);
        if (k == -1) k=line.gettoken_enum(1,rootkeys[1]);
        if (k == -1)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        build_header.install_reg_rootkey=(int)rootkey_tab[k];
        build_header.install_reg_key_ptr = add_string_main(line.gettoken_str(2));
        build_header.install_reg_value_ptr = add_string_main(line.gettoken_str(3));
        printf("InstallRegKey: \"%s\\%s\\%s\"\n",line.gettoken_str(1),line.gettoken_str(2),line.gettoken_str(3));
      }
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_UNINSTALLEXENAME:        
#ifndef NSIS_CONFIG_UNINSTALL_SUPPORT
      printf("UninstallExeName: installer not built with NSIS_CONFIG_UNINSTALL_SUPPORT\n");
      return PS_ERROR;
#else
      if (build_header.uninstall_exe_name_ptr>= 0)
      {
        warning("UninstallExeName: specified multiple times, wasting space (%s:%d)",curfilename,linecnt);
      }
      build_header.uninstall_exe_name_ptr=add_string_main(line.gettoken_str(1));
      printf("UninstallExeName: \"%s\"\n",line.gettoken_str(1));       
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
#endif
    case TOK_CRCCHECK:
      build_crcchk=line.gettoken_enum(1,"off\0on\0");
      if (build_crcchk==-1)
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
      printf("CRCCheck: %s\n",line.gettoken_str(1));
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_AUTOCLOSE:
      {
        int k=line.gettoken_enum(1,"false\0true\0");
        if (k == -1)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        build_header.auto_close=k;
        printf("AutoCloseWindow: %s\n",k?"true":"false");
      }
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_SHOWDETAILS:
      {
        int k=line.gettoken_enum(1,"hide\0show\0nevershow\0");
        if (k == -1)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        build_header.show_details=k;
        printf("ShowInstDetails: %s\n",line.gettoken_str(1));
      }
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_DIRSHOW:
      {
        int k=line.gettoken_enum(1,"show\0hide\0");
        if (k == -1)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        build_header.no_show_dirpage=k;
        printf("DirShow: %s\n",k?"hide":"show");
      }
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_BGGRADIENT:
#ifndef NSIS_SUPPORT_BGBG
      printf("BGGradient: support for BGGradient not compiled in (NSIS_SUPPORT_BGBG)\n");
      return PS_ERROR;
#else
      if (line.getnumtokens()==1)
      {
        printf("BGGradient: default colors\n");
        build_header.bg_color1=0;
        build_header.bg_color2=RGB(0,0,255);
      }
      else if (!stricmp(line.gettoken_str(1),"off"))
      {
        build_header.bg_color1=build_header.bg_color2=-1;
        printf("BGGradient: off\n");
        if (line.getnumtokens()>2)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
      }
      else
      {
        char *p = line.gettoken_str(1);
        int v1,v2,v3=-1;
        v1=strtoul(p,&p,16);
        build_header.bg_color1=((v1&0xff)<<16)|(v1&0xff00)|((v1&0xff0000)>>16);
        p=line.gettoken_str(2);
        v2=strtoul(p,&p,16);
        build_header.bg_color2=((v2&0xff)<<16)|(v2&0xff00)|((v2&0xff0000)>>16);

        p=line.gettoken_str(3);
        if (*p)
        {
          if (!stricmp(p,"notext")) build_header.bg_textcolor=-1;
          else
          {
            v3=strtoul(p,&p,16);
            build_header.bg_textcolor=((v3&0xff)<<16)|(v3&0xff00)|((v3&0xff0000)>>16);
          }
        }
        
        printf("BGGradient: %06X->%06X (text=%d)\n",v1,v2,v3);
      }

#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
      build_uninst.bg_color1=build_header.bg_color1;
      build_uninst.bg_color2=build_header.bg_color2;
      build_uninst.bg_textcolor=build_header.bg_textcolor;
#endif
#endif
    return make_sure_not_in_secorfunc(line.gettoken_str(0));
    case TOK_INSTCOLORS:
      {
        char *p = line.gettoken_str(1);
        if (p[0]=='/')
        {
          if (stricmp(p,"/windows") || line.getnumtokens()!=2) 
          {
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }
          build_header.lb_fg=build_header.lb_bg=-1;
          printf("InstallColors: windows default colors\n");
        }
        else
        {
          int v1,v2;
          if (line.getnumtokens()!=3) 
          {
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }
          v1=strtoul(p,&p,16);
          build_header.lb_fg=((v1&0xff)<<16)|(v1&0xff00)|((v1&0xff0000)>>16);
          p=line.gettoken_str(2);
          v2=strtoul(p,&p,16);
          build_header.lb_bg=((v2&0xff)<<16)|(v2&0xff00)|((v2&0xff0000)>>16);
          printf("InstallColors: fg=%06X bg=%06X\n",v1,v2);
        }

#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
        build_uninst.lb_fg=build_header.lb_fg;
        build_uninst.lb_bg=build_header.lb_bg;
#endif
      }
    return make_sure_not_in_secorfunc(line.gettoken_str(0));

    // preprocessor-ish (ifdef/ifndef/else/endif are handled one step out from here)
    ///////////////////////////////////////////////////////////////////////////////
    case TOK_P_DEFINE:
      definedlist.add(line.gettoken_str(1),0);
      printf("!define: \"%s\"\n",line.gettoken_str(1));
    return PS_OK;
    case TOK_P_PACKEXEHEADER:
      strncpy(build_packname,line.gettoken_str(1),sizeof(build_packname)-1);
      strncpy(build_packcmd,line.gettoken_str(2),sizeof(build_packcmd)-1);
      printf("!packhdr: filename=\"%s\", command=\"%s\"\n",
        build_packname, build_packcmd);
    return PS_OK;
    case TOK_P_SYSTEMEXEC:
      {
        char *exec=line.gettoken_str(1);
        int comp=line.gettoken_enum(2,"<\0>\0<>\0=\0ignore\0");
        if (comp == -1)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        int success=0;
        int cmpv=line.gettoken_int(3,&success);
        if (!success && comp != 4)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        printf("!system: \"%s\"\n",exec);
        int ret=system(exec);
        if (comp == 0 && ret < cmpv);
        else if (comp == 1 && ret > cmpv);
        else if (comp == 2 && ret != cmpv);
        else if (comp == 3 && ret == cmpv);
        else if (comp == 4);
        else
        {
          printf("!system: returned %d, aborting\n",ret);
          return PS_ERROR;
        }
        printf("!system: returned %d\n",ret);
      }
    return PS_OK;
    case TOK_P_INCLUDE:
      {
        char *f=line.gettoken_str(1);
        printf("!include: \"%s\"\n",f);
        FILE *incfp=fopen(f,"rt");
        if (!incfp)
        {
          printf("!include: could not open file: \"%s\"\n",f);
          return PS_ERROR;
        }
        static int depth;
        if (depth >= MAX_INCLUDEDEPTH)
        {
          printf("parseScript: too many levels of includes (%d max).\n",MAX_INCLUDEDEPTH);
          return PS_ERROR;
        }
        depth++;
        int lc=0;
        int r=parseScript(incfp,f,&lc,0);
        depth--;
        fclose(incfp);
        if (r != PS_EOF && r != PS_OK)
        {
          if (r == PS_ENDIF) printf("!endif: stray !endif\n");
          if (IS_PS_ELSE(r)) printf("!else: stray !else\n");
          printf("!include: error in script: \"%s\" on line %d\n",f,lc);
          return PS_ERROR;
        }
        printf("!include: closed: \"%s\"\n",f);
      }
    return PS_OK;
    case TOK_P_CD:
      if (!line.gettoken_str(1)[0] || !SetCurrentDirectory(line.gettoken_str(1)))
      {
        printf("!cd: error changing to: \"%s\"\n",line.gettoken_str(1));
        return PS_ERROR;
      }
    return PS_OK;
    case TOK_P_ERROR:
      printf("!error: %s\n",line.gettoken_str(1));
    return PS_ERROR;
    case TOK_P_WARNING:
      warning("!warning: %s (%s:%d)",line.gettoken_str(1),curfilename,linecnt);
    return PS_OK;

    // section/function shit
    ///////////////////////////////////////////////////////////////////////////////

    case TOK_SECTION:
      printf("Section: \"%s\"\n",line.gettoken_str(1));
#ifndef NSIS_CONFIG_UNINSTALL_SUPPORT
      if (!stricmp(line.gettoken_str(1),"uninstall"))
      {
        printf("Error: Uninstall section declared, no NSIS_CONFIG_UNINSTALL_SUPPORT\n");
        return PS_ERROR;
      }
#endif
      if (line.gettoken_str(1)[0]=='-') return add_section("",curfilename,linecnt);
    return add_section(line.gettoken_str(1),curfilename,linecnt);
    case TOK_SECTIONEND:
      printf("SectionEnd\n");
    return section_end();
    case TOK_SECTIONIN:
      {
        printf("SectionIn: ");
        int wt;
        for (wt = 1; wt < line.getnumtokens(); wt ++)
        {
          char *p=line.gettoken_str(wt);
          while (*p)
          {
            int x=*p-'1';
            if (x >= 0 && x < MAX_INST_TYPES)
            {
              if (section_add_flags(1<<x) != PS_OK) return PS_ERROR;
              printf("[%d] ",x);
            }
            p++;
          }
        }
        printf("\n");
      }
    return PS_OK;
    case TOK_SECTIONDIVIDER:
      printf("SectionDivider\n");
    return add_section("-",curfilename,linecnt);

    case TOK_FUNCTION:
      printf("Function: \"%s\"\n",line.gettoken_str(1));
#ifndef NSIS_CONFIG_UNINSTALL_SUPPORT
      if (!strnicmp(line.gettoken_str(1),"un.",3))
      {
        printf("Error: Uninstall function declared, no NSIS_CONFIG_UNINSTALL_SUPPORT\n");
        return PS_ERROR;
      }
#endif
      return add_function(line.gettoken_str(1));
    case TOK_FUNCTIONEND:
      printf("FunctionEnd\n");
    return function_end();

    // flag setters
    ///////////////////////////////////////////////////////////////////////////////

    case TOK_SETDATESAVE:
      build_datesave=line.gettoken_enum(1,"off\0on\0");
      if (build_datesave==-1)
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
      printf("SetDateSave: %s\n",line.gettoken_str(1));    
    return PS_OK;
    case TOK_SETOVERWRITE:
      build_overwrite=line.gettoken_enum(1,"on\0off\0try\0ifnewer\0");
      if (build_overwrite==-1)
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
      printf("SetOverwrite: %s\n",line.gettoken_str(1));
    return PS_OK;
    case TOK_SETCOMPRESS:
      build_compress=line.gettoken_enum(1,"off\0auto\0force\0");
      if (build_compress==-1)
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
      printf("SetCompress: %s\n",line.gettoken_str(1));
    return PS_OK;
    case TOK_DBOPTIMIZE:
      build_optimize_datablock=line.gettoken_enum(1,"off\0on\0");
      if (build_optimize_datablock==-1)
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
      printf("SetDatablockOptimize: %s\n",line.gettoken_str(1));
    return PS_OK;

    // instructions
    ///////////////////////////////////////////////////////////////////////////////
    case TOK_NOP: 
      printf("Nop\n");
      ent.which=EW_NOP;
    return add_entry(&ent);
    case TOK_GOTO: 
      ent.which=EW_NOP;
      if (process_jump(line.gettoken_str(1),&ent.offsets[0])) 
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
      printf("Goto: %s\n",line.gettoken_str(1));
    return add_entry(&ent);
    case TOK_CALL:
      if (!line.gettoken_str(1)[0])
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
      if (uninstall_mode && strnicmp(line.gettoken_str(1),"un.",3))
      {
        printf("Call must be used with function names starting with \"un.\" in the uninstall section.\n");
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
      if (!uninstall_mode && !strnicmp(line.gettoken_str(1),"un.",3))
      {
        printf("Call must not be used with functions starting with \"un.\" in the non-uninstall sections.\n");
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
#endif
      ent.which=EW_CALL;
      ent.offsets[0]=ns_func.add(line.gettoken_str(1),0);
      printf("Call \"%s\"\n",line.gettoken_str(1));
    return add_entry(&ent);
    case TOK_SETOUTPATH:
      {
        char *p=line.gettoken_str(1);
        if (*p == '-') cur_out_path[0]=0;
        else
        { 
          if (p[0] == '\\' && p[1] != '\\') p++;
          strncpy(cur_out_path,p,1024-1);
          if (cur_out_path[strlen(cur_out_path)-1]=='\\') 
            cur_out_path[strlen(cur_out_path)-1]=0; // remove trailing slash
        }
        if (!cur_out_path[0]) strcpy(cur_out_path,"$INSTDIR");
        printf("SetOutPath: \"%s\"\n",cur_out_path);
        ent.which=EW_SETOUTPUTDIR;
        ent.offsets[0]=add_string(cur_out_path);
      }
    return add_entry(&ent);
    case TOK_CREATEDIR:
      {
        char out_path[1024];
        char *p=line.gettoken_str(1);
        if (*p == '-') out_path[0]=0;
        else
        { 
          if (p[0] == '\\' && p[1] != '\\') p++;
          strncpy(out_path,p,1024-1);
          if (out_path[strlen(out_path)-1]=='\\') out_path[strlen(out_path)-1]=0; // remove trailing slash
        }
        if (!*out_path) 
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        printf("CreateDirectory: \"%s\"\n",out_path);
        ent.which=EW_CREATEDIR;
        ent.offsets[0]=add_string(out_path);
      }
    return add_entry(&ent);
    case TOK_EXEC:
    case TOK_EXECWAIT:
      ent.which=EW_EXECUTE;
      ent.offsets[0]=add_string(line.gettoken_str(1));
      ent.offsets[1] = (which_token == TOK_EXECWAIT);
      printf("%s: \"%s\"\n",ent.offsets[1]?"ExecWait":"Exec",line.gettoken_str(1));
    return add_entry(&ent);
    case TOK_EXECSHELL: // this uses improvements of Andras Varga
      ent.which=EW_SHELLEXEC;
      ent.offsets[0]=add_string(line.gettoken_str(1));
      ent.offsets[1]=add_string(line.gettoken_str(2));
      ent.offsets[2]=add_string(line.gettoken_str(3));
      ent.offsets[3]=SW_SHOWNORMAL;
      if (line.getnumtokens() > 4)
      {
        int tab[3]={SW_SHOWNORMAL,SW_SHOWMAXIMIZED,SW_SHOWMINIMIZED};
        int a=line.gettoken_enum(4,"SW_SHOWNORMAL\0SW_SHOWMAXIMIZED\0SW_SHOWMINIMIZED\0");
        if (a < 0)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        ent.offsets[3]=tab[a];
      }
      printf("ExecShell: %s: \"%s\" \"%s\" %s\n",line.gettoken_str(1),line.gettoken_str(2),
                                                 line.gettoken_str(3),line.gettoken_str(4));
    return add_entry(&ent);
    case TOK_REGDLL:
    case TOK_UNREGDLL:
#ifndef NSIS_SUPPORT_ACTIVEXREG
      printf("RegDLL/UnRegDLL: support for RegDLL not compiled in (NSIS_SUPPORT_ACTIVEXREG)\n");
      return PS_ERROR;
#else
    {
      char *t;
      ent.which=EW_REGISTERDLL;
      ent.offsets[0]=add_string(line.gettoken_str(1));
      if (which_token == TOK_UNREGDLL) 
      {
        t="DllUnregisterServer"; 
        ent.offsets[2]=add_string("Unregistering: ");
      }
      else // register
      {
        t=line.gettoken_str(2);
        if (!t || !*t) t="DllRegisterServer";
        ent.offsets[2]=add_string("Registering: ");
      }
      ent.offsets[1] = add_string(t);

      printf("%s: \"%s\"\n",t,line.gettoken_str(1));
    }
    return add_entry(&ent);
#endif
    case TOK_RENAME:
      {
        int a=1;
        ent.which=EW_RENAME;
        if (!stricmp(line.gettoken_str(1),"/REBOOTOK"))
        {
          ent.offsets[2]=1;
          a++;
        }
        else if (line.gettoken_str(1)[0]=='/')
        {
          a=line.getnumtokens(); // cause usage to go here:
        }
        if (line.getnumtokens()!=a+2)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        ent.offsets[0]=add_string(line.gettoken_str(a));
        ent.offsets[1]=add_string(line.gettoken_str(a+1));
        printf("Rename: %s%s->%s\n",ent.offsets[2]?"/REBOOTOK ":"",line.gettoken_str(a),line.gettoken_str(a+1));
      }
    return add_entry(&ent);
    case TOK_MESSAGEBOX:
      {
        #define MBD(x) {x,#x},
        struct 
        {
          int id;
          char *str;
        } list[]=
        {
          MBD(MB_ABORTRETRYIGNORE)
          MBD(MB_OK)
          MBD(MB_OKCANCEL)
          MBD(MB_RETRYCANCEL)
          MBD(MB_YESNO)
          MBD(MB_YESNOCANCEL)
          MBD(MB_ICONEXCLAMATION)
          MBD(MB_ICONINFORMATION)
          MBD(MB_ICONQUESTION)
          MBD(MB_ICONSTOP)
          MBD(MB_TOPMOST)
          MBD(MB_SETFOREGROUND)
          MBD(MB_RIGHT)
        };
        #undef MBD
        int r=0;
        int x;
        char *p=line.gettoken_str(1);

        while (*p)
        {
          char *np=p;
          while (*np && *np != '|') np++;
          if (*np) *np++=0;
          for (x  =0 ; x < sizeof(list)/sizeof(list[0]) && strcmp(list[x].str,p); x ++);
          if (x < sizeof(list)/sizeof(list[0]))
          {
            r|=list[x].id;
          }
          else
          {
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }            
          p=np;
        }
        ent.which=EW_MESSAGEBOX;
        ent.offsets[0]=r;
        ent.offsets[1]=add_string(line.gettoken_str(2));
        if (line.getnumtokens() > 3)
        {
          ent.offsets[2]=line.gettoken_enum(3,"0\0IDABORT\0IDCANCEL\0IDIGNORE\0IDNO\0IDOK\0IDRETRY\0IDYES\0");
          if (ent.offsets[2] < 0)
          {
              print_help(line.gettoken_str(0));
              return PS_ERROR;
          }
          int rettab[] = 
          {
            0,IDABORT,IDCANCEL,IDIGNORE,IDNO,IDOK,IDRETRY,IDYES
          };
          ent.offsets[2] = rettab[ent.offsets[2]];
          if (process_jump(line.gettoken_str(4),&ent.offsets[3])) 
          {
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }
        }
        printf("MessageBox: %d: \"%s\"",r,line.gettoken_str(2));
        if (line.getnumtokens()>4) printf(" (on %s goto %s)",line.gettoken_str(3),line.gettoken_str(4));
        printf("\n");
      }      
    return add_entry(&ent);
    case TOK_DELETEREGVALUE:
    case TOK_DELETEREGKEY:
      {
        int a=1;
        if (which_token==TOK_DELETEREGKEY)
        {
          char *s=line.gettoken_str(a);
          if (s[0] == '/')
          {
            if (stricmp(s,"/ifempty")) 
            {
              print_help(line.gettoken_str(0));
              return PS_ERROR;
            }
            a++;
            ent.offsets[3]=1;
          }
        }
        int k=line.gettoken_enum(a,rootkeys[0]);
        if (k == -1) k=line.gettoken_enum(a,rootkeys[1]);
        if (k == -1)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        ent.which=EW_DELREG;
        ent.offsets[0]=(int)rootkey_tab[k];      
        ent.offsets[1]=add_string(line.gettoken_str(a+1));
        ent.offsets[2]=(which_token==TOK_DELETEREGKEY)?-1:add_string(line.gettoken_str(a+2));
        if (which_token==TOK_DELETEREGKEY)
          printf("DeleteRegKey: %s\\%s\n",line.gettoken_str(a),line.gettoken_str(a+1));
        else
          printf("DeleteRegValue: %s\\%s\\%s\n",line.gettoken_str(a),line.gettoken_str(a+1),line.gettoken_str(a+2));
      }
    return add_entry(&ent);
    case TOK_WRITEREGSTR:
    case TOK_WRITEREGBIN:
    case TOK_WRITEREGDWORD:
      {
        int k=line.gettoken_enum(1,rootkeys[0]);
        if (k == -1) k=line.gettoken_enum(1,rootkeys[1]);
        if (k == -1)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        ent.which=EW_WRITEREG;
        ent.offsets[0]=(int)rootkey_tab[k];
        ent.offsets[1]=add_string(line.gettoken_str(2));
        ent.offsets[2]=add_string(line.gettoken_str(3));
        if (which_token == TOK_WRITEREGSTR)
        {
          printf("WriteRegStr: %s\\%s\\%s=%s\n",
            line.gettoken_str(1),line.gettoken_str(2),line.gettoken_str(3),line.gettoken_str(4));
          ent.offsets[3]=add_string(line.gettoken_str(4));
          ent.offsets[4]=1;
        }
        if (which_token == TOK_WRITEREGBIN) 
        {
          char data[512];
          char *p=line.gettoken_str(4);
          int data_len=0;
          while (*p)
          {
            int c;
            int a,b;
            a=*p;
            if (a >= '0' && a <= '9') a-='0';
            else if (a >= 'a' && a <= 'f') a-='a'-10;
            else if (a >= 'A' && a <= 'F') a-='A'-10;
            else break;
            b=*++p;
            if (b >= '0' && b <= '9') b-='0';
            else if (b >= 'a' && b <= 'f') b-='a'-10;
            else if (b >= 'A' && b <= 'F') b-='A'-10;
            else break;
            p++;
            c=(a<<4)|b;
            if (data_len >= 512) 
            {
              printf("WriteRegBin: 512 bytes of data exceeded\n");
              return PS_ERROR;
            }
            data[data_len++]=c;
          }
          if (*p)
          {
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }
          printf("WriteRegBin: %s\\%s\\%s=%s\n",
            line.gettoken_str(1),line.gettoken_str(2),line.gettoken_str(3),line.gettoken_str(4));
          ent.offsets[3]=add_data(data,data_len);
          if (ent.offsets[3] < 0) return PS_ERROR;
          ent.offsets[4]=3;
        }
        if (which_token == TOK_WRITEREGDWORD) 
        {
          int s;
          ent.offsets[3]=line.gettoken_int(4,&s);
          if (!s)
          {
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }
          ent.offsets[4]=2;
          printf("WriteRegDword: %s\\%s\\%s=%d\n",
            line.gettoken_str(1),line.gettoken_str(2),line.gettoken_str(3),ent.offsets[3]);
        }
      }
    return add_entry(&ent);
    case TOK_DELETEINISEC:
    case TOK_DELETEINISTR:
      {
        char *vname="<section>";
        ent.which=EW_WRITEINI;
        ent.offsets[0]=add_string(line.gettoken_str(2)); // section name
        if (line.getnumtokens() > 3) 
        {
          vname=line.gettoken_str(3);
          ent.offsets[1]=add_string(vname); // value name
        }
        else ent.offsets[1]=-1;
        ent.offsets[2]=-1;
        ent.offsets[3]=add_string(line.gettoken_str(1));
        printf("DeleteINI%s: [%s] %s in %s\n",vname?"Str":"Sec",
          line.gettoken_str(2),vname,line.gettoken_str(1));
      }
    return add_entry(&ent);
    case TOK_WRITEINISTR:
      ent.which=EW_WRITEINI;
      ent.offsets[0]=add_string(line.gettoken_str(2));
      ent.offsets[1]=add_string(line.gettoken_str(3));
      ent.offsets[2]=add_string(line.gettoken_str(4));
      ent.offsets[3]=add_string(line.gettoken_str(1));
      printf("WriteINIStr: [%s] %s=%s in %s\n",
        line.gettoken_str(2),line.gettoken_str(3),line.gettoken_str(4),line.gettoken_str(1));
    return add_entry(&ent);
    case TOK_CREATESHORTCUT:
      ent.which=EW_CREATESHORTCUT;
      ent.offsets[0]=add_string(line.gettoken_str(1));
      ent.offsets[1]=add_string(line.gettoken_str(2));
      ent.offsets[2]=add_string(line.gettoken_str(3));
      ent.offsets[3]=add_string(line.gettoken_str(4));
      int s;
      ent.offsets[4]=line.gettoken_int(5,&s)&0xff;
      if (!s)
      {
        if (line.getnumtokens() > 5)
        {
          printf("CreateShortCut: cannot interpret icon index\n");
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        ent.offsets[4]=0;
      }
      if (line.getnumtokens() > 6)
      {
        int tab[3]={SW_SHOWNORMAL,SW_SHOWMAXIMIZED,SW_SHOWMINIMIZED};
        int a=line.gettoken_enum(6,"SW_SHOWNORMAL\0SW_SHOWMAXIMIZED\0SW_SHOWMINIMIZED\0");
        if (a < 0)
        {
          printf("CreateShortCut: unknown show mode \"%s\"\n",line.gettoken_str(6));
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        ent.offsets[4]|=tab[a]<<8;
      }
      if (line.getnumtokens() > 7)
      {
        char *s=line.gettoken_str(7);
        if (*s)
        {
          int c=0;
          if (strstr(s,"ALT|")) ent.offsets[4]|=HOTKEYF_ALT << 24;
          if (strstr(s,"CONTROL|")) ent.offsets[4]|=HOTKEYF_CONTROL << 24;
          if (strstr(s,"EXT|")) ent.offsets[4]|=HOTKEYF_EXT << 24;
          if (strstr(s,"SHIFT|")) ent.offsets[4]|=HOTKEYF_SHIFT << 24;
          while (strstr(s,"|"))
          {
            s=strstr(s,"|")+1;
          }
          if ((s[0] == 'f' || s[0] == 'F') && (s[1] >= '1' && s[1] <= '9'))
          {
            c=VK_F1-1+atoi(s+1);
            if (atoi(s+1) < 1 || atoi(s+1) > 24)
            {
              warning("CreateShortCut: F-key \"%s\" out of range (%s:%d)\n",s,curfilename,linecnt);
            }
          }
          else if (s[0] >= 'a' && s[0] <= 'z' && !s[1])
            c=s[0]+'A'-'a';
          else if (((s[0] >= 'A' && s[0] <= 'Z') || (s[0] >= '0' && s[0] <= '9')) && !s[1])
            c=s[0];
          else
          {
            c=s[0];
            warning("CreateShortCut: unrecognized hotkey \"%s\" (%s:%d)\n",s,curfilename,linecnt);
          }
          ent.offsets[4] |= (c) << 16;
        }
      }
      printf("CreateShortCut: \"%s\"->\"%s\" %s  icon:%s,%d, showmode=0x%X, hotkey=0x%X\n",
        line.gettoken_str(1),line.gettoken_str(2),line.gettoken_str(3),
        line.gettoken_str(4),ent.offsets[4]&0xff,(ent.offsets[4]>>8)&0xff,ent.offsets[4]>>16);
    return add_entry(&ent);
    case TOK_FINDWINDOW:
    case TOK_FINDWINDOWBYTITLE:
      ent.which=EW_FINDWINDOW;
      ent.offsets[0]=line.gettoken_enum(1,"close\0closeinstant\0prompt\0");
      if (ent.offsets[0] < 0)
      {
        if (!strnicmp(line.gettoken_str(1),"goto:",5) && strlen(line.gettoken_str(1)) > 5)
        {
          ent.offsets[0]=3;
          if (process_jump(line.gettoken_str(1)+5,&ent.offsets[4])) 
          {
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }
        }
        if (ent.offsets[0] < 0)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
      }
      ent.offsets[1]=add_string(line.gettoken_str(2));
      ent.offsets[2]=add_string(line.gettoken_str(3));
      ent.offsets[3]=(which_token==TOK_FINDWINDOWBYTITLE);
      printf("FindWindow%s: mode=%s, class=\"%s\", text=\"%s\"\n",
        which_token==TOK_FINDWINDOWBYTITLE?"ByTitle":"",
        line.gettoken_str(1),line.gettoken_str(2),line.gettoken_str(3));    
    return add_entry(&ent);
    case TOK_INSTNSPLUG:
#ifndef NSIS_SUPPORT_NETSCAPEPLUGINS
      printf("InstNSPlug: support for InstNSPlug not compiled in (NSIS_SUPPORT_NETSCAPEPLUGINS)\n");
      return PS_ERROR;
#else
      {
        char dir[1024];
        char newfn[1024], *s;
        HANDLE h;
        WIN32_FIND_DATA d;
        strcpy(dir,line.gettoken_str(1));
        s=dir+strlen(dir);
        while (s > dir && *s != '\\') s--;
        *s=0;

        h = FindFirstFile(line.gettoken_str(1),&d);
        if (h != INVALID_HANDLE_VALUE)
        {
          do {
            HANDLE hFile,hFileMap;
            DWORD len;
            if (d.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) continue;
            sprintf(newfn,"%s%s%s",dir,dir[0]?"\\":"",d.cFileName);
            hFile=CreateFile(newfn,GENERIC_READ,FILE_SHARE_READ,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
            if (hFile == INVALID_HANDLE_VALUE)
            {
              printf("InstNSPlug: failed opening file \"%s\"\n",newfn);
              return PS_ERROR;
            }
            len = GetFileSize(hFile, NULL);
            if (!len || !(hFileMap = CreateFileMapping(hFile, NULL, PAGE_READONLY, 0, 0, NULL)))
            {
              CloseHandle(hFile);
              printf("InstNSPlug: failed creating mmap of \"%s\" (file is %d bytes)\n",newfn,len);
              return PS_ERROR;
            }
            char *filedata=(char*)MapViewOfFile(hFileMap, FILE_MAP_READ, 0, 0, 0);
            if (!filedata)
            {
              CloseHandle(hFileMap);
              CloseHandle(hFile);
              printf("InstNSPlug: failed mmapping file \"%s\"\n",newfn);
              return PS_ERROR;
            }

            section_add_size_kb((len+1023)/1024);

            printf("InstNSPlug: \"%s\"",d.cFileName);
            if (build_compress)
            {
              printf(" [compress]");
              fflush(stdout);
            }
            int last_build_datablock_used=getcurdbsize();
            ent.which=EW_INSTNETSCAPE;
            ent.offsets[0]=add_string(d.cFileName);
            ent.offsets[1]=0;
            ent.offsets[2]=add_data(filedata,len);

            UnmapViewOfFile(filedata);
            CloseHandle(hFileMap);

            if (ent.offsets[2] < 0)
            {
              CloseHandle(hFile);
              return PS_ERROR;
            }

            {
              DWORD s=getcurdbsize()-last_build_datablock_used;
              if (s) s-=4;
              if (s != len) printf(" %d/%d bytes\n",s,len);
              else printf(" %d bytes\n",len);
            }


            if (build_datesave)
            {
              FILETIME ft;
              if (GetFileTime(hFile,NULL,NULL,&ft))
              {
                ent.offsets[3]=ft.dwHighDateTime;
                ent.offsets[4]=ft.dwLowDateTime;
              }
              else
              {
                printf("InstNSPlug: failed getting file date from \"%s\"\n",newfn);
                CloseHandle(hFile);
                return PS_ERROR;
              }
            }
            else
            {
              ent.offsets[3]=0xffffffff;
              ent.offsets[4]=0xffffffff;
            }
            CloseHandle(hFile);

            int a=add_entry(&ent);
            if (a != PS_OK) return a;
          } while (FindNextFile(h,&d));
          FindClose(h);
        }
        else
        {
          printf("InstNSPlug: failed finding file \"%s\"\n",line.gettoken_str(1));
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
      }
    return PS_OK;
#endif
    case TOK_DELETENSPLUG:
#ifndef NSIS_SUPPORT_NETSCAPEPLUGINS
      printf("DeleteNSPlug: support for DeleteNSPlug not compiled in (NSIS_SUPPORT_NETSCAPEPLUGINS)\n");
      return PS_ERROR;
#else
      ent.which=EW_INSTNETSCAPE;
      ent.offsets[0]=add_string(line.gettoken_str(1));
      ent.offsets[1]=1;
      ent.offsets[2]=0;
      printf("DeleteNSPlug: \"%s\"\n",line.gettoken_str(1));
    return add_entry(&ent);
#endif
    case TOK_DELETE:
      {
        int a=1;
        ent.which=EW_DELETEFILE;
        if (!stricmp(line.gettoken_str(a),"/REBOOTOK"))
        {
          a++;
          ent.offsets[1]=1;
        }
        else if (line.gettoken_str(1)[0]=='/')
        {
          a=line.getnumtokens();
        }
        if (line.getnumtokens() != a+1)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        ent.offsets[0]=add_string(line.gettoken_str(a));
        printf("Delete: %s\"%s\"\n",ent.offsets[1]?"/REBOOTOK ":"",line.gettoken_str(a));
      }
    return add_entry(&ent);
    case TOK_RMDIR:
      {
        int a=1;
        ent.which=EW_RMDIR;
        if (!stricmp(line.gettoken_str(1),"/r"))
        {
          if (line.getnumtokens() < 3)
          {
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }
          a++;
          ent.offsets[1]=1;
        }
        else if (line.gettoken_str(1)[0]=='/')
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        ent.offsets[0]=add_string(line.gettoken_str(a));
        printf("RMDir: %s\"%s\"\n",ent.offsets[1]?"/r " : "",line.gettoken_str(a));
      }
    return add_entry(&ent);
    case TOK_FILE:
      {
        int a=1,rec=0;
        if (!stricmp(line.gettoken_str(a),"/r"))
        {
          rec=1;
          a++;
        }
        else if (!strnicmp(line.gettoken_str(a),"/oname=",7))
        {
          char *on=line.gettoken_str(a)+7;
          a++;
          if (!*on||line.getnumtokens()!=a+1||strstr(on,"*") || strstr(on,"?") || strstr(on,"\\"))
          {
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }

          int tf=0;
          int v=do_add_file(line.gettoken_str(a), 0, linecnt,&tf,on);
          if (v != PS_OK) return v;
          if (tf > 1)
          {
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }
          if (!tf)
          {
            print_help(line.gettoken_str(0));
            printf("File: \"%s\" -> no files found.\n",line.gettoken_str(a));
            return PS_ERROR;
          }


          return PS_OK;
        }
        else if (line.gettoken_str(a)[0] == '/')
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;          
        }
        if (line.getnumtokens()<a+1)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        while (a < line.getnumtokens())
        {
          if (line.gettoken_str(a)[0]=='/')
          {
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }
          char buf[32];
          char *t=line.gettoken_str(a++);
          if (t[0] && t[1] == ':' && t[2] == '\\' && !t[3])
          {
            strcpy(buf,"X:\\*.*");
            buf[0]=t[0];
            t=buf;
          }
          int tf=0;
          int v=do_add_file(t, rec, linecnt,&tf);
          if (v != PS_OK) return v;
          if (!tf)
          {
            printf("File: \"%s\" -> no files found.\n",t);
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }
        }
      }
    return PS_OK;
    case TOK_COPYFILES:
      {
        ent.which=EW_COPYFILES;
        ent.offsets[0]=add_string(line.gettoken_str(1));
        ent.offsets[1]=add_string(line.gettoken_str(2));
        int s;
        int size_kb=line.gettoken_int(3,&s);
        if (!s)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        section_add_size_kb(size_kb);
        printf("CopyFiles: \"%s\" -> \"%s\", size=%iKb\n",line.gettoken_str(1),line.gettoken_str(2),size_kb);
      }
    return add_entry(&ent);
    case TOK_SETFILEATTRIBUTES:
      {
        #define MBD(x) {x,#x},
        struct 
        {
          int id;
          char *str;
        } list[]=
        {
          MBD(FILE_ATTRIBUTE_NORMAL)
          MBD(FILE_ATTRIBUTE_ARCHIVE)
          MBD(FILE_ATTRIBUTE_HIDDEN)
          MBD(FILE_ATTRIBUTE_OFFLINE)
          MBD(FILE_ATTRIBUTE_READONLY)
          MBD(FILE_ATTRIBUTE_SYSTEM)
          MBD(FILE_ATTRIBUTE_TEMPORARY)
          {FILE_ATTRIBUTE_NORMAL,"NORMAL"},
          {FILE_ATTRIBUTE_ARCHIVE,"ARCHIVE"},
          {FILE_ATTRIBUTE_HIDDEN,"HIDDEN"},
          {FILE_ATTRIBUTE_OFFLINE,"OFFLINE"},
          {FILE_ATTRIBUTE_READONLY,"READONLY"},
          {FILE_ATTRIBUTE_SYSTEM,"SYSTEM"},
          {FILE_ATTRIBUTE_TEMPORARY,"TEMPORARY"},
          {FILE_ATTRIBUTE_NORMAL,"0"},
        };
        #undef MBD
        int r=0;
        int x;
        char *p=line.gettoken_str(2);

        while (*p)
        {
          char *np=p;
          while (*np && *np != '|') np++;
          if (*np) *np++=0;
          for (x  =0 ; x < sizeof(list)/sizeof(list[0]) && stricmp(list[x].str,p); x ++);

          if (x < sizeof(list)/sizeof(list[0]))
          {
            r|=list[x].id;
          }
          else
          {
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }            
          p=np;
        }
        ent.which=EW_SETFILEATTRIBUTES;
        ent.offsets[0]=add_string(line.gettoken_str(1));
        ent.offsets[1]=r;
      }
    return add_entry(&ent);
    case TOK_SLEEP:
      {
        int s;
        ent.which=EW_SLEEP;        
        ent.offsets[0]=line.gettoken_int(1,&s);
        if (!s)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        printf("Sleep: %d ms\n",ent.offsets[0]);
      }
    return add_entry(&ent);
    case TOK_BRINGTOFRONT:
      ent.which=EW_BRINGTOFRONT;        
      printf("BringToFront\n");
    return add_entry(&ent);
    case TOK_HIDEWINDOW:
      ent.which=EW_HIDEWINDOW;        
      printf("HideWindow\n");
    return add_entry(&ent);
    case TOK_IFFILEEXISTS:
      ent.which=EW_IFFILEEXISTS;
      ent.offsets[0] = add_string(line.gettoken_str(1));
      if (process_jump(line.gettoken_str(2),&ent.offsets[1]) || 
          process_jump(line.gettoken_str(3),&ent.offsets[2])) 
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
      printf("IfFileExists: \"%s\" ? %s : %s\n",line.gettoken_str(1),line.gettoken_str(2),line.gettoken_str(3));
    return add_entry(&ent);
    case TOK_ABORT:
      ent.which=EW_ABORT;
      ent.offsets[0] = add_string(line.gettoken_str(1));
      printf("Abort: \"%s\"\n",line.gettoken_str(1));
    return add_entry(&ent);
    case TOK_SETDETAILSVIEW:
      ent.which=EW_CHDETAILSVIEW;
      ent.offsets[0] = line.gettoken_enum(1,"hide\0show\0");
      if (ent.offsets[0] < 0)
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
      printf("SetDetailsView: %s\n",line.gettoken_str(1));
    return add_entry(&ent);
    case TOK_SETAUTOCLOSE:
      ent.which=EW_SETWINDOWCLOSE;
      ent.offsets[0] = line.gettoken_enum(1,"false\0true\0");
      if (ent.offsets[0] < 0)
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
      printf("SetAutoClose: %s\n",line.gettoken_str(1));
    return add_entry(&ent);
    case TOK_IFERRORS:
      {
        int s=1;
        ent.which=EW_IFERRORS;
        if (process_jump(line.gettoken_str(1),&ent.offsets[0]) || 
            process_jump(line.gettoken_str(2),&ent.offsets[1])) 
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        printf("IfErrors ?%s:%s\n",line.gettoken_str(1),line.gettoken_str(2));
      }
    return add_entry(&ent);
    case TOK_CLEARERRORS:
      ent.which=EW_IFERRORS;
      printf("ClearErrors\n");
    return add_entry(&ent);
    case TOK_SETERRORS:
      ent.which=EW_IFERRORS;
      ent.offsets[2]=1;
      printf("SetErrors\n");
    return add_entry(&ent);
    case TOK_STRCPY:
      ent.which=EW_ASSIGNVAR;
      ent.offsets[0]=line.gettoken_enum(1,usrvars);
      ent.offsets[1]=add_string(line.gettoken_str(2));
      if (line.getnumtokens()>3)
      {
        int s;
        ent.offsets[2]=line.gettoken_int(3,&s);
        if (!s || !ent.offsets[2]) ent.offsets[0]=-1;
      }
      if (ent.offsets[0] < 0)
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
      printf("StrCpy $%d \"%s\" (%d)\n",ent.offsets[0],line.gettoken_str(2),ent.offsets[2]);
    return add_entry(&ent);
    case TOK_GETPARENTDIR:
      ent.which=EW_GETPARENT;
      ent.offsets[0]=line.gettoken_enum(1,usrvars);
      ent.offsets[1]=add_string(line.gettoken_str(2));
      if (ent.offsets[0] < 0)
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
      printf("GetParentDir $%d \"%s\" (%d)\n",ent.offsets[0],line.gettoken_str(2),ent.offsets[2]);
    return add_entry(&ent);
    case TOK_STRCMP:
      ent.which=EW_STRCMP;
      ent.offsets[0]=add_string(line.gettoken_str(1));
      ent.offsets[1]=add_string(line.gettoken_str(2));
      if (process_jump(line.gettoken_str(3),&ent.offsets[2]) ||
          process_jump(line.gettoken_str(4),&ent.offsets[3])) 
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
      printf("StrCmp \"%s\" \"%s\" equal=%s, nonequal=%s\n",line.gettoken_str(1),line.gettoken_str(2), line.gettoken_str(3),line.gettoken_str(4));
    return add_entry(&ent);
    case TOK_READINISTR:
      ent.which=EW_READINISTR;
      ent.offsets[0]=line.gettoken_enum(1,usrvars);
      if (ent.offsets[0] < 0)
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
      ent.offsets[1]=add_string(line.gettoken_str(3));
      ent.offsets[2]=add_string(line.gettoken_str(4));
      ent.offsets[3]=add_string(line.gettoken_str(2));
      printf("ReadINIStr $%d [%s]:%s from %s\n",ent.offsets[0],line.gettoken_str(3),line.gettoken_str(4),line.gettoken_str(2));
    return add_entry(&ent);
    case TOK_READREGSTR:
      {
        ent.which=EW_READREGSTR;
        ent.offsets[0]=line.gettoken_enum(1,usrvars);
        int k=line.gettoken_enum(2,rootkeys[0]);
        if (k == -1) k=line.gettoken_enum(2,rootkeys[1]);
        if (ent.offsets[0] < 0 || k == -1)
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        ent.offsets[1]=(int)rootkey_tab[k];
        ent.offsets[2]=add_string(line.gettoken_str(3));
        ent.offsets[3]=add_string(line.gettoken_str(4));
        printf("ReadRegStr $%d %s\\%s\\%s\n",ent.offsets[0],line.gettoken_str(2),line.gettoken_str(3),line.gettoken_str(4));
      }
    return add_entry(&ent);
    case TOK_DETAILPRINT:
      ent.which=EW_UPDATETEXT;
      ent.offsets[0]=add_string(line.gettoken_str(1));
      printf("DetailPrint: \"%s\"\n",line.gettoken_str(1));
    return add_entry(&ent);
    case TOK_GETFULLDLLPATH:
      ent.which=EW_GETFULLDLLPATH;
      ent.offsets[0]=line.gettoken_enum(1,usrvars);
      if (ent.offsets[0] < 0)
      {
        print_help(line.gettoken_str(0));
        return PS_ERROR;
      }
      ent.offsets[1]=add_string(line.gettoken_str(2));
      printf("GetFullDLLPath $%d %s\n",ent.offsets[0],line.gettoken_str(2));
    return add_entry(&ent);
    case TOK_COMPAREDLLS: // The version/date storing is based on ideas from Davison Long
      {
        int a=1;
        int flag=0;
        ent.which=EW_COMPAREDLLS;
        if (!stricmp(line.gettoken_str(a),"/STOREFROM"))
        {
          DWORD s;
          DWORD d;
          s=GetFileVersionInfoSize(line.gettoken_str(++a),&d);
          if (s)
          {
            void *buf;
            buf=(void*)GlobalAlloc(GPTR,s);
            if (buf)
            {
              UINT uLen;
              VS_FIXEDFILEINFO *pvsf;
              if (GetFileVersionInfo(line.gettoken_str(a),0,s,buf) && VerQueryValue(buf,"\\",(void**)&pvsf,&uLen))
              {
                ent.offsets[0]=pvsf->dwFileVersionLS;
                ent.offsets[4]=pvsf->dwFileVersionMS;
                flag=0x80000000;
              }
              GlobalFree(buf);
            }

          }
          if (!flag)
          {
            printf("CompareDLLVersions: error reading version info from \"%s\"\n",line.gettoken_str(a));
            return PS_ERROR;
          }
          a++;
        }
        else
        {
          if (line.gettoken_str(a)[0]=='/')
          {
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }
          ent.offsets[0]=add_string(line.gettoken_str(a++));
        }
        ent.offsets[1]=add_string(line.gettoken_str(a++))|flag;

        if (process_jump(line.gettoken_str(a++),&ent.offsets[2]) ||
            process_jump(line.gettoken_str(a++),&ent.offsets[3]) || 
            a != line.getnumtokens()) 
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        if (flag)
          printf("CompareDLLVersions: %08X:%08X > \"%s\" ? \"%s\" : \"%s\"\n",
            ent.offsets[4],ent.offsets[0],
            line.gettoken_str(a-3),
            line.gettoken_str(a-2),
            line.gettoken_str(a-1));
        else
          printf("CompareDLLVersions: \"%s\" > \"%s\" ? \"%s\" : \"%s\"\n",
            line.gettoken_str(a-4),
            line.gettoken_str(a-3),
            line.gettoken_str(a-2),
            line.gettoken_str(a-1));
      }
    return add_entry(&ent);
    case TOK_COMPAREFILETIMES:
      {
        int a=1;
        int flag=0;
        ent.which=EW_COMPAREFILETIMES;
        if (!stricmp(line.gettoken_str(a),"/STOREFROM"))
        {
          HANDLE hFile=CreateFile(line.gettoken_str(++a),0,0,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
          if (hFile != INVALID_HANDLE_VALUE)
          {
            FILETIME ft;
            if (GetFileTime(hFile,NULL,NULL,&ft))
            {
              ent.offsets[4]=ft.dwHighDateTime;
              ent.offsets[0]=ft.dwLowDateTime;
              flag=0x80000000;
            }            
            CloseHandle(hFile);
          }
          if (!flag)
          {
            printf("CompareFileTimes: error reading date from \"%s\"\n",line.gettoken_str(a));
            return PS_ERROR;
          }
          a++;
        }
        else
        {
          if (line.gettoken_str(a)[0]=='/')
          {
            print_help(line.gettoken_str(0));
            return PS_ERROR;
          }
          ent.offsets[0]=add_string(line.gettoken_str(a++));
        }
        ent.offsets[1]=add_string(line.gettoken_str(a++))|flag;
        if (process_jump(line.gettoken_str(a++),&ent.offsets[2]) ||
            process_jump(line.gettoken_str(a++),&ent.offsets[3]) ||
            a != line.getnumtokens()) 
        {
          print_help(line.gettoken_str(0));
          return PS_ERROR;
        }
        if (flag)
          printf("CompareFileTimes: %08X:%08X > \"%s\" ? \"%s\" : \"%s\"\n",
            ent.offsets[4],ent.offsets[0],
            line.gettoken_str(a-3),
            line.gettoken_str(a-2),
            line.gettoken_str(a-1));
        else
          printf("CompareFileTimes: \"%s\" > \"%s\" ? \"%s\" : \"%s\"\n",
            line.gettoken_str(a-4),
            line.gettoken_str(a-3),
            line.gettoken_str(a-2),
            line.gettoken_str(a-1));
      }
    return add_entry(&ent);

	// mpg start mod
	
    case TOK_MUTEXWAIT:
		ent.which=EW_MUTEXWAIT;
		ent.offsets[0]=add_string(line.gettoken_str(1));
		ent.offsets[1] = (which_token == TOK_MUTEXWAIT);
		printf("%s: \"%s\"\n",ent.offsets[1]?"MutexWait":"MutexWait",line.gettoken_str(1));
		return add_entry(&ent);
		
	case TOK_OPENSTATUS:
		ent.which=EW_OPENSTATUS;
		ent.offsets[0]=add_string(line.gettoken_str(1));
		ent.offsets[1]=add_string(line.gettoken_str(2));
		ent.offsets[2] = (which_token == TOK_OPENSTATUS);
		printf("%s: \"%s\"\n",ent.offsets[2]?"OpenStatus":"OpenStatus",line.gettoken_str(2));
		return add_entry(&ent);
		
    case TOK_UPDATESTATUS:
		ent.which=EW_UPDATESTATUS;
		ent.offsets[0]=add_string(line.gettoken_str(1));
		ent.offsets[1] = (which_token == TOK_UPDATESTATUS);
		printf("%s: \"%s\"\n",ent.offsets[1]?"UpdateStatus":"UpdateStatus",line.gettoken_str(1));
		return add_entry(&ent);
		
    case TOK_CLOSESTATUS:
		ent.which=EW_CLOSESTATUS;
		ent.offsets[0] = (which_token == TOK_CLOSESTATUS);
		printf("%s: \"%s\"\n",ent.offsets[1]?"CloseStatus":"CloseStatus",line.gettoken_str(1));
		return add_entry(&ent);
	
	// mpg end mod

	// end of instructions
    ///////////////////////////////////////////////////////////////////////////////

    default: break;

  }
  printf("Error: doCommand: Invalid token \"%s\".\n",line.gettoken_str(0));
  return PS_ERROR;
}

int CEXEBuild::do_add_file(char *lgss, int recurse, int linecnt, int *total_files, char *name_override)
{
  char dir[1024];
  char newfn[1024], *s;
  HANDLE h;
  WIN32_FIND_DATA d;
  strcpy(dir,lgss);
  s=dir+strlen(dir);
  while (s > dir && *s != '\\') s--;
  *s=0;

  h = FindFirstFile(lgss,&d);
  if (h != INVALID_HANDLE_VALUE)
  {
    do 
    {
      if (d.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) 
      {
        if (recurse && strcmp(d.cFileName,"..") && strcmp(d.cFileName,".")) 
        {
          entry ent={0,};
          int a;
          char wd_save[1024];
          strcpy(wd_save,cur_out_path);
          if (cur_out_path[strlen(cur_out_path)-1]!='\\') strcat(cur_out_path,"\\");
          strcat(cur_out_path,d.cFileName);
          (*total_files)++;
          ent.which=EW_SETOUTPUTDIR;
          ent.offsets[0]=add_string(cur_out_path);
          a=add_entry(&ent);
          if (a != PS_OK) 
          {
            FindClose(h);
            return a;
          }

          char spec[1024];
          sprintf(spec,"%s%s%s",dir,dir[0]?"\\":"",d.cFileName);
          printf("File: Descending to: \"%s\" -> \"%s\"\n",spec,cur_out_path);
          strcat(spec,"\\*.*");
          a=do_add_file(spec,recurse,linecnt,total_files);
          if (a != PS_OK)
          {
            FindClose(h);
            return a;
          }

          strcpy(cur_out_path,wd_save);
          ent.which=EW_SETOUTPUTDIR;
          printf("File: Returning to: \"%s\" -> \"%s\"\n",dir,cur_out_path);
          ent.offsets[0]=add_string(cur_out_path);
          a=add_entry(&ent);
          if (a != PS_OK) 
          {
            FindClose(h);
            return a;
          }
        }
      }
      else
      {
        HANDLE hFile,hFileMap;
        DWORD len;
        (*total_files)++;
        sprintf(newfn,"%s%s%s",dir,dir[0]?"\\":"",d.cFileName);
        hFile=CreateFile(newfn,GENERIC_READ,FILE_SHARE_READ,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
        if (hFile == INVALID_HANDLE_VALUE)
        {
          printf("File: failed opening file \"%s\"\n",newfn);
          return PS_ERROR;
        }
        hFileMap=NULL;
        len = GetFileSize(hFile, NULL);
        if (len && !(hFileMap = CreateFileMapping(hFile, NULL, PAGE_READONLY, 0, 0, NULL)))
        {
          CloseHandle(hFile);
          printf("File: failed creating mmap of \"%s\"\n",newfn);
          return PS_ERROR;
        }
        char *filedata=NULL;
        if (len)
        {
          filedata=(char*)MapViewOfFile(hFileMap, FILE_MAP_READ, 0, 0, 0);
          if (!filedata)
          {
            if (hFileMap) CloseHandle(hFileMap);
            CloseHandle(hFile);
            printf("File: failed mmapping file \"%s\"\n",newfn);
            return PS_ERROR;
          }
        }

        section_add_size_kb((len+1023)/1024);
        if (name_override) printf("File: \"%s\"->\"%s\"",d.cFileName,name_override);
        else printf("File: \"%s\"",d.cFileName);
        if (build_compress) printf(" [compress]");
        fflush(stdout);

        int last_build_datablock_used=getcurdbsize();
        entry ent={0,};
        ent.which=EW_EXTRACTFILE;
        ent.offsets[0]=build_overwrite;
        ent.offsets[1]=add_string(name_override?name_override:d.cFileName);
        ent.offsets[2]=add_data(filedata?filedata:"",len);

        if (filedata) UnmapViewOfFile(filedata);
        if (hFileMap) CloseHandle(hFileMap);

        if (ent.offsets[2] < 0) 
        {
          CloseHandle(hFile);
          return PS_ERROR;
        }

        {
          DWORD s=getcurdbsize()-last_build_datablock_used;
          if (s) s-=4;
          if (s != len) printf(" %d/%d bytes\n",s,len);
          else printf(" %d bytes\n",len);
        }

        if (build_datesave || build_overwrite==0x3 /*ifnewer*/)
        {
          FILETIME ft;
          if (GetFileTime(hFile,NULL,NULL,&ft))
          {
            ent.offsets[3]=ft.dwHighDateTime;
            ent.offsets[4]=ft.dwLowDateTime;
          }
          else
          {
            CloseHandle(hFile);
            printf("File: failed getting file date from \"%s\"\n",newfn);
            return PS_ERROR;
          }
        }
        else
        {
          ent.offsets[3]=0xffffffff;
          ent.offsets[4]=0xffffffff;
        }

        CloseHandle(hFile);
        int a=add_entry(&ent);
        if (a != PS_OK) 
        {
          FindClose(h);
          return a;
        }
      }
    } while (FindNextFile(h,&d));
    FindClose(h);
  }
  return PS_OK;
}
  
