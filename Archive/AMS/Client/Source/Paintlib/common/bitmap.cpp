/*
/--------------------------------------------------------------------
|
|      $Id: bitmap.cpp,v 1.17 2000/03/31 12:20:05 Ulrich von Zadow Exp $
|      Device independent bitmap class
|
|      Manipulates uncompressed device independent bitmaps
|      of all color depths.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "bitmap.h"
#include "except.h"
#include "FilterResizeBilinear.h"
#include "FilterResizeBox.h"
#include "FilterResizeGaussian.h"
#include "FilterResizeHamming.h"
#include "FilterCrop.h"
#include "FilterGrayscale.h"
#include "FilterGetAlpha.h"
#include "FilterRotate.h"
#include "Dither8bit.h"
#include "VideoInvertFilter.h"

CBmp::CBmp ()
// Creates an empty bitmap.
// Derived classes have to create a small bitmap here so the
// class can assume that a valid bitmap is available all the
// time.
{
	// Michael Salzlechner - March 02 2000
	//
	// defaults for 8Bit color quantization
	m_DitherPaletteType = IDX_DEFAULT;
	m_DitherType = IDX_NONE;
}


CBmp::~CBmp ()
{}


void CBmp::CreateCopy (const CBmp & rSrcBmp, int BPPWanted)
// Creates a copy of rSrcBmp, converting color depth if nessesary.
// Supports 1, 8 and 32 BPP. Alpha channel information is preserved.
{
  PLASSERT_VALID (this);
  PLASSERT_VALID (&rSrcBmp);

  PLASSERT (BPPWanted == 32 || BPPWanted == 8 ||
          BPPWanted == 1 || BPPWanted == 0);
  int BPPSrc = rSrcBmp.GetBitsPerPixel();
  PLASSERT (BPPSrc == 32 || BPPSrc == 8 || BPPSrc == 1);

  if (BPPWanted == BPPSrc || BPPWanted == 0)
  {
    if (&rSrcBmp != this)
    {
      // Create empty bitmap.
      Create (rSrcBmp.GetWidth(), rSrcBmp.GetHeight(),
              BPPSrc, rSrcBmp.HasAlpha());
      BYTE ** pSrcLines = rSrcBmp.GetLineArray();
      BYTE ** pDstLines = GetLineArray();
      // Minimum possible line length.
      int LineLen = GetBytesPerLine();

      for (int y = 0; y<GetHeight(); y++)
        memcpy (pDstLines[y], pSrcLines[y], LineLen);

      if (GetBitsPerPixel() <= 8)
        SetPalette (rSrcBmp.GetPalette());

      PLASSERT_VALID (this);
    }
  }
  else
  {
    // Can't copy to self while changing bit depth.
    PLASSERT (&rSrcBmp != this);

    BOOL bDestAlpha = rSrcBmp.HasAlpha() && BPPWanted != 1;
    Create (rSrcBmp.GetWidth(), rSrcBmp.GetHeight(),
            BPPWanted, bDestAlpha);

    switch (BPPWanted)
    {
      case 32:
        create32BPPCopy (rSrcBmp);
        break;

      case 8:
        create8BPPCopy (rSrcBmp);
        break;

      case 1:
        create1BPPCopy (rSrcBmp);
        break;
      default:
        PLASSERT(FALSE);
    }
    PLASSERT_VALID (this);
  }
}

void CBmp::CreateGrayscaleCopy (CBmp & rSrcBmp)
{
  PLASSERT_VALID (this);
  PLASSERT_VALID (&rSrcBmp);
  CFilterGrayscale Filter;
  Filter.Apply (&rSrcBmp, this);
}

void CBmp::CreateRotatedCopy (CBmp & rSrcBmp, double angle, RGBAPIXEL color)
{
  PLASSERT_VALID (this);
  PLASSERT_VALID (&rSrcBmp);
  CFilterRotate Filter (angle, color);
  Filter.Apply (&rSrcBmp, this);
}


CPLPoint CBmp::CreateRotatedCopyPoint (CBmp & rSrcBmp, double angle, RGBAPIXEL color, 
                                       CPLPoint Point, CPLPoint Pos)
{
 // Point -> Punkt um den gedreht werden soll
 // Pos   -> Aktuelle linke obere Ecke des Source Bildes

 double DAbsolutX,DAbsolutY,DAbsDestMiddleX,DAbsDestMiddleY;

 CreateRotatedCopy(rSrcBmp, angle, color);

 // Mittelpunkte des Source Bildes
 CPLPoint SrcMiddle  = CPLPoint (Pos.x + rSrcBmp.GetWidth() / 2, Pos.y + rSrcBmp.GetHeight() / 2);

 // Absolute Werte zwischen Drehpunkt und Mittelpunkt des Source Bildes
 CPLPoint Absolut = CPLPoint (Point.x - SrcMiddle.x, Point.y - SrcMiddle.y);
 
   // Neuen Punkt Absolut berechnen
 DAbsolutX = (double)Absolut.x ;
 DAbsolutY = (double)Absolut.y ;
 DAbsDestMiddleX = DAbsolutX * cos(angle * -1) - DAbsolutY * sin(angle * -1);
 DAbsDestMiddleY = DAbsolutX * sin(angle * -1) + DAbsolutY * cos(angle * -1);

    CPLPoint AbsDestMiddle =  CPLPoint ( (int)DAbsDestMiddleX , (int)DAbsDestMiddleY );
 AbsDestMiddle.x *= -1;
 AbsDestMiddle.y *= -1;

   // Neuen Punkt berechnen
 CPLPoint DestMiddle = CPLPoint (Point.x - AbsDestMiddle.x, Point.y - AbsDestMiddle.y);

  // Rückgabe in Rotated
 return(CPLPoint (DestMiddle.x - GetWidth() / 2, DestMiddle.y - GetHeight() / 2));
};

void CBmp::CreateFromAlphaChannel (CBmp & rSrcBmp)
{
  PLASSERT_VALID (this);
  PLASSERT_VALID (&rSrcBmp);

  CFilterGetAlpha Filter;
  Filter.Apply (&rSrcBmp, this);
}


void CBmp::CreateResizedBilinear (CBmp& rSrcBmp, int NewXSize, int NewYSize)
{
  PLASSERT_VALID (this);
  PLASSERT_VALID (&rSrcBmp);
  CFilterResizeBilinear Filter (NewXSize, NewYSize);
  Filter.Apply (&rSrcBmp, this);
}

void CBmp::CreateResizedBox (CBmp& rSrcBmp, int NewXSize, int NewYSize, double NewRadius)
{
  PLASSERT_VALID (this);
  PLASSERT_VALID (&rSrcBmp);
  CFilterResizeBox Filter (NewXSize, NewYSize, NewRadius);
  Filter.Apply (&rSrcBmp, this);
}

void CBmp::CreateResizedGaussian (CBmp& rSrcBmp, int NewXSize, int NewYSize, double NewRadius)
{
  PLASSERT_VALID (this);
  PLASSERT_VALID (&rSrcBmp);
  CFilterResizeGaussian Filter (NewXSize, NewYSize, NewRadius);
  Filter.Apply (&rSrcBmp, this);
}

void CBmp::CreateResizedHamming (CBmp& rSrcBmp, int NewXSize, int NewYSize, double NewRadius)
{
  PLASSERT_VALID (this);
  PLASSERT_VALID (&rSrcBmp);
  CFilterResizeHamming Filter (NewXSize, NewYSize, NewRadius);
  Filter.Apply (&rSrcBmp, this);
}

void CBmp::CreateCropped (CBmp& rSrcBmp, int XMin, int XMax, int YMin, int YMax)
{
  PLASSERT_VALID (this);
  PLASSERT_VALID (&rSrcBmp);
  CFilterCrop Filter (XMin, XMax, YMin, YMax);
  Filter.Apply (&rSrcBmp, this);
}

void CBmp::SetQuantizationMode (int DitherType, int DitherPaletteType)
{
  PLASSERT_VALID (this);
  m_DitherType = DitherType;
  m_DitherPaletteType = DitherPaletteType;
}

#ifdef _DEBUG
void CBmp::AssertValid () const
{
  PLASSERT (m_pBits);

  PLASSERT (m_Height >= 0);
  PLASSERT (m_Width >= 0);

  // Color table only if 8 bpp.
  PLASSERT ((m_bpp > 8) == (m_pClrTab == NULL));

  // Alpha channel only if 32 bpp
  PLASSERT ((m_bpp == 32) || !m_bAlphaChannel);
}
#endif


void CBmp::Create (LONG Width, LONG Height, WORD BitsPerPixel, BOOL bAlphaChannel)
// Create a new empty bitmap. Bits are uninitialized.
{
  PLASSERT_VALID (this);

  freeMembers ();
  internalCreate (Width, Height, BitsPerPixel, bAlphaChannel);

  PLASSERT_VALID (this);
}


/////////////////////////////////////////////////////////////////////
// CBmp manipulation

void CBmp::SetGrayPalette ()
// Fills the color table with a grayscale palette.
{
  PLASSERT (m_pClrTab); // Bitmap must contain a palette!

  int i;
  int NumColors = GetNumColors();
  double ColFactor = 255/(NumColors-1);

  for (i=0; i<NumColors; i++)
    SetPaletteEntry (i, int(i*ColFactor), int(i*ColFactor), int(i*ColFactor), 0xFF);
}

void CBmp::SetPalette (RGBAPIXEL * pPal)
{
  PLASSERT (m_pClrTab); // Bitmap must contain a palette!

  // bdelmee code change
  memcpy (m_pClrTab, pPal, GetNumColors() * sizeof(RGBAPIXEL));
}


void CBmp::SetAlphaChannel (CBmp * pAlphaBmp)
// Replaces the alpha channel with a new one.
{
  BYTE * pLine;
  BYTE * pAlphaLine;
  int x,y;
  BYTE ** pLineArray;
  BYTE ** pAlphaLineArray;

  PLASSERT_VALID (this);
  // Current bitmap must be 32 bpp.
  PLASSERT (GetBitsPerPixel() == 32);

  PLASSERT_VALID (pAlphaBmp);
  // Alpha channel must be 8 bpp.
  PLASSERT (pAlphaBmp->GetBitsPerPixel() == 8);

  // The two bitmaps must have the same dimensions
  PLASSERT (pAlphaBmp->GetWidth() == GetWidth());
  PLASSERT (pAlphaBmp->GetHeight() == GetHeight());

  pLineArray = GetLineArray();
  pAlphaLineArray = pAlphaBmp->GetLineArray();

  for (y=0; y < GetHeight(); y++)
  {
    pLine = pLineArray[y];
    pAlphaLine = pAlphaLineArray[y];
    for (x=0; x < GetWidth(); x++)
    {
      pLine[x*4+RGBA_ALPHA] = pAlphaLine[x];
    }
  }

  m_bAlphaChannel = TRUE;

  PLASSERT_VALID (this);
}

void CBmp::ResizeBilinear (int NewXSize, int NewYSize)
{
  PLASSERT_VALID (this);
  CFilterResizeBilinear Filter (NewXSize, NewYSize);
  Filter.ApplyInPlace (this);
}

void CBmp::ResizeBox (int NewXSize, int NewYSize, double NewRadius)
{
  PLASSERT_VALID (this);
  CFilterResizeBox Filter (NewXSize, NewYSize, NewRadius);
  Filter.ApplyInPlace (this);
}

void CBmp::ResizeGaussian (int NewXSize, int NewYSize, double NewRadius)
{
  PLASSERT_VALID (this);
  CFilterResizeGaussian Filter (NewXSize, NewYSize, NewRadius);
  Filter.ApplyInPlace (this);
}

void CBmp::ResizeHamming (int NewXSize, int NewYSize, double NewRadius)
{
  PLASSERT_VALID (this);
  CFilterResizeHamming Filter (NewXSize, NewYSize, NewRadius);
  Filter.ApplyInPlace (this);
}

void CBmp::Crop (int XMin, int XMax, int YMin, int YMax)
{
  PLASSERT_VALID (this);
  CFilterCrop Filter (XMin, XMax, YMin, YMax);
  Filter.ApplyInPlace (this);
}

void CBmp::MakeGrayscale ()
{
  PLASSERT_VALID (this);
  CFilterGrayscale Filter;
  Filter.ApplyInPlace (this);
}

void CBmp::Rotate (double angle, RGBAPIXEL color)
{
  PLASSERT_VALID (this);
  CFilterRotate Filter (angle, color);
  Filter.ApplyInPlace (this);
}

void CBmp::Invert ()
{
  PLASSERT_VALID (this);
  CVideoInvertFilter Filter;
  Filter.ApplyInPlace (this);
}

RGBAPIXEL CBmp::GetPixel (int x, int y)
{
  RGBAPIXEL dwResult;
  BYTE ** pLines = GetLineArray();
  BYTE * pPixel = pLines[y];
  RGBAPIXEL * p32Pixel = NULL;

  switch (GetBitsPerPixel())
  {
    case 32:
      p32Pixel = (RGBAPIXEL*)(pPixel+x*4);
      dwResult = *p32Pixel;
      break;

    case 8:
      dwResult = pPixel[x];
      break;

    case 1:
      dwResult = pPixel[x / 8] & (128 >> (x & 7)) ? 1 : 0;
      break;

    default:
      PLASSERT(FALSE);
  }

  return dwResult;
}

void CBmp::SetPixel (int x, int y, RGBAPIXEL pixel)
{
  BYTE ** pLines = GetLineArray();
  BYTE * pPixel = pLines[y];
  RGBAPIXEL *p32Pixel = NULL;

  switch (GetBitsPerPixel())
  {
    case 32:
      p32Pixel = (RGBAPIXEL*)(pPixel+x*4);
      *p32Pixel = pixel;
      break;

    case 8:
      pPixel[x] = (BYTE)pixel;
      break;

    case 1:
      if(pixel&1)
        pPixel[x/8]|= 1<<(x%8);
      else
        pPixel[x/8]&=~(1<<x%8);
      break;

    default:
      PLASSERT(FALSE);
  }
}

int CBmp::FindNearestColor (RGBAPIXEL cr)
{
  int crR,crG,crB;
  crR = GetRValue(cr);
  crG = GetGValue(cr);
  crB = GetBValue(cr);

  int nBPP = GetBitsPerPixel();
  if (nBPP == 32)
    return cr;

  int d1, d2, dMin = 1000;
  RGBAPIXEL * pPalette = GetPalette();
  if (pPalette == NULL)
  {
    //1 bit bitmap, does not use palette
    d1 = abs(255 - crR)  + abs(255 - crG) + abs(255 - crB);
    d2 = crR + crG + crB;
    if (d1 < d2)
      return 1;    //white
    else
      return 0;    //black
  }

  //use palette;

  int i,index,nSize = 1<<nBPP;
  for (i = 0; i<nSize; i++)
  {
    d1 = abs(crR - GetRValue(pPalette[i]))
         + abs(crG - GetGValue(pPalette[i]))
         + abs(crB - GetBValue(pPalette[i]));
    if (d1 < dMin)
    {
      dMin = d1;
      index = i;
    }
  }
  return index;
}

/////////////////////////////////////////////////////////////////////
// Local functions


void CBmp::initLocals (LONG Width, LONG Height, WORD BitsPerPixel, BOOL bAlphaChannel)
{
  m_Width = Width;
  m_Height = Height;
  m_bpp = BitsPerPixel;
  m_bAlphaChannel = bAlphaChannel;

  // Initialize pointers to lines.
  initLineArray ();

  if (BitsPerPixel < 16)
  { // Color table exists
    SetGrayPalette ();
  }

  PLASSERT_VALID (this);
}

void CBmp::create32BPPCopy (const CBmp & rSrcBmp)
{
  int BPPSrc = rSrcBmp.GetBitsPerPixel();
  BYTE ** pSrcLines = rSrcBmp.GetLineArray();
  BYTE ** pDstLines = GetLineArray();
  int SrcLineLen = GetWidth();

  if (BPPSrc == 8) // Conversion 8->32 BPP
  {
    RGBAPIXEL *pPal = rSrcBmp.GetPalette();

    for (int y = 0; y<GetHeight(); ++y)
    { // For each line
      BYTE * pSrcPixel = pSrcLines[y];
      BYTE * pDstPixel = pDstLines[y];

      for (int x = 0; x < SrcLineLen; ++x)
      { // For each pixel
        *((RGBAPIXEL *)pDstPixel) = pPal[*pSrcPixel];
        ++pSrcPixel;
        pDstPixel += sizeof(RGBAPIXEL);
      }
    }
  }
  else // 1 -> 32
  {
    RGBAPIXEL *pPal = rSrcBmp.GetPalette();
    RGBAPIXEL blackDot, whiteDot;
    // if bi-tonal "palette" exists, use it...
    if (pPal)
    {
      whiteDot = pPal[0];
      blackDot = pPal[1];
    }
    else
    {
      SetRGBAPixel(&whiteDot,255,255,255,255);
      SetRGBAPixel(&blackDot,  0,  0,  0,255);
    }

    // assume msb is leftmost
    for (int y = 0; y<GetHeight(); ++y)
    { // For each line
      BYTE * pSrcPixel = pSrcLines[y];
      BYTE * pDstPixel = pDstLines[y];

      for (int x = 0; x < SrcLineLen; ++x)
      { // For each pixel
        if (pSrcPixel[x / 8] & (128 >> (x & 7)))  // black pixel
          *((RGBAPIXEL *)pDstPixel) = blackDot;
        else
          *((RGBAPIXEL *)pDstPixel) = whiteDot;
        pDstPixel += sizeof(RGBAPIXEL);
      }
    }
  }
}

void CBmp::create8BPPCopy (const CBmp & rSrcBmp)
{
  int BPPSrc = rSrcBmp.GetBitsPerPixel();
  BYTE ** pSrcLines = rSrcBmp.GetLineArray();
  BYTE ** pDstLines = GetLineArray();
  int SrcLineLen = GetWidth();

  if (BPPSrc == 32) // Conversion 32->8 BPP
  {
    // quantize (rSrcBmp);
	//
	// Michael Salzlechner - added quantization code for 8bpp copies
	//						 uses m_DitherPaletteType and ,m_DitherType to determine the
	//						  type of quantization
	//
	//	IDX_MEDIAN          0       // Median Cut
	//  IDX_POPULARITY      1       // Popularity Sort
	//  IDX_DEFAULT         2       // Use Default Palette
	//  IDX_NEUQUANTBEST	3		// Neural Quantization Best/Slowest Quality
	//  IDX_NEUQUANTMIDDLE	4		// Neural Quantization Middle/average Speed Quality
	//  IDX_NEUQUANTWORST	5		// Neural Quantization Worst/Fastest Quality
	//
	//  IDX_NONE            0       // None
	//  IDX_ORDERED         1       // Ordered Dithering
	//  IDX_JITTER          2       // Jitter preprocessing
	//  IDX_FS				3		// Floyd-Steinberg Dithering

	UCHAR palette[768];

	//BYTE* p8BitBits = DitherPicture8bit(rSrcBmp.m_pBits, IDX_DEFAULT, IDX_NONE, m_Width , m_Height, palette);
	BYTE* p8BitBits = DitherPicture8bit(rSrcBmp.m_pBits, m_DitherPaletteType, m_DitherType, m_Width , m_Height, palette);

	memcpy(m_pBits,p8BitBits,WIDTHBYTES(m_Width<<3)*m_Height);

	BYTE* pPalColor = palette;

	for (UINT i = 0; i<256; i++)
	{
		SetPaletteEntry(i, pPalColor[0],pPalColor[1],pPalColor[2], 255);
		pPalColor += 3;
	}

	delete [] p8BitBits;

  }
  else // 1 -> 8
  {
	RGBAPIXEL *pPal = rSrcBmp.GetPalette();
    // if bi-tonal "palette" exists, use it...
    if (pPal)
    {
      BYTE *pWhite = (BYTE *) pPal;
      BYTE *pBlack = (BYTE *) (pPal+1);
      SetPaletteEntry(0,
                      pWhite[RGBA_RED],pWhite[RGBA_GREEN],pWhite[RGBA_BLUE],
                      255);
      SetPaletteEntry(1,
                      pBlack[RGBA_RED],pBlack[RGBA_GREEN],pBlack[RGBA_BLUE],
                      255);
    }
    else
    {
      SetPaletteEntry(0,255,255,255,255);
      SetPaletteEntry(1,0,0,0,255);
    }

    // assume msb is leftmost
    for (int y = 0; y<GetHeight(); ++y)
    { // For each line
      BYTE * pSrcPixel = pSrcLines[y];
      BYTE * pDstPixel = pDstLines[y];

      for (int x = 0; x < SrcLineLen; ++x)  // For each pixel
        pDstPixel[x] = pSrcPixel[x / 8] & (128 >> (x & 7)) ? 1 : 0;
    }
  }
}

void CBmp::create1BPPCopy (const CBmp & rSrcBmp)
{
  int BPPSrc = rSrcBmp.GetBitsPerPixel();
  BYTE ** pSrcLines = rSrcBmp.GetLineArray();
  BYTE ** pDstLines = GetLineArray();
  int SrcLineLen = GetWidth();

  SetPaletteEntry(0,255,255,255,255);
  SetPaletteEntry(1,0,0,0,255);

  // downgrade to monochrome
  RGBAPIXEL *pPal = rSrcBmp.GetPalette();
  BYTE *pRGBA;
  int DstLineLen = GetBytesPerLine();

  for (int y = 0; y < GetHeight(); ++y)
  { // For each line
    BYTE * pSrcPixel = pSrcLines[y];
    BYTE * pDstPixel = pDstLines[y];
    // fill with background (index 0) color
    memset(pDstPixel,0,DstLineLen);

    for (int x = 0; x < SrcLineLen; ++x)  // For each pixel
    {
      pRGBA = BPPSrc == 8 ? (BYTE*) &pPal[*pSrcPixel] : pSrcPixel;
      // the following criterion supposedly finds "dark" pixels; it may
      // need some twiddling and maybe use the alpha channel as well
      if (pRGBA[RGBA_RED] < 128 &&
          pRGBA[RGBA_GREEN] < 128 &&
          pRGBA[RGBA_BLUE] < 128 )
        pDstPixel[x / 8] |= 128 >> (x & 7);
      pSrcPixel += BPPSrc == 8 ? 1 : sizeof(RGBAPIXEL);
    }
  }
}

/*
/--------------------------------------------------------------------
|
|      $Log: bitmap.cpp,v $
|      Revision 1.17  2000/03/31 12:20:05  Ulrich von Zadow
|      Video invert filter (beta)
|
|      Revision 1.16  2000/03/31 11:53:29  Ulrich von Zadow
|      Added quantization support.
|
|      Revision 1.15  2000/01/16 20:43:12  anonymous
|      Removed MFC dependencies
|
|      Revision 1.14  2000/01/10 23:52:59  Ulrich von Zadow
|      Changed formatting & removed tabs.
|
|      Revision 1.13  1999/12/31 17:59:54  Ulrich von Zadow
|      Corrected error in CBmp::SetPixel for 1 bpp.
|
|      Revision 1.12  1999/12/30 15:54:47  Ulrich von Zadow
|      Added CWinBmp::FromClipBoard() and CreateFromHBitmap().
|
|      Revision 1.11  1999/12/10 01:27:26  Ulrich von Zadow
|      Added assignment operator and copy constructor to
|      bitmap classes.
|
|      Revision 1.10  1999/12/08 15:39:45  Ulrich von Zadow
|      Unix compatibility changes
|
|      Revision 1.9  1999/12/02 17:07:34  Ulrich von Zadow
|      Changes by bdelmee.
|
|      Revision 1.8  1999/11/08 22:10:53  Ulrich von Zadow
|      no message
|
|      Revision 1.7  1999/10/22 21:25:51  Ulrich von Zadow
|      Removed buggy octree quantization
|
|      Revision 1.6  1999/10/21 18:47:26  Ulrich von Zadow
|      Added Rotate, GetPixel, SetPixel and FindNearestColor
|
|      Revision 1.5  1999/10/21 16:05:43  Ulrich von Zadow
|      Moved filters to separate directory. Added Crop, Grayscale and
|      GetAlpha filters.
|
|      Revision 1.4  1999/10/19 21:29:44  Ulrich von Zadow
|      Added filters.
|
|      Revision 1.3  1999/10/03 18:50:51  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
--------------------------------------------------------------------
*/
