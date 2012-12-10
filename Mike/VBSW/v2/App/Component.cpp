// Component.cpp: implementation of the CComponent class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "autorun.h"
#include "Component.h"
#include "utilities.h"
#include "version.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CComponent::CComponent()
{
}

CComponent::~CComponent()
{
}

CComponent::CComponent(const CComponent &src) {
	this->mstrName = src.mstrName;
}

CComponent CComponent::operator =(CComponent &rhs) {
	
	mblnOSVersionNT = rhs.mblnOSVersionNT;
	mblnOSVersion9x = rhs.mblnOSVersion9x;
	mblnQuietInstall = rhs.mblnQuietInstall;
	mstrNTServicePackCheckNumber = rhs.mstrNTServicePackCheckNumber;
	mintSetupTime = rhs.mintSetupTime;
	mblnInstall = rhs.mblnInstall;

	POSITION pos = rhs.mlstDepends.GetHeadPosition();
	for (int i = 0; i < rhs.mlstDepends.GetCount(); i++) 
		mlstDepends.AddTail(rhs.mlstDepends.GetNext(pos));

	CString strTemp = rhs.mstrId + " Copy: ";
	pos = mlstDepends.GetHeadPosition();
	for (i = 0; i < mlstDepends.GetCount(); i++) 
		strTemp = strTemp + " " + mlstDepends.GetNext(pos);

	mstrFileVersionCheckDLL = rhs.mstrFileVersionCheckDLL;
	mstrFileVersionCheckVersion = rhs.mstrFileVersionCheckVersion;
	mstrId = rhs.mstrId;
	mstrName = rhs.mstrName;
	mstrOSVersionMax = rhs.mstrOSVersionMax;
	mstrOSVersionMin = rhs.mstrOSVersionMin;
	mstrRegKeyCheckKey = rhs.mstrRegKeyCheckKey;
	mstrRegKeyCheckValue = rhs.mstrRegKeyCheckValue;
	mstrRegVersionCheckKey = rhs.mstrRegVersionCheckKey;
	mstrRegVersionCheckVersion = rhs.mstrRegVersionCheckVersion;
	mstrSetupCommand = rhs.mstrSetupCommand;
	mstrSetupCommandLine = rhs.mstrSetupCommandLine;
	mstrSetupMessage = rhs.mstrSetupMessage;
	mtypComponentType = rhs.mtypComponentType;
	mtypReboot = rhs.mtypReboot;
	return *this;

}

//////////////////////////////////////////////////////////////////////
// Member Functions
//////////////////////////////////////////////////////////////////////

