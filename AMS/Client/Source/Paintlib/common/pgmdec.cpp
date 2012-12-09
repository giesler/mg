/*
/--------------------------------------------------------------------
|
|      $Id: pgmdec.cpp,v 1.1 2000/03/16 13:56:37 Ulrich von Zadow Exp $
|      portable graymap Decoder Class
|
|      Original author: Jose Miguel Buenaposada Biencinto.
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "pgmdec.h"
#include "except.h"

IMPLEMENT_DYNAMIC (CPGMDecoder, CPicDecoder);

CPGMDecoder::CPGMDecoder
    ()
    // Creates a decoder
{
}

CPGMDecoder::~CPGMDecoder
    ()
{
}

void CPGMDecoder::DoDecode
    ( CBmp * pBmp,
      RGBAPIXEL ** ppPal,
      int * pDestBPP,
      CDataSource * pDataSrc
    )
{
  PGMHEADER PgmHead;
  Trace (2, "Decoding PGM.\n");

  readPgmHeader(&PgmHead, pDataSrc);

  CalcDestBPP (8, pDestBPP);

  // Image doesn't have alpha channel
  pBmp->Create (PgmHead.ImageWidth, PgmHead.ImageHeight,
                  *pDestBPP, FALSE);

  readImage (&PgmHead, pBmp, ppPal, *pDestBPP, pDataSrc);
}


void CPGMDecoder::readPgmHeader
    ( PGMHEADER * pPgmHead,       // Pointer to TGA header structure
      CDataSource * pDataSrc
    )
{  
  int current = 0;
  bool HeaderComplete = false;

  // Read type 
  m_LastByte = ReadByte (pDataSrc);
  if (m_LastByte!=0x50) // ASCII P
      raiseError (ERR_FORMAT_UNKNOWN,
                  "PGM decoder: Is not the correct identifier P5 or P2.");

  m_LastByte = ReadByte (pDataSrc);
  if (m_LastByte==0x32) // ASCII 2
	  pPgmHead->ImageType = PGM_P2;
  else if (m_LastByte==0x35) // ASCII 5
	  pPgmHead->ImageType = PGM_P5;
  else 
      raiseError (ERR_FORMAT_UNKNOWN,
                  "PGM decoder: Is not the correct identifier P5 or P2.");

  m_LastByte = ReadByte (pDataSrc);
	
  // Search for the with, height and Max gray value
  while (current<3)
  {
    if (m_LastByte==0x23) // # Stars a comment 
		skipComment(pDataSrc);
	else if ((m_LastByte>=0x30)&&(m_LastByte<=0x39)) // A digit
		switch (current)
		{
		case 0: // looking for the width
		  {
		  pPgmHead->ImageWidth = readASCIIDecimal(pDataSrc);
		  current++;
		  }
		  break;
		case 1: // looking for the height
		  {
		  pPgmHead->ImageHeight = readASCIIDecimal(pDataSrc);
		  current++;
		  }
		  break;
		case 2: // looking for the max gray value
		  {
		  pPgmHead->MaxGrayValue  = readASCIIDecimal(pDataSrc);
          if ((pPgmHead->MaxGrayValue>255)||(pPgmHead->MaxGrayValue<=0))
	        pPgmHead->MaxGrayValue=255;
		  current++;
		  }
		  break;
		default:
          continue;
		}
	else
      skipPgmASCIISeparators(pDataSrc);
  }  
}

BYTE *CPGMDecoder::readASCIILine(CDataSource *pDataSrc)
{
  int i = 0; 
  bool HaveLine = false;
  BYTE byte;
  BYTE* pLine   = new BYTE[PGM_MAXLINESIZE]; // Line should not be larger than 70 bytes

  do
  {
	  if (i==80)
		  raiseError (ERR_FORMAT_UNKNOWN,
	                 "PGM decoder: File Line to long.");

	  byte    =ReadByte(pDataSrc);
	  pLine[i]=byte;
	  if ((byte==0x0D)|| // Carriege Return 
	      (byte==0x0A))  // Line Feed	     		 
	  {
		  HaveLine=true;
		  pLine[i]=0x00;
	  }	  
	  i++;
  }
  while (!HaveLine);

  return pLine;
}


int CPGMDecoder::readASCIIDecimal(CDataSource * pDataSrc)
{
  int Value    = 0;
  int digit;

  while ((m_LastByte>=0x30)&&(m_LastByte<=0x39)) // Is ASCII digit
  {
	digit      = m_LastByte - 0x30;
	Value      = Value*10+digit;
	m_LastByte = ReadByte(pDataSrc);
  }

  return Value;
}

void CPGMDecoder::skipComment(CDataSource * pDataSrc)
{
	while ((m_LastByte!=0x0D)&& // Carriege Return 
	       (m_LastByte!=0x0A))  // New Line	     		 
	{
	  m_LastByte     = ReadByte(pDataSrc);
	}
}
void CPGMDecoder::skipPgmASCIISeparators(CDataSource * pDataSrc)
{
	while ((m_LastByte==0x20)|| // Space
	     (m_LastByte==0x0D)|| // Carriege Return 
//	     (m_LastByte<=0x10)|| // Tab
	     (m_LastByte==0x0A))  // New Line	     		 
	{
	  m_LastByte     = ReadByte(pDataSrc);
	}
}

void CPGMDecoder::setGrayPalette
    ( RGBAPIXEL ** ppPal
    )
    // Creates a grayscale palette.
{
  int i;
  *ppPal = new RGBAPIXEL [256];
  for (i=0; i<256; i++)
  {
    SetRGBAPixel ((*ppPal)+i, i, i, i, 0xFF);
  }
}

void CPGMDecoder::readImage
    ( PGMHEADER * pPgmHead,       // Pointer to PGM header structure
      CBmp * pBmp,
      RGBAPIXEL ** ppPal,	  
      int DestBPP,
	  CDataSource * pDataSrc
    )
{
  setGrayPalette (ppPal);

  switch (pPgmHead->ImageType)
  {
    case PGM_P5:
    case PGM_P2:
	  readData(pPgmHead, pBmp, *ppPal, DestBPP, pDataSrc);
	  break;
    default:
      raiseError (ERR_FORMAT_UNKNOWN, "Unknown PGM image type.");
  }
}

void CPGMDecoder::readData
    ( PGMHEADER * pPgmHead,       // Pointer to PGM header structure
      CBmp * pBmp,
      RGBAPIXEL * pPal,
      int DestBPP,
      CDataSource * pDataSrc
    )
{
  int Width  = pPgmHead->ImageWidth;
  int Height = pPgmHead->ImageHeight;
  int bpp    = DestBPP;

  BYTE * pDest;
  BYTE ** pLineArray = pBmp->GetLineArray();

  int y;

  if (pPgmHead->ImageType == PGM_P2)
  {
    skipPgmASCIISeparators(pDataSrc);
    m_UseLastByte = true;
  }
  
  for (y=0; y < Height; y++)
  {
      pDest = pLineArray[y];
	  if (pPgmHead->ImageType==PGM_P5) // P5
        expandByteLine (pDest, pPgmHead->MaxGrayValue,Width,  bpp, pDataSrc, pPal, DestBPP);	  
	  else // P2
        expandASCIILine (pDest, pPgmHead->MaxGrayValue,Width, bpp, pDataSrc, pPal, DestBPP);
  }
}

void CPGMDecoder::expandASCIILine
    ( BYTE * pDest,
	  int MaxGrayValue,
	  int Width, 
      int bpp,
      CDataSource * pDataSrc,
      RGBAPIXEL * pPal,
      int DestBPP
    )
{
  int x;

  for (x=0; x<Width; x++)
  {
    if (DestBPP == 32)
    {
      *((RGBAPIXEL *)pDest) = readASCIIPixel32 (MaxGrayValue, bpp, pDataSrc, pPal);
      pDest += 4;
    }
    else
    {
      *pDest = readASCIIPixel8 (MaxGrayValue, bpp, pDataSrc);
      pDest ++;
    }
  }
}

void CPGMDecoder::expandByteLine
    ( BYTE * pDest,
	  int MaxGrayValue,
	  int Width, 
      int bpp,
      CDataSource * pDataSrc,
      RGBAPIXEL * pPal,
      int DestBPP
    )
{
  int x;
  int CurEntry;
  BYTE *pLine; 
 
  pLine = pDataSrc->ReadNBytes(Width);
  if (pLine==NULL)
	  return;

  for (x=0; x<Width; x++)
  {
    if (DestBPP == 32)
    {
	  CurEntry = ((int)pLine[x]*255)/MaxGrayValue; 
      *((RGBAPIXEL *)pDest) = pPal[(BYTE)CurEntry];
      pDest += 4;
    }
    else
    {
	  *pDest = (BYTE)(((int)pLine[x]*255)/MaxGrayValue); 
      pDest ++;
    }
  }
  
//  delete pLine; // is this neccesary?
}

RGBAPIXEL CPGMDecoder::readASCIIPixel32
    ( int MaxGrayValue,
	  int bpp,
      CDataSource * pDataSrc,
      RGBAPIXEL * pPal
    )
{
  RGBAPIXEL Dest;
  int CurEntry;

  skipPgmASCIISeparators(pDataSrc);
  CurEntry = readASCIIDecimal(pDataSrc);
  CurEntry = (CurEntry*255)/MaxGrayValue; 

  Dest = pPal[(BYTE)CurEntry];

  return Dest;
}

BYTE CPGMDecoder::readASCIIPixel8
    ( int MaxGrayValue,
	  int bpp,
      CDataSource * pDataSrc
    )
{
  BYTE Dest;
  int Value;

  PLASSERT (bpp == 8);

  skipPgmASCIISeparators(pDataSrc);
  m_UseLastByte = true;
  Value    = readASCIIDecimal(pDataSrc);  
  Dest     = (BYTE)((Value*255)/MaxGrayValue); 

  return Dest;
}


/*
/--------------------------------------------------------------------
|
|      $Log: pgmdec.cpp,v $
|      Revision 1.1  2000/03/16 13:56:37  Ulrich von Zadow
|      Added pgm decoder by Jose Miguel Buenaposada Biencinto
|
|
\--------------------------------------------------------------------
*/
