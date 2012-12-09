/*
/--------------------------------------------------------------------
|
|      $Id: pictdec.cpp,v 1.6 2000/03/15 17:23:20 Ulrich von Zadow Exp $
|      Macintosh pict Decoder Class
|
|      This class decodes macintosh PICT files with 1,2,4,8,16 and 32
|      bits per pixel as well as PICT/JPEG. If an alpha channel is
|      present in a 32-bit-PICT, it is decoded as well.
|      The PICT format is a general picture file format and can
|      contain a lot of other elements besides bitmaps. These elements
|      are ignored.
|      There are several opcodes for which I did not find examples.
|      I have marked the appropriate code as "untested". It'll
|      probably work anyway.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "pictdec.h"
#ifdef SUPPORT_JPEG
#include "jpegdec.h"
#endif
#include "optable.h"
#include "except.h"


CPictDecoder::CPictDecoder
    ( CJPEGDecoder * pJPEGDecoder
    )
    // Creates a decoder
{
  m_pJPEGDecoder = pJPEGDecoder;
}


CPictDecoder::~CPictDecoder
    ()
{
}


void CPictDecoder::DoDecode
    ( CBmp * pBmp,
      RGBAPIXEL ** ppPal,
      int * pDestBPP,
      CDataSource * pDataSrc
    )
    // Called by MakeDIB to do the actual decoding.
{
  int Version;

  Trace (2, "Decoding mac pict.\n");

  // Skip empty 512 byte header.
  pDataSrc->Skip (512);

  // Read PICT header
  readHeader (pDataSrc, Version);

  interpretOpcodes (pBmp, ppPal, pDestBPP, pDataSrc, Version);
}

void CPictDecoder::readHeader
    ( CDataSource * pDataSrc,
      int& Version
    )
    // Decodes header and version information.
    // Performs checks to make sure the data is really a pict file.
{
  BYTE ch;
  WORD PicSize;  // Version 1 picture size. Ignored in version 2.
  char sz[256];
  MacRect Frame;

  PicSize = ReadMWord (pDataSrc);

  readRect (&Frame, pDataSrc);

  while ((ch = ReadByte(pDataSrc)) == 0);
  if (ch != 0x11)
                raiseError (ERR_WRONG_SIGNATURE,
                "Error decoding pict: Version number missing.");

  switch (ReadByte(pDataSrc))
  {
    case 1:
      Version = 1;
      break;
    case 2:
      if (ReadByte(pDataSrc) != 0xff)
        raiseError (ERR_WRONG_SIGNATURE,
                    "Illegal version number.");
      Version = 2;
      break;
    default:
      raiseError (ERR_WRONG_SIGNATURE,
                  "Illegal version number.");
  }

  sprintf (sz, "PICT version %d found.\n", Version);
  Trace (2, sz);
}


void CPictDecoder::interpretOpcodes
    ( CBmp * pBmp,
      RGBAPIXEL ** ppPal,
      int * pDestBPP,
      CDataSource * pDataSrc,
      int& Version
    )
    // This is the main decoder loop. The functions reads opcodes,
    // skips some, and hands the rest to opcode-specific functions.
    // It stops decoding after the first opcode containing bitmapped
    // data.
{
  WORD   Opcode;
  char   sz[256];

  BOOL   bDone = FALSE;

  while (!bDone)
  {
    Opcode = readOpcode(Version, pDataSrc);

    if (Opcode == 0xFF || Opcode == 0xFFFF)
    {
      bDone = TRUE;
      Trace (2, "Opcode: End of pict.\n");
      raiseError (ERR_FORMAT_NOT_SUPPORTED,
                  "PICT contained only vector data!\n");
    }
    else if (Opcode < 0xa2)
    {
      if (!strcmp(optable[Opcode].name, "reserved"))
        sprintf (sz, "Opcode: reserved=0x%x\n", Opcode);
      else
        sprintf (sz, "Opcode: %s\n", optable[Opcode].name);
      Trace (2, sz);

      switch (Opcode)
      {
        case 0x01: // Clip
          clip (pDataSrc);
          break;
        case 0x12:
        case 0x13:
        case 0x14:
          pixPat (pDataSrc);
          break;
        case 0x70:
        case 0x71:
        case 0x72:
        case 0x73:
        case 0x74:
        case 0x75:
        case 0x76:
        case 0x77:
          skipPolyOrRegion (pDataSrc);
          break;
        case 0x90:
        case 0x98:
          bitsRect (pDataSrc, ppPal, pBmp, pDestBPP);
          bDone = TRUE;
          break;
        case 0x91:
        case 0x99:
          bitsRegion (pDataSrc, ppPal, pBmp, pDestBPP);
          bDone = TRUE;
          break;
        case 0x9a:
          opcode9a (pDataSrc, pDestBPP, pBmp, ppPal);
          bDone = TRUE;
          break;
        case 0xa1:
          longComment (pDataSrc);
          break;
        default:
          // No function => skip to next Opcode
          if (optable[Opcode].len == WORD_LEN)
            pDataSrc->Skip(ReadMWord(pDataSrc));
           else
            pDataSrc->Skip(optable[Opcode].len);
      }
    }
    else if (Opcode == 0xc00)
    {
      Trace (2, "Opcode: Header.\n");
      pDataSrc->Skip(24);
    }
    else if (Opcode == 0x8200)
    {
      Trace (2, "Opcode: JPEG.\n");
      jpegOp (pDataSrc, pBmp, pDestBPP);
      bDone = TRUE;
    }
    else if (Opcode >= 0xa2 && Opcode <= 0xaf)
    {
      sprintf (sz, "Opcode: reserved 0x%x.\n", Opcode);
      Trace (2, sz);
      pDataSrc->Skip(ReadMWord(pDataSrc));
    }
    else if ((Opcode >= 0xb0 && Opcode <= 0xcf) ||
             (Opcode >= 0x8000 && Opcode <= 0x80ff))
    {
      // just a reserved Opcode, no data
      sprintf (sz, "Opcode: reserved 0x%x.\n", Opcode);
      Trace (2, sz);
    }
    else if ((Opcode >= 0xd0 && Opcode <= 0xfe) ||
             (Opcode >= 8100 && Opcode <= 0xffff))
    {
      sprintf (sz, "Opcode: reserved 0x%x.\n", Opcode);
      Trace (2, sz);
      pDataSrc->Skip(ReadMLong(pDataSrc));
    }
    else if (Opcode >= 0x100 && Opcode <= 0x7fff)
    {
      sprintf (sz, "Opcode: reserved 0x%x.\n", Opcode);
      Trace (2, sz);
      pDataSrc->Skip((Opcode >> 7) & 255);
    }
    else
    {
      char sz[256];
      sprintf (sz, "Can't handle Opcode %x.\n", Opcode);
      raiseError (ERR_FORMAT_UNKNOWN, sz);
    }
  }
}


WORD CPictDecoder::readOpcode
    ( int Version,
      CDataSource * pDataSrc
    )
    // moves to an even byte position in the file and returns the
    // opcode found.
{
  if (Version == 2)
    pDataSrc->AlignToWord();

  if (Version == 1)
    return ReadByte (pDataSrc);
   else
    return ReadMWord (pDataSrc);
}


/////////////////////////////////////////////////////////////////////
// Opcode functions

void CPictDecoder::clip
    ( CDataSource * pDataSrc
    )
    // skips clipping rectangle or region.
{
  MacRect ClipRect;

  WORD len = ReadMWord(pDataSrc);

  if (len == 0x000a)
    { /* null rgn */
      readRect(&ClipRect, pDataSrc);
    }
   else
    pDataSrc->Skip(len - 2);
}

