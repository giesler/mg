/*
/--------------------------------------------------------------------
|
|      $Id: tgadec.cpp,v 1.4 2000/01/16 20:43:14 anonymous Exp $
|      Targa Decoder Class
|
|      Targa file decoder. Decodes 8, 15, 16, 24 and 32 bpp
|      targa files (compressed and uncompressed) and returns a 32
|      bpp bitmap. Preserves the alpha channel.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "tgadec.h"
#include "except.h"


CTGADecoder::CTGADecoder
    ()
    // Creates a decoder
{
}


CTGADecoder::~CTGADecoder
    ()
{
}


void CTGADecoder::DoDecode
    ( CBmp * pBmp,
      RGBAPIXEL ** ppPal,
      int * pDestBPP,
      CDataSource * pDataSrc
    )
{
  TGAHEADER TgaHead;

  Trace (2, "Decoding TGA.\n");

  readTgaHeader (&TgaHead, pDataSrc);

  CalcDestBPP (TgaHead.PixelDepth, pDestBPP);

  if (TgaHead.PixelDepth == 16 || TgaHead.PixelDepth == 32)
    // Image has alpha channel
    pBmp->Create (TgaHead.ImageWidth, TgaHead.ImageHeight,
                    *pDestBPP, TRUE);
  else
    // Image doesn't have alpha channel
    pBmp->Create (TgaHead.ImageWidth, TgaHead.ImageHeight,
                  *pDestBPP, FALSE);

  // Read the color map data (Version 1.0 and 2.0).
  if (TgaHead.CmapType != 0)
    readPalette (TgaHead.CmapIndex,
                 TgaHead.CmapLength,
                 TgaHead.CmapEntrySize,
                 pBmp,
                 ppPal,
                 *pDestBPP,
                 pDataSrc);

  readImage (&TgaHead, pBmp, ppPal, *pDestBPP, pDataSrc);
}


void CTGADecoder::readTgaHeader
    ( TGAHEADER * pTgaHead,       // Pointer to TGA header structure
      CDataSource * pDataSrc
    )
{
  // Read the TGA header (Version 1.0 and 2.0).
  pTgaHead->IdLength      = ReadByte (pDataSrc);
  pTgaHead->CmapType      = ReadByte (pDataSrc);
  pTgaHead->ImageType     = ReadByte (pDataSrc);
  pTgaHead->CmapIndex     = ReadIWord (pDataSrc);
  pTgaHead->CmapLength    = ReadIWord (pDataSrc);
  pTgaHead->CmapEntrySize = ReadByte (pDataSrc);
  pTgaHead->X_Origin      = ReadIWord (pDataSrc);
  pTgaHead->Y_Origin      = ReadIWord (pDataSrc);
  pTgaHead->ImageWidth    = ReadIWord (pDataSrc);
  pTgaHead->ImageHeight   = ReadIWord (pDataSrc);
  pTgaHead->PixelDepth    = ReadByte (pDataSrc);
  pTgaHead->ImagDesc      = ReadByte (pDataSrc);

  // Skip image ID
  pDataSrc->Skip (pTgaHead->IdLength);
}


void CTGADecoder::readPalette
    ( int StartIndex,           // Index of first palette entry.
      int Length,               // Number of palette entries stored.
      int EntrySize,            // Size of palette entries in bits.
      CBmp * pBmp,
      RGBAPIXEL ** ppPal,
      int DestBPP,
      CDataSource * pDataSrc
    )
    // Reads the TGA palette and creates a windows palette.
{
  int   i;

  *ppPal = new RGBAPIXEL [256];
  if (!(*ppPal))
    raiseError (ERR_NO_MEMORY, "Out of memory for palette.");

  for (i=StartIndex; i<StartIndex+Length; i++)
  {
    (*ppPal)[i] = readPixel32 (EntrySize, pDataSrc, *ppPal);
  }

  if (DestBPP == 8)
    pBmp->SetPalette (*ppPal);
}


void CTGADecoder::setGrayPalette
    ( RGBAPIXEL ** ppPal
    )
    // Creates a grayscale palette.
{
  int i;
  *ppPal = new RGBAPIXEL [256];
  for (i=0; i<256; i++)
  {
    SetRGBAPixel ((*ppPal)+i, i, i, i, 0xFF);
  }
}


void CTGADecoder::readImage
    ( TGAHEADER * pTgaHead,       // Pointer to TGA header structure
      CBmp * pBmp,
      RGBAPIXEL ** ppPal,
      int DestBPP,
      CDataSource * pDataSrc
    )
{
  BOOL bCompressed;

  if (pTgaHead->ImageType == TGA_Mono ||
      pTgaHead->ImageType == TGA_RLEMono)
    setGrayPalette (ppPal);

  switch (pTgaHead->ImageType)
  {
    case TGA_Map:
    case TGA_RGB:
    case TGA_Mono:
      bCompressed = FALSE;
      break;
    case TGA_RLEMap:
    case TGA_RLERGB:
    case TGA_RLEMono:
      bCompressed = TRUE;
      break;
    default:
      raiseError (ERR_FORMAT_UNKNOWN, "Unknown TGA image type.");
  }
  readData (pTgaHead, bCompressed, pBmp, *ppPal, DestBPP, pDataSrc);
}


void CTGADecoder::readData
    ( TGAHEADER * pTgaHead,       // Pointer to TGA header structure
      BOOL bCompressed,
      CBmp * pBmp,
      RGBAPIXEL * pPal,
      int DestBPP,
      CDataSource * pDataSrc
    )
{
  int Width = pTgaHead->ImageWidth;
  int Height = pTgaHead->ImageHeight;
  int bpp = pTgaHead->PixelDepth;

  // Bits 4 & 5 of the Image Descriptor byte control the ordering of
  // the pixels.
  BOOL bXReversed = ((pTgaHead->ImagDesc & 16) == 16);
  BOOL bYReversed = ((pTgaHead->ImagDesc & 32) == 32);

  BYTE * pDest;
  BYTE ** pLineArray = pBmp->GetLineArray();

  int y;

  for (y=0; y < Height; y++)
  {
    if (bYReversed)
      pDest = pLineArray[y];
     else
      pDest = pLineArray[Height-y-1];

    if (!bCompressed)
      expandUncompressedLine (pDest, Width, bXReversed, bpp, pDataSrc, pPal, DestBPP);
     else
      expandCompressedLine (pDest, Width, bXReversed, bpp, pDataSrc, pPal, DestBPP);
  }
}


void CTGADecoder::expandUncompressedLine
    ( BYTE * pDest,
      int Width,
      BOOL bReversed,
      int bpp,
      CDataSource * pDataSrc,
      RGBAPIXEL * pPal,
      int DestBPP
    )
{
  int x;

  for (x=0; x<Width; x++)
  {
    if (DestBPP == 32)
    {
      *((RGBAPIXEL *)pDest) = readPixel32 (bpp, pDataSrc, pPal);
      pDest += 4;
    }
    else
    {
      *pDest = readPixel8 (bpp, pDataSrc);
      pDest ++;
    }
  }
}


void CTGADecoder::expandCompressedLine
    ( BYTE * pDest,
      int Width,
      BOOL bReversed,
      int bpp,
      CDataSource * pDataSrc,
      RGBAPIXEL * pPal,
      int DestBPP
    )
{
  int  x;
  int  i;
  BYTE Count;

  for (x=0; x<Width; )
  {
    Count = ReadByte (pDataSrc);
    if (Count & 128)
    { // RLE-Encoded packet
      Count -= 127; // Calculate real repeat count.
      if (DestBPP == 32)
      {
        *((RGBAPIXEL *)pDest) = readPixel32 (bpp, pDataSrc, pPal);
        for (i=1; i<Count; i++)
          *((RGBAPIXEL *)(pDest+i*4)) = *(RGBAPIXEL *)pDest;
      }
      else
      {
        *pDest = readPixel8 (bpp, pDataSrc);
        for (i=1; i<Count; i++)
          *(pDest+i) = *pDest;
      }
    }
    else
    { // Raw packet
      Count += 1; // Calculate real repeat count.
      for (i=0; i<Count; i++)
      {
        if (DestBPP == 32)
          *((RGBAPIXEL *)(pDest+i*4)) = readPixel32 (bpp, pDataSrc, pPal);
        else
          *(pDest+i) = readPixel8 (bpp, pDataSrc);
      }
    }
    if (DestBPP == 32)
      pDest += Count*4;
    else
      pDest += Count;
    x += Count;
  }
}


RGBAPIXEL CTGADecoder::readPixel32
    ( int bpp,
      CDataSource * pDataSrc,
      RGBAPIXEL * pPal
    )
    // The decoder could be made much faster if the big switch (bpp)
    // statement was taken out of the inner loop. On the other hand,
    // doing that would cause a lot of code to be repeated half a
    // dozen times...
{
  RGBAPIXEL Dest;
  WORD Src;
  BYTE * pCurEntry;

  switch (bpp)
  {
    case 8:
      Dest = pPal[ReadByte(pDataSrc)];
      break;
    case 15:
    case 16:
      Src = ReadIWord(pDataSrc);
      if (bpp == 16)
        SetRGBAPixel (&Dest,
                      ( Src >> 7 ) & 0x0F8,     // red
                      ( Src >> 2 ) & 0x0F8,     // green
                      ( Src & 0x1F ) * 8,       // blue
                      (BYTE)(Src & 32786 >> 8));
       else
        SetRGBAPixel (&Dest,
                      ( Src >> 7 ) & 0x0F8,     // red
                      ( Src >> 2 ) & 0x0F8,     // green
                      ( Src & 0x1F ) * 8,       // blue
                      0xFF);
      break;
    case 24:
      pCurEntry = pDataSrc->ReadNBytes (3);
      SetRGBAPixel (&Dest, *(pCurEntry+2),
                    *(pCurEntry+1), *(pCurEntry), 0xFF);
      break;
    case 32:
      pCurEntry = pDataSrc->ReadNBytes (4);
      SetRGBAPixel (&Dest, *(pCurEntry+2), *(pCurEntry+1),
                    *(pCurEntry), *(pCurEntry+3));
      break;
  }
  return Dest;
}

BYTE CTGADecoder::readPixel8
    ( int bpp,
      CDataSource * pDataSrc
    )
{
  BYTE Dest;

  PLASSERT (bpp == 8);

  Dest = ReadByte(pDataSrc);

  return Dest;
}

/*
/--------------------------------------------------------------------
|
|      $Log: tgadec.cpp,v $
|      Revision 1.4  2000/01/16 20:43:14  anonymous
|      Removed MFC dependencies
|
|      Revision 1.3  1999/10/03 18:50:52  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
--------------------------------------------------------------------
*/
