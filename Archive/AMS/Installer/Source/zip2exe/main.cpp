#include <windows.h>
#include <stdio.h>

extern "C"
{
#include "zlib/unzip.h"
};
#include "resource.h"

HINSTANCE g_hInstance;

static BOOL CALLBACK DlgProc(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam); 


int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInst,
                   LPSTR lpszCmdParam, int nCmdShow)
{
  g_hInstance=hInstance;


  return DialogBox(hInstance,MAKEINTRESOURCE(IDD_DIALOG1),GetDesktopWindow(),DlgProc);
}
char tempzip_path[1024];

static void doRMDir(char *buf)
{
  HANDLE h;
  WIN32_FIND_DATA fd;
  char *p=buf;
  while (*p) p++;
  strcpy(p,"\\*.*");
  h = FindFirstFile(buf,&fd);
  if (h != INVALID_HANDLE_VALUE) 
  {
    do
    {
      if (fd.cFileName[0] != '.' ||
          (fd.cFileName[1] != '.' && fd.cFileName[1]))
      {
        strcpy(p+1,fd.cFileName);
        if (fd.dwFileAttributes & FILE_ATTRIBUTE_READONLY) 
          SetFileAttributes(buf,fd.dwFileAttributes^FILE_ATTRIBUTE_READONLY);
        if (fd.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) doRMDir(buf);
        else 
        {
          DeleteFile(buf);
        }
      }
    } while (FindNextFile(h,&fd));
    FindClose(h);
  }
  p[0]=0; // fix buffer
  RemoveDirectory(buf);
}

static void doMKDir(char *directory)
{
	char *p, *p2;
	char buf[MAX_PATH];
  if (!*directory) return;
	strcpy(buf,directory);
  p=buf; while (*p) p++;
	while (p >= buf && *p != '\\') p--;
	p2 = buf;
	if (p2[1] == ':') p2+=4;
	else if (p2[0] == '\\' && p2[1] == '\\')
	{
		p2+=2;
		while (*p2 && *p2 != '\\') p2++;
		if (*p2) p2++;
		while (*p2 && *p2 != '\\') p2++;
		if (*p2) p2++;
	}
	if (p >= p2)
	{
		*p=0;
		doMKDir(buf);
	}
	CreateDirectory(directory,NULL);
}



void tempzip_cleanup(HWND hwndDlg)
{
  if (tempzip_path[0]) doRMDir(tempzip_path);
  tempzip_path[0]=0;
  SendDlgItemMessage(hwndDlg,IDC_ZIPINFO_FILES,LB_RESETCONTENT,0,0);
  SetDlgItemText(hwndDlg,IDC_ZIPINFO_SUMMARY,"");
  EnableWindow(GetDlgItem(hwndDlg,IDOK),0);
  SetDlgItemText(hwndDlg,IDC_ZIPFILE,"");
  SetDlgItemText(hwndDlg,IDC_OUTFILE,"");
}

