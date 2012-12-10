#include <windows.h>
#include <shlobj.h>
#include "resource.h"

#include "fileform.h"
#include "state.h"
#include "util.h"
#include "ui.h"
#include "libc.h"
#include "exec.h"

#define LB_ICONWIDTH 20
#define LB_ICONHEIGHT 20

extern const char *verstr;
char g_autoclose;

static char g_tmp[4096];

// sent to the last child window to tell it that the install thread is done
#define WM_NOTIFY_INSTPROC_DONE (WM_USER+0x4)

// sent to the outer window to tell it to go to the next inner window
#define WM_NOTIFY_OUTER_NEXT (WM_USER+0x8)

// update message used by DirProc and SelProc for space display
#define WM_IN_UPDATEMSG (WM_USER+0xf)

static BOOL CALLBACK DialogProc(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam); 
static BOOL CALLBACK DialogProcUninstall(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam); 
static int CALLBACK WINAPI BrowseCallbackProc( HWND hwnd, UINT uMsg, LPARAM lParam, LPARAM lpData);
static BOOL CALLBACK LicenseProc(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam); 
static BOOL CALLBACK DirProc(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam); 
static BOOL CALLBACK SelProc(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam); 
static BOOL CALLBACK InstProc(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam); 
static BOOL CALLBACK UninstProc(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam); 
static DWORD WINAPI install_thread(LPVOID p);


HWND bgWnd_Init(HINSTANCE hInstance, char *title, int color1, int color2, int);

int g_show_details;
HWND insthwnd, insthwnd2,insthwndbutton;
static char *g_space_requiredstr="Space required: ";

header *g_inst_header;
section *g_inst_section;
entry *g_inst_entry;
#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
uninstall_header *g_inst_uninstheader;
#endif

static int m_page,m_abort;
static HWND m_curwnd;
static int m_whichcfg;

#ifdef NSIS_CONFIG_LOG
static void build_g_logfile()
{
  char *p=g_log_file;
  lstrcpy(g_log_file,state_install_directory);
  if (g_log_file[0])
  {
    while (*p) p++;
    if (p[-1] != '\\') *p++='\\';
  }
  lstrcpy(p,"install.log");
}
#endif


int ui_doinstall(void)
{
#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
  if (g_inst_uninstheader)
  {
    HWND h=GetDesktopWindow();
    wsprintf(g_caption,"%s Uninstall",GetStringFromStringTab(g_inst_uninstheader->name_ptr));
#ifdef NSIS_SUPPORT_BGBG
    if (g_inst_uninstheader->bg_color1 != -1)
    {
      h=bgWnd_Init(g_hInstance,g_caption,g_inst_uninstheader->bg_color1,g_inst_uninstheader->bg_color2,g_inst_uninstheader->bg_textcolor);
    }
#endif
    return DialogBox(g_hInstance,MAKEINTRESOURCE(IDD_INST),h,DialogProcUninstall);
  }
#endif
  g_autoclose=g_inst_header->auto_close;
  {
    static char buf[1024];
    int st=0;

    if (!is_valid_instpath(state_install_directory)) 
    {
      if (GetStringFromStringTab(g_inst_header->install_reg_key_ptr)[0])
      {
        HKEY hKey;
		    if ( RegOpenKeyEx((HKEY)g_inst_header->install_reg_rootkey,GetStringFromStringTab(g_inst_header->install_reg_key_ptr),0,KEY_READ,&hKey) == ERROR_SUCCESS)
        {
			    int l = sizeof(buf);
			    int t=REG_SZ;
          if (RegQueryValueEx(hKey,GetStringFromStringTab(g_inst_header->install_reg_value_ptr),NULL,&t,buf,&l ) == ERROR_SUCCESS && t == REG_SZ && buf[0])
			    {
            char tmp[5];
            char *e;
            char *p=buf;
            while (*p && *p != '\"') p++;
            if (*p)
            {
              char *p2=++p;
              while (*p2 && *p2 != '\"') p2++;
              if (*p2)
              {
                *p2=0;
              }
              else p=buf;
            }
            else p=buf; 
            // p is the path now, check for .exe extension
            e=p;
            while (*e) e++;
            while (e>p && *e != '.' && *e != '\\') e--;
            mini_memcpy(tmp,e,4);
            tmp[4]=0;           
            if (!lstrcmpi(tmp,".exe"))        // check extension
            {
              DWORD d;
              e[4]=0;
              d=GetFileAttributes(p);               // get the file attributes
              if (d == (DWORD)-1 || !(d&FILE_ATTRIBUTE_DIRECTORY)) // if not exists, or not directory, then remove suffix
              {
                while (e>p && *e != '\\') e--;
                if (*e == '\\') *e=0;
              }
            }
            lstrcpy(state_install_directory,p);
          }
			    RegCloseKey(hKey);
        }
      }

    }
    if (!is_valid_instpath(state_install_directory)) 
    {
      state_install_directory[0]=0;
      process_string_fromtab(buf,g_inst_header->install_directory_ptr);
      lstrcpy(state_install_directory,buf);
    }

    lstrcpy(g_caption,GetStringFromStringTab(g_inst_header->caption_ptr));
#ifdef NSIS_CONFIG_LOG
    if (g_inst_header->silent_install==2) 
    {
      build_g_logfile();
      log_dolog=1;
    }
#endif

    if (!g_inst_header->silent_install) 
    {
      HWND h=GetDesktopWindow();
#ifdef NSIS_SUPPORT_BGBG
      if (g_inst_header->bg_color1 != -1)
      {
        h=bgWnd_Init(g_hInstance,g_caption,g_inst_header->bg_color1,g_inst_header->bg_color2,g_inst_header->bg_textcolor);
      }
#endif
      g_hwnd=h;
      if (ExecuteCodeSegment(g_inst_entry,g_inst_header->code_onInit,NULL)) return 1;
      g_hwnd=NULL;
      return DialogBox(g_hInstance,MAKEINTRESOURCE(IDD_INST),h,DialogProc);
    }
    else 
    {      
      if (ExecuteCodeSegment(g_inst_entry,g_inst_header->code_onInit,NULL)) return 1;
      if (install_thread(NULL))
      {
        ExecuteCodeSegment(g_inst_entry,g_inst_header->code_onInstFailed,NULL);
        return 1;
      }
      ExecuteCodeSegment(g_inst_entry,g_inst_header->code_onInstSuccess,NULL);
      return 0;
    }
  }
}

