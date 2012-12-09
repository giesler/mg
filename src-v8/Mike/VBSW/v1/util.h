// util.h: interface for the util class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_UTIL_H__1F13CAD3_BF51_4AA5_9AF7_78EC8238EDF1__INCLUDED_)
#define AFX_UTIL_H__1F13CAD3_BF51_4AA5_9AF7_78EC8238EDF1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class util  
{
public:
	bool InstallComCtlDLL();
	bool IsComCtlDLLInstalled();
	bool InstallIE();
	bool bCheckIE;
	bool IsIEInstalled();
	util();
	virtual ~util();

	bool IsDCOMInstalled();
	bool IsVBRTInstalled();
	bool IsMDACInstalled();
	bool IsInstInstalled();
	bool IsComCtlInstalled();
	bool IsWinNT();
	bool IsWin2000();
	
	bool InstallDCOM();
	bool InstallVBRT();
	bool InstallMDAC();
	bool InstallInst();
	bool InstallComCtl();

	void SetEstimatedTime(int);
	void DisplayError();
	CString EXEPath();
	CString EXEName();

	bool FileVerInfo(CString sFileName, VS_FIXEDFILEINFO* &ver);

	bool InstallAndWait(CString, CString, int, bool=false);
	bool RebootComp(CString);
	bool SysUpdates(HWND);
	bool GetSettings();

	CString sAppName;
	bool bCheckMDAC;
	bool bCheckVBRT;
	bool bCheckDCOM;
	bool bCheckMSI;
	bool bCheckComCtl;
	bool bCheckComCtlDLL;

	CString GetAppTitle();
	CString GetBMPFileName();
	CString Domain();
	bool util::FileExists(CString);

	void RemoveMDACKey();

	CSetupDlg sDlg;
	CString sSetup;
	CString sUserName;
	CString sPassword;
	CString sDomain;
	bool bAltCredentials;

private:
	CString WinSysDir();
	CString WinDir();
	CString CompName();
	bool CheckFileVersion(VS_FIXEDFILEINFO*, DWORD, DWORD,	DWORD, DWORD);
#if(WINVER >= 0x0500 && _WIN32_WINNT >= 0x0500)
	bool LogonAttempt(CString sUserName, CString sPassword, CString sDomain);
#endif
};

#endif // !defined(AFX_UTIL_H__1F13CAD3_BF51_4AA5_9AF7_78EC8238EDF1__INCLUDED_)
