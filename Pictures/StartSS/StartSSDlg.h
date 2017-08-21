// StartSSDlg.h : header file
//

#if !defined(AFX_STARTSSDLG_H__CB435D0D_D1FD_4747_9D33_59AAC837736B__INCLUDED_)
#define AFX_STARTSSDLG_H__CB435D0D_D1FD_4747_9D33_59AAC837736B__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// CStartSSDlg dialog

class CStartSSDlg : public CDialog
{
// Construction
public:
	CStartSSDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(CStartSSDlg)
	enum { IDD = IDD_STARTSS_DIALOG };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CStartSSDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CStartSSDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STARTSSDLG_H__CB435D0D_D1FD_4747_9D33_59AAC837736B__INCLUDED_)
