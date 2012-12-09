// util.cpp: implementation of the util class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "Autorun.h"
#include "SetupDlg.h"
#include "util.h"
#include "prompt.h"
#include <windows.h>
#include <stdlib.h>
#include <malloc.h>
#include <memory.h>
#include <stdio.h>
#include <atlconv.h>

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Private Functions
//////////////////////////////////////////////////////////////////////

CString util::CompName() {
	DWORD nSize = MAX_COMPUTERNAME_LENGTH + 1;
	char chTemp[MAX_COMPUTERNAME_LENGTH + 1];
	GetComputerName(chTemp, &nSize);
	return (chTemp);
};

CString util::WinSysDir() {
	char chTemp[MAX_PATH];
	GetSystemDirectory(chTemp, MAX_PATH);
	return (chTemp);
};

CString util::WinDir() {
	char chTemp[MAX_PATH];
	GetWindowsDirectory(chTemp, MAX_PATH);
	return (chTemp);
};

bool util::CheckFileVersion(VS_FIXEDFILEINFO *ver, DWORD dwMajorVersion, DWORD dwMinorVersion, 
											DWORD dwBuildNumber, DWORD dwRevision) {

	if (HIWORD(ver->dwFileVersionMS) > dwMajorVersion)   // dll is newer than dwMajorVersion
		return true;
	else if (HIWORD(ver->dwFileVersionMS) < dwMajorVersion) // dll is older than dwMajorVersion
		return false;
	else {  // dll is dwMajorVersion
		if (LOWORD(ver->dwFileVersionMS) > dwMinorVersion) // newer than dwMinorVersion
			return true;
		else if (LOWORD(ver->dwFileVersionMS) < dwMinorVersion) // older than dwMinorVersion
			return false;
		else { // dwMajorVersion.dwMinorVersion
			if (HIWORD(ver->dwFileVersionLS) > dwBuildNumber) // newer than dwBuildNumber
				return true;
			else if (HIWORD(ver->dwFileVersionLS) < dwBuildNumber) // older than dwBuildNumber
				return false;
			else { // dwMajorVersion.dwMinorVersion.dwBuildNumber
				if (LOWORD(ver->dwFileVersionLS) > dwRevision) // newer than dwRevision
					return true;
				else if (LOWORD(ver->dwFileVersionLS) < dwRevision) // older than dwRevision
					return false;
				else  // file is correct version
					return true;
			}		// end dwBuildNumber
		}			// end dwMinorVersion
	}       // end dwMajorVersion

};


