/*
|
|      $Id: filesink.cpp,v 1.5 2000/01/16 20:43:13 anonymous Exp $
|      File "Data Sink" Class
|
|      This class a file as a destination for picture data.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"

// not quite ready for prime-time; bdelmee; 2/99
// this is a very simple-minded implementation suitable for
// the needs of the TIFF encoder at this point

#include "filesink.h"
#include "except.h"


CFileSink::CFileSink ()
{
  m_pFile = NULL;
}

CFileSink::~CFileSink ()
{
  if (m_pFile)
    Close();
}

// Generic code assuming memory mapped files are not available.
int CFileSink::Open (const char * pszFName, int MaxFileSize)
{

  // we could actually open the file in "Close()",
  // but if e.g we cannot create it, it's pointless to proceed
  if ((m_pFile = fopen (pszFName, "wb")) &&
      (m_pDataBuf = new BYTE [MaxFileSize]))
  {
    CDataSink::Open(pszFName, m_pDataBuf, MaxFileSize);
    return 0;
  }
  else
    return -1;
}

// now flush the data to disk
void CFileSink::Close ()
{
  int towrite = GetDataSize();
  int written = fwrite( m_pStartData, 1, towrite, m_pFile );
  PLASSERT( written == towrite );
  fclose( m_pFile );
  m_pFile = 0;

  if (m_pDataBuf)
  {
    delete m_pDataBuf;
    m_pDataBuf = NULL;
  }
}

/*
/--------------------------------------------------------------------
|
|      $Log: filesink.cpp,v $
|      Revision 1.5  2000/01/16 20:43:13  anonymous
|      Removed MFC dependencies
|
|      Revision 1.4  2000/01/10 23:52:59  Ulrich von Zadow
|      Changed formatting & removed tabs.
|
|      Revision 1.3  1999/10/03 18:50:51  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
--------------------------------------------------------------------
*/
