// StartSS.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "StartSS.h"
#include "StartSSDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CStartSSApp

BEGIN_MESSAGE_MAP(CStartSSApp, CWinApp)
	//{{AFX_MSG_MAP(CStartSSApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CStartSSApp construction

CStartSSApp::CStartSSApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CStartSSApp object

CStartSSApp theApp;

/////////////////////////////////////////////////////////////////////////////
// CStartSSApp initialization

BOOL CStartSSApp::InitInstance()
{
	// Standard initialization
	// If you are not using these features and wish to reduce the size
	//  of your final executable, you should remove from the following
	//  the specific initialization routines you do not need.

	ShellExecute(NULL, "open", "pic01.html", NULL, NULL, SW_SHOWMAXIMIZED);

	// Since the dialog has been closed, return FALSE so that we exit the
	//  application, rather than start the application's message pump.
	return FALSE;
}