bool util::InstallAndWait(CString sEXEPath, CString sCmdLine, int iSeconds, bool bShellExec) {

	DWORD dwResult; int iStep;

	if (!bShellExec && !FileExists(sEXEPath)) {
		AfxMessageBox("The program could not be started.  The program should be at '" + sEXEPath + "'.");
		return false;
	} else if (bShellExec && !FileExists(sCmdLine)) {
		AfxMessageBox("The program could not be started.  The program should be at '" + sCmdLine + "'.");
		return false;
	}

	STARTUPINFO sInfo; PROCESS_INFORMATION pInfo; STARTUPINFOW swInfo;
	ZeroMemory(&sInfo, sizeof(sInfo));
	sInfo.cb = sizeof(STARTUPINFO);
	ZeroMemory(&swInfo, sizeof(swInfo));
	swInfo.cb = sizeof(STARTUPINFOW);

	int lReturn;
	bool bUserProfileOpen = false;
	HANDLE hProcess;
	/*
	PROFILEINFO pi; ZeroMemory(&pi, sizeof(pi)); pi.dwSize = sizeof(pi);
	HANDLE hToken;
	*/

	if (bShellExec) {
		SHELLEXECUTEINFO sexec;
		ZeroMemory(&sexec, sizeof(sexec));
		sexec.cbSize = sizeof(sexec);
		sexec.hwnd = GetDesktopWindow();
		sexec.lpVerb = sEXEPath;
		sexec.lpFile = sCmdLine;
		sexec.nShow = SW_SHOWDEFAULT;
		if (!ShellExecuteEx(&sexec)) {
			AfxMessageBox("The file " + sCmdLine + " could not be started.");
			return false;
		}
		hProcess = sexec.hProcess;
#ifdef WINVER
#if(WINVER >= 0x0500 && _WIN32_WINNT >= 0x0500)
	} else if (IsWin2000() && bAltCredentials) {
		BSTR sbEXEPath = sEXEPath.AllocSysString();
		BSTR sbCmdLine = sCmdLine.AllocSysString();
		lReturn = CreateProcessWithLogonW(sUserName.AllocSysString(), sDomain.AllocSysString(), sPassword.AllocSysString(), LOGON_WITH_PROFILE, 
							sbEXEPath, sbCmdLine, NORMAL_PRIORITY_CLASS, NULL, NULL, &swInfo, &pInfo);
		hProcess = pInfo.hProcess;
#endif
#endif
	} else {
		lReturn = CreateProcess(sEXEPath, sCmdLine.GetBuffer(sCmdLine.GetLength()+1), 
								NULL, NULL, true, NORMAL_PRIORITY_CLASS, NULL, NULL, &sInfo, &pInfo);
		sCmdLine.ReleaseBuffer();
		hProcess = pInfo.hProcess;
	}

	if (!lReturn) {
		DisplayError();
		return false;
	}

	if (iSeconds != 0) {

		for (iStep = 0; iStep < iSeconds * 2; iStep++) {
			sDlg.Progress();
			sDlg.UpdateWindow();
			dwResult = WaitForSingleObject(hProcess, 500);
			if (dwResult != WAIT_TIMEOUT) // we finished early
				break;		
		}
		if (iStep <= iSeconds * 2) // we didn't time out, wait more
			WaitForSingleObject(hProcess, 30000);

		// finish advancing if needed
		if (iStep < iSeconds * 2)
			for (; iStep < iSeconds * 2; iStep++) 
				sDlg.Progress();
	}
/*
	if (bUserProfileOpen) {
		WaitForSingleObject(hProcess, INFINITE);
		UnloadUserProfile(hToken, pi.hProfile);
	} */
	return true;

};


bool util::FileVerInfo(CString sFileName, VS_FIXEDFILEINFO* &ver) {

	LPTSTR lptstrFilename; DWORD dwLength; LPDWORD lpdwHandle = NULL;
	lptstrFilename = sFileName.GetBuffer(sFileName.GetLength());
	
	dwLength = GetFileVersionInfoSizeA(lptstrFilename, lpdwHandle);
	if (dwLength == 0) {
		DisplayError();
		return false;
	}

	LPVOID *lpPointer; UINT dwLen;
	byte * lpData = new byte[dwLength+1];
	if (!GetFileVersionInfo(lptstrFilename, 0, dwLength, lpData)) {
		DisplayError();
		delete lpData;
		return false;
	} else {
		// get the version info
		if (!VerQueryValue(lpData, "\\", (LPVOID*)&lpPointer, &dwLen)) {
			DisplayError();
			delete lpData;
			return false;
		} else {
			ver = (VS_FIXEDFILEINFO*)lpPointer;
			delete lpData;
			return true;
		}
	}

};


bool util::FileExists(CString sFileName) {
	HANDLE fHandle = CreateFile(sFileName, 0, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_READONLY, NULL);
	if (fHandle == INVALID_HANDLE_VALUE)
		return false;
	CloseHandle(fHandle);
	return true;
};

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

util::util()
{
}

util::~util()
{
}

//////////////////////////////////////////////////////////////////////
// Public Functions
//////////////////////////////////////////////////////////////////////

void util::DisplayError() {
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
		MessageBox( NULL, (LPCTSTR)lpMsgBuf, sTitle, MB_OK | MB_ICONINFORMATION );
	// Free the buffer.
	LocalFree( lpMsgBuf );
};

