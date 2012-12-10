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
#include "FilterResizeHamming.h"
#include "2PassScale.h"
#include "bitmap.h"


CFilterResizeHamming::CFilterResizeHamming (int NewXSize, int NewYSize, double NewRadius)
  : CFilterResize (NewXSize, NewYSize),
    m_NewRadius(NewRadius)
{
}

void CFilterResizeHamming::Apply(CBmp * pBmpSource, CBmp * pBmpDest)
{
  PLASSERT(pBmpSource->GetBitsPerPixel()==32);

  // Create a new Bitmap 
  pBmpDest->Create(m_NewXSize,
                   m_NewYSize,
                   pBmpSource->GetBitsPerPixel(),
                   pBmpSource->HasAlpha());
                     
  // Create a Filter Class from template
  C2PassScale <CHammingFilter, CDataPaintLib32> sS(m_NewRadius);
  sS.Scale ((CDataPaintLib32::_RowType *) pBmpSource->GetLineArray(), 
             pBmpSource->GetWidth(), 
             pBmpSource->GetHeight(), 
             (CDataPaintLib32::_RowType *) pBmpDest->GetLineArray(),
             pBmpDest->GetWidth(),
             pBmpDest->GetHeight());
}

/*
/--------------------------------------------------------------------
|
|      $Log: FilterResizeHamming.cpp,v $
|      Revision 1.3  2000/01/16 20:43:16  anonymous
|      Removed MFC dependencies
|
|      Revision 1.2  1999/12/08 16:31:40  Ulrich von Zadow
|      Unix compatibility
|
|      Revision 1.1  1999/10/21 16:05:18  Ulrich von Zadow
|      Moved filters to separate directory. Added Crop, Grayscale and
|      GetAlpha filters.
|
|      Revision 1.1  1999/10/19 21:29:45  Ulrich von Zadow
|      Added filters.
|
|
\--------------------------------------------------------------------
*/
