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
	// delete whole list since we are done with it
	CComponent * pobjComp;
	while (mlstComps.GetCount() > 0) {
		pobjComp = mlstComps.RemoveHead();
		delete pobjComp;
	}

	// delete buttons too
	CDlgButton * pobjButton;
	while (mlstButtons.GetCount() > 0) {
		pobjButton = mlstButtons.RemoveHead();
		delete pobjButton;
	}
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
	if (!gUtils.FileExists(strSettingsPath)) {
		CString strErrMsg; strErrMsg.Format(IDS_NOSETTINGS, strSettingsPath);
		AfxMessageBox(strErrMsg, MB_ICONSTOP);
		return false;
	}

	// load basic settings
	LoadSettings();

	// Check OS
	CString result;
	if (!gUtils.ValidateOS("RequiredOS", result)) {
		gLog.LogEvent("OS Validation returned: " + result);
		result = gUtils.GetINIString("RequiredOS", "InvalidOSMessage", result);
		AfxMessageBox(result, MB_ICONSTOP);
		return false;
	}

	// Start the log
	gLog.Init(cmdInfo.GetOption("log") || m_blnEnableLogging);
	gLog.LogEvent("Application: " + gUtils.EXEPath() + gUtils.EXEName());

	// Check if we care about single instance
	if (mblnSingleInstance) {

		// Check if any other instances before setting a mutex
		HANDLE hMutex = OpenMutex(MUTEX_ALL_ACCESS, FALSE, "ia3_mutex");
		if (hMutex != NULL && mblnSingleInstance) {
			gLog.LogEvent("Another instance of this program is already running, and 'single instance' has been set.  Exiting.");
			return false;
		}

		// No other instance is running, so set a mutex
		CreateMutex(NULL, false, "ia3_mutex");
	}

	// Check if we care about a user defined mutex
	if (mstrAbortMutex != "") {

		if (OpenMutex(MUTEX_ALL_ACCESS, FALSE, mstrAbortMutex) != NULL ) {
			gLog.LogEvent("The user defined mutex '" + mstrAbortMutex + "' was found.  Exiting.");
			return false;
		}
	}

	bool bIsSetup = false;
	bool showDialog = true;
	CDlgButton* pButton = NULL;
	CString strTemp;

	// Initialize dialog in case we need it below
	CAutorunDlg dlg;
	dlg.LoadButtons(&mlstButtons);

	// check if this is setup.exe, if so skip dialog
	CString strFileName = gUtils.EXEName(); 
	if (mtypDisplayType != DisplayTypeNormalDisplay) {
		if (mtypDisplayType == DisplayTypeSkipSplashDisplay || strFileName.CompareNoCase(mstrSkipProgramName) == 0) {
			bIsSetup = true;
			pButton = dlg.FindDefaultButton();
			gLog.LogEvent("Running in setup only mode");
		}
	}

	// Handle command line options
	if (cmdInfo.GetOption("dialog", strTemp)) { 
		gLog.LogEvent("Showing splash dialog since button '" + strTemp + "' completed");
	} else if (cmdInfo.GetOption("continue", strTemp)) {
		gLog.LogEvent("Continuing system updates for button '" + strTemp + "'");
		pButton = dlg.FindButtonById(strTemp);
	}

	// Finally, we may want to simply use the default button
	if (mtypDisplayType == DisplayTypeSkipSplashDisplay) {
		gLog.LogEvent("Skipping splash dialog and running default button.");
	}

	// loop until we want to stop showing the dialog
	while (showDialog) {

		// Show the dialog if we don't already have a button selected
		if (pButton == NULL) {
		
			gLog.LogEvent("Displaying autorun dialog.");
			int nResponse = dlg.DoModal();
		
			// Check if user hit a cancel button
			if (nResponse != IDOK) {
				gLog.LogEvent("User hit cancel on autorun dialog.");
				return false;
			}

			// No cancel, so check what we should do now
			pButton = dlg.selectedButton;

		}

		// If we have a null button, get the default button
		if (pButton == NULL)
			pButton = dlg.FindDefaultButton();

		// If no default button, abort
		if (pButton == NULL)
			return false;

		// See if we need to do system updates
		if (pButton->mblnComponentCheck) {
			
			// check what is installed
			if (SysUpdates()) {

				// Try installing components
				if (!InstallComponents()) {
					
					// check if user clicked cancel - if not, reboot needed
					if (m_blnComponentCancel) {
						gLog.LogEvent("Setup cancelled, application exiting.");
						return false;
					} else if (m_blnComponentRebootComputer) {
						RebootComputer("/continue " + pButton->mstrId); 
						return false;
					} else {
						// InstallComponents returned false, and we didn't cancel, so we must reboot
						gLog.LogEvent("SysUpdates returned false, possible error.");
					}
				}

				// Any system updates have now been installed
				gLog.LogEvent("All system updates installed.");
			}
		}	// done mblnComponentCheck

		// Perform button action
		CString sSetupCommand;
		bool async = (pButton->mtypDialogAction == DialogActionShowWhenActionComplete);

		if (!async)
	 		gUtils.PlaySoundFile(gUtils.GetINIString("Settings", "OnCloseSound", ""));

		switch (pButton->mtypDlgButtonType) {

			case DlgButtonTypeRunProgram:
				
				// replace any sys path, vars
				gUtils.ReplaceDirs(pButton->mstrSetupCommand);
				gUtils.ReplaceDirs(pButton->mstrSetupCommandLine);

				// check if file exists without adding exe path
				if (gUtils.FileExists(pButton->mstrSetupCommand)) {
					// start the program
					gUtils.Exec(pButton->mstrSetupCommand, pButton->mstrSetupCommandLine, async, false);
				} else {
					// start the program
					gUtils.Exec(gUtils.EXEPath() + pButton->mstrSetupCommand, pButton->mstrSetupCommandLine, async, false);
				}

				// Check if we should restart
				if (pButton->mblnRestartPrompt) {
					RebootComputer("/dialog " + pButton->mstrId);
					return false;
				}
				break;

			case DlgButtonTypeLaunchUrl:
				ShellExecute(NULL, "open", pButton->mstrUrl, NULL, NULL, SW_SHOWNORMAL);
				break;

			case DlgButtonTypeShellExecute:
				if (gUtils.FileExists(pButton->mstrFile)) {
					ShellExecute(NULL, "open", pButton->mstrFile, NULL, NULL, SW_SHOWNORMAL);
				} else if (gUtils.FileExists(gUtils.EXEPath() + pButton->mstrFile)) {
					ShellExecute(NULL, "open", gUtils.EXEPath() + pButton->mstrFile, NULL, NULL, SW_SHOWNORMAL);
				} else {
					gLog.LogEvent("The file '" + pButton->mstrFile + "' could not be found.");
					AfxMessageBox("The file '" + pButton->mstrFile + "' could not be found.", MB_ICONERROR);
				}
				break;

			default:
				gLog.LogEvent("Unknown button type.");
				break;
		}


		// Check if we want to show the dialog again, and reset selected button
		showDialog = (pButton->mtypDialogAction != DialogActionDoNotShowDialog);
		pButton = NULL;

	}

	// Since the dialog has been closed, return FALSE so that we exit the
	//  application, rather than start the application's message pump.
	gLog.LogEvent("Application ending.");
	return false;
}


