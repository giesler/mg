/*
/--------------------------------------------------------------------
|
|      $Id: anydec.cpp,v 1.10 2000/03/31 11:53:29 Ulrich von Zadow Exp $
|      Picture Decoder Class
|
|      Class which decodes pictures with any known format. It auto-
|      detects the format to use and delegates the work to one of the
|      other decoder classes.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "anydec.h"

#include "bitmap.h"
#include "except.h"

// needed for BMP recognition even when this format is not decoded
#include "windefs.h"

#ifdef SUPPORT_BMP
#include "bmpdec.h"
#endif

#ifdef SUPPORT_PICT
#include "pictdec.h"
#endif

#ifdef SUPPORT_TGA
#include "tgadec.h"
#endif

#ifdef SUPPORT_TIFF
#include "tiffdec.h"
#endif

#ifdef SUPPORT_JPEG
#include "jpegdec.h"
#endif

#ifdef SUPPORT_PNG
#include "pngdec.h"
#endif

#ifdef SUPPORT_WEMF
#include "wemfdec.h"
#endif

#ifdef SUPPORT_PCX
#include "pcxdec.h"
#endif

#ifdef SUPPORT_PGM
#include "pgmdec.h"
#endif

#define FT_UNKNOWN 0
#define FT_WINBMP  1
#define FT_MACPICT 2
#define FT_TARGA   3
#define FT_TIFF    4
#define FT_JPEG    5
#define FT_PNG     6
#define FT_EPSTIFF 7
#define FT_WMF     8
#define FT_EMF     9
#define FT_PCX    10
#define FT_PGM    11
#define FT_GIF    12

CAnyPicDecoder::CAnyPicDecoder ()
// Creates a decoder
{
  // Create sub-decoders...
#ifdef SUPPORT_BMP
  m_pBmpDec = new CBmpDecoder ();
#endif
#ifdef SUPPORT_TGA
  m_pTGADec = new CTGADecoder ();
#endif
#ifdef SUPPORT_TIFF
  m_pTIFFDec = new CTIFFDecoder ();
#endif
#ifdef SUPPORT_JPEG
  m_pJPEGDec = new CJPEGDecoder ();
#endif
#ifdef SUPPORT_PICT
#ifdef SUPPORT_JPEG
  m_pPictDec = new CPictDecoder (m_pJPEGDec);
#else
  m_pPictDec = new CPictDecoder (NULL);
#endif
#endif
#ifdef SUPPORT_PNG
  m_pPNGDec = new CPNGDecoder ();
#endif
#ifdef SUPPORT_WEMF
  m_pWEMFDec = new CWEMFDecoder();
#endif
#ifdef SUPPORT_PCX
  m_pPCXDec = new CPCXDecoder();
#endif
#ifdef SUPPORT_PGM
  m_pPGMDec = new CPGMDecoder();
#endif
}


CAnyPicDecoder::~CAnyPicDecoder ()
{
#ifdef SUPPORT_BMP
  delete m_pBmpDec;
#endif
#ifdef SUPPORT_PICT
  delete m_pPictDec;
#endif
#ifdef SUPPORT_TGA
  delete m_pTGADec;
#endif
#ifdef SUPPORT_TIFF
  delete m_pTIFFDec;
#endif
#ifdef SUPPORT_JPEG
  delete m_pJPEGDec;
#endif
#ifdef SUPPORT_PNG
  delete m_pPNGDec;
#endif
#ifdef SUPPORT_WEMF
  delete m_pWEMFDec;
#endif
#ifdef SUPPORT_PCX
  delete m_pPCXDec;
#endif
#ifdef SUPPORT_PGM
  delete m_pPGMDec;
#endif
}


void CAnyPicDecoder::MakeBmp (CDataSource * pDataSrc, CBmp * pBmp, int BPPWanted)
{
  int Type = FT_UNKNOWN;

#ifdef SUPPORT_WEMF
  // If we have support for WMF and EMF, we check only the
  // file extension and let Windows do the rest
  char* strname = strupr(strdup(pDataSrc->GetName()));
  if (strname == NULL)
  {
    raiseError (ERR_NO_MEMORY,"Out of memory during strdup.");
  }
  strupr(strname);
  if (strstr(strname,".WMF") != NULL)
  {
    Type = FT_WMF;
  }
  else if (strstr(strname,".EMF") != NULL)
  {
    Type = FT_EMF;
  }
  ::free(strname);
#endif

  if (Type == FT_UNKNOWN)
  {
    Type = getFileType (pDataSrc->GetBufferPtr (512),
                        pDataSrc->GetFileSize());
  }

  switch (Type)
  {
    case FT_UNKNOWN:
      raiseError (ERR_UNKNOWN_FILE_TYPE, "Unknown file type.");
#ifdef SUPPORT_BMP
    case FT_WINBMP:
      Trace (2, "Windows bitmap recognized.\n");
      m_pBmpDec->MakeBmp (pDataSrc, pBmp, BPPWanted);
      break;
#endif
#ifdef SUPPORT_PICT
    case FT_MACPICT:
      Trace (2, "Mac PICT recognized.\n");
      m_pPictDec->MakeBmp (pDataSrc, pBmp, BPPWanted);
      break;
#endif
#ifdef SUPPORT_TGA
    case FT_TARGA:
      Trace (2, "TGA file recognized.\n");
      m_pTGADec->MakeBmp (pDataSrc, pBmp, BPPWanted);
      break;
#endif
#ifdef SUPPORT_TIFF
    case FT_EPSTIFF:
      Trace (2, "TIFF preview in EPS file recognized.\n");
      // skip eps information
      pDataSrc->ReadNBytes (epsLongVal(20+pDataSrc->GetBufferPtr(30)));
      m_pTIFFDec->MakeBmp (pDataSrc, pBmp, BPPWanted);
      break;
    case FT_TIFF:
      Trace (2, "TIFF file recognized.\n");
      m_pTIFFDec->MakeBmp (pDataSrc, pBmp, BPPWanted);
      break;
#endif
#ifdef SUPPORT_JPEG
    case FT_JPEG:
      Trace (2, "JPEG file recognized.\n");
      m_pJPEGDec->MakeBmp (pDataSrc, pBmp, BPPWanted);
      break;
#endif
#ifdef SUPPORT_PNG
    case FT_PNG:
      Trace (2, "PNG file recognized.\n");
      m_pPNGDec->MakeBmp (pDataSrc, pBmp, BPPWanted);
      break;
#endif
#ifdef SUPPORT_WEMF
    case FT_WMF:
      Trace (2, "WMF file recognized.\n");
      m_pWEMFDec->MakeBmp (pDataSrc, pBmp, BPPWanted);
      break;
    case FT_EMF:
      Trace (2, "EMF file recognized.\n");
      m_pWEMFDec->MakeBmp (pDataSrc, pBmp, BPPWanted);
      break;
#endif
#ifdef SUPPORT_PCX
    case FT_PCX:
      Trace (2, "PCX file recognized.\n");
      m_pPCXDec->MakeBmp (pDataSrc, pBmp, BPPWanted);
      break;
#endif
#ifdef SUPPORT_GIF
    case FT_GIF:
      Trace (2, "GIF file recognized.\n");
      raiseError (ERR_FORMAT_NOT_SUPPORTED,
                  "paintlib does not support gif.");
      break;
#endif
#ifdef SUPPORT_PGM
    case FT_PGM:
      Trace (2, "PGM file recognized.\n");
      m_pPGMDec->MakeBmp (pDataSrc, pBmp, BPPWanted);
      break;
#endif
    default:
      raiseError (ERR_FORMAT_NOT_SUPPORTED,
                  "Library not compiled for this file type.");
      break;
  }
}


/////////////////////////////////////////////////////////////////////
// Local functions

int CAnyPicDecoder::getFileType (BYTE * pData, int DataLen)
// Check for file-format-specific data & return the file type if
// something fits.
{
  // Check for bitmap file signature: First 2 bytes are 'BM'.
  WINBITMAPFILEHEADER * pBFH;
  pBFH = (WINBITMAPFILEHEADER *) pData;
#ifdef _WINDOWS // Klaube
  if (pBFH->bfType == 0x4d42)
#else
if (pBFH->bfType == 0x424d)
#endif
    return FT_WINBMP;

  // Check for TGA file. The TGA format doesn't have a signature,
  // so the program checks for a meaningful TGA header.
  BOOL bCouldBeTGA = TRUE;
  if (*(pData+1) > 1)
    bCouldBeTGA = FALSE;
  BYTE TGAImgType = *(pData+2);
  if ((TGAImgType > 11) || (TGAImgType > 3 && TGAImgType < 9))
    bCouldBeTGA = FALSE;
  BYTE TGAColMapDepth = *(pData+7);
  if (TGAColMapDepth != 8 && TGAColMapDepth != 15 &&
      TGAColMapDepth != 16 && TGAColMapDepth != 24 &&
      TGAColMapDepth != 32 && TGAColMapDepth != 0)
    bCouldBeTGA = FALSE;
  BYTE TGAPixDepth = *(pData+16);
  if (TGAPixDepth != 8 && TGAPixDepth != 15 &&
      TGAPixDepth != 16 && TGAPixDepth != 24 &&
      TGAPixDepth != 32)
    bCouldBeTGA = FALSE;
  if (bCouldBeTGA)
    return FT_TARGA;

  // Check for GIF
  ULONG GIFSig = *((ULONG *)pData);
  if (GIFSig == 0x38464947)
    return FT_GIF;

  // Check for TIFF
  ULONG TIFFSig = *((ULONG *)pData);
  if (TIFFSig == 0x002A4949 || TIFFSig == 0x2A004D4D)
    return FT_TIFF;

  // Check for Mac PICT signature and Version.
  if (DataLen > 540)
  {
    BYTE * pPictSig = (BYTE *)(pData+0x20a);
    if ((pPictSig[0] == 0x00 && pPictSig[1] == 0x11 &&
         pPictSig[2] == 0x02 && pPictSig[3] == 0xFF) ||
        (pPictSig[0] == 0x00 && pPictSig[1] == 0x11 &&
         pPictSig[2] == 0x01) ||
        (pPictSig[0] == 0x11 && pPictSig[1] == 0x01 &&
         pPictSig[2] == 0x01 && pPictSig[3] == 0x00))
      return FT_MACPICT;
  }

  // Check for JPEG/JFIF.
  if ((*pData == 0xFF) && (*(pData+1) == 0xD8) &&
      (*(pData+2) == 0xFF))
    return FT_JPEG;

  // Check for PNG.
  if ((*pData == 0x89) && (*(pData+1) == 0x50) &&
      (*(pData+2) == 0x4E) && (*(pData+3) == 0x47))
    return FT_PNG;

  // Check for TIFF wrapped in EPS
  ULONG EPSSig = *((ULONG *)pData);
  if ((EPSSig == 0xc6d3d0c5 || EPSSig == 0xc5d0d3c6) &&
      *(ULONG *)(pData+20) && *(ULONG *)(pData+24))
    return FT_EPSTIFF;

  // Check for PCX
  // This check isn't really safe... should find other criteria.
  if (pData[0] == 0x0A && pData[2] == 0x01)
    return FT_PCX;

  // Check for PGM
  if (pData[0] == 0x50 && ((pData[1] == 0x32)||(pData[1] == 0x35)))
    return FT_PGM;

  return FT_UNKNOWN;
}


long CAnyPicDecoder::epsLongVal (unsigned char *p)
{
  unsigned long retval = 0;
  int i;
  // this may look like an endian dependency but its not - EPS headers
  // are always this way round
  for (i = 0; i < 4; i++)
    retval = ((retval >> 8) & 0xffffffL) + (((long)*p++) << 24);
  return (long) retval;
}

/*
/--------------------------------------------------------------------
|
|      $Log: anydec.cpp,v $
|      Revision 1.10  2000/03/31 11:53:29  Ulrich von Zadow
|      Added quantization support.
|
|      Revision 1.9  2000/03/16 13:56:37  Ulrich von Zadow
|      Added pgm decoder by Jose Miguel Buenaposada Biencinto
|
|      Revision 1.8  2000/01/16 20:43:12  anonymous
|      Removed MFC dependencies
|
|      Revision 1.7  2000/01/10 23:52:59  Ulrich von Zadow
|      Changed formatting & removed tabs.
|
|      Revision 1.6  1999/12/08 15:39:45  Ulrich von Zadow
|      Unix compatibility changes
|
|      Revision 1.5  1999/10/21 17:43:08  Ulrich von Zadow
|      Added pcx support by Meng Bo.
|
|      Revision 1.4  1999/10/03 18:50:51  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
--------------------------------------------------------------------
*/