// Loads settings for the current component into this class object
bool CComponent::LoadComponent(CString sCompName, CString sFileName) {

	// init vars
	mblnInstall = false;

	CString sTemp;
	TCHAR * lpReturnedString;
	DWORD nSize = 1500;
	lpReturnedString = (TCHAR*) malloc(nSize);
	int intTemp;

	intTemp = GetPrivateProfileInt(sCompName, "OSVersion9x", 1, sFileName);
	if (intTemp == 1)
		mblnOSVersion9x = true;
	else
		mblnOSVersion9x = false;
	intTemp = GetPrivateProfileInt(sCompName, "OSVersionNT", 1, sFileName);
	if (intTemp == 1)
		mblnOSVersionNT = true;
	else
		mblnOSVersionNT = false;

	// Set component ID
	mstrId = sCompName;

	// Get basic settings from INI file
	GetPrivateProfileString(sCompName, "Name", "", lpReturnedString, nSize, sFileName);
	mstrName = lpReturnedString;
	if (mstrName.IsEmpty()) {
		gLog.LogEvent("No name specified for component '" + mstrId + "'.  Ignoring component.");
//		AfxMessageBox("There was an error reading the settings file (" + sFileName + ") for ID: " + mstrId); 
		// TODO: Remove above line.
		free(lpReturnedString);
		return false;
	}
	
	// Get OS requirements
	GetPrivateProfileString(sCompName, "OSVersionMin", "0.00", lpReturnedString, nSize, sFileName);
	mstrOSVersionMin = lpReturnedString;
	GetPrivateProfileString(sCompName, "OSVersionMax", "0.00", lpReturnedString, nSize, sFileName);
	mstrOSVersionMax = lpReturnedString;
	

	// parse depends list
	GetPrivateProfileString(sCompName, "Dependencies", "", lpReturnedString, nSize, sFileName);
	sTemp = lpReturnedString;
	while (sTemp.GetLength() > 0) {
		if (sTemp.Find(" ") > 0)  {
			mlstDepends.AddTail(sTemp.Left(sTemp.Find(" ")));
			sTemp = sTemp.Mid(sTemp.Find(" ")+1);
		} else {
			mlstDepends.AddTail(sTemp);
			sTemp = "";
		}
	}
	GetPrivateProfileString(sCompName, "SetupCommand", "", lpReturnedString, nSize, sFileName);
	mstrSetupCommand = lpReturnedString;
	mstrSetupCommand = gUtils.EXEPath() + mstrSetupCommand;
	GetPrivateProfileString(sCompName, "SetupCommandLine", "", lpReturnedString, nSize, sFileName);
	mstrSetupCommandLine = lpReturnedString;
	mintSetupTime = GetPrivateProfileInt(sCompName, "SetupTime", 30, sFileName);
	GetPrivateProfileString(sCompName, "SetupMessage", "Installing...", lpReturnedString, nSize, sFileName);
	mstrSetupMessage = lpReturnedString;
	if (GetPrivateProfileInt(sCompName, "QuietInstall", 1, sFileName) == 1)
		mblnQuietInstall = true;
	else
		mblnQuietInstall = false;

	// Get reboot setting, assume none
	mtypReboot = (RebootType) GetPrivateProfileInt(sCompName, "RebootType", 0, sFileName);

	// Get type of component and then associated settings
	GetPrivateProfileString(sCompName, "Type", "", lpReturnedString, nSize, sFileName);
	sTemp = lpReturnedString;
	mtypComponentType = (ComponentType) GetPrivateProfileInt(sCompName, "ComponentType", 0, sFileName);
	switch (mtypComponentType) {
		case FileVersionCheck:
			GetPrivateProfileString(sCompName, "FileVersionCheckDLL", "", lpReturnedString, nSize, sFileName);
			mstrFileVersionCheckDLL = lpReturnedString;
			ReplaceDirs(mstrFileVersionCheckDLL);
			GetPrivateProfileString(sCompName, "FileVersionCheckVersion", "", lpReturnedString, nSize, sFileName);
			mstrFileVersionCheckVersion = lpReturnedString;
			break;
		case RegVersionCheck:
			GetPrivateProfileString(sCompName, "RegVersionCheckKey", "", lpReturnedString, nSize, sFileName);
			mstrRegVersionCheckKey = lpReturnedString;
			GetPrivateProfileString(sCompName, "RegVersionCheckVersion", "", lpReturnedString, nSize, sFileName);
			mstrRegVersionCheckVersion = lpReturnedString;
			break;
		case RegKeyCheck:
			GetPrivateProfileString(sCompName, "RegKeyCheckKey", "", lpReturnedString, nSize, sFileName);
			mstrRegKeyCheckKey = lpReturnedString;
			GetPrivateProfileString(sCompName, "RegKeyCheckValue", "", lpReturnedString, nSize, sFileName);
			mstrRegKeyCheckValue = lpReturnedString;
			break;
		case NTServicePackCheck:
			GetPrivateProfileString(sCompName, "NTServicePackCheckNumber", "", lpReturnedString, nSize, sFileName);
			mstrNTServicePackCheckNumber = lpReturnedString;
			break;
		default:
			gLog.LogEvent(mstrId + ": Invalid/no check type specified, ignoring component.");
			free(lpReturnedString);
			return false;
	}

	free(lpReturnedString);
	mblnInstall = true;
	return true;
}


int  CComponent::InstallTime() {
	return mintSetupTime;
}


bool CComponent::IsComponentInstalled()
{
	return mblnInstalled;
}

void CComponent::CheckComponent()
{
	// assume installed from beginning
	mblnInstalled = true;

	// first check OS to see if we are matching the OS
	if (!IsCorrectOS()) {
		gLog.LogEvent(mstrId + ": This component doesn't match OS requirements, skipping.");
		return;
	}

	// now check the appropriate component check
	switch (mtypComponentType) {
		case FileVersionCheck:
			mblnInstalled = RunFileVersionCheck();
			break;
		case RegVersionCheck:
			mblnInstalled = RunRegVersionCheck();
			break;
		case RegKeyCheck:
			mblnInstalled = RunRegKeyCheck();
			break;
		case NTServicePackCheck:
			mblnInstalled = RunNTServicePackCheck();
			break;
		default:
			return;
	}
	
}

