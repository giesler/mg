/*
/--------------------------------------------------------------------
|
|      $Id: pngenc.cpp,v 1.5 2000/01/16 20:43:14 anonymous Exp $
|      PNG Encoder Class
|
|      PNG file encoder. Uses LIBPNG to do the actual conversion.
|      TODO: Error handling is still missing.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"

// Quick PNG encoder -- needs more work :)
//
#include <stdarg.h>
#include "PNGenc.h"
#include "png.h"
#include "bitmap.h"
#include "except.h"


/////////////////////////////////////////////////////////////////////
// Class functions

// Creates an encoder
CPNGEncoder::CPNGEncoder()
{}



CPNGEncoder::~CPNGEncoder()
{}


CDataSink * CPNGEncoder::GetDataSink()
{
  return m_pDataSnk;
}

void raiseError (png_structp png_ptr, png_const_charp message)
{
  char msg[256];  // TODO: check buffer size required for error string.
  // Note that raiseError has the same maximum!

  strcpy( msg, message );

  CPicEncoder::raiseError (ERR_FORMAT_NOT_SUPPORTED, msg);
}

void raiseWarning (png_structp png_ptr, png_const_charp message)
{
}


void EncodeExtraInfo (png_structp png_ptr)
{
  /* Optionally write comments into the image */
  /*
   text_ptr[0].key = "Title";
   text_ptr[0].text = "Mona Lisa";
   text_ptr[0].compression = PNG_TEXT_COMPRESSION_NONE;
   text_ptr[1].key = "Author";
   text_ptr[1].text = "Leonardo DaVinci";
   text_ptr[1].compression = PNG_TEXT_COMPRESSION_NONE;
   text_ptr[2].key = "Description";
   text_ptr[2].text = "<long text>";
   text_ptr[2].compression = PNG_TEXT_COMPRESSION_zTXt;
   png_set_text(png_ptr, info_ptr, text_ptr, 3);
  */

  /* other optional chunks like cHRM, bKGD, tRNS, tIME, oFFs, pHYs, */
  /* note that if sRGB is present the cHRM chunk must be ignored
   * on read and must be written in accordance with the sRGB profile */
}

void EncodeTransformation (png_structp png_ptr)
{
  /* Once we write out the header, the compression type on the text
   * chunks gets changed to PNG_TEXT_COMPRESSION_NONE_WR or
   * PNG_TEXT_COMPRESSION_zTXt_WR, so it doesn't get written out again
   * at the end.
   */

  /* set up the transformations you want.  Note that these are
   * all optional.  Only call them if you want them.
   */

  /* invert monocrome pixels */
  //png_set_invert_mono(png_ptr);

  /* Shift the pixels up to a legal bit depth and fill in
   * as appropriate to correctly scale the image.
   */
  //png_set_shift(png_ptr, &sig_bit);

  /* pack pixels into bytes */
  //png_set_packing(png_ptr);

  /* swap location of alpha bytes from ARGB to RGBA */
  //png_set_swap_alpha(png_ptr);

  /* Get rid of filler (OR ALPHA) bytes, pack XRGB/RGBX/ARGB/RGBA into
   * RGB (4 channels -> 3 channels). The second parameter is not used.
   */
  //png_set_filler(png_ptr, 0, PNG_FILLER_BEFORE);

  /* flip BGR pixels to RGB */
  png_set_bgr(png_ptr);

  /* swap bytes of 16-bit files to most significant byte first */
  //png_set_swap(png_ptr);

  /* swap bits of 1, 2, 4 bit packed pixel formats */
  //png_set_packswap(png_ptr);
}


png_color * createPNGPalette(CBmp * pBmp, png_structp png_ptr)
{
  int i;
  png_color * pPNGPal = (png_colorp)png_malloc(png_ptr, 256 * sizeof (png_color));
  RGBAPIXEL * pBmpPal = pBmp->GetPalette();

  for (i=0; i<256; i++)
  {
    BYTE * pPalEntry = (BYTE *)(pBmpPal+i);
    pPNGPal[i].red = pPalEntry[RGBA_RED];
    pPNGPal[i].green = pPalEntry[RGBA_GREEN];
    pPNGPal[i].blue = pPalEntry[RGBA_BLUE];
  }
  return pPNGPal;
}

void EncodeData (png_structp png_ptr, png_bytep data, png_size_t length)
{
  CPNGEncoder *pClass;

  pClass = (CPNGEncoder *)(png_ptr->io_ptr);

  PLASSERT (pClass);
  CDataSink * pSink = pClass->GetDataSink();
  PLASSERT (pSink);

  // Write out PNG data & raise an error if the write fails

  if (pSink->WriteNBytes( length, data ) != length)
  {
    png_error(png_ptr, "Error writting file");
  }
  return;
}

void FlushData (png_structp png_ptr)
{
  // Do nothing for now.
}


