/*
/--------------------------------------------------------------------
|
|      $Id: trace.cpp,v 1.5 2000/01/16 20:43:15 anonymous Exp $
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/
#include "stdpch.h"

#ifdef _DEBUG

#include <stdarg.h>

void PLTrace(const char * pszFormat, ...)
{
  va_list args;
  va_start(args, pszFormat);

  int nBuf;
  char szBuffer[4096];

  nBuf = vsprintf(szBuffer, pszFormat, args);
  PLASSERT(nBuf < 4096);

#ifndef _WINDOWS
  fprintf (stderr, szBuffer);
#else
  ::OutputDebugString (szBuffer);
#endif //_WINDOWS

  va_end(args);
}

#endif //_DEBUG

/*
/--------------------------------------------------------------------
|
|      $Log: trace.cpp,v $
|      Revision 1.5  2000/01/16 20:43:15  anonymous
|      Removed MFC dependencies
|
|      Revision 1.4  2000/01/10 23:53:00  Ulrich von Zadow
|      Changed formatting & removed tabs.
|
|      Revision 1.3  1999/10/03 18:50:52  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
\--------------------------------------------------------------------
*/
