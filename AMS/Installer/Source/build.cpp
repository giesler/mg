#include <windows.h>
#include <stdio.h>
#include "zlib/zlib.h"
#include "exehead/config.h"
#include "exehead/fileform.h"
#include "xmmd5sum.h"					// MD5 code - include by mpg
#include "exedata.h"

#include "build.h"
#include "util.h"

void CEXEBuild::define(char *p) 
{ 
  definedlist.add(p,0); 
}


CEXEBuild::~CEXEBuild()
{
  free(header_data_new);
}

CEXEBuild::CEXEBuild()
{
  has_called_write_output=0;

  ns_func.add("",0); // make sure offset 0 is special on these (i.e. never used by a label)
  ns_label.add("",0);

  header_data_new=(unsigned char*)malloc(exeheader_size);
  exeheader_size_new=exeheader_size;

  if (!header_data_new) 
  {
    printf("Internal compiler error #12345: malloc(%d) failed\n",exeheader_size_new);
    extern void quit(); quit();
  }

  memcpy(header_data_new,header_data,exeheader_size);
  enabled_bitmap_offset = find_data_offset((char*)header_data_new,exeheader_size_new,(char*)bitmap1_data,BMP_HDRSKIP,sizeof(bitmap1_data));
  disabled_bitmap_offset = find_data_offset((char*)header_data_new,exeheader_size_new,(char*)bitmap2_data,BMP_HDRSKIP,sizeof(bitmap2_data));
  icon_offset = find_data_offset((char*)header_data_new,exeheader_size_new,(char*)icon_data,ICO_HDRSKIP,sizeof(icon_data));

  if (enabled_bitmap_offset < 0 || disabled_bitmap_offset < 0 || icon_offset < 0)
  {
    printf("Internal compiler error #12345: icons missing\n"); 
    extern void quit(); quit();
  }

  strcpy(cur_out_path,"$INSTDIR");

#ifdef NSIS_CONFIG_LOG
  definedlist.add("NSIS_CONFIG_LOG",0);
#endif
#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
  definedlist.add("NSIS_CONFIG_UNINSTALL_SUPPORT",0);
#endif
#ifdef NSIS_SUPPORT_NETSCAPEPLUGINS
  definedlist.add("NSIS_SUPPORT_NETSCAPEPLUGINS",0);
#endif
#ifdef NSIS_SUPPORT_ACTIVEXREG
  definedlist.add("NSIS_SUPPORT_ACTIVEXREG",0);
#endif
#ifdef NSIS_SUPPORT_BGBG
  definedlist.add("NSIS_SUPPORT_BGBG",0);
#endif

  db_opt_save=db_comp_save=db_full_size=db_opt_save_u=db_comp_save_u=db_full_size_u=0;

  cur_entries=&build_entries;
  cur_datablock=&build_datablock;
  cur_functions=&build_functions;
  cur_labels=&build_labels;

  build_cursection_isfunc=0;
  build_cursection=NULL;
  // init public data.
  build_packname[0]=build_packcmd[0]=build_output_filename[0]=0;

  build_overwrite=0;
  build_compress=1;
  build_crcchk=1; 
  build_datesave=1;
  build_optimize_datablock=1;

  memset(&build_header,-1,sizeof(build_header));

  build_header.install_reg_rootkey=0;
  build_header.no_custom_instmode_flag=0;
  build_header.num_sections=0;
  build_header.num_entries=0;
  build_header.silent_install=0;
  build_header.auto_close=0;
  build_header.show_details=0;
  build_header.no_show_dirpage=0;
  build_header.lb_bg=RGB(0,0,0);
  build_header.lb_fg=RGB(0,255,0);
  uninstall_mode=0;
  uninstall_size_full=0;
  uninstall_size=-1;
  
  memset(&build_uninst,-1,sizeof(build_uninst));
  build_uninst.lb_bg=RGB(0,0,0);
  build_uninst.lb_fg=RGB(0,255,0);
  build_uninst.num_entries=0;
  build_uninst.code[0]=build_uninst.code[1]=0;

  memcpy(m_unicon_data,unicon_data,sizeof(m_unicon_data));
}

int CEXEBuild::getcurdbsize() { return cur_datablock->getlen(); }

int CEXEBuild::add_string(char *string) // returns offset in stringblock
{
  if (uninstall_mode) return add_string_uninst(string);
  return add_string_main(string);
}
int CEXEBuild::add_string_main(char *string) // returns offset (in string block)
{
  if (!*string) return -1;
  if (strstr(string,"$WAVISDIR") || strstr(string,"$WADSPDIR"))
    warning("$WAVISDIR and $WADSPDIR are no longer supported.\n");
  return build_strlist.add(string,2);
}

