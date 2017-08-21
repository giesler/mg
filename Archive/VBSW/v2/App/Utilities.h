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
	CString EXEName();
	CString EXEPath();
	CString ComputerName();
	void LogDLLError(CString strArea);
	bool FileExists(CString p_strFileName);
	static CString TempPath();
	static CString WinSysPath();
	static CString WinPath();
	static CString CommonFilesPath();
	bool Exec(CString, CString);
	CString m_strAppName;

private:
};

#endif // !defined(AFX_UTILITIES_H__DAA5CE40_E0D8_4C6A_A771_446FED9E3BC1__INCLUDED_)
