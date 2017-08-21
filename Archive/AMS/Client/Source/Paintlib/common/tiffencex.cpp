/*
/--------------------------------------------------------------------
|
|      $Id: tiffencex.cpp,v 1.4 2000/01/16 20:43:15 anonymous Exp $
|      TIFF Encoder Class
|
|      TIFF file encoder. Uses LIBTIFF to do the actual conversion.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"

// Not quite ready for prime-time; bdelmee; 2/99
// I mostly needed monochrome TIFF writing, which seems robust enough.
// Palette or RGB writing seems to work but has not been sufficiently tested.

#include <stdarg.h>
#include "tiffenc.h"
#include "tiffencex.h"
#include "bitmap.h"
#include "except.h"

extern "C"
{
#include "tiffio.h"   // for the tags definitions
#include "tif_msrc.h"
}


/////////////////////////////////////////////////////////////////////
// more format-specific encoder


CTIFFEncoderEx::CTIFFEncoderEx() : m_TiffToken(0)
{}



CTIFFEncoderEx::~CTIFFEncoderEx()
{
  Dissociate();
}


BOOL CTIFFEncoderEx::Associate( CDataSink* pDataSnk )
{
  m_TiffToken = TIFFOpenMem (pDataSnk->m_pStartData,
                             pDataSnk->m_nMaxFileSize,
                             &(pDataSnk->m_nCurPos));
  return m_TiffToken != 0;
}


void CTIFFEncoderEx::Dissociate()
{
  if ( m_TiffToken )
  {
    TIFFClose( m_TiffToken );
    m_TiffToken = 0;
  }
}


void CTIFFEncoderEx::DoEncode (CBmp * pBmp, CDataSink* /* pDataSnk */)
{
  PLASSERT( m_TiffToken );
  // call base version on open tiff descriptor
  CTIFFEncoder::DoTiffEncode( pBmp, m_TiffToken );
}


// The following two calls make their base class equivalent usable,
// without requiring the user to know about the libtiff internals (TIFF*)
int CTIFFEncoderEx::SetBaseTags( CBmp* pBmp )
{
  return CTIFFEncoder::SetBaseTags( m_TiffToken, pBmp );
}


int CTIFFEncoderEx::SetField( int tag_id, ... )
{
  int retv;
  va_list marker;

  va_start( marker, tag_id );     /* Initialize variable arguments. */
  retv = TIFFVSetField( m_TiffToken, tag_id, marker );
  va_end( marker );              /* Reset variable arguments.      */

  return retv;
}
/*
/--------------------------------------------------------------------
|
|      $Log: tiffencex.cpp,v $
|      Revision 1.4  2000/01/16 20:43:15  anonymous
|      Removed MFC dependencies
|
|      Revision 1.3  2000/01/10 23:53:00  Ulrich von Zadow
|      Changed formatting & removed tabs.
|
|      Revision 1.2  1999/12/02 17:07:34  Ulrich von Zadow
|      Changes by bdelmee.
|
|      Revision 1.1  1999/10/19 21:30:42  Ulrich von Zadow
|      B. Delmee - Initial revision
|
|
\--------------------------------------------------------------------
*/
