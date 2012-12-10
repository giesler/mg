// StartSS.h : main header file for the STARTSS application
//

#if !defined(AFX_STARTSS_H__CE1BCEC9_BE78_427F_B0E1_1430AC29B322__INCLUDED_)
#define AFX_STARTSS_H__CE1BCEC9_BE78_427F_B0E1_1430AC29B322__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CStartSSApp:
// See StartSS.cpp for the implementation of this class
//

class CStartSSApp : public CWinApp
{
public:
	CStartSSApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CStartSSApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CStartSSApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STARTSS_H__CE1BCEC9_BE78_427F_B0E1_1430AC29B322__INCLUDED_)
