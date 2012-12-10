/*
/--------------------------------------------------------------------
|
|      $Id: except.cpp,v 1.4 2000/01/16 20:43:13 anonymous Exp $
|      EXCEPT.CPP        Exception Class
|
|        Exception containing an error code and a string
|        describing the error in a user-friendly way.
|        The header file defines the error codes used.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"

#include "except.h"


CTextException::CTextException
    ( int Code,
      const char * pszErr
    )
{
#ifdef _WINDOWS
  SetErrorMode (0);     // Restore normal error handling just in case
                        // file system error checking was disabled.
#endif

  m_pszErr = new char[strlen(pszErr)+1];
  strcpy (m_pszErr, pszErr);

  m_Code = Code;
}

CTextException::CTextException
    ( const CTextException& ex
    )
{
  m_Code = ex.GetCode();
  m_pszErr = new char [strlen((const char *)ex)+1];
  strcpy (m_pszErr, (const char *)ex);
}

CTextException::~CTextException
    ()
{
  delete m_pszErr;
}

int CTextException::GetCode
    ()
    const
{
  return m_Code;
}

CTextException::operator const char *
    ()
    const
    // This operator allows the exception to be treated as a string
    // whenever needed.
{
  return m_pszErr;
}
/*
/--------------------------------------------------------------------
|
|      $Log: except.cpp,v $
|      Revision 1.4  2000/01/16 20:43:13  anonymous
|      Removed MFC dependencies
|
|      Revision 1.3  1999/10/03 18:50:51  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
--------------------------------------------------------------------
*/
