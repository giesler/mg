/*
/--------------------------------------------------------------------
|
|      $Id: anybmp.cpp,v 1.6 2000/01/16 20:43:12 anonymous Exp $
|      Device independent bitmap class
|
|        Plattform-independent version
|
|        Manipulates uncompressed device independent bitmaps.
|
|        Supported data formats are 8 and 32 bpp. The data is stored
|        sequentially without padding in the bitmap.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "anybmp.h"
#include "except.h"


CAnyBmp::CAnyBmp
    ()
    // Creates an empty bitmap.
{
  internalCreate(16, 16, 8, FALSE);

  PLASSERT_VALID(this);
}


CAnyBmp::~CAnyBmp
    ()
{
  // Free the memory.
  freeMembers();
}


long CAnyBmp::GetMemUsed
    ()
    // Returns the memory used by the object.
{
  PLASSERT_VALID (this);

  return GetMemNeeded (GetWidth(), GetHeight(), GetBitsPerPixel())+
                                                sizeof (*this);
}


long CAnyBmp::GetBytesPerLine
    ()
    // Returns number of bytes used per line.
{
  // bdelmee code change
  int nBytes = GetWidth() * GetBitsPerPixel() / 8;
  if (GetBitsPerPixel() == 1 && GetWidth() % 8)
    ++nBytes;
  return nBytes;
}


/////////////////////////////////////////////////////////////////////
// Static functions

long CAnyBmp::GetBitsMemNeeded
    ( LONG width,
      LONG height,
      WORD BitsPerPixel
    )
    // Returns memory needed by bitmap bits.
{
  // Calculate memory per line.
  int LineMem = width*BitsPerPixel/8;

  // bdelmee code change
  if (BitsPerPixel == 1 && width % 8)
    ++LineMem;

  // Multiply by number of lines
  return LineMem*height;
}


long CAnyBmp::GetMemNeeded
    ( LONG width,
      LONG height,
      WORD BitsPerPixel
    )
    // Returns memory needed by a bitmap with the specified attributes.
{
  int HeaderMem = sizeof (CAnyBmp);
  if (BitsPerPixel < 16)
  { // Palette memory
    HeaderMem += (1 << BitsPerPixel)*sizeof (RGBAPIXEL);
  }

  return HeaderMem+GetBitsMemNeeded (width, height, BitsPerPixel);
}


/////////////////////////////////////////////////////////////////////
// Local functions


void CAnyBmp::internalCreate
    ( LONG Width,
      LONG Height,
      WORD BitsPerPixel,
      BOOL bAlphaChannel
    )
    // Create a new empty bitmap. Bits are uninitialized.
    // Assumes that no memory is allocated before the call.
{
  // Allocate memory

#ifdef MAX_BITMAP_SIZE
  int MemNeeded = GetMemNeeded (Width, Height, BitsPerPixel);

  if (MemNeeded > MAX_BITMAP_SIZE)
    throw CTextException(ERR_DIB_TOO_LARGE, "Bitmap size too large.\n");
#endif

//  m_pBits = (BYTE *) malloc (GetBitsMemNeeded (Width, Height,
//                                               BitsPerPixel));
  m_pBits = new BYTE [GetBitsMemNeeded (Width, Height, BitsPerPixel)];

  if (BitsPerPixel <= 8)
    m_pClrTab = new RGBAPIXEL [1 << BitsPerPixel];
   else
    m_pClrTab = NULL;
  initLocals (Width, Height, BitsPerPixel, bAlphaChannel);

  PLASSERT_VALID (this);
}


void CAnyBmp::initLineArray
    ()
{
//  m_pLineArray = (BYTE **) malloc (m_Height * sizeof (BYTE *));
  m_pLineArray = new BYTE * [m_Height];
  int LineLen = GetBytesPerLine();

  for (int y=0; y<m_Height; y++)
    m_pLineArray[y] = m_pBits + y*LineLen;
}

void CAnyBmp::freeMembers
    ()
{
  delete m_pBits;
  m_pBits = NULL;

  if (m_pClrTab)
  {
    delete m_pClrTab;
    m_pClrTab = NULL;
  }

  delete m_pLineArray;
  m_pLineArray = NULL;
}

/*
/--------------------------------------------------------------------
|
|      $Log: anybmp.cpp,v $
|      Revision 1.6  2000/01/16 20:43:12  anonymous
|      Removed MFC dependencies
|
|      Revision 1.5  2000/01/10 23:52:59  Ulrich von Zadow
|      Changed formatting & removed tabs.
|
|      Revision 1.4  1999/12/08 15:39:45  Ulrich von Zadow
|      Unix compatibility changes
|
|      Revision 1.3  1999/10/03 18:50:51  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
--------------------------------------------------------------------
*/