static int CALLBACK WINAPI BrowseCallbackProc( HWND hwnd, UINT uMsg, LPARAM lParam, LPARAM lpData)
{
	if (uMsg==BFFM_INITIALIZED)
	{
		GetDlgItemText((HWND)lpData,IDC_DIR,g_tmp,sizeof(g_tmp));
		SendMessage(hwnd,BFFM_SETSELECTION,(WPARAM)1,(LPARAM)g_tmp);
	}
	return 0;
}


static BOOL CALLBACK DialogProc(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	static HICON hIcon;
  if (uMsg == WM_DESTROY && hIcon) { DeleteObject(hIcon); hIcon=0; }
  if (uMsg == WM_INITDIALOG || uMsg == WM_NOTIFY_OUTER_NEXT)
	{
    int backenabled=0,iscp=0,islp=0;
    int delta=(uMsg == WM_NOTIFY_OUTER_NEXT)?wParam:0;
    int x;
    static struct 
    {
      char *id;
      WNDPROC proc;
      char *s;
    }
    windows[4]=
    {
      {MAKEINTRESOURCE(IDD_LICENSE),LicenseProc,"License Agreement"},
      {MAKEINTRESOURCE(IDD_SELCOM),SelProc,"Installation Options"},
      {MAKEINTRESOURCE(IDD_DIR),DirProc,"Installation Directory"},
      {MAKEINTRESOURCE(IDD_INSTFILES),InstProc,"Installing Files"},
    };
    if (uMsg == WM_INITDIALOG)
    {
      g_hwnd=hwndDlg;
      SetDlgItemText(hwndDlg,IDC_VERSTR,verstr);
		  hIcon=LoadIcon(g_hInstance,MAKEINTRESOURCE(IDI_ICON2));
		  SetClassLong(hwndDlg,GCL_HICON,(long)hIcon);
    }
    if (GetStringFromStringTab(g_inst_header->licensetext_ptr)[0] && 
        GetStringFromStringTab(g_inst_header->licensedata_ptr)[0]) islp++;
    if (GetStringFromStringTab(g_inst_header->componenttext_ptr)[0])
      for (x = 1; x < g_inst_header->num_sections && !iscp; x ++)
      {
        char c=GetStringFromStringTab(g_inst_section[x].name_ptr)[0];
        if (c && c != '-') iscp++;
      }
   
    m_page+=delta;
    if (m_page < 0) m_page=0;
    if (!delta) delta++;

    if (m_page==0 && !islp) m_page++;
    if (m_page==1 && !iscp) m_page+=delta;
    if (m_page == 2 && (!GetStringFromStringTab(g_inst_header->text_ptr)[0] ||
         (g_inst_header->no_show_dirpage && 
          is_valid_instpath(state_install_directory) && 
          !ExecuteCodeSegment(g_inst_entry,g_inst_header->code_onVerifyInstDir,NULL)))
      ) m_page+=delta;

    if (m_page==1&&islp) backenabled++;
    if (m_page==2&&(islp||iscp)) backenabled++;

    if (m_page>3) ExecuteCodeSegment(g_inst_entry,g_inst_header->code_onInstSuccess,NULL);

    if (m_curwnd) DestroyWindow(m_curwnd);
    m_curwnd=0;

    if (m_page < 0 || m_page > 3)
    {
      EndDialog(hwndDlg,0);
    }
    else
    {
      wsprintf(g_tmp,"%s: %s",g_caption,windows[m_page].s);
      SetWindowText(hwndDlg,g_tmp);
      m_curwnd=CreateDialog(g_hInstance,windows[m_page].id,hwndDlg,windows[m_page].proc); 
		  if (m_curwnd) 
      {
			  RECT r;
        GetWindowRect(GetDlgItem(hwndDlg,IDC_CHILDRECT),&r);
			  ScreenToClient(hwndDlg,(LPPOINT)&r);
			  SetWindowPos(m_curwnd,0,r.left,r.top,0,0,SWP_NOACTIVATE|SWP_NOSIZE|SWP_NOZORDER);
			  ShowWindow(m_curwnd,SW_SHOWNA);
        EnableWindow(GetDlgItem(hwndDlg,IDC_BACK),backenabled);
		  } 
      SetFocus(GetDlgItem(hwndDlg,IDOK));
    }
  }
  if (uMsg == WM_COMMAND)
  {
    int id=LOWORD(wParam);

    if (id == IDOK && m_curwnd)
    {
      SendMessage(hwndDlg,WM_NOTIFY_OUTER_NEXT,1,0);
    }
		if (id == IDC_BACK && m_curwnd && m_page>0)
    {
      SendMessage(hwndDlg,WM_NOTIFY_OUTER_NEXT,-1,0);
    }
	  if (id == IDCANCEL )
    {
      if (m_abort)
      {
        ExecuteCodeSegment(g_inst_entry,g_inst_header->code_onInstFailed,NULL);        
   			EndDialog(hwndDlg,2);
      }
      else
      {
        if (!ExecuteCodeSegment(g_inst_entry,g_inst_header->code_onUserAbort,NULL))
        {
     			EndDialog(hwndDlg,1);
        }
      }
		}
	}
  if (uMsg == WM_CLOSE)
  {
    if (!IsWindowEnabled(GetDlgItem(hwndDlg,IDCANCEL)) && IsWindowEnabled(GetDlgItem(hwndDlg,IDOK)))
      SendMessage(hwndDlg,WM_COMMAND,IDOK,0);
  }
	return 0;
}

