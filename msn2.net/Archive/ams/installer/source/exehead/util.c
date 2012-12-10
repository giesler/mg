#include <windows.h>
#include <shlobj.h>
#include "util.h"
#include "state.h"
#include "libc.h"
#include "config.h"

#include "fileform.h"

#ifdef NSIS_CONFIG_LOG
char g_log_file[1024];
#endif

char g_usrvars[12][1024];
char *state_install_directory=g_usrvars[10];
char *state_output_directory=g_usrvars[11];

HANDLE g_hInstance;

int is_valid_instpath(char *s)
{
  int ivp=0;
  if (s[0] == '\\' && s[1] == '\\') // \\ path
  {
    if (s[lstrlen(s)-1]!='\\') ivp++;
    while (*s) if (*s++ == '\\') ivp++;
    ivp/=5;
  }
  else
  {
    if (s[0] && s[1] == ':' && s[2] == '\\' && s[3] && s[3]!='\\') ivp++;
  }
  return ivp;
}

static char *findinmem(char *a, char *b, int len_of_a)
{
  len_of_a -= lstrlen(b);
  while (*a && len_of_a >= 0)
  {
    char *t=a,*u=b;
    while (*t && *t == *u)
    {
      t++;
      u++;
    }
    if (!*u) return a;
    a++;
    len_of_a--;
  }
  return NULL;
}

BOOL MoveFileOnReboot(LPCTSTR pszExisting, LPCTSTR pszNew)
{
  BOOL fOk = 0;
  HMODULE hLib=LoadLibrary("kernel32.dll");
  if (hLib)
  {
    typedef BOOL (WINAPI *mfea_t)(LPCSTR lpExistingFileName,LPCSTR lpNewFileName,DWORD dwFlags);
    mfea_t mfea;
    mfea=(mfea_t) GetProcAddress(hLib,"MoveFileExA");
    if (mfea)
    {
      fOk=mfea(pszExisting, pszNew, MOVEFILE_DELAY_UNTIL_REBOOT|MOVEFILE_REPLACE_EXISTING);
    }
    FreeLibrary(hLib);
  }

  if (!fOk)
  {
#if 0
    if (pszNew)
    {
      // contributed by Francis Irving.. supports long filenames,
      // change that #if 0 to #if 1 to try it out (I haven't really yet)

      HKEY runOnceKey;
      if (RegOpenKeyEx(
             HKEY_LOCAL_MACHINE,
             "Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce",
             0,
             KEY_SET_VALUE,
             &runOnceKey ) == ERROR_SUCCESS)
      {
        char *buf = (char*)GlobalAlloc(GMEM_FIXED,2048);
        lstrcpy(buf, "command.com /c move \"");
        lstrcat(buf, pszExisting);
        lstrcat(buf, "\" \"");
        lstrcat(buf, pszNew);
        lstrcat(buf, "\"");
        if (RegSetValueEx(runOnceKey,pszExisting,0,
                     REG_SZ, buf, lstrlen(buf) + 1 ) == ERROR_SUCCESS) fOk++;
        RegCloseKey(runOnceKey);
        GlobalFree(buf);
      }
    }
    else
#endif
    {
      char *szRenameLine=(char*)GlobalAlloc(GPTR,1024*3);   
      int cchRenameLine;
      char *szRenameSec = "[Rename]\r\n";
      HANDLE hfile, hfilemap;
      DWORD dwFileSize, dwRenameLinePos;
      char *wininit=szRenameLine+1024;
      char *tmpbuf=szRenameLine+2048;
      static char nulint[4]="NUL";

      if (pszNew) GetShortPathName(pszNew,tmpbuf,1024);
      else *((int *)tmpbuf) = *((int *)nulint);
      // wininit is used as a temporary here
      GetShortPathName(pszExisting,wininit,1024);
      pszExisting=wininit;
      cchRenameLine = wsprintf(szRenameLine,"%s=%s\r\n",tmpbuf,pszExisting);
    
      GetWindowsDirectory(wininit, 1024-16);
      lstrcat(wininit, "\\wininit.ini");
      hfile = CreateFile(wininit,      
          GENERIC_READ | GENERIC_WRITE, 0, NULL, OPEN_ALWAYS, 
          FILE_ATTRIBUTE_NORMAL | FILE_FLAG_SEQUENTIAL_SCAN, NULL);

      if (hfile != INVALID_HANDLE_VALUE) 
      {
        dwFileSize = GetFileSize(hfile, NULL);
        hfilemap = CreateFileMapping(hfile, NULL, PAGE_READWRITE, 0, dwFileSize + cchRenameLine + 10, NULL);

        if (hfilemap != NULL) 
        {
          LPSTR pszWinInit = (LPSTR) MapViewOfFile(hfilemap, FILE_MAP_WRITE, 0, 0, 0);

          if (pszWinInit != NULL) 
          {
            int do_write=0;
            LPSTR pszRenameSecInFile = mini_strstr(pszWinInit, szRenameSec);
            if (pszRenameSecInFile == NULL) 
            {
              lstrcpy(pszWinInit+dwFileSize, szRenameSec);
              dwFileSize += 10;
              dwRenameLinePos = dwFileSize;
              do_write++;
            } 
            else 
            {
              char *pszFirstRenameLine = mini_strstr(pszRenameSecInFile, "\n")+1;
              int l=pszWinInit + dwFileSize-pszFirstRenameLine;
              if (!findinmem(pszFirstRenameLine,szRenameLine,l))
              {
                mini_memmove(pszFirstRenameLine + cchRenameLine, pszFirstRenameLine, l);                  
                dwRenameLinePos = pszFirstRenameLine - pszWinInit;
                do_write++;
              }
            }

            if (do_write) 
            {
              mini_memcpy(&pszWinInit[dwRenameLinePos], szRenameLine,cchRenameLine);
              dwFileSize += cchRenameLine;
            }

            UnmapViewOfFile(pszWinInit);

            fOk++;
          }
          CloseHandle(hfilemap);
        }
        SetFilePointer(hfile, dwFileSize, NULL, FILE_BEGIN);
        SetEndOfFile(hfile);
        CloseHandle(hfile);
      }
      GlobalFree(szRenameLine);
    }
  }
  return fOk;
}

