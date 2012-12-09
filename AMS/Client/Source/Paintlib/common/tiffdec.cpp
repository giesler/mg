/*
/--------------------------------------------------------------------
|
|      $Id: tiffdec.cpp,v 1.8 2000/03/16 13:56:37 Ulrich von Zadow Exp $
|      TIFF Decoder Class
|
|      TIFF file decoder. Uses LIBTIFF to do the actual conversion.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "tiffdec.h"
#include "except.h"

extern "C"
{
#include "tif_msrc.h"
}


char CTIFFDecoder::m_szLastErr[256];

/////////////////////////////////////////////////////////////////////
//  helper class to setup libtiff callbacks

class _tiff_helper
{
public:
  _tiff_helper()
  {
    // Set up function pointers to error handling routines.
    TIFFSetErrorHandler (CTIFFDecoder::Win32ErrorHandler);
    TIFFSetWarningHandler (CTIFFDecoder::Win32WarningHandler);
  }

  // We could restore the internal (default) libtiff handlers,
  // but that is not generally useful
  ~_tiff_helper()
{}

};

static _tiff_helper _the_tiff_helper_singleton;


/////////////////////////////////////////////////////////////////////
// Class functions

CTIFFDecoder::CTIFFDecoder ()
// Creates a decoder
{}



CTIFFDecoder::~CTIFFDecoder ()
{}

void CTIFFDecoder::DoTiffDecode (CBmp * pBmp, RGBAPIXEL ** ppPal, int * pDestBPP,
                                 CDataSource * pDataSrc, TIFF* tif)
{
  uint16  BitsPerSample;
  uint16  SamplePerPixel;

  // get tagged image parameters
  TIFFGetFieldDefaulted(tif, TIFFTAG_BITSPERSAMPLE, &BitsPerSample);
  TIFFGetFieldDefaulted(tif, TIFFTAG_SAMPLESPERPIXEL, &SamplePerPixel);

  // For the time being, paintlib bitmaps only actually support 8
  // or 32bpp; so the following mapping should cover all cases ...
  if (SamplePerPixel == 1 && BitsPerSample <= 8
      && *pDestBPP < 32 && ! TIFFIsTiled(tif))
    doLoColor(tif, pBmp, pDestBPP, ppPal);
  else
    // complicated decoding; use higher-level API
    // it will promote all images to 32bpp, though
    doHiColor(tif, pBmp, pDestBPP);
}

void CTIFFDecoder::DoDecode (CBmp * pBmp, RGBAPIXEL ** ppPal, int * pDestBPP,
                             CDataSource * pDataSrc)
{
  TIFF* tif = TIFFOpenMem (pDataSrc->ReadEverything(),
                           pDataSrc->GetFileSize(),
                           NULL);
  if (!tif)
    raiseError (ERR_WRONG_SIGNATURE, m_szLastErr);
  // else
  DoTiffDecode( pBmp, ppPal, pDestBPP, pDataSrc, tif );

  TIFFClose(tif);
  /*
    --- Older decoder using the TIFFRGBAImage functions

    int ok;
    TIFFRGBAImage img;
    char emsg[1024];
    BYTE * pBits;
    ULONG x, y;

    ok = TIFFRGBAImageBegin(&img, tif, 0, emsg);

    if (ok == 0)
    {
      TIFFClose (tif);
      raiseError (ERR_WRONG_SIGNATURE, m_szLastErr);
    }

    try
    {
      pBmp->Create (img.width, img.height, 32, (img.alpha != 0));
      pBits = new BYTE [img.width*img.height*4];
      if (pBits == NULL)
        raiseError (ERR_NO_MEMORY, "Out of memory allocating TIFF buffer.");
    }
    catch (CTextException)
    {
      TIFFClose (tif);
      throw;
    }

    ok = TIFFRGBAImageGet(&img, (uint32 *) pBits, img.width, img.height);
    if (!ok)
    {
      TIFFRGBAImageEnd(&img);
      TIFFClose(tif);
      raiseError (ERR_WRONG_SIGNATURE, m_szLastErr);
    }

    BYTE ** pLineArray = pBmp->GetLineArray();

    // Correct the byte ordering
    for (y=0; y<img.height; y++)
    {
      BYTE * pSrc = pBits+(img.height-y-1)*img.width*4;
      RGBAPIXEL * pPixel = (RGBAPIXEL *)(pLineArray[y]);
      for  (x=0; x<img.width; x++)
      {
        SetRGBAPixel (pPixel, *pSrc, *(pSrc+1),
                      *(pSrc+2), *(pSrc+3));
        pPixel++;
        pSrc += 4;
      }
    }

    // Clean up.
    delete pBits;
    TIFFRGBAImageEnd(&img);
    TIFFClose(tif);
  */
}

