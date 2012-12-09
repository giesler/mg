/*
/--------------------------------------------------------------------
|
|      $Id: ressrc.cpp,v 1.5 2000/01/16 20:43:17 anonymous Exp $
|      Resource Data Source Class
|
|      This is a class which takes a windows resource as a source of
|      picture data.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"

#include "ressrc.h"
#include "except.h"


CResourceSource::CResourceSource
    ()
  : CDataSource (NULL)  // No progress notification nessesary when
                        // reading from resources.
{
  m_hRsrc = NULL;
  m_hGlobal = NULL;
}

CResourceSource::~CResourceSource
    ()
{
  if (m_hGlobal)
    Close();
}

int CResourceSource::Open (HINSTANCE lh_ResInst, int ResourceID, const char * pResType)
{
  if (!pResType)
    pResType= RT_RCDATA;

  m_hRsrc = FindResource (lh_ResInst, MAKEINTRESOURCE (ResourceID),
                          pResType);
  if (!m_hRsrc)
  {
    char sz[256];
    sprintf (sz, "FindResource Failed (Code %i).\n", GetLastError());
    PLTRACE (sz);
    return ERR_FILE_NOT_FOUND;
  }

  m_hGlobal = LoadResource (lh_ResInst, m_hRsrc);
  if (!m_hGlobal)
  {
    char sz[256];
    sprintf (sz, "LoadResource Failed (Code %i).\n", GetLastError());
    PLTRACE (sz);
    return ERR_FILE_NOT_FOUND;
  }

  BYTE * pBuffer;
  pBuffer = (BYTE *)LockResource (m_hGlobal);
  if (!pBuffer)
  {
    PLTRACE ("LockResource failed.\n");
    return (ERR_FILE_NOT_FOUND);
  }
  int DataLen = ::SizeofResource (lh_ResInst, m_hRsrc);

  // We've got the resource mapped to memory
  char sz[256];
  sprintf (sz, "Resource: %i", ResourceID);
  CDataSource::Open (sz, DataLen);
  m_pCurPos = pBuffer;
  return 0;
}

void CResourceSource::Close
    ()
{
  UnlockResource(m_hGlobal);
  FreeResource(m_hGlobal);
  m_hGlobal = NULL;
  CDataSource::Close();
}

BYTE * CResourceSource::ReadNBytes
    ( int n
    )
{
  CDataSource::ReadNBytes(n);
  m_pCurPos += n;
  return m_pCurPos-n;
}

BYTE * CResourceSource::ReadEverything
    ()
{
  return m_pCurPos;  // ;-)
}

// Read but don't advance file pointer.
BYTE * CResourceSource::GetBufferPtr
    ( int MinBytesInBuffer
    )
{
  return m_pCurPos;
}

/*
/--------------------------------------------------------------------
|
|      $Log: ressrc.cpp,v $
|      Revision 1.5  2000/01/16 20:43:17  anonymous
|      Removed MFC dependencies
|
|      Revision 1.4  2000/01/11 22:07:11  Ulrich von Zadow
|      Added instance handle parameter.
|
|      Revision 1.3  1999/11/02 21:20:06  Ulrich von Zadow
|      AfxFindResourceHandle statt AfxGetInstanceHandle
|      verwendet.
|
|
\--------------------------------------------------------------------
*/
