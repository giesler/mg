/*
/--------------------------------------------------------------------
|
|      $Id: winbmp.cpp,v 1.6 2000/01/17 23:37:12 Ulrich von Zadow Exp $
|      Device independent bitmap class
|
|        Windows version
|
|        Manipulates uncompressed device independent bitmaps
|        of all color depths.
|
|        Header and bits are in one buffer.
|        The bottom line is stored first. Color tables for 16-,
|        24-, and 32- bpp-bitmaps are not supported. biClrUsed is
|        always 0.
|
|        A CWinBmp can contain alpha channel information. As in TGA
|        files, 0 is completely transparent and 255 completely opaque.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "winbmp.h"
#include "except.h"

#include <stdio.h>


CWinBmp::CWinBmp ()
// Creates an empty bitmap.
{
  internalCreate(16, 16, 8, FALSE);

  PLASSERT_VALID(this);
}


CWinBmp::~CWinBmp ()
{
  // Free the memory.
  freeMembers();
}


#ifdef _DEBUG
void CWinBmp::AssertValid () const
{
  // call inherited PLASSERT_VALID first
  CBmp::AssertValid();

  // Bitmap must exist
  PLASSERT (m_pBMI);

  // Bitmapinfo must equal member variables
  PLASSERT (m_pBMI->biHeight == m_Height);
  PLASSERT (m_pBMI->biWidth == m_Width);
  PLASSERT (m_pBMI->biBitCount == m_bpp);

  // Only uncompressed bitmaps allowed.
  PLASSERT (m_pBMI->biCompression == BI_RGB);

  // No optimized color tables allowed.
  PLASSERT (m_pBMI->biClrUsed == 0 ||
          m_pBMI->biClrUsed == (DWORD)(1 << m_bpp));
}
#endif


/////////////////////////////////////////////////////////////////////
// CWinBmp manipulation

void CWinBmp::AlphaBlt (CWinBmp * pSrcBmp, int x, int y)
// Do a bitblt using the alpha channel of pSrcBmp.
// Legacy routine. Should not be used.
{
  PLASSERT_VALID (this);
  PLASSERT (GetBitsPerPixel() == 32);

  PLASSERT_VALID (pSrcBmp);

  // Overlay picture
  int DestLineLen = GetWidth()*4;
  int SrcLineLen = pSrcBmp->GetBytesPerLine();
  RGBAPIXEL * pPal = pSrcBmp->GetPalette();

  // Perform clipping.
  int maxy = min (pSrcBmp->GetHeight(),
                  GetHeight()-y);
  int maxx = min (pSrcBmp->GetWidth(),
                  GetWidth()-x);
  int miny = max (0,-y);
  int minx = max (0,-x);

  if (pSrcBmp->m_bAlphaChannel)
  {
    int alpha, negalpha;

    for (int sy = miny; sy<maxy; sy++)
    { // For each line
      BYTE * pDest = m_pBits+DestLineLen*(y+sy)+x*4;
      BYTE * pSrc = pSrcBmp->m_pBits+SrcLineLen*sy;

      for (int sx = minx; sx<maxx; sx++)
      { // For each pixel
        if (pPal)
        {
          RGBAPIXEL * pPixel = &(pPal[*pSrc]);
          alpha = pPixel[3];
          negalpha = 255-alpha;
          pDest[0] = (pDest[0]*negalpha+pPixel[0]*alpha)>>8;
          pDest[1] = (pDest[1]*negalpha+pPixel[1]*alpha)>>8;
          pDest[2] = (pDest[2]*negalpha+pPixel[2]*alpha)>>8;

          pSrc++;
        }
        else
        {
          alpha = pSrc[3];
          negalpha = 255-alpha;
          pDest[0] = (pDest[0]*negalpha+pSrc[0]*alpha)>>8;
          pDest[1] = (pDest[1]*negalpha+pSrc[1]*alpha)>>8;
          pDest[2] = (pDest[2]*negalpha+pSrc[2]*alpha)>>8;
          pSrc += 4;
        }

        pDest += 4;
      }
    }
  }
  else
  {
    for (int sy = miny; sy<maxy; sy++)
    { // For each line
      if (pPal)
      {
        BYTE * pDest = m_pBits+DestLineLen*(y+sy)+x*4;
        BYTE * pSrc = pSrcBmp->m_pBits+SrcLineLen*sy;

        for (int sx = minx; sx<maxx; sx++)
        { // For each pixel
          *((RGBAPIXEL *)pDest) = pPal[*pSrc];

          pDest += 4;
          pSrc++;
        }
      }
      else
      {
        BYTE * pDest = m_pBits+DestLineLen*(y+sy)+x*4;
        BYTE * pSrc = pSrcBmp->m_pBits+SrcLineLen*sy;

        memcpy (pDest, pSrc, 4*(maxx-minx));
      }
    }
  }
  PLASSERT_VALID (this);
}

/////////////////////////////////////////////////////////////////////
// CWinBmp information

long CWinBmp::GetMemUsed ()
// Returns the memory used by the object.
{
  PLASSERT_VALID (this);

  return GetMemNeeded (GetWidth(), GetHeight(), m_pBMI->biBitCount)+
         sizeof (*this);
}


long CWinBmp::GetBytesPerLine ()
// Returns number of bytes used per line.
{
  // bdelmee code change
  int nBytes = m_Width*m_bpp / 8;
  if (m_bpp == 1 && m_Width % 8)
    ++nBytes;
  // adjust to nearest DWORD-multiple
  return (nBytes + 3) & ~3;
}


/////////////////////////////////////////////////////////////////////
// Windows-specific interface


void CWinBmp::CreateRes ( HINSTANCE lh_ResInst, int ID)
    // Loads a DIB from a resource. Fails if the bitmap is compressed.
{
  HRSRC  hRsrc;
  HGLOBAL hGlobal;
  BITMAPINFOHEADER * pBMI;

  PLASSERT_VALID (this);

  hRsrc = FindResource(lh_ResInst,
                       MAKEINTRESOURCE (ID),
                       RT_BITMAP);

  PLASSERT (hRsrc);  // Make sure resource exists.

  hGlobal = LoadResource(lh_ResInst, hRsrc);
  pBMI = (BITMAPINFOHEADER *) LockResource(hGlobal);

  // Delete any existing stuff.
  freeMembers ();

  // Copy data into local memory & init locals.
  internalCreate (pBMI);

  PLASSERT_VALID (this);
}


void CWinBmp::CreateFromHBitmap(HBITMAP hBitMap)
{
  HDC hdc = ::GetDC (NULL);
  BITMAPINFO * pBMI;
  pBMI = (BITMAPINFO *)malloc (sizeof (BITMAPINFOHEADER)+256*4);
  BITMAPINFOHEADER * pBIH = (BITMAPINFOHEADER *)pBMI;
  pBIH->biSize = sizeof (BITMAPINFOHEADER);
  pBIH->biBitCount = 0;
  pBIH->biPlanes = 1;
  pBIH->biSizeImage = 0;
  pBIH->biXPelsPerMeter = 0;
  pBIH->biYPelsPerMeter = 0;
  pBIH->biClrUsed = 0;           // Always use the whole palette.
  pBIH->biClrImportant = 0;

  // Get bitmap format.
  int rc = ::GetDIBits(hdc, hBitMap, 0, 0, NULL, pBMI, DIB_RGB_COLORS);
  PLASSERT (rc);

  // Convert to format paintlib can use.
  if (pBIH->biBitCount == 16 || pBIH->biBitCount == 24)
    pBIH->biBitCount = 32;
  pBIH->biCompression = BI_RGB;   // No compression

  freeMembers ();

	int MemNeeded = GetMemNeeded (pBIH->biWidth, pBIH->biHeight,
								  pBIH->biBitCount);
	m_pBMI = (BITMAPINFOHEADER *) malloc (MemNeeded);

	// out of memory?
	if (!m_pBMI)
    throw (CTextException (ERR_NO_MEMORY, "Can't create bitmap."));

  memcpy (m_pBMI, pBIH, sizeof (BITMAPINFOHEADER));

  // Set color table pointer & pointer to bits.
  initPointers ();

  initLocals (m_pBMI->biWidth, m_pBMI->biHeight,
              m_pBMI->biBitCount, FALSE);

  rc = ::GetDIBits (hdc, hBitMap, 0, GetHeight(), GetBits(),
                    (BITMAPINFO *)GetBMI(), DIB_RGB_COLORS);
  PLASSERT (rc);
  free (pBMI);
  ::ReleaseDC(NULL, hdc);
  PLASSERT_VALID (this);
}


SIZE CWinBmp::GetSize ()
    // Returns size in pixels
{
  SIZE sz;

  PLASSERT_VALID (this);

  sz.cx = GetWidth();
  sz.cy = GetHeight();

  return sz;
}


BITMAPINFOHEADER * CWinBmp::GetBMI ()
{
  PLASSERT_VALID (this);

  return m_pBMI;
}

void CWinBmp::SaveAsBmp
    ( const char * pszFName
    )
{
  // 11.01.2000 - Modified by Michael Salzlechner
  //			  Logic moved back to support non MFC builds
  //			  A CAFXBmp could support the CFile style

  int BPP = GetBitsPerPixel();

  FILE* pFile = NULL;
  BITMAPFILEHEADER BFH;
  BFH.bfType = *((WORD*)"BM");
  BFH.bfReserved1 = 0;
  BFH.bfReserved2 = 0;
  BFH.bfOffBits = sizeof (BITMAPFILEHEADER) +
                  sizeof (BITMAPINFOHEADER);
  if (BPP <= 8)   // include palette
    BFH.bfOffBits += (1 << BPP) * sizeof(RGBQUAD);

  BFH.bfSize = BFH.bfOffBits;
  if (BPP <= 8)
    BFH.bfSize += GetBytesPerLine()*m_Height;
  else
	  BFH.bfSize += (((m_Width * 3) + 3) & ~3)*m_Height;

  if (pFile = fopen (pszFName, "wb"))
  {
    fwrite( &BFH, 1, sizeof (BITMAPFILEHEADER), pFile );
    if (BPP <= 8)
    {
      fwrite( m_pBMI, 1, sizeof (BITMAPINFOHEADER), pFile );
 	    fwrite( m_pClrTab, 1, (1 << BPP) * sizeof(RGBQUAD), pFile );
 	    fwrite( m_pBits, 1, GetBytesPerLine()*m_Height, pFile );
    }
    else
    {
      BITMAPINFOHEADER FileBMI = *m_pBMI;
      FileBMI.biBitCount = 24;  // not 32...

      fwrite( &FileBMI, 1, sizeof (BITMAPINFOHEADER), pFile );

      BYTE * pCurLine;
      int x,y;
      int LinePadding = 4-((m_Width*3)&3);
      if (LinePadding == 4)
        LinePadding = 0;
      for (y=m_Height-1; y>=0; y--)
      {
        pCurLine = m_pLineArray[y];
        for (x=0; x<m_Width; x++)
        {
  	      fwrite( pCurLine+x*4, 1, 3, pFile );
        }
        fwrite( "    ", 1, LinePadding, pFile );
      }
	  }
    fclose (pFile);
  }
  else
   throw (CTextException(ERR_ACCESS_DENIED, "SaveAsBmp:Can't open file."));
}


/////////////////////////////////////////////////////////////////////
// CWinBmp output

void CWinBmp::Draw (HDC hDC, int x, int y, DWORD rop /* = SRCCOPY */)
// Draw the DIB to a given DC.
{
  PLASSERT_VALID (this);

  ::StretchDIBits(hDC,
                  x,                        // Destination x
                  y,                        // Destination y
                  GetWidth(),               // Destination width
                  GetHeight(),              // Destination height
                  0,                        // Source x
                  0,                        // Source y
                  GetWidth(),               // Source width
                  GetHeight(),              // Source height
                  m_pBits,                  // Pointer to bits
                  (BITMAPINFO *) m_pBMI,    // BITMAPINFO
                  DIB_RGB_COLORS,           // Options
                  rop);                     // Raster operator code
}


