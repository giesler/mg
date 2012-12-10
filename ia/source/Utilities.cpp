// Utilities.cpp: implementation of the CUtilities class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "autorun.h"
#include "Utilities.h"
#include "mmsystem.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

CUtilities::CUtilities() 
{
	// set the filename used for getting settings
	m_strFileName		= EXEPath() + "ia\\settings.ini";

	// allocate a string for INI values
	m_pReturnedString	= (TCHAR*) malloc(1000);

	// get the current app's name
	m_strAppName		= GetINIString("Settings", "ProgramName", "Application");
}

CUtilities::~CUtilities() 
{
	// free allocated string
	free(m_pReturnedString);
}


CString CUtilities::CommonFilesPath() 
{

	HRESULT hResult; HKEY hKey; char chData[255]; DWORD lDataLen = 255;
	
	// get common files directory
	CString sCommonFilesKey = "Software\\Microsoft\\Windows\\CurrentVersion";
	
	hResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, sCommonFilesKey, NULL, KEY_QUERY_VALUE, &hKey);
	if (hResult != ERROR_SUCCESS) 
	{
		hResult = RegCloseKey(hKey); return "";
	} 
	else 
	{
		hResult = RegQueryValueEx(hKey, "CommonFilesDir", NULL, NULL, (LPBYTE)&chData, &lDataLen);
		if (hResult != ERROR_SUCCESS) 
		{
			hResult = RegCloseKey(hKey); return "";
		}
	}			

	hResult = RegCloseKey(hKey);
	return chData;
}


CString CUtilities::WinPath() 
{
	char chTemp[MAX_PATH];
	GetWindowsDirectory(chTemp, MAX_PATH);
	return (chTemp);
}


CString CUtilities::WinSysPath() 
{
	char chTemp[MAX_PATH];
	GetSystemDirectory(chTemp, MAX_PATH);
	return (chTemp);
}


CString CUtilities::TempPath() 
{
	char chTemp[MAX_PATH];
	GetTempPath(MAX_PATH, chTemp);
	return (chTemp);
}


bool CUtilities::FileExists(CString p_strFileName) 
{
	HANDLE fHandle = CreateFile(p_strFileName, 0, FILE_SHARE_READ, 
		NULL, OPEN_EXISTING, FILE_ATTRIBUTE_READONLY, NULL);

	if (fHandle == INVALID_HANDLE_VALUE)
		return false;
	CloseHandle(fHandle);
	
	return true;
}


void CUtilities::LogDLLError(CString strArea) 
{
	LPVOID lpMsgBuf;
	CString sTitle = "Error #"; char * buffer;
	buffer = new char[20];
	ltoa(GetLastError(), buffer, 10);
	sTitle += buffer;

	FormatMessage( 
	    FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
	    NULL, GetLastError(), MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPTSTR) &lpMsgBuf, 0, NULL);

	// Display the string.
	if (GetLastError() != 0) 
		MessageBox( NULL, (LPCTSTR)lpMsgBuf, m_strAppName, MB_OK | MB_ICONINFORMATION );

	// Free the buffer.
	LocalFree( lpMsgBuf );
	sTitle += " ";
	sTitle += (LPCTSTR) lpMsgBuf;
	gLog.LogEvent(sTitle);
}


CString CUtilities::ComputerName() 
{
	DWORD nSize = MAX_COMPUTERNAME_LENGTH + 1;
	char chTemp[MAX_COMPUTERNAME_LENGTH + 1];
	GetComputerName(chTemp, &nSize);
	return (chTemp);
}


CString CUtilities::EXEPath() 
{
	char chTemp[MAX_PATH];
	GetModuleFileName(NULL, chTemp, MAX_PATH);
	*(strrchr(chTemp, '\\')+1) = '\0';  // cut off .exe file
	return (chTemp);
}


CString CUtilities::EXEName() 
{
	char chTemp[MAX_PATH];
	GetModuleFileName(NULL, chTemp, MAX_PATH);
	CString cTemp = chTemp;
	cTemp = cTemp.Mid(cTemp.ReverseFind('\\')+1);
	return (cTemp);
}


