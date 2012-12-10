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
	mblnListed = false;
}


CComponent::~CComponent()
{
}


CComponent::CComponent(const CComponent &src) {
	this->mstrName = src.mstrName;
}


CComponent CComponent::operator =(CComponent &rhs) 
{
	
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
bool CComponent::Load(CString sCompName) 
{

	// init vars
	mblnInstall = false;

	// Set component ID
	mstrId = sCompName;

	// Get basic settings from INI file
	mstrName			= gUtils.GetINIString(sCompName, "Name", mstrId);

	// parse depends list
	CString strDepends	= gUtils.GetINIString(sCompName, "Dependencies", "");
	while (strDepends.GetLength() > 0) 
	{
		if (strDepends.Find(" ") > 0)  
		{
			mlstDepends.AddTail(strDepends.Left(strDepends.Find(" ")));
			strDepends = strDepends.Mid(strDepends.Find(" ")+1);
		} 
		else 
		{
			mlstDepends.AddTail(strDepends);
			strDepends = "";
		}
	}

	// parse includes list
	CString strIncludes	= gUtils.GetINIString(sCompName, "Includes", "");
	while (strIncludes.GetLength() > 0) 
	{
		if (strIncludes.Find(" ") > 0)  
		{
			mlstIncludes.AddTail(strIncludes.Left(strIncludes.Find(" ")));
			strIncludes = strIncludes.Mid(strIncludes.Find(" ")+1);
		} 
		else 
		{
			mlstIncludes.AddTail(strIncludes);
			strIncludes = "";
		}
	}

	// Installation info
	mstrSetupCommand		= gUtils.GetINIString(sCompName, "SetupCommand", "");
	mstrSetupCommandLine	= gUtils.GetINIString(sCompName, "SetupCommandLine", "");
	mintSetupTime			= gUtils.GetINIInt(sCompName, "SetupTime", 30);
	mstrSetupMessage		= gUtils.GetINIString(sCompName, "SetupMessage", "Installing...");
	mblnQuietInstall		= gUtils.GetINIBool(sCompName, "QuietInstall", true);

	// Get reboot setting, assume none
	mtypReboot = (RebootType) gUtils.GetINIInt(sCompName, "RebootType", 0);

	// Get type of component and then associated settings
	mtypComponentType	= (ComponentType) gUtils.GetINIInt(sCompName, "ComponentType", 0);
	switch (mtypComponentType) 
	{
		case FileVersionCheck:
			mstrFileVersionCheckDLL			= gUtils.GetINIString(sCompName, "FileVersionCheckDLL", "");
			mstrFileVersionCheckVersion		= gUtils.GetINIString(sCompName, "FileVersionCheckVersion", "");
			gUtils.ReplaceDirs(mstrFileVersionCheckDLL);
			break;
		case RegVersionCheck:
			mstrRegVersionCheckKey			= gUtils.GetINIString(sCompName, "RegVersionCheckKey", "");
			mstrRegVersionCheckVersion		= gUtils.GetINIString(sCompName, "RegVersionCheckVersion", "");
			break;
		case RegKeyCheck:
			mstrRegKeyCheckKey				= gUtils.GetINIString(sCompName, "RegKeyCheckKey", "");
			mstrRegKeyCheckValue			= gUtils.GetINIString(sCompName, "RegKeyCheckValue", "");
			break;
		case NTServicePackCheck:
			mstrNTServicePackCheckNumber	= gUtils.GetINIString(sCompName, "NTServicePackCheckNumber", "");
			break;
		case NoCheck:
			break;
		case NetFrameworkCheck:
			mstrNetFrameworkCheckVersion	= gUtils.GetINIString(sCompName, "NetFrameworkCheckVersion", "");
			break;
		default:
			gLog.LogEvent(mstrName + ": Invalid/no check type specified, ignoring component.");
			return false;
	}

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
	CString result;
	if (!gUtils.ValidateOS(mstrId, result)) 
	{
		gLog.LogEvent(mstrName + ": " + result);
		return;
	}

	// now check the appropriate component check
	switch (mtypComponentType) 
	{
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
		case NoCheck:
			mblnInstalled = false;
			break;
		case NetFrameworkCheck:
			mblnInstalled = RunNetFrameworkCheck();
			break;
		default:
			return;
	}
	
}


bool CComponent::RunFileVersionCheck()
{

	DWORD dwMajorVersion=0, dwMinorVersion=0, dwBuildNumber=0, dwRevision=0;
	VERSION_INFO vinfo; CFileVersion fv;

	// Check if file exists, if not, assume not installed
	if (!gUtils.FileExists(mstrFileVersionCheckDLL)) 
	{
		gLog.LogEvent(mstrName + ": File " + mstrFileVersionCheckDLL + " not found, assuming not installed.");
		return false;
	}

	// Try to open the version info on the DLL
	if (!fv.Open(mstrFileVersionCheckDLL)) 
	{
		gLog.LogEvent(mstrName + ": Failed checking '" + mstrFileVersionCheckDLL + "' version info, assuming not installed");
		return false;
	}
	vinfo = fv.GetFixedFileVersionInfo();

	// Split the version info string from config
	SplitVersionString(mstrFileVersionCheckVersion, dwMajorVersion, 
		dwMinorVersion, dwBuildNumber, dwRevision);

	// Log what versions were found
	gLog.LogEvent(mstrName + ": File: " + mstrFileVersionCheckDLL + ", version: " 
			+ fv.GetFixedFileVersion() + "; required: " + mstrFileVersionCheckVersion);

	if (vinfo.dwMajorVerison > dwMajorVersion)   // dll is newer than dwMajorVersion
		return true;
	else if (vinfo.dwMajorVerison < dwMajorVersion) // dll is older than dwMajorVersion
		return false;
	else 
	{  // dll is dwMajorVersion
		if (vinfo.dwMinorVersion > dwMinorVersion) // newer than dwMinorVersion
			return true;
		else if (vinfo.dwMinorVersion < dwMinorVersion) // older than dwMinorVersion
			return false;
		else 
		{ // dwMajorVersion.dwMinorVersion
			if (vinfo.dwBuildNumber > dwBuildNumber) // newer than dwBuildNumber
				return true;
			else if (vinfo.dwBuildNumber < dwBuildNumber) // older than dwBuildNumber
				return false;
			else 
			{ // dwMajorVersion.dwMinorVersion.dwBuildNumber
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
	for (int iLoop = 0; iLoop < 4; iLoop++) 
	{
		iTemp = atoi(strTemp.Left(strTemp.Find(strSplitChar)));
		if (iTemp == 0)
			iTemp = atoi(strTemp);
		strTemp = strTemp.Mid(strTemp.Find(strSplitChar)+1);
		switch (iLoop) 
		{
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
	return (gUtils.ValidateSP(mstrNTServicePackCheckNumber, mstrName, true));
}


//typedef HRESULT (WINAPI *FNGETCORVERSION) (LPWSTR buffer, DWORD ccBuffer, DWORD* pcBuffer);

bool CComponent::RunNetFrameworkCheck()
{

	HRESULT hResult; HKEY hKey; char chData[255]; DWORD lDataLen = 255;
	CString strSubKey, strValueName, strTemp;

	// get common files directory
	if (mstrNetFrameworkCheckVersion.GetLength() == 0) 
	{
		gLog.LogEvent(mstrName + ": Version not specified, assuming installed.");
		return true;
	}

	CString mainVersion = mstrNetFrameworkCheckVersion.Left(mstrNetFrameworkCheckVersion.ReverseFind('.'));
	CString buildNumber = mstrNetFrameworkCheckVersion.Mid(mstrNetFrameworkCheckVersion.ReverseFind('.')+1);
	
	if (mainVersion.Left(1) != "v")
		mainVersion = "v" + mainVersion;

	CString regKey = "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NETFramework\\policy\\" 
		+ mainVersion + "\\" + buildNumber;

	SplitRegEntry(regKey, hKey, strSubKey, strValueName);
	if (strSubKey.GetLength() == 0) return true;

	hResult = RegOpenKeyEx(hKey, strSubKey, NULL, KEY_QUERY_VALUE, &hKey);
	if (hResult != ERROR_SUCCESS) 
	{  // key not found, assume not installed
		hResult = RegCloseKey(hKey); 
		return false;
	} 
	else 
	{
		hResult = RegQueryValueEx(hKey, strValueName, NULL, NULL, (LPBYTE)&chData, &lDataLen);
		if (hResult != ERROR_SUCCESS) 
		{  // key not found, assume not installed
			hResult = RegCloseKey(hKey); 
			return false;
		}
	}			
	hResult = RegCloseKey(hKey);

	// We got a return value, so we are good to go - it is installed
	gLog.LogEvent(mstrName + ": .Net framework version " + mstrNetFrameworkCheckVersion + " is installed.");
	return true;

	// Attempt to load the .Net framework DLL
//	HMODULE hModule = LoadLibrary("mscoree.dll");

	// Check if we could even load mscoree.dll
//	if (hModule == NULL)  
//		return false;
	
	// Attempt to find the function to retreive the version
//	FNGETCORVERSION fnNetVersion = (FNGETCORVERSION) GetProcAddress(hModule, "GetCORVersion");

	// Make sure we got a pointer
//	if (fnNetVersion == NULL)
//		return false;

	// Alloc a string to retreive the version
//	DWORD returnSize;
//	LPWSTR tempVersion = (LPWSTR) malloc(1000);
	//m_pReturnedString = (char*) malloc(25);

//	(fnNetVersion)(tempVersion, 25, &returnSize);
	
//	CString strVersion = tempVersion;
//	if (strVersion.Left(1) == "v")
//		strVersion = strVersion.Mid(1);

	// split version strings
//	DWORD dwRMajorVersion=0, dwRMinorVersion=0, dwRBuildNumber=0, dwRRevision=0;
//	DWORD dwIMajorVersion=0, dwIMinorVersion=0, dwIBuildNumber=0, dwIRevision=0;
//	SplitVersionString(strVersion, dwIMajorVersion, dwIMinorVersion, dwIBuildNumber, dwIRevision);
//	SplitVersionString(mstrNetFrameworkCheckVersion, dwRMajorVersion, dwRMinorVersion, dwRBuildNumber, dwRRevision);

//	gLog.LogEvent(mstrName + ": Reg Version Check, current: " + strVersion + ", reqd: " + mstrNetFrameworkCheckVersion);
	
//	if (dwIMajorVersion > dwRMajorVersion)   // dll is newer than dwMajorVersion
//		return true;
//	else if (dwIMajorVersion < dwRMajorVersion) // dll is older than dwMajorVersion
//		return false;
//	else {  // dll is dwMajorVersion
//		if (dwIMinorVersion > dwRMinorVersion) // newer than dwMinorVersion
//			return true;
//		else if (dwIMinorVersion < dwRMinorVersion) // older than dwMinorVersion
//			return false;
//		else { // dwMajorVersion.dwMinorVersion
//			if (dwIBuildNumber > dwRBuildNumber) // newer than dwBuildNumber
//				return true;
//			else if (dwIBuildNumber < dwRBuildNumber) // older than dwBuildNumber
//				return false;
//			else { // dwMajorVersion.dwMinorVersion.dwBuildNumber
//				if (dwIRevision > dwRRevision) // newer than dwRevision
//					return true;
//				else if (dwIRevision < dwRRevision) // older than dwRevision
//					return false;
//				else  // file is correct version
//					return true;
//			}		// end dwBuildNumber
//		}			// end dwMinorVersion
//	}       // end dwMajorVersion
//	
//	return true;
//	free(tempVersion);


	// we have the framework installed
//	return true;
}


bool CComponent::RunRegKeyCheck()
{
	HRESULT hResult; HKEY hKey; char chData[255]; DWORD lDataLen = 255;
	CString strSubKey, strValueName, strTemp;

	// get common files directory
	if (mstrRegKeyCheckKey.GetLength() == 0) 
	{
		gLog.LogEvent(mstrName + ": Reg Key not specified, assuming installed.");
		return true;
	}
	SplitRegEntry(mstrRegKeyCheckKey, hKey, strSubKey, strValueName);
	if (strSubKey.GetLength() == 0) 
		return true;

	hResult = RegOpenKeyEx(hKey, strSubKey, NULL, KEY_QUERY_VALUE, &hKey);
	if (hResult != ERROR_SUCCESS) 
	{  // key not found, assume not installed
		hResult = RegCloseKey(hKey); 
		return false;
	} 
	else 
	{
		hResult = RegQueryValueEx(hKey, strValueName, NULL, NULL, (LPBYTE)&chData, &lDataLen);
		if (hResult != ERROR_SUCCESS)
		{  // key not found, assume not installed
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
	gLog.LogEvent(mstrName + ": Registry key found and matches required value.");
	return true;

}


// determines which registry branch to open, HKEY_????
void CComponent::SplitRegEntry(CString pstrKey, HKEY &hKey, CString &strSubKey, 
															 CString &strValueName) {

	if (pstrKey.Find("HKEY_LOCAL_MACHINE") != -1) 
	{
		hKey = HKEY_LOCAL_MACHINE;
		strSubKey = pstrKey.Mid(18 + 1);
		strValueName = pstrKey.Mid(pstrKey.ReverseFind('\\') + 1);
		strSubKey = strSubKey.Left(strSubKey.ReverseFind('\\'));
	} 
	else if (pstrKey.Find("HKEY_CLASSES_ROOT") != -1) 
	{
		hKey = HKEY_CLASSES_ROOT;
		strSubKey = pstrKey.Mid(17 + 1);
		strValueName = pstrKey.Mid(pstrKey.ReverseFind('\\') + 1);
		strSubKey = strSubKey.Left(strSubKey.ReverseFind('\\'));
	} 
	else 
	{
		gLog.LogEvent(mstrName + ": Invalid key specified, no reg check ('" + pstrKey + "')");
	}
}


bool CComponent::RunRegVersionCheck()
{
	HRESULT hResult; HKEY hKey; char chData[255]; DWORD lDataLen = 255;
	CString strSubKey, strValueName, strTemp;
	DWORD dwRMajorVersion=0, dwRMinorVersion=0, dwRBuildNumber=0, dwRRevision=0;
	DWORD dwIMajorVersion=0, dwIMinorVersion=0, dwIBuildNumber=0, dwIRevision=0;

	gLog.LogEvent(mstrName + ": Checking reg version value");

	// check for valid settings
	if (mstrRegVersionCheckKey.GetLength() == 0) 
	{
		gLog.LogEvent(mstrName + ": Reg Version Key not specified, assuming installed.");
		return true;
	}
	if (mstrRegVersionCheckVersion.GetLength() == 0)
	{
		gLog.LogEvent(mstrName + ": Reg Version value not specified, assuming installed.");
		return true;
	}
	SplitRegEntry(mstrRegVersionCheckKey, hKey, strSubKey, strValueName);
	if (strSubKey.GetLength() == 0) 
	{
		gLog.LogEvent(mstrName + ": Reg subkey not split correctly by SplitRegEntry.");	
		return true;
	}

	hResult = RegOpenKeyEx(hKey, strSubKey, NULL, KEY_QUERY_VALUE, &hKey);
	if (hResult != ERROR_SUCCESS) 
	{  // key not found, assume not installed
		hResult = RegCloseKey(hKey); 
		return false;
	} 
	else 
	{
		hResult = RegQueryValueEx(hKey, strValueName, NULL, NULL, (LPBYTE)&chData, &lDataLen);
		if (hResult != ERROR_SUCCESS) 
		{  // key not found, assume not installed
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

	gLog.LogEvent(mstrName + ": Reg Version Check, current: " + strTemp + ", reqd: " + mstrRegVersionCheckVersion);

	if (dwIMajorVersion > dwRMajorVersion)   // dll is newer than dwMajorVersion
		return true;
	else if (dwIMajorVersion < dwRMajorVersion) // dll is older than dwMajorVersion
		return false;
	else 
	{  // dll is dwMajorVersion
		if (dwIMinorVersion > dwRMinorVersion) // newer than dwMinorVersion
			return true;
		else if (dwIMinorVersion < dwRMinorVersion) // older than dwMinorVersion
			return false;
		else 
		{ // dwMajorVersion.dwMinorVersion
			if (dwIBuildNumber > dwRBuildNumber) // newer than dwBuildNumber
				return true;
			else if (dwIBuildNumber < dwRBuildNumber) // older than dwBuildNumber
				return false;
			else 
			{ // dwMajorVersion.dwMinorVersion.dwBuildNumber
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

	// Check for vars to replace
	gUtils.ReplaceDirs(mstrSetupCommand);

	// If the file mstrSetupCommand doesn't exist as is, prepend the EXE path (normal case)
	if (!gUtils.FileExists(mstrSetupCommand))
		mstrSetupCommand = gUtils.EXEPath() + mstrSetupCommand;

	// check for explorer.exe
	if (mstrSetupCommand.Find("explorer.exe", 0) != -1) 
	{
		mstrSetupCommand = gUtils.WinPath() + mstrSetupCommand;
	}

	// check for installer
	while (!gUtils.FileExists(mstrSetupCommand)) 
	{
		// prompt user what to do since installer not found
		strMsg.Format(IDS_COMPONENT_FNF, mstrName, mstrSetupCommand);
		intResult = MessageBox(sDlg->m_hWnd, strMsg, sDlg->m_strAppName, MB_ABORTRETRYIGNORE | MB_ICONQUESTION | MB_TASKMODAL);
		if (intResult == IDABORT) 
		{
			return false;
		} 
		else if (intResult == IDIGNORE) 
		{
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

	if (!lReturn) 
	{
		gUtils.LogDLLError(mstrName + ": Create Process");
		return false;
	}

	// set up timer and init vars
	sDlg->StartTimer(mintSetupTime * 2);

	while (true) 
	{

		// check if done with task
		dwResult = WaitForSingleObject(hProcess, 0);
		if (dwResult != WAIT_TIMEOUT) //we finished early
			break;

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
	}

	// done with task, stop timer
	sDlg->StopTimer();
	
	// Get and log return code
	DWORD returnCode;
	if (GetExitCodeProcess(hProcess, &returnCode)) 
	{
		CString strTemp;
		strTemp.Format("%d", returnCode);
		gLog.LogEvent(mstrName + ": Install returned code: " + strTemp);
	}
	
	// open reg key, and set that we installed this component
	long h; HKEY hRegKey; LPDWORD hResult = 0;
	char chTemp[2] = "1";
	LPCTSTR sKey = "Software\\Install Assistant\\Installed Components";	
	h = RegCreateKeyEx(HKEY_CURRENT_USER, sKey,0,NULL,REG_OPTION_NON_VOLATILE, KEY_SET_VALUE, NULL, &hRegKey, hResult);
	if (h == ERROR_SUCCESS) 
	{
		h = RegSetValueEx(hRegKey, mstrId, 0, REG_SZ, (byte*)&chTemp, strlen(chTemp) + 1);
		h = RegCloseKey(hRegKey);
	} 
	else 
	{
		gLog.LogEvent(mstrName + ": Error setting component to installed state in registry.");
		h = RegCloseKey(hRegKey);
	}

	gLog.LogEvent(mstrName + ": Component installation complete.");

	if (pInfo.hProcess) CloseHandle(pInfo.hProcess);
	if (pInfo.hThread) CloseHandle(pInfo.hThread);

	// just in case cancel hit on dialog
	return true;

}

bool CComponent::InstallAttempted()
{

	// If an 'always install' component, ignore if an install was attempted
	if (mtypComponentType == NoCheck)
		return false;

	// open the registry key and look for pstrComponentId
	HRESULT hResult; HKEY hKey; char chData[255]; DWORD lDataLen = 255;
	CString strSubKey;
	LPCTSTR sKey = "Software\\Install Assistant\\Installed Components";	

	hResult = RegOpenKeyEx(HKEY_CURRENT_USER, sKey, NULL, KEY_QUERY_VALUE, &hKey);
	if (hResult != ERROR_SUCCESS) 
	{  // key not found, assume not installed
		hResult = RegCloseKey(hKey); 
		return false;
	} 
	else 
	{
		hResult = RegQueryValueEx(hKey, mstrId, NULL, NULL, (LPBYTE)&chData, &lDataLen);
		if (hResult != ERROR_SUCCESS) 
		{  // key not found, assume not installed
			hResult = RegCloseKey(hKey); 
			return false;
		}
	}			

	hResult = RegCloseKey(hKey);
	
	// an install was attempted, so we will want to warn user
	return true;

}