void CWinBmp::StretchDraw (HDC hDC, int x, int y, double Factor,
                           DWORD rop /* = SRCCOPY */)
// Draw the DIB to a given DC.
{
  PLASSERT_VALID (this);

  ::StretchDIBits(hDC,
                  x,                        // Destination x
                  y,                        // Destination y
                  int (Factor*GetWidth()),  // Destination width
                  int (Factor*GetHeight()), // Destination height
                  0,                        // Source x
                  0,                        // Source y
                  GetWidth(),               // Destination width
                  GetHeight(),              // Destination height
                  m_pBits,                  // Pointer to bits
                  (BITMAPINFO *) m_pBMI,    // BITMAPINFO
                  DIB_RGB_COLORS,           // Options
                  rop);                     // Raster operator code
}


void CWinBmp::StretchDraw (HDC hDC, int x, int y, int w, int h,
                           DWORD rop /* = SRCCOPY */)
// Draw the DIB to a given DC.
{
  PLASSERT_VALID (this);

  ::StretchDIBits(hDC,
                  x,                        // Destination x
                  y,                        // Destination y
                  w,            // Destination width
                  h,            // Destination height
                  0,                        // Source x
                  0,                        // Source y
                  GetWidth(),               // Destination width
                  GetHeight(),              // Destination height
                  m_pBits,                  // Pointer to bits
                  (BITMAPINFO *) m_pBMI,    // BITMAPINFO
                  DIB_RGB_COLORS,           // Options
                  rop);                     // Raster operator code
}