int CEXEBuild::add_string_uninst(char *string) // returns offset (in string block)
{
  if (!*string) return -1;
  if (strstr(string,"$WAVISDIR") || strstr(string,"$WADSPDIR"))
    warning("$WAVISDIR and $WADSPDIR are no longer supported.\n");
  return ubuild_strlist.add(string,2);
}

// what it does is, when you pass it the offset of the last item added, it will determine if 
// that data is already present in the datablock, and if so, reference it instead (and shorten
// the datablock as necessary). Reduces overhead if you want to add files to a couple places.
// Woo, an optimizing installer generator, now we're styling.

int CEXEBuild::datablock_optimize(int start_offset)
{
  int this_len = cur_datablock->getlen()-start_offset;
  int pos=0;

  if (!build_optimize_datablock) return start_offset;

  char *db=(char*)cur_datablock->get();
  int first_int=*(int*)(db+start_offset);
  if (this_len >= 4) while (pos < start_offset)
  {
    int this_int = *(int*)(db+pos);
    if (this_int == first_int && !memcmp(db+pos,db+start_offset,this_len))
    {
      db_opt_save+=this_len;
      cur_datablock->resize(max(start_offset,pos+this_len));
      return pos;
    }
    pos += 4 + (this_int&0x7fffffff);
  }

  return start_offset;
}

int CEXEBuild::add_data(char *data, int length) // returns offset
{
  int done=0;

  if (length < 0)
  {
    printf("Error: add_data() called with length=%d\n",length);
    return -1;
  }

  int st=cur_datablock->getlen();

  if (build_compress)
  {

    z_stream stream={0,};

    // grow datablock so that there is room to compress into
    int bufferlen=length+1024+length/1000;
    cur_datablock->resize(st+bufferlen+sizeof(int));

    if (deflateInit(&stream, 9) != Z_OK) 
    {
      printf("Error: deflateInit() failed in add_data()\n");
      return -1;
    }

    stream.next_in = (Bytef*)data;
    stream.avail_in = (uInt)length;

    stream.next_out = (unsigned char *)cur_datablock->get() + st + sizeof(int);
    stream.avail_out = bufferlen;

    deflate(&stream, Z_SYNC_FLUSH);

    int used=bufferlen-stream.avail_out;

    deflateEnd(&stream);

    
    if (build_compress == 2 || used < length)
    {
      done=1;
      cur_datablock->resize(st+used+sizeof(int));

      *((int*)((char *)cur_datablock->get()+st)) = used|0x80000000;
      int nst=datablock_optimize(st);
      if (nst == st) db_comp_save+=length-used;
      else st=nst;
    }
  }

  if (!done)
  {
    cur_datablock->resize(st);
    cur_datablock->add(&length,sizeof(int));
    cur_datablock->add(data,length);
    st=datablock_optimize(st);
  }

  db_full_size += length + sizeof(int);

  return st;
}

int CEXEBuild::add_label(char *name)
{
  if (!build_cursection && !uninstall_mode)
  {
    printf("Error: Label declaration not valid outside of function/section\n"); 
    return PS_ERROR;
  }
  if ((name[0] >= '0' && name[0] <= '9') || name[0] == '-' || name[0] == ' ' || name[0] == ':')
  {
    printf("Error: labels must not begin with 0-9, -, :, or a space.\n"); 
    return PS_ERROR;
  }
  int cs;
  int ce;
  if (build_cursection)
  {
    cs=build_cursection->code[0];
    ce=build_cursection->code[1];
  }
  else
  {
    cs=build_uninst.code[0];
    ce=build_uninst.code[1];
  }

  char *p=strdup(name);
  if (p[strlen(p)-1] == ':') p[strlen(p)-1]=0;
  int offs=ns_label.add(p,0);
  free(p);

  int n=cur_labels->getlen()/sizeof(section);
  if (n)
  {
    section *t=(section*)cur_labels->get();
    while (n--)
    {
      if (t->code[0] >= cs && t->code[0] <= ce && t->name_ptr==offs)
      {
        printf("Error: label \"%s\" already declared in section/function\n",name);
        return PS_ERROR;
      }
      t++;
    }
  }
  
  section s={0,};
  s.name_ptr = offs;
  s.code[0] = ce;
  cur_labels->add(&s,sizeof(s));

  return PS_OK;
}

