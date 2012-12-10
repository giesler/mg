/*
|
|      $Id: picenc.cpp,v 1.6 2000/01/16 20:43:14 anonymous Exp $
|      Generic Picture Encoder Class
|
|      Abstract base class to dump picture data to memory or file.
|      Classes derived from this class implement concrete encoders
|      for specific file formats.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

// not quite ready for prime-time; bdelmee; 2/99

#include "stdpch.h"
#include "picenc.h"
#include "filesink.h"
#include "except.h"
// only for the tracing facility
#include "picdec.h"


CPicEncoder::CPicEncoder()
// Creates an encoder
{}


CPicEncoder::~CPicEncoder()
{}


// Encodes a picture by creating a file data sink and
// calling SaveBmp with this data sink.

void CPicEncoder::MakeFileFromBmp (const char * pszFName, CBmp * pBmp)
{
  CFileSink FileSink;
  int err;

  char sz[256];
  sprintf (sz, "--- Encoding file %s. ---\n", pszFName);
  Trace (1, sz);

  // We allocate a buffer large enough to hold the raw bitmap
  // plus some overhead.In most cases this should be enough to
  // hold the uncompressed data in any format, plus headers, etc...
  // Some "pathological" cases however may end up with a CODEC
  // producing more data than the uncompressed version!
  int bufsize = pBmp->GetMemUsed();
  bufsize = bufsize < 20000 ? bufsize + 4096 : int(1.2 * bufsize);
  err = FileSink.Open( pszFName, bufsize );
  if (err)
  {
    sprintf (sz, "Opening %s failed", pszFName);
    raiseError (err, sz);
  }

  SaveBmp ( pBmp, &FileSink );
  FileSink.Close ();
}

// Encodes a picture by getting the encoded data from pDataSrc.
// Saves the results to pDataSrc
// Actually, a wrapper so thin around "DoEncode", you could see through...
void CPicEncoder::SaveBmp (CBmp* pBmp, CDataSink* pDataSnk)
{
  DoEncode( pBmp, pDataSnk );
}

////////////////////
// As long as the tracing code lives in the base decoder,
// we'll just forward everything to it without bothering the user

void CPicEncoder::SetTraceConfig( int Level, char * pszFName )
{
  CPicDecoder::SetTraceConfig( Level, pszFName );
}

void CPicEncoder::raiseError( int Code, char * pszErr )
{
  CPicDecoder::raiseError( Code, pszErr );
}

void CPicEncoder::Trace( int TraceLevel, const char * pszMessage )
{
  CPicDecoder::Trace( TraceLevel, pszMessage );
}
/*
/--------------------------------------------------------------------
|
|      $Log: picenc.cpp,v $
|      Revision 1.6  2000/01/16 20:43:14  anonymous
|      Removed MFC dependencies
|
|      Revision 1.5  2000/01/10 23:52:59  Ulrich von Zadow
|      Changed formatting & removed tabs.
|
|      Revision 1.4  2000/01/08 15:51:30  Ulrich von Zadow
|      Misc. modifications to png encoder.
|
|      Revision 1.3  1999/10/03 18:50:51  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
--------------------------------------------------------------------
*/