BOOL CWinBmp::DrawExtract (HDC hDC, POINT pntDest, RECT rcSrc)
// Draw part of the DIB on a DC.
{
  PLASSERT_VALID (this);
  return ::SetDIBitsToDevice (hDC,
                              pntDest.x,         // Destination x
                              pntDest.y,         // Destination y
                              rcSrc.right-rcSrc.left,    // Source width
                              rcSrc.bottom-rcSrc.top,   // Source height
                              rcSrc.left,        // Source x
                              m_pBMI->biHeight - rcSrc.bottom,
                              // Source lower y
                              0,
                              m_pBMI->biHeight,
                              m_pBits,
                              (BITMAPINFO *) m_pBMI,
                              DIB_RGB_COLORS );
}


void CWinBmp::ToClipboard ()
// Puts a copy of the DIB in the clipboard
{
  PLASSERT_VALID (this);

  if (::OpenClipboard(NULL))
  {
    EmptyClipboard();
    HANDLE hDIB = createCopyHandle ();
    SetClipboardData (CF_DIB, hDIB);
    CloseClipboard();
  }
}


bool CWinBmp::FromClipboard()
{
  if (IsClipboardFormatAvailable (CF_BITMAP))
  {
    if (OpenClipboard(NULL))
    {
      HBITMAP hbm = (HBITMAP)GetClipboardData(CF_BITMAP);
      if ( hbm != NULL )
      {
        GlobalLock(hbm);
        CreateFromHBitmap(hbm);
        GlobalUnlock(hbm);
        CloseClipboard();
        return true;
      }
      else
      {
        CloseClipboard();
        return false;
      }
    }
    else
      return false;
  }
  else
    return false;
}


