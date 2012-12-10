// AutorunDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Autorun.h"
#include "AutorunDlg.h"
#include "util.h"

#ifdef _DEBUG
	#define new DEBUG_NEW
	#undef THIS_FILE
	static char THIS_FILE[] = __FILE__;
#endif


/////////////////////////////////////////////////////////////////////////////
// CAboutDlg dialog used for App About

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// Dialog Data
	//{{AFX_DATA(CAboutDlg)
	enum { IDD = IDD_ABOUTBOX };
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAboutDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	//{{AFX_MSG(CAboutDlg)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
	//{{AFX_DATA_INIT(CAboutDlg)
	//}}AFX_DATA_INIT
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutDlg)
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
	//{{AFX_MSG_MAP(CAboutDlg)
		// No message handlers
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// CAutorunDlg dialog

CAutorunDlg::CAutorunDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CAutorunDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CAutorunDlg)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);

	// get defaults
	mstrAppName.LoadString(IDS_DEFAULTPROGRAMNAME);
	LoadSettings();
}

void CAutorunDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAutorunDlg)
	DDX_Control(pDX, IDC_BETAPIC, m_BetaBanner);
	DDX_Control(pDX, IDC_B2, m_b2);
	DDX_Control(pDX, IDC_B1, m_b1);
	DDX_Control(pDX, IDOK, mbtnOK);
	DDX_Control(pDX, IDCANCEL, mbtnCancel);
	DDX_Control(pDX, IDC_PIC, m_pic);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CAutorunDlg, CDialog)
	//{{AFX_MSG_MAP(CAutorunDlg)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_WM_LBUTTONDOWN()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CAutorunDlg message handlers
 
