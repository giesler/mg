/*
/--------------------------------------------------------------------
|
|      $Id: tiffenc.cpp,v 1.10 2000/01/16 20:43:15 anonymous Exp $
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
#include "bitmap.h"
#include "except.h"
extern "C"
{
#include "tiffio.h"   // for the tags definitions
#include "tif_msrc.h"
}


/////////////////////////////////////////////////////////////////////
// Class functions

// Creates an encoder
CTIFFEncoder::CTIFFEncoder()
{}



CTIFFEncoder::~CTIFFEncoder()
{}



void CTIFFEncoder::DoEncode (CBmp * pBmp, CDataSink* pDataSnk)
{
  TIFF* tif = TIFFOpenMem (pDataSnk->m_pStartData,
                           pDataSnk->m_nMaxFileSize,
                           &(pDataSnk->m_nCurPos));
  PLASSERT( tif );
  /*
  if (!tif)
  raiseError (ERR_WRONG_SIGNATURE, CTIFFDecoder::m_szLastErr);
  */

  // initialize TIFF "directory"
  SetBaseTags( tif, pBmp );

  DoTiffEncode( pBmp, tif );

  TIFFClose( tif );
}


void CTIFFEncoder::DoTiffEncode (CBmp* pBmp, TIFF* tif)
{
  // we only support monochrome images in this initial version
  // ASSERT( pBmp->GetBitsPerPixel() == 1);
  // raiseError();

  int k;
  uint32 l, c, image_length, image_width;
  // iterate over data
  BYTE **pla = pBmp->GetLineArray();
  PLASSERT( pla );

  image_length = (uint32) pBmp->GetHeight();
  image_width  = (uint32) pBmp->GetWidth();
  switch (pBmp->GetBitsPerPixel())
  {
    case 8:
      {
        // first, save the colormap
        uint16 red[256];
        uint16 green[256];
        uint16 blue[256];

        BYTE* pPal = (BYTE*) pBmp->GetPalette();
        PLASSERT( pPal );
        for (int i = 0; i < pBmp->GetNumColors(); i++, pPal += sizeof(RGBAPIXEL))
        {
          red[i]   = pPal[RGBA_RED]   << 8;
          green[i] = pPal[RGBA_GREEN] << 8;
          blue[i]  = pPal[RGBA_BLUE]  << 8;
        }
        SetField( tif, TIFFTAG_COLORMAP, red, green, blue );
      }
      // fall-through

    case 1:  // TODO: a bit of error checking
      for (l = 0; l < image_length; l++)
        k = TIFFWriteScanline( tif, pla[l], l, 0 );
      break;

    case 32:
      {
        // TODO: check whether (r,g,b) components come in the correct order here...
        BYTE* pBuf = new BYTE[3*image_width];
        for (l = 0; l < image_length; l++)
        {
          for (c = 0; c < image_width; c++)
          {
            pBuf[c*3 + 0] = pla[l][c*sizeof(RGBAPIXEL) + RGBA_RED];
            pBuf[c*3 + 1] = pla[l][c*sizeof(RGBAPIXEL) + RGBA_GREEN];
            pBuf[c*3 + 2] = pla[l][c*sizeof(RGBAPIXEL) + RGBA_BLUE];
          }
          k = TIFFWriteScanline( tif, pBuf, l, 0 );
        }
        delete [] pBuf;
      }
      break;

    default:
      PLASSERT(FALSE);
  }
  // we could flush at this point, but TIFFClose will do it anyway
}


// According to the characteristics of the given bitmap,
// set the baseline tags