void CPictDecoder::pixPat
    ( CDataSource * pDataSrc
    )
    // skips pattern definition.
{
  WORD        PatType;
  WORD        rowBytes;
  MacpixMap   p;
  RGBAPIXEL * pCT;
  WORD        NumColors;

  PatType = ReadMWord(pDataSrc);

  switch (PatType)
    {
      case 2:
        pDataSrc->Skip(8);
        pDataSrc->Skip(5);
        break;
      case 1:
        pDataSrc->Skip(8);
        rowBytes = ReadMWord(pDataSrc);
        readRect(&p.Bounds, pDataSrc);
        readPixmap(&p, pDataSrc);

        pCT = readColourTable(&NumColors, pDataSrc);
        skipBits(&p.Bounds, rowBytes, p.pixelSize, pDataSrc);
        delete pCT;
        break;
      default:
        raiseError (ERR_FORMAT_UNKNOWN,
                    "Unknown pattern type in pixPat.");
    }
}

void CPictDecoder::skipPolyOrRegion
    ( CDataSource * pDataSrc
    )
{
  Trace (3, "Skipping polygon or region.\n");
  pDataSrc->Skip (ReadMWord (pDataSrc) - 2);
}

void CPictDecoder::bitsRect
    ( CDataSource * pDataSrc,
      RGBAPIXEL **ppPal,
      CBmp * pBmp,
      int * pDestBPP
    )
    // Bitmap/pixmap data clipped by a rectangle.
{
  WORD rowBytes;

  rowBytes = ReadMWord(pDataSrc);    // Bytes per row in source when
                                     // uncompressed.
  if (rowBytes & 0x8000)
    doPixmap(rowBytes, FALSE, ppPal, pBmp, pDestBPP, pDataSrc);
   else
    doBitmap(rowBytes, FALSE, ppPal, pBmp, pDestBPP, pDataSrc);
}

void CPictDecoder::bitsRegion
    ( CDataSource * pDataSrc,
      RGBAPIXEL **ppPal,
      CBmp * pBmp,
      int * pDestBPP
    )
    // Bitmap/pixmap data clipped by a region.
    // Untested...
{
  WORD rowBytes;

  rowBytes = ReadMWord(pDataSrc);    // Bytes per row in source when
                                     // uncompressed.
  if (rowBytes & 0x8000)
    doPixmap(rowBytes, TRUE, ppPal, pBmp, pDestBPP, pDataSrc);
   else
    doBitmap(rowBytes, TRUE, ppPal, pBmp, pDestBPP, pDataSrc);
}

