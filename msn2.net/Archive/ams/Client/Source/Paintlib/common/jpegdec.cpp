/*
/--------------------------------------------------------------------
|
|      $Id: jpegdec.cpp,v 1.9 2000/01/16 20:43:13 anonymous Exp $
|      JPEG Decoder Class
|
|      JPEG file decoder. Uses the independent JPEG group's library
|      to do the actual conversion.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#define JPEG_INTERNALS
#include "jpegdec.h"

#include "except.h"

extern "C"
{
#include "jmemsrc.h"
}


// Check to see if the pixel format defined in jmorecfg.h matches the
// one defined in bitmap.h.
// If the program reports an error here, you're probably using a wrong
// version of jmorecfg.h. Several files in libjpeg and libtiff need to
// replaced for the libraries to be used with paintlib. See the
// documentation for details.
#if (RGBA_RED != RGB_RED) || (RGBA_GREEN != RGB_GREEN)   \
    || (RGBA_BLUE != RGB_BLUE) || (RGB_PIXELSIZE != PIXEL_SIZE)
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

  CPicDecoder::raiseError (ERR_FORMAT_NOT_SUPPORTED, sz);
}

/////////////////////////////////////////////////////////////////////
// Class functions

CJPEGDecoder::CJPEGDecoder
    ()
    // Creates a decoder
{
  cinfo.err = jpeg_std_error (&jerr);
  jerr.error_exit = error_exit;  // Register custom error manager.

  jpeg_create_decompress (&cinfo);

  m_bFast = TRUE;
}


CJPEGDecoder::~CJPEGDecoder
    ()
{
  jpeg_destroy_decompress (&cinfo);
}

void CJPEGDecoder::SetFast
    ( BOOL bFast
    )
    // TRUE (the default) selects fast but sloppy decoding.
{
  m_bFast = bFast;
}

bool CJPEGDecoder::GetDimensions(char* file, DWORD *width, DWORD *height)
{
	//open file
	FILE * pFile = fopen (file, "rb");
	if (!pFile)
		return false;

	//read header
	try
	{
		jpeg_stdio_src(&cinfo, pFile);
		jpeg_read_header(&cinfo, TRUE);
		*width = (DWORD)cinfo.image_width;
		*height = (DWORD)cinfo.image_height;
	}
	catch (...)
	{
		jpeg_abort_decompress(&cinfo);
		fclose (pFile);
		return false;
	}
	jpeg_abort_decompress(&cinfo);

	//success
	fclose (pFile);
	return true;
}

void CJPEGDecoder::DoDecode
    ( CBmp * pBmp,
      RGBAPIXEL ** ppPal,
      int * pDestBPP,
      CDataSource * pDataSrc
    )
{
  try
  {
	// Jo Hagelberg 15.4.99: added progress notification callback
    JMETHOD( void, notify, (j_common_ptr));
    notify = JNotification;

    // Initialize custom data source.
    // Jo Hagelberg 15.4.99: added pDataSrc and notify
    jpeg_mem_src (&cinfo, pDataSrc->ReadEverything(),
                  pDataSrc->GetFileSize(), (void*)pDataSrc, notify);

    jpeg_read_header (&cinfo, TRUE);

    if (m_bFast)
    {
      cinfo.do_fancy_upsampling = FALSE;
    }

    // Choose floating point DCT method.
    cinfo.dct_method = JDCT_FLOAT;

    int w = cinfo.image_width;
    int h = cinfo.image_height;

    jpeg_start_decompress (&cinfo);

    if (cinfo.out_color_space == JCS_GRAYSCALE)
      decodeGray (pBmp, w, h, pDestBPP);
    else
      decodeRGB (pBmp, w, h, pDestBPP);
    jpeg_finish_decompress (&cinfo);
  }
  catch (CTextException)
  {
    jpeg_abort_decompress(&cinfo);
    throw;
  }


}

void CJPEGDecoder::decodeRGB
    ( CBmp * pBmp,
      int w,
      int h,
      int * pDestBPP
    )
    // Assumes IJPEG decoder is already set up.
{
  BYTE * pDst;
  int CurLine = 0;
  JSAMPARRAY ppDst = &pDst;

  CalcDestBPP (32, pDestBPP);
  pBmp->Create (w, h, 32, FALSE);

  BYTE ** pLineArray = pBmp->GetLineArray();

  while (CurLine < h)
  {
    pDst = pLineArray[CurLine];
    jpeg_read_scanlines (&cinfo, ppDst, 1);
    CurLine++;
  }
}

void CJPEGDecoder::decodeGray
    ( CBmp * pBmp,
      int w,
      int h,
      int * pDestBPP
    )
    // Assumes IJPEG decoder is already set up.
{
  BYTE * pDst;
  int CurLine = 0;
  BYTE * pBuf = new BYTE [w];
  JSAMPARRAY ppBuf = &pBuf;

  CalcDestBPP (8, pDestBPP);
  pBmp->Create (w, h, *pDestBPP, FALSE);

  BYTE ** pLineArray = pBmp->GetLineArray();

  while (CurLine < h)
  {
    if (*pDestBPP == 32)
    {
      jpeg_read_scanlines (&cinfo, ppBuf, 1);

      pDst = pLineArray[CurLine];

      for (int i=0; i<w; i++)
      {
        pDst[i*4] = pBuf[i];
        pDst[i*4+1] = pBuf[i];
        pDst[i*4+2] = pBuf[i];
        pDst[i*4+3] = 0xFF;
      }
    }
    else
    {
      ppBuf = &pDst;
      *ppBuf = pLineArray[CurLine];
      jpeg_read_scanlines (&cinfo, ppBuf, 1);

    }
    CurLine++;
  }
  delete pBuf;
}


/*
 * Jo Hagelberg 15.4.99
 * progress notification callback
 * since this is a static function we need pDataSrc from cinfo->client_data
 * progress is calculated (0...1) and PaintLib's Notification called
 */

void CJPEGDecoder::JNotification (j_common_ptr cinfo)
{
  double       part;
  CDataSource *pDataSrc;

  /* calculated according to jpeg lib manual
   * note: this may not be precice when using buffered image mode
   * todo: think hard of alternatives 4 this case ... :-)
   */
  part = ( (double)cinfo->progress->completed_passes +
 	         ((double)cinfo->progress->pass_counter/cinfo->progress->pass_limit) ) /
	       (double)cinfo->progress->total_passes;

  // call Notification in CDataSource
  pDataSrc = (CDataSource*) cinfo->client_data;
  if (pDataSrc)
  {
    pDataSrc->OProgressNotification( part);
  }

}

/*
/--------------------------------------------------------------------
|
|      $Log: jpegdec.cpp,v $
|      Revision 1.9  2000/01/16 20:43:13  anonymous
|      Removed MFC dependencies
|
|      Revision 1.8  1999/12/14 12:30:13  Ulrich von Zadow
|      Corrected copy constructor and assignment operator.
|
|      Revision 1.7  1999/12/08 16:31:40  Ulrich von Zadow
|      Unix compatibility
|
|      Revision 1.6  1999/12/08 15:39:45  Ulrich von Zadow
|      Unix compatibility changes
|
|      Revision 1.5  1999/10/19 21:25:16  Ulrich von Zadow
|      no message
|
|      Revision 1.4  1999/10/03 18:50:51  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
\--------------------------------------------------------------------
*/
