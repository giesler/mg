/*
/--------------------------------------------------------------------
|
|      $Id: FilterRotate.cpp,v 1.7 2000/01/16 20:43:16 anonymous Exp $
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include <math.h>

#include "bitmap.h"
#include "FilterRotate.h"
#include "plpoint.h"


//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CFilterRotate::CFilterRotate(double angle, RGBAPIXEL crDefault)
    : m_angle(angle),
    m_crDefault(crDefault)
{}


CFilterRotate::~CFilterRotate()
{}


void CFilterRotate::Apply(CBmp * pBmpSource, CBmp * pBmpDest)
{
  // Negative the angle, because the y-axis is negative.
  double angle = -m_angle;
  int newWidth, newHeight;
  int nWidth = pBmpSource->GetWidth();
  int nHeight= pBmpSource->GetHeight();
  double cos_angle = cos(angle);
  double sin_angle = sin(angle);

  // Calculate the size of the new bitmap
  CPLPoint p1(0,0),
  p2(nWidth-1,0),
  p3(0,nHeight-1),
  p4(nWidth-1,nHeight-1);
  CPLPoint newP1,newP2,newP3,newP4, leftTop, rightTop, leftBottom, rightBottom;

  newP1 = p1;
  newP2.x = (long)(p2.x*cos_angle - p2.y*sin_angle);
  newP2.y = (long)(p2.x*sin_angle + p2.y*cos_angle);
  newP3.x = (long)(p3.x*cos_angle - p3.y*sin_angle);
  newP3.y = (long)(p3.x*sin_angle + p3.y*cos_angle);
  newP4.x = (long)(p4.x*cos_angle - p4.y*sin_angle);
  newP4.y = (long)(p4.x*sin_angle + p4.y*cos_angle);

  leftTop.x = min(min(newP1.x,newP2.x),min(newP3.x,newP4.x));
  leftTop.y = min(min(newP1.y,newP2.y),min(newP3.y,newP4.y));
  rightBottom.x = max(max(newP1.x,newP2.x),max(newP3.x,newP4.x));
  rightBottom.y = max(max(newP1.y,newP2.y),max(newP3.y,newP4.y));
  leftBottom.x = leftTop.x;
  leftBottom.y = rightBottom.y;
  rightTop.x = rightBottom.x;
  rightTop.y = leftTop.y;

  newWidth = rightTop.x - leftTop.x + 1;
  newHeight= leftBottom.y - leftTop.y + 1;

  //ROTATE the bitmap
  pBmpDest->Create(newWidth, newHeight, pBmpSource->GetBitsPerPixel(), pBmpSource->HasAlpha());

  int x,y,newX,newY,oldX,oldY;
  RGBAPIXEL pix,crIndex = pBmpSource->FindNearestColor (m_crDefault);
  for (y = leftTop.y, newY = 0; y<=leftBottom.y; y++,newY++)
    for (x = leftTop.x, newX = 0; x<=rightTop.x; x++,newX++)
    {
      oldX = (long)(x*cos_angle + y*sin_angle);
      oldY = (long)(y*cos_angle - x*sin_angle);
      if ((oldX<0) || (oldX>=nWidth) ||
          (oldY<0) || (oldY>=nHeight))
        pix = crIndex;
      else
        pix = pBmpSource->GetPixel(oldX,oldY);
      pBmpDest->SetPixel(newX,newY,pix);
    }

  RGBAPIXEL * pPalette = pBmpSource->GetPalette();
  if ((pBmpSource->GetBitsPerPixel()<16) && (pPalette))
    pBmpDest->SetPalette(pPalette);
}

/*
/--------------------------------------------------------------------
|
|      $Log: FilterRotate.cpp,v $
|      Revision 1.7  2000/01/16 20:43:16  anonymous
|      Removed MFC dependencies
|
|      Revision 1.6  2000/01/10 23:53:00  Ulrich von Zadow
|      Changed formatting & removed tabs.
|
|      Revision 1.5  1999/12/09 16:35:58  Ulrich von Zadow
|      Added CPLPoint.
|
|      Revision 1.4  1999/12/08 16:31:40  Ulrich von Zadow
|      Unix compatibility
|
|      Revision 1.3  1999/12/08 15:58:02  Ulrich von Zadow
|      Unix compatibility changes
|
|      Revision 1.2  1999/12/08 15:39:46  Ulrich von Zadow
|      Unix compatibility changes
|
|      Revision 1.1  1999/10/21 18:47:43  Ulrich von Zadow
|      no message
|
|
\--------------------------------------------------------------------
*/