void CPictDecoder::opcode9a
    ( CDataSource * pDataSrc,
      int * pDestBPP,
      CBmp * pBmp,
      RGBAPIXEL ** ppPal
    )
    // DirectBitsRect.
{
  MacpixMap PixMap;

  pDataSrc->Skip(4);           // Skip fake len and fake EOF.
  ReadMWord(pDataSrc);         // bogus row bytes.

  // Read in the PixMap fields.
  readRect(&PixMap.Bounds, pDataSrc);
  readPixmap (&PixMap, pDataSrc);

  // Ignore source & destination rectangle as well as transfer mode.
  MacRect TempRect;
  readRect (&TempRect, pDataSrc);
  readRect (&TempRect, pDataSrc);
  WORD mode = ReadMWord(pDataSrc);

  // Create empty DIB
  CalcDestBPP (PixMap.pixelSize, pDestBPP);
  createOutputBmp (PixMap, *pDestBPP, pBmp);

  // Do the actual unpacking.
  switch (PixMap.pixelSize)
  {
    case 32:
      unpack32bits (&PixMap.Bounds, 0, PixMap.cmpCount, pBmp, pDataSrc);
      break;
    case 8:
      unpack8bits (&PixMap.Bounds, 0, pBmp, pDataSrc, ppPal, *pDestBPP);
      break;
    default:
      unpackbits (&PixMap.Bounds, 0, PixMap.pixelSize, pBmp, pDataSrc, ppPal, *pDestBPP);
  }
}

void CPictDecoder::longComment
    ( CDataSource * pDataSrc
    )
{
  WORD type;
  WORD len;

  type = ReadMWord(pDataSrc);
  len = ReadMWord(pDataSrc);
  if (len > 0)
    pDataSrc->Skip (len);
}

void CPictDecoder::jpegOp
    ( CDataSource * pDataSrc,
      CBmp * pBmp,
      int * pDestBPP
    )
    // Invoke the JPEG decoder for this PICT.
{
  long OpLen = ReadMLong(pDataSrc);
  BOOL bFound = FALSE;
  int i = 0;

  // Make sure the client allowed 32-bit output.
  CalcDestBPP (32, pDestBPP);

  // skip to JPEG header.
  while (!bFound && i < OpLen)
  {
    BYTE * pData = pDataSrc->GetBufferPtr (3);
    if (pData[0] == 0xFF && pData[1] == 0xD8 && pData[2] == 0xFF)
      bFound = TRUE;
    else
    {
      ReadByte(pDataSrc);
      i++;
    }
  }
  if (bFound)
    // Pass the data to the JPEG decoder.
    if (m_pJPEGDecoder)
#ifdef SUPPORT_JPEG
      m_pJPEGDecoder->MakeBmp (pDataSrc, pBmp);
#else
      raiseError (ERR_FORMAT_NOT_SUPPORTED,
                  "Library not compiled for PICT/JPEG.");
#endif
     else
      raiseError (ERR_FORMAT_NOT_SUPPORTED,
                  "Library not compiled for PICT/JPEG.");
   else
    raiseError (ERR_FORMAT_NOT_SUPPORTED,
                "PICT file contains unrecognized quicktime data.\n");
}

/////////////////////////////////////////////////////////////////////
// Bitmap & Pixmap functions

void CPictDecoder::createOutputBmp
    ( MacpixMap PixMap,
      int DestBPP,
      CBmp * pBmp
    )
{
  // Create empty DIB
  if (DestBPP == 32)
  { // 32-bit output.
    if (PixMap.cmpCount == 4)
    { // Alpha channel in picture.
      pBmp->Create (PixMap.Bounds.right - PixMap.Bounds.left,
                      PixMap.Bounds.bottom - PixMap.Bounds.top,
                      32,
                      TRUE);
    }
    else
    { // No alpha channel in picture.
      pBmp->Create (PixMap.Bounds.right - PixMap.Bounds.left,
                      PixMap.Bounds.bottom - PixMap.Bounds.top,
                      32,
                      FALSE);
    }
  }
  else
  { // 8-bit-output.
    pBmp->Create (PixMap.Bounds.right - PixMap.Bounds.left,
                    PixMap.Bounds.bottom - PixMap.Bounds.top,
                    8,
                    FALSE);
  }
}


void CPictDecoder::doBitmap
    ( int rowBytes,
      BOOL bIsRegion,
      RGBAPIXEL ** ppPal,
      CBmp * pBmp,
      int * pDestBPP,
      CDataSource * pDataSrc
    )
    // Decode version 1 bitmap: 1 bpp.
{
  MacRect Bounds;
  MacRect SrcRect;
  MacRect DstRect;
  WORD    mode;
  RGBAPIXEL pPal[] = { 0xFFFFFFFF, 0x00000000 };
  ((BYTE *)pPal)[4+RGBA_ALPHA] = 0xFF;

  WORD    width;        // Width in pixels
  WORD    height;       // Height in pixels

  Trace (2, "Reading version 1 bitmap.\n");

  readRect(&Bounds, pDataSrc);
  dumpRect ("  Bounds", &Bounds);
  readRect(&SrcRect, pDataSrc);
  readRect(&DstRect, pDataSrc);

  width = Bounds.right - Bounds.left;
  height = Bounds.bottom - Bounds.top;
  *ppPal = pPal;

  CalcDestBPP (1, pDestBPP);
  // Create empty DIB without resolution info.
  pBmp->Create (width, height, *pDestBPP, FALSE);

  mode = ReadMWord(pDataSrc);

  if (bIsRegion)
    skipPolyOrRegion (pDataSrc);

  unpackbits (&Bounds, rowBytes, 1, pBmp, pDataSrc, ppPal, *pDestBPP);
  *ppPal = NULL;
}

