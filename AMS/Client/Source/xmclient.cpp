// XMClient.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include <objbase.h>
#include "xmclient.h"
#include "xmnet.h"
#include "xmpipeline.h"
#include "xmgui.h"

// ---------------------------------------------------------------------- ABOUT DIALOG

class CAbout : public CDialog
{
public:
	CAbout(CWnd *parent)
		: CDialog(IDD_ABOUT, parent)
	{
	}
	~CAbout()
	{
	}
	BOOL OnInitDialog()
	{
		//create underlined font
		CFont font, ansi;
		LOGFONTA lfa;
		ansi.CreateStockObject(ANSI_VAR_FONT);
		ansi.GetLogFont(&lfa);
		lfa.lfUnderline = TRUE;
		lfa.lfWeight = FW_BOLD;
		font.CreateFontIndirect(&lfa);
		GetDlgItem(IDC_WEB)->SetFont(&font);
		font.Detach();

		//set the version
		CString str;
		str.Format("Version: %s\n", app()->Version());
		SetDlgItemText(IDC_VERSION, str);

		return FALSE;
	}
	HBRUSH OnCtlColor(CDC *pDC, CWnd *pWnd, UINT nCtlColor)
	{
		//let dialog go first
		HBRUSH hb = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);

		//Everything except username, passwords, and buttons gets
		//black back, white text
		if ((nCtlColor==CTLCOLOR_STATIC)||(nCtlColor==CTLCOLOR_DLG))
		{
			pDC->SetTextColor(RGB(255,255,255));
			pDC->SetBkColor(RGB(0,0,0));
			pDC->SetBkMode(TRANSPARENT);
			hb = (HBRUSH)::GetStockObject(BLACK_BRUSH);
		}
		if (pWnd->GetDlgCtrlID()==IDC_WEB)
		{
			pDC->SetTextColor(RGB(0,0,255));
		}
		return hb;
	}
	void OnWebClick()
	{
		//open a web browser window with the help
		UINT retval = (UINT)ShellExecute(
								::GetDesktopWindow(),
								"open",
								"iexplore",
								"http://www.adultmediaswapper.com/",
								"",
								SW_SHOWDEFAULT);
		if (retval<=32)
		{
			//error
			ASSERT(FALSE);
		}
	}
	DECLARE_MESSAGE_MAP();
};
BEGIN_MESSAGE_MAP(CAbout, CDialog)
		ON_WM_CTLCOLOR()
		ON_CONTROL(STN_CLICKED, IDC_WEB, OnWebClick)
END_MESSAGE_MAP()

void DoAbout(CWnd *parent)
{
	CAbout dlg(parent);
	dlg.DoModal();
}

// ---------------------------------------------------------------------- PASSWORD PROTECT

class CPasswordProtect : public CDialog
{
public:
	CPasswordProtect(CWnd *parent)
		: CDialog(IDD_PWD, parent)
	{
		m_iTry = 3;
		m_bFailed = false;
	}
	CString m_strPassword;
	CString m_strGuess;
	int m_iTry;
	bool m_bFailed;
	void DoDataExchange(CDataExchange *pDX)
	{
		if (pDX->m_bSaveAndValidate)
		{
			//get md5 of what they typed
			DDX_Text(pDX, IDC_PWD, m_strGuess);
			CMD5 md5;
			md5.FromBuf((BYTE*)m_strGuess.LockBuffer(), m_strGuess.GetLength());
			m_strGuess.UnlockBuffer();

			//compare to md5 of password
			CMD5 pass = m_strPassword;
			if (!md5.IsEqual(pass) &&
				m_strPassword != m_strGuess)
			{
				//one less try
				m_iTry--;
				if (m_iTry<1)
				{
					AfxMessageBox("You must enter the correct password to use this program.");
					m_bFailed = true;
					return;
				}
				AfxMessageBox("Incorrect password.");
				pDX->Fail();
			}
		}
	}
};

bool DoPasswordCheck()
{
	//only allow 1 dialog at time
	static bool incheck = false;
	if (incheck)
		return false;
	incheck = true;

	CPasswordProtect dlg(NULL);
	dlg.m_strPassword = config()->GetField(FIELD_LOGIN_PROTECT_PASSWORD, false);
	dlg.m_iTry = 3;
	if (dlg.DoModal()!=IDOK ||
		dlg.m_bFailed)
	{	
		incheck = false;
		return false;
	}
	incheck = false;
	return true;
}

// ---------------------------------------------------------------------- Win App