int CEXEBuild::add_function(char *funname)
{
  if (build_cursection_isfunc)
  {
    printf("Error: Function open when creating function (use FunctionEnd first)\n");
    return PS_ERROR;
  }
  if (build_cursection)
  {
    printf("Error: Section open when creating function (use SectionEnd first)\n");
    return PS_ERROR;
  }
  if (!funname[0])
  {
    printf("Error: Function must have a name\n");
    return PS_ERROR;
  }

  if (!strnicmp(funname,"un.",3))
  {
    set_uninstall_mode(1);
  }

  int addr=ns_func.add(funname,0);
  int x;
  int n=cur_functions->getlen()/sizeof(section);
  section *tmp=(section*)cur_functions->get();
  for (x = 0; x < n; x ++)
  {
    if (tmp[x].name_ptr == addr)
    {
      printf("Error: Function named \"%s\" already exists.\n",funname);
      return PS_ERROR;
    }
  }

  cur_functions->resize((n+1)*sizeof(section));
  build_cursection=((section*)cur_functions->get())+n;
  build_cursection_isfunc=1;
  build_cursection->name_ptr=addr;
  build_cursection->code[0]=cur_entries->getlen()/sizeof(entry);
  build_cursection->code[1]=build_cursection->code[0];
  build_cursection->default_state=0;
  build_cursection->size_kb=0;
  return PS_OK;
}

int CEXEBuild::function_end()
{
  if (!build_cursection_isfunc)
  {
    printf("Error: No function open, FunctionEnd called\n");
    return PS_ERROR;
  }
  build_cursection_isfunc=0;
  build_cursection=NULL;

  // add invalid opcode, useful for error checking, and
  // makes sure that labels at end of functions work properly.
  entry ent={0,};
  cur_entries->add(&ent,sizeof(entry));
  if (!uninstall_mode) build_header.num_entries++;
  else build_uninst.num_entries++;

  set_uninstall_mode(0);
  return PS_OK;
}


int CEXEBuild::section_add_flags(int flags)
{
  if (uninstall_mode)
  {
    printf("Error: can't modify flags of uninstall section\n");
    return PS_ERROR;
  }
  if (!build_cursection || build_cursection_isfunc)
  {
    printf("Error: can't modify flags when no section is open\n");
    return PS_ERROR;
  }
  build_cursection->default_state|=flags;
  return PS_OK;
}

void CEXEBuild::section_add_size_kb(int kb)
{
  if (build_cursection)
  {
    build_cursection->size_kb+=kb;
  }
}

int CEXEBuild::section_end()
{
  if (build_cursection_isfunc)
  {
    printf("Error: SectionEnd specified in function (not section)\n");
    return PS_ERROR;
  }
  else if (uninstall_mode)
  {
    set_uninstall_mode(0);
  }
  else if (!build_cursection)
  {
    printf("Error: SectionEnd specified and no sections open\n");
    return PS_ERROR;
  }
  else 
  {
    // add invalid opcode, useful for error checking, and
    // makes sure that labels at end of inst sections work properly.
    entry ent={0,};
    cur_entries->add(&ent,sizeof(entry));
    build_header.num_entries++;
  }
  build_cursection=NULL;
  return PS_OK;
}

int CEXEBuild::add_section(char *secname, char *file, int line)
{
  if (build_cursection_isfunc)
  {
    printf("Error: Section can't create section (already in function, use FunctionEnd first)\n");
    return PS_ERROR;
  }
  if (build_cursection || uninstall_mode)
  {
    printf("Error: Section already open, call SectionEnd first\n");
    return PS_ERROR;
  }
  if (!stricmp(secname,"uninstall"))
  {
    if (build_uninst.code[0] != build_uninst.code[1])
    {
      printf("Error: Uninstall section already specified\n");
      return PS_ERROR;
    }
    build_uninst.code[0]=ubuild_entries.getlen()/sizeof(entry);
    build_uninst.code[1]=build_uninst.code[0];
    set_uninstall_mode(1);
    build_cursection=NULL;
    return PS_OK;
  }
  else if (uninstall_mode)
  {
    set_uninstall_mode(0);
  }
  int n=(build_sections.getlen()/sizeof(section));
  build_sections.resize(build_sections.getlen()+sizeof(section));
  build_cursection=((section*)build_sections.get()) + n;
  build_cursection->default_state=(!n||!secname[0])?0x80000000:0;
  build_cursection->name_ptr=add_string(secname);
  build_cursection->code[0]=cur_entries->getlen()/sizeof(entry);
  build_cursection->code[1]=build_cursection->code[0];
  build_cursection->size_kb=0;

  if (secname[0]=='-')
  {
    if (!n)
    {
      printf("Error: SectionDivider cannot be first section\n");
      return PS_ERROR;
    }
    build_cursection=NULL;
  }
  
  build_header.num_sections++;

  return PS_OK;
}