bool util::IsDCOMInstalled() {
	if (!bCheckMDAC) return true;
	OSVERSIONINFO osv; HRESULT hResult; HKEY hKey; char chData[255]; DWORD lDataLen = 255;
	osv.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
	GetVersionEx(&osv);
	// check if we are running Win95 - if not we do not need to check for DCOM
	if (osv.dwMajorVersion == 4 && osv.dwMinorVersion == 0 && osv.dwPlatformId != VER_PLATFORM_WIN32_NT) {
		CString sDCOMKey = "Software\\Microsoft\\OLE";
		// open OLE key
		hResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, sDCOMKey, NULL, KEY_QUERY_VALUE, &hKey);
		if (hResult != ERROR_SUCCESS) {
			hResult = RegCloseKey(hKey); return false;
		}
		// query EnableDCOM value
		hResult = RegQueryValueEx(hKey, "EnableDCOM", NULL, NULL, (LPBYTE)&chData, &lDataLen);
		if (hResult != ERROR_SUCCESS) {
			DisplayError(); hResult = RegCloseKey(hKey); return false;
		}
		return (chData[0] == 'Y');   // see if EnableDCOM is set
	} else {  // we aren't running Win95
		return true;
	}
};


bool util::IsWin2000() {
	OSVERSIONINFO osv; 
	osv.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
	GetVersionEx(&osv);
	return (osv.dwPlatformId == VER_PLATFORM_WIN32_NT && osv.dwMajorVersion >= 5);
}


bool util::IsWinNT() {
	OSVERSIONINFO osv; 
	osv.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
	GetVersionEx(&osv);
	return (osv.dwPlatformId == VER_PLATFORM_WIN32_NT);
};


bool util::IsInstInstalled() {
	if (!bCheckMSI) return true;
	// check for dll
	CString sDLLPath = WinSysDir() + "\\msi.dll";
	VS_FIXEDFILEINFO *ver;
	if (!FileExists(sDLLPath))
		return false;
	if (!FileVerInfo(sDLLPath, ver))
		return false;
	else
		return CheckFileVersion(ver, 1, 10, 1029, 0);
};

bool util::IsComCtlDLLInstalled()
{
	if (!bCheckComCtlDLL) return true;
	// check for dll
	CString sDLLPath = WinSysDir() + "\\comctl32.dll";
	VS_FIXEDFILEINFO *ver;
	if (!FileExists(sDLLPath))
		return false;
	if (!FileVerInfo(sDLLPath, ver))
		return false;
	else
		return CheckFileVersion(ver, 5, 80, 2614, 3600);
}

bool util::IsComCtlInstalled() {
	if (!bCheckComCtl) return true;
	// check for dll
	CString sDLLPath = WinSysDir() + "\\mscomctl.ocx";
	VS_FIXEDFILEINFO *ver;
	if (!FileExists(sDLLPath))
		return false;
	if (!FileVerInfo(sDLLPath, ver))
		return false;
	else
		return CheckFileVersion(ver, 6, 0, 88, 62);
};

bool util::IsVBRTInstalled() {
	if (!bCheckVBRT) return true;
	// check for dll
	CString sDLLPath = WinSysDir() + "\\msvbvm60.dll";
	VS_FIXEDFILEINFO *ver;
	if (!FileExists(sDLLPath))
		return false;
	if (!FileVerInfo(sDLLPath, ver))
		return false;
	else
		return CheckFileVersion(ver, 6, 0, 88, 77);
};

