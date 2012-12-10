// Autorun.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "Autorun.h"
#include "log.h"
#include "utilities.h"
#include "AutorunDlg.h"
#include "SetupDlg.h"
#include "util.h"

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

/////////////////////////////////////////////////////////////////////////////
// The one and only CAutorunApp object

CAutorunApp theApp;

/////////////////////////////////////////////////////////////////////////////
// CAutorunApp initialization

BOOL CAutorunApp::InitInstance()
{

	// Initialize things, get basic settings
	gLog.Init(true);
	gLog.LogEvent("Application: " + gUtils.EXEPath() + gUtils.EXEName());
	m_blnCancel = false;

	// make sure we have a valid settings.ini file
	CString strSettingsPath = gUtils.EXEPath() + "vbsw\\settings.ini";
	if (!gUtils.FileExists(strSettingsPath)) {
		gLog.LogEvent("No settings.ini found at '" + strSettingsPath + "'.  Program terminated.");
		CString strErrMsg; strErrMsg.Format(IDS_NOSETTINGS, strSettingsPath);
		AfxMessageBox(strErrMsg, MB_ICONSTOP);
		return false;
	}

	// load basic settings
	LoadSettings();

	// check if this is setup.exe, if so skip dialog
	bool bIsSetup = false;
	CString strFileName = gUtils.EXEName(); 
	if (strFileName.CompareNoCase("setup.exe") == 0) {
		bIsSetup = true;
		gLog.LogEvent("Running in setup only mode");
	}

	// if no command line options, show dialog allowing cancel  (options are passed by RunOnce)
	if (!bIsSetup && (m_lpCmdLine[0] == '\0')) {
		gLog.LogEvent("Displaying autorun dialog.");
		CAutorunDlg dlg;
		int nResponse = dlg.DoModal();
		if (nResponse != IDOK) {
			gLog.LogEvent("User hit cancel on autorun dialog.");
			return false;
		}
	}

	// check system updates
	if (SysUpdates()) {

		// now install components
		if (InstallComponents()) {
			// now run setup
			gLog.LogEvent("All components installed, starting setup/msi.");
			CString sSetupCommand = gUtils.EXEPath() + mstrSetup;
			gUtils.Exec(sSetupCommand, mstrCmdLine);
		} else {
			// check if cancelled, otherwise we need to reboot
			if (m_blnCancel) {
				gLog.LogEvent("Setup cancelled, application exiting.");
				return false;
			}
			// InstallComponents returned false, and we didn't cancel, so we must reboot
			if (mblnRebootComputer)
				RebootComputer("/continue");
			else
				gLog.LogEvent("SysUpdates returned false, possible error.");
		}
	}

	// delete whole list since we are done with it
	CComponent * pobjComp;
	while (mlstComps.GetCount() > 0) {
		pobjComp = mlstComps.RemoveHead();
		delete pobjComp;
	}
	
	// Since the dialog has been closed, return FALSE so that we exit the
	//  application, rather than start the application's message pump.
	gLog.LogEvent("Application ending.");
	return false;
}


// Process components in settings.ini, check for req'd components
bool CAutorunApp::SysUpdates() {

	int intResult = 0;

	// Read components, creating list
	LPCTSTR lpAppName; TCHAR * lpReturnedString; 
	lpReturnedString = (TCHAR*) malloc(1000);

	CString lpFileName = gUtils.EXEPath() + "vbsw\\settings.ini";
	lpAppName = "Settings";
	
	// NOTE: must have called a get string before this due to win95/98 bug, Q198906
	GetPrivateProfileSection("Components", lpReturnedString, 255, lpFileName);

	// now break up the string of components
	CComponent * pobjComp; CString strCompName; CString strTemp;
	TCHAR * lpTemp = lpReturnedString;
	while (lpTemp != NULL) {
		strCompName = lpTemp;
		// check if component enabled
		strTemp = strCompName.Mid(strCompName.Find("="),1);
		if (strTemp.CompareNoCase("1")) {
			if (strCompName.Right(1).CompareNoCase("0") == 0) {
				 // skip this component
			} else {
				strCompName = strCompName.Left(strCompName.Find("="));
				pobjComp = new CComponent;
				if (pobjComp->LoadComponent(strCompName, lpFileName))
					mlstComps.AddTail(pobjComp);
			}
			while (*lpTemp != '\0')		// advance to next section in string
				lpTemp++;
			if (*lpTemp == '\0')			// new sections
				lpTemp++;
			if (*lpTemp == '\0')			// if at second null in a row, done
				break;
		}	// end if enabled
	}	// end while

	// Free allocated strings
	free(lpReturnedString);

	// now go though components
	int i; 
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
			gLog.LogEvent(pobjComp->mstrId + ": Component install already attempted.  Prompting user.");
			strCompMsg.Format(IDS_COMPABORTRETRY, pobjComp->mstrName);
			intResult = MessageBox(NULL, strCompMsg, mstrAppName, MB_ABORTRETRYIGNORE | MB_ICONQUESTION | MB_TASKMODAL);
			if (intResult == IDABORT)	{						// abort everything
				gLog.LogEvent(pobjComp->mstrId + ": Aborting installation at user request.");
				return false;
			} else if (intResult == IDIGNORE) {			// we don't care prev install failed, go on
				gLog.LogEvent(pobjComp->mstrId + ": Ignoring component; will not attempt to install again.");
				pobjComp->mblnInstall = false;
			}	else if (intResult ==  IDRETRY)
				gLog.LogEvent(pobjComp->mstrId + ": Retrying component install.");
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
			gLog.LogEvent(pobjComp->mstrId + ": Component on final list to install this run.");
	}

	return true;
}


// Retreives basic settings from settings file
void CAutorunApp::LoadSettings() {

	TCHAR * lpReturnedString; 
	lpReturnedString = (TCHAR*) malloc(1000);

	// Start by getting basic settings
	GetPrivateProfileString("Settings", "ProgramName", mstrAppName, lpReturnedString, 255, gUtils.EXEPath() + "vbsw\\settings.ini");
	mstrAppName = lpReturnedString;
	gUtils.m_strAppName = mstrAppName;

	GetPrivateProfileString("Settings", "Setup", "setup\\setup.exe", lpReturnedString, 255, gUtils.EXEPath() + "vbsw\\settings.ini");
	mstrSetup   = lpReturnedString;

	GetPrivateProfileString("Settings", "CmdLine", "", lpReturnedString, 255, gUtils.EXEPath() + "vbsw\\settings.ini");
	mstrCmdLine = lpReturnedString;

		// Free allocated strings
	free(lpReturnedString);

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
				gLog.LogEvent(pobjComp->mstrId + ": Dependency '" + pobjTemp->mstrId + "' not installed.");
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
			gLog.LogEvent(pobjComp->mstrId + ": Component installation started: " + pobjComp->mstrSetupCommand);

			// refresh dialog with current status
			sDlg->m_CurStatus = pobjComp->mstrSetupMessage;
			sDlg->UpdateData(false);
			
			if (!pobjComp->Install(sDlg) || sDlg->m_blnCancel) {
				m_blnCancel = true;
				return false;
			}
			if (pobjComp->mtypReboot != NoReboot) {
				mblnRebootComputer = true;
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
		gUtils.LogDLLError("Restart VBSW");
	}
	gLog.LogEvent("VBSW set to resume on next startup.");

	CRestartDlg dlg;
	dlg.m_strAppName = mstrAppName;
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