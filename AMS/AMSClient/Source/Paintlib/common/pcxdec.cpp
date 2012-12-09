/*
/--------------------------------------------------------------------
|
|      $Id: pcxdec.cpp,v 1.6 2000/03/30 21:23:43 Ulrich von Zadow Exp $
|      PCX Decoder Class
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "pcxdec.h"
#include "except.h"


CPCXDecoder::CPCXDecoder()
// Creates a decoder
{}


CPCXDecoder::~CPCXDecoder()
{}


void CPCXDecoder::DoDecode (CBmp * pBmp, RGBAPIXEL ** ppPal, int * pDestBPP, CDataSource * pDataSrc)
{
  LPPCXHEADER pcxHeader;
  int i, x, y;
  BYTE ColorMap[PCX_MAXCOLORS][3];
  LPBYTE pcximage = NULL, lpHead1 = NULL, lpHead2 = NULL;
  LPBYTE pcxplanes, pcxpixels;
  BYTE c;
  int nbytes, count;

  int Height, Width;

  // Check if the file is a valid PCX file or not
  Trace (2, "Decoding PCX.\n");

  try
  {
    pcxHeader = (LPPCXHEADER)pDataSrc->ReadNBytes(sizeof(tagPCXHEADER));

    if (pcxHeader->Manufacturer != PCX_MAGIC)
    {
      raiseError (ERR_WRONG_SIGNATURE, "Error decoding pcx: Not a PCX file.");
    }
    // Check for PCX run length encoding
    if (pcxHeader->Encoding != 1)
    {
      raiseError (ERR_WRONG_SIGNATURE, "File has unknown encoding scheme.");
    }

    Width = (pcxHeader->Xmax - pcxHeader->Xmin) + 1;
    Height = (pcxHeader->Ymax - pcxHeader->Ymin) + 1;
    pBmp->Create (Width, Height, 32, FALSE);

    // Check that we can handle this image format
    switch (pcxHeader->BitsPerPixel)
    {
      case 1:
        if (pcxHeader->ColorPlanes > 4)
        {
          raiseError(ERR_FORMAT_UNKNOWN, "Can't handle image with more than 4 planes.");
        }
        break;
      case 2:
      case 4:
      case 8:
        if (pcxHeader->ColorPlanes == 1 || pcxHeader->ColorPlanes == 3)
          break;
      default:
        raiseError(ERR_FORMAT_UNKNOWN, "Can't handle bits per pixel image with planes.");
        break;
    }

    CalcDestBPP (pcxHeader->BitsPerPixel, pDestBPP);
    nbytes = pcxHeader->BytesPerLine * pcxHeader->ColorPlanes * Height;
    lpHead1 = pcximage = (LPBYTE)malloc(nbytes);
    while (nbytes > 0)
    {
      c = ReadByte(pDataSrc);
      if ((c & 0XC0) != 0XC0) // Repeated group
      {
        *pcximage++ = c;
        --nbytes;
        continue;
      }
      count = c & 0X3F; // extract count
      c = ReadByte(pDataSrc);
      if (count > nbytes)
      {
        raiseError(ERR_INTERNAL, "repeat count spans end of image.");
      }
      nbytes -= count;
      while (--count >=0)
        *pcximage++ = c;
    }
    pcximage = lpHead1;

    for (i = 0; i < 16; i++)
    {
      ColorMap[i][0] = pcxHeader->ColorMap[i][0];
      ColorMap[i][1] = pcxHeader->ColorMap[i][1];
      ColorMap[i][2] = pcxHeader->ColorMap[i][2];
    }
    if (pcxHeader->BitsPerPixel == 8 && pcxHeader->ColorPlanes == 1)
    {
      if (ReadByte(pDataSrc) != PCX_256_COLORS)
      {
        raiseError(ERR_INTERNAL, "bad color map signature.");
      }
      for (i = 0; i < PCX_MAXCOLORS; i++)
      {
        ColorMap[i][0] = ReadByte(pDataSrc);
        ColorMap[i][1] = ReadByte(pDataSrc);
        ColorMap[i][2] = ReadByte(pDataSrc);
      }
    }
    if (pcxHeader->BitsPerPixel == 1 && pcxHeader->ColorPlanes == 1)
    {
      ColorMap[0][0] = ColorMap[0][1] = ColorMap[0][2] = 0;
      ColorMap[1][0] = ColorMap[1][1] = ColorMap[1][2] = 255;
    }

    lpHead2 = pcxpixels = (BYTE *)malloc(Width + pcxHeader->BytesPerLine * 8);
    // Convert the image
    for (y = 0; y < Height; y++)
    {
      pcxpixels = lpHead2;
      pcxplanes = pcximage + (y * pcxHeader->BytesPerLine * pcxHeader->ColorPlanes);
      if (pcxHeader->ColorPlanes == 3 && pcxHeader->BitsPerPixel == 8)
      {
        // Deal with 24 bit color image
        for (x = 0; x < Width; x++)
        {
          BYTE ** pLineArray = pBmp->GetLineArray();
          RGBAPIXEL * pPixel = (RGBAPIXEL *)(pLineArray[y]);
          SetRGBAPixel (pPixel + x, pcxplanes[x],
            pcxplanes[Width + x],
            pcxplanes[Width + Width + x],
            0xFF);
        }
        continue;
      }
      else if (pcxHeader->ColorPlanes == 1)
      {
        PCX_UnpackPixels(pcxpixels, pcxplanes, pcxHeader->BytesPerLine, pcxHeader->ColorPlanes, pcxHeader->BitsPerPixel);
      }
      else
      {
        PCX_PlanesToPixels(pcxpixels, pcxplanes, pcxHeader->BytesPerLine, pcxHeader->ColorPlanes, pcxHeader->BitsPerPixel);
      }
      for (x = 0; x < Width; x++)
      {
        i = pcxpixels[x];
        BYTE ** pLineArray = pBmp->GetLineArray();
        RGBAPIXEL * pPixel = (RGBAPIXEL *)(pLineArray[y]);
        SetRGBAPixel (pPixel + x, ColorMap[i][0], ColorMap[i][1],
          ColorMap[i][2], 0xFF);
      }
    }
  }

  catch (CTextException)
  {
    if (lpHead1)
    {
      free(lpHead1);
      lpHead1 = NULL;
    }
    if (lpHead2)
    {
      free(lpHead2);
      lpHead2 = NULL;
    }
    throw;
  }

  if (lpHead1)
  {
    free(lpHead1);
    lpHead1 = NULL;
  }
  if (lpHead2)
  {
    free(lpHead2);
    lpHead2 = NULL;
  }
}

// Convert multi-plane format into 1 pixel per byte
// from unpacked file data bitplanes[] into pixel row pixels[]
// image Height rows, with each row having planes image planes each
// bytesperline bytes
void CPCXDecoder::PCX_PlanesToPixels(BYTE * pixels, BYTE * bitplanes,
                                     short bytesperline, short planes, short bitsperpixel)
{
  int i, j;
  int npixels;
  BYTE * p;
  if (planes > 4)
  {
    raiseError(ERR_INTERNAL, "Can't handle more than 4 planes.");
  }
  if (bitsperpixel != 1)
  {
    raiseError(ERR_INTERNAL, "Can't handle more than 1 bit per pixel.");
  }

  // Clear the pixel buffer
  npixels = ((bytesperline-1) * 8) / bitsperpixel;
  p = pixels;
  while (--npixels >= 0)
    *p++ = 0;

  // Do the format conversion
  for (i = 0; i < planes; i++)
  {
    int pixbit, bits, mask;
    p = pixels;
    pixbit = (1 << i);  // pixel bit for this plane
    for (j = 0; j < bytesperline; j++)
    {
      bits = *bitplanes++;
      for (mask = 0X80; mask != 0; mask >>= 1, p++)
        if (bits & mask)
          *p |= pixbit;
    }
  }
}

// convert packed pixel format into 1 pixel per byte
// from unpacked file data bitplanes[] into pixel row pixels[]
// image Height rows, with each row having planes image planes each
// bytesperline bytes
void CPCXDecoder::PCX_UnpackPixels(BYTE * pixels, BYTE * bitplanes,
                                   short bytesperline, short planes, short bitsperpixel)
{
  register int bits;
  if (planes != 1)
  {
    raiseError(ERR_INTERNAL, "Can't handle packed pixels with more than 1 plane.");
  }
  if (bitsperpixel == 8)  // 8 bits/pixels, no unpacking needed
  {
    while (--bytesperline >= 0)
      *pixels++ = *bitplanes++;
  }
  else if (bitsperpixel == 4)  // 4 bits/pixel, two pixels per byte
  {
    while (--bytesperline > 0)
    {
      bits = *bitplanes++;
      *pixels++ = (BYTE)((bits >> 4) & 0X0F);
      *pixels++ = (BYTE)((bits) & 0X0F);
    }
  }
  else if (bitsperpixel == 2)  // 2 bits/pixel, four pixels per byte
  {
    while (--bytesperline > 0)
    {
      bits = *bitplanes++;
      *pixels++ = (BYTE)((bits >> 6) & 0X03);
      *pixels++ = (BYTE)((bits >> 4) & 0X03);
      *pixels++ = (BYTE)((bits >> 2) & 0X03);
      *pixels++ = (BYTE)((bits) & 0X03);
    }
  }
  else if (bitsperpixel == 1)  // 1 bits/pixel, 8 pixels per byte
  {
    while (--bytesperline > 0)
    {
      bits = *bitplanes++;
      *pixels++ = ((bits & 0X80) != 0);
      *pixels++ = ((bits & 0X40) != 0);
      *pixels++ = ((bits & 0X20) != 0);
      *pixels++ = ((bits & 0X10) != 0);
      *pixels++ = ((bits & 0X08) != 0);
      *pixels++ = ((bits & 0X04) != 0);
      *pixels++ = ((bits & 0X02) != 0);
      *pixels++ = ((bits & 0X01) != 0);
    }
  }
}

/*
/--------------------------------------------------------------------
|
|      $Log: pcxdec.cpp,v $
|      Revision 1.6  2000/03/30 21:23:43  Ulrich von Zadow
|      no message
|
|      Revision 1.5  2000/03/17 10:51:12  Ulrich von Zadow
|      Bugfix for b/w images.
|
|      Revision 1.4  2000/01/16 20:43:13  anonymous
|      Removed MFC dependencies
|
|
\--------------------------------------------------------------------
*/