bool util::IsMDACInstalled() {
	if (!bCheckMDAC) return true;
	HRESULT hResult; HKEY hKey; char chData[255]; DWORD lDataLen = 255;
	// get common files directory
	CString sCommonFilesKey = "Software\\Microsoft\\Windows\\CurrentVersion";
	hResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, sCommonFilesKey, NULL, KEY_QUERY_VALUE, &hKey);
	if (hResult != ERROR_SUCCESS) {
		DisplayError(); hResult = RegCloseKey(hKey); return false;
	} else {
		hResult = RegQueryValueEx(hKey, "CommonFilesDir", NULL, NULL, (LPBYTE)&chData, &lDataLen);
		if (hResult != ERROR_SUCCESS) {
			DisplayError(); hResult = RegCloseKey(hKey); return false;
		}
	}			
	hResult = RegCloseKey(hKey);
	
	// check for MSADO15.dll
	CString sDLLPath = "";
	sDLLPath.Insert(0, chData);
	sDLLPath += "\\System\\Ado\\msado15.dll";
	
	VS_FIXEDFILEINFO *ver;
	if (!FileExists(sDLLPath))
		return false;
	if (!FileVerInfo(sDLLPath, ver))
		return false;
	else
		return CheckFileVersion(ver, 2, 50, 4403, 9);
};

void util::SetEstimatedTime(int iSeconds) {
	sDlg.SetMaxProgress(iSeconds);
};

bool util::InstallMDAC() {
	sDlg.m_CurStatus = "Installing MDAC";
	sDlg.UpdateData(false);
	return InstallAndWait(EXEPath() + "runtime\\mdac_typ.exe", "mdac_typ.exe /q:a /c:\"setup.exe /QN1\"", 90);
};

bool util::InstallVBRT() {
	sDlg.m_CurStatus = "Installing Visual Basic files";
	sDlg.UpdateData(false);
	return InstallAndWait(EXEPath() + "runtime\\vbrun60.exe", "vbrun60.exe /q:a", 30);
};

bool util::InstallDCOM() {
	sDlg.m_CurStatus = "Installing DCOM";
	sDlg.UpdateData(false);
	return InstallAndWait(EXEPath() + "runtime\\dcom95.exe", "dcom95.exe /q:a", 10);
};

bool util::InstallInst() {
	sDlg.m_CurStatus = "Installing Windows Installer";
	sDlg.UpdateData(false);
	if (IsWinNT())
		return InstallAndWait(EXEPath() + "runtime\\InstMsiW.exe", "InstMsiW.exe /q", 20);
	else
		return InstallAndWait(EXEPath() + "runtime\\InstMsiA.exe", "InstMsiA.exe /q", 20);
};

bool util::InstallComCtlDLL() {
	sDlg.m_CurStatus = "Installing MS Common Control Update";
	sDlg.UpdateData(false);
	return InstallAndWait(EXEPath() + "runtime\\50comupd.exe", "50comupd.exe /q:a", 15);
}

bool util::InstallComCtl() {
	sDlg.m_CurStatus = "Installing MS Common Controls";
	sDlg.UpdateData(false);
	
	// install shell exec method
	if (false) {
		CString sCommandLine = EXEPath() + "runtime\\mscomctl\\mscomctl.inf";
		return InstallAndWait("install", sCommandLine, 10, true);
	} else {
		CString sCommandLine, sEXE;
		if (IsWinNT()) {
			sEXE = WinSysDir() + "\\rundll32.exe";
			sCommandLine = "rundll32.exe setupapi,InstallHinfSection DefaultInstall 132 ";
			sCommandLine += EXEPath() + "runtime\\mscomctl\\mscomctl.inf";
		} else {
			sEXE = WinDir() + "\\rundll.exe";
			sCommandLine = "rundll.exe setupx.dll,InstallHinfSection DefaultInstall 132 ";
			sCommandLine += EXEPath() + "runtime\\mscomctl\\mscomctl.inf";
		}
		return InstallAndWait(sEXE, sCommandLine, 10, false);
	}
};

