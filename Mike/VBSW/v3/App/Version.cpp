// FileVersion.cpp: implementation of the CFileVersion class.
// by Manuel Laflamme 
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "Version.h"

#pragma comment(lib, "version")

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////

CFileVersion::CFileVersion() 
{ 
    m_lpVersionData = NULL;
    m_dwLangCharset = 0;
}


CFileVersion::~CFileVersion() 
{ 
    Close();
} 


void CFileVersion::Close()
{
    delete[] m_lpVersionData; 
    m_lpVersionData = NULL;
    m_dwLangCharset = 0;
}


BOOL CFileVersion::Open(LPCTSTR lpszModuleName)
{
    ASSERT(_tcslen(lpszModuleName) > 0);
    ASSERT(m_lpVersionData == NULL);

    // Get the version information size for allocate the buffer
    DWORD dwHandle;     
    DWORD dwDataSize = ::GetFileVersionInfoSize((LPTSTR)lpszModuleName, &dwHandle); 
    if ( dwDataSize == 0 ) 
        return FALSE;

    // Allocate buffer and retrieve version information
    m_lpVersionData = new BYTE[dwDataSize]; 
    if (!::GetFileVersionInfo((LPTSTR)lpszModuleName, dwHandle, dwDataSize, 
	                          (void**)m_lpVersionData) )
    {
        Close();
        return FALSE;
    }

    // Retrieve the first language and character-set identifier
    UINT nQuerySize;
    DWORD* pTransTable;
    if (!::VerQueryValue(m_lpVersionData, _T("\\VarFileInfo\\Translation"),
                         (void **)&pTransTable, &nQuerySize) )
    {
        Close();
        return FALSE;
    }

    // Swap the words to have lang-charset in the correct format
    m_dwLangCharset = MAKELONG(HIWORD(pTransTable[0]), LOWORD(pTransTable[0]));

    return TRUE;
}


BOOL CFileVersion::GetFixedInfo(VS_FIXEDFILEINFO& vsffi)
{
    // Must call Open() first
    ASSERT(m_lpVersionData != NULL);
    if ( m_lpVersionData == NULL )
        return FALSE;

    UINT nQuerySize;
	VS_FIXEDFILEINFO* pVsffi;
    if ( ::VerQueryValue((void **)m_lpVersionData, _T("\\"),
                         (void**)&pVsffi, &nQuerySize) )
    {
        vsffi = *pVsffi;
        return TRUE;
    }

    return FALSE;
}


CString CFileVersion::GetFixedFileVersion()
{
    CString strVersion;
	VS_FIXEDFILEINFO vsffi;

    if ( GetFixedInfo(vsffi) )
    {
        strVersion.Format ("%u,%u,%u,%u",HIWORD(vsffi.dwFileVersionMS),
            LOWORD(vsffi.dwFileVersionMS),
            HIWORD(vsffi.dwFileVersionLS),
            LOWORD(vsffi.dwFileVersionLS));
    }
    return strVersion;
}


VERSION_INFO CFileVersion::GetFixedFileVersionInfo() 
{
    CString strVersion;
	VS_FIXEDFILEINFO vsffi;
	VERSION_INFO vinfo;

    if ( GetFixedInfo(vsffi) )
    {
				vinfo.dwMajorVerison = HIWORD(vsffi.dwFileVersionMS);
				vinfo.dwMinorVersion = LOWORD(vsffi.dwFileVersionMS);
				vinfo.dwBuildNumber  = HIWORD(vsffi.dwFileVersionLS);
				vinfo.dwRevision     = LOWORD(vsffi.dwFileVersionLS);
    }
    return vinfo;
}