#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
static BOOL CALLBACK DialogProcUninstall(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	static HICON hIcon;
  if (uMsg == WM_DESTROY && hIcon) { DeleteObject(hIcon); hIcon=0; }
  if (uMsg == WM_INITDIALOG || uMsg == WM_NOTIFY_OUTER_NEXT)
	{
    static struct 
    {
      char *id;
      WNDPROC proc;
      char *s;
    }
    windows[2]=
    {
      {MAKEINTRESOURCE(IDD_UNINST),UninstProc,"Confirmation"},
      {MAKEINTRESOURCE(IDD_INSTFILES),InstProc,"Uninstalling Files"},
    };
    if (uMsg == WM_INITDIALOG)
    {
      SetDlgItemText(hwndDlg,IDC_VERSTR,verstr);
      g_hwnd=hwndDlg;
		  hIcon=LoadIcon(g_hInstance,MAKEINTRESOURCE(IDI_ICON2));
		  SetClassLong(hwndDlg,GCL_HICON,(long)hIcon);
      EnableWindow(GetDlgItem(hwndDlg,IDC_BACK),0);
    }
    else m_page++;
    if (m_curwnd) DestroyWindow(m_curwnd);
    m_curwnd=0;
    if (m_page < 0 || m_page > 1)
    {
      EndDialog(hwndDlg,0);
    }
    else
    {
      m_curwnd=CreateDialog(g_hInstance,windows[m_page].id,hwndDlg,windows[m_page].proc); 
      wsprintf(g_tmp,"%s: %s",g_caption,windows[m_page].s);
      SetWindowText(hwndDlg,g_tmp);
    }
		if (m_curwnd) 
    {
			RECT r;
      GetWindowRect(GetDlgItem(hwndDlg,IDC_CHILDRECT),&r);
			ScreenToClient(hwndDlg,(LPPOINT)&r);
			SetWindowPos(m_curwnd,0,r.left,r.top,0,0,SWP_NOACTIVATE|SWP_NOSIZE|SWP_NOZORDER);
			ShowWindow(m_curwnd,SW_SHOWNA);
		} 
    SetFocus(GetDlgItem(hwndDlg,IDOK));
  }
  if (uMsg == WM_COMMAND)
  {
    if (LOWORD(wParam) == IDOK && m_curwnd)
    {
      SendMessage(hwndDlg,WM_NOTIFY_OUTER_NEXT,1,0);
    }
	  if (LOWORD(wParam) == IDCANCEL)
    {
   		EndDialog(hwndDlg,2);
		}
	}
  if (uMsg == WM_CLOSE)
  {
    if (!IsWindowEnabled(GetDlgItem(hwndDlg,IDCANCEL)) && IsWindowEnabled(GetDlgItem(hwndDlg,IDOK)))
      SendMessage(hwndDlg,WM_COMMAND,IDOK,0);
  }
	return 0;
}
#endif