void CPictDecoder::doPixmap
    ( int rowBytes,
      BOOL bIsRegion,
      RGBAPIXEL ** ppPal,
      CBmp * pBmp,
      int * pDestBPP,
      CDataSource * pDataSrc
    )
    // Decode version 2 pixmap
{
  MacpixMap   PixMap;
  WORD        NumColors;    // Palette size.

  readRect(&PixMap.Bounds, pDataSrc);
  readPixmap(&PixMap, pDataSrc);

  CalcDestBPP (PixMap.pixelSize, pDestBPP);
  createOutputBmp (PixMap, *pDestBPP, pBmp);

  // Read mac colour table into windows palette.
  *ppPal = readColourTable (&NumColors, pDataSrc);
  if (*pDestBPP == 8)
  { // Copy palette
    pBmp->SetPalette (*ppPal);
  }

  // Ignore source & destination rectangle as well as transfer mode.
  MacRect TempRect;
  readRect (&TempRect, pDataSrc);
  readRect (&TempRect, pDataSrc);
  WORD mode = ReadMWord(pDataSrc);

  if (bIsRegion)
    skipPolyOrRegion (pDataSrc);

  switch (PixMap.pixelSize)
  {
    case 32:
      unpack32bits (&PixMap.Bounds, rowBytes, PixMap.cmpCount, pBmp, pDataSrc);
      break;
    case 8:
      unpack8bits (&PixMap.Bounds, rowBytes, pBmp, pDataSrc, ppPal, *pDestBPP);
      break;
    default:
      unpackbits (&PixMap.Bounds, rowBytes, PixMap.pixelSize,
                  pBmp, pDataSrc, ppPal, *pDestBPP);
  }
}

void CPictDecoder::unpack32bits
    ( MacRect* pBounds,
      WORD rowBytes,
      int NumBitPlanes,    // 3 if RGB, 4 if RGBA
      CBmp * pBmp,
      CDataSource * pDataSrc
    )
    // This routine decompresses BitsRects with a packType of 4 (and
    // 32 bits per pixel). In this format, each line is separated
    // into 8-bit-bitplanes and then compressed via RLE. To decode,
    // the routine decompresses each line & then juggles the bytes
    // around to get pixel-oriented data.
{
  BYTE * pSrcLine;           // Pointer to source line in file.
  int    i,j;
  WORD   BytesPerRow;        // bytes per row when uncompressed.
  int    linelen;            // length of source line in bytes.
  BYTE * pDestLine;
  BYTE   FlagCounter;
  int    len;

  BYTE * pLinebuf;           // Temporary buffer for line data. In
                             // this buffer, pixels are uncompressed
                             // but still plane-oriented.
  BYTE * pBuf;               // Current location in pLinebuf.
  BYTE ** pLineArray = pBmp->GetLineArray();


  int Height = pBounds->bottom - pBounds->top;
  int Width = pBounds->right - pBounds->left;

  BytesPerRow = Width*NumBitPlanes;

  if (rowBytes == 0)
    rowBytes = Width*4;

  // Allocate temporary line buffer.
  pLinebuf = new BYTE [BytesPerRow];

  try
  {
    for (i = 0; i < Height; i++)
    { // for each line do...
      if (rowBytes > 250)
        linelen = ReadMWord(pDataSrc);
       else
        linelen = ReadByte(pDataSrc);

      pSrcLine = pDataSrc->ReadNBytes(linelen);
      pBuf = pLinebuf;

      // Unpack bytewise RLE.
      for (j = 0; j < linelen; )
      {
        FlagCounter = pSrcLine[j];
        if (FlagCounter & 0x80)
        {
          if (FlagCounter == 0x80)
            // Special case: repeat 0 times.
            // Apple sais this happens with third-party encoders.
            // Ignore, since it's nonsense anyway.
            j++;
          else
          { // Real packed data.
            len = ((FlagCounter ^ 255) & 255) + 2;
            memset (pBuf, *(pSrcLine+j+1), len);
            pBuf += len;
            j += 2;
          }
        }
        else
        { // Unpacked data
          len = (FlagCounter & 255) + 1;
          memcpy (pBuf, pSrcLine+j+1, len);
          pBuf += len;
          j += len + 1;
        }
      }

      // Convert plane-oriented data into pixel-oriented data &
      // copy into destination bitmap.
      pDestLine = pLineArray[i];
      pBuf = pLinebuf;

      if (NumBitPlanes == 3)
      { // No alpha channel.
        for (j = 0; j < Width; j++)
        { // For each pixel in line...
          *(pDestLine+RGBA_BLUE) = *(pBuf+Width*2); // Blue
          *(pDestLine+RGBA_GREEN) = *(pBuf+Width);  // Green
          *(pDestLine+RGBA_RED) = *pBuf;            // Red
          *(pDestLine+RGBA_ALPHA) = 0xFF;           // Alpha
          pDestLine +=4;
          pBuf++;
        }
      }
      else
      { // Image with alpha channel.
        for (j = 0; j < Width; j++)
        { // For each pixel in line...
          *(pDestLine+RGBA_BLUE) = *(pBuf+Width*3);  // Blue
          *(pDestLine+RGBA_GREEN) = *(pBuf+Width*2); // Green
          *(pDestLine+RGBA_RED) = *(pBuf+Width);     // Red
          *(pDestLine+RGBA_ALPHA) = *pBuf;           // Alpha
          pDestLine+=4;
          pBuf++;
        }
      }
    }
  }
  catch (CTextException)
  {
    delete pLinebuf;
    throw;
  }
  catch(...)
  {
    delete pLinebuf;
    throw;
  }
  delete pLinebuf;
}