void recursive_create_directory(char *directory)
{
	char *p;
  p=directory;
  while (*p == ' ') p++;
  if (!*p) return;
  if (p[1] == ':' && p[2] == '\\') p+=2;
  else if (p[0] == '\\' && p[1] == '\\')
  {
    while (*p != '\\' && *p) p++; // skip host
    if (*p) p++;
    while (*p != '\\' && *p) p++; // skip share
    if (*p) p++;
  }
  else return;
  while (*p)
  {
    while (*p != '\\' && *p) p++;
    if (!*p) CreateDirectory(directory,NULL);
    else
    {
      *p=0;
  	  CreateDirectory(directory,NULL);
      *p++ = '\\';
    }
  }
}

static int strcmp_nstr2(char **s1, char *s2)
{
  char *ps1=*s1;
  while (*ps1 && *ps1 == *s2) { ps1++; s2++; }
  if (!*s2) *s1=ps1;
  return *s2;
}

static void queryShellFolders(char *name, char *out)
{
	HKEY hKey;
	if ( RegOpenKeyEx(HKEY_CURRENT_USER,"Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders",0,KEY_READ,&hKey) == ERROR_SUCCESS)
  {
		int l = 1024;
		int t=REG_SZ;
		RegQueryValueEx(hKey,name,NULL,&t,out,&l );
    RegCloseKey(hKey);
  }
}

int process_string_fromtab(char *out, int offs)
{
  return process_string(out,GetStringFromStringTab(offs));
}


// returns 0 if out==in
// returns 2 if invalid symbol
// returns 3 if error looking up symbol
int process_string(char *out, char *in)
{
  static char *_desktop="DESKTOP";
  static char *_smprograms="SMPROGRAMS";
  static char *_smstartup="SMSTARTUP";
  static const char winamp[] = "Winamp";
  char *insave=in;
  char *outsave=out;
  while (*in)
  {
    char rts=0;
    static char buf[1024];
    buf[0]=0;
    if (*in != '$') *out++=*in++;
    else 
    {
      in++;
      if (*in == '$')
      {
        *out++=*in++;
      }
      else
      {
        if (in[0] >= '0' && in[0] <= '9')
        {
          lstrcpy(out,g_usrvars[in[0]-'0']);
          in++;
          rts++;
        }
        else if (!strcmp_nstr2(&in,"PROGRAMFILES"))
        {
		      HKEY hKey;
          lstrcpy(out,"C:\\Program Files");
		      if ( RegOpenKeyEx(HKEY_LOCAL_MACHINE,"SOFTWARE\\Microsoft\\Windows\\CurrentVersion",0,KEY_READ,&hKey) == ERROR_SUCCESS)
          {
			      int l=1024;
			      int t=REG_SZ;
			      RegQueryValueEx(hKey,"ProgramFilesDir",NULL,&t,out,&l);
			      RegCloseKey(hKey);
          }
        }
        else if (!strcmp_nstr2(&in,"HWNDPARENT"))
        {
          wsprintf(out,"%u",(unsigned int)g_hwnd);
        }
        else if (!strcmp_nstr2(&in,_desktop))
        {
          queryShellFolders(_desktop,out);
          if (!out[0]) goto copyst;
        }
        else if (!strcmp_nstr2(&in,"EXEDIR"))
        {
          lstrcpy(out,state_exe_directory);
        }
        else if (!strcmp_nstr2(&in,"INSTDIR"))
        {
          lstrcpy(out,state_install_directory);
        }
        else if (!strcmp_nstr2(&in,"OUTDIR"))
        {
          lstrcpy(out,state_output_directory);
        }
        else if (!strcmp_nstr2(&in,"WINDIR"))
        {
          GetWindowsDirectory(out,1024);
        }
        else if (!strcmp_nstr2(&in,"SYSDIR"))
        {
          GetSystemDirectory(out,1024);
        }
        else if (!strcmp_nstr2(&in,"TEMP"))
        {
          GetTempPath(1024,out);
        }
        else if (!strcmp_nstr2(&in,"STARTMENU"))
        {
          queryShellFolders("Start Menu",out);
          if (!out[0]) goto copyst;
        }
        else if (!strcmp_nstr2(&in,_smprograms))
        {
          queryShellFolders(_smprograms+2,out);
          if (!out[0]) goto copyst;
        }
        else if (!strcmp_nstr2(&in,_smstartup))
        {
          queryShellFolders(_smstartup+2,out);
          if (!out[0]) goto copyst;
        }
        else if (!strcmp_nstr2(&in,"QUICKLAUNCH"))
        {
          queryShellFolders("AppData",buf);
          if (buf[0])
          {
            DWORD f;
            lstrcat(buf,"\\Microsoft\\Internet Explorer\\Quick Launch");
            f=GetFileAttributes(buf);
            if (f == (DWORD)-1 || !(f&FILE_ATTRIBUTE_DIRECTORY)) buf[0]=0;
          }
          if (!buf[0])
          {
            GetTempPath(1024,buf);
          }
          lstrcpy(out,buf);
        }
        else goto copyst;
        if (!rts) // remove trailing slash
        {
          while (*out && out[1]) out++;
          if (*out=='\\') *out=0;
        }
        while (*out) out++;
      }
    } // *in == $
  } // while
  *out=0;
  return 0;
copyst:
  lstrcpy(outsave,insave);
  return 3;
}

