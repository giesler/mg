// RestartDlg.cpp : implementation file
//

#include "stdafx.h"
#include "autorun.h"
#include "RestartDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CRestartDlg dialog


CRestartDlg::CRestartDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CRestartDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CRestartDlg)
	m_strMessage = _T("");
	//}}AFX_DATA_INIT
	m_strMessage.LoadString(IDS_REBOOTMSG);
	m_strAppName = "Autorun";
}


void CRestartDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CRestartDlg)
	DDX_Control(pDX, IDC_PBAR, m_PBar);
	DDX_Text(pDX, IDC_MESSAGE, m_strMessage);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CRestartDlg, CDialog)
	//{{AFX_MSG_MAP(CRestartDlg)
	ON_WM_TIMER()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CRestartDlg message handlers

void CRestartDlg::OnTimer(UINT nIDEvent) 
{
	
	if (m_intCurProgress < m_intMaxProgress) {
		m_PBar.StepIt();
		m_intCurProgress++;
	} else {
		OnOK();
		KillTimer(m_nTimer);
	}
	CDialog::OnTimer(nIDEvent);
}

BOOL CRestartDlg::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	if (mblnTimerReboot) {
		m_intMaxProgress = mintTimerSeconds * 10;
		m_intCurProgress = 0;
		m_PBar.SetRange(0, m_intMaxProgress);
		m_PBar.SetStep(1);
		m_nTimer = SetTimer(0, 100, 0);
	} else {
		m_PBar.MoveWindow(500, 500, 0, 0, false);
	}

	SetWindowText(m_strAppName);

	return true;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}
