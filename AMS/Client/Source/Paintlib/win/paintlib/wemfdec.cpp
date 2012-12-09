/*
/--------------------------------------------------------------------
|
|      $Id: wemfdec.cpp,v 1.5 2000/01/16 20:43:18 anonymous Exp $
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"

#if _MSC_VER > 1
#pragma hdrstop
#endif

// This only makes sense for Windows
#ifdef _WINDOWS

#include "filesrc.h"
#include "except.h"
#include "wemfdec.h"
#include "winbmp.h"


// Some defines and types for Aldus Placeable Metafiles (APM)
// I got these from the Windows 3.1 SDK and modified them to
// match the 32-Bit environment

#define  ALDUSKEY   0x9AC6CDD7

#pragma pack(2)
struct SAPMFILEHEADER
{
  DWORD   key;
  WORD  hmf;
  short  Left;    // left, top, right, bottom in twips
  short  Top;
  short  Right;
  short  Bottom;
  WORD    inch;    // Number of twips per inch
  DWORD   reserved;
  WORD    checksum;
};
#pragma pack()


//###########################################################################
// CWEMFDecoder
//###########################################################################


/////////////////////////////////////////////////////////////////////////////
// Ctor
CWEMFDecoder::CWEMFDecoder()
    : CPicDecoder()
{}


/////////////////////////////////////////////////////////////////////////////
// Dtor

CWEMFDecoder::~CWEMFDecoder()
{}


/////////////////////////////////////////////////////////////////////////////
// Debugging support

#ifdef _DEBUG
void CWEMFDecoder::AssertValid() const
{
}
#endif


/////////////////////////////////////////////////////////////////////////////
// This is the main decoder routine
void CWEMFDecoder::DoDecode (CBmp * pBmp, RGBAPIXEL** ppPal, int* pDestBPP, CDataSource* pDataSrc)
{
	PLASSERT_VALID(this);
	HENHMETAFILE hemf = NULL;
	SAPMFILEHEADER* pplaceablehdr = NULL;
	bool isadobe = false;
	*ppPal = NULL;
	HDC dc = NULL;
	LPENHMETAHEADER phdr = NULL;
	LPLOGPALETTE plogpal = NULL;
	HPALETTE hpal = NULL;
	HPALETTE holdpal = NULL;
	HBITMAP bm = NULL;
	HDC memdc = NULL;

	try {
		// Get the type of the file (WMF or EMF) from the file name
		char* strname = strdup(pDataSrc->GetName());
		PLASSERT(strname);
		if (strname == NULL) {
			// This should never happen under 32-Bit, but who nows?
			PLASSERT(FALSE);
			raiseError (ERR_NO_MEMORY,"Out of memory during strdup.");
		}
		strupr(strname);
		bool isemf = strstr(strname,".EMF") != NULL;
		free(strname);

		// Get a DC for the display
		dc = ::GetDC(NULL);
		PLASSERT(dc);
		if (dc == NULL) {
			PLASSERT(FALSE);
			raiseError (ERR_NO_MEMORY,"Cannot allocate device context.");
		}

		if (isemf) {
			// We have an enhanced meta file which makes it alot easier
			hemf = SetEnhMetaFileBits(pDataSrc->GetFileSize(),pDataSrc->ReadEverything());
		}
		else {
			// Buh, old 16-Bit WMF, Convert it to an enhanced metafile before proceeding.
			// Also, check if this is a placeable metafile with an Adobe Placeable header
			pplaceablehdr = (SAPMFILEHEADER*)pDataSrc->ReadEverything();
			BYTE* p = NULL;
			UINT size;
			// If we have an adobe header, skip it to use only the real windows-conform data
			if (pplaceablehdr->key == ALDUSKEY) {
				isadobe = true;
				p = pDataSrc->ReadEverything()+sizeof(SAPMFILEHEADER);
				size = pDataSrc->GetFileSize() - sizeof(SAPMFILEHEADER);
			}
			else {
				// Else use the whole file contents as the metafile and assume
				// a native 16-Bit Windows-conform WMF
				p = pDataSrc->ReadEverything();
				size = pDataSrc->GetFileSize();
			}
			#ifdef _MFC_VER
			PLASSERT(AfxIsValidAddress(p,size,FALSE));
			#endif
			hemf = SetWinMetaFileBits(size,p,dc,NULL);
		}

		// If hemf is NULL, windows refused to load the metafile. If this is
		// the case, we're done. Notify the caller
		if (hemf == NULL) {
			raiseError (ERR_FORMAT_NOT_SUPPORTED,"Windows Metafile functions failed to load this image.");
		}

		// Get the header from the enhanced metafile, It contains some
		// useful information which will aid us during constuction of
		// the bitmap.
		// The header is of variable length. First get the amount of
		//  memory required for the header
		UINT sizeneeded = GetEnhMetaFileHeader(hemf,0,NULL);
		if (sizeneeded == 0) {
			raiseError (ERR_FORMAT_UNKNOWN,"No header information in metafile");
		}

		// Allocate storage for the header and read it in
		phdr = (LPENHMETAHEADER) new BYTE[sizeneeded];
		if (phdr == NULL) {
			PLASSERT(FALSE);
			raiseError (ERR_NO_MEMORY,"Out of memory during allocation of header.");
		}
		phdr->iType = EMR_HEADER;
		phdr->nSize = sizeneeded;
		#ifdef _MFC_VER
		PLASSERT(AfxIsValidAddress(phdr,sizeneeded,TRUE));
		#endif
		GetEnhMetaFileHeader(hemf,sizeneeded,phdr);

		int bpp = GetDeviceCaps(dc,BITSPIXEL);

		// Calculate the dimensions of the final bitmap. If we have
		// a placeable header in the WMF, we use the dimensions of
		// that image, else we use the calculated dimensions in the
		// EMF
		int width,height;
		if (isadobe) {
			PLASSERT(pplaceablehdr);
			int lpx = GetDeviceCaps(dc,LOGPIXELSX);
			int lpy = GetDeviceCaps(dc,LOGPIXELSY);
			// Calculate the absolute with and height and transform from twips to pixel
			width  = (int) (pplaceablehdr->Right-pplaceablehdr->Left) * lpx / pplaceablehdr->inch;
			height = (int) (pplaceablehdr->Bottom-pplaceablehdr->Top) * lpy / pplaceablehdr->inch;
		}
		else {
			// Use the rclFrame of the header because it is the true device independent
			// information and also some applications (e.g. Corel) don't fill the
			// rclBounds correctly
			// Using:
			//     MetaPixelsX = MetaWidthMM * MetaPixels / (MetaMM * 100);
			// where:
			//     MetaWidthMM = metafile width in 0.01mm units
			//     MetaPixels  = width in pixels of the reference device
			//     MetaMM      = width in millimeters of the reference device
			// Same applies to the Y axis
			width  = ((phdr->rclFrame.right  - phdr->rclFrame.left) * phdr->szlDevice.cx) / (phdr->szlMillimeters.cx*100);
			height = ((phdr->rclFrame.bottom  - phdr->rclFrame.top) * phdr->szlDevice.cy) / (phdr->szlMillimeters.cy*100);
		}

		// If this is a very old WMF without a PLACEABLE info,
		// we use somewhat meaningful defaults. Also, if the header was
		// not written correctly, we use this as a fallback
		if (width <= 0) {
			width = 320;
		}
		if (height <= 0) {
			height = 200;
		}

		// Create a device content for the screen, and a memory device
		// content to play the metafile to

		memdc = CreateCompatibleDC(dc);
		PLASSERT(memdc);
		if (memdc == NULL) {
			PLASSERT(FALSE);
			raiseError (ERR_NO_MEMORY,"Cannot allocate device context.");
		}

		bm = CreateCompatibleBitmap(dc,width,height);
		if (bm == NULL) {
			PLASSERT(FALSE);
			raiseError (ERR_NO_MEMORY,"Cannot allocate memory bitmap.");
		}

		HGDIOBJ holdbm = SelectObject(memdc,bm);

		// If the metafile has a palette, read it in
		UINT pe = GetEnhMetaFilePaletteEntries(hemf, 0, NULL);

		// pe is the real number of palette entries. To make the resulting
		// bitmap more useful, we always setup a 256 color palette if the
		// metafile has a palette
		UINT palentries = 0;
		if ((pe > 0) && (pe < 256)) palentries = 256;

		if (palentries > 0) {
			plogpal = (LPLOGPALETTE)new BYTE[sizeof(LOGPALETTE) + (sizeof(PALETTEENTRY) * palentries)];
			memset(plogpal,0x0,sizeof(LOGPALETTE) + sizeof(PALETTEENTRY)*palentries);
			plogpal->palVersion = 0x300;
			plogpal->palNumEntries = palentries;
			if (plogpal == NULL) {
				PLASSERT(FALSE);
				raiseError (ERR_NO_MEMORY,"Cannot allocate palette.");
			}
			GetEnhMetaFilePaletteEntries(hemf, pe, plogpal->palPalEntry);
		}

		// Setup a logical palette for our memory dc and also a
		// paintlib compatible palette for the paintlib bitmap
		if (plogpal) {
			*ppPal = new RGBAPIXEL[palentries];
			if (*ppPal == NULL) {
				PLASSERT(FALSE);
				raiseError (ERR_NO_MEMORY,"Cannot allocate palette info.");
			}
			for (UINT i = 0; i < palentries; i++) {
				(*ppPal)[i] = *(RGBAPIXEL*)&plogpal->palPalEntry[i];
			}

			if ((hpal = CreatePalette((LPLOGPALETTE)plogpal))) {
				holdpal = SelectPalette(memdc, hpal, FALSE);
				RealizePalette(memdc);
			}
		}

		// Play the metafile into the device context
		// First, setup a bounding rectangle and fill
		// the memory dc with white (some metafiles only
		// use a black pen to draw and have no actual fill
		// color set, This would cause a black on black
		// painting which is rather useless
		RECT rc;
		rc.left = rc.top = 0;
		rc.bottom = height;
		rc.right = width;

		FillRect(memdc,&rc,(HBRUSH)GetStockObject(WHITE_BRUSH));

		// Heeere we go....
		BOOL bres = PlayEnhMetaFile(memdc,hemf,&rc);

		DeleteEnhMetaFile(hemf);
		hemf = NULL;

		// Free all windows DC stuff
		SelectObject(memdc,holdpal);
		SelectObject(memdc,holdbm);

		// Free resources (set to null for exception safety)
		delete [] plogpal;
		plogpal = NULL;
		delete [] phdr;
		phdr = NULL;
		DeleteDC(memdc);
		memdc = NULL;

		// Finally, convert the Windows bitmap into a paintlib bitmap
		// Since metafiles are valid under Windows, we can simply convert
		// the given CBmp to a CWinBmp -- Ulrich told me this is OK ;-)

		// If we have a palette, we assume 8 BBP bitmaps, else we
		// use the good 32 BBP's

		PLASSERT((*pDestBPP == 0) || (*pDestBPP >= 24) || (*pDestBPP == 8));

		if (*ppPal) {
			if (*pDestBPP == 0) {
				*pDestBPP = 8;
			}
			pBmp->Create (width, height, *pDestBPP, *pDestBPP < 32 ? FALSE : TRUE);
			if (*pDestBPP < 24) pBmp->SetPalette(*ppPal);
		}
		else {
			if (*pDestBPP == 0) {
				*pDestBPP = 32;
			}
			pBmp->Create (width, height, *pDestBPP, *pDestBPP < 32 ? FALSE : TRUE);
		}

		// The GetBits() method must be added --> Ulrich
		CWinBmp* pwinbmp = (CWinBmp*)pBmp;
		BITMAPINFO* pBMI = (BITMAPINFO*)pwinbmp->GetBMI();
		BYTE* pBits = (BYTE*)pwinbmp->GetBits();
		if (*ppPal) {
			GetDIBits(dc, bm, 0, height, pwinbmp->GetBits(), pBMI, DIB_RGB_COLORS);
		}
		else {
			GetDIBits(dc, bm, 0, height, pwinbmp->GetBits(), pBMI, DIB_PAL_COLORS);
		}

		// Clean-up the remaining stuff
		ReleaseDC(0,dc);
		dc = 0;
		DeleteObject(bm);
		bm = NULL;
	}
	catch(...) {
		// Cleanup all resources allocated by us and let the
		// caller do the rest.
		delete [] plogpal;
		delete [] phdr;
		if (memdc) DeleteDC(memdc);
		if (dc) ReleaseDC(0,dc);
		if (bm) ::DeleteObject(bm);
		if (hemf) DeleteEnhMetaFile(hemf);
		throw;
	}
}

#endif // _WINDOWS
/*
/--------------------------------------------------------------------
|
|      $Log: wemfdec.cpp,v $
|      Revision 1.5  2000/01/16 20:43:18  anonymous
|      Removed MFC dependencies
|
|      Revision 1.4  2000/01/10 23:53:01  Ulrich von Zadow
|      Changed formatting & removed tabs.
|
|      Revision 1.3  1999/10/21 18:48:18  Ulrich von Zadow
|      no message
|
|
\--------------------------------------------------------------------
*/
