/*
/--------------------------------------------------------------------
|
|      $Id: jpegenc.cpp,v 1.3 2000/01/16 20:43:13 anonymous Exp $
|     
|      JPEG file encoder. Uses the independent JPEG group's library
|      to do the actual conversion.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#define JPEG_INTERNALS

extern "C"
{
#include "jpeglib.h"
}

#include "jpegenc.h"
#include "except.h"
#include "jmemdest.h"


// Check to see if the pixel format defined in jmorecfg.h matches the
// one defined in bitmap.h.
// If the program reports an error here, you're probably using a wrong
// version of jmorecfg.h. Several files in libjpeg and libtiff need to
// replaced for the libraries to be used with paintlib. See the
// documentation for details.
#if (RGBA_RED != RGB_RED) || (RGBA_GREEN != RGB_GREEN)   \
    || (RGBA_BLUE != RGB_BLUE)
#error Illegal JPEG pixel format
#endif
#if (RGB_PIXELSIZE != PIXEL_SIZE)
#error Illegal JPEG pixel format
#endif

/////////////////////////////////////////////////////////////////////
// Error handling.

METHODDEF(void)
error_exit (j_common_ptr pcinfo)
// This procedure is called by the IJPEG library when an error
// occurs.
{
  /* Create the message string */
  char sz[256];
  (pcinfo->err->format_message) (pcinfo, sz);
  strcat (sz, "\n");

  CPicEncoder::raiseError (ERR_FORMAT_NOT_SUPPORTED, sz);
}

/////////////////////////////////////////////////////////////////////
// Class functions

CJPEGEncoder::CJPEGEncoder
    ()
    // Creates an encoder
{
  m_pjerr = new jpeg_error_mgr;
  m_pcinfo = new jpeg_compress_struct;

  m_pcinfo->err = jpeg_std_error (m_pjerr);
  m_pjerr->error_exit = error_exit;  // Register custom error manager.

  jpeg_create_compress (m_pcinfo);

}


CJPEGEncoder::~CJPEGEncoder
    ()
{
  jpeg_destroy_compress (m_pcinfo);
  delete m_pjerr;
  delete m_pcinfo;
}

void CJPEGEncoder::DoEncode
    ( CBmp * pBmp,
      CDataSink * pDataSink
    )
{
  PLASSERT (pBmp->GetBitsPerPixel() == 32); // Only true-color supported for now.
  try
  {
  // todo: notification not yet implemented for encoders (3.6.99 MS)
  /*
    JMETHOD( void, notify, (j_common_ptr));
    notify = JNotification;
  */

    // Initialize custom data destination
	  jpeg_mem_dest(m_pcinfo, pDataSink->GetBufferPtr(), pDataSink->GetMaxDataSize(), pDataSink);

    // Set Header Fields
    m_pcinfo->image_width = pBmp->GetWidth();
    m_pcinfo->image_height = pBmp->GetHeight();

    m_pcinfo->input_components = 4;
    m_pcinfo->in_color_space = JCS_RGB;

    jpeg_set_defaults (m_pcinfo);

    // on good FPUs (e.g. Pentium) this is the fastest and "best" DCT method
    m_pcinfo->dct_method = JDCT_FLOAT;

    jpeg_start_compress (m_pcinfo,TRUE);

    encodeRGB (pBmp, pBmp->GetHeight());

    jpeg_finish_compress (m_pcinfo);
  }
  catch (CTextException)
  {
    jpeg_abort_compress(m_pcinfo);
    throw;
  }

}

void CJPEGEncoder::encodeRGB
    ( CBmp * pBmp,
      int iScanLines
    )
    // Assumes IJPEG decoder is already set up.
{
  BYTE * pDst;
  int CurLine = 0;
  JSAMPARRAY ppDst = &pDst;

  BYTE ** pLineArray = pBmp->GetLineArray();

  while (CurLine < iScanLines)
  {
    pDst = pLineArray[CurLine];
    jpeg_write_scanlines (m_pcinfo, ppDst, 1);
    CurLine++;
  }
}

/*
/--------------------------------------------------------------------
|
|      $Log: jpegenc.cpp,v $
|      Revision 1.3  2000/01/16 20:43:13  anonymous
|      Removed MFC dependencies
|
|      Revision 1.2  1999/12/08 15:39:45  Ulrich von Zadow
|      Unix compatibility changes
|
|      Revision 1.1  1999/10/19 21:28:05  Ulrich von Zadow
|      Added jpeg encoder
|
|
\--------------------------------------------------------------------
*/
