/*
|
|      $Id: tiffdecex.cpp,v 1.4 2000/01/16 20:43:15 anonymous Exp $
|      TIFF Decoder Class
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "tiffdec.h"
#include "tiffdecex.h"
#include "except.h"

extern "C"
{
#include "tif_msrc.h"
}


/////////////////////////////////////////////////////////////////////
// slightly more capable decoder...

CTIFFDecoderEx::CTIFFDecoderEx() : m_TiffToken(0)
{}



CTIFFDecoderEx::~CTIFFDecoderEx()
{
  Dissociate();
}


BOOL CTIFFDecoderEx::Associate( CDataSource* pDataSrc )
{
  m_TiffToken = TIFFOpenMem (pDataSrc->ReadEverything(),
                             pDataSrc->GetFileSize(),
                             NULL);
  return m_TiffToken != 0;
}


void CTIFFDecoderEx::Dissociate()
{
  if ( m_TiffToken )
  {
    TIFFClose( m_TiffToken );
    m_TiffToken = 0;
  }
}


void CTIFFDecoderEx::DoDecode (CBmp * pBmp, RGBAPIXEL ** ppPal, int * pDestBPP,
                               CDataSource * pDataSrc)
{
  PLASSERT( m_TiffToken );

  // call base version on associated data source
  CTIFFDecoder::DoTiffDecode(
    pBmp,
    ppPal,
    pDestBPP,
    pDataSrc,
    m_TiffToken
  );
}

// It'd be nicer to define a bunch of type-safe functions like:
//    uint32  CTIFFDecoderEx::GetImageLength();
//    CString CTIFFDecoderEx::GetImageDescription();
int CTIFFDecoderEx::GetField( int tag_id, ... )
{
  va_list marker;
  int retv;

  va_start( marker, tag_id );     /* Initialize variable arguments. */
  retv = TIFFVGetFieldDefaulted( m_TiffToken, (ttag_t) tag_id, marker );
  va_end( marker );              /* Reset variable arguments.      */

  return retv;
}

/*
/--------------------------------------------------------------------
|
|      $Log: tiffdecex.cpp,v $
|      Revision 1.4  2000/01/16 20:43:15  anonymous
|      Removed MFC dependencies
|
|      Revision 1.3  2000/01/10 23:53:00  Ulrich von Zadow
|      Changed formatting & removed tabs.
|
|      Revision 1.2  2000/01/09 22:24:10  Ulrich von Zadow
|      Corrected tiff callback bug.
|
|      Revision 1.1  1999/10/19 21:30:42  Ulrich von Zadow
|      B. Delmee - Initial revision
|
|
\--------------------------------------------------------------------
*/
