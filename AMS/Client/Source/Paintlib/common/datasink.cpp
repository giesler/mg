/*
/--------------------------------------------------------------------
|
|      $Id: datasink.cpp,v 1.5 2000/01/16 20:43:13 anonymous Exp $
|      Data Destination Base Class
|
|      This is a base class for a destination of picture data.
|      It defines methods to open, write to, and close data sources.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"

// not quite ready for prime-time; bdelmee; 2/99
// does not (yet?) offer support for endiannes independency
// and the such, as this is not needed for TIFF encoding

#include "datasink.h"


CDataSink::CDataSink
    ()
{
  m_pszName = NULL;
  m_pStartData = NULL;
  m_nCurPos = 0;
}


CDataSink::~CDataSink
    ()
{
  if (m_pStartData)
    Close();
}


// the actual data buffer is allocated in the derived classes
void CDataSink::Open
    ( const char * pszName,
      BYTE*  pData,
      size_t MaxFileSize
    )
{
  // Data source may not be open already!
  PLASSERT (! m_pStartData);
  PLASSERT (MaxFileSize > 0);

  m_nMaxFileSize = MaxFileSize;
  // unchecked memory allocation, here!
  m_pszName = new char [strlen (pszName)+1];
  strcpy (m_pszName, pszName);
  m_pStartData = pData;
  m_nCurPos = 0;
}


void CDataSink::Close
    ()
{
  if (m_pszName)
  { delete m_pszName; m_pszName = NULL; }
    m_pStartData = NULL;
  m_nCurPos = 0;
}


char * CDataSink::GetName
    ()
{
  return m_pszName;
}

size_t CDataSink::WriteNBytes
    ( size_t   n,
    BYTE* pData
    )
{
  if (m_nCurPos+n > m_nMaxFileSize)
    throw CTextException (ERR_END_OF_FILE,
          "Buffer overflow while encoding.\n");

  memcpy(m_pStartData + m_nCurPos, pData, n);
  m_nCurPos += n;
  return n;
}

/*
/--------------------------------------------------------------------
|
|      $Log: datasink.cpp,v $
|      Revision 1.5  2000/01/16 20:43:13  anonymous
|      Removed MFC dependencies
|
|      Revision 1.4  1999/10/19 21:23:23  Ulrich von Zadow
|      Martin Skinner: Added WriteNBytes()
|
|      Revision 1.3  1999/10/03 18:50:51  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
--------------------------------------------------------------------
*/
