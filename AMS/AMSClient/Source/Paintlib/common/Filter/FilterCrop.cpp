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
#include "FilterCrop.h"
#include "bitmap.h"
#include "anybmp.h"


CFilterCrop::CFilterCrop(int XMin, int XMax, int YMin, int YMax)
{
  m_XMin = XMin;
  m_XMax = XMax;
  m_YMin = YMin;
  m_YMax = YMax;
}

CFilterCrop::~CFilterCrop()
{

}

void CFilterCrop::Apply(CBmp * pBmpSource, CBmp * pBmpDest) 
{
  PLASSERT (m_XMin >= 0);
  PLASSERT (m_XMax <= pBmpSource->GetWidth());
  PLASSERT (m_YMin >= 0);
  PLASSERT (m_YMax <= pBmpSource->GetHeight());
  PLASSERT (m_XMin < m_XMax);
  PLASSERT (m_YMin < m_YMax);
  // Only 8 and 32 bpp supported for now.
  PLASSERT (pBmpSource->GetBitsPerPixel() != 1);

  pBmpDest->Create (m_XMax-m_XMin, m_YMax-m_YMin, 
                    pBmpSource->GetBitsPerPixel(), pBmpSource->HasAlpha());
  BYTE ** pSrcLineArray = pBmpSource->GetLineArray();
  BYTE ** pDstLineArray = pBmpDest->GetLineArray();

  int y;
  for (y = m_YMin; y < m_YMax; y++)
  {
    BYTE * pSrcLine = pSrcLineArray[y];
    BYTE * pDstLine = pDstLineArray[y-m_YMin];
    if (pBmpSource->GetBitsPerPixel() == 8)
      memcpy (pDstLine, pSrcLine + m_XMin, m_XMax-m_XMin);
    else
      memcpy (pDstLine, pSrcLine + m_XMin*4, (m_XMax-m_XMin)*4);
  }
}

/*
/--------------------------------------------------------------------
|
|      $Log: FilterCrop.cpp,v $
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