int CEXEBuild::make_sure_not_in_secorfunc(char *str)
{
  if (build_cursection || uninstall_mode)
  {
    printf("Error: command %s not valid in %s\n",str,build_cursection_isfunc?"function":"section");
    return PS_ERROR;
  }
  return PS_OK;
}

int CEXEBuild::add_entry(entry *ent)
{
  if (!build_cursection && !uninstall_mode) 
  {
    printf("Error: Can't add entry, no section or function is open!\n");
    return PS_ERROR;
  }

  cur_entries->add(ent,sizeof(entry));

  if (!uninstall_mode)
  {
    if (!build_cursection_isfunc && build_cursection->name_ptr >=0 && build_strlist.get()[build_cursection->name_ptr] == '-')
    {
      printf("Error: cannot add entry to divider section\n");
      return PS_ERROR;
    }
    build_cursection->code[1]=++build_header.num_entries;
  }
  else
  {
    build_uninst.num_entries++;
    if (build_cursection) build_cursection->code[1]=build_uninst.num_entries;
    else build_uninst.code[1]=build_uninst.num_entries;
  }

  return PS_OK;
}

void CEXEBuild::printline(int l)
{
  while (l > 2) 
  {
    printf("-=");
    l-=2;
  }
  if (l) printf("-");
  printf("\n");
}


int CEXEBuild::resolve_jump_int(int *a, int offs, int start, int end)
{
  if (*a != 0)
  {
    section *s = (section*)cur_labels->get();
    int n=cur_labels->getlen()/sizeof(section);
    while (n-->0)
    {
      if (s->code[0] >= start && s->code[0] <= end && s->name_ptr == *a)
      {
//        printf("Resolved jump label \"%s\" to %d\n",(char*)ns_label.get()+*a,s->code_start-offs-1);
        *a = s->code[0]-offs - 1;
        s->default_state++;
        return 0;
      }
      s++;
    }

    printf("Error: resolve_jump_int: could not resolve jump \"%s\"\n",(char*)ns_label.get()+*a);
    return 1;
  }
  return 0;
}

int CEXEBuild::resolve_instruction(entry *w, int offs, int start, int end)
{
  if (w->which == EW_NOP)
  {
    if (resolve_jump_int(&w->offsets[0],offs,start,end)) return 1;
  }
  else if (w->which == EW_FINDWINDOW)
  {
    if (resolve_jump_int(&w->offsets[4],offs,start,end)) return 1;
  }
  else if (w->which == EW_MESSAGEBOX)
  {
    if (resolve_jump_int(&w->offsets[3],offs,start,end)) return 1;
  }
  else if (w->which == EW_IFFILEEXISTS)
  {
    if (resolve_jump_int(&w->offsets[1],offs,start,end)) return 1;
    if (resolve_jump_int(&w->offsets[2],offs,start,end)) return 1;
  }
  else if (w->which == EW_IFERRORS)
  {
    if (resolve_jump_int(&w->offsets[0],offs,start,end)) return 1;
    if (resolve_jump_int(&w->offsets[1],offs,start,end)) return 1;
  }
  else if (w->which == EW_STRCMP)
  {
    if (resolve_jump_int(&w->offsets[2],offs,start,end)) return 1;
    if (resolve_jump_int(&w->offsets[3],offs,start,end)) return 1;
  }
  else if (w->which == EW_COMPAREDLLS || w->which == EW_COMPAREFILETIMES)
  {
    if (resolve_jump_int(&w->offsets[2],offs,start,end)) return 1;
    if (resolve_jump_int(&w->offsets[3],offs,start,end)) return 1;
  }
  return 0;
}

int CEXEBuild::resolve_function(char *str, int fptr, int *ofs)
{
  if (fptr < 0) return 0;
  int nf=cur_functions->getlen()/sizeof(section);
  section *sec=(section *)cur_functions->get();
  while (nf-- > 0)
  {
    if (sec->name_ptr && sec->name_ptr == fptr)
    {
//      printf("Resolved %s function: \"%s\" to (%d,%d)\n",str,
  //      (char*)ns_func.get()+fptr,sec->code_start,sec->code_end);
      ofs[0]=sec->code[0];
      ofs[1]=sec->code[1];
      sec->default_state++;
      return 0;
    }
    sec++;
  }
  printf("Error resolving %s function \"%s\"\n",str,(char*)ns_func.get()+fptr);
  printf("Note: uninstall functions must begin with \"un.\", and install functions must not\n");
  return 1;
}