void CPictDecoder::unpack8bits
    ( MacRect* pBounds,
      WORD rowBytes,
      CBmp * pBmp,
      CDataSource * pDataSrc,
      RGBAPIXEL ** ppPal,
      int DestBPP
    )
    // Decompression routine for 8 bpp. rowBytes is the number of
    // bytes each source row would take if it were uncompressed.
    // This _isn't_ equal to the number of pixels in the row - it
    // seems apple pads the data to a word boundary and then
    // compresses it. Of course, we have to decompress the excess
    // data and then throw it away.
{
  BYTE  * pSrcLine;           // Pointer to source line in file.
  int     i,j,k;
  int     linelen;            // length of source line in bytes.
  BYTE  * pDestPixel;
  BYTE    FlagCounter;
  int     len;
  BYTE  * pLineBuf;
  BYTE ** pLineArray = pBmp->GetLineArray();

  int Height = pBounds->bottom - pBounds->top;
  int Width = pBounds->right - pBounds->left;

  // High bit of rowBytes is flag.
  rowBytes &= 0x7fff;

  if (rowBytes == 0)
    rowBytes = Width;

  pLineBuf = new BYTE [rowBytes * 4];

  try
  {
    if (rowBytes < 8)
    { // Ah-ha!  The bits aren't actually packed.  This will be easy.
      for (i = 0; i < Height; i++)
      {
        pSrcLine = pDataSrc->ReadNBytes (rowBytes);
        if (DestBPP == 32)
        {
          pDestPixel = pLineArray[i];
          expandBuf (pDestPixel, pSrcLine, Width, 8, *ppPal);
        }
        else
        {
          pDestPixel = pLineArray[i];
          memcpy (pDestPixel, pSrcLine, Width);
        }
      }
    }
    else
    {
      for (i = 0; i < Height; i++)
      { // For each line do...
        if (rowBytes > 250)
          linelen = ReadMWord(pDataSrc);
         else
          linelen = ReadByte(pDataSrc);

        pSrcLine = pDataSrc->ReadNBytes(linelen);
        pDestPixel = pLineBuf;

        // Unpack RLE. The data is packed bytewise.
        for (j = 0; j < linelen; )
        {
          FlagCounter = pSrcLine[j];
          if (FlagCounter & 0x80)
          {
            if (FlagCounter == 0x80)
              // Special case: repeat value of 0.
              // Apple sais ignore.
              j++;
            else
            { // Packed data.
              len = ((FlagCounter ^ 255) & 255) + 2;

              if (DestBPP == 32)
              {
                RGBAPIXEL PixValue = (*ppPal)[*(pSrcLine+j+1)];
                for (k = 0; k < len; k++)
                { // Repeat the pixel len times.
                  *(RGBAPIXEL *)(pDestPixel+k*4) = PixValue;
                }
                pDestPixel += len*4;
              }
              else
              {
                BYTE PixValue = *(pSrcLine+j+1);
                for (k = 0; k < len; k++)
                { // Repeat the pixel len times.
                  *(pDestPixel+k) = PixValue;
                }
                pDestPixel += len;
              }
              j += 2;
            }
          }
          else
          { // Unpacked data
            len = (FlagCounter & 255) + 1;
            if (DestBPP == 32)
            {
              for (k=0; k<len; k++)
              {
                *((RGBAPIXEL *)pDestPixel) =
                           (*ppPal)[*(pSrcLine+j+k+1)];
                pDestPixel += 4;
              }
            }
            else
            {
              for (k=0; k<len; k++)
              {
                *(pDestPixel) = *(pSrcLine+j+k+1);
                pDestPixel ++;
              }
            }
            j += len + 1;
          }
        }
        if (DestBPP == 32)
        {
          pDestPixel = pLineArray[i];
          memcpy (pDestPixel, pLineBuf, 4*Width);
        }
        else
        {
          pDestPixel = pLineArray[i];
          memcpy (pDestPixel, pLineBuf, Width);
        }
      }
    }
  }
  catch (CTextException)
  {
    delete pLineBuf;
    throw;
  }
  delete pLineBuf;
}