static BOOL CALLBACK LicenseProc(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
  if (uMsg == WM_INITDIALOG)
  {    
    SetDlgItemText(hwndDlg,IDC_EDIT1,GetStringFromStringTab(g_inst_header->licensedata_ptr));
    SetDlgItemText(hwndDlg,IDC_INTROTEXT,GetStringFromStringTab(g_inst_header->licensetext_ptr));
  }
  if (uMsg == WM_CLOSE) SendMessage(GetParent(hwndDlg),WM_CLOSE,0,0);
  return 0;
}

#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
static BOOL CALLBACK UninstProc(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
  if (uMsg == WM_INITDIALOG)
  {
    SetDlgItemText(hwndDlg,IDC_UNINSTFROM,"Uninstalling from:");
    SetDlgItemText(hwndDlg,IDC_INTROTEXT,GetStringFromStringTab(g_inst_uninstheader->uninstalltext_ptr));
    SetDlgItemText(hwndDlg,IDC_EDIT1,state_install_directory);
  }
  return 0;
}
#endif


static void inttosizestr(int kb, char *str)
{
  while (*str) str++;
  if (kb < 1024) wsprintf(str,"%dKB",kb);
  else if (kb < 1024*1024) wsprintf(str,"%dMB",kb>>10);
  else wsprintf(str,"%dGB+",kb>>20);
}

static BOOL CALLBACK DirProc(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
  if (uMsg == WM_DESTROY)
  {
    GetDlgItemText(hwndDlg,IDC_DIR,state_install_directory,1024);
#ifdef NSIS_CONFIG_LOG
    build_g_logfile();
    log_dolog = !!IsDlgButtonChecked(hwndDlg,IDC_CHECK1);
#endif
  }
  if (uMsg == WM_INITDIALOG)
  {
    char str[256];
#ifdef NSIS_CONFIG_LOG
    if (GetAsyncKeyState(VK_SHIFT)&0x8000) 
    {
      HWND h=GetDlgItem(hwndDlg,IDC_CHECK1);
      SetWindowText(h,"Log install process");
      ShowWindow(h,SW_SHOWNA);
    }
#endif
    SetDlgItemText(hwndDlg,IDC_DIR,state_install_directory);
    SetDlgItemText(hwndDlg,IDC_INTROTEXT,GetStringFromStringTab(g_inst_header->text_ptr));
    SetDlgItemText(hwndDlg,IDC_BROWSE,"Browse...");
    wsprintf(str,"Select the directory to install %s in:",GetStringFromStringTab(g_inst_header->name_ptr));
    SetDlgItemText(hwndDlg,IDC_SELDIRTEXT,str);
    SendMessage(hwndDlg,WM_IN_UPDATEMSG,0,0);
  }
  if (uMsg == WM_COMMAND)
  {
    int id=LOWORD(wParam);
    if (id == IDC_DIR && HIWORD(wParam) == EN_CHANGE)
    {
      SendMessage(hwndDlg,WM_IN_UPDATEMSG,0,0);
    }
		if (id == IDC_BROWSE)
		{
			char name[1024];
      char str[1024];
      BROWSEINFO bi={0,};
			ITEMIDLIST *idlist;
			GetDlgItemText(hwndDlg,IDC_DIR,name,1024);
			GetDlgItemText(hwndDlg,IDC_SELDIRTEXT,str,1024);
			bi.hwndOwner = hwndDlg;
			bi.pszDisplayName = name;
			bi.lpfn=BrowseCallbackProc;
			bi.lParam=(LPARAM)hwndDlg;
			bi.lpszTitle = str;
			bi.ulFlags = BIF_RETURNONLYFSDIRS;
			idlist = SHBrowseForFolder( &bi );
			if (idlist) 
      {
        char *post_str;
        char *p;
        IMalloc *m;
				SHGetPathFromIDList( idlist, name );        
        SHGetMalloc(&m);
        if (m)
        {
          m->lpVtbl->Free(m,idlist);
          m->lpVtbl->Release(m);
        }
        p=post_str=GetStringFromStringTab(g_inst_header->install_directory_ptr);
        while (*p) p++;
        while (p >= post_str && *p != '\\') p--;               
        if (p >= post_str && *++p)
        {
          post_str=p;
          p=name+lstrlen(name)-lstrlen(post_str);
          if (p <= name || p[-1]!='\\' || lstrcmpi(p,post_str))
          {
            if (name[lstrlen(name)-1]!='\\') lstrcat(name,"\\");
            lstrcat(name,post_str);
          }
        }

				SetDlgItemText(hwndDlg,IDC_DIR,name);
        uMsg = WM_IN_UPDATEMSG;
			}
		}
  }
  if (uMsg == WM_IN_UPDATEMSG)
  {
    static char s[1024];
    int is_valid_path;
    int x;
    int total=0, available=-1;
    DWORD spc,bps,fc,tc;
    GetDlgItemText(hwndDlg,IDC_DIR,state_install_directory,1024);
    is_valid_path=is_valid_instpath(state_install_directory);

    mini_memcpy(s,state_install_directory,sizeof(s));
    s[sizeof(s)-1]=0;
    if (s[1] == ':') s[3]=0;
    else if (s[0] == '\\' && s[1] == '\\') // \\ path
    {
      if (s[lstrlen(s)-1]!='\\') 
        lstrcat(s,"\\");
    }

    if (GetDiskFreeSpace(s,&spc,&bps,&fc,&tc))
    {
      DWORD r;
      DWORD v=0x7fffffff;
      r=bps*spc*(fc>>10);
      if (!r) r=(bps*spc*fc)>>10;
      if (r > v) r=v;
      available=(int)r;
    }
    for (x = 0; x < g_inst_header->num_sections; x ++)
    {
      if (!x || !GetStringFromStringTab(g_inst_section[x].name_ptr)[0]||g_inst_section[x].default_state&0x80000000)
       total+=g_inst_section[x].size_kb;
    }
    lstrcpy(s,g_space_requiredstr);
    inttosizestr(total,s);
    SetDlgItemText(hwndDlg,IDC_SPACEREQUIRED,s);
    if (available != -1)
    {
      lstrcpy(s,"Space available: ");
      inttosizestr(available,s);
      SetDlgItemText(hwndDlg,IDC_SPACEAVAILABLE,s);
    }
    else
      SetDlgItemText(hwndDlg,IDC_SPACEAVAILABLE,"");

    EnableWindow(GetDlgItem(GetParent(hwndDlg),IDOK),
      is_valid_path && (available >= total || available == -1) &&
      !ExecuteCodeSegment(g_inst_entry,g_inst_header->code_onVerifyInstDir,NULL));
  }
  return 0;
}