BOOL CAutorunDlg::OnInitDialog()
{
 	CDialog::OnInitDialog();


	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
	
	SetWindowText(mstrAppName);
	CString strSplash;

	// check for picture
	CString strTemp;
	HBITMAP hBmp;
	strTemp = GetINIString("Splash");
	if (strTemp.GetLength() > 0) {
		strTemp = EXEPath() + strTemp;
		hBmp = (HBITMAP)::LoadImage(NULL, strTemp, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
		if (hBmp != NULL) 
			m_pic.SetBitmap(hBmp);
	}

	// Load button images
	strTemp = EXEPath() + GetINIString("InstallStandard");
	m_b1bmp = (HBITMAP)::LoadImage(NULL, strTemp, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
	strTemp = EXEPath() + GetINIString("InstallMouseOver");
	m_b1MObmp = (HBITMAP)::LoadImage(NULL, strTemp, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
	strTemp = EXEPath() + GetINIString("InstallMouseClick");
	m_b1MCbmp = (HBITMAP)::LoadImage(NULL, strTemp, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
	strTemp = EXEPath() + GetINIString("CancelStandard");
	m_b2bmp = (HBITMAP)::LoadImage(NULL, strTemp, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
	strTemp = EXEPath() + GetINIString("CancelMouseOver");
	m_b2MObmp = (HBITMAP)::LoadImage(NULL, strTemp, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
	strTemp = EXEPath() + GetINIString("CancelMouseClick");
	m_b2MCbmp = (HBITMAP)::LoadImage(NULL, strTemp, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);

	if (m_b1bmp != NULL)  m_b1.SetBitmap(m_b1bmp);
	if (m_b2bmp != NULL)  m_b2.SetBitmap(m_b2bmp);
	m_b1MO = false; m_b2MO = false;
	m_b1MC = false; m_b2MC = false;

	// Get coords set up
	CRect rect_pic;
	m_b1.GetWindowRect(&m_b1rect);
	m_b2.GetWindowRect(&m_b2rect);
	m_pic.GetWindowRect(&rect_pic);

	// adjust window position, sizes, etc.
	MoveWindow(0, 0, rect_pic.Width(), rect_pic.Height());
	int intTop = 0, intLeft = 0;
	intTop = rect_pic.Height() - m_b1rect.Height() - 40;
	intLeft = rect_pic.Width() - m_b1rect.Width() - m_b2rect.Width() - 40;
	intTop = GetINIInt("InstallTop", intTop);
	intLeft = GetINIInt("InstallLeft", intLeft);
	m_b1.MoveWindow(intLeft, intTop, m_b1rect.Width(), m_b1rect.Height());

	intLeft = rect_pic.Width() - m_b2rect.Width() - 20;
	intTop = GetINIInt("CancelTop", intTop);
	intLeft = GetINIInt("CancelLeft", intLeft);
	m_b2.MoveWindow(intLeft, intTop, m_b2rect.Width(), m_b2rect.Height());
	
	m_b1.GetWindowRect(&m_b1rect);
	m_b2.GetWindowRect(&m_b2rect);

	// adjust coords
	m_b1rect.left   = m_b1rect.left - rect_pic.left;
	m_b1rect.right  = m_b1rect.right  - rect_pic.left;
	m_b1rect.top    = m_b1rect.top  - rect_pic.top;
	m_b1rect.bottom = m_b1rect.bottom - rect_pic.top;

	m_b2rect.left   = m_b2rect.left - rect_pic.left;
	m_b2rect.right  = m_b2rect.right  - rect_pic.left;
	m_b2rect.top    = m_b2rect.top  - rect_pic.top;
	m_b2rect.bottom = m_b2rect.bottom - rect_pic.top;

	// beta stuff
	m_BetaBanner.GetWindowRect(&m_betarect);
	m_betarect.left   = m_betarect.left - rect_pic.left;
	m_betarect.right  = m_betarect.right  - rect_pic.left;
	m_betarect.top    = m_betarect.top  - rect_pic.top;
	m_betarect.bottom = m_betarect.bottom - rect_pic.top;

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL) {
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty()) {
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}
	
	return TRUE;  // return TRUE  unless you set the focus to a control
}


void CAutorunDlg::OnSysCommand(UINT nID, LPARAM lParam) {
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CAutorunDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CAutorunDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}

void CAutorunDlg::OnOK() 
{
	CDialog::OnOK();
}


CString CAutorunDlg::EXEPath()
{
	char chTemp[MAX_PATH];
	GetModuleFileName(NULL, chTemp, MAX_PATH);
	*(strrchr(chTemp, '\\')+1) = '\0';  // cut off .exe file
	return (chTemp);
}

void CAutorunDlg::OnLButtonUp(UINT nFlags, CPoint point) 
{

	if (m_b1MC) {
		m_b1MC = false;
		OnMouseMove(nFlags, point);
	} else if (m_b2MC) {
		m_b2MC = false;
		OnMouseMove(nFlags, point);
	}

	// check if install button clicked
	if (m_b1rect.left < point.x && m_b1rect.right > point.x &&
		m_b1rect.top  < point.y && m_b1rect.bottom > point.y) {
		OnOK();
	} else if (m_b2rect.left < point.x && m_b2rect.right > point.x &&
		m_b2rect.top  < point.y && m_b2rect.bottom > point.y) {
		OnCancel();
	}

	// beta - check if beta region clicked
	if (m_betarect.left < point.x && m_betarect.right > point.x &&
		m_betarect.top  < point.y && m_betarect.bottom > point.y) {
		AfxMessageBox("This program is currently in beta.  It should not be distributed.  For more details, visit http://giesler.org/vbsw.", MB_ICONINFORMATION);
	}

	CDialog::OnLButtonUp(nFlags, point);
}

void CAutorunDlg::OnMouseMove(UINT nFlags, CPoint point) 
{

	// see if within install button region
	if (m_b1MC || m_b2MC) {
	} else if (m_b1rect.left < point.x && m_b1rect.right > point.x &&
			m_b1rect.top  < point.y && m_b1rect.bottom > point.y) {
		if (!m_b1MO) {
			if (m_b1MObmp != NULL)  m_b1.SetBitmap(m_b1MObmp);
			m_b1.RedrawWindow();
			m_b1MO = true;
		} else if (m_b2MO) {
			if (m_b2bmp != NULL)  m_b2.SetBitmap(m_b2bmp);
			m_b2.RedrawWindow();
			m_b2MO = false;
		}
	// check if in cancel button region
	} else if (m_b2rect.left < point.x && m_b2rect.right > point.x &&
						 m_b2rect.top  < point.y && m_b2rect.bottom > point.y) {
		if (!m_b2MO) {
			if (m_b2MObmp != NULL)  m_b2.SetBitmap(m_b2MObmp);
			m_b2.RedrawWindow();
			m_b2MO = true;
		} else if (m_b1MO) {
			if (m_b1bmp != NULL)  m_b1.SetBitmap(m_b1bmp);
			m_b1.RedrawWindow();
			m_b1MO = false;
		}
	} else {
		// see if we were in install and now are out
		if (m_b1MO) {
			if (m_b1bmp != NULL)  m_b1.SetBitmap(m_b1bmp);
			m_b1.RedrawWindow();
			m_b1MO = false;
		}
		// see if we were in cancel and now are out
		if (m_b2MO) {
			if (m_b2bmp != NULL)  m_b2.SetBitmap(m_b2bmp);
			m_b2.RedrawWindow();
			m_b2MO = false;
		}
	}

	CDialog::OnMouseMove(nFlags, point);
}

void CAutorunDlg::OnLButtonDown(UINT nFlags, CPoint point) 
{

	// see if within install button region
	if (m_b1rect.left < point.x && m_b1rect.right > point.x &&
			m_b1rect.top  < point.y && m_b1rect.bottom > point.y) {
		if (!m_b1MC) {
			if (m_b1MCbmp != NULL)  m_b1.SetBitmap(m_b1MCbmp);
			m_b1.RedrawWindow();
			m_b1MC = true;
		}
	// check if in cancel button region
	} else if (m_b2rect.left < point.x && m_b2rect.right > point.x &&
						 m_b2rect.top  < point.y && m_b2rect.bottom > point.y) {
		if (!m_b2MC) {
			if (m_b2MCbmp != NULL)  m_b2.SetBitmap(m_b2MCbmp);
			m_b2.RedrawWindow();
			m_b2MC = true;
		}
	} else if (m_b1MC) {
		m_b1MC = false;
	} else if (m_b2MC) {
		m_b2MC = false;
	}
	
	CDialog::OnLButtonDown(nFlags, point);
}


void CAutorunDlg::LoadSettings()
{

	TCHAR * lpReturnedString; 
	lpReturnedString = (TCHAR*) malloc(1000);

	// Start by getting basic settings
	GetPrivateProfileString("Settings", "ProgramName", mstrAppName, lpReturnedString, 255, gUtils.EXEPath() + "vbsw\\settings.ini");
	mstrAppName = lpReturnedString;

		// Free allocated strings
	free(lpReturnedString);
}

CString CAutorunDlg::GetINIString(CString strName)
{

	CString strReturn = "";
	TCHAR * lpReturnedString; 
	lpReturnedString = (TCHAR*) malloc(1000);

	// Start by getting basic settings
	GetPrivateProfileString("Settings", strName, strReturn, lpReturnedString, 255, gUtils.EXEPath() + "vbsw\\settings.ini");
	strReturn = lpReturnedString;

		// Free allocated strings
	free(lpReturnedString);

	return strReturn;

}


int CAutorunDlg::GetINIInt(CString strName, int intDefault)
{		
	return (GetPrivateProfileInt("Settings", strName, intDefault, gUtils.EXEPath() + "vbsw\\settings.ini"));
}
