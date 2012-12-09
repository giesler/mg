#include <windows.h>
#include <shlobj.h>
#include "fileform.h"
#include "util.h"
#include "state.h"
#include "ui.h"
#include "libc.h"
#include "exec.h"

char *g_deletefilecolon="Delete file: ";
char *g_errdll="Error registering DLL";
char *g_errdecomp="Error decompressing data! Installer corrupted.";

// return EXEC_ERROR (0x10000000) on error, otherwise, if return is >0,
// advance by 1+return, or if return < 0, move back by return
// or 0 is advance by 1.

static int file_exists(char *buf)
{
  int a=0;
  HANDLE h;
  WIN32_FIND_DATA fd;
  h = FindFirstFile(buf,&fd);
  if (h != INVALID_HANDLE_VALUE) 
  {
    FindClose(h);
    a++;
  }
  return a;
}

static void doRMDir(char *buf, int recurse)
{
  if (recurse)
  {
    HANDLE h;
    WIN32_FIND_DATA fd;
    char *p=buf;
    while (*p) p++;
    lstrcpy(p,"\\*.*");
    h = FindFirstFile(buf,&fd);
    if (h != INVALID_HANDLE_VALUE) 
    {
      do
      {
        if (fd.cFileName[0] != '.' ||
            (fd.cFileName[1] != '.' && fd.cFileName[1]))
        {
          lstrcpy(p+1,fd.cFileName);
          if (fd.dwFileAttributes & FILE_ATTRIBUTE_READONLY) 
            SetFileAttributes(buf,fd.dwFileAttributes^FILE_ATTRIBUTE_READONLY);
          if (fd.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) doRMDir(buf,recurse);
          else 
          {
            update_status_text(g_deletefilecolon,buf);
            DeleteFile(buf);
          }
        }
      } while (FindNextFile(h,&fd));
      FindClose(h);
    }
    p[0]=0; // fix buffer
  }
  log_printf2("RMDir: RemoveDirectory(\"%s\")",buf);
  update_status_text("Removing directory: ",buf);
  RemoveDirectory(buf);
}



// based loosely on code from Tim Kosse
// in win9x this isn't necessary (RegDeleteKey() can delete a tree of keys),
// but in win2k you need to do this manually.
static LONG myRegDeleteKeyEx(HKEY thiskey, LPCTSTR lpSubKey)
{
	HKEY key;
	int retval=RegOpenKey(thiskey,lpSubKey,&key);
	if (retval==ERROR_SUCCESS)
	{
		char buffer[1024];
		while (RegEnumKey(key,0,buffer,1024)==ERROR_SUCCESS)
      if ((retval=myRegDeleteKeyEx(key,buffer)) != ERROR_SUCCESS) break;
		RegCloseKey(key);
		retval=RegDeleteKey(thiskey,lpSubKey);
	}
	return retval;
}



extern HWND insthwnd,insthwndbutton;

extern int g_show_details;

int exec_errorflag;


int ExecuteCodeSegment(entry *entries, int range[2], HWND hwndProgress) 
{
  int pos=range[0];
  while (pos < range[1])
  {
    int rv=ExecuteEntry(entries,pos);
    if (rv == EXEC_ERROR) return rv;
    pos+=++rv;
    if (pos<range[0]) pos=range[0];
    if (hwndProgress) SendMessage(hwndProgress,PBM_DELTAPOS,rv,0);
    Sleep(1);
  }
  return 0;
}