int CEXEBuild::resolve_functions(char *str)
{
  // resolve calls
  {
    int l=cur_entries->getlen()/sizeof(entry);
    if (l)
    {
      int x;
      entry *w=(entry*)cur_entries->get();
      for (x = 0; x < l; x ++)
      {
        if (w[x].which == EW_CALL) // call, that needs resolve
        {
          if (resolve_function(str,w[x].offsets[0],w[x].offsets)) return 1;
        }
      }
    }
  }

  // resolve jumps
  {
    section *sec=(section *)cur_functions->get();    
    int l=cur_functions->getlen()/sizeof(section);
    entry *w=(entry*)cur_entries->get();
    while (l-- > 0)
    {
      int x;
      for (x = sec->code[0]; x < sec->code[1]; x ++)
        if (resolve_instruction(w+x,x,sec->code[0],sec->code[1])) return 1;

      sec++;
    }
    if (uninstall_mode)
    {
      int x;
      for (x = build_uninst.code[0]; x < build_uninst.code[1]; x ++)
        if (resolve_instruction(w+x,x,build_uninst.code[0],build_uninst.code[1])) return 1;
    }
    else
    {
      sec=(section *)build_sections.get();
      l=build_sections.getlen()/sizeof(section);
      while (l-- > 0)
      {
        int x;
        for (x = sec->code[0]; x < sec->code[1]; x ++)
          if (resolve_instruction(w+x,x,sec->code[0],sec->code[1])) return 1;
        sec++;
      }
    }
  }

  // optimize unused functions
  {
    section *sec=(section *)cur_functions->get();    
    int l=cur_functions->getlen()/sizeof(section);
    entry *w=(entry*)cur_entries->get();
    while (l-- > 0)
    {
      if (sec->name_ptr)
      {
        if (!sec->default_state)
        {
          if (sec->code[1]>sec->code[0])
          {
            printf("%s function \"%s\" not referenced - zeroing code (%d-%d) out\n",str,
              ns_func.get()+sec->name_ptr,
              sec->code[0],sec->code[1]);
            memset(w+sec->code[0],0,(sec->code[1]-sec->code[0])*sizeof(entry));
          }
        }
      }
      sec++;
    }
  }

  // give warnings on unused labels
  {
    section *t=(section*)cur_labels->get();
    int n=cur_labels->getlen()/sizeof(section);
    while (n-->0)
    {
      if (!t->default_state)
      {
        warning("label \"%s\" not used",(char*)ns_label.get()+t->name_ptr);
      }
      t++;
    }
  }
  
  return 0;
}