BEGIN_MESSAGE_MAP(CXMClientApp, CWinApp)
END_MESSAGE_MAP()

CXMClientApp::CXMClientApp()
{
	m_bHasInit = false;
	m_hMutex = NULL;
}

char* CXMClientApp::Version()
{
	return "0.70";
}
	

BOOL CXMClientApp::InitInstance()
{
	//only one instance of the application is allowed to run
	m_hMutex = CreateMutex(NULL, TRUE, "ams_mutex");
	if (!m_hMutex)
	{
		AfxMessageBox("AMS could not start do to an internal error.");
		return FALSE;
	}
	if (GetLastError()==ERROR_ALREADY_EXISTS)
	{
		//another instance of the app is running
		AfxMessageBox("You can only start one instance of AMS at a time. If AMS does not "
					"appear to be running, close this dialog box then follow these instructions:\n"
					"Win95/98/ME: Press the control, alt, and delete keys simultaneously. "
					"If you see AMSClient in the list, click on it, then press End Task.\n"
					"WinNT/2000: Press the control, alt, and delete keys simultaneously. "
					"Press the Task Manager button. Click the Processes tab in the Windows "
					"Task Manager window. Find AMSClient in the list and click on it.  Press "
					"the End Process button.\n\n"
					"You should now be able to start AMS again.", MB_ICONERROR, 0);
		CloseHandle(m_hMutex);
		m_hMutex = NULL;
		return FALSE;
	}

	//base class initialization
	if (!CWinApp::InitInstance()) {
		AfxMessageBox("!CWinApp::InitInstance()");
		return FALSE;
	}

	//initialize com
	if (FAILED(CoInitializeEx(NULL, COINIT_APARTMENTTHREADED))) {
		AfxMessageBox("FAILED(CoInitializeEx(NULL))");
		return FALSE;
	}
	//InitCommonControls();

	//initialize control containment
	AfxEnableControlContainer();

	//initialize sockets
	WORD wsockVer = MAKEWORD(2,2);		//winsock 2.2
	WSADATA wsockData;
	if (WSAStartup(wsockVer, &wsockData)!=0) {
		AfxMessageBox("Failed to initialize WinSock.");
		return FALSE;
	}
	if (HIBYTE(wsockData.wVersion) != 2 ||
		LOBYTE(wsockData.wVersion) != 2) {
		AfxMessageBox("Could not set the proper version (2.2) of\n" \
					  "Windows Sockets.  The application might not\n" \
					  "function properly, but initialization will\n" \
					  "proceed.");
	}

	//load registry settings
	if (!config()->RegLoad()) {
		AfxMessageBox("Could not load registry settings.");
		return FALSE;
	}

	//read config file
	if (!config()->LoadFromFile(config()->RegGetConfigPath(), Version())) {
		AfxMessageBox("!theConfig.LoadFromFile(\"config.xml\")");
		return FALSE;
	}

	//password protected?
	if (config()->GetFieldBool(FIELD_LOGIN_PROTECT_ENABLE))
	{
		if (!DoPasswordCheck())
			return FALSE;
	}

	//setup database
	/*
	if (!dbman()->DatabaseStartup()) {
		AfxMessageBox("Local file database failed to initialize.");
		return FALSE;
	}
	*/
	//DB NOW LOADED IN LOGIN DIALOG
	if (!dbman()->DatabaseStartupEarly())
	{
		AfxMessageBox("Error creating database paths.", MB_ICONERROR);
		return FALSE;
	}

	//show configuation?
	if (!config()->GetFieldBool(FIELD_HASRUN))
	{
		DoPrefs();
		config()->SetField(FIELD_HASRUN, "true");
	}

	//global session init (regwndclass)
	if (!CXMSession::InitOnce(m_hInstance)) {
		AfxMessageBox("!CXMSession::InitOnce()");
		return FALSE;
	}
	if (!CXMPipelineBase::InitOnce()) {
		AfxMessageBox("!CXMPipelineBase::InitOnce()");
		return FALSE;
	}

	//start pipelines
	cm()->Start( );
	sm()->Start();

	//login
	if (!sm()->LoginUI())
	{
		AfxMessageBox(	"You must login to use AMS.  If you do not " 
						"have a username or password, you may get one "
						"from the AMS webs site at 'www.adultmediaswapper.com'." );
		return FALSE;
	}

	//if we auto-updated during login, we need to abort
	if (sm()->AuComplete())
	{
		return TRUE;
	}

	//show main window
	m_pMainWnd= new CMainFrame;
	if (m_pMainWnd->m_hWnd)
	{
		m_pMainWnd->ShowWindow(
				(config()->GetFieldLong(FIELD_GUI_MAIN_WND_MODE)==SW_MAXIMIZE)
				?SW_SHOWMAXIMIZED:SW_SHOW);
		m_pMainWnd->UpdateWindow();
	}
	else
	{
		ASSERT(FALSE);
		return FALSE;
	}

	return TRUE;
}