void CTIFFDecoder::doHiColor (TIFF * tif, CBmp * pBmp, int * pDestBPP)
{
  int ok;
  ULONG x, y;

  TIFFRGBAImage img;
  char emsg[1024];
  BYTE * pBits;

  CalcDestBPP (32, pDestBPP); // non-palette formats handled here

  ok = TIFFRGBAImageBegin(&img, tif, 0, emsg);

  if (ok == 0)
  {
    TIFFClose (tif);
    raiseError (ERR_WRONG_SIGNATURE, m_szLastErr);
  }

  try
  {
    pBmp->Create (img.width, img.height, 32, (img.alpha != 0));
    pBits = new BYTE [img.width*img.height*4];
    if (pBits == NULL)
      raiseError (ERR_NO_MEMORY, "Out of memory allocating TIFF buffer.");
  }
  catch (CTextException)
  {
    TIFFClose (tif);
    throw;
  }

  ok = TIFFRGBAImageGet(&img, (uint32 *) pBits, img.width, img.height);
  if (!ok)
  {
    TIFFRGBAImageEnd(&img);
    TIFFClose(tif);
    raiseError (ERR_WRONG_SIGNATURE, m_szLastErr);
  }

  BYTE ** pLineArray = pBmp->GetLineArray();

  // Correct the byte ordering
  for (y=0; y<img.height; y++)
  {
    BYTE * pSrc = pBits+(img.height-y-1)*img.width*4;
    RGBAPIXEL * pPixel = (RGBAPIXEL *)(pLineArray[y]);
    for  (x=0; x<img.width; x++)
    {
      SetRGBAPixel (pPixel, *pSrc, *(pSrc+1),
                    *(pSrc+2), *(pSrc+3));
      pPixel++;
      pSrc += 4;
    }
  }

  // Clean up.
  delete pBits;
  TIFFRGBAImageEnd(&img);
}

/*
 * TIFF decoding for 1, 4 and 8 bit(s) per pixel
 * bdelmee; 10/98
 */

/* check if color map holds old-style 8-bit values */
static int checkcmap(int n, uint16* r, uint16* g, uint16* b)
{
  while (n-- > 0)
    if (*r++ >= 256 || *g++ >= 256 || *b++ >= 256)
      return (16);

  return (8);
}

#define CVT(x)      (((x) * 255L) / ((1L<<16)-1))

