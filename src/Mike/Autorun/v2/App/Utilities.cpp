// Utilities.cpp: implementation of the CUtilities class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "autorun.h"
#include "Utilities.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif


CString CUtilities::CommonFilesPath() {
	HRESULT hResult; HKEY hKey; char chData[255]; DWORD lDataLen = 255;
	// get common files directory
	CString sCommonFilesKey = "Software\\Microsoft\\Windows\\CurrentVersion";
	hResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, sCommonFilesKey, NULL, KEY_QUERY_VALUE, &hKey);
	if (hResult != ERROR_SUCCESS) {
		hResult = RegCloseKey(hKey); return "";
	} else {
		hResult = RegQueryValueEx(hKey, "CommonFilesDir", NULL, NULL, (LPBYTE)&chData, &lDataLen);
		if (hResult != ERROR_SUCCESS) {
			hResult = RegCloseKey(hKey); return "";
		}
	}			
	hResult = RegCloseKey(hKey);
	return chData;
}

CString CUtilities::WinPath() {
	char chTemp[MAX_PATH];
	GetWindowsDirectory(chTemp, MAX_PATH);
	return (chTemp);
}

CString CUtilities::WinSysPath() {
	char chTemp[MAX_PATH];
	GetSystemDirectory(chTemp, MAX_PATH);
	return (chTemp);
}

CString CUtilities::TempPath() {
	char chTemp[MAX_PATH];
	GetTempPath(MAX_PATH, chTemp);
	return (chTemp);
}

bool CUtilities::FileExists(CString p_strFileName) {
	HANDLE fHandle = CreateFile(p_strFileName, 0, FILE_SHARE_READ, 
		NULL, OPEN_EXISTING, FILE_ATTRIBUTE_READONLY, NULL);
	if (fHandle == INVALID_HANDLE_VALUE)
		return false;
	CloseHandle(fHandle);
	return true;
}

void CUtilities::LogDLLError(CString strArea) {
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

CString CUtilities::ComputerName() {
	DWORD nSize = MAX_COMPUTERNAME_LENGTH + 1;
	char chTemp[MAX_COMPUTERNAME_LENGTH + 1];
	GetComputerName(chTemp, &nSize);
	return (chTemp);
}

CString CUtilities::EXEPath() {
	char chTemp[MAX_PATH];
	GetModuleFileName(NULL, chTemp, MAX_PATH);
	*(strrchr(chTemp, '\\')+1) = '\0';  // cut off .exe file
	return (chTemp);
}

CString CUtilities::EXEName() {
	char chTemp[MAX_PATH];
	GetModuleFileName(NULL, chTemp, MAX_PATH);
	CString cTemp = chTemp;
	cTemp = cTemp.Mid(cTemp.ReverseFind('\\')+1);
	return (cTemp);
}

bool CUtilities::Exec(CString strCommand, CString strCmdLine)
{

	CString strMsg; int intResult;

	// check for setup exe
	while (!gUtils.FileExists(strCommand)) {
		strMsg.Format(IDS_SETUP_FNF, strCommand);
		intResult = MessageBox(NULL, strMsg, m_strAppName, MB_RETRYCANCEL | MB_ICONQUESTION | MB_TASKMODAL);
		if (intResult == IDCANCEL) {
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
	if (strCommand.Find(".msi") > 0) {
		strCmdLine = "msiexec.exe /i \"" + strCommand + "\" " + strCmdLine;
		strCommand = WinSysPath() + "\\msiexec.exe";
	}

	// launch process
	gLog.LogEvent("Starting command '" + strCommand + "', CmdLine '" + strCmdLine + "'");

	lReturn = CreateProcess(strCommand, strCmdLine.GetBuffer(strCmdLine.GetLength()+1), 
							NULL, NULL, true, NORMAL_PRIORITY_CLASS, NULL, NULL, &sInfo, &pInfo);
	strCmdLine.ReleaseBuffer();
	hProcess = pInfo.hProcess;

	if (!lReturn) {
		gUtils.LogDLLError("Setup launch failed ('" + strCommand + ").");
		return false;
	}

	// close handles
	if (pInfo.hProcess) CloseHandle(pInfo.hProcess);
	if (pInfo.hThread) CloseHandle(pInfo.hThread);

	return true;

}