int tempzip_make(HWND hwndDlg, char *fn)
{
  char buf[MAX_PATH];
  GetTempPath(MAX_PATH,buf);
  GetTempFileName(buf,"z2e",GetTickCount(),tempzip_path);
  if (!CreateDirectory(tempzip_path,NULL))
  {
    GetTempPath(MAX_PATH,tempzip_path);
    strcat(tempzip_path,"\\nsi");
    if (!CreateDirectory(tempzip_path,NULL))
    {
      tempzip_path[0]=0;
      MessageBox(hwndDlg,"Error creating temporary directory","ZIP2EXE Error",MB_OK|MB_ICONSTOP);
      return 1;
    }
  }
  unzFile f;
  f = unzOpen(fn);
  if (!f || unzGoToFirstFile(f) != UNZ_OK)
  {
    if (f) unzClose(f);
    MessageBox(hwndDlg,"Error opening ZIP file","ZIP2EXE Error",MB_OK|MB_ICONSTOP);
    return 1;
  }
	
  int nf=0, nkb=0;
	do {
		char filename[MAX_PATH];
	  unzGetCurrentFileInfo(f,NULL,filename,sizeof(filename),NULL,0,NULL,0);
    if (filename[0] && 
        filename[strlen(filename)-1] != '\\' && 
        filename[strlen(filename)-1] != '/')
    {
      char *pfn=filename;
      while (*pfn)
      {
        if (*pfn == '/') *pfn='\\';
        pfn++;
      }
      pfn=filename;
      if (pfn[1] == ':' && pfn[2] == '\\') pfn+=3;
      while (*pfn == '\\') pfn++;

      char out_filename[1024];
      strcpy(out_filename,tempzip_path);
      strcat(out_filename,"\\");
      strcat(out_filename,pfn);
      if (strstr(pfn,"\\"))
      {
        char buf[1024];
        strcpy(buf,out_filename);
        char *p=buf+strlen(buf);
        while (p > buf && *p != '\\') p--;
        *p=0;
        if (buf[0]) doMKDir(buf);
      }

  		if (unzOpenCurrentFile(f) == UNZ_OK)
	  	{
        SendDlgItemMessage(hwndDlg,IDC_ZIPINFO_FILES,LB_ADDSTRING,0,(LPARAM)pfn);
			  FILE *fp;
			  int l;
			  fp = fopen(out_filename,"wb");
			  if (fp)
			  {
				  do
				  {
					  char buf[1024];
					  l=unzReadCurrentFile(f,buf,sizeof(buf));
					  if (l > 0) 
            {
              if (fwrite(buf,1,l,fp) != (unsigned int)l)
              {
                unzClose(f);
                fclose(fp);
                MessageBox(hwndDlg,"Error writing output file(s)","ZIP2EXE Error",MB_OK|MB_ICONSTOP);
                return 1;
              }
            }
				  } while (l > 0);
          nkb+=ftell(fp)/1024;
				  fclose(fp);
			  }
        else
        {
          unzClose(f);
          MessageBox(hwndDlg,"Error opening output file(s)","ZIP2EXE Error",MB_OK|MB_ICONSTOP);
          return 1;
        }
        nf++;
			  unzCloseCurrentFile(f);
		  }
      else
      {
        unzClose(f);
        MessageBox(hwndDlg,"Error extracting from ZIP file","ZIP2EXE Error",MB_OK|MB_ICONSTOP);
        return 1;
      }
    }
  } while (unzGoToNextFile(f) == UNZ_OK);
  
  wsprintf(buf,"%d files, %dKB",nf,nkb);
  SetDlgItemText(hwndDlg,IDC_ZIPINFO_SUMMARY,buf);
  unzClose(f);
  return 0;
}

char *gp_winamp = "(WINAMP DIRECTORY)";
char *gp_winamp_plugins = "(WINAMP PLUG-INS DIRECTORY)";
char *gp_winamp_vis = "(WINAMP VIS PLUG-INS DIRECTORY)";
char *gp_winamp_dsp = "(WINAMP DSP PLUG-INS DIRECTORY)";
char *gp_winamp_skins = "(WINAMP SKINS DIRECTORY)";
char *gp_poi = "(PATH OF INSTALLER)";