int ExecuteEntry(entry *entries, int pos)
{
  int x;
  entry *thisentry=entries+pos;
  static char buf[1024],buf2[1024],buf3[1024],buf4[1024];
  switch (thisentry->which)
  {
    case EW_CALL:
      log_printf3("Call: %d->%d",thisentry->offsets[0],thisentry->offsets[1]); 
    return ExecuteCodeSegment(entries,thisentry->offsets,NULL);
    case EW_NOP: 
      log_printf2("Jump: %d",thisentry->offsets[0]); 
    return thisentry->offsets[0];
    case EW_RENAME:
      {
        process_string_fromtab(buf,thisentry->offsets[0]);
        process_string_fromtab(buf2,thisentry->offsets[1]);
        lstrcpy(buf4,buf);
        lstrcat(buf4,"->");
        lstrcat(buf4,buf2);
        log_printf2("Rename: %s",buf4);
        if (MoveFile(buf,buf2))
        {
          update_status_text("Rename: ",buf4);
        }
        else
        {
          if (thisentry->offsets[2] && file_exists(buf))
          {
            MoveFileOnReboot(buf,buf2);
            update_status_text("Rename on reboot: ",buf4);
            log_printf2("Rename on reboot: %s",buf4);
          }
          else
          {
            exec_errorflag++;
            log_printf2("Rename failed: %s",buf4);
          }
        }
      }
    return 0;
    case EW_UPDATETEXT:
      process_string_fromtab(buf4,thisentry->offsets[0]);
      log_printf2("detailprint: %s",buf4);
      update_status_text(buf4,"");
    return 0;
    case EW_SLEEP:
      x=thisentry->offsets[0];
      if (x < 1) x=1;
      update_status_text("Wait...","");
      log_printf2("Sleep(%d)",x);
      Sleep(x);
    return 0;
    case EW_HIDEWINDOW:
      log_printf("HideWindow");
      ShowWindow(g_hwnd,SW_HIDE);
    return 0;
    case EW_BRINGTOFRONT:
      log_printf("BringToFront");
      ShowWindow(g_hwnd,SW_SHOW);
      SetForegroundWindow(g_hwnd);
    return 0;
    case EW_ABORT:
      {
        char *p=GetStringFromStringTab(thisentry->offsets[0]);
        log_printf2("Aborting: \"%s\"",p);
        update_status_text("",p);
      }
    return EXEC_ERROR;
    case EW_CHDETAILSVIEW:
      if (thisentry->offsets[0])
      {
        if (insthwndbutton) ShowWindow(insthwndbutton,SW_HIDE);
        if (insthwnd) ShowWindow(insthwnd,SW_SHOWNA);
      }
      else
      {
        if (insthwndbutton && g_show_details!=2) ShowWindow(insthwndbutton,SW_SHOWNA);
        if (insthwnd) ShowWindow(insthwnd,SW_HIDE);
      }
    return 0;
    case EW_SETFILEATTRIBUTES:
      process_string_fromtab(buf,thisentry->offsets[0]);
      log_printf3("SetFileAttributes: \"%s\":%08X",buf,thisentry->offsets[1]);
      if (!SetFileAttributes(buf,thisentry->offsets[1]))
      {
        exec_errorflag++;
        log_printf("SetFileAttributes failed.");
      }
    return 0;
    case EW_CREATEDIR:
      process_string_fromtab(buf2,thisentry->offsets[0]);
      log_printf3("CreateDirectory: \"%s\"->\"%s\"",GetStringFromStringTab(thisentry->offsets[0]),buf2);
      update_status_text("Creating directory: ",buf2);
      recursive_create_directory(buf2);
    return 0;
    case EW_SETOUTPUTDIR:
      process_string_fromtab(buf4,thisentry->offsets[0]);
      log_printf3("SetOutPath: \"%s\"->\"%s\"",GetStringFromStringTab(thisentry->offsets[0]),state_output_directory);
      update_status_text("Output directory: ",buf4);
      lstrcpy(state_output_directory,buf4);
      recursive_create_directory(buf4);
    return 0;
    case EW_IFFILEEXISTS:
      {
        process_string_fromtab(buf,thisentry->offsets[0]);
      	if (file_exists(buf)) 
        {
          log_printf3("IfFileExists: file \"%s\" exists, jumping %d",buf,thisentry->offsets[1]);
          return thisentry->offsets[1];
        }
        log_printf3("IfFileExists: file \"%s\" does not exist, jumping %d",buf,thisentry->offsets[2]);
      }
    return thisentry->offsets[2];
    case EW_EXTRACTFILE:
      {
        HANDLE hOut;
        int ret;
        int overwriteflag=thisentry->offsets[0];
        char *p;
        lstrcpy(buf,state_output_directory);
        if (buf[lstrlen(buf)-1]!='\\') lstrcat(buf,"\\");

        p=GetStringFromStringTab(thisentry->offsets[1]);
        log_printf3("File: overwriteflag=%d, name=\"%s\"",overwriteflag,p);
        lstrcat(buf,p);
      _tryagain:
        if (overwriteflag==0)
        {
          int attr=GetFileAttributes(buf);
          if (attr & FILE_ATTRIBUTE_READONLY)
            SetFileAttributes(buf,attr^FILE_ATTRIBUTE_READONLY);
        }
        if (overwriteflag == 3) // check date and time
        {
          overwriteflag=1; // if it doesn't exist, fall back to no overwrites (since it shouldn't matter anyway)
          hOut=CreateFile(buf,GENERIC_READ,0,NULL,OPEN_EXISTING,0,NULL);
          if (hOut != INVALID_HANDLE_VALUE)
          {
            FILETIME ft;
            if (GetFileTime(hOut,NULL,NULL,&ft))
            {
              FILETIME ft2;
              ft2.dwHighDateTime=thisentry->offsets[3];
              ft2.dwLowDateTime=thisentry->offsets[4];
              overwriteflag=(CompareFileTime(&ft,&ft2) >= 0);  // if first one is newer, then don't overwrite
            }
            CloseHandle(hOut);              
          }
        }
        hOut=CreateFile(buf,GENERIC_WRITE,0,NULL,(overwriteflag==1)?CREATE_NEW:CREATE_ALWAYS,0,NULL);
        if (hOut == INVALID_HANDLE_VALUE)
        {
          if (overwriteflag) 
          {
            update_status_text("Skipped: ",p);
            if (overwriteflag==2) exec_errorflag++;
            log_printf3("File: skipping: \"%s\" (overwriteflag=%d)",buf,overwriteflag); 
            return 0;
          }
          log_printf2("File: error creating \"%s\"",buf); 
          lstrcpy(buf2,"Error opening file for writing:\r\n  \"");
          lstrcat(buf2,buf);
          lstrcat(buf2,"\"\r\n"
                        "Hit abort to abort installation,\r\n"
                        "retry to retry writing the file, or\r\n"
                        "ignore to skip this file");
          
          switch (MessageBox(g_hwnd,buf2,g_caption,MB_ABORTRETRYIGNORE|MB_ICONSTOP))
          {
            case IDRETRY:
              log_printf("File: error, user retry"); 
              goto _tryagain;
            case IDIGNORE:
              log_printf("File: error, user cancel"); 
              exec_errorflag++;
              return 0;
            default:
              log_printf("File: error, user abort"); 
              update_status_text("Aborted when couldn't write file: ",buf);
            return EXEC_ERROR;
          }
        }

        update_status_text("Extract: ",p);
        ret=GetCompressedDataFromDataBlock(thisentry->offsets[2],hOut);

        log_printf3("File: wrote %d to \"%s\"",ret,buf);

        if (thisentry->offsets[3] != 0xffffffff || thisentry->offsets[4] != 0xffffffff)
        {
          FILETIME ft;
          ft.dwHighDateTime=thisentry->offsets[3];
          ft.dwLowDateTime=thisentry->offsets[4];
          SetFileTime(hOut,&ft,NULL,&ft);
        }

        CloseHandle(hOut);

        if (ret < 0)
        {
          if (ret == -2)
          {
            lstrcpy(buf,"Extract: error writing to file ");
            lstrcat(buf,p);
          }
          else
          {
            lstrcpy(buf,g_errdecomp);
          }
          log_printf2("%s",buf);
          MessageBox(g_hwnd,buf,g_caption,MB_OK|MB_ICONSTOP);
          return EXEC_ERROR;
        }
      }
    return 0;
    case EW_SHELLEXEC: // this uses improvements of Andras Varga
      process_string_fromtab(buf,thisentry->offsets[0]);
      process_string_fromtab(buf2,thisentry->offsets[1]);
      process_string_fromtab(buf3,thisentry->offsets[2]);
      wsprintf(buf4, "%s %s", buf, buf2);
      update_status_text("ExecShell: ", buf4);
      x=(int)ShellExecute(g_hwnd,buf[0]?buf:NULL,buf2,buf3[0]?buf3:NULL,state_output_directory,thisentry->offsets[3]);
      if (x > 32)
      {
        log_printf4("ExecShell: success (\"%s\": file:\"%s\" params:\"%s\")",buf,buf2,buf3);
      }
      else
      {
        log_printf5("ExecShell: warning: error (\"%s\": file:\"%s\" params:\"%s\")=%d",buf,buf2,buf3,x);
        exec_errorflag++;
      }
    return 0;
    case EW_EXECUTE:
      {
        PROCESS_INFORMATION ProcInfo={0,};
        STARTUPINFO StartUp={sizeof(STARTUPINFO),};
        exec_errorflag++;
        process_string_fromtab(buf,thisentry->offsets[0]);
        log_printf2("Exec: command=\"%s\"",GetStringFromStringTab(thisentry->offsets[0]));
        update_status_text("Execute: ",buf);
        if (CreateProcess( NULL, buf, NULL, NULL, FALSE, 0, NULL, state_output_directory, &StartUp, &ProcInfo))
        {
          log_printf2("Exec: success (\"%s\")",buf);
          if (NULL != ProcInfo.hThread) CloseHandle( ProcInfo.hThread );
          if (NULL != ProcInfo.hProcess)
          {
            exec_errorflag--;
            if (thisentry->offsets[1]==1) 
            {
              DWORD lExitCode;
              while (WaitForSingleObject(ProcInfo.hProcess,100) == WAIT_TIMEOUT)
              {
                static MSG msg;
                while (PeekMessage(&msg,NULL,WM_PAINT,WM_PAINT,PM_REMOVE))
                  DispatchMessage(&msg);
              }
              GetExitCodeProcess(ProcInfo.hProcess, &lExitCode);
              if( lExitCode != 0 )
              {
                exec_errorflag++;
              }
            }
            CloseHandle( ProcInfo.hProcess );
          }
        }
        else 
        { 
          log_printf2("Exec: failed createprocess (\"%s\")",buf); 
        }
      }
    return 0;
    case EW_COMPAREFILETIMES:
      {
        HANDLE h1, h2;
        FILETIME ft1,ft2;
        int rv=0;
        int a=thisentry->offsets[1]&0x7FFFFFFF;
        ft1.dwLowDateTime=thisentry->offsets[0];
        ft1.dwHighDateTime=thisentry->offsets[4];
        if (a == thisentry->offsets[1]) // high bit not set
        {
          exec_errorflag++;
          process_string_fromtab(buf,thisentry->offsets[0]);
          h1=CreateFile(buf,GENERIC_READ,0,NULL,OPEN_EXISTING,0,NULL);
          if (h1 != INVALID_HANDLE_VALUE)
          {
            if (GetFileTime(h1,NULL,NULL,&ft1)) exec_errorflag--;
            CloseHandle(h1);
          }
        }
        exec_errorflag++;
        process_string_fromtab(buf,a);
        h2=CreateFile(buf,GENERIC_READ,0,NULL,OPEN_EXISTING,0,NULL);
        if (h2 != INVALID_HANDLE_VALUE)
        {
          if (GetFileTime(h2,NULL,NULL,&ft2))
          {
            int a=CompareFileTime(&ft1,&ft2);
            if (a > 0) rv=thisentry->offsets[2];
            if (a < 0) rv=thisentry->offsets[3];
            exec_errorflag--;
          }
          CloseHandle(h2);
        }
        return rv;
      }
    case EW_COMPAREDLLS:
      {
        int rv=0;
        DWORD s1=1,s2;
        DWORD t[4]; // our two members are the 3rd and 4th..
        VS_FIXEDFILEINFO *pvsf1=(VS_FIXEDFILEINFO*)t, *pvsf2;
        DWORD d;
        int a=thisentry->offsets[1]&0x7FFFFFFF;
        pvsf1->dwFileVersionLS=thisentry->offsets[0];
        pvsf1->dwFileVersionMS=thisentry->offsets[4];

        if (a==thisentry->offsets[1]) // high bit not set
        {
          process_string_fromtab(buf,thisentry->offsets[0]);
          s1=GetFileVersionInfoSize(buf,&d);
          pvsf1=NULL;
        }

        process_string_fromtab(buf2,a);
        s2=GetFileVersionInfoSize(buf2,&d);
        exec_errorflag++;
        if (s1 && s2)
        {
          void *b1;
          b1=(void*)GlobalAlloc(GPTR,s1+s2);
          if (b1)
          {
            UINT uLen;
            void *b2=(char*)b1+s1;
            if( (pvsf1 || (GetFileVersionInfo(buf,0,s1,b1) && 
                          VerQueryValue(b1,"\\",&pvsf1,&uLen))) &&
                GetFileVersionInfo(buf2,0,s2,b2) && VerQueryValue(b2,"\\",&pvsf2,&uLen))
            {
              if (pvsf1->dwFileVersionMS > pvsf2->dwFileVersionMS)
                rv=thisentry->offsets[2];
              else if (pvsf1->dwFileVersionMS < pvsf2->dwFileVersionMS)
                rv=thisentry->offsets[3];
              else if (pvsf1->dwFileVersionLS > pvsf2->dwFileVersionLS)
                rv=thisentry->offsets[2];
              else if (pvsf1->dwFileVersionLS < pvsf2->dwFileVersionLS)
                rv=thisentry->offsets[3];
              exec_errorflag--;
            }
            GlobalFree(b1);
          }
        }
        return rv;
      }
    case EW_GETFULLDLLPATH:
      {
        HANDLE h;
        process_string_fromtab(buf,thisentry->offsets[1]);          
        h=LoadLibrary(buf);
        if (h)
        {
          GetModuleFileName(h,g_usrvars[thisentry->offsets[0]],1024);
          FreeLibrary(h);
        }
        else 
        {
          g_usrvars[thisentry->offsets[0]][0]=0;
          exec_errorflag++;
        }
      }
    return 0;
#ifdef NSIS_SUPPORT_ACTIVEXREG
    case EW_REGISTERDLL:
      {
        HRESULT hres=OleInitialize(NULL);
        exec_errorflag++;
        if (hres == S_FALSE || hres == S_OK)
        {
          HANDLE h;
          process_string_fromtab(buf,thisentry->offsets[0]);
          
          h=LoadLibrary(buf);
          if (h)
          {
            FARPROC funke = GetProcAddress(h,GetStringFromStringTab(thisentry->offsets[1]));
            if (funke) 
            {
              exec_errorflag--;
              update_status_text(GetStringFromStringTab(thisentry->offsets[2]),buf);
              funke();
            }
            else if (!thisentry->offsets[1])
            {
              update_status_text("Could not find symbol: ",buf); 
              log_printf4("%s%s not found in %s",g_errdll,GetStringFromStringTab(thisentry->offsets[1]),buf);
            }
            FreeLibrary(h);
          }
          else if (!thisentry->offsets[1])
          {
            update_status_text("Could not load: ",buf); 
            log_printf3("%sCould not load %s",g_errdll,buf);
          }
          OleUninitialize();
        }
        else
        {
          update_status_text("No OLE for: ",buf); 
          log_printf2("%sCould not initialize OLE",g_errdll);;
        }
      }
    return 0;
#endif
#ifdef NSIS_SUPPORT_NETSCAPEPLUGINS
    case EW_INSTNETSCAPE: // install netscape plug-in
      {
        HKEY hKey;
		    if ( RegOpenKeyEx(HKEY_LOCAL_MACHINE,"SOFTWARE\\Netscape\\Netscape Navigator",0,KEY_READ,&hKey) == ERROR_SUCCESS)
		    {
          x=0;
          for (;;)
          {
            FILETIME pft;
            HKEY subKey;
            DWORD lname=sizeof(buf3);
            if (RegEnumKeyEx(hKey,x++,buf3,&lname,NULL,NULL,NULL,&pft) != ERROR_SUCCESS) break;
            lstrcat(buf3,"\\Main");
            if (RegOpenKeyEx(hKey,buf3,0,KEY_READ,&subKey) == ERROR_SUCCESS)
            {
			        int l = sizeof(buf);
			        int t=REG_SZ;
			        if (RegQueryValueEx(subKey,"Plugins Directory",NULL,&t,buf,&l ) == ERROR_SUCCESS && t == REG_SZ)
			        {
				        lstrcat(buf,"\\");
                lstrcat(buf,GetStringFromStringTab(thisentry->offsets[0]));
                {
                  const char *nserrstr="Error accessing Netscape plug-in.\r\nMake sure all windows of Netscape are closed.\r\nHit Retry to try again, Cancel to skip";
                  HANDLE hOut=INVALID_HANDLE_VALUE;
                  retryagainns:
                  {
                    int attr=GetFileAttributes(buf);
                    if (attr & FILE_ATTRIBUTE_READONLY)
                      SetFileAttributes(buf,attr^FILE_ATTRIBUTE_READONLY);
                  }
                  
                  if (thisentry->offsets[1]) // uninstall
                  {
                    hOut=CreateFile(buf,0,0,NULL,OPEN_EXISTING,0,NULL);                  
                    if (hOut != INVALID_HANDLE_VALUE)
                    {
                      CloseHandle(hOut);
                      if (!DeleteFile(buf))
                      {
                        log_printf2("InstNSPlug: error removing: %s",buf);
                        hOut=INVALID_HANDLE_VALUE;
                        if (MessageBox(g_hwnd,nserrstr,g_caption,MB_RETRYCANCEL|MB_APPLMODAL|MB_TOPMOST)==IDOK) goto retryagainns;
                        log_printf2("InstNSPlug: uninstall from %s aborted by user",buf);
                        exec_errorflag++;
                      }
                      else
                        log_printf2("InstNSPlug: removed: %s",buf);
                    }
                  }
                  else
                  {
                    hOut=CreateFile(buf,GENERIC_WRITE,0,NULL,CREATE_ALWAYS,0,NULL);                  
                    if (hOut == INVALID_HANDLE_VALUE)
                    {
                      if (MessageBox(g_hwnd,nserrstr,g_caption,MB_RETRYCANCEL|MB_APPLMODAL|MB_TOPMOST)==IDOK) goto retryagainns;
                      log_printf2("InstNSPlug: install to %s aborted by user",buf);
                      exec_errorflag++;
                    }

                    if (hOut != INVALID_HANDLE_VALUE && !thisentry->offsets[1])
                    {
                      int ret=GetCompressedDataFromDataBlock(thisentry->offsets[2],hOut);

                      if (thisentry->offsets[3] != 0xffffffff && thisentry->offsets[4] != 0xffffffff)
                      {
                        FILETIME ft;
                        ft.dwHighDateTime=thisentry->offsets[3];
                        ft.dwLowDateTime=thisentry->offsets[4];
                        SetFileTime(hOut,&ft,NULL,&ft);
                      }

                      CloseHandle(hOut);

                      if (ret<0)
                      {
                        DeleteFile(buf);
                        if (ret == -2)
                        {
                          lstrcpy(buf2,"Extract: error writing to file ");
                          lstrcat(buf2,buf);
                        }
                        else
                        {
                          lstrcpy(buf2,g_errdecomp);
                        }
                        RegCloseKey(subKey);
              			    RegCloseKey(hKey);
                        log_printf2("%s",buf2);
                        MessageBox(g_hwnd,buf2,g_caption,MB_OK|MB_ICONSTOP);
                        return EXEC_ERROR;
                      }
                      update_status_text("Installed Netscape plug-in: ",buf);
                      log_printf2("InstNSPlug: wrote: %s",buf);
                    }
                  } // install
                }
			        }
              RegCloseKey(subKey);
            }
          }
			    RegCloseKey(hKey);
        }
        else
        {
          log_printf("InstNSPlug: Netscape registry settings not found");
        }
      }
    return 0;
#endif
    case EW_DELREG:
      {
        char *subkey=GetStringFromStringTab(thisentry->offsets[1]);
        int rootkey=thisentry->offsets[0];
        exec_errorflag++;
        if (thisentry->offsets[2] != -1)
        {
          HKEY hKey;
          if (RegOpenKey((HKEY)rootkey,subkey,&hKey) == ERROR_SUCCESS) 
          {
            process_string_fromtab(buf,thisentry->offsets[2]);
            log_printf4("DeleteRegValue: %d\\%s\\%s",rootkey,subkey,buf);
            if (RegDeleteValue(hKey,buf) == ERROR_SUCCESS) exec_errorflag--;
            RegCloseKey(hKey);
          }
        }
        else
        {
          log_printf3("DeleteRegKey: %d\\%s",rootkey,subkey);
          if (myRegDeleteKeyEx((HKEY)rootkey,subkey) == ERROR_SUCCESS) exec_errorflag--;
        }
      }
    return 0;
    case EW_WRITEREG: // write registry value
      {
        HKEY hKey;
        int rootkey=thisentry->offsets[0];
        int type=thisentry->offsets[4];
        char *p;
        exec_errorflag++; 
        process_string_fromtab(buf2,thisentry->offsets[2]);
        p=GetStringFromStringTab(thisentry->offsets[1]);
        if (RegCreateKey((HKEY)rootkey,p,&hKey) == ERROR_SUCCESS) 
        {
          if (type == 1)
          {
            process_string_fromtab(buf3,thisentry->offsets[3]);
            if (RegSetValueEx(hKey,buf2,0,REG_SZ,buf3,lstrlen(buf3)+1) == ERROR_SUCCESS) exec_errorflag--;
            log_printf5("WriteRegStr: set %d\\%s\\%s to %s",rootkey,p,buf2,buf3);
          }
          else if (type == 2)
          {
            if (RegSetValueEx(hKey,buf2,0,REG_DWORD,(unsigned char*)&thisentry->offsets[3],4) == ERROR_SUCCESS) exec_errorflag--;
            log_printf5("WriteRegDword: set %d\\%s\\%s to %d",rootkey,p,buf2,thisentry->offsets[3]);
          }
          else if (type == 3)
          {
            int len=GetCompressedDataFromDataBlockToMemory(thisentry->offsets[3], buf4, 1024);
            if (len >= 0)
            {
              if (RegSetValueEx(hKey,buf2,0,REG_BINARY,buf4,len) == ERROR_SUCCESS) exec_errorflag--;
            }
            log_printf5("WriteRegBin: set %d\\%s\\%s with %d bytes",rootkey,p,buf2,len);

          }
          RegCloseKey(hKey);
        }
        else { log_printf3("WriteReg: error creating key %d\\%s",rootkey,p); }
      }
    return 0;
    case EW_WRITEINI:
      {
        char *sec, *ent;
        sec=ent=0;
        lstrcpy(buf2,"<RM>");
        lstrcpy(buf3,buf2);
        process_string_fromtab(buf,thisentry->offsets[0]);
        if (thisentry->offsets[1]>=0) 
        { 
          process_string_fromtab(buf2,thisentry->offsets[1]); 
          sec=buf2; 
        }
        if (thisentry->offsets[2]>=0) 
        { 
          process_string_fromtab(buf3,thisentry->offsets[2]); 
          ent=buf3; 
        }
        process_string_fromtab(buf4,thisentry->offsets[3]); 
        log_printf5("WriteINIStr: wrote [%s] %s=%s in %s",buf,buf2,buf3,buf4);
        if (!WritePrivateProfileString(buf,sec,ent,buf4)) exec_errorflag++;
      }
    return 0;
    case EW_CREATESHORTCUT:
      process_string_fromtab(buf3,thisentry->offsets[0]);
      process_string_fromtab(buf2,thisentry->offsets[1]);
      process_string_fromtab(buf, thisentry->offsets[2]);
      process_string_fromtab(buf4,thisentry->offsets[3]);

      log_printf8("CreateShortCut: out: \"%s\", in: \"%s %s\", icon: %s,%d, sw=%d, hk=%d",
          buf3,buf2,buf,buf4,thisentry->offsets[4],thisentry->offsets[5]&0xffff,thisentry->offsets[5]>>16); 

      if (CreateShortCut(g_hwnd, buf3, buf4[0]?buf4:NULL, thisentry->offsets[4]&0xff, buf2, buf[0]?buf:NULL,
          state_output_directory,(thisentry->offsets[4]&0xff00)>>8,thisentry->offsets[4]>>16))
      {
        exec_errorflag++;
        update_status_text("Error creating shortcut: ",buf3);
      }
      else
      {
        update_status_text("Created shortcut: ",buf3);
      }
    return 0;
    case EW_DELETEFILE:
      log_printf2("Delete: \"%s\"",buf); 
      {
		    HANDLE h;
		    WIN32_FIND_DATA fd;
        char *p=buf;
        process_string_fromtab(buf2,thisentry->offsets[0]);
        lstrcpy(buf,buf2);
        while (*p) p++;
        while (p > buf && *p != '\\') p--;
        *p=0;
    		h=FindFirstFile(buf2,&fd);
		    if (h != INVALID_HANDLE_VALUE)
		    {
          do
          {
			      if (!(fd.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)) 
            {
              wsprintf(buf2,"%s\\%s",buf,fd.cFileName);
              if (fd.dwFileAttributes & FILE_ATTRIBUTE_READONLY) 
                SetFileAttributes(buf2,fd.dwFileAttributes^FILE_ATTRIBUTE_READONLY);
              if (DeleteFile(buf2))
              {
                log_printf2("Delete: DeleteFile(\"%s\")",buf2); 
                update_status_text(g_deletefilecolon,buf2);
              }
              else
              {
                if (thisentry->offsets[1])
                {
                  log_printf2("Delete: DeleteFile on Reboot(\"%s\")",buf2); 
                  update_status_text("Delete on reboot: ",buf2);
                  MoveFileOnReboot(buf2,NULL);
                }
                else
                {
                  exec_errorflag++;
                }
              }
            }
          } while (FindNextFile(h,&fd));
			    FindClose(h);
    		}
      }
    return 0;
    case EW_FINDWINDOW:
      {
        int whattodo=thisentry->offsets[0];
        char *wndclass=GetStringFromStringTab(thisentry->offsets[1]);
        char *wndtitle=NULL;
        char *mytext=wndclass;
        log_printf3("FindWindow: checking for window class: %s . whattodo=%d",wndclass,whattodo); 
        if (thisentry->offsets[3])
        {
          wndtitle=wndclass;
          wndclass=NULL;
        }
        if (whattodo==2)
        {
          process_string_fromtab(buf4,thisentry->offsets[2]);
          while (FindWindow(wndclass,wndtitle))
          {
            int r=MessageBox(g_hwnd,buf4,g_caption,MB_ABORTRETRYIGNORE);
            if (r == IDABORT) 
            {
              static char *t="FindWindow: user abort";
              log_printf(t); 
              update_status_text(t,"");
              return EXEC_ERROR;
            }
            if (r == IDIGNORE) 
            {
              log_printf("FindWindow: user ignore"); 
              break;
            }
          }
        }
        else if (whattodo==1)
        {
          HWND hwnd=FindWindow(wndclass,wndtitle);
          if (hwnd)
          {
            log_printf2("FindWindow: closing window (%s) (one-shot)",mytext); 
            SendMessage(hwnd,WM_CLOSE,0,0);
          }
        }
        else if (whattodo==0)
        {
          HWND hwnd;
          int r=8;
          while ((hwnd=FindWindow(wndclass,wndtitle)))
          {
            SendMessage(hwnd,WM_CLOSE,0,0);
            Sleep(250);
            if (r--<0) 
            {
              break;
            }
          }
          if (!hwnd)
          {
            log_printf3("FindWindow: closed window (%s) (multi-try). %d",mytext,r); 
          }
          else
          {
            log_printf2("FindWindow: gave up closing window (%s)",mytext); 
          }
        }
        else if (whattodo==3)
        {
          if (FindWindow(wndclass,wndtitle)) return thisentry->offsets[4];
        }
      }
    return 0;
    case EW_MESSAGEBOX: // MessageBox      
      {
        int v;
        process_string_fromtab(buf4,thisentry->offsets[1]);
        log_printf3("MessageBox: %d,\"%s\"",thisentry->offsets[0],buf4); 
        v=MessageBox(g_hwnd,buf4,g_caption,thisentry->offsets[0]);
        if (v && v==thisentry->offsets[2])
        {
          return thisentry->offsets[3];
        }
      }
    return 0;
    case EW_RMDIR:
      {
        char *p;
        log_printf2("RMDir: \"%s\"",GetStringFromStringTab(thisentry->offsets[0])); 
        process_string_fromtab(buf,thisentry->offsets[0]);
        p=buf + lstrlen(buf)-1;
        if (*p=='\\') *p=0;

        doRMDir(buf,thisentry->offsets[1]);
        if (file_exists(buf)) exec_errorflag++;
      }
    return 0;
    case EW_COPYFILES: // CopyFile (added by NOP)
      {
        int res;
		    SHFILEOPSTRUCT op;
        process_string_fromtab(buf,thisentry->offsets[0]);
        process_string_fromtab(buf2,thisentry->offsets[1]);
			  log_printf3("CopyFiles \"%s\"->\"%s\"",buf,buf2);
			  op.hwnd=g_hwnd;
			  op.wFunc=FO_COPY;
			  buf[lstrlen(buf)+1]=0;
			  buf2[lstrlen(buf2)+1]=0;
			  op.pFrom=buf;
			  op.pTo=buf2;
			  op.fFlags=FOF_NOCONFIRMATION|FOF_NOCONFIRMMKDIR;
        op.fAnyOperationsAborted=FALSE;
			  res=SHFileOperation(&op);
        update_status_text("Copying files: ",buf);
			  if (op.fAnyOperationsAborted || res) 
        {
				  if (op.fAnyOperationsAborted) MessageBox(g_hwnd,"Aborted by user",g_caption,MB_OK|MB_ICONSTOP);
				  else MessageBox(g_hwnd,"Unable to copy files.",g_caption,MB_OK|MB_ICONSTOP);
				  return EXEC_ERROR;
			  }
    	}
		return 0;
    case EW_IFERRORS:
      {
        int f=exec_errorflag;
        exec_errorflag=thisentry->offsets[2];
        if (f)
        {
          return thisentry->offsets[0];
        }
      }
    return thisentry->offsets[1];
    case EW_ASSIGNVAR:
      process_string_fromtab(buf4,thisentry->offsets[1]);
      if (thisentry->offsets[2]>0 && thisentry->offsets[2] < 1024)
        buf4[thisentry->offsets[2]]=0;
      lstrcpy(g_usrvars[thisentry->offsets[0]],buf4);        
    return 0;
    case EW_READREGSTR: // read registry string
      {
        HKEY hKey;
        char *p=g_usrvars[thisentry->offsets[0]];
        int rootkey=thisentry->offsets[1];
        process_string_fromtab(buf,thisentry->offsets[2]); // buf == subkey
        process_string_fromtab(buf2,thisentry->offsets[3]); // buf == key name
        p[0]=0;
 		    if ( RegOpenKeyEx((HKEY)rootkey,buf,0,KEY_READ,&hKey) == ERROR_SUCCESS)
        {
			    int l = 1024;
			    int t=REG_SZ;
			    if (RegQueryValueEx(hKey,buf2,NULL,&t,p,&l ) != ERROR_SUCCESS || t != REG_SZ)
            exec_errorflag++;
          RegCloseKey(hKey);
        }
        else exec_errorflag++;
     }
    return 0;
    case EW_READINISTR:
      {
        static const char *errstr="!nsiser";
        char *p=g_usrvars[thisentry->offsets[0]];
        process_string_fromtab(buf,thisentry->offsets[1]);
        process_string_fromtab(buf2,thisentry->offsets[2]);
        process_string_fromtab(buf3,thisentry->offsets[3]);
        GetPrivateProfileString(buf,buf2,errstr,p,1023,buf3);
        if (*((int*)errstr) == *((int*)p) && *(((int*)errstr)+1) == *(((int*)p)+1))
        {
          exec_errorflag++;
          p[0]=0;
        }
      }
    return 0;
    case EW_STRCMP:
      process_string_fromtab(buf3,thisentry->offsets[0]);
      process_string_fromtab(buf4,thisentry->offsets[1]);
      if (!lstrcmpi(buf3,buf4)) return thisentry->offsets[2];
    return thisentry->offsets[3];
  }
  MessageBox(g_hwnd,"Install corrupted: invalid opcode",g_caption,MB_OK|MB_ICONSTOP);
  return EXEC_ERROR;
}