/////////////////////////////////////////////////////////////////////
// Static functions

long CWinBmp::GetBitsMemNeeded (LONG width, LONG height, WORD BitsPerPixel)
// Returns memory needed by bitmap bits.
{
  // Calculate memory per line.
  int LineMem = width * BitsPerPixel / 8;

  // bdelmee code change
  if (BitsPerPixel == 1 && width % 8)
    ++LineMem;

  // Multiply by number of (DWORD-aligned) lines
  return height * ((LineMem + 3) & ~3);
}


long CWinBmp::GetMemNeeded (LONG width, LONG height, WORD BitsPerPixel)
// Returns memory needed by a bitmap with the specified attributes.
{
  int HeaderMem = sizeof(BITMAPINFOHEADER); // Header memory
  if (BitsPerPixel < 16)
  { // Palette memory
    HeaderMem += (1 << BitsPerPixel)*sizeof (RGBQUAD);
  }

  return HeaderMem+GetBitsMemNeeded (width, height, BitsPerPixel);
}


/////////////////////////////////////////////////////////////////////
// Protected callbacks

void CWinBmp::internalCreate (LONG Width, LONG Height, WORD BitsPerPixel,
                              BOOL bAlphaChannel)
// Create a new empty DIB. Bits are uninitialized.
// Assumes that no memory is allocated before the call.
{
  // Allocate memory
  int MemNeeded = GetMemNeeded (Width, Height, BitsPerPixel);

#ifdef MAX_BITMAP_SIZE
  if (MemNeeded > MAX_BITMAP_SIZE)
    throw CTextException(ERR_DIB_TOO_LARGE, "Bitmap size too large.\n");
#endif

  m_pBMI = (BITMAPINFOHEADER*) malloc (MemNeeded);
  if (!m_pBMI)
    throw (CTextException (ERR_NO_MEMORY, "Can't create bitmap."));

  // Fill in the header info.
  m_pBMI->biSize = sizeof(BITMAPINFOHEADER);
  m_pBMI->biWidth = Width;
  m_pBMI->biHeight = Height;
  m_pBMI->biPlanes = 1;
  m_pBMI->biBitCount = BitsPerPixel;
  m_pBMI->biCompression = BI_RGB;   // No compression
  m_pBMI->biSizeImage = 0;
  m_pBMI->biXPelsPerMeter = 0;
  m_pBMI->biYPelsPerMeter = 0;
  m_pBMI->biClrUsed = 0;           // Always use the whole palette.
  m_pBMI->biClrImportant = 0;

  // Set color table pointer & pointer to bits.
  initPointers ();

  initLocals (Width, Height, BitsPerPixel, bAlphaChannel);

  PLASSERT_VALID (this);
}


