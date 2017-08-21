// AutorunDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Autorun.h"
#include "AutorunDlg.h"
#include "util.h"
#include "winsock2.h"
#include "windows.h"
#include "Hyperlink.h"
#include "RestartDlg.h"

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
	CStatic	m_LinkControl;
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
	DDX_Control(pDX, IDC_HYPERLINK, m_LinkControl);
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

	// get settings
	mstrAppName.LoadString(IDS_DEFAULTPROGRAMNAME);
	mstrAppName		= gUtils.GetINIString("Settings", "ProgramName", mstrAppName);
	hideTitlebar	= gUtils.GetINIBool("Settings", "HideTitleBar", false);
	onOpenSoundPlayed = false;


	// Load buttons
	CString buttonList[20][2];
	int buttonCount;

	buttonCount = gUtils.GetINISection("Buttons", buttonList);

	// Loop through list, adding enabled buttons
	for (int i = 0; i < buttonCount; i++) 
	{
		if (buttonList[i][1] == "1") 
		{
			CDlgButton * button = new CDlgButton();
			if (button->Load(buttonList[i][0]))
				mlstButtons.AddTail(button);
		}
	}

	setupOnlyMode = false;
}


CAutorunDlg::~CAutorunDlg() {

	// delete buttons
	CStatic * pButton;
	while (mlstStatics.GetCount() > 0) 
	{
		pButton = mlstStatics.RemoveHead();
		delete pButton;
	}

	// delete whole list since we are done with it
	CComponent * pobjComp;
	while (mlstComps.GetCount() > 0) 
	{
		pobjComp = mlstComps.RemoveHead();
		delete pobjComp;
	}

	// delete buttons too
	CDlgButton * pobjButton;
	while (mlstButtons.GetCount() > 0) 
	{
		pobjButton = mlstButtons.RemoveHead();
		delete pobjButton;
	}

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
	ON_WM_SHOWWINDOW()
	ON_WM_SETCURSOR()
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
	
	// Add "About..." menu item to system menu.
	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL) 
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty()) 
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// bail now if in setup mode
	if (setupOnlyMode)
	{
		return true;
	}

	SetWindowText(mstrAppName);
	CString strSplash;

	// check for splash picture
	CString strTemp;
	HBITMAP hBmp;
	strTemp = gUtils.GetINIString("Settings", "Splash", "");
	hBmp = (HBITMAP)::LoadImage(NULL, strTemp, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
	if (hBmp != NULL) 
		m_pic.SetBitmap(hBmp);

	// Get coords set up
	CRect rect_pic;
	m_pic.GetWindowRect(&rect_pic);

	// adjust window position, sizes, etc.
	MoveWindow(0, 0, rect_pic.Width(), rect_pic.Height());

	CDlgButton * pobjDlgButton;

	// Loop through list of buttons to add them
	int i; 
	POSITION pos = mlstButtons.GetHeadPosition();
	for (i = 0; i < mlstButtons.GetCount(); i++) 
	{
		pobjDlgButton = mlstButtons.GetNext(pos);

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

		// Keep it for deleting on destruct
		mlstStatics.AddTail(button);
	}

	// Check if we want to hide the titlebar
	if (hideTitlebar) 
	{
		ModifyStyle(WS_CAPTION, 0);		// hide title
		ModifyStyleEx(WS_EX_DLGMODALFRAME, 0);  // hide frame
	}

	// Play the open sound if not in setup mode
	if (!onOpenSoundPlayed && !setupOnlyMode) 
	{
		onOpenSoundPlayed = true;
 		gUtils.PlaySoundFile(gUtils.GetINIString("Settings", "OnOpenSound", ""));
	}

	gLog.LogEvent("Initialized dialog");

	return TRUE;  // return TRUE  unless you set the focus to a control
}


void CAutorunDlg::OnSysCommand(UINT nID, LPARAM lParam) 
{
	// Check if we want to display the About box
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)	
	{
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
	PerformButtonAction(FindDefaultButton());
	//CDialog::OnOK();
}


