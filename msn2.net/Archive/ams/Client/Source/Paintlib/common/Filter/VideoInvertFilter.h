/*
/--------------------------------------------------------------------
|
|      $Id: VideoInvertFilter.h,v 1.1 2000/03/31 12:20:06 Ulrich von Zadow Exp $
|
|      Copyright (c) 1996-1998 Ulrich von Zadow
|
\--------------------------------------------------------------------
*/

#if !defined(AFX_VIDEOINVERTFILTER_H__5AB120C1_CEB0_11D3_AD70_444553540000__INCLUDED_)
#define AFX_VIDEOINVERTFILTER_H__5AB120C1_CEB0_11D3_AD70_444553540000__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "Filter.h"

class CVideoInvertFilter : public CFilter  
{
public:
	void Apply(CBmp * pBmpSource, CBmp * pBmpDest);
	virtual double Filter (double dVal);
	CVideoInvertFilter();

	virtual ~CVideoInvertFilter();

};

#endif // !defined(AFX_VIDEOINVERTFILTER_H__5AB120C1_CEB0_11D3_AD70_444553540000__INCLUDED_)

/*
/--------------------------------------------------------------------
|
|      $Log: VideoInvertFilter.h,v $
|      Revision 1.1  2000/03/31 12:20:06  Ulrich von Zadow
|      Video invert filter (beta)
|
|
|
\--------------------------------------------------------------------
*/