bool util::RebootComp(CString chSetupCmd) {

	// set RunOnce key
	char chTemp[MAX_PATH];
	GetModuleFileName(NULL, chTemp, MAX_PATH);
	strcat(chTemp, chSetupCmd);
	long h; HKEY hRegKey; LPDWORD hResult = 0;
	LPCTSTR sKey = "Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce";	
	h = RegCreateKeyEx(HKEY_CURRENT_USER, sKey,0,NULL,REG_OPTION_NON_VOLATILE, KEY_SET_VALUE, NULL, &hRegKey, hResult);
	if (h == ERROR_SUCCESS) {
		h = RegSetValueEx(hRegKey, "VB Setup Program Autorun", 0, REG_SZ, (byte*)&chTemp, strlen(chTemp) + 1);
		h = RegCloseKey(hRegKey);
	} else {
		AfxMessageBox("Error setting autorun to restart!");
	}

	int iResult = MessageBox(NULL, "Your computer must be restarted in order to continue setup.  Would you like to restart now?\n\nSetup will continue when your computer starts again.", GetAppTitle(), MB_YESNO | MB_ICONQUESTION);
	if (iResult == IDYES) {
		// we want to reboot, but first adjust process privs
		HANDLE hToken; TOKEN_PRIVILEGES tkp;
		OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, &hToken);
		LookupPrivilegeValue(NULL, SE_SHUTDOWN_NAME, &tkp.Privileges[0].Luid);
		tkp.PrivilegeCount = 1;   // one priv to set
		tkp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
		AdjustTokenPrivileges(hToken, FALSE, &tkp, 0, (PTOKEN_PRIVILEGES)NULL, 0);
		ExitWindowsEx(EWX_REBOOT, 0);
	};
	return (iResult == IDYES);

};

CString util::EXEPath() {
	char chTemp[MAX_PATH];
	GetModuleFileName(NULL, chTemp, MAX_PATH);
	*(strrchr(chTemp, '\\')+1) = '\0';  // cut off .exe file
	return (chTemp);
};

CString util::EXEName() {
	char chTemp[MAX_PATH];
	GetModuleFileName(NULL, chTemp, MAX_PATH);
	return (chTemp);
};

bool util::SysUpdates(HWND hWnd) {

	if (!GetSettings())
		return false;

	int iEstimate = 0;
	bool bIsIEInstalled   = IsIEInstalled();
	bool bIsDCOMInstalled = IsDCOMInstalled();
	bool bIsMDACInstalled = IsMDACInstalled();
	bool bIsVBRTInstalled = IsVBRTInstalled();
	bool bIsInstInstalled = IsInstInstalled();
	bool bIsComCtlInstalled = IsComCtlInstalled();
	bool bIsComCtlDLLInstalled = IsComCtlDLLInstalled();
	if (!bIsIEInstalled) 
		iEstimate = 275 * 2;
	else if (!bIsDCOMInstalled) 
		iEstimate = 10  * 2;
	else {
		if (!bIsMDACInstalled) iEstimate += 90 * 2;
		if (!bIsVBRTInstalled) iEstimate += 30 * 2;
		if (!bIsInstInstalled) iEstimate += 20 * 2;
		if (!bIsComCtlDLLInstalled) iEstimate += 15 * 2;
		if (!bIsComCtlInstalled) iEstimate += 10 * 2;
	}
	
	if (iEstimate > 0) {
		CWnd* hw;
		hw = &sDlg;
		sDlg.Create(NULL);
		sDlg.ShowWindow(SW_SHOW);
		sDlg.UpdateWindow();
		SetEstimatedTime(iEstimate);
		// if we need to install ie, we have to do that then reboot
		if (!bIsIEInstalled) {
			InstallIE();
			sDlg.DestroyWindow();
			delete sDlg;
			RebootComp(" /dcomdone");
			return false;
		}			
		// if we need to install dcom, we have to do that then reboot
		if (!bIsDCOMInstalled) {
			InstallDCOM();
			sDlg.DestroyWindow();
			delete sDlg;
			RebootComp(" /dcomdone");
			return false;
		}
		if (!bIsMDACInstalled) InstallMDAC();
		if (!bIsVBRTInstalled) InstallVBRT();
		if (!bIsInstInstalled) InstallInst();
		if (!bIsComCtlDLLInstalled) InstallComCtlDLL();
		if (!bIsComCtlInstalled) InstallComCtl();
		if (!bIsVBRTInstalled || !bIsMDACInstalled || !bIsComCtlInstalled || !bIsComCtlDLLInstalled)  {
			sDlg.DestroyWindow();
			delete sDlg;
			RebootComp(" /alldone");
			return false;
		} else {
			sDlg.DestroyWindow();
			delete sDlg;
		}
	}
		
	// launch setup
	CString sSetupCommand = EXEPath() + sSetup;
	if (!FileExists(sSetupCommand)) {
		AfxMessageBox("The installation program could not be found.  It should be located at '" + sSetupCommand + "'");
	} else {
		CString sSetupCmd, sCmdLine;
		if (sSetup.Find(".msi") > 0) {
			sSetupCmd = WinSysDir() + "\\msiexec.exe";
			sCmdLine  = "msiexec.exe /i \"" + EXEPath() + sSetup + "\"";
		} else {
			sSetupCmd = sSetupCommand;
			sCmdLine = sSetupCommand.Mid(sSetupCommand.ReverseFind('\\')+1);
		}
		InstallAndWait(sSetupCmd, sCmdLine, 0);
	}

	return true;
}