void makeEXE(HWND hwndDlg)
{
  char buf[2048];
  char nsifilename[MAX_PATH];
  GetTempPath(MAX_PATH,buf);
  GetTempFileName(buf,"zne",0,nsifilename);
  FILE *fp=fopen(nsifilename,"w");
  if (!fp)
  {
    MessageBox(hwndDlg,"Error writing .NSI file","ZIP2EXE Error",MB_OK|MB_ICONSTOP);
    return;
  }
  GetDlgItemText(hwndDlg,IDC_INSTNAME,buf,sizeof(buf));
  fprintf(fp,"Name `%s`\n",buf);
  fprintf(fp,"Caption `%s Self Extractor`\n",buf);
  GetDlgItemText(hwndDlg,IDC_OUTFILE,buf,sizeof(buf));
  fprintf(fp,"OutFile `%s`\n",buf);
  GetDlgItemText(hwndDlg,IDC_INSTPATH,buf,sizeof(buf));
  char *outpath = "$INSTDIR";
  int iswinamp=0;
  char *iswinampmode=NULL;
  if (!strcmp(buf,gp_poi)) strcpy(buf,"$EXEDIR");
  
  if (!strcmp(buf,gp_winamp))
  {
    iswinamp=1;
    fprintf(fp,"Function SetMyOutPath\n"
               "  SetOutPath $INSTDIR\n"
               "FunctionEnd\n");
  }
  if (!strcmp(buf,gp_winamp_plugins))
  {
    iswinamp=1;
    fprintf(fp,"Function SetMyOutPath\n"
               "  SetOutPath $INSTDIR\\Plugins\n"
               "FunctionEnd\n");
  }
  if (!strcmp(buf,gp_winamp_vis))
  {
    iswinamp=1;
    iswinampmode="VisDir";
  }
  if (!strcmp(buf,gp_winamp_dsp))
  {
    iswinamp=1;
    iswinampmode="DSPDir";
  }
  if (!strcmp(buf,gp_winamp_skins))
  {
    iswinamp=1;
    iswinampmode="SkinDir";
  }

  if (iswinamp)
  {
    fprintf(fp,"InstallDir `$PROGRAMFILES\\Winamp`\n");
    fprintf(fp,"InstallDirRegKey HKEY_LOCAL_MACHINE `Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Winamp` `UninstallString`\n");

    fprintf(fp,"Function .onVerifyInstDir\n"
               "  IfFileExists $INSTDIR\\winamp.exe WinampInstalled\n"
               "    Abort\n"
               "  WinampInstalled:\n"
               "FunctionEnd\n");

    if (iswinampmode)
    {
      fprintf(fp,"Function SetMyOutPath\n"
                 "  StrCpy $1 $INSTDIR\\Plugins\n"
                 "  ReadINIStr $9 $INSTDIR\\winamp.ini Winamp %s\n"
                 "  StrCmp $9 '' End\n"
                 "    IfFileExists $9 0 End\n"
                 "      StrCpy $1 $9\n"
                 "  End:\n"
                 "  SetOutPath $1\n"
                 "FunctionEnd\n",iswinampmode);
    }
  }
  else  // set out path to $INSTDIR
  {
    fprintf(fp,"InstallDir `%s`\n",buf);
    fprintf(fp,"Function SetMyOutPath\n"
               "  SetOutPath $INSTDIR\n"
               "FunctionEnd\n");
  }

  GetDlgItemText(hwndDlg,IDC_DESCTEXT,buf,sizeof(buf));
  fprintf(fp,"DirText `%s`\n",buf);

  fprintf(fp,"Section\n");
  fprintf(fp,"Call SetMyOutPath\n");
  fprintf(fp,"File /r `%s\\*.*`\n",tempzip_path);
  fprintf(fp,"SectionEnd\n");
  fclose(fp);

  // execute makensis
  char makensis_path[MAX_PATH];
  PROCESS_INFORMATION ProcInfo={0,};
  STARTUPINFO StartUp={sizeof(STARTUPINFO),};
  {
    char *p=makensis_path;
    GetModuleFileName(g_hInstance,makensis_path,sizeof(makensis_path));
    while (*p) p++;
    while (p >= makensis_path && *p != '\\') p--;
    strcpy(++p,"makensis.exe");
  }
  wsprintf(buf,"\"%s\" /PAUSE \"%s\"",makensis_path,nsifilename);
  if (CreateProcess( NULL, buf, NULL, NULL, FALSE, 0, NULL, tempzip_path, &StartUp, &ProcInfo))
  {
    if (NULL != ProcInfo.hThread) CloseHandle( ProcInfo.hThread );
    if (NULL != ProcInfo.hProcess)
    {
      WaitForSingleObject(ProcInfo.hProcess,INFINITE);
      CloseHandle( ProcInfo.hProcess );
    }
  }
  else
  {
    MessageBox(hwndDlg,"Error opening Makensis","ZIP2EXE Error",MB_OK|MB_ICONSTOP);    
  }

  DeleteFile(nsifilename);
}


