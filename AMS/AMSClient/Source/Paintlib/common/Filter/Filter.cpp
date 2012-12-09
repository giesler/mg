/*
/--------------------------------------------------------------------
|
|      $Id: Filter.cpp,v 1.7 2000/01/16 20:43:15 anonymous Exp $
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "Filter.h"
#include "bitmap.h"
#include "anybmp.h"


CFilter::CFilter()
{

}

CFilter::~CFilter()
{

}

void CFilter::ApplyInPlace(CBmp * pBmp) 
{
  // Use a bitmap class that's guaranteed to work on all platforms as
  // temporary storage.
  CAnyBmp TempBmp;  
  Apply (pBmp, &TempBmp);
  *pBmp = TempBmp;
}

void CFilter::Apply(CBmp * pBmpSource, CBmp * pBmpDest)
{
  *pBmpDest = *pBmpSource;
  ApplyInPlace (pBmpDest);
}

/*
/--------------------------------------------------------------------
|
|      $Log: Filter.cpp,v $
|      Revision 1.7  2000/01/16 20:43:15  anonymous
|      Removed MFC dependencies
|
|      Revision 1.6  1999/12/14 12:29:47  Ulrich von Zadow
|      no message
|
|      Revision 1.5  1999/12/10 01:27:27  Ulrich von Zadow
|      Added assignment operator and copy constructor to
|      bitmap classes.
|
|      Revision 1.4  1999/12/08 16:31:40  Ulrich von Zadow
|      Unix compatibility
|
|      Revision 1.3  1999/11/27 18:45:48  Ulrich von Zadow
|      Added/Updated doc comments.
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