bool util::GetSettings() {

	LPCTSTR lpAppName; TCHAR lpReturnedString[255]; 
	DWORD nSize;

	CString lpFileName = EXEPath() + "settings.ini";
	lpAppName = "Autorun Settings";
	nSize = 255;

	GetPrivateProfileString(lpAppName, "Title", "Autorun Program", lpReturnedString, nSize, lpFileName);
	GetPrivateProfileString(lpAppName, "Setup", "setup\\setup.exe", lpReturnedString, nSize, lpFileName);
	sSetup = lpReturnedString;

	bCheckMDAC = GetPrivateProfileInt(lpAppName, "MDAC", 0, lpFileName) == 1;
	bCheckDCOM = GetPrivateProfileInt(lpAppName, "DCOM", 0, lpFileName) == 1;
	bCheckVBRT = GetPrivateProfileInt(lpAppName, "VBRT", 0, lpFileName) == 1;
	bCheckMSI  = GetPrivateProfileInt(lpAppName, "MSI",  0, lpFileName) == 1;
	bCheckComCtl = GetPrivateProfileInt(lpAppName, "ComCtl",  0, lpFileName) == 1;
	bCheckIE = GetPrivateProfileInt(lpAppName, "IE", 0, lpFileName) == 1;
	bCheckComCtlDLL = GetPrivateProfileInt(lpAppName, "ComCtlDLL", 0, lpFileName) == 1;

	bAltCredentials = GetPrivateProfileInt(lpAppName, "AltCredentials",  0, lpFileName) == 1;
	
#if(WINVER >= 0x0500 && _WIN32_WINNT >= 0x0500)
	if (bAltCredentials && IsWin2000()) {
		sUserName = "Administrator";
		sDomain = CompName();
		sPassword = "120960011014";
		if (!LogonAttempt(sUserName, sPassword, sDomain)) {
			sPassword = "figure1610";
			if (!LogonAttempt(sUserName, sPassword, sDomain)) {
				sPassword = "amdcamp";
				if (!LogonAttempt(sUserName, sPassword, sDomain)) {		
					CPrompt cp;
					int nResponse = IDOK;
					while (nResponse == IDOK) {
						nResponse = cp.DoModal();
						if (nResponse == IDOK) {
							if (LogonAttempt(sUserName, cp.m_inputbox, sDomain)) {
								sPassword = cp.m_inputbox;
								return true;
							}
						}
					}
					AfxMessageBox("The setup program could not continue.  An invalid password was specified.  Please contact support.");
					return false;
				}
			}
		}
	}
#endif
	return true;
}