int CEXEBuild::write_output(void)
{
  if (has_called_write_output)
  {
    printf("Error (write_output): write_output already called, can't continue\n");
    return PS_ERROR;
  }
  has_called_write_output++;
  if (!build_output_filename[0])
  {
    printf("Error: invalid script: never had OutFile command\n");
    return PS_ERROR;
  }

  {
    int ns=build_sections.getlen()/sizeof(section);
    if (!ns)
    {
      printf("Error: invalid script: no sections specified\n");
      return PS_ERROR;
    }

    if ((build_header.componenttext_ptr < 0 ||
        !build_strlist.get()[build_header.componenttext_ptr]) &&
        ns > 1 &&
        !build_header.silent_install
        )
    {
      section *s=(section*)build_sections.get();
      while (ns--)
      {
        s->default_state|=0x80000000;
        s++;
      }
      printf("Component Page disabled, making all sections enabled by default\n");
    }
  }
  if (!build_entries.getlen())
  {
    printf("Error: invalid script: no entries specified\n");
    return PS_ERROR;
  }

  if (build_header.name_ptr < 0)
  {
    warning("Name command not specified. Assuming default.");
    build_header.name_ptr=add_string_main("Name");
    build_uninst.name_ptr=add_string_uninst("Name");
  }

#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
  if (ubuild_entries.getlen() && build_header.uninstall_exe_name_ptr < 0)
  {
    warning("Uninstall turned on but UninstallExeName command not specified. Assuming default.");
    build_header.uninstall_exe_name_ptr=add_string_main("nsuninst.exe");
  }
#endif

  if (build_cursection || uninstall_mode)
  {
    printf("Error: Section left open at EOF\n");
    return PS_ERROR;
  }
  
  // deal with functions, for both install and uninstall modes.
  if (build_cursection_isfunc)
  {
    printf("Error: Function still open at EOF, cannot proceed\n");
    return 1;
  }
  set_uninstall_mode(1);
  if (resolve_functions("uninstall")) return PS_ERROR;
  set_uninstall_mode(0);

  if (resolve_function("install callback",ns_func.find(".onInit",0),build_header.code_onInit)) return PS_ERROR;
  if (resolve_function("install callback",ns_func.find(".onInstSuccess",0),build_header.code_onInstSuccess)) return PS_ERROR;
  if (resolve_function("install callback",ns_func.find(".onInstFailed",0),build_header.code_onInstFailed)) return PS_ERROR;
  if (resolve_function("install callback",ns_func.find(".onUserAbort",0),build_header.code_onUserAbort)) return PS_ERROR;
  if (resolve_function("install callback",ns_func.find(".onVerifyInstDir",0),build_header.code_onVerifyInstDir)) return PS_ERROR;
  
  if (resolve_functions("install")) return PS_ERROR;


  if (build_header.caption_ptr < 0)
  {
    char buf[1024];
    wsprintf(buf,"%s Setup",build_strlist.get()+build_header.name_ptr);
    build_header.caption_ptr=add_string_main(buf);
  }
 
  if (build_packname[0] && build_packcmd[0])
  {
    FILE *tmpfile=fopen(build_packname,"wb");
    if (!tmpfile)
    {
      printf("Error: writing temporary file \"%s\" for pack\n",build_packname);
      return PS_ERROR;
    }
    fwrite(header_data_new,1,exeheader_size_new,tmpfile);
    fclose(tmpfile);
    if (system(build_packcmd) == -1)
    {
      remove(build_packname);
      printf("Error: calling packer on \"%s\"\n",build_packname);
      return PS_ERROR;
    }
    tmpfile=fopen(build_packname,"rb");
    if (!tmpfile)
    {
      remove(build_packname);
      printf("Error: reading temporary file \"%s\" after pack\n",build_packname);
      return PS_ERROR;
    }
    fseek(tmpfile,0,SEEK_END);
    exeheader_size_new=ftell(tmpfile);
    exeheader_size_new+=511;
    exeheader_size_new&=~511; // align to 512.
    fseek(tmpfile,0,SEEK_SET);
    unsigned char *header_data_older=header_data_new;
    header_data_new=(unsigned char *)malloc(exeheader_size_new);
    if (!header_data_new)
    {
      free(header_data_older);
      fclose(tmpfile);
      printf("Error: malloc(%d) failed (exepack)\n",exeheader_size_new);
      return PS_ERROR;
    }
    memset(header_data_new,0,exeheader_size_new);
    fread(header_data_new,1,exeheader_size_new,tmpfile);
    fclose(tmpfile);
    remove(build_packname);

    printf("Locating install icon after compress: ");
    icon_offset = find_data_offset((char*)header_data_new,exeheader_size_new,
                     (char*)header_data_older+icon_offset,ICO_HDRSKIP,sizeof(icon_data));
    free(header_data_older);
    if (icon_offset < 0)
    {
      return PS_ERROR;
    }
    printf("found at offset %d\n",icon_offset);
    // write out exe header, pack, read back in, align to 512, and
    // update the header info
  }

  build_optimize_datablock=0;

  if (uninstall_generate() != PS_OK) 
  {
    return PS_ERROR;
  }

  int crc=0;

  printf("\n");
  printline(79);

  printf("Output: \"%s\"\n",build_output_filename);
  FILE *fp = fopen(build_output_filename,"wb");
  if (!fp) 
  {
    perror("Can't open output file");
    return PS_ERROR;
  }

  if ((int)fwrite(header_data_new,1,exeheader_size_new,fp) != exeheader_size_new)
  {
    printf("Error: can't write %d bytes to output\n",exeheader_size_new);
    fclose(fp);
    return PS_ERROR;
  }
  crc=adler32(crc,header_data_new,exeheader_size_new);

  firstheader fh={0,};
  fh.nsinst[0]=FH_INT1;
  fh.nsinst[1]=FH_INT2;
  fh.nsinst[2]=FH_INT3;
  fh.flags=(build_crcchk?FH_FLAGS_CRC:0);
  if (build_header.silent_install) fh.flags |= FH_FLAGS_SILENT;
  fh.siginfo=FH_SIG;

  int installinfo_compressed;
  {
    GrowBuf hdrcomp;
    hdrcomp.add(&build_header,sizeof(build_header)); 
    hdrcomp.add(build_sections.get(),build_sections.getlen()); 
    hdrcomp.add(build_entries.get(),build_entries.getlen());
    hdrcomp.add(build_strlist.get(),build_strlist.getlen());

    installinfo_compressed=-build_datablock.getlen();
    fh.header_ptr = add_data((char*)hdrcomp.get(),hdrcomp.getlen());
    installinfo_compressed+=build_datablock.getlen()-sizeof(int);

 
    fh.length_of_header=hdrcomp.getlen();
    if (fh.header_ptr < 0) return PS_ERROR;
  }

  fh.length_of_all_following_data=build_datablock.getlen()+(int)sizeof(firstheader)+(build_crcchk?sizeof(int):0);

  int do_padding=0;
  if (build_crcchk && fh.length_of_all_following_data < 516)
  {
    do_padding=516-fh.length_of_all_following_data;
    fh.length_of_all_following_data=516;
  }

  if (fwrite(&fh,1,sizeof(fh),fp) != sizeof(fh))
  {
    printf("Error: can't write %d bytes to output\n",sizeof(fh));
    fclose(fp);
    return PS_ERROR;
  }
  crc=adler32(crc,(unsigned char*)&fh,sizeof(fh));

  {
    int ns=build_sections.getlen()/sizeof(section);
    section *s=(section*)build_sections.get();
    int x;
    int req=1;
    int div=0;
    int divptr=build_strlist.find("-",2);
    for (x = 1; x < ns; x ++)
    {
      if (s[x].name_ptr == divptr) div++;
      if (s[x].name_ptr == -1)  req++;
    }
    printf("Install: %d section%s",ns,ns==1?"":"s");
    if (req||div) 
    {
      printf(" (");
      if (req) 
      {
        printf("%d required",req);
        if (div) printf(", ");
      }
      if (div) printf("%d divider%s",div,div==1?"":"s");
      printf(")");
    }
    printf(".\n");
  }
  int ne=build_entries.getlen()/sizeof(entry);
  printf("Install: %d instruction%s, ",ne,ne==1?"":"s");
  printf("%d byte string table.\n",build_strlist.getlen());
  if (ubuild_entries.getlen()) 
  {
    int tmp=ubuild_entries.getlen()/sizeof(entry);
    printf("Uninstall: ");
    printf("%d instruction%s, ",tmp,tmp==1?"":"s");
    printf("%d byte string table.\n",ubuild_strlist.getlen());
  }


  if (db_opt_save) 
  {
    int total_out_size_estimate=
      exeheader_size_new+sizeof(fh)+build_datablock.getlen()+(build_crcchk?4:0);
    int pc=MulDiv(db_opt_save,1000,db_opt_save+total_out_size_estimate);
    printf("Datablock optimizer saved %d bytes (~%d.%d%%).\n",db_opt_save,
      pc/10,pc%10);
  }

  printf("\n");
  int total_usize=exeheader_size;

  printf("EXE header size:        %10d / %d bytes\n",exeheader_size_new,exeheader_size);

  printf("Install code+strings:   %10d / %d bytes\n",
    sizeof(fh)+installinfo_compressed+sizeof(int),
    sizeof(fh)+fh.length_of_header+sizeof(int)); 

  total_usize+=sizeof(fh)+fh.length_of_header+sizeof(int);

  {
    int dbsize, dbsizeu;
    dbsize = build_datablock.getlen()-installinfo_compressed - sizeof(int);
    if (uninstall_size>0) dbsize-=uninstall_size;

    dbsizeu = db_full_size-fh.length_of_header - sizeof(int) - uninstall_size_full;

    printf("Install data:           %10d / %d bytes\n",dbsize,dbsizeu);
    total_usize+=dbsizeu;
  }


  if (build_datablock.getlen())
  {
    char *dbptr=(char *)build_datablock.get();
    int dbl=build_datablock.getlen();
    while (dbl > 0)
    {
      int l=dbl;
      if (l > 32768) l=32768;
      crc=adler32(crc,(unsigned char *)dbptr,l);
      if ((int)fwrite(dbptr,1,l,fp) != l)
      {
        printf("Error: can't write %d bytes to output\n",l);
        fclose(fp);
        return PS_ERROR;
      }
      dbptr+=l;
      dbl-=l;
    }
  }

  if (uninstall_size>=0) 
  {
    printf("Uninstall code+data+strings:%6d / %d bytes\n",uninstall_size,uninstall_size_full);
    total_usize+=uninstall_size_full;
  }
  if (do_padding)
  {
    unsigned char buf[516];
    memset(buf,0,sizeof(buf));
    if ((int)fwrite(buf,1,do_padding,fp) != do_padding)
    {
      printf("Error: can't write %d bytes to output\n",do_padding);
      fclose(fp);
      return PS_ERROR;
    }
    crc=adler32(crc,buf,do_padding);
    printf("Padding:                  %8d / %d bytes\n",do_padding,do_padding);
    total_usize+=do_padding;
  }

  if (build_crcchk) 
  {
    total_usize+=sizeof(int);
    if (fwrite(&crc,1,sizeof(int),fp) != sizeof(int))
    {
      printf("Error: can't write %d bytes to output\n",sizeof(int));
      fclose(fp);
      return PS_ERROR;
    }
    printf("CRC (0x%08X):                4 / 4 bytes\n",crc);
  }

  printline(57);
  {
    int pc=MulDiv(ftell(fp),1000,total_usize);
    printf("Total size:             %10d / %d bytes (%d.%d%%)\n",ftell(fp),total_usize,pc/10,pc%10);
  }
  fclose(fp);

  // mpg code mod start
  
  CMD5 md5;
  md5.FromFile(build_output_filename);
  printf("\nMD5:  ");
  printf(md5.GetString());
  printf("\n");
  
  // mpg code mod end
  
  
  
  print_warnings();
  return PS_OK;
}