int CXMClientApp::ExitInstance()
{	
	//close any sessions that remain open
	cm()->Stop();
	sm()->ServerClose();
	sm()->Stop();
	sessions()->FreeAll();

	//save database
	db()->Flush();
	db()->Close();

	//save config
	config()->SaveToDefaultFile(Version());

	//free winsock
	WSACleanup();

	//free COM
	CoUninitialize();

	//release the mutex
	if (m_hMutex)
	{
		CloseHandle(m_hMutex);
		m_hMutex = NULL;
	}

	return CWinApp::ExitInstance();
}

// ---------------------------------------------------------------------- Globals

CXMClientApp theApp;
CXMClientManager theCM;
CXMServerManager theSM;
CXMAsyncResizer theResizer;


CXMClientManager* cm() {
	return &theCM;
}
CXMServerManager* sm() {
	return &theSM;
}
CXMAsyncResizer* ar() {
	return &theResizer;
}

// ---------------------------------------------------------------------- Utils



CString BuildSavedFilename(CMD5 md5)
{
	CString retval;
	char szmd5[9];
	md5tohex(szmd5, md5.GetValue(), 4);
	szmd5[8] = '\0';
	retval.Format(
			"%s\\%s - %s.jpg",
			config()->GetField(FIELD_DB_SAVE_PATH, false),
			CTime::GetCurrentTime().Format("%y%m%d"),
			szmd5);
	return retval;
}

bool ChoosePath(HWND hwnd, const char* message, char* path)
{
	//my computer is root
	LPITEMIDLIST pidlmc = NULL;
	if (FAILED(SHGetSpecialFolderLocation(hwnd, CSIDL_DRIVES, &pidlmc)))
		return false;

	//ask for search path
	LPITEMIDLIST idlist = NULL;
	BROWSEINFO bi;
	bi.hwndOwner = hwnd;
	bi.iImage = 0;
	bi.lParam = 0;
	bi.lpfn = NULL;
	bi.lpszTitle = message;
	bi.pidlRoot = pidlmc;
	bi.pszDisplayName = path;
	bi.ulFlags = BIF_RETURNONLYFSDIRS |
				 BIF_VALIDATE |
				 BIF_RETURNFSANCESTORS;
	idlist = SHBrowseForFolder(&bi);
	if (!idlist)
	{
		//cancel
		return false;
	}

	//convert to string
	bool success = SHGetPathFromIDList(idlist, path)?true:false;
	
	//free memory
	if (idlist)
	{
		IMalloc *pmalloc;
		SHGetMalloc(&pmalloc);
		pmalloc->Free(idlist);
		pmalloc->Free(pidlmc);
		pmalloc->Release();
	}
	return success;
}

// ----------------------------------------------------------------------------------- CHANGE PASSWORD

class CChangePassword : public CDialog
{
public:
	CChangePassword(CString oldPassword, CWnd* pwndParent)
		: CDialog(IDD_CHANGEPWD, pwndParent)
	{
		moldPassword = oldPassword;
		mbCheckOldPassword = true;
	}
	bool ChangePassword()
	{
		return (DoModal()==IDOK)?true:false;
	}
	CString GetNewPassword()
	{
		CMD5 md5;
		md5.FromBuf((BYTE*)mnewPassword.LockBuffer(), mnewPassword.GetLength());
		mnewPassword.UnlockBuffer();
		return CString(md5.GetString());
	}
	bool mbCheckOldPassword;

protected:
	
	//vars
	CString moldPassword;
	CString moldPasswordGuess;
	CString mnewPassword;
	CString mverifyPassword;

	BOOL OnInitDialog()
	{
		//disable the old password box?
		if (!mbCheckOldPassword)
		{
			::EnableWindow(::GetDlgItem(m_hWnd, IDC_PWD_OLD), FALSE);
			::EnableWindow(::GetDlgItem(m_hWnd, IDC_PWD_LABEL), FALSE);
			::SetDlgItemText(m_hWnd, IDC_INSTRUCTIONS, "Enter the new password, then retype it for accuracy:");
		}
		::SetFocus(::GetDlgItem(m_hWnd, mbCheckOldPassword?IDC_PWD_OLD:IDC_PWD_NEW));
		return TRUE;
	}

