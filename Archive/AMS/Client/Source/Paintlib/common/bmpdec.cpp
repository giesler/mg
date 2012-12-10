/*
/--------------------------------------------------------------------
|
|      $Id: bmpdec.cpp,v 1.9 2000/01/16 20:43:12 anonymous Exp $
|      Windows Bitmap Decoder Class
|
|      Windows bitmap file decoder. Decodes 1, 4, 8, 24 and 32 bpp
|      bitmap files (compressed and uncompressed) and returns a 32
|      bpp DIB.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "bmpdec.h"
#include "except.h"


CBmpDecoder::CBmpDecoder
    ()
    // Creates a decoder
{
}


CBmpDecoder::~CBmpDecoder
    ()
{
}


void CBmpDecoder::DoDecode
    ( CBmp * pBmp,
      RGBAPIXEL ** ppPal,
      int * pDestBPP,
      CDataSource * pDataSrc
    )
{
  WINBITMAPINFOHEADER * pBMI = NULL;  // Pointer to bitmapinfoheader of the file.

  try
  {
    pBMI = getInfoHeader (pDataSrc, ppPal);

    CalcDestBPP (pBMI->biBitCount, pDestBPP);
    pBmp->Create (pBMI->biWidth, pBMI->biHeight, *pDestBPP, FALSE);
    if (*pDestBPP == 8)
      pBmp->SetPalette (*ppPal);

    switch (pBMI->biBitCount)
    {
      case 1:
        decode1bpp (pDataSrc, pBmp, *pDestBPP);
        break;
      case 4:
        if (pBMI->biCompression == BI_RGB)
          decode4bpp (pDataSrc, pBmp, *ppPal, *pDestBPP);
         else
          decodeRLE4 (pDataSrc, pBmp, *ppPal, *pDestBPP);
        break;
      case 8:
        if (pBMI->biCompression == BI_RGB)
          decode8bpp (pDataSrc, pBmp, *ppPal, *pDestBPP);
         else
          decodeRLE8 (pDataSrc, pBmp, *ppPal, *pDestBPP);
        break;
      case 24:
        decode24bpp (pDataSrc, pBmp);
        break;
      case 32:
        decode32bpp (pDataSrc, pBmp);
        break;
      default:
        // This is not a standard bmp file.
        raiseError (ERR_FORMAT_UNKNOWN,
                    "Decoding bmp: Illegal bpp value.");
    }
    delete pBMI;
    pBMI = NULL;
    Trace (3, "Decoding finished.\n");
  }
  catch (CTextException)
  {
    if (pBMI)
      delete pBMI;
    throw;
  }
  catch(...)
  {
	  delete pBMI;
	  throw;
  }
}

WINBITMAPINFOHEADER * CBmpDecoder::getInfoHeader
    ( CDataSource * pDataSrc,
      RGBAPIXEL ** ppPal
    )
    // Decodes the bitmap file & info headers
{
  WINBITMAPFILEHEADER BFH;
  WINBITMAPINFOHEADER * pBMI;

  BFH.bfType = ReadIWord (pDataSrc);
  BFH.bfSize = ReadILong (pDataSrc);
  BFH.bfReserved1 = ReadIWord (pDataSrc);
  BFH.bfReserved2 = ReadIWord (pDataSrc);
  BFH.bfOffBits = ReadILong (pDataSrc);

  // Check for bitmap file signature: First 2 bytes are 'BM'
  if (BFH.bfType != 0x4d42)
    raiseError (ERR_WRONG_SIGNATURE,
                "Bitmap decoder: This isn't a bitmap.");

  Trace (2, "Bitmap file signature found\n");

  pBMI = new WINBITMAPINFOHEADER;

  try
  {
    pBMI->biSize = ReadILong (pDataSrc);
    pBMI->biWidth = ReadILong (pDataSrc);
    pBMI->biHeight = ReadILong (pDataSrc);
    pBMI->biPlanes = ReadIWord (pDataSrc);
    pBMI->biBitCount = ReadIWord (pDataSrc);
    pBMI->biCompression = ReadILong (pDataSrc);
    pBMI->biSizeImage = ReadILong (pDataSrc);
    pBMI->biXPelsPerMeter = ReadILong (pDataSrc);
    pBMI->biYPelsPerMeter = ReadILong (pDataSrc);
    pBMI->biClrUsed = ReadILong (pDataSrc);
    pBMI->biClrImportant = ReadILong (pDataSrc);

    // Do sanity check
    if (pBMI->biSize < sizeof (WINBITMAPINFOHEADER))
      raiseError (ERR_FORMAT_UNKNOWN,
                  "Bitmap decoder: BITMAPINFOHEADER has wrong size.");

    Trace (2, "Bitmap header is ok.\n");

    // Read palette if 8 bpp or less.
    int PaletteSize = 0;
    if (pBMI->biBitCount <= 8)
      PaletteSize = readPalette (pBMI, pDataSrc, ppPal);

    //Jump to the bitmap bits offset=20
    pDataSrc->Skip(BFH.bfOffBits -sizeof (BITMAPFILEHEADER)-sizeof
                   (BITMAPINFOHEADER)-PaletteSize);

  }
  catch (CTextException)
  {
    if (pBMI)
      delete pBMI;
    throw;
  }
  catch(...)
  {
	  delete pBMI;
	  throw;
  }
  return pBMI;
}

void CBmpDecoder::decode1bpp
    ( CDataSource * pDataSrc,
      CBmp * pBmp,
      int DestBPP
    )
    // Decodes a 2-color bitmap. Ignores the palette & just uses
    // black & white as 'colors' if decoding to 32 bit
{
  int i;
  int y;                           // Current row
  int x;                           // Current column

  BYTE * pDest;                    // Current destination.
  BYTE * pSrc;                     // Current position in file.
  BYTE   BTable[8];                // Table of bit masks.
  BYTE   SrcByte;                  // Source byte cache.
  int    XSize = pBmp->GetWidth(); // Width of bitmap in pixels.
  int    LineLen = ((XSize+7)/8 + 3) & ~3;
                                   // Width of source in bytes
                                   //   (DWORD-aligned).
  int    LinePadding = LineLen-((XSize+7)/8);
  BYTE ** pLineArray = pBmp->GetLineArray();
                                   // Pointers to dest. lines.

  int    OpaqueBlack = 0x00000000;
  *(((BYTE*)&OpaqueBlack)+RGBA_ALPHA) = 0xFF;

  Trace (2, "Decoding 1 bit per pixel bitmap.\n");

  // Initialize bit masks.
  for (i=0; i<8; i++)
  {
    BTable[i] = 1<<i;
  }

  for (y=0; y<pBmp->GetHeight(); y++)
  { // For each line...
    pDest = pLineArray[pBmp->GetHeight()-y-1];
    for (x=0; x<XSize/8; x++)
    { // For each source byte...
      pSrc = pDataSrc->Read1Byte();
      SrcByte = *(pSrc);
      for (i=7; i>=0; i--)
      { // For each bit...
        if (DestBPP == 32)
        {
          if (SrcByte & BTable[i]) // Test if bit i is set
            *((LONG *)pDest) = 0xFFFFFFFF;
           else
            *((LONG *)pDest) = OpaqueBlack;
          pDest += 4;
        }
        else
        {
          if (SrcByte & BTable[i]) // Test if bit i is set
            *pDest = 0x01;
           else
            *pDest = 0x00;
          pDest++;
        }
      }
    }

    // Last few bits in line...
    if (XSize & 7)
    {
      pSrc = pDataSrc->Read1Byte();
      SrcByte = *(pSrc);
      for (i=7; i>7-(XSize & 7); i--)
      { // For each bit...
        if (DestBPP == 32)
        {
          if (SrcByte & BTable[i]) // Test if bit i is set
            *((LONG *)pDest) = 0xFFFFFFFF;
           else
            *((LONG *)pDest) = OpaqueBlack;
          pDest += 4;
        }
        else
        {
          if (SrcByte & BTable[i]) // Test if bit i is set
            *pDest = 0x01;
           else
            *pDest = 0x00;
          pDest++;
        }
      }
    }
    pDataSrc->Skip (LinePadding);
  }
}


void CBmpDecoder::decode4bpp
    ( CDataSource * pDataSrc,
      CBmp * pBmp,
      RGBAPIXEL * pPal,
      int DestBPP
    )
    // Decodes an uncompressed 16-color-bitmap.
{
  int y;                            // Current row
  int x;                            // Current column

  BYTE * pDest;                     // Current destination.
  BYTE * pSrc;                      // Current position in file.
  BYTE   SrcByte;                   // Source byte cache.
  int    XSize = pBmp->GetWidth();  // Width of bitmap in pixels.
  int    LineLen = ((XSize+1)/2 + 3) & ~3;
                                    // Width of source in bytes
                                    //   (DWORD-aligned).
  int    LinePadding = LineLen-((XSize+1)/2);
  BYTE ** pLineArray = pBmp->GetLineArray();
                                   // Pointers to dest. lines.

  Trace (2, "Decoding uncompressed 4 bit per pixel bitmap.\n");

  for (y=0; y<pBmp->GetHeight(); y++)
  { // For each line...
    pDest = pLineArray[pBmp->GetHeight()-y-1];
    for (x=0; x<XSize/2; x++)
    { // For each source byte...
      pSrc = pDataSrc->Read1Byte();
      SrcByte = *(pSrc);

      if (DestBPP == 32)
      {
        *((RGBAPIXEL *)pDest) = pPal[SrcByte>>4];
        pDest += 4;
        *((RGBAPIXEL *)pDest) = pPal[SrcByte & 15];
        pDest += 4;
      }
      else
      {
        *pDest = SrcByte>>4;
        pDest++;
        *pDest = SrcByte & 15;
        pDest++;
      }
   }

    // Last nibble in line if line length is odd.
    if (XSize & 1)
    {
      pSrc = pDataSrc->Read1Byte();
      if (DestBPP == 32)
      {
        *((RGBAPIXEL *)pDest) = pPal[(*(pSrc))>>4];
        pDest += 4;
      }
      else
      {
        *pDest = (*(pSrc))>>4;
        pDest++;
      }
    }
    pDataSrc->Skip (LinePadding);
  }
}


void CBmpDecoder::decode8bpp
    ( CDataSource * pDataSrc,
      CBmp * pBmp,
      RGBAPIXEL * pPal,
      int DestBPP
    )
    // Decodes an uncompressed 256-color-bitmap.
{
  int y;                            // Current row
  int x;                            // Current column

  BYTE * pDest;                     // Current destination.
  BYTE * pSrc;                      // Current position in file.
  int    XSize = pBmp->GetWidth();// Width of bitmap in pixels.
  int    LineLen = (XSize + 3) & ~3;
                                    // Width of source in bytes
                                    //   (DWORD-aligned).
  int    LinePadding = LineLen-XSize;
  BYTE ** pLineArray = pBmp->GetLineArray();
                                   // Pointers to dest. lines.

  Trace (2, "Decoding uncompressed 8 bit per pixel bitmap.\n");

  for (y=0; y<pBmp->GetHeight(); y++)
  { // For each line...
    pDest = pLineArray[pBmp->GetHeight()-y-1];
    for (x=0; x<XSize; x++)
    { // For each source byte...
      pSrc = pDataSrc->Read1Byte();
      if (DestBPP == 32)
      {
        *((RGBAPIXEL *)pDest) = pPal[*pSrc];
        pDest += 4;
      }
      else
      {
        *pDest = *pSrc;
        pDest++;
      }
    }
    pDataSrc->Skip (LinePadding);
  }
}


void CBmpDecoder::decodeRLE4
    ( CDataSource * pDataSrc,
      CBmp * pBmp,
      RGBAPIXEL * pPal,
      int DestBPP
    )
    // Decodes a compressed 16-color-bitmap.
{
  int y;                              // Current row

  BYTE * pSrc;
  BYTE * pDest;                       // Current destination.
  int    XSize = pBmp->GetWidth();  // Width of bitmap in pixels.
  BYTE   SrcByte;                     // Source byte cache.

  BYTE   RunLength;    // Length of current run.
  BOOL   bOdd;         // TRUE if current run has odd length.

  BOOL   bEOL;         // TRUE if end of line reached.
  BOOL   bEOF=FALSE;   // TRUE if end of file reached.

  BYTE * pLineBuf;     // Current line as uncompressed nibbles.
  BYTE * pBuf;         // Current position in pLineBuf.
  BYTE ** pLineArray = pBmp->GetLineArray();
                                   // Pointers to dest. lines.

  Trace (2, "Decoding RLE4-compressed bitmap.\n");

  // Allocate enough memory for DWORD alignment in original 4 bpp
  // bitmap.
  pLineBuf = new BYTE [XSize*4+28];

  for (y=0; y<pBmp->GetHeight() && !bEOF; y++)
  { // For each line...
    pBuf = pLineBuf;
    bEOL=FALSE;
    while (!bEOL)
    { // For each packet do
      pSrc = pDataSrc->Read1Byte();
      RunLength = *pSrc;
      if (RunLength==0)
      { // Literal or escape.
        pSrc = pDataSrc->Read1Byte();
        RunLength = *pSrc;
        switch (RunLength)
        {
          case 0: // End of line escape
            bEOL = TRUE;
            break;
          case 1: // End of file escape
            bEOF = TRUE;
            bEOL = TRUE;
            break;
          case 2: // Delta escape.
            // I have never seen a file using this.
            delete pLineBuf;
            raiseError (ERR_FORMAT_NOT_SUPPORTED,
                        "Encountered delta escape.");
            break;
          default:
            // Literal packet
            bOdd = (RunLength & 1);
            RunLength /= 2; // Convert pixels to bytes.
            for (int i=0; i<RunLength; i++)
            { // For each source byte...
              pSrc = pDataSrc->Read1Byte();
              decode2Nibbles (pBuf, *pSrc, pPal, DestBPP);
              if (DestBPP == 32)
                pBuf += 8;
              else
                pBuf += 2;
            }
            if (bOdd)
            { // Odd length packet -> one nibble left over
              pSrc = pDataSrc->Read1Byte();
              if (DestBPP == 32)
              {
                *((RGBAPIXEL *)pBuf) = pPal[(*(pSrc))>>4];
                pBuf += 4;
              }
              else
              {
                *pBuf = (*(pSrc))>>4;
                pBuf++;
              }
            }
            // Word alignment at end of literal packet.
            if ((RunLength + bOdd) & 1) pDataSrc->Skip(1);
        }
      }
      else
      { // Encoded packet:
        // RunLength 4 bpp pixels with 2 alternating
        // values.
        pSrc = pDataSrc->Read1Byte();
        SrcByte = *pSrc;
        for (int i=0; i<RunLength/2; i++)
        {
          decode2Nibbles (pBuf, SrcByte, pPal, DestBPP);
          if (DestBPP == 32)
            pBuf += 8;
          else
            pBuf += 2;
        }
        if (RunLength & 1)
        {
          if (DestBPP == 32)
          {
            *((RGBAPIXEL *)pBuf) = pPal[(*(pSrc))>>4];
            pBuf += 4;
          }
          else
          {
            *pBuf = (*(pSrc))>>4;
            pBuf++;
          }
        }
      }
    }
    pDest = pLineArray[pBmp->GetHeight()-y-1];
    if (DestBPP == 32)
      memcpy (pDest, pLineBuf, XSize*4);
    else
      memcpy (pDest, pLineBuf, XSize);
  }
  delete pLineBuf;
}


void CBmpDecoder::decodeRLE8
    ( CDataSource * pDataSrc,
      CBmp * pBmp,
      RGBAPIXEL * pPal,
      int DestBPP
    )
    // Decodes a compressed 256-color-bitmap.
{
  int y;                              // Current row

  BYTE * pDest;                       // Current destination.
  BYTE * pSrc;                        // Current position in file.
  BYTE   RunLength;                   // Length of current run.
  BOOL   bEOL;                        // TRUE if end of line reached.
  BOOL   bEOF=FALSE;                  // TRUE if end of file reached.
  BYTE ** pLineArray = pBmp->GetLineArray();
                                      // Pointers to dest. lines.

  Trace (2, "Decoding RLE8-compressed bitmap.\n");

  for (y=0; y<pBmp->GetHeight() && !bEOF; y++)
  { // For each line...
    pDest = pLineArray[pBmp->GetHeight()-y-1];
    bEOL=FALSE;
    while (!bEOL)
    { // For each packet do
      pSrc = pDataSrc->Read1Byte();
      RunLength = *pSrc;
      if (RunLength==0)
      { // Literal or escape.
        pSrc = pDataSrc->Read1Byte();
        RunLength = *pSrc;
        switch (RunLength)
        {
          case 0: // End of line escape
            bEOL = TRUE;
            break;
          case 1: // End of file escape
            bEOF = TRUE;
            bEOL = TRUE;
            break;
          case 2: // Delta escape.
            // I have never seen a file using this...
            raiseError (ERR_FORMAT_NOT_SUPPORTED,
                        "Encountered delta escape.");
            bEOL = TRUE;
            bEOF = TRUE;
            break;
          default:
            // Literal packet
            if (DestBPP == 32)
            {
              for (int i=0; i<RunLength; i++)
              { // For each source byte...
                pSrc = pDataSrc->Read1Byte();
                *((RGBAPIXEL *)pDest) = pPal[*pSrc];
                pDest += 4;
              }
            }
            else
            {
              pSrc = pDataSrc->ReadNBytes(RunLength);
              memcpy (pDest, pSrc, RunLength);
              pDest += RunLength;
            }
            // Word alignment at end of literal packet.
            if (RunLength & 1) pDataSrc->Skip(1);
        }
      }
      else
      { // Encoded packet:
        // RunLength pixels, all with the same value
        if (DestBPP == 32)
        {
          pSrc = pDataSrc->Read1Byte();
          RGBAPIXEL PixelVal = pPal[*pSrc];
          for (int i=0; i<RunLength; i++)
          {
            *((RGBAPIXEL *)pDest) = PixelVal;
            pDest += 4;
          }
        }
        else
        {
          pSrc = pDataSrc->Read1Byte();
          memset (pDest, *pSrc, RunLength);
          pDest += RunLength;
        }
      }
    }
  }
}


void CBmpDecoder::decode24bpp
    ( CDataSource * pDataSrc,
      CBmp * pBmp
    )
    // Decodes true-color bitmap
{
  int y;                            // Current row
  int x;                            // Current column

  BYTE * pDest;                     // Current destination.
  BYTE * pSrc;                      // Current position in file.
  int    XSize = pBmp->GetWidth();// Width of bitmap in pixels.
  int    LineLen = (XSize*3 + 3) & ~3;
                                    // Width of source in bytes
                                    //   (DWORD-aligned).
  int    LinePadding = LineLen - XSize*3;
  BYTE ** pLineArray = pBmp->GetLineArray();
                                   // Pointers to dest. lines.

  Trace (2, "Decoding 24 bit per pixel bitmap.\n");

  for (y=0; y<pBmp->GetHeight(); y++)
  { // For each line...
    pDest = pLineArray[pBmp->GetHeight()-y-1];
    for (x=0; x<XSize; x++)
    { // For each pixel...
      pSrc = pDataSrc->ReadNBytes(3);
      *(pDest+RGBA_BLUE) = ((WINRGBQUAD *)pSrc)->rgbBlue;
      *(pDest+RGBA_GREEN) = ((WINRGBQUAD *)pSrc)->rgbGreen;
      *(pDest+RGBA_RED) = ((WINRGBQUAD *)pSrc)->rgbRed;
      *(pDest+RGBA_ALPHA) = 0xFF;
      pDest += 4;
    }
    pDataSrc->Skip (LinePadding);
  }
}

void CBmpDecoder::decode32bpp
    ( CDataSource * pDataSrc,
      CBmp * pBmp
    )
    // Decodes true-color bitmap
{
  int y;                            // Current row

  BYTE * pSrc;                      // Start of current row in file.
  BYTE * pDest;                     // Current destination.
  int    XSize = pBmp->GetWidth();// Width of bitmap in pixels.
  int    LineLen = XSize*4;
                                    // Width of source in bytes
                                    //   (DWORD-aligned).
  BYTE ** pLineArray = pBmp->GetLineArray();
                                   // Pointers to dest. lines.

  Trace (2, "Decoding 32 bit per pixel bitmap.\n");

  for (y=0; y<pBmp->GetHeight(); y++)
  { // For each line...
    pDest = pLineArray[pBmp->GetHeight()-y-1];
    pSrc = pDataSrc->ReadNBytes (LineLen);
    memcpy (pDest, pSrc, XSize*4);
  }
}

void CBmpDecoder::decode2Nibbles
    ( BYTE * pDest,
      BYTE SrcByte,
      RGBAPIXEL * pPal,
      int DestBPP
    )
    // Decodes two 4-bit pixels.
{
  if (DestBPP == 32)
  {
    // High nibble
    *((RGBAPIXEL *)pDest) = pPal[SrcByte>>4];
    // Low nibble
    *((RGBAPIXEL *)(pDest+4)) = pPal[SrcByte & 15];
  }
  else
  {
    *pDest = SrcByte>>4;
    *(pDest+1) = SrcByte & 15;
  }
}

int CBmpDecoder::readPalette
    ( WINBITMAPINFOHEADER * pBMI,     // Pointer to bitmapinfoheader in file.
      CDataSource * pDataSrc,
      RGBAPIXEL ** ppPal
    )
    // Assumes 8 bpp or less.
{
  Trace (3, "Reading palette.\n");
  int i;

  int NumColors;
  if (pBMI->biClrUsed == 0)
    NumColors = 1<<(pBMI->biBitCount);
   else
    NumColors = pBMI->biClrUsed;

  WINRGBQUAD * pFilePal = (WINRGBQUAD *) pDataSrc->ReadNBytes
                                    (NumColors*sizeof (WINRGBQUAD));
  *ppPal = new RGBAPIXEL  [256];
  RGBAPIXEL * pPal = *ppPal;
  // Correct the byte ordering & copy the data.
  for (i=0; i<NumColors; i++)
  {
    *(((BYTE *)pPal)+i*4+RGBA_BLUE) = pFilePal[i].rgbBlue;
    *(((BYTE *)pPal)+i*4+RGBA_GREEN) = pFilePal[i].rgbGreen;
    *(((BYTE *)pPal)+i*4+RGBA_RED) = pFilePal[i].rgbRed;
    *(((BYTE *)pPal)+i*4+RGBA_ALPHA) = 0xFF;
  }

  return NumColors*4;
}
/*
/--------------------------------------------------------------------
|
|      $Log: bmpdec.cpp,v $
|      Revision 1.9  2000/01/16 20:43:12  anonymous
|      Removed MFC dependencies
|
|      Revision 1.8  2000/01/04 18:36:02  Ulrich von Zadow
|      Corrected handling of bitmap files with extended headers.
|
|      Revision 1.7  1999/12/30 15:54:02  Ulrich von Zadow
|      no message
|
|      Revision 1.6  1999/12/08 15:39:45  Ulrich von Zadow
|      Unix compatibility changes
|
|      Revision 1.5  1999/11/27 18:45:48  Ulrich von Zadow
|      Added/Updated doc comments.
|
|      Revision 1.4  1999/10/03 18:50:51  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
--------------------------------------------------------------------
*/