void CTIFFDecoder::doLoColor (TIFF * tif, CBmp * pBmp, int * pDestBPP,
                              RGBAPIXEL ** ppPal)
{
  uint32  imageLength;
  uint32  imageWidth;
  uint16  BitsPerSample;
  uint16  SamplePerPixel;
  int32  LineSize;
  int16  PhotometricInterpretation;
  uint32  row;
  BYTE  *pBits;

  TIFFGetFieldDefaulted(tif, TIFFTAG_IMAGEWIDTH, &imageWidth);
  TIFFGetFieldDefaulted(tif, TIFFTAG_IMAGELENGTH, &imageLength);
  TIFFGetFieldDefaulted(tif, TIFFTAG_BITSPERSAMPLE, &BitsPerSample);
  TIFFGetFieldDefaulted(tif, TIFFTAG_SAMPLESPERPIXEL, &SamplePerPixel);
  TIFFGetFieldDefaulted(tif, TIFFTAG_PHOTOMETRIC, &PhotometricInterpretation);

  LineSize = TIFFScanlineSize(tif); //Number of bytes in one line

  int nBPP = SamplePerPixel == 1 && BitsPerSample == 1 ? 1 : 8;
  if (*pDestBPP == 0)
    *pDestBPP = nBPP;

  try
  {
    pBmp->Create (imageWidth, imageLength, nBPP, 0);
    *ppPal = new RGBAPIXEL[1 << BitsPerSample];
    // memset( pPal, 0, 256 * sizeof(RGBAPIXEL));
    if (*ppPal == NULL)
      raiseError (ERR_NO_MEMORY, "Out of memory allocating TIFF palette.");
    pBits = new BYTE [LineSize];
    if (pBits == NULL)
      raiseError (ERR_NO_MEMORY, "Out of memory allocating TIFF buffer.");
  }
  catch (CTextException)
  {
    TIFFClose (tif);
    throw;
  }


  // phase one: build color map

  if /* monochrome (=bitonal) or grayscale */
  (PhotometricInterpretation == PHOTOMETRIC_MINISWHITE ||
      PhotometricInterpretation == PHOTOMETRIC_MINISBLACK)
  {
    int numColors = 1 << BitsPerSample;
    BYTE step = 255 / (numColors-1);
    BYTE *pb = (BYTE *) (*ppPal);
    int offset = sizeof(RGBAPIXEL);
    if (PhotometricInterpretation == PHOTOMETRIC_MINISWHITE)
    {
      pb += (numColors-1) * sizeof(RGBAPIXEL);
      offset = -offset;
    }
    // warning: the following ignores possible halftone hints
    for (int i = 0; i < numColors; ++i, pb += offset)
    {
      pb[RGBA_RED] = pb[RGBA_GREEN] = pb[RGBA_BLUE] = i * step;
      pb[RGBA_ALPHA] = 255;
    }

  }
  //PhotometricInterpretation = 2 image is RGB
  //PhotometricInterpretation = 3 image has a color palette
  else if (PhotometricInterpretation == PHOTOMETRIC_PALETTE)
  {
    uint16* red;
    uint16* green;
    uint16* blue;
    int16 i, Palette16Bits;

    // we get pointers to libtiff-owned colormaps
    i = TIFFGetField(tif, TIFFTAG_COLORMAP, &red, &green, &blue);

    //Is the palette 16 or 8 bits ?
    Palette16Bits = checkcmap(1<<BitsPerSample, red, green, blue) == 16;

    //load the palette in the DIB
    for (i = 0; i < 1<<BitsPerSample; ++i)
    {
      BYTE *pb = (BYTE *) ((*ppPal)+i);
      pb[RGBA_RED  ] = (BYTE) (Palette16Bits ? CVT(  red[i]) :   red[i]);
      pb[RGBA_GREEN] = (BYTE) (Palette16Bits ? CVT(green[i]) : green[i]);
      pb[RGBA_BLUE ] = (BYTE) (Palette16Bits ? CVT( blue[i]) :  blue[i]);
      pb[RGBA_ALPHA] = 255;
    }
  }
  else
    Trace( 2, "unexpected PhotometricInterpretation in CTIFFDecoder::DoLoColor()" );

  // phase two: read image itself

  //generally, TIFF images are ordered from upper-left to bottom-right
  // we implicitly assume PLANARCONFIG_CONTIG
  BYTE **pLineArray = pBmp->GetLineArray();

  if (BitsPerSample > 8)
    Trace( 2, "unexpected bit-depth in CTIFFDecoder::DoLoColor()" );
  else for ( row = 0; row < imageLength; ++row )
    {
      uint16 x;
      int status = TIFFReadScanline( tif, pBits, row, 0 );
      if (status == -1 && row < imageLength / 3)
      {
        delete pBits;
        // we should maybe free the BMP memory as well...
        raiseError (ERR_INTERNAL, m_szLastErr);
      }
      /*
      if (BitsPerSample == 1)  // go ahead, waste space ;-)
        for (x=0; x < imageWidth; ++x)
          pLineArray[row][x] = pBits[x / 8] & (128 >> (x & 7)) ? 1 : 0;
      else */
      if (BitsPerSample == 4)
      {
        for (x=0; x < imageWidth / 2; ++x)
        {
          pLineArray[row][2*x  ] = (pBits[x] & 0xf0) >> 4;
          pLineArray[row][2*x+1] = (pBits[x] & 0x0f);
        }
        // odd number of pixels
        if (imageWidth & 1)
          pLineArray[row][imageWidth-1] = (pBits[x] & 0xf0) >> 4;
      }
      else //if (BitsPerSample == 8 || BitsPerSample == 1)
        memcpy( pLineArray[row], pBits, LineSize );
    }

  // propagate colormap
  pBmp->SetPalette( *ppPal );
  // clean up
  delete pBits;
}

/////////////////////////////////////////////////////////////////////
// Static functions used as Callbacks from the TIFF library

void CTIFFDecoder::Win32ErrorHandler (const char* module, const char* fmt, va_list ap)
{
  int k = vsprintf(m_szLastErr, fmt, ap);
  if (k >= 0) strcat( m_szLastErr + k, "\n" );

  Trace (0, m_szLastErr);
  //raiseError (ERR_INTERNAL, m_szLastErr);

  return;
}

void CTIFFDecoder::Win32WarningHandler (const char* module, const char* fmt, va_list ap)
{
  char szTemp[256];
  char szMessage[256];

  int k = vsprintf(szMessage, fmt, ap);
  if (k >= 0) strcat( szMessage + k, "\n" );

  if (module != NULL)
    sprintf (szTemp, "Warning in LIBTIFF(%s): %s\n", module, szMessage);
  else
    sprintf (szTemp, "Warning in LIBTIFF: %s\n", szMessage);

  Trace (2, szTemp);

  return;
}
/*
/--------------------------------------------------------------------
|
|      $Log: tiffdec.cpp,v $
|      Revision 1.8  2000/03/16 13:56:37  Ulrich von Zadow
|      Added pgm decoder by Jose Miguel Buenaposada Biencinto
|
|      Revision 1.7  2000/01/16 20:43:14  anonymous
|      Removed MFC dependencies
|
|      Revision 1.6  2000/01/10 23:53:00  Ulrich von Zadow
|      Changed formatting & removed tabs.
|
|      Revision 1.5  2000/01/09 22:24:10  Ulrich von Zadow
|      Corrected tiff callback bug.
|
|      Revision 1.4  1999/10/03 18:50:52  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
\--------------------------------------------------------------------
*/