	void DoDataExchange(CDataExchange *pDX)
	{
		//transfer text
		DDX_Text(pDX, IDC_PWD_OLD, moldPasswordGuess);
		DDX_Text(pDX, IDC_PWD_NEW, mnewPassword);
		DDX_Text(pDX, IDC_PWD_VERIFY, mverifyPassword);

		//do checks
		if (mbCheckOldPassword &&
			pDX->m_bSaveAndValidate)
		{
			//generate md5 from password guess
			CMD5 md5;
			md5.FromBuf((BYTE*)moldPasswordGuess.LockBuffer(), moldPasswordGuess.GetLength());
			moldPasswordGuess.UnlockBuffer();

			//compare old passwod
			if (moldPassword != moldPasswordGuess &&
				!CMD5(moldPassword).IsEqual(md5))
			{
				AfxMessageBox("Incorrect password.");
				pDX->Fail();
			}
			else
			{
				if (mnewPassword!=mverifyPassword)
				{
					AfxMessageBox("New passwords do not match.  You must re-type your new password identically in both the 'New Password' and 'Verify New Password' boxes.");
					pDX->Fail();
				}
			}
		}
	}
};

// ----------------------------------------------------------------------------------- PREFERENCES

class CPreferences : CPropertySheet
{
public:

	//construction
	CPreferences(CString strCaption, CXMClientConfig* newConfig) :
	  CPropertySheet(strCaption, NULL)
	{
		mpConfig = newConfig;
		CXMClientConfig::Copy(mpConfig, &mSandbox);
	}
	~CPreferences()
	{
	}

	//show the dialog
	bool DoPreferences(bool alt)
	{
		//create common pages
		mPageGeneral.Construct(IDD_PREFS_GENERAL);
		mPageDownloading.Construct(IDD_PREFS_DOWNLOADING);
		mPageSharing.Construct(IDD_PREFS_UPLOADING);
			
		mPageGeneral.mSandbox = &mSandbox;
		mPageSearching.mSandbox = &mSandbox;
		mPageDownloading.mSandbox = &mSandbox;
		mPageSharing.mSandbox = &mSandbox;
		mPageAdvanced.mSandbox = &mSandbox;

		mPageGeneral.LoadData();
		mPageDownloading.LoadData();
		mPageSharing.LoadData();

		bool retval = false;
		if (alt)
		{
			//add pages in order
			AddPage(&mPageGeneral);
			AddPage(&mPageDownloading);
			AddPage(&mPageSharing);

			//show dialog
			//SetWizardMode();
			//SetWizardButtons(PSWIZB_BACK|PSWIZB_NEXT|PSWIZB_FINISH);
			if (DoModal()==IDOK)
			{
				//save data
				retval = true;
				CXMClientConfig::Copy(&mSandbox, mpConfig);
			}
		}
		else
		{
			//create the other pages
			mPageSearching.Construct(IDD_PREFS_SEARCHING);
			//mPageAdvanced.Construct(IDD_PREFS_ADVANCED);

			//add pages in order
			AddPage(&mPageGeneral);
			AddPage(&mPageSearching);
			AddPage(&mPageDownloading);
			AddPage(&mPageSharing);
			//AddPage(&mPageAdvanced);

			mPageSearching.LoadData();
			//mPageAdvanced.LoadData();

			//show dialog
			if (DoModal()==IDOK)
			{
				//save data
				retval = true;
				CXMClientConfig::Copy(&mSandbox, mpConfig);
			}
			
		}
		return retval;
	}

private:
	
	//store a point to the original config
	CXMClientConfig *mpConfig;

	//store a sandbox copy of the config
	CXMClientConfig mSandbox;

	//------------------------------------------ pages
	
