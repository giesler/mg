/*
/--------------------------------------------------------------------
|
|      $Id: memsrc.cpp,v 1.1 2000/03/17 10:51:38 Ulrich von Zadow Exp $
|      Memory Data Source Class
|
|      This is a class which takes a memory region as a source of
|      picture data.
|      Original author: Patrick Strömstedt.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"

#include "memsrc.h"
#include "except.h"


CMemSource::CMemSource() : CDataSource (NULL)  // No progress notification nessesary when
                        // reading from memory.
{
  m_pCurPos = NULL;
}

CMemSource::~CMemSource
    ()
{
  if (m_pCurPos)
    Close();
}

int CMemSource::Open (unsigned char *pek, int size)
{
  CDataSource::Open ("Mem", size);
  m_pCurPos = pek;
  return 0;
}

void CMemSource::Close
    ()
{
  m_pCurPos = NULL;
  CDataSource::Close();
}

BYTE * CMemSource::ReadNBytes
    ( int n
    )
{
  CDataSource::ReadNBytes(n);

  m_pCurPos += n;
  return m_pCurPos-n;
}

BYTE * CMemSource::ReadEverything
    ()
{
  return m_pCurPos;  // ;-)
}

// Read but don't advance file pointer.
BYTE * CMemSource::GetBufferPtr
    ( int MinBytesInBuffer
    )
{
  return m_pCurPos;
}

/*
/--------------------------------------------------------------------
|
|      $Log: memsrc.cpp,v $
|      Revision 1.1  2000/03/17 10:51:38  Ulrich von Zadow
|      no message
|
|
\--------------------------------------------------------------------
*/
