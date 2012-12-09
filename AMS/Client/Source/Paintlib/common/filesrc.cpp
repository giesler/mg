/*
/--------------------------------------------------------------------
|
|      $Id: filesrc.cpp,v 1.6 2000/01/16 20:43:13 anonymous Exp $
|      File Data Source Class
|
|      This is a class which takes a file as a source of picture data.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"

#include "filesrc.h"
#include "except.h"


CFileSource::CFileSource
    ( IProgressNotification * pNotification
    )
  : CDataSource (pNotification)
{
#ifdef FILE_MAPPING
  m_hf = NULL;
  m_hm = NULL;
#else
  m_pFile = NULL;
#endif
}

CFileSource::~CFileSource
    ()
{
#ifdef FILE_MAPPING
  if (m_hf)
    Close();
#else
  if (m_pFile)
    Close();
#endif
}

int CFileSource::Open
    ( const char * pszFName
    )
{
  int FileSize;

#ifdef FILE_MAPPING
  BYTE * pBuffer = NULL;
  try
  {
    m_hf = CreateFile (pszFName, GENERIC_READ, FILE_SHARE_READ, NULL,
                       OPEN_EXISTING,
                       FILE_ATTRIBUTE_NORMAL | FILE_FLAG_SEQUENTIAL_SCAN,
                       NULL);

    if (m_hf == INVALID_HANDLE_VALUE)
    {
      switch (GetLastError())
      {
        case ERROR_PATH_NOT_FOUND:
          // sprintf (sz, "Path not found.", pszFName);
          return ERR_PATH_NOT_FOUND;
        case ERROR_FILE_NOT_FOUND:
          // sprintf (sz, "File not found.", pszFName);
          return ERR_FILE_NOT_FOUND;
        case ERROR_ACCESS_DENIED:
          // sprintf (sz, "Access denied.", pszFName);
          return ERR_ACCESS_DENIED;
        case ERROR_SHARING_VIOLATION:
          // sprintf (sz, "Sharing violation.", pszFName);
          return ERR_ACCESS_DENIED;
        default:
          // sprintf (sz, "CreateFile returned %d.",
          //          pszFName, GetLastError());
          return ERR_FILE_NOT_FOUND;
      }
    }

    FileSize = ::GetFileSize (m_hf, NULL);

    m_hm = CreateFileMapping (m_hf, NULL, PAGE_READONLY, 0, 0, NULL);

    // This happens if the file is empty.
    if (m_hm == NULL)
    {  // raiseError (ERR_ACCESS_DENIED, "CreateFileMapping failed.");
      if (m_hf) CloseHandle (m_hf);
      m_hf = NULL;
      return ERR_ACCESS_DENIED;
    }

    pBuffer = (BYTE *) MapViewOfFile (m_hm, FILE_MAP_READ, 0, 0, 0);

    if (pBuffer == NULL)
      // raiseError (ERR_ACCESS_DENIED, "MapViewOfFile failed.");
      return ERR_ACCESS_DENIED;
    m_pStartData = pBuffer;
    m_pCurPos = pBuffer;

    // We've got the file mapped to memory.
    CDataSource::Open (pszFName, FileSize);
  }
  catch (CTextException)
  {
    // Clean up on error
    if (pBuffer) UnmapViewOfFile (pBuffer);
    if (m_hm) CloseHandle (m_hm);
    if (m_hf) CloseHandle (m_hf);
    throw;
  }
  return 0;

#else

  // Generic code assuming memory mapped files are not available.
  m_pFile = NULL;
  if (strcmp (pszFName, ""))
    m_pFile = fopen (pszFName, "rb");

  if (m_pFile == NULL)
  { // Crude...
    m_pFile = 0;
    return -1;
  }

  // Determine file size. Can this be done in an easier way using ANSI C?
  fseek (m_pFile, 0, SEEK_END);
  FileSize = ftell (m_pFile);
  fseek (m_pFile, 0, SEEK_SET);

  // Create a buffer for the file.
  m_pBuffer = new BYTE[FileSize];
  //  this only works if our implementation of "new" does not throw...
  if (m_pBuffer == 0)
  {
  	fclose (m_pFile);
	  return -1;
  }

  m_pReadPos = m_pBuffer;
  m_pCurPos = m_pBuffer;
  m_BytesReadFromFile = 0;
  CDataSource::Open (pszFName, FileSize);
  fillBuffer ();
  return 0;
#endif
}

void CFileSource::Close
    ()
{
#ifdef FILE_MAPPING
  UnmapViewOfFile (m_pStartData);
  CDataSource::Close ();
  CloseHandle (m_hm);
  CloseHandle (m_hf);
  m_hm = NULL;
  m_hf = NULL;
#else
  free (m_pBuffer);
  m_pBuffer = NULL;
  CDataSource::Close ();
  fclose (m_pFile);
  m_pFile = NULL;
#endif
}

BYTE * CFileSource::ReadNBytes
    ( int n
    )
{
  CDataSource::ReadNBytes(n);
#ifndef FILE_MAPPING
  PLASSERT (n < 4096);
  if (!bytesAvailable(n))
    fillBuffer();
#endif
  m_pCurPos += n;
  return m_pCurPos-n;
}

//! Read but don't advance file pointer.
BYTE * CFileSource::GetBufferPtr
    ( int MinBytesInBuffer
    )
{
  PLASSERT (MinBytesInBuffer < 4096);
#ifndef FILE_MAPPING
  if (!bytesAvailable(MinBytesInBuffer))
    fillBuffer();
#endif
  return m_pCurPos;
}

BYTE * CFileSource::ReadEverything
    ()
{
#ifdef FILE_MAPPING
  return m_pCurPos;
#else
  int BytesToRead = GetFileSize()-m_BytesReadFromFile;
  int i = fread (m_pReadPos, 1, BytesToRead, m_pFile);
  PLASSERT (i==BytesToRead);
  m_BytesReadFromFile += BytesToRead;
  m_pReadPos += BytesToRead;
  return m_pCurPos;
#endif
}


#ifndef FILE_MAPPING
void CFileSource::fillBuffer
    ()
{
  // bdelmee; code change for the sake of portability
  int BytesToRead = GetFileSize() - m_BytesReadFromFile;
  if ( BytesToRead > 4096 ) BytesToRead = 4096;
  int i = fread (m_pReadPos, 1, BytesToRead, m_pFile);
  PLASSERT (i==BytesToRead);
  m_BytesReadFromFile += BytesToRead;
  m_pReadPos += BytesToRead;
}

BOOL CFileSource::bytesAvailable
    ( int n
    )
{
  if (m_pReadPos-m_pCurPos >= n)
    return TRUE;
  else
    return FALSE;
}
#endif
/*
/--------------------------------------------------------------------
|
|      $Log: filesrc.cpp,v $
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
--------------------------------------------------------------------
*/