	class PageGeneral : public CPropertyPage {
	public:
		CXMClientConfig *mSandbox;
		BOOL mPwdProtect_Enable;
		CString mPwdProtect_Pwd;
		BOOL mAutoLogin_Enable;
		CString mAutoLogin_Username;
		CString mAutoLogin_Password;
		BOOL mReconnect_Enable;
		int mReconnect_Delay;
		void LoadData()
		{
			mPwdProtect_Enable = mSandbox->GetFieldBool(FIELD_LOGIN_PROTECT_ENABLE);
			mPwdProtect_Pwd = mSandbox->GetField(FIELD_LOGIN_PROTECT_PASSWORD, false);
			mAutoLogin_Enable = mSandbox->GetFieldBool(FIELD_LOGIN_AUTO_ENABLE);
			mAutoLogin_Username = mSandbox->GetField(FIELD_LOGIN_AUTO_USERNAME, false);
			mAutoLogin_Password = mSandbox->GetField(FIELD_LOGIN_AUTO_PASSWORD, false);
			mReconnect_Enable = mSandbox->GetFieldBool(FIELD_NET_RECONNECT_ENABLE);
			mReconnect_Delay = (int)mSandbox->GetFieldLong(FIELD_NET_RECONNECT_DELAY);
		}
		void DoDataExchange(CDataExchange *pDX)
		{
			DDX_Check(pDX, IDC_PWDPROTECT, mPwdProtect_Enable);
			DDX_Check(pDX, IDC_AUTOLOGIN, mAutoLogin_Enable);
			DDX_Check(pDX, IDC_RECONNECT, mReconnect_Enable);
			DDX_Text(pDX, IDC_RECONNCT_DELAY, mReconnect_Delay);
			DDX_Text(pDX, IDC_USERNAME, mAutoLogin_Username);
			/*
			if (pDX->m_bSaveAndValidate)
				DDX_Text(pDX, IDC_PASSWORD, mAutoLogin_Password);
			else
				DDX_Text(pDX, IDC_PASSWORD, CString(""));
			*/
			DDV_MinMaxInt(pDX, mReconnect_Delay, 1, 1440);
			if (!pDX->m_bSaveAndValidate)
			{
				OnEnableControls();
			}
		}
		void OnALChangePwd()
		{
			CChangePassword dlg("", this);
			dlg.mbCheckOldPassword = false;
			if (dlg.ChangePassword())
			{
				mAutoLogin_Password = dlg.GetNewPassword();
			}
		}
		void OnChangePassword()
		{
			CChangePassword pwd(mPwdProtect_Pwd, this);
			if (pwd.ChangePassword())
			{
				mPwdProtect_Pwd = pwd.GetNewPassword();
			}
		}
		void OnOK()
		{
			mSandbox->SetField(FIELD_LOGIN_PROTECT_ENABLE, mPwdProtect_Enable?"true":"false");
			mSandbox->SetField(FIELD_LOGIN_PROTECT_PASSWORD, mPwdProtect_Pwd);
			mSandbox->SetField(FIELD_LOGIN_AUTO_ENABLE, mAutoLogin_Enable?"true":"false");
			mSandbox->SetField(FIELD_LOGIN_AUTO_USERNAME, mAutoLogin_Username);
			mSandbox->SetField(FIELD_LOGIN_AUTO_PASSWORD, mAutoLogin_Password);
			mSandbox->SetField(FIELD_NET_RECONNECT_ENABLE, mReconnect_Enable?"true":"false");
			mSandbox->SetField(FIELD_NET_RECONNECT_DELAY, mReconnect_Delay);
		}
		void OnEnableControls()
		{
			//password protect
			bool temp = (IsDlgButtonChecked(IDC_PWDPROTECT)==BST_CHECKED);
			GetDlgItem(IDC_CHANGEPWD)->EnableWindow(temp);

			//auto login
			temp = (IsDlgButtonChecked(IDC_AUTOLOGIN)==BST_CHECKED);
			GetDlgItem(IDC_STATIC_USERNAME)->EnableWindow(temp);
			GetDlgItem(IDC_STATIC_PASSWORD)->EnableWindow(temp);
			GetDlgItem(IDC_USERNAME)->EnableWindow(temp);
			GetDlgItem(IDC_AL_PWD_CHANGE)->EnableWindow(temp);

			//reconnect
			temp = (IsDlgButtonChecked(IDC_RECONNECT)==BST_CHECKED);
			GetDlgItem(IDC_STATIC_RECONNECT1)->EnableWindow(temp);
			GetDlgItem(IDC_STATIC_RECONNECT2)->EnableWindow(temp);
			GetDlgItem(IDC_RECONNCT_DELAY)->EnableWindow(temp);
		}
		DECLARE_MESSAGE_MAP();
	} mPageGeneral;

