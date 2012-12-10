// AutorunDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Autorun.h"
#include "AutorunDlg.h"
#include "util.h"
#include "winsock2.h"
#include "windows.h"
#include "mmsystem.h"

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
	selectedButton = NULL;
}

void CAutorunDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAutorunDlg)
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

	// check for splash picture
	CString strTemp;
	HBITMAP hBmp;
	strTemp = GetINIString("Splash");
	if (strTemp.GetLength() > 0) {
		strTemp = EXEPath() + strTemp;
		hBmp = (HBITMAP)::LoadImage(NULL, strTemp, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
		if (hBmp != NULL) 
			m_pic.SetBitmap(hBmp);
	}

	// Get coords set up
	CRect rect_pic;
	m_pic.GetWindowRect(&rect_pic);

	// adjust window position, sizes, etc.
	MoveWindow(0, 0, rect_pic.Width(), rect_pic.Height());

	// Add "About..." menu item to system menu.
	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CDlgButton * pobjDlgButton;

	int i; 
	POSITION pos = mlstButtons->GetHeadPosition();
	for (i = 0; i < mlstButtons->GetCount(); i++) {
		pobjDlgButton = mlstButtons->GetNext(pos);

		// Create the button and add it to the dialog
		CStatic * button;
		button = new CStatic();
		button->Create(pobjDlgButton->mstrId, SS_BITMAP | WS_GROUP | WS_TABSTOP, 
			CRect(20,20,150,50), this, 0);

		// Create the bitmap and set it for the button
		if (pobjDlgButton->mbmpStandard != NULL)
			button->SetBitmap(pobjDlgButton->mbmpStandard);

		// Get the image size back
		button->GetWindowRect(& (pobjDlgButton->mrect) );

		// Now position correctly
		button->MoveWindow(pobjDlgButton->mintLeft, pobjDlgButton->mintTop, pobjDlgButton->mrect.Width(), pobjDlgButton->mrect.Height());
		button->ShowWindow(TRUE);

		// Adjust coords
		button->GetWindowRect(& (pobjDlgButton->mrect) );
		pobjDlgButton->mrect.left	= pobjDlgButton->mrect.left - rect_pic.left;
		pobjDlgButton->mrect.right	= pobjDlgButton->mrect.right  - rect_pic.left;
		pobjDlgButton->mrect.top	= pobjDlgButton->mrect.top - rect_pic.top;
		pobjDlgButton->mrect.bottom	= pobjDlgButton->mrect.bottom - rect_pic.top;

		// Save it
		pobjDlgButton->mstatic = button;

		gLog.LogEvent("Initialized dialog");
//		mlstStatics.AddTail(button);
	}

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
	
	CDlgButton * pobjDlgButton;
	POSITION pos;
	
	// Check if any buttons are in the clicked state, if so, change to normal state
	pos = mlstButtons->GetHeadPosition();
	for (int i = 0; i < mlstButtons->GetCount(); i++) {

		pobjDlgButton = mlstButtons->GetNext(pos);
		if (pobjDlgButton->mblnMouseClick) 
		{
			PlayButtonSound(pobjDlgButton->mstrMouseUp);
			
			// We were in clicked state here, so check for an action
			if (pobjDlgButton->mblnCancel)
			{
				OnCancel();
			} else {
				selectedButton = pobjDlgButton;
				OnOK();
			}

			pobjDlgButton->mblnMouseClick = false;
			OnMouseMove(nFlags, point);
		}
	}

	CDialog::OnLButtonUp(nFlags, point);
}

void CAutorunDlg::OnMouseMove(UINT nFlags, CPoint point) 
{
	CDlgButton * pobjDlgButton;
	POSITION pos;
	
	// Bail if any button is in 'click' state
	pos = mlstButtons->GetHeadPosition();
	for (int i = 0; i < mlstButtons->GetCount(); i++) {
		pobjDlgButton = mlstButtons->GetNext(pos);
		if (pobjDlgButton->mblnMouseClick)
			return;
	}

	// Loop through the button list
	pos = mlstButtons->GetHeadPosition();
	for (i = 0; i < mlstButtons->GetCount(); i++) {
		pobjDlgButton = mlstButtons->GetNext(pos);
		CRect rect = pobjDlgButton->mrect;

		// Check if we are within the region of current button
		if (rect.left < point.x && rect.right  > point.x &&
			rect.top  < point.y && rect.bottom > point.y)
		{
			if (!pobjDlgButton->mblnMouseOver) 
			{
				if (pobjDlgButton->mbmpMouseOver != NULL)
					pobjDlgButton->mstatic->SetBitmap(pobjDlgButton->mbmpMouseOver);
				pobjDlgButton->mstatic->RedrawWindow();
				pobjDlgButton->mblnMouseOver = true;

				PlayButtonSound(pobjDlgButton->mstrMouseEnter);
			}
		}
		// Since we aren't in region, make sure we weren't in it before
		else if (pobjDlgButton->mblnMouseOver) 
		{
			// Change back to standard image
			if (pobjDlgButton->mbmpStandard != NULL)
				pobjDlgButton->mstatic->SetBitmap(pobjDlgButton->mbmpStandard);
			pobjDlgButton->mstatic->RedrawWindow();
			pobjDlgButton->mblnMouseOver = false;

			PlayButtonSound(pobjDlgButton->mstrMouseExit);
		}

	}

	CDialog::OnMouseMove(nFlags, point);
}