int CEXEBuild::uninstall_generate()
{
#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
  if (ubuild_entries.getlen()) 
  {
    firstheader fh={0,};

    // add one more bit (the code+strtabs) to the uninstall datablock
    {
      GrowBuf udata;

      udata.add(&build_uninst,sizeof(build_uninst));
      udata.add(ubuild_entries.get(),ubuild_entries.getlen());
      udata.add(ubuild_strlist.get(),ubuild_strlist.getlen()); 
    
      set_uninstall_mode(1);
      fh.length_of_header=udata.getlen();
      fh.header_ptr=add_data((char*)udata.get(),udata.getlen());      
      set_uninstall_mode(0);
      if (fh.header_ptr < 0) return PS_ERROR;
    }
        
    int crc=0;

    build_header.uninstdata_offset=build_datablock.getlen();
    build_header.uninstexehead_iconoffset = icon_offset+ICO_HDRSKIP;

    if (add_data((char *)m_unicon_data+ICO_HDRSKIP,766-ICO_HDRSKIP) < 0)
      return PS_ERROR;
    crc=adler32(crc,header_data_new,icon_offset+ICO_HDRSKIP);
    crc=adler32(crc,m_unicon_data+ICO_HDRSKIP,766-ICO_HDRSKIP);
    crc=adler32(crc,header_data_new+icon_offset+766,exeheader_size_new-766-icon_offset);

    fh.nsinst[0]=FH_INT1;
    fh.nsinst[1]=FH_INT2;
    fh.nsinst[2]=FH_INT3;
    fh.flags = FH_FLAGS_UNINSTALL | (build_crcchk?FH_FLAGS_CRC:0);
    fh.siginfo=FH_SIG;
    fh.length_of_all_following_data=
      ubuild_datablock.getlen()+(int)sizeof(firstheader)+(build_crcchk?sizeof(int):0);

    if (build_crcchk && fh.length_of_all_following_data < 516)
    {
      fh.length_of_all_following_data=516;
    }

    GrowBuf udata;
    udata.add(&fh,sizeof(fh));
    udata.add(ubuild_datablock.get(),ubuild_datablock.getlen());

    if (build_crcchk)
    {
      if (udata.getlen() < 512)
      {
        char buf[512];
        memset(buf,0,512);
        udata.add(buf,512-udata.getlen());
      }
      int s=adler32(crc,(unsigned char*)udata.get(),udata.getlen());
      udata.add(&s,sizeof(int));
    }

    if (add_data((char*)udata.get(),fh.length_of_all_following_data) < 0) 
      return PS_ERROR;

    uninstall_size_full=fh.length_of_all_following_data + sizeof(int) + 766 - 32 + sizeof(int);

    // compressed size
    uninstall_size=build_datablock.getlen()-build_header.uninstdata_offset;
  }
#endif
  return PS_OK;
}