// Process components in settings.ini, check for req'd components
bool CAutorunApp::SysUpdates() {

	int intResult = 0;

	gLog.LogEvent("OS: " + CurrentOS());

	int i; CComponent * pobjComp;
	CString componentList[20][2];
	int componentCount;

	componentCount = gUtils.GetINISection("Components", componentList);

	// Loop through list, adding enabled components
	for (i = 0; i < componentCount; i++) 
	{
		if (componentList[i][1] == "1") 
		{
			pobjComp = new CComponent();
			if (pobjComp->Load(componentList[i][0]))
				mlstComps.AddTail(pobjComp);
		}
	}


	// now go though components
	POSITION pos = mlstComps.GetHeadPosition();
	for (i = 0; i < mlstComps.GetCount(); i++) {
		pobjComp = mlstComps.GetNext(pos);

		// see if installed
		if (pobjComp->mblnInstall) {
			pobjComp->CheckComponent();
			if (pobjComp->IsComponentInstalled()) {
				// component is installed, flag to not install
				pobjComp->mblnInstall = false;
			}
		}
	}

	gLog.LogEvent("Components all checked, evaluating whether to install...");

	// look through list to see if we attempted to install any of them
	CString strCompMsg;
	pos = mlstComps.GetHeadPosition();
	for (i = 0; i < mlstComps.GetCount(); i++) {
		pobjComp = mlstComps.GetNext(pos);
		if (pobjComp->mblnInstall && pobjComp->InstallAttempted()) {
			// we have tried to install this component already
			gLog.LogEvent(pobjComp->mstrName + ": Component install already attempted.  Prompting user.");
			strCompMsg.Format(IDS_COMPABORTRETRY, pobjComp->mstrName);
			intResult = MessageBox(NULL, strCompMsg, mstrAppName, MB_ABORTRETRYIGNORE | MB_ICONQUESTION | MB_TASKMODAL);
			if (intResult == IDABORT)	{						// abort everything
				gLog.LogEvent(pobjComp->mstrName + ": Aborting installation at user request.");
				return false;
			} else if (intResult == IDIGNORE) {			// we don't care prev install failed, go on
				gLog.LogEvent(pobjComp->mstrName + ": Ignoring component; will not attempt to install again.");
				pobjComp->mblnInstall = false;
			}	else if (intResult ==  IDRETRY)
				gLog.LogEvent(pobjComp->mstrName + ": Retrying component install.");
		}
	}

	// we now have a list of components to install, check depends first though
	pos = mlstComps.GetHeadPosition();
	for (i = 0; i < mlstComps.GetCount(); i++) {
		
		pobjComp = mlstComps.GetNext(pos);

		// check if any depends are not installed
		if (pobjComp->mblnInstall && DependsInstalled(pobjComp)) {
			if (pobjComp->mtypReboot == ImmediateReboot) // we only want to install this component
				break;
		} else {
			// if depends not installed, we want to skip this component
			pobjComp->mblnInstall = false;
		}
	}

	// see if we have any immediate reboot items, end list there
	bool blnCutList = false;
	pos = mlstComps.GetHeadPosition();
	for (i = 0; i < mlstComps.GetCount(); i++) {
		pobjComp = mlstComps.GetNext(pos);
		if (pobjComp->mblnInstall) {
			if (blnCutList) 
				pobjComp->mblnInstall = false;
			else if (pobjComp->mtypReboot == ImmediateReboot)
				blnCutList = true;
		}
	}

	// debug: output the components to install this run
	pos = mlstComps.GetHeadPosition();
	for (i = 0; i < mlstComps.GetCount(); i++) {
		pobjComp = mlstComps.GetNext(pos);
		if (pobjComp->mblnInstall)
			gLog.LogEvent(pobjComp->mstrName + ": Component on final list to install this run.");
	}

	return true;
}