void CPNGEncoder::DoEncode (CBmp * pBmp, CDataSink* pDataSnk)
{
  png_structp png_ptr;
  png_infop info_ptr;
  png_color_8 sig_bit;

  /* Create and initialize the png_struct with the desired error handler
   * functions.  If you want to use the default stderr and longjump method,
   * you can supply NULL for the last three parameters.  We also check that
   * the library version is compatible with the one used at compile time,
   * in case we are using dynamically linked libraries.  REQUIRED.
   */
  png_ptr = png_create_write_struct(PNG_LIBPNG_VER_STRING,
                                    (png_voidp) pDataSnk->GetName(), ::raiseError, ::raiseWarning);

  if (png_ptr == NULL)
  {
    /* close the file */
    return;
  }

  /* Allocate/initialize the image information data.  REQUIRED */
  info_ptr = png_create_info_struct(png_ptr);
  if (info_ptr == NULL)
  {
    /* close the file */
    // TODO: We should raise an exception so the caller knows what's
    // going on.
    png_destroy_write_struct(&png_ptr,  (png_infopp)NULL);
    return;
  }

  /* Set error handling.  REQUIRED if you aren't supplying your own
   * error handling functions in the png_create_write_struct() call.
   */
  if (setjmp(png_ptr->jmpbuf))
  {
    /* If we get here, we had a problem reading the file */
    /* close the file */
    png_destroy_write_struct(&png_ptr,  (png_infopp)NULL);
    return;
  }

  m_pBmp = pBmp;
  m_pDataSnk = pDataSnk;

  /* If you are using replacement read functions, instead of calling
   * png_init_io() here you would call */
  png_set_write_fn(png_ptr, (void *)this, EncodeData, FlushData);
  /* where pWriteInfo is a structure you want available to the callbacks */


  /* Set the image information here.  Width and height are up to 2^31,
   * bit_depth is one of 1, 2, 4, 8, or 16, but valid values also depend on
   * the color_type selected. color_type is one of PNG_COLOR_TYPE_GRAY,
   * PNG_COLOR_TYPE_GRAY_ALPHA, PNG_COLOR_TYPE_PALETTE, PNG_COLOR_TYPE_RGB,
   * or PNG_COLOR_TYPE_RGB_ALPHA.  interlace is either PNG_INTERLACE_NONE or
   * PNG_INTERLACE_ADAM7, and the compression_type and filter_type MUST
   * currently be PNG_COMPRESSION_TYPE_BASE and PNG_FILTER_TYPE_BASE. REQUIRED
   */

  int bit_depth, color_type;

  switch(pBmp->GetBitsPerPixel())
  {
    case 1:
      bit_depth = 1;
      color_type = PNG_COLOR_TYPE_GRAY;
      break;
    case 8:
      bit_depth = 8;
      color_type = PNG_COLOR_TYPE_PALETTE;
      break;
    case 24:
      bit_depth = 8;
      color_type = PNG_COLOR_TYPE_RGB;
      break;
    case 32:
      bit_depth = 8;
      color_type = PNG_COLOR_TYPE_RGB_ALPHA;
      break;
    default:
      // Can't handle other bit depths.
      PLASSERT (FALSE);
  }

  png_set_IHDR (png_ptr,
                info_ptr,
                pBmp->GetWidth(),
                pBmp->GetHeight(),
                bit_depth,
                color_type,
                PNG_INTERLACE_NONE,
                PNG_COMPRESSION_TYPE_BASE,
                PNG_FILTER_TYPE_BASE);


  /* set the palette if there is one.  REQUIRED for indexed-color images */
  if (pBmp->GetPalette())
  {
    png_color * pPalette = createPNGPalette (pBmp, png_ptr);
    png_set_PLTE(png_ptr, info_ptr, pPalette, 256);
  }

  // TODO: Set grey or B&W information if available

  /* optional significant bit chunk */
  /* if we are dealing with a grayscale image then */
  //sig_bit.gray = true_bit_depth;

  /* otherwise, if we are dealing with a color image then */
  sig_bit.red   = bit_depth;
  sig_bit.green = bit_depth;
  sig_bit.blue  = bit_depth;

  if (pBmp->HasAlpha())
    sig_bit.alpha = 8;
  else
    sig_bit.alpha = 0;
  png_set_sBIT(png_ptr, info_ptr, &sig_bit);


  /* Optional gamma chunk is strongly suggested if you have any guess
   * as to the correct gamma of the image.
   */
  /*
  png_set_gAMA(png_ptr, info_ptr, gamma);
  */

  /* Write text image fields */
  EncodeExtraInfo( png_ptr );

  /* Write the file header information.  REQUIRED */
  png_write_info(png_ptr, info_ptr);

  /* Write image image transformation fields */
  EncodeTransformation( png_ptr );


  /* turn on interlace handling if you are not using png_write_image() */
  //if (interlacing)
  //   number_passes = png_set_interlace_handling(png_ptr);
  //else
  int number_passes = 1;

  // iterate over data and write it out
  BYTE **pla = pBmp->GetLineArray();
  PLASSERT( pla );

  png_write_image(png_ptr, pla);


  /* You can write optional chunks like tEXt, zTXt, and tIME at the end
   * as well.
   */

  /* It is REQUIRED to call this to finish writing the rest of the file */
  png_write_end(png_ptr, info_ptr);

  /* if you malloced the palette, free it here */
  if (bit_depth == 8)
    free(info_ptr->palette);

  /* if you allocated any text comments, free them here */

  /* clean up after the write, and free any memory allocated */
  png_destroy_write_struct(&png_ptr, &info_ptr);
}

/*
/--------------------------------------------------------------------
|
|      $Log: pngenc.cpp,v $
|      Revision 1.5  2000/01/16 20:43:14  anonymous
|      Removed MFC dependencies
|
|      Revision 1.4  2000/01/10 23:52:59  Ulrich von Zadow
|      Changed formatting & removed tabs.
|
|      Revision 1.3  2000/01/08 23:24:21  Ulrich von Zadow
|      Added encoder for palette files.
|
|      Revision 1.2  2000/01/08 15:53:12  Ulrich von Zadow
|      Moved several functions to the cpp file so applications don't
|      need the png directory in the include path.
|
|      Revision 1.1  2000/01/04 22:06:16  Ulrich von Zadow
|      Initial version by Neville Richards.
|
|
\--------------------------------------------------------------------
*/