static HBITMAP hBMcheck[2];

static BOOL CALLBACK SelProc(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
  HWND hwndCombo1 = GetDlgItem(hwndDlg,IDC_COMBO1);
  HWND hwndList1 = GetDlgItem(hwndDlg,IDC_LIST1);
  if (uMsg == WM_INITDIALOG)
  {
    hBMcheck[0]=LoadBitmap(g_hInstance, MAKEINTRESOURCE(IDB_BITMAP1)); 
    hBMcheck[1]=LoadBitmap(g_hInstance, MAKEINTRESOURCE(IDB_BITMAP2)); 
    SetDlgItemText(hwndDlg,IDC_INTROTEXT,GetStringFromStringTab(g_inst_header->componenttext_ptr));
    if (!GetStringFromStringTab(g_inst_header->install_types_ptr[0])[0])
    {
      ShowWindow(hwndCombo1,SW_HIDE);
      SetDlgItemText(hwndDlg,IDC_TEXT2,"Select components to install:");
    }
    else
    {
      int x;
      SetDlgItemText(hwndDlg,IDC_TEXT1,"Select the type of install:");
      if (!g_inst_header->no_custom_instmode_flag) SetDlgItemText(hwndDlg,IDC_TEXT2,"Or, select the optional components you wish to install:");     
      for (x = 0; x < MAX_INST_TYPES && GetStringFromStringTab(g_inst_header->install_types_ptr[x])[0]; x ++)
      {
        SendMessage(hwndCombo1,CB_ADDSTRING,0,(LPARAM)GetStringFromStringTab(g_inst_header->install_types_ptr[x]));
      }
      if (!g_inst_header->no_custom_instmode_flag) SendMessage(hwndCombo1,CB_ADDSTRING,0,(LPARAM)"Custom");
      SendMessage(hwndCombo1,CB_SETCURSEL,m_whichcfg,0);
    }
    {
      int x;
      int a=0;

      for (x = 0; x < g_inst_header->num_sections; x ++)
      {
        if (GetStringFromStringTab(g_inst_section[x].name_ptr)[0])
        {
          SendMessage(hwndList1,LB_ADDSTRING,0,(LPARAM)GetStringFromStringTab(g_inst_section[x].name_ptr));
          if (!x || (g_inst_section[x].default_state & (0x80000000|(1<<m_whichcfg))) 
              || !GetStringFromStringTab(g_inst_header->install_types_ptr[0])[0])
            SendMessage(hwndList1,LB_SETSEL,TRUE,a);
          a++;
        }
      }
      SendDlgItemMessage(hwndDlg,IDC_LIST1,WM_VSCROLL,SB_TOP,0);
    }
    SendMessage(hwndDlg,WM_IN_UPDATEMSG,0,0);
  }
  if (uMsg == WM_COMMAND)
  {
    int id=LOWORD(wParam),code=HIWORD(wParam);
    if (id == IDC_LIST1 && code==LBN_SELCHANGE)
    {
      int r,l=SendMessage(hwndCombo1,CB_GETCOUNT,0,0)-1;

      if (g_inst_header->no_custom_instmode_flag) 
      {
        int a=0;
        int x;
        for (x = 0; x < g_inst_header->num_sections; x ++)
        {
          char c=GetStringFromStringTab(g_inst_section[x].name_ptr)[0];
          if (c)
          {
            if (x && c != '-')
            {
              if (!(g_inst_section[x].default_state&(1<<m_whichcfg)) != !SendMessage(hwndList1,LB_GETSEL,a,0)) 
              {
                SendMessage(hwndList1,LB_SETSEL,(g_inst_section[x].default_state>>m_whichcfg)&1,a);
              }
            }
            a++;
          }
        }
      }

      for (r = 0; r < l; r ++)
      {
        int a=0;
        int x;
        for (x = 0; x < g_inst_header->num_sections; x ++)
        {
          char c=GetStringFromStringTab(g_inst_section[x].name_ptr)[0];
          if (c)
          {
            if (x && c != '-')
            {
              if (!(g_inst_section[x].default_state&(1<<r)) != !SendMessage(hwndList1,LB_GETSEL,a,0)) break;
            }
            a++;
          }
        }
        if (x == g_inst_header->num_sections) break;
      }

      SendMessage(hwndCombo1,CB_SETCURSEL,r,0);
      m_whichcfg=r;
    
      uMsg=WM_IN_UPDATEMSG;
    }
    if (id == IDC_COMBO1 && code==CBN_SELCHANGE)
    {
      int t=SendMessage(hwndCombo1,CB_GETCURSEL,0,0);
      if (t != CB_ERR)
      {
        m_whichcfg=t;
        if (g_inst_header->no_custom_instmode_flag || 
          t != SendMessage(hwndCombo1,CB_GETCOUNT,0,0)-1)
        {
          int x,a=0;
          for (x = 0; x < g_inst_header->num_sections; x ++)
          {
            if (GetStringFromStringTab(g_inst_section[x].name_ptr)[0])
            {
              SendMessage(hwndList1,LB_SETSEL,(!x||(g_inst_section[x].default_state & (1<<t))),a);
              a++;
            }
          }
          SendMessage(hwndList1,WM_VSCROLL,SB_TOP,0);
        }
        uMsg=WM_IN_UPDATEMSG;
      }
    }
  }
  if (uMsg == WM_DESTROY)
  {
    int x,a=0;
    for (x = 0; x < g_inst_header->num_sections; x ++)
    {
      if (!GetStringFromStringTab(g_inst_section[x].name_ptr)[0] ||
          SendDlgItemMessage(hwndDlg,IDC_LIST1,LB_GETSEL,a++,0) ||
          !x)
        g_inst_section[x].default_state|=0x80000000;
      else
        g_inst_section[x].default_state&=~0x80000000;
    }  
    DeleteObject(hBMcheck[0]);
    DeleteObject(hBMcheck[1]);
  }
  if (uMsg == WM_IN_UPDATEMSG)
  {
    int x;
    int a=0;
    int total=0;
    char s[128];
    for (x = 0; x < g_inst_header->num_sections; x ++)
    {
      if (!GetStringFromStringTab(g_inst_section[x].name_ptr)[0] ||
          SendDlgItemMessage(hwndDlg,IDC_LIST1,LB_GETSEL,a++,0) || !x)
        total+=g_inst_section[x].size_kb;
    }
    lstrcpy(s,g_space_requiredstr);
    inttosizestr(total,s);
    SetDlgItemText(hwndDlg,IDC_SPACEREQUIRED,s);
  }
  if (uMsg == WM_MEASUREITEM)
  {
    LPMEASUREITEMSTRUCT lpmis = (LPMEASUREITEMSTRUCT) lParam; 
    char text[64];
    SendDlgItemMessage(hwndDlg,IDC_LIST1,LB_GETTEXT,lpmis->itemID,(LPARAM)text);
    lpmis->itemHeight = (text[0]=='-')?5:LB_ICONHEIGHT; 
    return TRUE; 
  }
  if (uMsg == WM_CTLCOLORLISTBOX)
  {
    SetTextColor((HDC)wParam,GetNearestColor((HDC)wParam,RGB(0,0,0)));
    SetBkColor((HDC)wParam,RGB(255,255,255));
    return (int)GetStockObject(WHITE_BRUSH);
  }
  if (uMsg == WM_DRAWITEM)
  {
    LPDRAWITEMSTRUCT lpdis = (LPDRAWITEMSTRUCT) lParam; 

    if (lpdis->itemID != -1) 
    {
      if (lpdis->itemAction == ODA_SELECT || lpdis->itemAction == ODA_DRAWENTIRE)
      {
        TEXTMETRIC tm;
        int y;

        HBITMAP oldbm;
        HDC memdc;

        SendMessage(lpdis->hwndItem, LB_GETTEXT, lpdis->itemID, (LPARAM) g_tmp); 

        if (g_tmp[0]!='-')
        {
          GetTextMetrics(lpdis->hDC, &tm); 

          y = (lpdis->rcItem.bottom + lpdis->rcItem.top - tm.tmHeight) / 2; 

          TextOut(lpdis->hDC, LB_ICONWIDTH + 2, y, g_tmp, lstrlen(g_tmp)); 

          memdc=CreateCompatibleDC(lpdis->hDC);
          oldbm=SelectObject(memdc,hBMcheck[!((lpdis->itemState & ODS_SELECTED)||(!lpdis->itemID&&GetStringFromStringTab(g_inst_section[0].name_ptr)[0]))]);
          BitBlt(lpdis->hDC,lpdis->rcItem.left,lpdis->rcItem.top,LB_ICONWIDTH,LB_ICONHEIGHT,memdc,0,0,SRCCOPY);
          SelectObject(memdc,oldbm);
          DeleteObject(memdc);
        }
        else
        {
          HPEN oldpen;
          HPEN pen;
          HDC memdc;
          int w=lpdis->rcItem.right-lpdis->rcItem.left;
          y=lpdis->rcItem.top/2+lpdis->rcItem.bottom/2;
          memdc=lpdis->hDC;
          pen=CreatePen(PS_SOLID,0,RGB(192,192,192));
          oldpen=SelectObject(memdc,pen);
          MoveToEx(memdc,lpdis->rcItem.left+w/8,y,NULL);
          LineTo(memdc,lpdis->rcItem.right-w/8,y);
          SelectObject(memdc,oldpen);
          DeleteObject(pen);
        }
      }
    } 
    return TRUE; 
  }
  return 0;
}