#define SWAP(x,y,i) { i _ii; _ii=x; x=y; y=_ii; }

void CEXEBuild::set_uninstall_mode(int un)
{
  if (un != uninstall_mode)
  {
    uninstall_mode=un;
    if (un) cur_datablock=&ubuild_datablock;
    else cur_datablock=&build_datablock;
    if (un) cur_entries=&ubuild_entries;
    else cur_entries=&build_entries;
    if (un) cur_functions=&ubuild_functions;
    else cur_functions=&build_functions;
    if (un) cur_labels=&ubuild_labels;
    else cur_labels=&build_labels;

    SWAP(db_opt_save_u,db_opt_save,int);
    SWAP(db_comp_save_u,db_comp_save,int);
    SWAP(db_full_size_u,db_full_size,int);
  }
}

void CEXEBuild::warning(char *s, ...)
{
  char buf[4096];
  va_list val;
  va_start(val,s);
  vsprintf(buf,s,val);
  va_end(val);
  m_warnings.add(buf,-1);
  printf("warning: %s\n",buf);
}

void CEXEBuild::print_warnings()
{
  int nw=0,x=m_warnings.getlen();
  if (!x) return;
  char *p=m_warnings.get();
  while (x>0) if (!p[--x]) nw++;
  printf("\n%d warning%s:\n",nw,nw==1?"":"s");
  for (x = 0; x < nw; x ++)
  {
    printf("  %s\n",p);
    p+=strlen(p)+1;
  }
}