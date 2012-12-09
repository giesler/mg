/*
/--------------------------------------------------------------------
|
|      $id$
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "FilterGrayscale.h"
#include "bitmap.h"
#include "anybmp.h"


//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CFilterGrayscale::CFilterGrayscale()
{
}

CFilterGrayscale::~CFilterGrayscale()
{

}

void CFilterGrayscale::Apply(CBmp * pBmpSource, CBmp * pBmpDest) 
{
  // Only works for 32 bpp bitmaps at the moment.
  PLASSERT (pBmpSource->GetBitsPerPixel() == 32);

  pBmpDest->Create (pBmpSource->GetWidth(), pBmpSource->GetHeight(), 8, FALSE);
  BYTE ** pSrcLines = pBmpSource->GetLineArray();
  BYTE ** pDstLines = pBmpDest->GetLineArray();

  for (int y = 0; y<pBmpDest->GetHeight(); ++y)
  { // For each line
    BYTE * pSrcPixel = pSrcLines[y];
    BYTE * pDstPixel = pDstLines[y];

    for (int x = 0; x < pBmpDest->GetWidth(); ++x)
    { // For each pixel
      *pDstPixel = BYTE (pSrcPixel[RGBA_RED]*0.212671+
	                       pSrcPixel[RGBA_GREEN]*0.715160+
                         pSrcPixel[RGBA_BLUE]*0.072169);
	    pSrcPixel += sizeof(RGBAPIXEL);
      ++pDstPixel;
    }
  }
}

/*
/--------------------------------------------------------------------
|
|      $Log: FilterGrayscale.cpp,v $
|      Revision 1.3  2000/01/16 20:43:15  anonymous
|      Removed MFC dependencies
|
|      Revision 1.2  1999/12/08 16:31:40  Ulrich von Zadow
|      Unix compatibility
|
|      Revision 1.1  1999/10/21 16:05:17  Ulrich von Zadow
|      Moved filters to separate directory. Added Crop, Grayscale and
|      GetAlpha filters.
|
|      Revision 1.1  1999/10/19 21:29:44  Ulrich von Zadow
|      Added filters.
|
|
\--------------------------------------------------------------------
*/