void update_status_text(char *text1, char *text2)
{
  if (insthwnd||insthwnd2)
  {
    static char buf[4096]; // different thread, must use private buf.
    if (lstrlen(text1)+lstrlen(text2) >= sizeof(buf)) return;

    lstrcpy(buf,text1);
    lstrcat(buf,text2);
    if (insthwnd) 
    {
      SendMessage(insthwnd,LB_ADDSTRING,(WPARAM)0,(LPARAM)buf);
      if (IsWindowVisible(insthwnd)) SendMessage(insthwnd,WM_VSCROLL,SB_BOTTOM,0);
    }
    if (insthwnd2) SetWindowText(insthwnd2,buf);
  }
}


static DWORD WINAPI install_thread(LPVOID p)
{
  HWND hwndDlg=(HWND)p;
  int m_inst_sec=0;
#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
  if (!g_inst_header) 
  {
    ExecuteCodeSegment(g_inst_entry,g_inst_uninstheader->code,hwndDlg?GetDlgItem(hwndDlg,IDC_PROGRESS1):0);
  }
  else
  {
#endif
    while (m_inst_sec<g_inst_header->num_sections && !m_abort)
    {
      if (g_inst_section[m_inst_sec].default_state&0x80000000 || 
          g_inst_header->silent_install) 
      {
        log_printf2("Section: \"%s\"",GetStringFromStringTab(g_inst_section[m_inst_sec].name_ptr));
        if (ExecuteCodeSegment(g_inst_entry,g_inst_section[m_inst_sec].code,hwndDlg?GetDlgItem(hwndDlg,IDC_PROGRESS1):0)) m_abort++;
      }
      else
      {
        log_printf2("Skipping section: \"%s\"",GetStringFromStringTab(g_inst_section[m_inst_sec].name_ptr));
      }
      m_inst_sec++;
    }
#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
    if (!m_abort && g_inst_header) 
    {
      if (g_inst_header->uninstdata_offset != -1)
      {
        int ret=-666;
        char *p=GetStringFromStringTab(g_inst_header->uninstall_exe_name_ptr);
        static char buf[1024];
        HANDLE hFile;
      
        lstrcpy(buf,state_install_directory);
        if (buf[lstrlen(buf)-1]!='\\') lstrcat(buf,"\\");
        lstrcat(buf,p);

        hFile=CreateFile(buf,GENERIC_WRITE,0,NULL,CREATE_ALWAYS,0,NULL);
        if (hFile != INVALID_HANDLE_VALUE)
        {
          char *filebuf;
          DWORD l;
          filebuf=(char *)GlobalAlloc(GMEM_FIXED,g_filehdrsize);
          if (filebuf)
          {
            int fixoffs=0;
            SetFilePointer(g_db_hFile,0,NULL,FILE_BEGIN);
            ReadFile(g_db_hFile,filebuf,g_filehdrsize,&l,NULL);
            if (g_inst_header->uninstexehead_iconoffset > 0)
            {
              GetCompressedDataFromDataBlockToMemory(g_inst_header->uninstdata_offset,
                filebuf+g_inst_header->uninstexehead_iconoffset,766-ICO_HDRSKIP);
            }
            WriteFile(hFile,filebuf,g_filehdrsize,&l,NULL);
            GlobalFree(filebuf);
            ret=GetCompressedDataFromDataBlock(-1,hFile);         
          }
          CloseHandle(hFile);
        }
        log_printf3("created uninstaller: %d, \"%s\"",ret,buf);
        if (ret < 0)
        {
          update_status_text("Error creating: ",p);
          DeleteFile(buf);
        }
        else
          update_status_text("Created uninstaller: ",p);
      }
    }
  }
#endif
  if (hwndDlg) SendMessage(hwndDlg,WM_NOTIFY_INSTPROC_DONE,m_abort,0);
  return m_abort;
}

