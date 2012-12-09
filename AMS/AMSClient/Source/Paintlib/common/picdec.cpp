/*
/--------------------------------------------------------------------
|
|      $Id: picdec.cpp,v 1.8 2000/03/30 21:24:15 Ulrich von Zadow Exp $
|      Generic Picture Decoder Class
|
|      Abstract base class to construct CBmps from picture data in
|      memory or in a file. Classes derived from this class implement
|      concrete decoders for specific file formats. The data is
|      returned in 32-bit DIBs with a valid alpha channel.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "picdec.h"
#include "filesrc.h"
#ifdef _WINDOWS
#include "ressrc.h"
#endif
#include "memsrc.h"
#include "except.h"


// Set default Trace configuration here. The defined levels are
// explained in picdec.h.
int CPicDecoder::m_TraceLevel = 0;
char * CPicDecoder::m_pszTraceFName = NULL;


CPicDecoder::CPicDecoder
    ()
    // Creates a decoder
{
}


CPicDecoder::~CPicDecoder
    ()
{
  if (m_pszTraceFName)
    delete m_pszTraceFName;
  m_pszTraceFName = NULL;
}


void CPicDecoder::MakeBmpFromFile
    ( const char * pszFName,
      CBmp * pBmp,
      int BPPWanted,
      IProgressNotification * pProgNot
    )
    // Decodes a picture in a file by creating a file data source and
    // calling MakeBmp with this data source.
{
  CFileSource * pFileSrc = NULL;
  int err;

  char sz[256];
  sprintf (sz, "--- Decoding file %s. ---\n", pszFName);
  Trace (1, sz);

  try
  {
    pFileSrc = new CFileSource (pProgNot);
    err = pFileSrc->Open (pszFName);
    if (err)
    {
      sprintf (sz, "Opening %s failed", pszFName);
      raiseError (err, sz);
    }

    MakeBmp (pFileSrc, pBmp, BPPWanted);
    pFileSrc->Close ();
  }
  catch (CTextException)
  {
    // Clean up on error
    if (pFileSrc) delete pFileSrc;
    throw;
  }

  // Clean up
  delete pFileSrc;
}

#ifdef _WINDOWS
void CPicDecoder::MakeBmpFromResource
    ( HINSTANCE hInstResource, int ResourceID,
      CBmp * pBmp,
      int BPPWanted,
	    const char * ResType,
      HMODULE hResModule
    )
    // Decodes a picture in a resource by creating a resource data
    // source and calling MakeBmp with this data source.
{
  CResourceSource * pResSrc = NULL;
  int err;

  char sz[256];
  sprintf (sz, "--- Decoding resource ID %i. ---\n", ResourceID);
  Trace (1, sz);

  try
  {
    pResSrc = new CResourceSource ();
    err = pResSrc->Open (hInstResource, ResourceID, ResType);
    if (err)
    {
      sprintf (sz, "Opening resource %i failed", ResourceID);
      raiseError (err, sz);
    }

    MakeBmp (pResSrc, pBmp, BPPWanted);
    pResSrc->Close ();
  }
  catch (CTextException)
  {
    // Clean up on error
    if (pResSrc) delete pResSrc;
    throw;
  }

  // Clean up
  delete pResSrc;
}
#endif


void CPicDecoder::MakeBmpFromMemory
    ( unsigned char * ucMemSrc,
      int MemSrcSize,
      CBmp * pBmp,
      int BPPWanted,
      IProgressNotification * pProgNot
    )
    // Decodes a picture from memory directly resembling the image file by
    // creating a memory data source and calling MakeBmp with this data source.
{
  CMemSource * pMemSrc = NULL;
  int err;

  char sz[256];
  sprintf (sz, "--- Decoding from memory at %p. ---\n", ucMemSrc);
  Trace (1, sz);

  try
  {
    pMemSrc = new CMemSource ();
    err = pMemSrc->Open (ucMemSrc, MemSrcSize);
    if (err)
    {
      sprintf (sz, "Reading from memory at %p failed", ucMemSrc);
      raiseError (err, sz);
    }

    MakeBmp (pMemSrc, pBmp, BPPWanted);
    pMemSrc->Close ();
  }
  catch (CTextException)
  {
    // Clean up on error
    if (pMemSrc) delete pMemSrc;
    throw;
  }

  // Clean up
  delete pMemSrc;
}


void CPicDecoder::MakeBmp
    ( CDataSource * pDataSrc,
      CBmp * pBmp,
      int BPPWanted
    )
    // Decodes a picture by getting the encoded data from pDataSrc.
    // Stores the results in pBmp.
{
  RGBAPIXEL * pPal = NULL;

  PLASSERT (BPPWanted == 0 || BPPWanted == 8 || BPPWanted == 32);

  int DestBPP = BPPWanted;

  try
  {
    DoDecode (pBmp, &pPal, &DestBPP, pDataSrc);
    if (pPal) delete pPal;
  }
  catch (CTextException)
  {
    if (pPal) delete pPal;
    throw;  // Pass the error on.
  }
}

void CPicDecoder::SetTraceConfig
    ( int Level,
      char * pszFName
    )
{
  // Level has to be between 0 and 3.
  PLASSERT (Level < 4);

  m_TraceLevel = Level;

  if (m_pszTraceFName)
    delete m_pszTraceFName;

  if (pszFName)
  {
    m_pszTraceFName = new char[strlen (pszFName)+1];
    strcpy (m_pszTraceFName, pszFName);

    // Delete any old Trace file with the same name.
    remove (m_pszTraceFName);
  }
  else
    m_pszTraceFName = NULL;
}

void CPicDecoder::raiseError
    ( int Code,
      char * pszErr
    )
    // This function is needed by callbacks outside of any object,
    // so it's public and static. It should not be called from
    // outside of the library.
{
  char sz[256];
  sprintf (sz, "Decoder error: %s\n", pszErr);
  Trace (0, sz);
  throw (CTextException (Code, sz));
}

void CPicDecoder::Trace
    ( int TraceLevel,
      const char * pszMessage
    )
    // Outputs debugging data to a file or to the MSVC debug console.
{
  if (TraceLevel <= m_TraceLevel)
  {
    if (m_pszTraceFName)
    {
      // The file is closed after every call so no data is lost
      // if the program crashes.
      FILE * pFile = fopen (m_pszTraceFName, "a+t");
      if (pFile != (FILE *)0)
      {
        fprintf (pFile, pszMessage);
        fclose (pFile);
      }
      else
      { // No permission? File locked? Filename nonsense?
        PLTRACE ("Error opening Trace file!\n");
      }
    }
    else
      PLTRACE (pszMessage);
  }
}

void CPicDecoder::DoDecode
    ( CBmp * pBmp,
      RGBAPIXEL ** ppPal,
      int * pDestBPP,
      CDataSource * pDataSrc
    )
    // Implements the actual decoding process. Uses variables local to
    // the object to retrieve and store the data. Implemented in
    // derived classes.
{
  // This routine should never be called. It's here so derived classes
  // can override MakeDIB directly if they want to. (CAnyDecoder does
  // this).
  PLASSERT (FALSE);
}

/////////////////////////////////////////////////////////////////////
// Protected routines



// These routines expand pixel data of various bit depths to 32 bpp.
// The original intent was for several derived classes to use them.
// As it is, they are too slow & therefore almost unused.

void CPicDecoder::Expand1bpp
    ( BYTE * pDest,
      BYTE * pSrc,
      int Width,      // Width in pixels
      RGBAPIXEL * pPal
    )
{
  int i;

  for (i=0; i<Width/8; i++)
  {
    *((RGBAPIXEL *)pDest) = *(pPal+((*pSrc >> 7) & 1));
    *((RGBAPIXEL *)(pDest+4)) = *(pPal+((*pSrc >> 6) & 1));
    *((RGBAPIXEL *)(pDest+8)) = *(pPal+((*pSrc >> 5) & 1));
    *((RGBAPIXEL *)(pDest+12)) = *(pPal+((*pSrc >> 4) & 1));
    *((RGBAPIXEL *)(pDest+16)) = *(pPal+((*pSrc >> 3) & 1));
    *((RGBAPIXEL *)(pDest+20)) = *(pPal+((*pSrc >> 2) & 1));
    *((RGBAPIXEL *)(pDest+24)) = *(pPal+((*pSrc >> 1) & 1));
    *((RGBAPIXEL *)(pDest+28)) = *(pPal+(*pSrc & 1));
    pSrc++;
    pDest += 32;
  }
  if (Width & 7)  // Check for leftover pixels
    for (i=7; i>(8-Width & 7); i--)
    {
      *((RGBAPIXEL *)pDest) = *(pPal+((*pSrc >> i) & 1));
      pDest += 4;
    }
}

void CPicDecoder::Expand2bpp
    ( BYTE * pDest,
      BYTE * pSrc,
      int Width,      // Width in pixels
      RGBAPIXEL * pPal
    )
{
  int i;
  for (i=0; i<Width/4; i++)
  {
    *((RGBAPIXEL *)pDest) = *(pPal+((*pSrc >> 6) & 3));
    *((RGBAPIXEL *)(pDest+4)) = *(pPal+((*pSrc >> 4) & 3));
    *((RGBAPIXEL *)(pDest+8)) = *(pPal+((*pSrc >> 2) & 3));
    *((RGBAPIXEL *)(pDest+12)) = *(pPal+(*pSrc & 3));
    pSrc++;
    pDest += 16;
  }
  if (Width & 3)  // Check for leftover pixels
    for (i=6; i>8-(Width & 3)*2; i-=2)
    {
      *((RGBAPIXEL *)pDest) = *(pPal+((*pSrc >> i) & 3));
      pDest += 4;
    }
}

void CPicDecoder::Expand4bpp
    ( BYTE * pDest,
      BYTE * pSrc,
      int Width,      // Width in pixels
      RGBAPIXEL * pPal
    )
{
  int i;

  for (i=0; i<Width/2; i++)
  {
    *((RGBAPIXEL *)pDest) = *(pPal+((*pSrc >> 4) & 15));
    *((RGBAPIXEL *)(pDest+4)) = *(pPal+(*pSrc & 15));
    pSrc++;
    pDest += 8;
  }
  if (Width & 1) // Odd Width?
  {
    *((RGBAPIXEL *)pDest) = *(pPal+((*pSrc >> 4) & 15));
    pDest += 4;
  }
}

void CPicDecoder::Expand8bpp
    ( BYTE * pDest,
      BYTE * pSrc,
      int Width,      // Width in pixels
      RGBAPIXEL * pPal
    )
{
  int i;
  for (i=0; i<Width; i++)
  {
    *((RGBAPIXEL *)pDest) = *(pPal+*pSrc);
    pSrc++;
    pDest += 4;
  }
}

void CPicDecoder::CalcDestBPP
    ( int SrcBPP,
      int * pDestBPP
    )
{
  PLASSERT (*pDestBPP == 32 || *pDestBPP == 0);
  if (*pDestBPP == 0)
  {
    if (SrcBPP <= 8)
      *pDestBPP = 8;
    else
      *pDestBPP = 32;
  }
}

/*
/--------------------------------------------------------------------
|
|      $Log: picdec.cpp,v $
|      Revision 1.8  2000/03/30 21:24:15  Ulrich von Zadow
|      Added MakeBmpFromMemory() function by Markus Ewald
|
|      Revision 1.7  2000/01/16 20:43:14  anonymous
|      Removed MFC dependencies
|
|      Revision 1.6  2000/01/11 21:40:30  Ulrich von Zadow
|      Added instance handle parameter to LoadFromResource()
|
|      Revision 1.5  2000/01/08 15:51:30  Ulrich von Zadow
|      Misc. modifications to png encoder.
|
|      Revision 1.4  1999/11/08 22:12:51  Ulrich von Zadow
|      Andreas Koepf: Added resource type as parameter to
|      MakeBmpFromResource
|
|      Revision 1.3  1999/10/03 18:50:51  Ulrich von Zadow
|      Added automatic logging of changes.
|
|
\--------------------------------------------------------------------
*/
