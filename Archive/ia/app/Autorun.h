// Autorun.h : main header file for the AUTORUN application
//

#if !defined(AFX_AUTORUN_H__81ADC6A1_6924_4CA0_82D4_511D1807929B__INCLUDED_)
#define AFX_AUTORUN_H__81ADC6A1_6924_4CA0_82D4_511D1807929B__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols
#include "SetupDlg.h"
#include "component.h"			// for item components
#include "button.h"				// for buttons on autorun dialog
#include "utilities.h"
#include "log.h"
#include "restartdlg.h"
#include "CommandLineInfoEx.h"

extern CLog gLog;
extern CUtilities gUtils;

enum DisplayType {
	DisplayTypeNormalDisplay = 0,
	DisplayTypeSkipSplashDisplay,
	DisplayTypeProgramNameDisplay
};

/////////////////////////////////////////////////////////////////////////////
// CAutorunApp:
// See Autorun.cpp for the implementation of this class
//

class CAutorunApp : public CWinApp
{
public:
	CAutorunApp();
	~CAutorunApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAutorunApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	public:
		void LoadSettings();
		CString mstrAppName;
		CString CurrentOS();
		CCommandLineInfoEx cmdInfo;

	private:
		CString mstrSkipProgramName;
		DisplayType mtypDisplayType;
		bool mblnSingleInstance;
		CString mstrAbortMutex;
		bool m_blnEnableLogging;


	//{{AFX_MSG(CAutorunApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_AUTORUN_H__81ADC6A1_6924_4CA0_82D4_511D1807929B__INCLUDED_)
