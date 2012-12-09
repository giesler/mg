/*-------------------------------------------------------------------------------------------*\
++                                                                                           ++
++            Copyright (C) 1998 - 2000 by Punk Productions Electronic Entertainment.        ++
++                                http://cust.nol.at/ppee                                    ++
++                                                                                           ++
++     Content: Quantization/Dithering				                                         ++
++  Programmer: Nikolaus Brennig (virtualnik@nol.at)										 ++
++                                                                                           ++
\*-------------------------------------------------------------------------------------------*/
// This code is partially based on:
///////////////////////////////////////////////////////////////////////////
// DIBQuant version 1.0
// Copyright (c) 1993 Edward McCreary.
// All rights reserved.
//
// Redistribution and use in source and binary forms are freely permitted
// provided that the above copyright notice and attibution and date of work
// and this paragraph are duplicated in all such forms.
// THIS SOFTWARE IS PROVIDED "AS IS" AND WITHOUT ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED
// WARRANTIES OF MERCHANTIBILILTY AND FITNESS FOR A PARTICULAR PURPOSE.
///////////////////////////////////////////////////////////////////////////

#ifndef _DITHER8BIT_H
#define _DITHER8BIT_H


//---------------------------------------------------------------------------------------------
// Important defines...
//---------------------------------------------------------------------------------------------
// For 8bit Dithering and Palette Generation...
#define IDX_MEDIAN          0       // Median Cut
#define IDX_POPULARITY      1       // Popularity Sort
#define IDX_DEFAULT         2       // Use Default Palette
#define IDX_NEUQUANTBEST	3		// Neural Quantization Best/Slowest Quality
#define IDX_NEUQUANTMIDDLE	4		// Neural Quantization Middle/average Speed Quality
#define IDX_NEUQUANTWORST	5		// Neural Quantization Worst/Fastest Quality

#define IDX_NONE            0       // None
#define IDX_ORDERED         1       // Ordered Dithering
#define IDX_JITTER          2       // Jitter preprocessing
#define IDX_FS				3		// Floyd-Steinberg Dithering

#define MIN(a,b)			((a)<(b)?(a):(b))
#define MAX(a,b)			((a)>(b)?(a):(b))
#define CLIP(x)				((x)>255?255:((x)<0?0:(x)))
#define COLOR_MAX			32
#define COUNT_LIMIT			0xFFFFL
#define SWAP(a, b)			{ ULONG tmp = a; a = b; b = tmp; }
#define INDEX(r,g,b)		(((DWORD)b) | (((DWORD)g)<<5) | (((DWORD)r)<<10) )
#define WIDTHBYTES(bits)    ((((bits) + 31) >> 5) << 2)

// Some defines for jitter-dither...
#define JITTER_TABLE_BITS   10
#define JITTER_TABLE_SIZE   (1<<JITTER_TABLE_BITS)
#define JITTER_MASK         (JITTER_TABLE_SIZE-1)

// jitter macros...
#define jitterx(x,y,s) (uranx[((x+(y<<2))+irand[(x+s)&JITTER_MASK])&JITTER_MASK])
#define jittery(x,y,s) (urany[((y+(x<<2))+irand[(y+s)&JITTER_MASK])&JITTER_MASK])


//---------------------------------------------------------------------------------------------
// Important structs...
//---------------------------------------------------------------------------------------------
typedef struct box
{
	INT				r0, r1, g0, g1, b0, b1;
	LONG			rave,gave,bave;
	ULONG			count;
} Box;

typedef struct node
{
	INT				index;
	ULONG			count; 
} Node, FAR *LPNode;

typedef struct tagBuffers
{
	FARPROC			lpStatus;
	INT				red[256],green[256],blue[256];
	Box				box[256];
	LONG			SQR[256];
	LPNode			*lpHisto;
} QUBUF, FAR *LPQUBUF;


//---------------------------------------------------------------------------------------------
// local prototypes...
//---------------------------------------------------------------------------------------------
VOID		m_box(LPQUBUF lpBuffer);
VOID		make_box(int r, int g, int b, int index, unsigned long c,LPQUBUF lpBuffer);
VOID		squeeze(int b,LPQUBUF lpBuffer);
VOID		add_color(int r, int g, int b, unsigned long c,LPQUBUF lpBuffer);
VOID		force(int r, int g, int b, unsigned long c,LPQUBUF lpBuffer);
VOID		pop(LPQUBUF lpBuffer);
INT			GetNeighbor(int r,int g,int b,LPQUBUF lpBuffer);
LPQUBUF		InitLUT();
VOID		ClearLUT(LPQUBUF lpBuffers);
BYTE		*Dither( BYTE *lpDIB, int nDither, LPQUBUF lpBuffer, INT W, INT H );
VOID		jitter(long x, long y, int *r,int *g,int *b);
VOID		LoadDefaultPal(LPQUBUF lpBuffer);

BYTE *DitherPicture8bit( BYTE *MyBuffer, INT PaletteType, INT DitherType, INT W, INT H, 
                                                 UCHAR palette[768] );

#endif /* _DITHER8BIT_H */