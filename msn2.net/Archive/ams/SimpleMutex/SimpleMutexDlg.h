// SimpleMutexDlg.h : header file
//

#if !defined(AFX_SIMPLEMUTEXDLG_H__E869CD13_152E_44D3_BCF9_E0FA349096B3__INCLUDED_)
#define AFX_SIMPLEMUTEXDLG_H__E869CD13_152E_44D3_BCF9_E0FA349096B3__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// CSimpleMutexDlg dialog

class CSimpleMutexDlg : public CDialog
{
// Construction
public:
	CSimpleMutexDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(CSimpleMutexDlg)
	enum { IDD = IDD_SIMPLEMUTEX_DIALOG };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSimpleMutexDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CSimpleMutexDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SIMPLEMUTEXDLG_H__E869CD13_152E_44D3_BCF9_E0FA349096B3__INCLUDED_)
