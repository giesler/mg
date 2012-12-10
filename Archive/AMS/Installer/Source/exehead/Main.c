const char *verstr=""; // MPG change "Nullsoft Install System v1.44";

/* 
  Nullsoft "SuperPimp" Installation System - main.c - executable header main code
  version 1.44  - June 11, 2001

  Copyright (C) 1999-2001 Nullsoft, Inc.

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  This source distribution includes portions of zlib. see zlib/zlib.h for 
  its license and so forth. Note that this license is also borrowed from zlib.
*/


#include <windows.h>
#include "resource.h"
#include "util.h"
#include "fileform.h"
#include "state.h"
#include "ui.h"
#include "libc.h"
#include "../zlib/zlib.h"

char *g_readerrorstr="Error reading data from installer";
char *g_errorcopyinginstall="Error launching uninstaller";

char state_exe_directory[1024];
char g_caption[256];
int g_filehdrsize;
HWND g_hwnd;

static int m_length;
static int m_pos;

static BOOL CALLBACK verProc(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
  static char *t="verifying installer: %d%%";
  if (uMsg == WM_INITDIALOG) 
  {
    SetTimer(hwndDlg,1,250,NULL);
    uMsg = WM_TIMER;
  }
  if (uMsg == WM_TIMER)
  {
    static char bt[64];
    wsprintf(bt,t,m_pos/(m_length/100));

    SetWindowText(hwndDlg,bt);
    SetDlgItemText(hwndDlg,IDC_STR,bt);
  }
  return 0;
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInst,LPSTR lpszCmdParam, int nCmdShow)
{
  static HWND hwnd;
  unsigned int verify_time=GetTickCount()+1000;
  static char temp[1024];
  int left;
  static char no_crc;
  static char do_crc;
  static char silent;
  static int ret;
  static char *m_Err;
  static int crc;
  char *cmdline=GetCommandLine();
  if (*cmdline == '\"')
  {
    do cmdline++; while (*cmdline && *cmdline != '\"');
    if (*cmdline) cmdline++;
  }
  else
  {
    while (*cmdline && *cmdline != ' ') cmdline++;
  }

  do
  {
    static char v[4]={'N','C','R','C'};
    while (*cmdline == ' ') cmdline++;
    if (cmdline[0] != '/') break;
    cmdline++;
    if (cmdline[0] == 'S' && (cmdline[1] == ' ' || !cmdline[1]))
    {
      silent++;
      cmdline++;
    }
    else if (*(int*)cmdline == *(int*)v && (cmdline[4] == ' ' || !cmdline[4]))
    {
      no_crc++;
      cmdline+=4;
    }
    else if (cmdline[0] == 'D' && cmdline[1] == '=' && cmdline[2])
    {
      cmdline+=2;
      lstrcpy(state_install_directory,cmdline);
      cmdline+=lstrlen(cmdline);
    }
    else break;
  }
  while (*cmdline);

  lstrcpy(g_caption,(char*)verstr);
  lstrcat(g_caption," ERROR");

  g_hInstance=GetModuleHandle(NULL);
  GetModuleFileName(g_hInstance,temp,sizeof(temp));

  {
    char *p=state_exe_directory;
    lstrcpy(state_exe_directory,temp);
    while (*p) p++;
    while (p > state_exe_directory && *p != '\\') p--;
    *p=0;
  }

  g_db_hFile=CreateFile(temp,GENERIC_READ,FILE_SHARE_READ,NULL,OPEN_EXISTING,0,NULL);
  if (g_db_hFile==INVALID_HANDLE_VALUE)
  {
    m_Err = "Can't open self";
    goto end;
  }

  left = m_length = GetFileSize(g_db_hFile,NULL);
  while (left > 0)
  {
    DWORD l=left;
    if (l > 512) l=512;
    if (!ReadFile(g_db_hFile,temp,l,&l,NULL))
    {
      m_Err=g_readerrorstr;
      if (hwnd) DestroyWindow(hwnd);
      goto end;
    }
    crc=adler32(crc, temp, l);

    if (!g_filehdrsize)
    {
      int dbl;
      dbl=isheader((firstheader*)temp);
      if (dbl)
      {
        int a=*(int*)temp;
        g_filehdrsize=m_pos;
        if (dbl > left)
        {
          m_Err="Installer too small";
          goto end;
        }

        if (a&FH_FLAGS_SILENT) silent++;

        if (no_crc || !(a&FH_FLAGS_CRC)) break; // if first bit is not set, then no crc checking. 

        // end crc checking at crc :) this means you can tack shit on the end 
        // and it'll still work.
        left=dbl-4;
        do_crc++;
      }
    }
    else if (!silent)
    {
      if (hwnd)
      {
        static MSG msg;
        if (PeekMessage(&msg,NULL,0,0,PM_REMOVE)) DispatchMessage(&msg);
      }
      else if (GetTickCount() > verify_time) 
        hwnd=CreateDialog(g_hInstance,MAKEINTRESOURCE(IDD_VERIFY),NULL,verProc);
    }
    m_pos+=l;
    left -= l;
  }
  if (hwnd) DestroyWindow(hwnd);
  if (g_filehdrsize)
  {
    DWORD l;
    int fcrc;
    if (do_crc && (!ReadFile(g_db_hFile,&fcrc,4,&l,NULL) || crc != fcrc))
    {
      m_Err="Installer CRC invalid";
      goto end;
    }
    SetFilePointer(g_db_hFile,g_filehdrsize,NULL,FILE_BEGIN);    
    g_db_offset=g_filehdrsize+sizeof(firstheader);

    if (loadHeaders()) m_Err=g_readerrorstr;
  }
  if (m_Err) goto end;

#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
  if (g_inst_uninstheader)
  {
    if (cmdline[0] == '_' && cmdline[1] == '=' && cmdline[2])
    {
      cmdline+=2;
      if (is_valid_instpath(cmdline))
      {
        lstrcpy(state_install_directory,cmdline);
        lstrcpy(state_output_directory,cmdline);
      }
      else
      {
        m_Err = g_errorcopyinginstall;
        goto end;
      }
    }
    else
    {
      int x,done=0;

      for (x = 0; x < 26; x ++)
      {
        static char s[]="A~NSISu_.exe";
        static char buf2[1024];
        static char ibuf[1024];
      
        buf2[0]='\"';
        GetTempPath(sizeof(buf2)-1,buf2+1);
        lstrcat(buf2,s);

        DeleteFile(buf2+1); // clean up after all the other ones if they are there
        
        if (!done)
        {
          // get current name
          int l=GetModuleFileName(g_hInstance,ibuf,sizeof(ibuf));
          // check if it is ?~NSISu_.exe - if so, fuck it
          if (!lstrcmpi(ibuf+l-(sizeof(s)-2),s+1)) break;

          // copy file
          if (CopyFile(ibuf,buf2+1,FALSE))
          {
            PROCESS_INFORMATION ProcInfo={0,};
            STARTUPINFO StartUp={sizeof(STARTUPINFO),};
            char *p=ibuf;
            MoveFileOnReboot(buf2+1,NULL);
            while (*p) p++;
            while (p > ibuf && *p != '\\') p--;
            *p=0;
            if (!is_valid_instpath(ibuf)) break;
            done++;
            lstrcat(buf2,"\" _=");
            lstrcat(buf2,ibuf);
            GetTempPath(sizeof(ibuf),ibuf);
            if (CreateProcess( NULL, buf2, NULL, NULL, FALSE, 0, NULL, ibuf, &StartUp, &ProcInfo) )
            {
              if (ProcInfo.hThread) CloseHandle(ProcInfo.hThread);
              if (ProcInfo.hProcess) CloseHandle(ProcInfo.hProcess);
            }
            else m_Err = g_errorcopyinginstall;
          }
        }
        s[0]++;
      }
      if (!done) m_Err=g_errorcopyinginstall;
      goto end;      
    }
  }
#endif
  if (g_inst_header&&!g_inst_header->silent_install)
    g_inst_header->silent_install=silent;
  ret=ui_doinstall();
  if (g_inst_header) GlobalFree((HGLOBAL)g_inst_header);
#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
  if (g_inst_uninstheader) GlobalFree((HGLOBAL)g_inst_uninstheader);
#endif

#ifdef NSIS_CONFIG_LOG
  log_write(1);
#endif
end:
  if (g_db_hFile!=INVALID_HANDLE_VALUE) CloseHandle(g_db_hFile);
  if (m_Err) MessageBox(NULL,m_Err,g_caption,MB_OK|MB_ICONSTOP);
  ExitProcess(ret);
}

