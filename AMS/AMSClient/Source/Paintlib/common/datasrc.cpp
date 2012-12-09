/*
/--------------------------------------------------------------------
|
|      $Id: datasrc.cpp,v 1.6 2000/01/16 20:43:13 anonymous Exp $
|      Data Source Base Class
|
|      This is a base class for a source of picture data.
|      It defines methods to open, close, and read from data sources.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"

#include "datasrc.h"
#include "prognot.h"


CDataSource::CDataSource
    ( IProgressNotification * pNotification
    )
{
  m_pszName = NULL;
  m_FileSize = 0;
  m_pNotification = pNotification;
}


CDataSource::~CDataSource
    ()
{
}


void CDataSource::Open
    ( const char * pszName,
      int    FileSize
    )
{
  // Data source may not be open already!
  PLASSERT (!m_FileSize);

  m_pszName = new char [strlen (pszName)+1];
  strcpy (m_pszName, pszName);
  m_FileSize = FileSize;
  m_BytesRead = 0;
}


void CDataSource::Close
    ()
{
  if (m_pNotification)
    // be smart and tell the world: ich habe fertig!
    m_pNotification->OnProgress( 1);
  delete m_pszName;
  m_pszName = NULL;
}


char * CDataSource::GetName
    ()
{
  return m_pszName;
}

BYTE * CDataSource::ReadNBytes
    ( int n
    )
{
  int OldBytesRead = m_BytesRead;
  m_BytesRead += n;
  if (m_BytesRead/1024 > OldBytesRead/1024 && m_pNotification)
    m_pNotification->OnProgress (double(m_BytesRead)/m_FileSize);
  CheckEOF();
  return NULL;
}

// Jo Hagelberg 15.4.99:
// for use by other libs that handle progress internally (eg libjpeg)
void CDataSource::OProgressNotification
    ( double part
    )
{
  if( m_pNotification)
    m_pNotification->OnProgress( part);
}
/*
/--------------------------------------------------------------------
|
|      $Log: datasrc.cpp,v $
|      Revision 1.6  2000/01/16 20:43:13  anonymous
|      Removed MFC dependencies
|
|      Revision 1.5  1999/12/08 15:39:45  Ulrich von Zadow
|      Unix compatibility changes
|
|      Revision 1.4  1999/10/03 18:50:51  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
\--------------------------------------------------------------------
*/