int CTIFFEncoder::SetBaseTags (TIFF* tif, CBmp* pBmp)
{
  PLASSERT( tif && pBmp );

  uint16 ui16 = 0;
  uint32 ui32 = 0;

  ui32 = pBmp->GetWidth();
  SetField( tif, TIFFTAG_IMAGEWIDTH,      ui32 );
  ui32 = pBmp->GetHeight();
  SetField( tif, TIFFTAG_IMAGELENGTH,     ui32 );
  // one strip = the whole image
  // SetField( tif, TIFFTAG_ROWSPERSTRIP,    ui32 );
  ui16 = pBmp->GetBitsPerPixel();
  if (ui16 > 8) ui16 = 8;
  SetField( tif, TIFFTAG_BITSPERSAMPLE,   ui16 );
  ui16 = pBmp->GetBitsPerPixel();
  ui16 = ui16 <= 8 ? 1 : 3;
  SetField( tif, TIFFTAG_SAMPLESPERPIXEL, ui16 );
  /*
  ui16 = COMPRESSION_NONE;
  SetField( tif, TIFFTAG_COMPRESSION,     ui16 );
  */
  ui16 = PLANARCONFIG_CONTIG;
  SetField( tif, TIFFTAG_PLANARCONFIG,    ui16 );

  /*
   * The following tags are supposedly mandatory,
   * but libtiff seems to have sensible defaults for us
   *

  ui32 = 0;
  SetField( TIFFTAG_SUBFILETYPE,     ui32 );
  ?!?
  SetField( TIFFTAG_STRIPOFFSETS,    ui32 );
  ?!?
  SetField( TIFFTAG_STRIPBYTECOUNT,  ui32 );
  float r = 0.0;
  r = 300.0;
  SetField( TIFFTAG_XRESOLUTION,     r    );
  SetField( TIFFTAG_YRESOLUTION,     r    );
  ui16 = RESUNIT_INCH;
  SetField( TIFFTAG_RESOLUTIONUNIT,  ui16 );

   *
   *
   */

  switch (pBmp->GetBitsPerPixel())
  {
    case 1:
      {
        // look at bi-level palette...
        BYTE* p = (BYTE*) pBmp->GetPalette();
        ui16 = p[RGBA_RED] < p[RGBA_RED + sizeof(RGBAPIXEL)] &&
               p[RGBA_GREEN] < p[RGBA_GREEN + sizeof(RGBAPIXEL)] &&
               p[RGBA_BLUE] < p[RGBA_BLUE + sizeof(RGBAPIXEL)] ?
               PHOTOMETRIC_MINISBLACK : PHOTOMETRIC_MINISWHITE;
        SetField( tif, TIFFTAG_PHOTOMETRIC,   ui16 );
      }
      break;

    case 8:
      ui16 = PHOTOMETRIC_PALETTE;
      SetField( tif, TIFFTAG_PHOTOMETRIC,    ui16 );
      break;

    case 32:
      ui16 = PHOTOMETRIC_RGB;
      SetField( tif, TIFFTAG_PHOTOMETRIC,    ui16 );
      break;

    default:
      PLASSERT(FALSE);
  }

  return 1; // should reflect the successful directory initialisation
}


// Set field in directory.

int CTIFFEncoder::SetField( TIFF* tif, int tag_id, ... )
{
  int retv;
  va_list marker;

  va_start( marker, tag_id );     /* Initialize variable arguments. */
  retv = TIFFVSetField( tif, tag_id, marker );
  va_end( marker );               /* Reset variable arguments.      */

  return retv;
}


/*
/--------------------------------------------------------------------
|
|      $Log: tiffenc.cpp,v $
|      Revision 1.10  2000/01/16 20:43:15  anonymous
|      Removed MFC dependencies
|
|      Revision 1.9  2000/01/10 23:53:00  Ulrich von Zadow
|      Changed formatting & removed tabs.
|
|      Revision 1.8  1999/12/10 01:27:26  Ulrich von Zadow
|      Added assignment operator and copy constructor to
|      bitmap classes.
|
|      Revision 1.7  1999/12/08 15:39:45  Ulrich von Zadow
|      Unix compatibility changes
|
|      Revision 1.6  1999/12/02 17:07:34  Ulrich von Zadow
|      Changes by bdelmee.
|
|      Revision 1.5  1999/10/03 18:50:51  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
\--------------------------------------------------------------------
*/
