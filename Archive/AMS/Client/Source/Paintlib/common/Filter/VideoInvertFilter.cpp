/*
/--------------------------------------------------------------------
|
|      $Id: VideoInvertFilter.cpp,v 1.1 2000/03/31 12:20:06 Ulrich von Zadow Exp $
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#include "stdpch.h"
#include "Filter.h"
#include "VideoInvertFilter.h"
#include "bitmap.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////


CVideoInvertFilter::CVideoInvertFilter()
{

}

CVideoInvertFilter::~CVideoInvertFilter()
{

}

double CVideoInvertFilter::Filter(double dVal)
{
	return 0;
}

void
fp_rgb_to_hsv (double *r,
            double *g,
            double *b)
{
  int red, green, blue;
  double h=0, s, v;
  int min, max;
  int delta;

  red = (int)*r;
  green = (int)*g;
  blue = (int)*b;

  if (red > green)
    {
      if (red > blue)
        max = red;
      else
        max = blue;

      if (green < blue)
        min = green;
      else
        min = blue;
    }
  else
    {
      if (green > blue)
        max = green;
      else
        max = blue;

      if (red < blue)
        min = red;
      else
        min = blue;
    }

  v = max;

  if (max != 0)
    s = ((max - min) * 255) / (double) max;
  else
    s = 0;

  if (s == 0)
    h = 0;
  else
    {
      delta = max - min;
      if (red == max)
        h = (green - blue) / (double) delta;
      else if (green == max)
        h = 2 + (blue - red) / (double) delta;
      else if (blue == max)
        h = 4 + (red - green) / (double) delta;
      h *= 42.5;

      if (h < 0)
        h += 255;
      if (h > 255)
        h -= 255;
    }

  *r = h;
  *g = s;
  *b = v;
}


void
fp_hsv_to_rgb (double *h,
            double *s,
            double *v)
{
  double hue, saturation, value;
  double f, p, q, t;

  if (((int)*s) == 0)
    {
      *h = *v;
      *s = *v;
      *v = *v;
    }
  else
    {
      hue = *h * 6.0 / 255.0;
      saturation = *s / 255.0;
      value = *v / 255.0;

      f = hue - (int) hue;
      p = value * (1.0 - saturation);
      q = value * (1.0 - (saturation * f));
      t = value * (1.0 - (saturation * (1.0 - f)));

      switch ((int) hue)
        {
        case 0:
          *h = value * 255.0;
          *s = t * 255.0;
          *v = p * 255.0;
          break;
        case 1:
          *h = q * 255.0;
          *s = value * 255.0;
          *v = p * 255.0;
          break;
        case 2:
          *h = p * 255.0;
          *s = value * 255.0;
          *v = t * 255.0;
          break;
        case 3:
          *h = p * 255.0;
          *s = q * 255.0;
          *v = value * 255.0;
          break;
        case 4:
          *h = t * 255.0;
          *s = p * 255.0;
          *v = value * 255.0;
          break;
        case 5:
          *h = value * 255.0;
          *s = p * 255.0;
          *v = q * 255.0;
          break;
        }
    }
}

void CVideoInvertFilter::Apply(CBmp *pBmpSource, CBmp *pBmpDest)
{
  // Only works for 32 bpp bitmaps at the moment.
  PLASSERT (pBmpSource->GetBitsPerPixel() == 32);

  pBmpDest->Create (pBmpSource->GetWidth(), pBmpSource->GetHeight(), 32, FALSE);
  BYTE ** pSrcLines = pBmpSource->GetLineArray();
  BYTE ** pDstLines = pBmpDest->GetLineArray();

  for (int y = 0; y<pBmpDest->GetHeight(); ++y)
  { // For each line
    BYTE * pSrcPixel = pSrcLines[y];
    BYTE * pDstPixel = pDstLines[y];

    for (int x = 0; x < pBmpDest->GetWidth(); ++x)
    { // For each pixel
      double v1, v2, v3;

      v1 = (double)pSrcPixel[RGBA_RED];
      v2 = (double)pSrcPixel[RGBA_GREEN];
      v3 = (double)pSrcPixel[RGBA_BLUE];

      fp_rgb_to_hsv(&v1, &v2, &v3);
      v3 = 255.0-v3;
      fp_hsv_to_rgb(&v1, &v2, &v3);

      *pDstPixel = (int)v3;
	  pDstPixel++;
      *pDstPixel = (int)v2;
	  pDstPixel++;
      *pDstPixel = (int)v1;
	  pDstPixel++;

	  pDstPixel++;

	  /*BYTE (pSrcPixel[RGBA_RED]*0.212671+
	                       pSrcPixel[RGBA_GREEN]*0.715160+
                         pSrcPixel[RGBA_BLUE]*0.072169);*/
	    pSrcPixel += sizeof(RGBAPIXEL);
    }
  }

}

/*
/--------------------------------------------------------------------
|
|      $Log: VideoInvertFilter.cpp,v $
|      Revision 1.1  2000/03/31 12:20:06  Ulrich von Zadow
|      Video invert filter (beta)
|
|
|
\--------------------------------------------------------------------
*/
