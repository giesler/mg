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
	void SetComponents(CList<CComponent*, CComponent*> & lstComps);
// Dialog Data
	//{{AFX_DATA(CUpdateWarning)
	enum { IDD = IDD_UPDATEWARNING };
	CButton	m_OKButton;
	CButton	m_CancelButton;
	CListCtrl	m_ComponentList;
	CString	m_Message;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CUpdateWarning)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	CList<CComponent*, CComponent*> mlstComps;
	CImageList m_ImageList;
	bool DependsListed(CComponent *pobjComp);
	void RemoveIncludes(CComponent *pobjComp);

	// Generated message map functions
	//{{AFX_MSG(CUpdateWarning)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_UPDATEWARNING_H__5CCD1F58_45D8_453F_BFE6_0DABB71F259F__INCLUDED_)