	class PageSearching : public CPropertyPage
	{
	public:
		CXMClientConfig *mSandbox;
		BOOL mAutoSave_Enable;
		int mAutoSave_LastN;
		void LoadData()
		{
			mAutoSave_Enable = mSandbox->GetFieldBool(FIELD_SEARCH_AUTOSAVE_ENABLE);
			mAutoSave_LastN = mSandbox->GetFieldLong(FIELD_SEARCH_AUTOSAVE_COUNT);
		}
		void DoDataExchange(CDataExchange *pDX)
		{
			DDX_Check(pDX, IDC_SAVESEARCHES, mAutoSave_Enable);
			DDX_Text(pDX, IDC_LASTN, mAutoSave_LastN);
			if (!pDX->m_bSaveAndValidate)
			{
				OnEnableControls();
			}
		}
		void OnOK()
		{
			mSandbox->SetField(FIELD_SEARCH_AUTOSAVE_ENABLE, mAutoSave_Enable?"true":"false");
			mSandbox->SetField(FIELD_SEARCH_AUTOSAVE_COUNT, mAutoSave_LastN);
		}
		void OnEnableControls()
		{
			bool temp = (IsDlgButtonChecked(IDC_SAVESEARCHES)==BST_CHECKED);
			GetDlgItem(IDC_LASTN)->EnableWindow(temp);
		}
		DECLARE_MESSAGE_MAP();
	} mPageSearching;

	class PageDownloading : public CPropertyPage
	{
	public:
		CXMClientConfig *mSandbox;
		int mSpeed;
		BOOL mAuto;
		int mMaxThumbs, mMaxPictures;
		CString mPath;
		void LoadData()
		{
			mSpeed = mSandbox->GetFieldLong(FIELD_NET_DATARATE);
			mAuto = mSandbox->GetFieldBool(FIELD_PIPELINE_AUTO_DOWNLOAD);
			mMaxThumbs = mSandbox->GetFieldLong(FIELD_PIPELINE_MAXTHUMB);
			mMaxPictures = mSandbox->GetFieldLong(FIELD_PIPELINE_MAXFILE);
			mPath = mSandbox->GetField(FIELD_DB_SAVE_PATH, false);
		}
		void DoDataExchange(CDataExchange *pDX)
		{
			DDX_CBIndex(pDX, IDC_SPEED, mSpeed);
			DDX_Check(pDX, IDC_TRANSFER_AUTO, mAuto);
			if (!pDX->m_bSaveAndValidate)
			{
				CheckDlgButton(IDC_TRANSFER_MANUAL, !mAuto);
			}
			DDX_Text(pDX, IDC_MAXTHUMBS, mMaxThumbs);
			DDX_Text(pDX, IDC_MAXPICTURES, mMaxPictures);
			DDX_Text(pDX, IDC_SAVEDFILES, mPath);
			if (pDX->m_bSaveAndValidate)
			{
				//validate path
				if (mPath.GetLength()<2)
				{
					AfxMessageBox("Please specify a valid path.");
					pDX->Fail();
				}
				else if (mPath.GetAt(1)!=':')
				{
					//must specify a drive letter
					AfxMessageBox("You must specify a folder belonging to a drive on your computer. (Mapped Network Drives are acceptable.)");
					pDX->Fail();
				}
				else if (!CreateDirectory(mPath, NULL))
				{
					//does the dir already exist?
					if (GetLastError()!=ERROR_ALREADY_EXISTS)
					{
						AfxMessageBox("Unable to create the saved files folder.  Please make sure it is a valid drive on which a folder can be created.");
						pDX->Fail();
					}
				}
			}
			if (!pDX->m_bSaveAndValidate)
			{
				OnEnableControls();
			}
		}
		void OnOK()
		{
			mSandbox->SetField(FIELD_NET_DATARATE, mSpeed);
			mSandbox->SetField(FIELD_PIPELINE_AUTO_DOWNLOAD, mAuto?"true":"false");
			mSandbox->SetField(FIELD_PIPELINE_MAXTHUMB, mMaxThumbs);
			mSandbox->SetField(FIELD_PIPELINE_MAXFILE, mMaxPictures);
			mSandbox->SetField(FIELD_DB_SAVE_PATH, mPath);
		}
		void OnEnableControls()
		{
			bool temp = (IsDlgButtonChecked(IDC_TRANSFER_MANUAL)==BST_CHECKED);
			GetDlgItem(IDC_STATIC_MAXTHUMBS)->EnableWindow(temp);
			GetDlgItem(IDC_STATIC_MAXPICTURES)->EnableWindow(temp);
			GetDlgItem(IDC_MAXTHUMBS)->EnableWindow(temp);
			GetDlgItem(IDC_MAXPICTURES)->EnableWindow(temp);
		}
		void OnChoose()
		{
			char szPath[MAX_PATH+1];
			strcpy(szPath, mPath);
			if (ChoosePath(m_hWnd, "Select the folder you wish to save files to:", szPath))
			{
				mPath = szPath;
				SetDlgItemText(IDC_SAVEDFILES, szPath);
			}
		}
		DECLARE_MESSAGE_MAP();
	} mPageDownloading;

