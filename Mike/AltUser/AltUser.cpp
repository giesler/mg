// AltUser.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "AltUser.h"
#include "AltUserDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CAltUserApp

BEGIN_MESSAGE_MAP(CAltUserApp, CWinApp)
	//{{AFX_MSG_MAP(CAltUserApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CAltUserApp construction

CAltUserApp::CAltUserApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CAltUserApp object

CAltUserApp theApp;

/////////////////////////////////////////////////////////////////////////////
// CAltUserApp initialization

BOOL CAltUserApp::InitInstance()
{
	// Standard initialization
	// If you are not using these features and wish to reduce the size
	//  of your final executable, you should remove from the following
	//  the specific initialization routines you do not need.

/*	CAltUserDlg dlg;
	m_pMainWnd = &dlg;
	int nResponse = dlg.DoModal();
	if (nResponse == IDOK)
	{
*/
	
	// first enable token priv

   HANDLE hProcToken = NULL;

   TOKEN_PRIVILEGES    tp;

   // Initialize structures.
   ZeroMemory(&tp, sizeof(tp));

  // Retrieve a handle to the process token with the proper access.
  if (!OpenProcessToken(GetCurrentProcess(), TOKEN_QUERY 
        | TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_SESSIONID 
        | TOKEN_ADJUST_DEFAULT | TOKEN_ASSIGN_PRIMARY
        | TOKEN_DUPLICATE, &hProcToken)) {
	    DisplayError(L"OpenProcessToken");
      return false;
    }

	// Look up the LUID for the TCB Name privilege.
  if (!LookupPrivilegeValue(NULL, SE_TCB_NAME, 
        &tp.Privileges[0].Luid)) {
    DisplayError("LookupPrivilegeValue");
		return false;
  }

  // Enable the TCB Name privilege in the process token.
  tp.PrivilegeCount = 1;
  tp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
  if (!AdjustTokenPrivileges(hProcToken, FALSE, &tp, 0, NULL, 0)) {
   DisplayError("Adjust Token Privs");
   return false;
	}
  


	// figure out outlook command line
	HRESULT hResult; HKEY hKey; char chData[255]; DWORD lDataLen = 255;
	CString sOutlookCmd = "Software\\Microsoft\\Windows\\CurrentVersion\\App Paths\\OUTLOOK.EXE";
	hResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, sOutlookCmd, NULL, KEY_QUERY_VALUE, &hKey);
	if (hResult != ERROR_SUCCESS) {
		DisplayError("Open OUTLOOK.EXE key"); hResult = RegCloseKey(hKey); return false;
	}
	
	hResult = RegQueryValueEx(hKey, NULL /* return default */, NULL, NULL, (LPBYTE)&chData, &lDataLen);
	if (hResult != ERROR_SUCCESS) {
		DisplayError("Query OUTLOOK.EXE key"); hResult = RegCloseKey(hKey); return false;
	}
	hResult = RegCloseKey(hKey);
	
	int lReturn;
	STARTUPINFO sInfo; PROCESS_INFORMATION pInfo; STARTUPINFOW swInfo;
	HANDLE hProcess;
	swInfo.dwFlags = STARTF_USESHOWWINDOW;
	swInfo.wShowWindow = SW_MAXIMIZE;
	ZeroMemory(&sInfo, sizeof(sInfo));
	sInfo.cb = sizeof(STARTUPINFO);
	ZeroMemory(&swInfo, sizeof(swInfo));
	swInfo.cb = sizeof(STARTUPINFOW);
	CString strUser = "sara";
	BSTR bsUser = strUser.AllocSysString();
	CString strDomain = "wisc";
	BSTR bsDomain = strDomain.AllocSysString();
	CString strPwd = "tcgd";
	BSTR bsPwd = strPwd.AllocSysString();
	CString strPath = chData;
	BSTR bsPath = strPath.AllocSysString();
	CString strCmdLine = "outlook.exe";
	BSTR bsCmdLine = strCmdLine.AllocSysString();

	PROFILEINFO pi; ZeroMemory(&pi, sizeof(pi)); pi.dwSize = sizeof(pi);
	HANDLE hToken;
	
	// Log on user
	if (!LogonUser(strUser.AllocSysString(), strDomain.AllocSysString(), strPwd.AllocSysString(), LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_WINNT50, &hToken)) {
		DisplayError("Log On User");
		return false;
	}
	// Load profile
	if (!LoadUserProfile(hToken, &pi)) {
		DisplayError("Load Profile");
		return false;
	}



	lReturn = CreateProcessWithLogonW(bsUser,  // user
	  bsDomain, // domain
	  bsPwd, 
	  LOGON_WITH_PROFILE,
	  bsPath,
	  bsCmdLine,
	  NORMAL_PRIORITY_CLASS, NULL, NULL, &swInfo, &pInfo);

	
	if (!lReturn) {
		DisplayError("Create Process With Logon");
		return false;
	}
	
	// Get process and wait for it to end
	hProcess = pInfo.hProcess;
	WaitForSingleObject(hProcess, INFINITE);

	// Unload profile
	AfxMessageBox("Unloading profile...");
	if (!UnloadUserProfile(hToken, pi.hProfile)) {
		DisplayError("Unloading Profile");
		return false;
	}

	if (hToken) CloseHandle(hToken);
	if (hProcToken) CloseHandle(hProcToken);
	if (pInfo.hProcess) CloseHandle(pInfo.hProcess);
	if (pInfo.hThread) CloseHandle(pInfo.hThread);

		// TODO: Place code here to handle when the dialog is
		//  dismissed with OK
/*	}
	else if (nResponse == IDCANCEL)
	{
		// TODO: Place code here to handle when the dialog is
		//  dismissed with Cancel
	}
*/
	// Since the dialog has been closed, return FALSE so that we exit the
	//  application, rather than start the application's message pump.
	return FALSE;
}

void CAltUserApp::DisplayError(CString strTitle) {
	LPVOID lpMsgBuf;
	CString sTitle = strTitle + ": Error #"; char * buffer;
	buffer = new char[20];
	ltoa(GetLastError(), buffer, 10);
	sTitle += buffer;

	FormatMessage( 
	    FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
	    NULL, GetLastError(), MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPTSTR) &lpMsgBuf, 0, NULL);
	// Display the string.
	if (GetLastError() != 0) 
		MessageBox( NULL, (LPCTSTR)lpMsgBuf, strTitle, MB_OK | MB_ICONINFORMATION );
	// Free the buffer.
	LocalFree( lpMsgBuf );
};