bool CComponent::IsCorrectOS()
{
	OSVERSIONINFO osv; DWORD dwMajor; DWORD dwMinor;
	osv.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
	GetVersionEx(&osv);

	// first check if we want only Win9x or only WinNT
	if (!mblnOSVersionNT && !mblnOSVersion9x)
		return false;
	// see if only NT reqd, but we aren't running nt
	if (mblnOSVersionNT && !mblnOSVersion9x && osv.dwPlatformId != VER_PLATFORM_WIN32_NT)
		return false;
	// see if only 9x reqd, but we aren't running 9x
	if (mblnOSVersion9x && !mblnOSVersionNT && osv.dwPlatformId != VER_PLATFORM_WIN32_WINDOWS)
		return false;

	// now check version info if we need to
	if (mstrOSVersionMin.CompareNoCase("0") == 0 || mstrOSVersionMin.Find("0.") >= 0 ||
			mstrOSVersionMax.CompareNoCase("0") == 0 || mstrOSVersionMax.Find("0.") >= 0)
		return true;		// this means we should check this component

	// now check actual version numbers
	dwMajor = atoi(mstrOSVersionMin.Left(mstrOSVersionMin.Find(".")));
	dwMinor = atoi(mstrOSVersionMin.Mid(mstrOSVersionMin.Find(".")));
	if (osv.dwMajorVersion < dwMajor || 
			(osv.dwMajorVersion == dwMajor && osv.dwMinorVersion < dwMinor))
		return false;			// this OS is older then reqd

	dwMajor = atoi(mstrOSVersionMax.Left(mstrOSVersionMin.Find(".")));
	dwMinor = atoi(mstrOSVersionMax.Mid(mstrOSVersionMin.Find(".")));
	if (osv.dwMajorVersion > dwMajor || 
			(osv.dwMajorVersion == dwMajor && osv.dwMinorVersion > dwMinor))
		return false;			// this OS is newer then reqd

	// we have an OS between versions and such
	return true;
}

bool CComponent::RunFileVersionCheck()
{

	DWORD dwMajorVersion=0, dwMinorVersion=0, dwBuildNumber=0, dwRevision=0;
	VERSION_INFO vinfo; CFileVersion fv;

	// Check if file exists, if not, assume not installed
	if (!gUtils.FileExists(mstrFileVersionCheckDLL)) {
		gLog.LogEvent(mstrId + ": File " + mstrFileVersionCheckDLL + " not found.");
		return false;
	}

	// Try to open the version info on the DLL
	if (!fv.Open(mstrFileVersionCheckDLL)) {
		gLog.LogEvent(mstrId + ": Failed checking '" + mstrFileVersionCheckDLL + "' version info");
		return false;
	}
	vinfo = fv.GetFixedFileVersionInfo();

	// Split the version info string from config
	SplitVersionString(mstrFileVersionCheckVersion, dwMajorVersion, 
		dwMinorVersion, dwBuildNumber, dwRevision);

	// Log what versions were found
	gLog.LogEvent(mstrId + ": File: " + mstrFileVersionCheckDLL + ", version: " 
			+ fv.GetFixedFileVersion() + "; required: " + mstrFileVersionCheckVersion);

	if (vinfo.dwMajorVerison > dwMajorVersion)   // dll is newer than dwMajorVersion
		return true;
	else if (vinfo.dwMajorVerison < dwMajorVersion) // dll is older than dwMajorVersion
		return false;
	else {  // dll is dwMajorVersion
		if (vinfo.dwMinorVersion > dwMinorVersion) // newer than dwMinorVersion
			return true;
		else if (vinfo.dwMinorVersion < dwMinorVersion) // older than dwMinorVersion
			return false;
		else { // dwMajorVersion.dwMinorVersion
			if (vinfo.dwBuildNumber > dwBuildNumber) // newer than dwBuildNumber
				return true;
			else if (vinfo.dwBuildNumber < dwBuildNumber) // older than dwBuildNumber
				return false;
			else { // dwMajorVersion.dwMinorVersion.dwBuildNumber
				if (vinfo.dwRevision > dwRevision) // newer than dwRevision
					return true;
				else if (vinfo.dwRevision < dwRevision) // older than dwRevision
					return false;
				else  // file is correct version
					return true;
			}		// end dwBuildNumber
		}			// end dwMinorVersion
	}       // end dwMajorVersion
	
	return true;
}