void CAutorunDlg::OnCancel()
{
	CloseDialog();
	CDialog::OnCancel();
}


void CAutorunDlg::OnLButtonUp(UINT nFlags, CPoint point) 
{
	
	CDlgButton * pobjDlgButton;
	POSITION pos;
	
	// Check if any buttons are in the clicked state, if so, change to normal state
	pos = mlstButtons.GetHeadPosition();
	for (int i = 0; i < mlstButtons.GetCount(); i++) 
	{

		pobjDlgButton = mlstButtons.GetNext(pos);
		if (pobjDlgButton->mblnMouseClick) 
		{
			gUtils.PlaySoundFile(pobjDlgButton->mstrMouseUp);
			
			// We were in clicked state here, so check for an action
			if (pobjDlgButton->mblnCancel)
			{
				CloseDialog();
				return;
			} 
			else 
			{
				PerformButtonAction(pobjDlgButton);
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
	pos = mlstButtons.GetHeadPosition();
	for (int i = 0; i < mlstButtons.GetCount(); i++) 
	{
		pobjDlgButton = mlstButtons.GetNext(pos);
		if (pobjDlgButton->mblnMouseClick)
			return;
	}

	// Loop through the button list
	pos = mlstButtons.GetHeadPosition();
	for (i = 0; i < mlstButtons.GetCount(); i++) 
	{
		pobjDlgButton = mlstButtons.GetNext(pos);
		CRect rect = pobjDlgButton->mrect;

		// Check if we are within the region of current button
		if (rect.left < point.x && rect.right  > point.x &&
			rect.top  < point.y && rect.bottom > point.y)
		{
			if (!pobjDlgButton->mblnMouseOver) 
			{
				if (pobjDlgButton->mbmpMouseOver != NULL)
				{
					pobjDlgButton->mstatic->SetBitmap(pobjDlgButton->mbmpMouseOver);
					pobjDlgButton->mstatic->Invalidate();
				}
				pobjDlgButton->mblnMouseOver = true;

				TRACE("mouseover currently: %s\n", pobjDlgButton->mstrId);

				if (pobjDlgButton->mhMouseCursor != NULL)
					SetCursor(pobjDlgButton->mhMouseCursor);

				gUtils.PlaySoundFile(pobjDlgButton->mstrMouseEnter);
			}
		}
		// Since we aren't in region, make sure we weren't in it before
		else if (pobjDlgButton->mblnMouseOver) 
		{
			// Change back to standard image
			if (pobjDlgButton->mbmpStandard != NULL)
			{
				pobjDlgButton->mstatic->SetBitmap(pobjDlgButton->mbmpStandard);
				pobjDlgButton->mstatic->Invalidate();
			}
			pobjDlgButton->mblnMouseOver = false;

			TRACE("mouseover not: %s\n", pobjDlgButton->mstrId);

			// The OnSetCursor function will handle setting this correctly
			//SetCursor(LoadCursor(NULL, IDC_ARROW));

			gUtils.PlaySoundFile(pobjDlgButton->mstrMouseExit);
		}

	}

	CDialog::OnMouseMove(nFlags, point);
}

void CAutorunDlg::OnLButtonDown(UINT nFlags, CPoint point) 
{

	CDlgButton * pobjDlgButton;
	POSITION pos;
	
	// Loop through the button list
	pos = mlstButtons.GetHeadPosition();
	for (int i = 0; i < mlstButtons.GetCount(); i++) 
	{
		pobjDlgButton = mlstButtons.GetNext(pos);
		CRect rect = pobjDlgButton->mrect;

		// Check if we are within the region of current button
		if (rect.left < point.x && rect.right  > point.x &&
			rect.top  < point.y && rect.bottom > point.y)
		{
			if (pobjDlgButton->mbmpMouseClick != NULL)
				pobjDlgButton->mstatic->SetBitmap(pobjDlgButton->mbmpMouseClick);
			pobjDlgButton->mstatic->Invalidate();
			pobjDlgButton->mblnMouseClick = true;

			gUtils.PlaySoundFile(pobjDlgButton->mstrMouseDown);
		}
		// Since we aren't in region, make sure we weren't in it before
		else if (pobjDlgButton->mblnMouseClick) 
		{
			pobjDlgButton->mblnMouseClick = false;
		}

	}
	
	CDialog::OnLButtonDown(nFlags, point);
}



CDlgButton* CAutorunDlg::FindButtonById(CString id)
{

	CDlgButton * pobjDlgButton;
	POSITION pos;
	
	// Loop through the button list
	pos = mlstButtons.GetHeadPosition();
	for (int i = 0; i < mlstButtons.GetCount(); i++) 
	{
		pobjDlgButton = mlstButtons.GetNext(pos);
		
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
	pos = mlstButtons.GetHeadPosition();
	for (int i = 0; i < mlstButtons.GetCount(); i++) 
	{
		pobjDlgButton = mlstButtons.GetNext(pos);
		
		if (pobjDlgButton->mblnDefault)
			return pobjDlgButton;

	}

	gLog.LogEvent("Unable to find default button");
	return NULL;
}

BOOL CAutorunDlg::OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message) 
{
	
	CDlgButton * pobjDlgButton;
	POSITION pos;
	
	// check if we are over a button, if so bail

	// Loop through the button list
	pos = mlstButtons.GetHeadPosition();
	for (int i = 0; i < mlstButtons.GetCount(); i++) 
	{
		pobjDlgButton = mlstButtons.GetNext(pos);
	
		if (pobjDlgButton->mblnMouseOver)
			return true;

	}
	
	return CDialog::OnSetCursor(pWnd, nHitTest, message);
}

void CAutorunDlg::CloseDialog() 
{
	running = false;
	ShowWindow(SW_HIDE);
}

void CAutorunDlg::PerformButtonAction(CDlgButton* button)
{

	// If no default button, abort
	if (button == NULL)
		return;

	gLog.LogEvent("Performing button action for button: " + button->mstrName);

	// Check for cancel button
	if (button->mtypDlgButtonType == DlgButtonTypeCancel) 
	{
		CloseDialog();
		return;
	}

	// See if we need to do system updates
	if (button->mblnComponentCheck) 
	{
			
		// check what is installed
		if (SysUpdates()) 
		{

			// Hide the splash if visible
			ShowWindow(SW_HIDE);
	
			// Try installing components
			if (!InstallComponents()) 
			{
					
				// check if user clicked cancel - if not, reboot needed
				if (m_blnComponentCancel) 
				{
					gLog.LogEvent("Setup cancelled, application exiting.");
					CloseDialog();
					return;
				} 
				else if (m_blnComponentRebootComputer) 
				{
					RebootComputer("/continue " + button->mstrId); 
					CloseDialog();
					return;
				} 
				else 
				{
					// InstallComponents returned false, and we didn't cancel, so we must reboot
					gLog.LogEvent("SysUpdates returned false, possible error.");
				}
			}

			// Any system updates have now been installed
			gLog.LogEvent("All system updates installed.");

		}
	
	}	// done mblnComponentCheck


	// Perform button action
	CString sSetupCommand;
	bool async = (button->mtypDialogAction == DialogActionShowWhenActionComplete);

	if (!async)
 		gUtils.PlaySoundFile(gUtils.GetINIString("Settings", "OnCloseSound", ""));

	switch (button->mtypDlgButtonType) 
	{

		case DlgButtonTypeRunProgram:
				
			// replace any sys path, vars
			gUtils.ReplaceDirs(button->mstrSetupCommand);
			gUtils.ReplaceDirs(button->mstrSetupCommandLine);

			// check if file exists without adding exe path
			if (gUtils.FileExists(button->mstrSetupCommand)) 
			{
				// start the program
				gUtils.Exec(button->mstrSetupCommand, button->mstrSetupCommandLine, async, false);
			} 
			else 
			{
				// start the program
				gUtils.Exec(gUtils.EXEPath() + button->mstrSetupCommand, button->mstrSetupCommandLine, async, false);
			}

			// Check if we should restart
			if (button->mblnRestartPrompt) 
			{
				RebootComputer("/dialog " + button->mstrId);
				CloseDialog();
				return;
			}

			// Check if setup only mode
			if (setupOnlyMode) 
			{
				CloseDialog();
				return;
			}

			break;

		case DlgButtonTypeLaunchUrl:
			ShellExecute(NULL, "open", button->mstrUrl, NULL, NULL, SW_SHOWNORMAL);
			break;

		case DlgButtonTypeShellExecute:
			if (gUtils.FileExists(button->mstrFile)) 
			{
				ShellExecute(NULL, "open", button->mstrFile, NULL, NULL, SW_SHOWNORMAL);
			} 
			else if (gUtils.FileExists(gUtils.EXEPath() + button->mstrFile)) 
			{
				ShellExecute(NULL, "open", gUtils.EXEPath() + button->mstrFile, NULL, NULL, SW_SHOWNORMAL);
			} 
			else 
			{
				gLog.LogEvent("The file '" + button->mstrFile + "' could not be found.");
				MessageBox("The file '" + button->mstrFile + "' could not be found.", mstrAppName, MB_ICONERROR);
			}
			break;
		default:
			gLog.LogEvent("Unknown button type.");
			break;
	}

	// See what next behavior should be
	if (button->mtypDialogAction == DialogActionDoNotShowDialog || setupOnlyMode) 
	{
		CloseDialog();
		return;
	}

	// Make sure window is redrawn
	ShowWindow(SW_SHOW);
	ResetButtons();
//	Invalidate();

}




/////////////////////////////////////
// functions that used to be in Init
/////////////////////////////////////


// Process components in settings.ini, check for req'd components
bool CAutorunDlg::SysUpdates() {

	int intResult = 0;

	int i; CComponent * pobjComp;
	CString componentList[20][2];
	int componentCount;

	// get a string just to be safe before getting the INI section (Win95/98 bug)
	gUtils.GetINIString("Settings", "AppName", "WhoCares");

	componentCount = gUtils.GetINISection("Components", componentList);

	// Loop through list, adding enabled components
	for (i = 0; i < componentCount; i++) 
	{
		if (componentList[i][1] == "1") 
		{
			pobjComp = new CComponent();
			if (pobjComp->Load(componentList[i][0]))
				mlstComps.AddTail(pobjComp);
		}
	}


	// now go though components
	POSITION pos = mlstComps.GetHeadPosition();
	for (i = 0; i < mlstComps.GetCount(); i++) 
	{
		pobjComp = mlstComps.GetNext(pos);

		// see if installed
		if (pobjComp->mblnInstall) 
		{
			pobjComp->CheckComponent();
			if (pobjComp->IsComponentInstalled()) 
			{
				// component is installed, flag to not install
				pobjComp->mblnInstall = false;
			}
		}
	}

	gLog.LogEvent("Components all checked, evaluating whether to install...");

	// look through list to see if we attempted to install any of them
	CString strCompMsg;
	pos = mlstComps.GetHeadPosition();
	for (i = 0; i < mlstComps.GetCount(); i++) 
	{
		pobjComp = mlstComps.GetNext(pos);
		if (pobjComp->mblnInstall && pobjComp->InstallAttempted()) 
		{
			// we have tried to install this component already
			gLog.LogEvent(pobjComp->mstrName + ": Component install already attempted.  Prompting user.");
			strCompMsg.Format(IDS_COMPABORTRETRY, pobjComp->mstrName);
			intResult = MessageBox(strCompMsg, mstrAppName, MB_ABORTRETRYIGNORE | MB_ICONQUESTION | MB_TASKMODAL);
			//intResult = AfxMessageBox(mstrAppName, MB_ABORTRETRYIGNORE | MB_ICONQUESTION | MB_TASKMODAL);
			if (intResult == IDABORT)	
			{						// abort everything
				gLog.LogEvent(pobjComp->mstrName + ": Aborting installation at user request.");
				return false;
			} 
			else if (intResult == IDIGNORE) 
			{			// we don't care prev install failed, go on
				gLog.LogEvent(pobjComp->mstrName + ": Ignoring component; will not attempt to install again.");
				pobjComp->mblnInstall = false;
			}	
			else if (intResult ==  IDRETRY)
			{
				gLog.LogEvent(pobjComp->mstrName + ": Retrying component install.");
			}
		}
	}

	// we now have a list of components to install, check depends first though
	pos = mlstComps.GetHeadPosition();
	for (i = 0; i < mlstComps.GetCount(); i++) 
	{
		
		pobjComp = mlstComps.GetNext(pos);

		// check if any depends are not installed
		if (pobjComp->mblnInstall && DependsInstalled(pobjComp)) 
		{
			if (pobjComp->mtypReboot == ImmediateReboot) // we only want to install this component
				break;
		} 
		else 
		{
			// if depends not installed, we want to skip this component
			pobjComp->mblnInstall = false;
		}
	}

	// see if we have any immediate reboot items, end list there
	bool blnCutList = false;
	pos = mlstComps.GetHeadPosition();
	for (i = 0; i < mlstComps.GetCount(); i++) 
	{
		pobjComp = mlstComps.GetNext(pos);
		if (pobjComp->mblnInstall) 
		{
			if (blnCutList) 
				pobjComp->mblnInstall = false;
			else if (pobjComp->mtypReboot == ImmediateReboot)
				blnCutList = true;
		}
	}

	// debug: output the components to install this run
	pos = mlstComps.GetHeadPosition();
	for (i = 0; i < mlstComps.GetCount(); i++) 
	{
		pobjComp = mlstComps.GetNext(pos);
		if (pobjComp->mblnInstall)
			gLog.LogEvent(pobjComp->mstrName + ": Component on final list to install this run.");
	}

	return true;
}


bool CAutorunDlg::DependsInstalled(CComponent *pobjComp)
{

	POSITION lstPos; CString strDependId; CComponent * pobjTemp;

	// go through the depends for object pobjComp
	POSITION pos = pobjComp->mlstDepends.GetHeadPosition();
	for (int i = 0; i < pobjComp->mlstDepends.GetCount(); i++) 
	{
		strDependId = pobjComp->mlstDepends.GetNext(pos);

		// look through list to see if any depends are in list of to install stuff
 		lstPos = mlstComps.GetHeadPosition();
		for (int j = 0; j < mlstComps.GetCount(); j++) 
		{
			pobjTemp = mlstComps.GetNext(lstPos);
			if (pobjTemp->mblnInstall && pobjTemp->mstrId.CompareNoCase(strDependId) == 0) 
			{
				// we have a depend in the list that is not installed
				gLog.LogEvent(pobjComp->mstrName + ": Dependency '" + pobjTemp->mstrName + "' not installed.");
				return false;
			}
		}	// end looping through install list
	}		// end looping through depends list

	return true;	// all depends are installed
}

bool CAutorunDlg::InstallComponents()
{

	int intTotalTime = 0, i;
	POSITION pos;
	CComponent * pobjComp;
	bool blnReturnValue = true;

	// Count total time
	pos = mlstComps.GetHeadPosition();
	for (i = 0; i < mlstComps.GetCount(); i++) 
	{
		pobjComp = mlstComps.GetNext(pos);
		if (pobjComp->mblnInstall)
			intTotalTime += pobjComp->InstallTime();
	}

	// set global flag appropriately
	m_blnComponentCancel = false;

	// make sure there are components to install
	if (intTotalTime == 0)
		return true;

	// Init setup dialog
	CSetupDlg * sDlg;
	sDlg = new CSetupDlg;
	CWnd * hw;
	hw = sDlg;
	sDlg->m_strAppName = mstrAppName;
	sDlg->Create(NULL);
	sDlg->ShowWindow(SW_SHOW);
	sDlg->UpdateWindow();
	sDlg->SetMaxProgress(intTotalTime*2);

	// now go through list installing components
	pos = mlstComps.GetHeadPosition();
	for (i = 0; i < mlstComps.GetCount(); i++) 
	{
		pobjComp = mlstComps.GetNext(pos);
		
		if (pobjComp->mblnInstall) 
		{
			gLog.LogEvent(pobjComp->mstrName + ": Component installation started: " + pobjComp->mstrSetupCommand);

			// refresh dialog with current status
			sDlg->m_CurStatus = pobjComp->mstrSetupMessage;
			sDlg->UpdateData(false);
			
			if (!pobjComp->Install(sDlg) || sDlg->m_blnCancel) 
			{
				m_blnComponentCancel = true;
				return false;
			}
			if (pobjComp->mtypReboot != NoReboot) 
			{
				m_blnComponentRebootComputer = true;
				blnReturnValue = false;
			}
		}
	}


	// Destroy setup dialog
	sDlg->DestroyWindow();
	delete sDlg;
	return blnReturnValue;

}


bool CAutorunDlg::RebootComputer(CString strCmdLine) 
{

	// set RunOnce key
	char chTemp[MAX_PATH];
	GetModuleFileName(NULL, chTemp, MAX_PATH);
	strCmdLine = " " + strCmdLine;
	strcat(chTemp, strCmdLine);
	long h; HKEY hRegKey; LPDWORD hResult = 0;
	LPCTSTR sKey = "Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce";	
	h = RegCreateKeyEx(HKEY_CURRENT_USER, sKey,0,NULL,REG_OPTION_NON_VOLATILE, KEY_SET_VALUE, NULL, &hRegKey, hResult);
	if (h == ERROR_SUCCESS) 
	{
		h = RegSetValueEx(hRegKey, "VB Setup Program Autorun", 0, REG_SZ, (byte*)&chTemp, strlen(chTemp) + 1);
		h = RegCloseKey(hRegKey);
	} 
	else 
	{
		gUtils.LogDLLError("Restart IA");
	}
	gLog.LogEvent("IA set to resume on next startup.");

	CRestartDlg dlg;
	dlg.m_strAppName = mstrAppName;

	if (dlg.DoModal() == IDOK) 
	{
		// we want to reboot, but first adjust process privs
		gLog.LogEvent("Restarting computer...");
		HANDLE hToken; TOKEN_PRIVILEGES tkp;
		OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, &hToken);
		LookupPrivilegeValue(NULL, SE_SHUTDOWN_NAME, &tkp.Privileges[0].Luid);
		tkp.PrivilegeCount = 1;   // one priv to set
		tkp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
		AdjustTokenPrivileges(hToken, FALSE, &tkp, 0, (PTOKEN_PRIVILEGES)NULL, 0);
		ExitWindowsEx(EWX_REBOOT, 0);
	}
	return true;
}


BOOL CAutorunDlg::Create(CWnd* pParentWnd) 
{
	BOOL retVal = CDialog::Create(IDD, pParentWnd);

	return retVal;
}


void CAutorunDlg::ResetButtons()
{
	CDlgButton * pobjDlgButton;
	POSITION pos;
	
	// Set all buttons to correct state
	pos = mlstButtons.GetHeadPosition();
	for (int i = 0; i < mlstButtons.GetCount(); i++) 
	{

		pobjDlgButton = mlstButtons.GetNext(pos);

		if (pobjDlgButton->mbmpStandard != NULL)
			pobjDlgButton->mstatic->SetBitmap(pobjDlgButton->mbmpStandard);
		pobjDlgButton->mstatic->Invalidate();

		pobjDlgButton->mblnMouseOver	= false;
		pobjDlgButton->mblnMouseClick	= false;

	}

}