void CAutorunDlg::OnLButtonDown(UINT nFlags, CPoint point) 
{

	CDlgButton * pobjDlgButton;
	POSITION pos;
	
	// Loop through the button list
	pos = mlstButtons->GetHeadPosition();
	for (int i = 0; i < mlstButtons->GetCount(); i++) {
		pobjDlgButton = mlstButtons->GetNext(pos);
		CRect rect = pobjDlgButton->mrect;

		// Check if we are within the region of current button
		if (rect.left < point.x && rect.right  > point.x &&
			rect.top  < point.y && rect.bottom > point.y)
		{
			if (pobjDlgButton->mbmpMouseClick != NULL)
				pobjDlgButton->mstatic->SetBitmap(pobjDlgButton->mbmpMouseClick);
			pobjDlgButton->mstatic->RedrawWindow();
			pobjDlgButton->mblnMouseClick = true;

			PlayButtonSound(pobjDlgButton->mstrMouseDown);
		}
		// Since we aren't in region, make sure we weren't in it before
		else if (pobjDlgButton->mblnMouseClick) 
		{
			pobjDlgButton->mblnMouseClick = false;
		}

	}
	
	CDialog::OnLButtonDown(nFlags, point);
}


void CAutorunDlg::LoadSettings()
{

	TCHAR * lpReturnedString; 
	lpReturnedString = (TCHAR*) malloc(1000);

	// Start by getting basic settings
	GetPrivateProfileString("Settings", "ProgramName", mstrAppName, lpReturnedString, 255, gUtils.EXEPath() + "ia\\settings.ini");
	mstrAppName = lpReturnedString;

	// Free allocated strings
	free(lpReturnedString);
}

// Loads the images and info for the buttons
void CAutorunDlg::LoadButtons(CList<CDlgButton*, CDlgButton*> * lstButtons)
{	
	mlstButtons = lstButtons;
}


CString CAutorunDlg::GetINIString(CString strName)
{

	CString strReturn = "";
	TCHAR * lpReturnedString; 
	lpReturnedString = (TCHAR*) malloc(1000);

	// Start by getting basic settings
	GetPrivateProfileString("Settings", strName, strReturn, lpReturnedString, 255, gUtils.EXEPath() + "ia\\settings.ini");
	strReturn = lpReturnedString;

		// Free allocated strings
	free(lpReturnedString);

	return strReturn;

}


int CAutorunDlg::GetINIInt(CString strName, int intDefault)
{		
	return (GetPrivateProfileInt("Settings", strName, intDefault, gUtils.EXEPath() + "ia\\settings.ini"));
}

void CAutorunDlg::PlayButtonSound(CString sound) 
{
	if (!sound.IsEmpty())
		PlaySound(gUtils.EXEPath() + sound, 0, SND_FILENAME | SND_ASYNC | SND_NOWAIT);
}

CDlgButton* CAutorunDlg::FindButtonById(CString id)
{

	CDlgButton * pobjDlgButton;
	POSITION pos;
	
	// Loop through the button list
	pos = mlstButtons->GetHeadPosition();
	for (int i = 0; i < mlstButtons->GetCount(); i++) {
		pobjDlgButton = mlstButtons->GetNext(pos);
		
		if (pobjDlgButton->mstrId == id)
			return pobjDlgButton;

	}

	gLog.LogEvent("Unable to find button " + id);
	return NULL;
}

CDlgButton* CAutorunDlg::FindDefaultButton()
{

	CDlgButton * pobjDlgButton;
	POSITION pos;
	
	// Loop through the button list
	pos = mlstButtons->GetHeadPosition();
	for (int i = 0; i < mlstButtons->GetCount(); i++) {
		pobjDlgButton = mlstButtons->GetNext(pos);
		
		if (pobjDlgButton->mblnDefault)
			return pobjDlgButton;

	}

	gLog.LogEvent("Unable to find default button");
	return NULL;
}