void CComponent::ReplaceDirs(CString &p_strIn)
{
	// check for directory macros
	if (p_strIn.Find("%WinSysPath%") >= 0)
		p_strIn.Replace("%WinSysPath%", gUtils.WinSysPath());
	if (p_strIn.Find("%WinPath%") >= 0)
		p_strIn.Replace("%WinPath%", gUtils.WinPath());
	if (p_strIn.Find("%CommonFilesPath%") >= 0)
		p_strIn.Replace("%CommonFilesPath%", gUtils.CommonFilesPath());
}





void CComponent::SplitVersionString(CString p_strVersion, DWORD &dwMajorVersion, DWORD &dwMinorVersion, DWORD &dwBuildNumber, DWORD &dwRevision)
{

	int iTemp; 
	CString strSplitChar, strTemp;
	strTemp = p_strVersion;

	// figure out what char was used to split version string
	if (p_strVersion.Find(",") > 0)
		strSplitChar = ",";
	else if (p_strVersion.Find(".") > 0)
		strSplitChar = ".";
	else
		return;

	// loop while still finding chars
	for (int iLoop = 0; iLoop < 4; iLoop++) {
		iTemp = atoi(strTemp.Left(strTemp.Find(strSplitChar)));
		if (iTemp == 0)
			iTemp = atoi(strTemp);
		strTemp = strTemp.Mid(strTemp.Find(strSplitChar)+1);
		switch (iLoop) {
			case 0:
				dwMajorVersion = iTemp;
				break;
			case 1:
				dwMinorVersion = iTemp;
				break;
			case 2:
				dwBuildNumber = iTemp;
				break;
			case 3:
				dwRevision = iTemp;
				break;
		}
	} // end for loop

}

bool CComponent::RunNTServicePackCheck()
{

	OSVERSIONINFO osv; CString strOSVersion;

	// check if NT service pack check is set
	if (mstrNTServicePackCheckNumber.GetLength() == 0) {
		gLog.LogEvent(mstrId + ": Service Pack string not set, component assumed installed");
		return true;
	}

	osv.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
	GetVersionEx(&osv);
	strOSVersion = osv.szCSDVersion;

	gLog.LogEvent(mstrId + ": Version Found: '" + strOSVersion + "', Required: '" + mstrNTServicePackCheckNumber + "'");

	// check service pack string
	if (mstrNTServicePackCheckNumber.CompareNoCase(osv.szCSDVersion) > 0)
		return false;

	// we have the specified service pack or newer
	return true;
}

bool CComponent::RunRegKeyCheck()
{
	HRESULT hResult; HKEY hKey; char chData[255]; DWORD lDataLen = 255;
	CString strSubKey, strValueName, strTemp;

	// get common files directory
	if (mstrRegKeyCheckKey.GetLength() == 0) {
		gLog.LogEvent(mstrId + ": Reg Key not specified, assuming installed.");
		return true;
	}
	SplitRegEntry(mstrRegKeyCheckKey, hKey, strSubKey, strValueName);
	if (strSubKey.GetLength() == 0) return true;

	hResult = RegOpenKeyEx(hKey, strSubKey, NULL, KEY_QUERY_VALUE, &hKey);
	if (hResult != ERROR_SUCCESS) {  // key not found, assume not installed
		hResult = RegCloseKey(hKey); 
		return false;
	} else {
		hResult = RegQueryValueEx(hKey, strValueName, NULL, NULL, (LPBYTE)&chData, &lDataLen);
		if (hResult != ERROR_SUCCESS) {  // key not found, assume not installed
			hResult = RegCloseKey(hKey); 
			return false;
		}
	}			
	hResult = RegCloseKey(hKey);
	
	// now check actual value
	strTemp.Insert(0, chData);

	if (mstrRegKeyCheckValue.CompareNoCase(strTemp) > 0)
		return false;

	// it is installed...
	return true;

}

