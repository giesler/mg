// Autorun.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "Autorun.h"
#include "AutorunDlg.h"
#include "SetupDlg.h"
#include "util.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CAutorunApp

BEGIN_MESSAGE_MAP(CAutorunApp, CWinApp)
	//{{AFX_MSG_MAP(CAutorunApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CAutorunApp construction

CAutorunApp::CAutorunApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CAutorunApp object

CAutorunApp theApp;

/////////////////////////////////////////////////////////////////////////////
// CAutorunApp initialization

BOOL CAutorunApp::InitInstance()
{

	util u;

	// check if this is setup.exe, if so skip dialog
	char chTemp[MAX_PATH];
	GetModuleFileName(NULL, chTemp, MAX_PATH);
	CString sEXEName = chTemp; 
	sEXEName = sEXEName.Mid( sEXEName.ReverseFind('\\') + 1 );	
	bool bIsSetup = false;
	if (sEXEName.CompareNoCase("setup.exe") == 0)
		bIsSetup = true;

	// if no command line options, show dialog allowing cancel
	if (!bIsSetup && (m_lpCmdLine[0] == '\0')) {
		CAutorunDlg dlg;
//		m_pMainWnd = &dlg;
		int nResponse = dlg.DoModal();
		if (nResponse == IDOK) {
		} else if (nResponse == IDCANCEL) {	
			return FALSE;
		}
	}

	u.RemoveMDACKey();		// in case it is present
	u.SysUpdates(NULL);

	// Since the dialog has been closed, return FALSE so that we exit the
	//  application, rather than start the application's message pump.
	return FALSE;
}
