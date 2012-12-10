// Autorun.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "Autorun.h"
#include "log.h"
#include "utilities.h"
#include "AutorunDlg.h"
#include "SetupDlg.h"
#include "util.h"
#include "CommandLineInfoEx.h"

CLog gLog;
CUtilities gUtils;

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
//	mstrAppName.LoadString(IDS_DEFAULTPROGRAMNAME);

	// Place all significant initialization in InitInstance
}


// Clean up after ourselves
CAutorunApp::~CAutorunApp()
{
}


/////////////////////////////////////////////////////////////////////////////
// The one and only CAutorunApp object

CAutorunApp theApp;

/////////////////////////////////////////////////////////////////////////////
// CAutorunApp initialization

BOOL CAutorunApp::InitInstance()
{
	// Parse the command line
	ParseCommandLine(cmdInfo);
	
	// make sure we have a valid settings.ini file
	CString strSettingsPath = gUtils.EXEPath() + "ia\\settings.ini";
	if (!gUtils.FileExists(strSettingsPath)) 
	{
		CString strErrMsg; strErrMsg.Format(IDS_NOSETTINGS, strSettingsPath);
		AfxMessageBox(strErrMsg, MB_ICONSTOP);
		return false;
	}

	// load basic settings
	LoadSettings();

	// Check OS
	CString result;
	if (!gUtils.ValidateOS("RequiredOS", result)) 
	{
		gLog.LogEvent("OS Validation returned: " + result);
		result = gUtils.GetINIString("RequiredOS", "InvalidOSMessage", result);
		MessageBox(NULL, result, mstrAppName, MB_ICONSTOP);
		return false;
	}

	// Start the log
	gLog.Init(cmdInfo.GetOption("log") || m_blnEnableLogging);
	gLog.LogEvent("Application: " + gUtils.EXEPath() + gUtils.EXEName());
	gLog.LogEvent("OS: " + CurrentOS());

	// Check if we care about single instance
	if (mblnSingleInstance) 
	{
		// Check if any other instances before setting a mutex
		HANDLE hMutex = OpenMutex(MUTEX_ALL_ACCESS, FALSE, "ia3_mutex");
		if (hMutex != NULL && mblnSingleInstance) 
		{
			gLog.LogEvent("Another instance of this program is already running, and 'single instance' has been set.  Exiting.");
			CString singleInstanceError = gUtils.GetINIString("Settings", "SingleInstanceError", "");
			if (!singleInstanceError.IsEmpty())
				MessageBox(NULL, singleInstanceError, mstrAppName, MB_ICONSTOP);
			return false;
		}

		// No other instance is running, so set a mutex
		CreateMutex(NULL, false, "ia3_mutex");
	}

	// Check if we care about a user defined mutex
	if (mstrAbortMutex != "") 
	{
		// Attempt to open user defined mutex
		if (OpenMutex(MUTEX_ALL_ACCESS, FALSE, mstrAbortMutex) != NULL ) 
		{
			gLog.LogEvent("The user defined mutex '" + mstrAbortMutex + "' was found.  Exiting.");
			return false;
		}
	}

	bool showDialog = true;
	bool continueMode = false;
	CString continueButton = "";
	bool setupOnlyMode = false;
	CDlgButton* pButton = NULL;
	CString strTemp;

	// check if this is setup.exe, if so skip dialog
	CString strFileName = gUtils.EXEName(); 
	if (mtypDisplayType != DisplayTypeNormalDisplay) 
	{
		// Check if display type is always skip splash or program name is correct to skip
		if (mtypDisplayType == DisplayTypeSkipSplashDisplay || strFileName.CompareNoCase(mstrSkipProgramName) == 0) 
		{
			gLog.LogEvent("Running in setup only mode");
			setupOnlyMode = true;

			// If setup only, see if we should confirm (and make sure we aren't in continue mode
			CString setupConfirm = gUtils.GetINIString("Settings", "ConfirmSetupText", ""); 
			if (!setupConfirm.IsEmpty() && !cmdInfo.GetOption("continue"))
			{
				int result = MessageBox(NULL, setupConfirm, mstrAppName, MB_YESNO|MB_ICONQUESTION);
				if (result == IDNO)
				{
					gLog.LogEvent("User cancelled setup only mode.");
					return false;
				}
			}

		}
	}

	// Initialize dialog in case we need it below
	CDlgButton * button;
	CAutorunDlg dlg;
	dlg.Create(NULL);
	//dlg.ShowWindow(SW_HIDE);
	dlg.running = true;


	// Handle command line options
	if (cmdInfo.GetOption("dialog", strTemp)) 
	{ 
		button = dlg.FindButtonById(strTemp);
		if (button != NULL)
			gLog.LogEvent("Showing splash dialog since button '" + button->mstrName + "' completed");
	} 
	else if (cmdInfo.GetOption("continue", strTemp)) 
	{
		button = dlg.FindButtonById(strTemp);
		if (button != NULL)
		{
			gLog.LogEvent("Continuing system updates for button '" + button->mstrName + "'");
			continueButton	= strTemp;
			continueMode	= true;
		}
	}

	// Finally, we may want to simply use the default button
	if (mtypDisplayType == DisplayTypeSkipSplashDisplay) 
	{
		gLog.LogEvent("Skipping splash dialog and running default button.");
	}

	// Either start a button or just show the dialog
	if (setupOnlyMode) 
	{
		dlg.setupOnlyMode = true;
		button = dlg.FindDefaultButton();
		if (button != NULL)
			dlg.PerformButtonAction(button);
		else
			return false;
	}
	else if (continueMode)
	{
		button = dlg.FindButtonById(continueButton);
		if (button != NULL)
			dlg.PerformButtonAction(button);
		else
			return false;
	}
	else
	{
		dlg.ShowWindow(SW_SHOW);
	}
	
	while (true)
	{
		// process messages
		MSG msg;
		while ( ::PeekMessage( &msg, NULL, 0, 0, PM_NOREMOVE ) )  
		{ 
			if ( !AfxGetApp()->PumpMessage( ) ) 
			{ 
				::PostQuitMessage(0); 
				break; 
			} 
		} 

		// let MFC do its idle processing
		LONG lIdle = 0;
		while ( AfxGetApp()->OnIdle(lIdle++ ) )
			;

		if (!dlg.running) 
		{
			gLog.LogEvent("Dialog complete.");
			break;
		}
	}
	
	// Since the dialog has been closed, return FALSE so that we exit the
	//  application, rather than start the application's message pump.
	gLog.LogEvent("Application ending.");
	return true;
}