#ifdef NSIS_CONFIG_LOG

char log_text[4096];
int log_dolog;
void log_write(int close)
{ 
  extern char g_log_file[1024];
  static HANDLE fp=INVALID_HANDLE_VALUE;
  if (close)
  {
    if (fp!=INVALID_HANDLE_VALUE) 
    {
      CloseHandle(fp);
    }
    fp=INVALID_HANDLE_VALUE;
    return;
  }
  if (log_dolog)
  {
    if (g_log_file[0] && fp==INVALID_HANDLE_VALUE)
    {
      fp = CreateFile(g_log_file,GENERIC_WRITE,FILE_SHARE_READ,NULL,OPEN_ALWAYS,0,NULL);
      if (fp!=INVALID_HANDLE_VALUE) 
        SetFilePointer(fp,0,NULL,FILE_END);
    }
    if (fp!=INVALID_HANDLE_VALUE)
    {
      DWORD d;
      lstrcat(log_text,"\r\n");
      WriteFile(fp,log_text,lstrlen(log_text),&d,NULL);
    }
  }
}


#endif


int CreateShortCut(HWND hwnd, LPCSTR pszShortcutFile, LPCSTR pszIconFile, int iconindex, LPCSTR pszExe, LPCSTR pszArg, LPCSTR workingdir, int showmode, int hotkey)
{
  HRESULT hres;
  int rv=1;
  IShellLink* psl;
  hres=OleInitialize(NULL);
  if (hres != S_FALSE && hres != S_OK) return rv;

  hres = CoCreateInstance(&CLSID_ShellLink, NULL, CLSCTX_INPROC_SERVER,
                            &IID_IShellLink, (void **) &psl);
  if (SUCCEEDED(hres))
  {
    IPersistFile* ppf;

    hres = psl->lpVtbl->QueryInterface(psl,&IID_IPersistFile, (void **) &ppf);
    if (SUCCEEDED(hres))
    {
      WCHAR wsz[1024];
      MultiByteToWideChar(CP_ACP, 0, pszShortcutFile, -1, wsz, 1024);

       hres = psl->lpVtbl->SetPath(psl,pszExe);
       psl->lpVtbl->SetWorkingDirectory(psl,workingdir);
       if (showmode) psl->lpVtbl->SetShowCmd(psl,showmode);
       if (hotkey) psl->lpVtbl->SetHotkey(psl,(unsigned short)hotkey);
       if (pszIconFile) psl->lpVtbl->SetIconLocation(psl,pszIconFile,iconindex);
       if (pszArg) 
       {
         psl->lpVtbl->SetArguments(psl,pszArg);
       }

       if (SUCCEEDED(hres))
       {
		      hres=ppf->lpVtbl->Save(ppf,(const WCHAR*)wsz,TRUE);
          if (SUCCEEDED(hres)) rv=0;
       }
      ppf->lpVtbl->Release(ppf);
    }
    psl->lpVtbl->Release(psl);
  }
  OleUninitialize();
  return rv;
}