	class PageSharing : public CPropertyPage
	{
	public:
		CXMClientConfig *mSandbox;
		BOOL mShare, mAuto, mUseSaved;
		int mMaxUploads;
		CString mPath;
		void LoadData()
		{
			mShare = mSandbox->GetFieldBool(FIELD_DB_SHARE_ENABLE);	
			mAuto = mSandbox->GetFieldBool(FIELD_PIPELINE_AUTO_UPLOAD);
			mUseSaved = mSandbox->GetFieldBool(FIELD_DB_SHARE_USESAVED);
			mMaxUploads = mSandbox->GetFieldLong(FIELD_PIPELINE_MAXUP);
			mPath = mSandbox->GetField(FIELD_DB_SHARE_PATH, false);
		}
		void DoDataExchange(CDataExchange *pDX)
		{
			DDX_Check(pDX, IDC_UPLOAD_ENABLE, mShare);
			DDX_Check(pDX, IDC_TRANSFER_AUTO, mAuto);
			DDX_Check(pDX, IDC_SHARE_SAVED, mUseSaved);
			DDX_Text(pDX, IDC_MAXUPLOADS, mMaxUploads);
			DDX_Text(pDX, IDC_SHAREDFILES, mPath);
			if (pDX->m_bSaveAndValidate)
			{
				//validate path
				if (mShare && !mUseSaved)
				{
					if (mPath.GetLength()<2)
					{
						AfxMessageBox("Please specify a valid path.");
						pDX->Fail();
					}
					else if (mPath.GetAt(1)!=':')
					{
						//must specify a drive letter
						AfxMessageBox("You must specify a folder belonging to a drive on your computer. (Mapped Network Drives are acceptable.)");
						pDX->Fail();
					}
					else if (!CreateDirectory(mPath, NULL))
					{
						//does the dir already exist?
						if (GetLastError()!=ERROR_ALREADY_EXISTS)
						{
							AfxMessageBox("Unable to create the shared files folder.  Please make sure it is a valid drive on which a folder can be created.");
							pDX->Fail();
						}
					}
				}
			}
			if (!pDX->m_bSaveAndValidate)
			{
				CheckDlgButton(IDC_UPLOAD_DISABLE, !mShare);
				CheckDlgButton(IDC_TRANSFER_MANUAL, !mAuto);
				CheckDlgButton(IDC_SHARE_CUSTOM, !mUseSaved);
			}
			if (!pDX->m_bSaveAndValidate)
			{	
				OnEnableControls();
			}
		}
		void OnOK()
		{
			mSandbox->SetField(FIELD_DB_SHARE_ENABLE, mShare?"true":"false");	
			mSandbox->SetField(FIELD_PIPELINE_AUTO_UPLOAD, mAuto?"true":"false");
			mSandbox->SetField(FIELD_DB_SHARE_USESAVED, mUseSaved?"true":"false");
			mSandbox->SetField(FIELD_PIPELINE_MAXUP, mMaxUploads);
			mSandbox->SetField(FIELD_DB_SHARE_PATH, mPath);
		}
		void OnEnableControls()
		{
			bool temp = (IsDlgButtonChecked(IDC_UPLOAD_ENABLE)==BST_CHECKED);
			GetDlgItem(IDC_STATIC_UPLOADS)->EnableWindow(temp);
			GetDlgItem(IDC_TRANSFER_AUTO)->EnableWindow(temp);
			GetDlgItem(IDC_TRANSFER_MANUAL)->EnableWindow(temp);
			GetDlgItem(IDC_STATIC_SHARED)->EnableWindow(temp);
			GetDlgItem(IDC_STATIC_SHARED2)->EnableWindow(temp);
			GetDlgItem(IDC_SHARE_CUSTOM)->EnableWindow(temp);
			GetDlgItem(IDC_SHARE_SAVED)->EnableWindow(temp);
			if (temp)
			{
				temp = (IsDlgButtonChecked(IDC_TRANSFER_MANUAL)==BST_CHECKED);
				GetDlgItem(IDC_STATIC_MAXUPLOADS)->EnableWindow(temp);
				GetDlgItem(IDC_MAXUPLOADS)->EnableWindow(temp);
				temp = (IsDlgButtonChecked(IDC_SHARE_CUSTOM)==BST_CHECKED);
				GetDlgItem(IDC_SHAREDFILES)->EnableWindow(temp);
				GetDlgItem(IDC_CHOOSE)->EnableWindow(temp);
			}
			else
			{
				GetDlgItem(IDC_STATIC_MAXUPLOADS)->EnableWindow(FALSE);
				GetDlgItem(IDC_MAXUPLOADS)->EnableWindow(FALSE);
				GetDlgItem(IDC_SHAREDFILES)->EnableWindow(FALSE);
				GetDlgItem(IDC_CHOOSE)->EnableWindow(FALSE);
			}
		}
		void OnChoose()
		{
			char szPath[MAX_PATH+1];
			strcpy(szPath, mPath);
			if (ChoosePath(m_hWnd, "Select the folder you wish to save files to:", szPath))
			{
				mPath = szPath;
				SetDlgItemText(IDC_SHAREDFILES, szPath);
			}
		}
		DECLARE_MESSAGE_MAP();
	} mPageSharing;

