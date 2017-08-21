/*
/--------------------------------------------------------------------
|
|      $Id: FilterGetAlpha.cpp,v 1.4 2000/01/16 20:43:15 anonymous Exp $
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "FilterGetAlpha.h"
#include "bitmap.h"
#include "anybmp.h"


CFilterGetAlpha::CFilterGetAlpha()
{
}

CFilterGetAlpha::~CFilterGetAlpha()
{

}

void CFilterGetAlpha::Apply(CBmp * pBmpSource, CBmp * pBmpDest) 
{
  // Only works for 32 bpp bitmaps.
  PLASSERT (pBmpSource->GetBitsPerPixel() == 32);

  // Actually, we could return a white bitmap if the source doesn't have
  // an alpha channel.
  PLASSERT (pBmpSource->HasAlpha());  

  pBmpDest->Create (pBmpSource->GetWidth(), pBmpSource->GetHeight(), 8, FALSE);
  BYTE ** pSrcLines = pBmpSource->GetLineArray();
  BYTE ** pDstLines = pBmpDest->GetLineArray();

  for (int y = 0; y<pBmpDest->GetHeight(); ++y)
  { // For each line
    BYTE * pSrcPixel = pSrcLines[y];
    BYTE * pDstPixel = pDstLines[y];

    for (int x = 0; x < pBmpDest->GetWidth(); ++x)
    { // For each pixel
      *pDstPixel = BYTE (pSrcPixel[RGBA_ALPHA]);
	    pSrcPixel += sizeof(RGBAPIXEL);
      ++pDstPixel;
    }
  }
}

/*
/--------------------------------------------------------------------
|
|      $Log: FilterGetAlpha.cpp,v $
|      Revision 1.4  2000/01/16 20:43:15  anonymous
|      Removed MFC dependencies
|
|      Revision 1.3  1999/12/08 16:31:40  Ulrich von Zadow
|      Unix compatibility
|
|      Revision 1.2  1999/10/21 18:48:03  Ulrich von Zadow
|      no message
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
