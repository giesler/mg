#if !defined(AFX_PROMPT_H__A97E410E_F51A_42D8_BFA7_CEEC15EBCA61__INCLUDED_)
#define AFX_PROMPT_H__A97E410E_F51A_42D8_BFA7_CEEC15EBCA61__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Prompt.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CPrompt dialog

class CPrompt : public CDialog
{
// Construction
public:
	CPrompt(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CPrompt)
	enum { IDD = ID_TEXTPROMPT };
	CString	m_inputbox;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPrompt)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CPrompt)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_PROMPT_H__A97E410E_F51A_42D8_BFA7_CEEC15EBCA61__INCLUDED_)
