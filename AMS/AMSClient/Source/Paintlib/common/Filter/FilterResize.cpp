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
#include "FilterResize.h"


//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CFilterResize::CFilterResize(int NewXSize, int NewYSize) 
: m_NewXSize(NewXSize), 
  m_NewYSize(NewYSize)
{
}

CFilterResize::~CFilterResize()
{
}

void CFilterResize::SetNewSize(int NewXSize, int NewYSize)
{
  m_NewXSize = NewXSize;
  m_NewYSize = NewYSize;
}

/*
/--------------------------------------------------------------------
|
|      $Log: FilterResize.cpp,v $
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