#if(WINVER >= 0x0500 && _WIN32_WINNT >= 0x0500)

bool util::LogonAttempt(CString sUserName, CString sPassword, CString sDomain) {

	STARTUPINFO sInfo; PROCESS_INFORMATION pInfo; STARTUPINFOW swInfo;
	ZeroMemory(&sInfo, sizeof(sInfo));
	sInfo.cb = sizeof(STARTUPINFO);
	ZeroMemory(&swInfo, sizeof(swInfo));
	swInfo.cb = sizeof(STARTUPINFOW);
	CString sEXEPath = WinSysDir() + "\\rundll32.exe";
	CString sCmdLine = "rundll32.exe";

	int lReturn;
	lReturn = CreateProcessWithLogonW(sUserName.AllocSysString(), sDomain.AllocSysString(), sPassword.AllocSysString(), LOGON_WITH_PROFILE, 
				sEXEPath.AllocSysString(), sCmdLine.AllocSysString(), NORMAL_PRIORITY_CLASS, NULL, NULL, &swInfo, &pInfo);

	if (!lReturn) {
		if (GetLastError() == 1326) {
			return false;
		} else {
			DisplayError();
			return false;
		}
	} else {
		return true;
	}

}
#endif


CString util::GetAppTitle() {

	char chTemp[MAX_PATH];
	GetModuleFileName(NULL, chTemp, MAX_PATH);
	*(strrchr(chTemp, '\\')+1) = '\0';  // cut off .exe file
	TCHAR sAppName[255];
	CString sFileName = chTemp;
	sFileName += "settings.ini";
	GetPrivateProfileString("Autorun Settings", "Title", "Autorun", sAppName, 255, sFileName);
	return sAppName;
}

void util::RemoveMDACKey() {

	long hResult; HKEY hKey; char chData[255]; DWORD lDataLen = 255;
	LPCTSTR sKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";	
	hResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, sKey, NULL, KEY_ALL_ACCESS, &hKey);
	if (hResult == ERROR_SUCCESS) {
		hResult = RegQueryValueEx(hKey, "mdac_runonce", NULL, NULL, (LPBYTE)&chData, &lDataLen);
		if (hResult == ERROR_SUCCESS) {
			hResult = RegDeleteValue(hKey, "mdac_runonce");
		}
	}
	hResult = RegCloseKey(hKey);

}



CString util::Domain()
{
	char chTemp[MAX_PATH];
	ExpandEnvironmentStrings("%USERDOMAIN%", chTemp, MAX_PATH);
	return (chTemp);
}

CString util::GetBMPFileName()
{
	char chTemp[MAX_PATH];
	GetModuleFileName(NULL, chTemp, MAX_PATH);
	*(strrchr(chTemp, '\\')+1) = '\0';  // cut off .exe file
	TCHAR sAppName[255];
	CString sFileName = chTemp;
	sFileName += "settings.ini";
	GetPrivateProfileString("Autorun Settings", "Splash", "", sAppName, 255, sFileName);
	return sAppName;

}

bool util::IsIEInstalled()
{
	if (!bCheckIE) return true;
	// check for dll
	CString sDLLPath = WinSysDir() + "\\wininet.dll";
	VS_FIXEDFILEINFO *ver;
	if (!FileExists(sDLLPath))
		return false;
	if (!FileVerInfo(sDLLPath, ver))
		return false;
	else
		return CheckFileVersion(ver, 4, 70, 1300, 0);
}

bool util::InstallIE()
{
	sDlg.m_CurStatus = "Upgrading Internet Explorer";
	sDlg.UpdateData(false);
	return InstallAndWait(EXEPath() + "runtime\\ie\\ie5setup.exe", 
			"ie5setup.exe /q:a /c:\"ie5wzd.exe /s:\"\"#E\"\" /q:a /r:n\"", 275);
}