void CPictDecoder::unpackbits
    ( MacRect* pBounds,
      WORD rowBytes,
      int pixelSize,         // Source bits per pixel.
      CBmp * pBmp,
      CDataSource * pDataSrc,
      RGBAPIXEL ** ppPal,
      int DestBPP
    )
    // Decompression routine for everything but 8 & 32 bpp. This
    // routine is slower than the two routines above since it has to
    // deal with a lot of special cases :-(.
    // It's also a bit chaotic because of these special cases...
    // unpack8bits is basically a dumber version of unpackbits.
{
  BYTE * pSrcLine;           // Pointer to source line in file.
  int    i,j,k;
  WORD   pixwidth;           // bytes per row when uncompressed.
  int    linelen;            // length of source line in bytes.
  int    pkpixsize;
  BYTE * pDestLine;
  BYTE   FlagCounter;
  int    len;
  int    PixelPerRLEUnit;
  BYTE * pLineBuf;
  BYTE * pBuf;
  BYTE ** pLineArray = pBmp->GetLineArray();

  int Height = pBounds->bottom - pBounds->top;
  int Width = pBounds->right - pBounds->left;

  // High bit of rowBytes is flag.
  if (pixelSize <= 8)
    rowBytes &= 0x7fff;

  pixwidth = Width;
  pkpixsize = 1;          // RLE unit: one byte for everything...
  if (pixelSize == 16)    // ...except 16 bpp.
  {
    pkpixsize = 2;
    pixwidth *= 2;
  }

  if (rowBytes == 0)
    rowBytes = pixwidth;

  try
  {
    // I allocate the temporary line buffer here. I allocate too
    // much memory to compensate for sloppy (& hence fast)
    // decompression.
    switch (pixelSize)
    {
      case 1:
        PixelPerRLEUnit = 8;
        pLineBuf = new BYTE [(rowBytes+1) * 32];
        break;
      case 2:
        PixelPerRLEUnit = 4;
        pLineBuf = new BYTE [(rowBytes+1) * 16];
        break;
      case 4:
        PixelPerRLEUnit = 2;
        pLineBuf = new BYTE [(rowBytes+1) * 8];
        break;
      case 8:
        PixelPerRLEUnit = 1;
        pLineBuf = new BYTE [rowBytes * 4];
        break;
      case 16:
        PixelPerRLEUnit = 1;
        pLineBuf = new BYTE [rowBytes * 2 + 4];
        break;
      default:
        char sz[256];
        sprintf (sz,
                 "Illegal bpp value in unpackbits: %d\n",
                 pixelSize);
        raiseError (ERR_FORMAT_UNKNOWN, sz);
    }

    if (rowBytes < 8)
    { // ah-ha!  The bits aren't actually packed.  This will be easy.
      for (i = 0; i < Height; i++)
      {
        pDestLine = pLineArray[i];
        pSrcLine = pDataSrc->ReadNBytes (rowBytes);
        if (DestBPP == 32)
          expandBuf(pDestLine, pSrcLine, Width, pixelSize, *ppPal);
        else
          expandBuf8(pDestLine, pSrcLine, Width, pixelSize);
      }
    }
    else
    {
      for (i = 0; i < Height; i++)
      { // For each line do...
        if (rowBytes > 250)
          linelen = ReadMWord(pDataSrc);
         else
          linelen = ReadByte(pDataSrc);

        pSrcLine = pDataSrc->ReadNBytes(linelen);
        pBuf = pLineBuf;

        // Unpack RLE. The data is packed bytewise - except for
        // 16 bpp data, which is packed per pixel :-(.
        for (j = 0; j < linelen; )
        {
          FlagCounter = pSrcLine[j];
          if (FlagCounter & 0x80)
          {
            if (FlagCounter == 0x80)
              // Special case: repeat value of 0.
              // Apple sais ignore.
              j++;
            else
            { // Packed data.
              len = ((FlagCounter ^ 255) & 255) + 2;

              // This is slow for some formats...
              if (DestBPP == 32)
              {
                expandBuf (pBuf, pSrcLine+j+1, 1, pixelSize, *ppPal);
                for (k = 1; k < len; k++)
                { // Repeat the pixel (for 16 bpp) or
                  // byte (for everything else) len times.
                  memcpy (pBuf+(k*4*PixelPerRLEUnit), pBuf,
                          4*PixelPerRLEUnit);
                }
                pBuf += len*4*PixelPerRLEUnit;
              }
              else
              {
                expandBuf8 (pBuf, pSrcLine+j+1, 1, pixelSize);
                for (k = 1; k < len; k++)
                { // Repeat the expanded byte len times.
                  memcpy (pBuf+(k*PixelPerRLEUnit), pBuf,
                          PixelPerRLEUnit);
                }
                pBuf += len*PixelPerRLEUnit;
              }
              j += 1 + pkpixsize;
            }
          }
          else
          { // Unpacked data
            len = (FlagCounter & 255) + 1;
            if (DestBPP == 32)
            {
              expandBuf (pBuf, pSrcLine+j+1, len, pixelSize, *ppPal);
              pBuf += len*4*PixelPerRLEUnit;
            }
            else
            {
              expandBuf8 (pBuf, pSrcLine+j+1, len, pixelSize);
              pBuf += len*PixelPerRLEUnit;
            }
            j += len * pkpixsize + 1;
          }
        }
        pDestLine = pLineArray[i];
        if (DestBPP == 32)
          memcpy (pDestLine, pLineBuf, 4*Width);
        else
          memcpy (pDestLine, pLineBuf, Width);
      }
    }
  }
  catch (CTextException)
  {
    delete pLineBuf;
    throw;
  }

  delete pLineBuf;
}

