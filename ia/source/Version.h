// FileVersion.h: interface for the CFileVersion class.
// by Manuel Laflamme
//////////////////////////////////////////////////////////////////////

#ifndef __FILEVERSION_H_
#define __FILEVERSION_H_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

struct VERSION_INFO {
	DWORD dwMajorVerison;
	DWORD dwMinorVersion;
	DWORD dwBuildNumber;
	DWORD dwRevision;
};

class CFileVersion
{ 
// Construction
public: 
    CFileVersion();

// Operations	
public: 
    BOOL    Open(LPCTSTR lpszModuleName);
    void    Close();

	VERSION_INFO GetFixedFileVersionInfo();

    BOOL    GetFixedInfo(VS_FIXEDFILEINFO& vsffi);
    CString GetFixedFileVersion();
//    CString GetFixedProductVersion();

// Attributes
protected:
    LPBYTE  m_lpVersionData; 
    DWORD   m_dwLangCharset; 

// Implementation
public:
    ~CFileVersion(); 
}; 

#endif  // __FILEVERSION_H_
