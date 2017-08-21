// Component.h: interface for the CComponent class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_COMPONENT_H__8C1AD7E7_E03F_4309_89A7_C69AE1765DCE__INCLUDED_)
#define AFX_COMPONENT_H__8C1AD7E7_E03F_4309_89A7_C69AE1765DCE__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

enum ComponentType {
	FileVersionCheck = 0,
	RegVersionCheck,
	RegKeyCheck,
	NTServicePackCheck,
	NoCheck,
	NetFrameworkCheck
};

enum RebootType {
	NoReboot = 0, 
	BatchReboot,
	ImmediateReboot
};

class CComponent  
{
public:
	bool InstallAttempted();
	bool Install(CSetupDlg* &sDlg);
	bool RunRegKeyCheck();
	bool RunNTServicePackCheck();
	bool RunNetFrameworkCheck();
	void ReplaceDirs(CString &p_strIn);
	bool RunFileVersionCheck();
	bool mblnInstalled;
	void CheckComponent();
	bool mblnListed;
	
	// constructor / destructors
	CComponent();
	~CComponent();
	CComponent (const CComponent& src);
	CComponent operator=(CComponent& rhs);

	bool IsComponentInstalled();
	bool Load(CString sSectionName);
	int InstallTime();

	// basic settings
	CString mstrId;
	CString mstrName;
	ComponentType mtypComponentType;
	RebootType mtypReboot;
	CList<CString, CString&> mlstDepends;
	CList<CString, CString&> mlstIncludes;
	CString mstrSetupCommand;
	CString mstrSetupCommandLine;
	CString mstrSetupMessage;
	int mintSetupTime;
	bool mblnQuietInstall;

	// for CTFileVersionCheck
	CString mstrFileVersionCheckDLL;
	CString mstrFileVersionCheckVersion;

	// for CTRegVersionCheck
	CString mstrRegVersionCheckKey;
	CString mstrRegVersionCheckVersion;

	// for CTRegKeyCheck
	CString mstrRegKeyCheckKey;
	CString mstrRegKeyCheckValue;

	// for NTServicePackCheck
	CString mstrNTServicePackCheckNumber;

	// for Net framework check
	CString mstrNetFrameworkCheckVersion;

	// whether or not to install this component
	bool mblnInstall;

private:
	bool RunRegVersionCheck();
	void SplitRegEntry(CString pstrKey, HKEY & hKey, CString & strSubKey, 
		CString & strValueName);
	void SplitVersionString(CString p_strVersion, DWORD &dwMajorVersion, 
		DWORD &dwMinorVersion, DWORD &dwBuildNumber, DWORD &dwRevision);
};

#endif // !defined(AFX_COMPONENT_H__8C1AD7E7_E03F_4309_89A7_C69AE1765DCE__INCLUDED_)
