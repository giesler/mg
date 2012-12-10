#if !defined(AFX_RESTARTDLG_H__FA2A9269_7C1C_42F2_9408_3F5EBAED4886__INCLUDED_)
#define AFX_RESTARTDLG_H__FA2A9269_7C1C_42F2_9408_3F5EBAED4886__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// RestartDlg.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CRestartDlg dialog

class CRestartDlg : public CDialog
{
// Construction
public:
	CRestartDlg(CWnd* pParent = NULL);   // standard constructor
	CString m_strAppName;
	bool mblnTimerReboot;
	int mintTimerSeconds;

// Dialog Data
	//{{AFX_DATA(CRestartDlg)
	enum { IDD = IDD_RESTART };
	CProgressCtrl	m_PBar;
	CString	m_strMessage;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CRestartDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CRestartDlg)
	afx_msg void OnTimer(UINT nIDEvent);
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:
	int m_intMaxProgress;
	int m_intCurProgress;
	int m_nTimer;

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_RESTARTDLG_H__FA2A9269_7C1C_42F2_9408_3F5EBAED4886__INCLUDED_)