// determines which registry branch to open, HKEY_????
void CComponent::SplitRegEntry(CString pstrKey, HKEY &hKey, CString &strSubKey, 
															 CString &strValueName) {

	if (pstrKey.Find("HKEY_LOCAL_MACHINE") != -1) {
		hKey = HKEY_LOCAL_MACHINE;
		strSubKey = pstrKey.Mid(18 + 1);
		strValueName = pstrKey.Mid(pstrKey.ReverseFind('\\') + 1);
		strSubKey = strSubKey.Left(strSubKey.ReverseFind('\\'));
	} else if (pstrKey.Find("HKEY_CLASSES_ROOT") != -1) {
		hKey = HKEY_CLASSES_ROOT;
		strSubKey = pstrKey.Mid(17 + 1);
		strValueName = pstrKey.Mid(pstrKey.ReverseFind('\\') + 1);
		strSubKey = strSubKey.Left(strSubKey.ReverseFind('\\'));
	} else {
		gLog.LogEvent(mstrId + ": Invalid key specified, no reg check ('" + pstrKey + "')");
	}
}

bool CComponent::RunRegVersionCheck()
{
	HRESULT hResult; HKEY hKey; char chData[255]; DWORD lDataLen = 255;
	CString strSubKey, strValueName, strTemp;
	DWORD dwRMajorVersion=0, dwRMinorVersion=0, dwRBuildNumber=0, dwRRevision=0;
	DWORD dwIMajorVersion=0, dwIMinorVersion=0, dwIBuildNumber=0, dwIRevision=0;

	gLog.LogEvent(mstrId + ": Checking reg version value");

	// check for valid settings
	if (mstrRegVersionCheckKey.GetLength() == 0) {
		gLog.LogEvent(mstrId + ": Reg Version Key not specified, assuming installed.");
		return true;
	}
	if (mstrRegVersionCheckVersion.GetLength() == 0) {
		gLog.LogEvent(mstrId + ": Reg Version value not specified, assuming installed.");
		return true;
	}
	SplitRegEntry(mstrRegVersionCheckKey, hKey, strSubKey, strValueName);
	if (strSubKey.GetLength() == 0) {
		gLog.LogEvent(mstrId + ": Reg subkey not split correctly by SplitRegEntry.");	
		return true;
	}

	hResult = RegOpenKeyEx(hKey, strSubKey, NULL, KEY_QUERY_VALUE, &hKey);
	if (hResult != ERROR_SUCCESS) {  // key not found, assume not installed
		hResult = RegCloseKey(hKey); 
		return false;
	} else {
		hResult = RegQueryValueEx(hKey, strValueName, NULL, NULL, (LPBYTE)&chData, &lDataLen);
		if (hResult != ERROR_SUCCESS) {  // key not found, assume not installed
			hResult = RegCloseKey(hKey); 
			return false;
		}
	}			
	hResult = RegCloseKey(hKey);
	
	// now check actual value
	strTemp.Insert(0, chData);

	// split version strings
	SplitVersionString(strTemp, dwIMajorVersion, dwIMinorVersion, dwIBuildNumber, dwIRevision);
	SplitVersionString(mstrRegVersionCheckVersion, dwRMajorVersion, dwRMinorVersion, dwRBuildNumber, dwRRevision);

	gLog.LogEvent(mstrId + ": Reg Version Check, current: " + strTemp + ", reqd: " + mstrRegVersionCheckVersion);

	if (dwIMajorVersion > dwRMajorVersion)   // dll is newer than dwMajorVersion
		return true;
	else if (dwIMajorVersion < dwRMajorVersion) // dll is older than dwMajorVersion
		return false;
	else {  // dll is dwMajorVersion
		if (dwIMinorVersion > dwRMinorVersion) // newer than dwMinorVersion
			return true;
		else if (dwIMinorVersion < dwRMinorVersion) // older than dwMinorVersion
			return false;
		else { // dwMajorVersion.dwMinorVersion
			if (dwIBuildNumber > dwRBuildNumber) // newer than dwBuildNumber
				return true;
			else if (dwIBuildNumber < dwRBuildNumber) // older than dwBuildNumber
				return false;
			else { // dwMajorVersion.dwMinorVersion.dwBuildNumber
				if (dwIRevision > dwRRevision) // newer than dwRevision
					return true;
				else if (dwIRevision < dwRRevision) // older than dwRevision
					return false;
				else  // file is correct version
					return true;
			}		// end dwBuildNumber
		}			// end dwMinorVersion
	}       // end dwMajorVersion
	
	return true;

}

