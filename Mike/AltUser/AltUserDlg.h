// AltUserDlg.h : header file
//

#if !defined(AFX_ALTUSERDLG_H__84A2C9B8_1F3D_413B_8D38_F5E1AAA1475F__INCLUDED_)
#define AFX_ALTUSERDLG_H__84A2C9B8_1F3D_413B_8D38_F5E1AAA1475F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// CAltUserDlg dialog

class CAltUserDlg : public CDialog
{
// Construction
public:
	CAltUserDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(CAltUserDlg)
	enum { IDD = IDD_ALTUSER_DIALOG };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAltUserDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CAltUserDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ALTUSERDLG_H__84A2C9B8_1F3D_413B_8D38_F5E1AAA1475F__INCLUDED_)