void CPictDecoder::skipBits
    ( MacRect* pBounds,
      WORD rowBytes,
      int pixelSize,         // Source bits per pixel.
      CDataSource * pDataSrc
    )
    // skips unneeded packbits.
{
  int    i;
  WORD   pixwidth;           // bytes per row when uncompressed.
  int    linelen;            // length of source line in bytes.

  int Height = pBounds->bottom - pBounds->top;
  int Width = pBounds->right - pBounds->left;

  // High bit of rowBytes is flag.
  if (pixelSize <= 8)
    rowBytes &= 0x7fff;

  pixwidth = Width;

  if (pixelSize == 16)
    pixwidth *= 2;

  if (rowBytes == 0)
    rowBytes = pixwidth;

  if (rowBytes < 8)
  {
    pDataSrc->Skip (rowBytes*Height);
  }
  else
  {
    for (i = 0; i < Height; i++)
    {
      if (rowBytes > 250)
        linelen = ReadMWord(pDataSrc);
       else
        linelen = ReadByte(pDataSrc);
      pDataSrc->Skip (linelen);
    }
  }
}


void CPictDecoder::expandBuf
    ( BYTE * pDestBuf,
      BYTE * pSrcBuf,
      int Width,       // Width in bytes for 8 bpp or less.
                       // Width in pixels for 16 bpp.
      int bpp,         // bits per pixel
      RGBAPIXEL * pPal
    )
    // Expands Width units to 32-bit pixel data.
{
  BYTE * pSrc;
  BYTE * pDest;
  int i;

  pSrc = pSrcBuf;
  pDest = pDestBuf;

  switch (bpp)
  {
    case 16:
      for (i=0; i<Width; i++)
      {
        WORD Src = pSrcBuf[1]+(pSrcBuf[0]<<8);
        *(pDestBuf+RGBA_BLUE) = ((Src) & 31)*8;          // Blue
        *(pDestBuf+RGBA_GREEN) = ((Src >> 5) & 31)*8;    // Green
        *(pDestBuf+RGBA_RED) = ((Src  >> 10) & 31)*8;    // Red
        *(pDestBuf+RGBA_ALPHA) = 0xFF;                   // Alpha
        pSrcBuf += 2;
        pDestBuf += 4;
      }
      break;
    case 8:
      Expand8bpp (pDestBuf, pSrcBuf, Width, pPal);
      break;
    case 4:
      Expand4bpp (pDestBuf, pSrcBuf, Width*2, pPal);
      break;
    case 2:
      Expand2bpp (pDestBuf, pSrcBuf, Width*4, pPal);
      break;
    case 1:
      Expand1bpp (pDestBuf, pSrcBuf, Width*8, pPal);
      break;
    default:
      raiseError (ERR_FORMAT_UNKNOWN,
                  "Bad bits per pixel in expandBuf.");
  }
  return;
}


void CPictDecoder::expandBuf8
    ( BYTE * pDestBuf,
      BYTE * pSrcBuf,
      int Width,       // Width in bytes.
      int bpp          // bits per pixel.
    )
    // Expands Width units to 8-bit pixel data.
    // Max. 8 bpp source format.
{
  BYTE * pSrc;
  BYTE * pDest;
  int i;

  pSrc = pSrcBuf;
  pDest = pDestBuf;

  switch (bpp)
  {
    case 8:
      memcpy (pDestBuf, pSrcBuf, Width);
      break;
    case 4:
      for (i=0; i<Width; i++)
      {
        *pDest = 17*((*pSrc >> 4) & 15);
        *(pDest+1) = 17*(*pSrc & 15);
        pSrc++;
        pDest += 2;
      }
      if (Width & 1) // Odd Width?
      {
        *pDest = 17*((*pSrc >> 4) & 15);
        pDest++;
      }
      break;
    case 2:
      for (i=0; i<Width; i++)
      {
        *pDest = 85*((*pSrc >> 6) & 3);
        *(pDest+1) = 85*((*pSrc >> 4) & 3);
        *(pDest+2) = 85*((*pSrc >> 2) & 3);
        *(pDest+3) = 85*((*pSrc & 3));
        pSrc++;
        pDest += 4;
      }
      if (Width & 3)  // Check for leftover pixels
        for (i=6; i>8-(Width & 3)*2; i-=2)
        {
          *pDest = 85*((*pSrc >> i) & 3);
          pDest++;
        }
      break;
    case 1:
      for (i=0; i<Width; i++)
      {
        *pDest = 255*((*pSrc >> 7) & 1);
        *(pDest+1) = 255*((*pSrc >> 6) & 1);
        *(pDest+2) = 255*((*pSrc >> 5) & 1);
        *(pDest+3) = 255*((*pSrc >> 4) & 1);
        *(pDest+4) = 255*((*pSrc >> 3) & 1);
        *(pDest+5) = 255*((*pSrc >> 2) & 1);
        *(pDest+6) = 255*((*pSrc >> 1) & 1);
        *(pDest+7) = 255*(*pSrc  & 1);
        pSrc++;
        pDest += 8;
      }
      if (Width & 7)  // Check for leftover pixels
        for (i=7; i>(8-Width & 7); i--)
        {
          *pDest = 255*((*pSrc >> i) & 1);
          pDest++;
        }
      break;
    default:
      raiseError (ERR_FORMAT_UNKNOWN,
                  "Bad bits per pixel in expandBuf8.");
  }
  return;
}


