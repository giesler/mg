// Utilities.h: interface for the CUtilities class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_UTILITIES_H__DAA5CE40_E0D8_4C6A_A771_446FED9E3BC1__INCLUDED_)
#define AFX_UTILITIES_H__DAA5CE40_E0D8_4C6A_A771_446FED9E3BC1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class CUtilities  
{
public:
	bool ValidateSP(CString sp, CString componentId, bool minSP);
	CUtilities();
	~CUtilities();
	CString EXEName();
	CString EXEPath();
	CString ComputerName();
	void LogDLLError(CString strArea);
	bool FileExists(CString p_strFileName);
	static CString TempPath();
	static CString WinSysPath();
	static CString WinPath();
	static CString CommonFilesPath();
	static void ReplaceDirs(CString& p_strIn);
	CString GetINIString(CString sectionName, CString keyName, CString defaultValue);
	bool GetINIBool(CString sectionName, CString keyName, bool defaultValue);
	int GetINIInt(CString sectionName, CString keyName, int defaultValue);
	int GetINISection(CString sectionName, CString sectionContents[20][2]);
	void PlaySoundFile(CString sound);
	bool ValidateOS(CString iniSection, CString & result);

	bool Exec(CString strCommand, CString strCmdLine, bool waitForCompletion, bool shellExecute);

private:
	CString m_strFileName;
	TCHAR * m_pReturnedString; 
	CString m_strAppName;
	
};

#endif // !defined(AFX_UTILITIES_H__DAA5CE40_E0D8_4C6A_A771_446FED9E3BC1__INCLUDED_)