BOOL CALLBACK DlgProc(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
   static HICON hIcon;
  switch (uMsg)
  {
    if (uMsg == WM_DESTROY && hIcon) { DeleteObject(hIcon); hIcon=0; }
    case WM_INITDIALOG:
      SendDlgItemMessage(hwndDlg,IDC_INSTPATH,CB_ADDSTRING,0,(LPARAM)gp_poi);
      SendDlgItemMessage(hwndDlg,IDC_INSTPATH,CB_ADDSTRING,0,(LPARAM)"$TEMP");
      SendDlgItemMessage(hwndDlg,IDC_INSTPATH,CB_ADDSTRING,0,(LPARAM)"$SYSDIR");
      SendDlgItemMessage(hwndDlg,IDC_INSTPATH,CB_ADDSTRING,0,(LPARAM)"$WINDIR");
      SendDlgItemMessage(hwndDlg,IDC_INSTPATH,CB_ADDSTRING,0,(LPARAM)"$DESKTOP");
      SendDlgItemMessage(hwndDlg,IDC_INSTPATH,CB_ADDSTRING,0,(LPARAM)"$DESKTOP\\Poop");
      SendDlgItemMessage(hwndDlg,IDC_INSTPATH,CB_ADDSTRING,0,(LPARAM)"$PROGRAMFILES\\Poop");
      SendDlgItemMessage(hwndDlg,IDC_INSTPATH,CB_ADDSTRING,0,(LPARAM)"$STARTMENU");
      SendDlgItemMessage(hwndDlg,IDC_INSTPATH,CB_ADDSTRING,0,(LPARAM)"$SMPROGRAMS");

      SendDlgItemMessage(hwndDlg,IDC_INSTPATH,CB_ADDSTRING,0,(LPARAM)gp_winamp);
      SendDlgItemMessage(hwndDlg,IDC_INSTPATH,CB_ADDSTRING,0,(LPARAM)gp_winamp_plugins);
      SendDlgItemMessage(hwndDlg,IDC_INSTPATH,CB_ADDSTRING,0,(LPARAM)gp_winamp_vis);
      SendDlgItemMessage(hwndDlg,IDC_INSTPATH,CB_ADDSTRING,0,(LPARAM)gp_winamp_dsp);
      SendDlgItemMessage(hwndDlg,IDC_INSTPATH,CB_ADDSTRING,0,(LPARAM)gp_winamp_skins);
     
      SetDlgItemText(hwndDlg,IDC_INSTPATH,gp_poi);
      SetDlgItemText(hwndDlg,IDC_DESCTEXT,"Select the folder where you would like to extract the files to:");
		  hIcon=LoadIcon(g_hInstance,MAKEINTRESOURCE(IDI_ICON1));
		  SetClassLong(hwndDlg,GCL_HICON,(long)hIcon);

    return 1;
    case WM_CLOSE:
      tempzip_cleanup(hwndDlg);
      EndDialog(hwndDlg,1);
    break;
    case WM_COMMAND:
      switch (LOWORD(wParam))
      {
        case IDC_BROWSE:
          {
            OPENFILENAME l={sizeof(l),};
            char buf[1024];
            l.hwndOwner = hwndDlg;
            l.lpstrFilter = "ZIP files\0*.zip\0All files\0*.*\0";
            l.lpstrFile = buf;
            l.nMaxFile = 1023;
            l.lpstrTitle = "Open ZIP file";
            l.lpstrDefExt = "zip";
            l.lpstrInitialDir = NULL;
            l.Flags = OFN_HIDEREADONLY|OFN_EXPLORER;  	        
            buf[0]=0;
            if (GetOpenFileName(&l)) 
            {
              tempzip_cleanup(hwndDlg);
              if (tempzip_make(hwndDlg,buf)) tempzip_cleanup(hwndDlg);             
              else
              {
                EnableWindow(GetDlgItem(hwndDlg,IDOK),1);
                SetDlgItemText(hwndDlg,IDC_ZIPFILE,buf);
                char *t=buf+strlen(buf);
                while (t > buf && *t != '\\' && *t != '.') t--;
                {
                  char *p=t;
                  while (p >= buf && *p != '\\') p--;
                  p++;
                  *t=0;
                  SetDlgItemText(hwndDlg,IDC_INSTNAME,p[0]?p:"Stuff");
                }
                strcpy(t,".exe");
                SetDlgItemText(hwndDlg,IDC_OUTFILE,buf);
              }
            }
          }          
        break;
        case IDC_BROWSE2:
          {
            OPENFILENAME l={sizeof(l),};
            char buf[1024];
            l.hwndOwner = hwndDlg;
            l.lpstrFilter = "EXE files\0*.exe\0All files\0*.*\0";
            l.lpstrFile = buf;
            l.nMaxFile = 1023;
            l.lpstrTitle = "Select output EXE file";
            l.lpstrDefExt = "exe";
            l.lpstrInitialDir = NULL;
            l.Flags = OFN_HIDEREADONLY|OFN_EXPLORER;  	        
            GetDlgItemText(hwndDlg,IDC_OUTFILE,buf,sizeof(buf));
            if (GetSaveFileName(&l)) 
            {
              SetDlgItemText(hwndDlg,IDC_OUTFILE,buf);
            }
          }   
        break;
        case IDOK:
          makeEXE(hwndDlg);
          tempzip_cleanup(hwndDlg);
          EndDialog(hwndDlg,0);
        break;
      }
    break;
  }
  return 0;
}