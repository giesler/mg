#if !defined(AFX_UPDATEWARNING_H__5CCD1F58_45D8_453F_BFE6_0DABB71F259F__INCLUDED_)
#define AFX_UPDATEWARNING_H__5CCD1F58_45D8_453F_BFE6_0DABB71F259F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// UpdateWarning.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CUpdateWarning dialog

class CUpdateWarning : public CDialog
{
// Construction
public:
	CUpdateWarning(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CUpdateWarning)
	enum { IDD = IDD_UPDATEWARNING };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CUpdateWarning)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CUpdateWarning)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_UPDATEWARNING_H__5CCD1F58_45D8_453F_BFE6_0DABB71F259F__INCLUDED_)