// Retreives basic settings from settings file
void CAutorunApp::LoadSettings() {

	// Start by getting basic settings
	mstrAppName			= gUtils.GetINIString("Settings", "ProgramName", mstrAppName);

	// Reboot settings
	mblnTimerReboot		= gUtils.GetINIBool("Settings", "RebootPromptType", true);
	mintTimerSeconds	= gUtils.GetINIInt("Settings", "RebootPromptSeconds", 20);

	// Global settings
	mblnSingleInstance	= gUtils.GetINIBool("Settings", "SingleInstance", false);
	m_blnEnableLogging	= gUtils.GetINIBool("Settings", "EnableLogging", false);
	mstrAbortMutex		= gUtils.GetINIString("Settings", "AbortMutex", "");
	mstrSkipProgramName	= gUtils.GetINIString("Settings", "SkipProgramName", "");
	mtypDisplayType		= (DisplayType) gUtils.GetINIInt("Settings", "DisplayType", 0);

	// Splash settings
	

	// Load buttons
	// NOTE: must have called a get string before this due to win95/98 bug, Q198906
	
	CString buttonList[20][2];
	int buttonCount;

	buttonCount = gUtils.GetINISection("Buttons", buttonList);

	// Loop through list, adding enabled buttons
	for (int i = 0; i < buttonCount; i++) 
	{
		if (buttonList[i][1] == "1") 
		{
			CDlgButton * button = new CDlgButton();
			if (button->Load(buttonList[i][0]))
				mlstButtons.AddTail(button);
		}
	}


}

bool CAutorunApp::DependsInstalled(CComponent *pobjComp)
{

	POSITION lstPos; CString strDependId; CComponent * pobjTemp;

	// go through the depends for object pobjComp
	POSITION pos = pobjComp->mlstDepends.GetHeadPosition();
	for (int i = 0; i < pobjComp->mlstDepends.GetCount(); i++) {
		strDependId = pobjComp->mlstDepends.GetNext(pos);

		// look through list to see if any depends are in list of to install stuff
 		lstPos = mlstComps.GetHeadPosition();
		for (int j = 0; j < mlstComps.GetCount(); j++) {
			pobjTemp = mlstComps.GetNext(lstPos);
			if (pobjTemp->mblnInstall && pobjTemp->mstrId.CompareNoCase(strDependId) == 0) {
				// we have a depend in the list that is not installed
				gLog.LogEvent(pobjComp->mstrName + ": Dependency '" + pobjTemp->mstrName + "' not installed.");
				return false;
			}
		}	// end looping through install list
	}		// end looping through depends list

	return true;	// all depends are installed
}