bool CComponent::Install(CSetupDlg* &sDlg)
{

	int intResult; CString strMsg; int i;

	sDlg->m_CurStatus = mstrSetupMessage;
	sDlg->UpdateData(false);

	// check for setup exe
	while (!gUtils.FileExists(mstrSetupCommand)) {
		strMsg.Format(IDS_COMPONENT_FNF, mstrName, mstrSetupCommand);
		intResult = MessageBox(sDlg->m_hWnd, strMsg, sDlg->m_strAppName, MB_ABORTRETRYIGNORE | MB_ICONQUESTION | MB_TASKMODAL);
		if (intResult == IDABORT) {
			return false;
		} else if (intResult == IDIGNORE) {
			for (i = 0; i < mintSetupTime; i++)
				sDlg->Progress();
			return true;
		}
	} // end while loop, look again...

	// launch component
	STARTUPINFO sInfo; PROCESS_INFORMATION pInfo; STARTUPINFOW swInfo; DWORD dwResult;
	ZeroMemory(&sInfo, sizeof(sInfo));
	sInfo.cb = sizeof(STARTUPINFO);
	ZeroMemory(&swInfo, sizeof(swInfo));
	swInfo.cb = sizeof(STARTUPINFOW);
	int lReturn;
	bool bUserProfileOpen = false;
	HANDLE hProcess;

	lReturn = CreateProcess(mstrSetupCommand, mstrSetupCommandLine.GetBuffer(mstrSetupCommandLine.GetLength()+1), 
							NULL, NULL, true, NORMAL_PRIORITY_CLASS, NULL, NULL, &sInfo, &pInfo);
	mstrSetupCommandLine.ReleaseBuffer();
	hProcess = pInfo.hProcess;

	if (!lReturn) {
		gUtils.LogDLLError(mstrId + ": Create Process");
		return false;
	}

	// set up timer and init vars
	sDlg->StartTimer(mintSetupTime * 2);

	while (true) {

		// check if done with task
		dwResult = WaitForSingleObject(hProcess, 0);
		if (dwResult != WAIT_TIMEOUT) //we finished early
			break;

		// process messages
		MSG msg;
		while ( ::PeekMessage( &msg, NULL, 0, 0, PM_NOREMOVE ) )  { 
			if ( !AfxGetApp()->PumpMessage( ) ) { 
				::PostQuitMessage(0); 
				break; 
			} 
		} 

		// let MFC do its idle processing
		LONG lIdle = 0;
		while ( AfxGetApp()->OnIdle(lIdle++ ) )
						;  
	}

	// done with task, stop timer
	sDlg->StopTimer();
	
	// open reg key, and set that we installed this component
	long h; HKEY hRegKey; LPDWORD hResult = 0;
	char chTemp[2] = "1";
	LPCTSTR sKey = "Software\\giesler.org\\VBSW\\Installed Components";	
	h = RegCreateKeyEx(HKEY_CURRENT_USER, sKey,0,NULL,REG_OPTION_NON_VOLATILE, KEY_SET_VALUE, NULL, &hRegKey, hResult);
	if (h == ERROR_SUCCESS) {
		h = RegSetValueEx(hRegKey, mstrId, 0, REG_SZ, (byte*)&chTemp, strlen(chTemp) + 1);
		h = RegCloseKey(hRegKey);
	} else {
		gLog.LogEvent(mstrId + ": Error setting component to installed state in registry.");
		h = RegCloseKey(hRegKey);
	}

	gLog.LogEvent(mstrId + ": Component installation complete.");

	if (pInfo.hProcess) CloseHandle(pInfo.hProcess);
	if (pInfo.hThread) CloseHandle(pInfo.hThread);

	// just in case cancel hit on dialog
	return true;

}

bool CComponent::InstallAttempted()
{

	// open the registry key and look for pstrComponentId
	HRESULT hResult; HKEY hKey; char chData[255]; DWORD lDataLen = 255;
	CString strSubKey;
	LPCTSTR sKey = "Software\\giesler.org\\VBSW\\Installed Components";	

	hResult = RegOpenKeyEx(HKEY_CURRENT_USER, sKey, NULL, KEY_QUERY_VALUE, &hKey);
	if (hResult != ERROR_SUCCESS) {  // key not found, assume not installed
		hResult = RegCloseKey(hKey); 
		return false;
	} else {
		hResult = RegQueryValueEx(hKey, mstrId, NULL, NULL, (LPBYTE)&chData, &lDataLen);
		if (hResult != ERROR_SUCCESS) {  // key not found, assume not installed
			hResult = RegCloseKey(hKey); 
			return false;
		}
	}			

	hResult = RegCloseKey(hKey);
	
	// an install was attempted, so we will want to warn user
	return true;

}