// Retreives basic settings from settings file
void CAutorunApp::LoadSettings() 
{

	// Start by getting basic settings
	mstrAppName			= gUtils.GetINIString("Settings", "ProgramName", mstrAppName);

	// Global settings
	mblnSingleInstance	= gUtils.GetINIBool("Settings", "SingleInstance", false);
	m_blnEnableLogging	= gUtils.GetINIBool("Settings", "EnableLogging", false);
	mstrAbortMutex		= gUtils.GetINIString("Settings", "AbortMutex", "");
	mstrSkipProgramName	= gUtils.GetINIString("Settings", "SkipProgramName", "");
	mtypDisplayType		= (DisplayType) gUtils.GetINIInt("Settings", "DisplayType", 0);

}


CString CAutorunApp::CurrentOS() 
{

	OSVERSIONINFO osv; CString temp;
	osv.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
	GetVersionEx(&osv);

	// Find out if WinNT or Win9x
	if (osv.dwPlatformId == VER_PLATFORM_WIN32_NT) 
	{
		temp.Format("WinNT: %d.%d.%d", osv.dwMajorVersion, osv.dwMinorVersion, osv.dwBuildNumber);
	} 
	else 
	{
		temp.Format("Win9x: %d.%d.%d", osv.dwMajorVersion, osv.dwMinorVersion, osv.dwBuildNumber);
	}

	return temp;
}