static BOOL CALLBACK InstProc(HWND hwndDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
  static HBRUSH hBrush;
  static int lb_bg,lb_fg;
  if (uMsg == WM_DESTROY && hBrush) DeleteObject(hBrush);
  if (uMsg == WM_INITDIALOG)
  {
    DWORD id;
    HWND hwnd;
    int num;
    int x;
    LOGBRUSH lb;
    lb.lbStyle=BS_SOLID;

    insthwndbutton=GetDlgItem(hwndDlg,IDC_SHOWDETAILS);
    SetWindowText(insthwndbutton,"Show details");
    insthwnd2=GetDlgItem(hwndDlg,IDC_PROGRESSTEXT);
    insthwnd=GetDlgItem(hwndDlg,IDC_LIST1);
    if (g_inst_header)
    {
      log_printf3("New install of \"%s\" to \"%s\"",GetStringFromStringTab(g_inst_header->name_ptr),state_install_directory);
      num=0;
      for (x = 0; x < g_inst_header->num_sections; x ++)
      {
        if (g_inst_section[x].default_state&0x80000000) num+=g_inst_section[x].code[1]-g_inst_section[x].code[0];
      }
      g_show_details=g_inst_header->show_details;
      if (g_inst_header->show_details)
      {
        ShowWindow(insthwndbutton,SW_HIDE);
        if (g_inst_header->show_details != 2) ShowWindow(insthwnd,SW_SHOWNA);
      }
      lb.lbColor=g_inst_header->lb_bg;
      lb_bg=g_inst_header->lb_bg;
      lb_fg=g_inst_header->lb_fg;
    }
#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
    else 
    {
      num=g_inst_uninstheader->code[1]-g_inst_uninstheader->code[0];
      lb.lbColor=g_inst_uninstheader->lb_bg;
      lb_bg=g_inst_uninstheader->lb_bg;
      lb_fg=g_inst_uninstheader->lb_fg;
    }
#endif
    if (lb_fg != -1) hBrush=CreateBrushIndirect(&lb);

    hwnd=GetDlgItem(hwndDlg,IDC_PROGRESS1);
  	SendMessage(hwnd,PBM_SETRANGE,0,MAKELPARAM(0,num));
	  SendMessage(hwnd,PBM_SETPOS,0,0);       

    hwnd=GetParent(hwndDlg);
    EnableWindow(GetDlgItem(hwnd,IDC_BACK),0);
    EnableWindow(GetDlgItem(hwnd,IDOK),0);
    EnableWindow(GetDlgItem(hwnd,IDCANCEL),0);

    CloseHandle(CreateThread(NULL,0,install_thread,(LPVOID)hwndDlg,0,&id));
  }
  if (uMsg == WM_CTLCOLORLISTBOX && lb_fg != -1)
  {
    SetTextColor((HDC)wParam,GetNearestColor((HDC)wParam,lb_fg));
    SetBkColor((HDC)wParam,lb_bg);
    return (int)hBrush;
  }
  if (uMsg == WM_COMMAND && LOWORD(wParam) == IDC_SHOWDETAILS)
  {
    ShowWindow(GetDlgItem(hwndDlg,IDC_SHOWDETAILS),SW_HIDE);
    SendMessage(insthwnd,WM_VSCROLL,SB_BOTTOM,0);
    ShowWindow(insthwnd,SW_SHOWNA);
  }
  if (uMsg == WM_NOTIFY_INSTPROC_DONE)
  {
    if (!wParam)
    {
      HWND h2=GetParent(hwndDlg);
      HWND h=GetDlgItem(h2,IDOK);
      EnableWindow(h,1);
      if (!g_inst_header || !g_autoclose)
      {
        ShowWindow(g_hwnd,SW_SHOWNA);
        lstrcat(g_caption,": Completed");
        SetWindowText(h2,g_caption);
        update_status_text("Completed","");
        SetWindowText(h,"Close");
        SetFocus(h);
      }
      else
      {
        SendMessage(GetParent(hwndDlg),WM_NOTIFY_OUTER_NEXT,1,0);
      }
    }
    else
    {
      HWND h=GetDlgItem(GetParent(hwndDlg),IDCANCEL);
      EnableWindow(h,1);
      SetFocus(h);
    }
  }
  return 0;
}