bool CUtilities::Exec(CString strCommand, CString strCmdLine, bool waitForCompletion, bool shellExecute)
{

	CString strMsg; int intResult;

	// check for setup exe
	while (!gUtils.FileExists(strCommand)) 
	{
		strMsg.Format(IDS_SETUP_FNF, strCommand);
		intResult = MessageBox(NULL, strMsg, m_strAppName, MB_RETRYCANCEL | MB_ICONQUESTION | MB_TASKMODAL);
		if (intResult == IDCANCEL)
		{
			return false;
		}

	} // end while loop, look again...

	// launch component
	STARTUPINFO sInfo; PROCESS_INFORMATION pInfo; STARTUPINFOW swInfo;
	ZeroMemory(&sInfo, sizeof(sInfo));
	sInfo.cb = sizeof(STARTUPINFO);
	ZeroMemory(&swInfo, sizeof(swInfo));
	swInfo.cb = sizeof(STARTUPINFOW);
	int lReturn;
	bool bUserProfileOpen = false;
	HANDLE hProcess;

	// check if an MSI file
	if (strCommand.Find(".msi") > 0) 
	{
		strCmdLine = "msiexec.exe /i \"" + strCommand + "\" " + strCmdLine;
		strCommand = WinSysPath() + "\\msiexec.exe";
	}

	// launch process
	gLog.LogEvent("Starting command '" + strCommand + "', CmdLine '" + strCmdLine + "'");

	lReturn = CreateProcess(strCommand, strCmdLine.GetBuffer(strCmdLine.GetLength()+1), 
							NULL, NULL, true, NORMAL_PRIORITY_CLASS, NULL, NULL, &sInfo, &pInfo);
	strCmdLine.ReleaseBuffer();
	hProcess = pInfo.hProcess;

	if (!lReturn) 
	{
		gUtils.LogDLLError("Setup launch failed ('" + strCommand + ").");
		return false;
	}

	// wait for process to finish if we want to
	if (waitForCompletion) 
	{
		
		while (true) 
		{

			// check if done with task
			DWORD dwResult = WaitForSingleObject(hProcess, 0);
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
	}

	// close handles
	if (pInfo.hProcess) CloseHandle(pInfo.hProcess);
	if (pInfo.hThread) CloseHandle(pInfo.hThread);

	return true;

}


CString CUtilities::GetINIString(CString sectionName, CString keyName, CString defaultValue = "")
{
	CString strTemp;
	GetPrivateProfileString(sectionName, keyName, defaultValue, m_pReturnedString, 1000, m_strFileName);
	strTemp = m_pReturnedString;
	return strTemp;
}


bool CUtilities::GetINIBool(CString sectionName, CString keyName, bool defaultValue = false)
{
	int intTemp;
	intTemp = (defaultValue ? 1 : 0);
	intTemp = GetPrivateProfileInt(sectionName, keyName, intTemp, m_strFileName);
	return (intTemp == 1);
}


int CUtilities::GetINIInt(CString sectionName, CString keyName, int defaultValue = 0)
{
	return GetPrivateProfileInt(sectionName, keyName, defaultValue, m_strFileName);
}


int CUtilities::GetINISection(CString sectionName, CString sectionContents[20][2]) 
{
	
	CString strKey, strValue, strLine;
	int intCount;
	intCount = 0;

	GetPrivateProfileSection(sectionName, m_pReturnedString, 255, m_strFileName);

	// Now break up the string of buttons, and load info for each button
	TCHAR * lpTemp = m_pReturnedString;
	while (lpTemp != NULL) 
	{
		// get the info for this line
		strLine		= lpTemp;
		strKey		= strLine.Left(strLine.Find("="));
		strValue	= strLine.Mid(strLine.Find("=")+1);
		
		sectionContents[intCount][0] = strKey;
		sectionContents[intCount][1] = strValue;
		intCount++;

		while (*lpTemp != '\0')		// advance to next section in string
			lpTemp++;
		if (*lpTemp == '\0')			// new sections
			lpTemp++;
		if (*lpTemp == '\0')			// if at second null in a row, done
			break;

	}	// end while

	return intCount;

}


void CUtilities::ReplaceDirs(CString &p_strIn)
{
	// check for directory macros
	if (p_strIn.Find("%WinSysPath%") >= 0)
		p_strIn.Replace("%WinSysPath%", gUtils.WinSysPath());
	if (p_strIn.Find("%WinPath%") >= 0)
		p_strIn.Replace("%WinPath%", gUtils.WinPath());
	if (p_strIn.Find("%CommonFilesPath%") >= 0)
		p_strIn.Replace("%CommonFilesPath%", gUtils.CommonFilesPath());
	if (p_strIn.Find("%ExePath%") >= 0)
		p_strIn.Replace("%ExePath%", gUtils.EXEPath());
	if (p_strIn.Find("%TempPath%") >= 0)
		p_strIn.Replace("%TempPath%", gUtils.TempPath());
}


void CUtilities::PlaySoundFile(CString sound) 
{
	if (!sound.IsEmpty())
		PlaySound(gUtils.EXEPath() + sound, 0, SND_FILENAME | SND_ASYNC | SND_NOWAIT);
}


bool CUtilities::ValidateOS(CString iniSection, CString & result)
{
	OSVERSIONINFO osv; DWORD dwMajor; DWORD dwMinor;
	osv.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
	GetVersionEx(&osv);

	// Check if we only want a certain 9x version
	if (GetINIBool(iniSection, "Win9x", true) && osv.dwPlatformId == VER_PLATFORM_WIN32_WINDOWS) 
	{	
		// We are on a 9x system, check actual versions
		if (GetINIBool(iniSection, "Windows95", true) && (osv.dwMajorVersion == 4) && (osv.dwMinorVersion == 0) )
			return true;
		if (GetINIBool(iniSection, "Windows98", true) && (osv.dwMajorVersion == 4) && (osv.dwMinorVersion == 10) )
			return true;
		if (GetINIBool(iniSection, "Windows98", true) && (osv.dwMajorVersion == 4) && (osv.dwMinorVersion == 90) )
			return true;

		// We are on a 9x system but not the right one
		result = "This component does not apply to this version of Win9x.";
		return false;
	}

	// We must be on WinNT - so check versions if we need to
	if (GetINIBool(iniSection, "WinNT", true) && osv.dwPlatformId == VER_PLATFORM_WIN32_NT) 
	{

		// We are on an NT system, check actual version
	
		// MINIMUM VERSION
		CString strNTVersionMin		= GetINIString(iniSection, "NTMinVersion", "0");
		strNTVersionMin.Replace(",", ".");

		// see if the versions specified are 0's
		if (strNTVersionMin.CompareNoCase("0") == 0 || strNTVersionMin.Find("0.") >= 0) 
		{
			dwMajor = osv.dwMajorVersion;
			dwMinor = osv.dwMinorVersion;
		}
		else 
		{
			// read the actual version number
			dwMajor = atoi(strNTVersionMin.Left(strNTVersionMin.Find(".")));
			dwMinor = atoi(strNTVersionMin.Mid(strNTVersionMin.Find(".")+1));
		}

		// now check actual version numbers
		if (osv.dwMajorVersion < dwMajor || 
			(osv.dwMajorVersion == dwMajor && osv.dwMinorVersion < dwMinor)) 
		{
	
			result = "Windows version is older then minimum version for this component";
			return false;			// this OS is older then reqd
		} 
		else if (osv.dwMajorVersion == dwMajor && osv.dwMinorVersion == dwMinor) 
		{
			// correct win version, check SP level
			CString strSPMin = GetINIString(iniSection, "NTMinServicePack", "");

			if (!strSPMin.IsEmpty() && !ValidateSP(strSPMin, "", true)) 
			{
				result = "Windows service pack is older then required.";
				return false;
			} 
		}

		// MAXIMUM VERSION
		CString strNTVersionMax		= GetINIString(iniSection, "NTMaxVersion", "0");
		strNTVersionMax.Replace(",", ".");

		// see if the versions specified are 0's
		if (strNTVersionMax.CompareNoCase("0") == 0 || strNTVersionMax.Find("0.") >= 0) 
		{
			dwMajor = osv.dwMajorVersion;
			dwMinor	= osv.dwMinorVersion;
		}
		else 
		{
			// read the actual version number
			dwMajor = atoi(strNTVersionMax.Left(strNTVersionMax.Find(".")));
			dwMinor = atoi(strNTVersionMax.Mid(strNTVersionMax.Find(".")+1));
		}

		if (osv.dwMajorVersion > dwMajor || 
			(osv.dwMajorVersion == dwMajor && osv.dwMinorVersion > dwMinor)) 
		{

			result = "Windows version is newer then maximum version for this component";
			return false;			// this OS is newer then reqd
		} 
		else if (osv.dwMajorVersion == dwMajor && osv.dwMinorVersion == dwMinor) 
		{
			// correct win version, check SP level
			CString strSPMax = GetINIString(iniSection, "NTMaxServicePack", "");
		
			if (!strSPMax.IsEmpty() && !ValidateSP(strSPMax, "", false)) 
			{
				result = "Windows service pack is newer then required.";
				return false;
			}
		}

		// We have an OS of the correct version
		return true;

	}

	// fall through - neither Win9x nor WinNT was correct
	result = "This component does not apply to this OS.";
	return false;
}


bool CUtilities::ValidateSP(CString sp, CString componentId, bool minSP = true)
{
	OSVERSIONINFO osv; CString strOSVersion;

	// check if NT service pack check is set
	if (sp.GetLength() == 0) 
	{
		return true;
	}

	osv.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
	GetVersionEx(&osv);
	strOSVersion = osv.szCSDVersion;

	if (!componentId.IsEmpty())
		gLog.LogEvent(componentId + ": Version Found: '" + strOSVersion + "', Required: '" + sp + "'");

	// check service pack string
	if (minSP) 
	{
		if (sp.CompareNoCase(osv.szCSDVersion) > 0)
			return false;
	} 
	else 
	{
		if (sp.CompareNoCase(osv.szCSDVersion) < 0)
			return false;
	}

	// we have the specified service pack or newer
	return true;

}