void CWinBmp::internalCreate (BITMAPINFOHEADER* pBMI)
// Creates a CWinBmp from an existing bitmap pointer.
// Assumes that no memory is allocated before the call.
{
  int MemNeeded = GetMemNeeded (pBMI->biWidth, pBMI->biHeight,
                                pBMI->biBitCount);
#ifdef MAX_BITMAP_SIZE
  if (MemNeeded > MAX_BITMAP_SIZE)
    throw CTextException(ERR_DIB_TOO_LARGE, "Bitmap size too large.\n");
#endif

  m_pBMI = (BITMAPINFOHEADER *) malloc (MemNeeded);

	// out of memory?
	if (!m_pBMI)
    throw (CTextException (ERR_NO_MEMORY, "Can't create bitmap."));

  memcpy (m_pBMI, pBMI, MemNeeded);

  // Set color table pointer & pointer to bits.
  initPointers ();

  initLocals (m_pBMI->biWidth, m_pBMI->biHeight,
              m_pBMI->biBitCount, FALSE);

  PLASSERT_VALID (this);
}


void CWinBmp::freeMembers ()
{
  free(m_pBMI);
  m_pBMI = NULL;

  delete m_pLineArray;
  m_pLineArray = NULL;
}


void CWinBmp::initLineArray ()
{
  m_pLineArray = new BYTE * [m_Height];
  int LineLen = GetBytesPerLine();

  for (int y=0; y<m_Height; y++)
    m_pLineArray[y] = m_pBits + (m_Height-y-1)*LineLen;
}


HANDLE CWinBmp::createCopyHandle ()
// Creates a copy of the current bitmap in a global memory block
// and returns a handle to this block.
{
  HANDLE  hCopy;

  int MemUsed = GetMemUsed();

  hCopy = (HANDLE) ::GlobalAlloc (GMEM_MOVEABLE | GMEM_DDESHARE,
                                  MemUsed);
  // Michael Salzlechner - 01/11/2000
  // changed AfxThrow to just throw see ms011100.txt
  if (hCopy == NULL)
         throw (CTextException (ERR_NO_MEMORY, "Can't allocate global memory block."));

  long * lpCopy = (long *) ::GlobalLock((HGLOBAL) hCopy);
  memcpy (lpCopy, m_pBMI, MemUsed);

  ::GlobalUnlock((HGLOBAL) hCopy);

  return hCopy;
}


void CWinBmp::initPointers ()
// Set color table pointer & pointer to bits based on m_pBMI.
{
  if (m_pBMI->biBitCount < 16)
  { // Color table exists
    m_pClrTab = (RGBAPIXEL *)(((BITMAPINFO *) (m_pBMI))->bmiColors);

    m_pBits = (BYTE *)m_pClrTab +
              (1 << m_pBMI->biBitCount)*sizeof (RGBQUAD);
  }
  else
  { // No color table for 16 bpp and up.
    m_pClrTab = NULL;
    m_pBits =  (BYTE *)(((BITMAPINFO *) (m_pBMI))->bmiColors);
  }
}


BYTE * CWinBmp::GetBits ()
{
  return m_pBits;
}

/*
/--------------------------------------------------------------------
|
|      $Log: winbmp.cpp,v $
|      Revision 1.6  2000/01/17 23:37:12  Ulrich von Zadow
|      Corrected bug in assignment operator.
|
|      Revision 1.5  2000/01/16 20:43:18  anonymous
|      Removed MFC dependencies
|
|      Revision 1.4  2000/01/10 23:53:01  Ulrich von Zadow
|      Changed formatting & removed tabs.
|
|
--------------------------------------------------------------------
*/