bool CAutorunApp::InstallComponents()
{

	int intTotalTime = 0, i;
	POSITION pos;
	CComponent * pobjComp;
	bool blnReturnValue = true;

	// Count total time
	pos = mlstComps.GetHeadPosition();
	for (i = 0; i < mlstComps.GetCount(); i++) {
		pobjComp = mlstComps.GetNext(pos);
		if (pobjComp->mblnInstall)
			intTotalTime += pobjComp->InstallTime();
	}

	// set global flag appropriately
	m_blnComponentCancel = false;

	// make sure there are components to install
	if (intTotalTime == 0)
		return true;

	// Init setup dialog
	CSetupDlg * sDlg;
	sDlg = new CSetupDlg;
	CWnd * hw;
	hw = sDlg;
	sDlg->m_strAppName = mstrAppName;
	sDlg->Create(NULL);
	sDlg->ShowWindow(SW_SHOW);
	sDlg->UpdateWindow();
	sDlg->SetMaxProgress(intTotalTime*2);

	// now go through list installing components
	pos = mlstComps.GetHeadPosition();
	for (i = 0; i < mlstComps.GetCount(); i++) {
		pobjComp = mlstComps.GetNext(pos);
		
		if (pobjComp->mblnInstall) {
			gLog.LogEvent(pobjComp->mstrName + ": Component installation started: " + pobjComp->mstrSetupCommand);

			// refresh dialog with current status
			sDlg->m_CurStatus = pobjComp->mstrSetupMessage;
			sDlg->UpdateData(false);
			
			if (!pobjComp->Install(sDlg) || sDlg->m_blnCancel) {
				m_blnComponentCancel = true;
				return false;
			}
			if (pobjComp->mtypReboot != NoReboot) {
				m_blnComponentRebootComputer = true;
				blnReturnValue = false;
			}
		}
	}


	// Destroy setup dialog
	sDlg->DestroyWindow();
	delete sDlg;
	return blnReturnValue;

}

bool CAutorunApp::RebootComputer(CString strCmdLine) {


	// set RunOnce key
	char chTemp[MAX_PATH];
	GetModuleFileName(NULL, chTemp, MAX_PATH);
	strCmdLine = " " + strCmdLine;
	strcat(chTemp, strCmdLine);
	long h; HKEY hRegKey; LPDWORD hResult = 0;
	LPCTSTR sKey = "Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce";	
	h = RegCreateKeyEx(HKEY_CURRENT_USER, sKey,0,NULL,REG_OPTION_NON_VOLATILE, KEY_SET_VALUE, NULL, &hRegKey, hResult);
	if (h == ERROR_SUCCESS) {
		h = RegSetValueEx(hRegKey, "VB Setup Program Autorun", 0, REG_SZ, (byte*)&chTemp, strlen(chTemp) + 1);
		h = RegCloseKey(hRegKey);
	} else {
		gUtils.LogDLLError("Restart IA");
	}
	gLog.LogEvent("IA set to resume on next startup.");

	CRestartDlg dlg;
	dlg.m_strAppName = mstrAppName;
	dlg.mblnTimerReboot = mblnTimerReboot;
	dlg.mintTimerSeconds = mintTimerSeconds;

	if (dlg.DoModal() == IDOK) {
		// we want to reboot, but first adjust process privs
		gLog.LogEvent("Restarting computer...");
		HANDLE hToken; TOKEN_PRIVILEGES tkp;
		OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, &hToken);
		LookupPrivilegeValue(NULL, SE_SHUTDOWN_NAME, &tkp.Privileges[0].Luid);
		tkp.PrivilegeCount = 1;   // one priv to set
		tkp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
		AdjustTokenPrivileges(hToken, FALSE, &tkp, 0, (PTOKEN_PRIVILEGES)NULL, 0);
		ExitWindowsEx(EWX_REBOOT, 0);
	}
	return true;
}

CString CAutorunApp::CurrentOS() {

	OSVERSIONINFO osv; CString temp;
	char * tempVersion;
	tempVersion = (char*) malloc(25);
	osv.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
	GetVersionEx(&osv);

	// Find out if WinNT or Win9x
	if (osv.dwPlatformId == VER_PLATFORM_WIN32_NT) {
		temp = "WinNT: ";
	} else {
		temp = "Win9x: ";
	}

	// Append the actual version
	itoa(osv.dwMajorVersion, tempVersion, 25);
	temp = temp + tempVersion + ".";
	free(tempVersion);
	tempVersion = (char*) malloc(25);
	itoa(osv.dwMinorVersion, tempVersion, 25);
	temp = temp + tempVersion;
	free(tempVersion);
	tempVersion = (char*) malloc(25);
	itoa(osv.dwBuildNumber, tempVersion, 25);
	temp = temp + "." + tempVersion;
	
	// return the string
	free(tempVersion);
	return temp;
}
