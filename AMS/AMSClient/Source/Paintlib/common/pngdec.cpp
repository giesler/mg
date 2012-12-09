/*
/--------------------------------------------------------------------
|
|      $Id: pngdec.cpp,v 1.7 2000/01/16 20:43:14 anonymous Exp $
|      PNG Decoder Class
|
|      PNG file decoder. Uses LibPng to do the actual decoding.
|      PNG supports many pixel formats not supported by paintlib.
|      These pixel formats are converted to the nearest paintlib
|      equivalent. Images with less or more than 8 bits per channel
|      are converted to 8 bits per channel. Images with 16-bit
|      palettes or grayscale images with an alpha channel are
|      returned as full 32-bit RGBA bitmaps.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "pngdec.h"
#include "except.h"
#include "png.h"


CPNGDecoder::CPNGDecoder
    ()
    // Creates a decoder
{
}


CPNGDecoder::~CPNGDecoder
    ()
{
}



void my_read_data(png_structp png_ptr, png_bytep data, png_size_t length)
{
  // todo : check data erasing
  BYTE *ptr;
  CDataSource* pSourceInfo=(CDataSource*)png_get_io_ptr(png_ptr);

  ptr = pSourceInfo->ReadNBytes(length);
  memcpy(data,ptr,length);
}


void CPNGDecoder::DoDecode
    ( CBmp * pBmp,
      RGBAPIXEL ** ppPal,
      int * pDestBPP,
      CDataSource * pDataSrc
    )
{
  png_uint_32 width, height;
  int bit_depth, color_type, interlace_type;
  BOOL bHasAlpha;
  png_structp png_ptr = NULL;
  png_infop info_ptr;

  try
  {
    png_ptr = png_create_read_struct
                (PNG_LIBPNG_VER_STRING,
                 (void *)NULL,
                 user_error_fn,
                 user_warning_fn);
    PLASSERT (png_ptr);

    info_ptr = png_create_info_struct(png_ptr);
    PLASSERT (info_ptr);

    png_set_read_fn(png_ptr, (void*)pDataSrc, my_read_data);

    /* The call to png_read_info() gives us all of the information from the
     * PNG file before the first IDAT (image data chunk).  REQUIRED
     */
    png_read_info(png_ptr, info_ptr);

    png_get_IHDR(png_ptr, info_ptr, &width, &height, &bit_depth, &color_type,
                 &interlace_type, NULL, NULL);

    switch (color_type)
    {
      case PNG_COLOR_TYPE_RGB:
        CalcDestBPP (32, pDestBPP);
        bHasAlpha = FALSE;
        break;
      case PNG_COLOR_TYPE_RGB_ALPHA:
        CalcDestBPP (32, pDestBPP);
        bHasAlpha = TRUE;
        break;
      case PNG_COLOR_TYPE_GRAY:
        if (*pDestBPP != 32)
        {
          CalcDestBPP (8, pDestBPP);
          bHasAlpha = FALSE;
        }
        else
        {
          CalcDestBPP (32, pDestBPP);
          png_set_gray_to_rgb (png_ptr);
          png_set_expand(png_ptr);
          bHasAlpha = FALSE;
        }
        break;
      case PNG_COLOR_TYPE_GRAY_ALPHA:
        CalcDestBPP (32, pDestBPP);
        png_set_gray_to_rgb(png_ptr);
        png_set_expand(png_ptr);
        bHasAlpha = TRUE;
        break;
      case PNG_COLOR_TYPE_PALETTE:
        if (bit_depth != 16 && *pDestBPP != 32)
          CalcDestBPP (8, pDestBPP);
        else
        { // 16-bit palette image
          png_set_expand(png_ptr);
          CalcDestBPP (32, pDestBPP);
        }
        bHasAlpha = FALSE;
        break;
    }

    if (*pDestBPP == 32)
    { // Make sure we use the correct byte order
#if (RGBA_BLUE + 2 == RGBA_RED)
      png_set_bgr (png_ptr);
#else
  #if (RGBA_BLUE == RGBA_RED + 2)
  #else
  #error Unsupported byte ordering!
  #endif
#endif
      if (!bHasAlpha)
#if (RGBA_ALPHA == 3)
        png_set_filler(png_ptr, 0xff, PNG_FILLER_AFTER);
#else
        png_set_filler(png_ptr, 0xff, PNG_FILLER_BEFORE);
#endif
    }

    if (bHasAlpha)
      // Image has alpha channel
      pBmp->Create (width, height,
                    *pDestBPP, TRUE);
    else
      // Image doesn't have alpha channel
      pBmp->Create (width, height,
                    *pDestBPP, FALSE);

    if (color_type == PNG_COLOR_TYPE_GRAY  && *pDestBPP != 32)
    {
      int i;
      int NumColors = 1<<(bit_depth);
      for (i=0; i<NumColors; i++)
      {
        int CurColor = (i*255)/(NumColors-1);
        pBmp->SetPaletteEntry(i, CurColor, CurColor, CurColor, 0xFF);
      }
    }

  if (color_type == PNG_COLOR_TYPE_PALETTE && *pDestBPP != 32)
    {
      png_color* ppng_color_tab=NULL;

      int   i;
      int   nbColor=0;

      png_get_PLTE(png_ptr,info_ptr,&ppng_color_tab,&nbColor);

      for (i=0; i<nbColor; i++)
      {
        pBmp->SetPaletteEntry(i,
                              (*(ppng_color_tab+i)).red,
                              (*(ppng_color_tab+i)).green,
                              (*(ppng_color_tab+i)).blue,
                              0xFF);
      }
    }

    if (bit_depth == 16)
      png_set_strip_16(png_ptr);
    if (bit_depth < 8)
      png_set_packing(png_ptr);

    BYTE ** pLineArray = pBmp->GetLineArray();
    png_read_image(png_ptr, pLineArray);

    /* read rest of file, and get additional chunks in info_ptr - REQUIRED */
    png_read_end(png_ptr, info_ptr);

    /* clean up after the read, and free any memory allocated - REQUIRED */
    png_destroy_read_struct(&png_ptr, &info_ptr, (png_infopp)NULL);
  }
  catch (CTextException)
  {
    // Cleaning up in this catch seems to break Visual C++ memory management
    // completely in some cases, so we don't do it.
    // The result is a small memory leak whenever a png file can't be decoded.
    // if (png_ptr)
    //   png_destroy_read_struct(&png_ptr, &info_ptr, (png_infopp)NULL);
    throw;
  }
  catch(...)
  {
	  PLASSERT(FALSE);
    // Cleaning up in this catch seems to break Visual C++ memory management
    // completely in some cases, so we don't do it.
	  // png_destroy_read_struct(&png_ptr, &info_ptr, (png_infopp)NULL);
	  throw;
  }
}

void CPNGDecoder::user_error_fn
    ( png_structp png_ptr,
      png_const_charp error_msg
    )
{
  raiseError (ERR_FORMAT_UNKNOWN, (char *)error_msg);
}

void CPNGDecoder::user_warning_fn
    ( png_structp png_ptr,
      png_const_charp warning_msg
    )
{
  PLTRACE ((char *)warning_msg);
}

/*
/--------------------------------------------------------------------
|
|      $Log: pngdec.cpp,v $
|      Revision 1.7  2000/01/16 20:43:14  anonymous
|      Removed MFC dependencies
|
|      Revision 1.6  2000/01/04 18:35:23  Ulrich von Zadow
|      no message
|
|      Revision 1.5  1999/12/08 16:31:40  Ulrich von Zadow
|      Unix compatibility
|
|      Revision 1.4  1999/12/08 15:39:45  Ulrich von Zadow
|      Unix compatibility changes
|
|      Revision 1.3  1999/10/03 18:50:51  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
--------------------------------------------------------------------
*/
