// SetupDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Autorun.h"
#include "SetupDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CSetupDlg dialog


CSetupDlg::CSetupDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CSetupDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CSetupDlg)
	m_CurStatus = _T("");
	m_SetupDlgMsg = _T("");
	//}}AFX_DATA_INIT

	CString s;
	s.LoadString(IDS_CANCEL);
	m_btnCancel.SetWindowText(s);
}


void CSetupDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CSetupDlg)
	DDX_Control(pDX, IDC_CANCEL, m_btnCancel);
	DDX_Control(pDX, IDC_PROGRESS1, m_PBar);
	DDX_Text(pDX, IDC_CURSTATUS, m_CurStatus);
	DDX_Text(pDX, IDC_SETUPDLG_MSG, m_SetupDlgMsg);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CSetupDlg, CDialog)
	//{{AFX_MSG_MAP(CSetupDlg)
	ON_BN_CLICKED(IDC_CANCEL, OnCancel)
	ON_WM_TIMER()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CSetupDlg message handlers

BOOL CSetupDlg::Create(CWnd* pParentWnd) 
{
	// Add your specialized code here and/or call the base class

	// the getdesktopwindow will make the dialog have a taskbar icon
	return CDialog::Create(IDD, CWnd::GetDesktopWindow());
}


void CSetupDlg::SetMaxProgress(int iMax) {
	m_PBar.SetRange(0, iMax);
	m_PBar.SetStep(1);
}


void CSetupDlg::Progress() {
	m_PBar.StepIt();
}


void CSetupDlg::OnCancel() 
{
	CString strMessage;
	strMessage.LoadString(IDS_SETUPDLG_CANCEL);

	if (MessageBox(strMessage, m_strAppName, MB_YESNO | MB_ICONQUESTION) == IDYES) 
	{
		m_btnCancel.EnableWindow(false);
		m_blnCancel = true;
		m_CurStatus.LoadString(IDS_SETUPDLG_CANCELMSG);
		UpdateData(false);
	}
}


BOOL CSetupDlg::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	SetWindowText(m_strAppName);
	m_SetupDlgMsg.LoadString(IDS_SETUPDLG_MSG);
	m_CurStatus.LoadString(IDS_SETUPDLG_WAIT);
	m_blnCancel = false;
	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}


void CSetupDlg::OnTimer(UINT nIDEvent) 
{
	// advance progress bar if less than max for this item
	if (m_intMaxProgress > m_intCurProgress) 
	{
		m_PBar.StepIt();
		m_intCurProgress++;
	} 
	else 
	{
		StopTimer();
	}
	
	CDialog::OnTimer(nIDEvent);
}

void CSetupDlg::StartTimer(int intMaxProgress)
{
	m_intMaxProgress = intMaxProgress;
	m_intCurProgress = 0;

	m_nTimer = SetTimer(0, 500, 0);
}

void CSetupDlg::StopTimer()
{
	// kill timer if not already killed
	if (m_nTimer != 0) 
	{
		KillTimer(m_nTimer);
		m_nTimer = 0;
	}

	// finish advancing progress bar if needed
	if (m_intMaxProgress > m_intCurProgress) 
	{
		m_PBar.StepIt();
		m_PBar.UpdateWindow();
		m_intCurProgress++;
	}

}
