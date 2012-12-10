#if !defined(AFX_SETUPDLG_H__7B503A55_4714_4ED1_911B_23731DF50406__INCLUDED_)
#define AFX_SETUPDLG_H__7B503A55_4714_4ED1_911B_23731DF50406__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// SetupDlg.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CSetupDlg dialog

class CSetupDlg : public CDialog
{
// Construction
public:
	CString m_strAppName;
	void StartTimer(int intMaxProgress);
	void StopTimer();

	bool m_blnCancel;
	CSetupDlg(CWnd* pParent = NULL);   // standard constructor
	
	void SetMaxProgress(int);
	void Progress();

// Dialog Data
	//{{AFX_DATA(CSetupDlg)
	enum { IDD = IDD_SETUP };
	CButton	m_btnCancel;
	CProgressCtrl	m_PBar;
	CString	m_CurStatus;
	CString	m_SetupDlgMsg;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSetupDlg)
	public:
	virtual BOOL Create(CWnd*);
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CSetupDlg)
	afx_msg void OnCancel();
	virtual BOOL OnInitDialog();
	afx_msg void OnTimer(UINT nIDEvent);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:
	int m_intMaxProgress;
	int m_intCurProgress;
	int m_nTimer;

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SETUPDLG_H__7B503A55_4714_4ED1_911B_23731DF50406__INCLUDED_)