/////////////////////////////////////////////////////////////////////
// Auxillary functions

void CPictDecoder::readPixmap
    ( MacpixMap * pPixMap,
      CDataSource * pDataSrc
    )
{
  pPixMap->version = ReadMWord(pDataSrc);
  pPixMap->packType = ReadMWord(pDataSrc);
  pPixMap->packSize = ReadMLong(pDataSrc);
  pPixMap->hRes = ReadMLong(pDataSrc);
  pPixMap->vRes = ReadMLong(pDataSrc);
  pPixMap->pixelType = ReadMWord(pDataSrc);
  pPixMap->pixelSize = ReadMWord(pDataSrc);
  pPixMap->cmpCount = ReadMWord(pDataSrc);
  pPixMap->cmpSize = ReadMWord(pDataSrc);
  pPixMap->planeBytes = ReadMLong(pDataSrc);
  pPixMap->pmTable = ReadMLong(pDataSrc);
  pPixMap->pmReserved = ReadMLong(pDataSrc);

  tracePixMapHeader (2, pPixMap);
}

RGBAPIXEL * CPictDecoder::readColourTable
    ( WORD * pNumColors,
      CDataSource * pDataSrc
    )
    // Reads a mac colour table into a bitmap palette.
{
  LONG        ctSeed;
  WORD        ctFlags;
  WORD        val;
  int         i;
  RGBAPIXEL * pColTable;

  Trace (3, "Getting color table info.\n");

  ctSeed = ReadMLong(pDataSrc);
  ctFlags = ReadMWord(pDataSrc);
  *pNumColors = ReadMWord(pDataSrc)+1;

  char sz[256];
  sprintf (sz, "Palette Size:  %d\n", *pNumColors);
  Trace (2, sz);
  Trace (3, "Reading Palette.\n");

  pColTable = new RGBAPIXEL[256];

  if (!pColTable)
    raiseError (ERR_NO_MEMORY, "Out of memory allocationg color table.");

  for (i = 0; i < *pNumColors; i++)
  {
    val = ReadMWord(pDataSrc);
    if (ctFlags & 0x8000)
      // The indicies in a device colour table are bogus and
      // usually == 0, so I assume we allocate up the list of
      // colours in order.
      val = i;
    if (val >= *pNumColors)
    {
      delete pColTable;
      raiseError (ERR_FORMAT_UNKNOWN,
                  "pixel value greater than colour table size.");
    }
    // Mac colour tables contain 16-bit values for R, G, and B...
    *(((BYTE*)(pColTable+val))+RGBA_RED) = HIBYTE (ReadMWord(pDataSrc));
    *(((BYTE*)(pColTable+val))+RGBA_GREEN) = HIBYTE (ReadMWord(pDataSrc));
    *(((BYTE*)(pColTable+val))+RGBA_BLUE) = HIBYTE (ReadMWord(pDataSrc));
    *(((BYTE*)(pColTable+val))+RGBA_ALPHA) = 0xFF;
  }

  return pColTable;
}

void CPictDecoder::readRect
    ( MacRect * pr,
      CDataSource * pDataSrc
    )
{
  pr->top = ReadMWord(pDataSrc);
  pr->left = ReadMWord(pDataSrc);
  pr->bottom = ReadMWord(pDataSrc);
  pr->right = ReadMWord(pDataSrc);
}


void CPictDecoder::dumpRect
    ( char * psz,
      MacRect * pr
    )
{
  char sz[256];
  sprintf (sz, "%s (%d,%d) (%d,%d).\n",
           psz, pr->left, pr->top, pr->right, pr->bottom);
  Trace (2, sz);
}


void CPictDecoder::tracePixMapHeader
    ( int Level,
      MacpixMap * pPixMap
    )
{
  char sz[256];
  Trace (Level, "PixMap header info:\n");
  dumpRect ("  Bounds:", &(pPixMap->Bounds));

  sprintf (sz, "  version: 0x%x\n", pPixMap->version);
  Trace (Level, sz);
  sprintf (sz, "  packType: %d\n", pPixMap->packType);
  Trace (Level, sz);
  sprintf (sz, "  packSize: %ld\n", pPixMap->packSize);
  Trace (Level, sz);
  sprintf (sz, "  pixelSize: %d\n", pPixMap->pixelSize);
  Trace (Level, sz);
  sprintf (sz, "  cmpCount: %d\n", pPixMap->cmpCount);
  Trace (Level, sz);
  sprintf (sz, "  cmpSize: %d.\n", pPixMap->cmpSize);
  Trace (Level, sz);
  sprintf (sz, "  planeBytes: %ld.\n", pPixMap->planeBytes);
  Trace (Level, sz);
}

/*
/--------------------------------------------------------------------
|
|      $Log: pictdec.cpp,v $
|      Revision 1.6  2000/03/15 17:23:20  Ulrich von Zadow
|      Fixed bug decoding pixmaps with < 8 bpp.
|
|      Revision 1.5  2000/01/16 20:43:14  anonymous
|      Removed MFC dependencies
|
|      Revision 1.4  1999/11/22 15:00:27  Ulrich von Zadow
|      Fixed bug decoding small 24 bpp pict files.
|
|      Revision 1.3  1999/10/03 18:50:51  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
\--------------------------------------------------------------------
*/
