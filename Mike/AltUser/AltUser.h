// AltUser.h : main header file for the ALTUSER application
//

#if !defined(AFX_ALTUSER_H__94C0830A_7825_4257_8374_2EF08A34A3C4__INCLUDED_)
#define AFX_ALTUSER_H__94C0830A_7825_4257_8374_2EF08A34A3C4__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CAltUserApp:
// See AltUser.cpp for the implementation of this class
//

class CAltUserApp : public CWinApp
{
public:
	CAltUserApp();
	
	void DisplayError();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAltUserApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CAltUserApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ALTUSER_H__94C0830A_7825_4257_8374_2EF08A34A3C4__INCLUDED_)
