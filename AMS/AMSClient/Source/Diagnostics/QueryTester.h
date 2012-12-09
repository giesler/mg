#if !defined(AFX_QUERYTESTER_H__CECA4F33_B3A2_4DEB_BF93_0AE78E4515CC__INCLUDED_)
#define AFX_QUERYTESTER_H__CECA4F33_B3A2_4DEB_BF93_0AE78E4515CC__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// QueryTester.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CQueryTester dialog

class CQueryTester : public CDialog
{
// Construction
public:
	CQueryTester(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CQueryTester)
	enum { IDD = IDD_QUERYTEST };
	CStatic	mStatus;
	CListBox	mResults;
	CStatic	mPreview;
	CListBox	mHosts;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CQueryTester)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	//misc
	void ClearUI();

	//Session Messages
	afx_msg LRESULT OnClientMessage(WPARAM wParam, LPARAM lParam);
	afx_msg LRESULT OnServerMessage(WPARAM wParam, LPARAM lParam);

	// Generated message map functions
	//{{AFX_MSG(CQueryTester)
	afx_msg void OnRunquery();
	afx_msg void OnSelchangeResults();
	afx_msg void OnQuery();
	afx_msg void OnGetfile();
	afx_msg void OnGetthumb();
	virtual BOOL OnInitDialog();
	afx_msg void OnDestroy();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_QUERYTESTER_H__CECA4F33_B3A2_4DEB_BF93_0AE78E4515CC__INCLUDED_)