	class PageAdvanced : public CPropertyPage
	{
	public:
		CXMClientConfig *mSandbox;
		void LoadData()
		{

		}
		void DoDataExchange(CDataExchange *pDX)
		{

		}
		void OnOK()
		{

		}
		void OnEnableControls()
		{
			//bool temp = (IsDlgButtonChecked(IDC_)==BST_CHECKED);
			//GetDlgItem(IDC_)->EnableWindow(temp);
		}
		DECLARE_MESSAGE_MAP();
	} mPageAdvanced;
};

BEGIN_MESSAGE_MAP(CPreferences::PageGeneral, CPropertyPage)
	ON_BN_CLICKED(IDC_CHANGEPWD, CPreferences::PageGeneral::OnChangePassword)
	ON_BN_CLICKED(IDC_PWDPROTECT, CPreferences::PageGeneral::OnEnableControls)
	ON_BN_CLICKED(IDC_AUTOLOGIN, CPreferences::PageGeneral::OnEnableControls)
	ON_BN_CLICKED(IDC_RECONNECT, CPreferences::PageGeneral::OnEnableControls)
	ON_BN_CLICKED(IDC_AL_PWD_CHANGE, CPreferences::PageGeneral::OnALChangePwd)
END_MESSAGE_MAP()

BEGIN_MESSAGE_MAP(CPreferences::PageSearching, CPropertyPage)
	ON_BN_CLICKED(IDC_SAVESEARCHES, CPreferences::PageSearching::OnEnableControls)
END_MESSAGE_MAP()

BEGIN_MESSAGE_MAP(CPreferences::PageDownloading, CPropertyPage)
	ON_BN_CLICKED(IDC_CHOOSE, CPreferences::PageDownloading::OnChoose)
	ON_BN_CLICKED(IDC_TRANSFER_AUTO, CPreferences::PageDownloading::OnEnableControls)
	ON_BN_CLICKED(IDC_TRANSFER_MANUAL, CPreferences::PageDownloading::OnEnableControls)
END_MESSAGE_MAP()

BEGIN_MESSAGE_MAP(CPreferences::PageSharing, CPropertyPage)
	ON_BN_CLICKED(IDC_CHOOSE, CPreferences::PageSharing::OnChoose)
	ON_BN_CLICKED(IDC_TRANSFER_AUTO, CPreferences::PageSharing::OnEnableControls)
	ON_BN_CLICKED(IDC_TRANSFER_MANUAL, CPreferences::PageSharing::OnEnableControls)
	ON_BN_CLICKED(IDC_SHARE_SAVED, CPreferences::PageSharing::OnEnableControls)
	ON_BN_CLICKED(IDC_SHARE_CUSTOM, CPreferences::PageSharing::OnEnableControls)
	ON_BN_CLICKED(IDC_UPLOAD_ENABLE, CPreferences::PageSharing::OnEnableControls)
	ON_BN_CLICKED(IDC_UPLOAD_DISABLE, CPreferences::PageSharing::OnEnableControls)
END_MESSAGE_MAP()

BEGIN_MESSAGE_MAP(CPreferences::PageAdvanced, CPropertyPage)
END_MESSAGE_MAP()

void DoPrefs()
{
	CPreferences prefs("AMS Options", config());
	prefs.DoPreferences(false);
}
