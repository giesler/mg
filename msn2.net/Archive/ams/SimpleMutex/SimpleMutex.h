// SimpleMutex.h : main header file for the SIMPLEMUTEX application
//

#if !defined(AFX_SIMPLEMUTEX_H__B1B52170_EC1F_454C_8377_3A7F94D00A5A__INCLUDED_)
#define AFX_SIMPLEMUTEX_H__B1B52170_EC1F_454C_8377_3A7F94D00A5A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CSimpleMutexApp:
// See SimpleMutex.cpp for the implementation of this class
//

class CSimpleMutexApp : public CWinApp
{
public:
	CSimpleMutexApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSimpleMutexApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CSimpleMutexApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SIMPLEMUTEX_H__B1B52170_EC1F_454C_8377_3A7F94D00A5A__INCLUDED_)
