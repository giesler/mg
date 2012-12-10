// AutorunDlg.h : header file
//

#if !defined(AFX_AUTORUNDLG_H__05C1D6C2_D75E_4ADE_B704_0F6A947ACACD__INCLUDED_)
#define AFX_AUTORUNDLG_H__05C1D6C2_D75E_4ADE_B704_0F6A947ACACD__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// CAutorunDlg dialog

class CAutorunDlg : public CDialog
{
// Construction
public:
	CAutorunDlg(CWnd* pParent = NULL);	// standard constructor

	void LoadButtons(CList<CDlgButton*, CDlgButton*> * mlstButtons);	// loads buttons for dialog
	CDlgButton* FindButtonById(CString id);

	CDlgButton* selectedButton;
	CList<CDlgButton*, CDlgButton*> * mlstButtons;

	CList<CStatic*, CStatic*> mlstStatics;  // used to keep track of statics
// Dialog Data
	//{{AFX_DATA(CAutorunDlg)
	enum { IDD = IDD_AUTORUN_DIALOG };
	CButton	mbtnOK;
	CButton	mbtnCancel;
	CStatic	m_pic;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAutorunDlg)
	public:
//	virtual BOOL Create(LPCTSTR lpszClassName, LPCTSTR lpszWindowName, DWORD dwStyle, const RECT& rect, CWnd* pParentWnd, UINT nID, CCreateContext* pContext = NULL);
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CAutorunDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	virtual void OnOK();
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
private:
	void PlayButtonSound(CString sound);
	CString GetINIString();
	int GetINIInt(CString strName, int intDefault);
	void LoadSettings();
	CString mstrAppName;
	CString EXEPath();
	CString GetINIString(CString strName);
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_AUTORUNDLG_H__05C1D6C2_D75E_4ADE_B704_0F6A947ACACD__INCLUDED_)
