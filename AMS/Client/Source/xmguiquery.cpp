// XMGUIQUERY.CPP ------------------------------------------------------------- XMGUIQUERY.CPP

#include "stdafx.h"
#include "xmclient.h"
#include "xmquery.h"
#include "xmdb.h"
#include "xmnet.h"
#include "xmpipeline.h"
#include "xmgui.h"

// ------------------------------------------------------------------------- CONTEST

class CContestDialog : public CDialog
{
public:
	
	//constructor
	CContestDialog(CWnd *pwnd)
		: CDialog(IDD_CONTEST, pwnd)
	{
		mState = CONTEST_UNKNOWN;
		mCurrentQuery = 0;
		mFile = NULL;
		memset(&mCF, 0, sizeof(CXMClientManager::CompletedFile));
	}
	~CContestDialog()
	{
		//free memory
		if (mCF.mBuffer)
			free(mCF.mBuffer);
	}

	//subscribe, start first dl
	BOOL OnInitDialog()
	{
		//load the basic bitmaps
		if (!mBmpWaiting.LoadBitmap(IDB_THUMBNAIL_WAITING) ||
			!mBmpDownloading.LoadBitmap(IDB_THUMBNAIL_DOWNLOADING) ||
			!mBmpError.LoadBitmap(IDB_THUMBNAIL_ERROR))
		{
			AfxMessageBox("Error loading standard bitmaps!");
			EndDialog(IDCANCEL);
			return FALSE;
		}

		//subscribe
		sm()->Subscribe(m_hWnd, true);
		cm()->Subscribe(m_hWnd, true);

		//simply 'skip' the current picture
		OnSkip();

		return FALSE;
	}

	//unsubscribe
	void OnDestroy()
	{
		sm()->UnSubscribe(m_hWnd);
		cm()->UnSubscribe(m_hWnd);
	}

private:

	//state data
	enum StateTag
	{
		CONTEST_IDLE,
		CONTEST_QUERYING,
		CONTEST_WAITING,
		CONTEST_DOWNLOADING,
		CONTEST_ERROR,
		CONTEST_UNKNOWN
	} mState;
	void SetState(StateTag newState)
	{
		//set the new state
		mState = newState;

		//get the controls
		CStatic *preview = (CStatic*)GetDlgItem(IDC_PREVIEW);
		CButton *index = (CButton*)GetDlgItem(IDC_INDEX);
		CButton *skip = (CButton*)GetDlgItem(IDC_SKIP);

		CString sSkip = "Skip Picture", sNext = "Next Picture";

		switch(newState)
		{
		case CONTEST_IDLE:
			index->EnableWindow(TRUE);
			skip->EnableWindow(TRUE);
			skip->SetWindowText(sSkip);
			SetStatus("Ready.");
			break;
		
		case CONTEST_QUERYING:
			index->EnableWindow(FALSE);
			skip->EnableWindow(FALSE);
			skip->SetWindowText(sSkip);
			preview->SetBitmap(mBmpWaiting);
			SetStatus("Querying for picture...");
			break;
		
		case CONTEST_WAITING:
			index->EnableWindow(FALSE);
			skip->EnableWindow(TRUE);
			preview->SetBitmap(mBmpWaiting);
			skip->SetWindowText(sSkip);
			SetStatus("Waiting for download to begin...");
			break;

		case CONTEST_DOWNLOADING:
			index->EnableWindow(FALSE);
			skip->EnableWindow(TRUE);
			preview->SetBitmap(mBmpDownloading);
			skip->SetWindowText(sSkip);
			SetStatus("Downloading file...");
			break;

		case CONTEST_ERROR:
			index->EnableWindow(FALSE);
			skip->EnableWindow(TRUE);
			skip->SetWindowText(sSkip);
			preview->SetBitmap(mBmpError);
			break;

		case CONTEST_UNKNOWN:
			index->EnableWindow(FALSE);
			skip->EnableWindow(TRUE);
			skip->SetWindowText(sSkip);
			preview->SetBitmap(mBmpError);
			break;
		}
	}
	void SetStatus(const char *msg)
	{
		::SetWindowText(::GetDlgItem(m_hWnd, IDC_STATUS), msg);
	}
	
	//download state
	CMD5 mCurrentPic;
	DWORD mCurrentQuery;

	//picture/file data
	CXMDBFile *mFile;
	CXMClientManager::CompletedFile mCF;
	
	//image handles
	CBitmap	mBmpWaiting,
			mBmpDownloading,
			mBmpError;

	afx_msg void OnContestHelp()
	{
		//open a web browser window with the help
		CString url = "http://www.adultmediaswapper.com/contest/";
		ShellExecute(
			::GetDesktopWindow(),
			"open",
			"iexplore",
			url,
			NULL,
			SW_SHOWDEFAULT);
	}

	afx_msg void OnIndex()
	{
		//check state
		if (mState != CONTEST_IDLE)
		{
			SetStatus("Inconsistent state detected!");
			return;
		}

		//do we already have this file?
		mFile = db()->FindFile(mCurrentPic.GetValue(), false);
		if (!mFile)
		{
			//save the file
			CString fname = BuildSavedFilename(mCurrentPic);
			FILE *f = fopen(fname, "wb");
			if (f)
			{
				fwrite(mCF.mBuffer, 1, mCF.mBufferSize, f);
				fclose(f);
			}
			else
			{
				CString err;
				err.Format("Could not open file for writing:\n%s", fname);
				AfxMessageBox(err, MB_OK | MB_ICONERROR);
				SetState(CONTEST_ERROR);
				SetStatus(err);
				return;
			}

			//share the file
			mFile = db()->AddFile(
					fname,				//filename
					mCF.mMD5,			//md5
					mCF.mBuffer,		//data
					mCF.mBufferSize,	//data size
					mCF.mWidth,			//width
					mCF.mHeight);		//height

			if (!mFile)
			{
				SetState(CONTEST_ERROR);
				SetStatus("Error sharing file.");
				return;
			}
		}

		//set the file's contest flag
		mFile->GetIndex()->data.Contest = true;

		//start indexing
		QueryBuildIndex(mFile);

		//set the skip button to say 'next picture'
		GetDlgItem(IDC_SKIP)->SetWindowText("Next Picture");
	}

	afx_msg void OnSkip()
	{
		//attempt to stop the current download, query
		if (mState == CONTEST_QUERYING)
		{
			//this is dangerous, since the server won't allow re-entrant queries
			SetState(CONTEST_ERROR);
			SetStatus("Inconsistent state detected!");
			return;
		}
		else if (mState == CONTEST_WAITING)
		{
			//find the queied file, cancel it
			cm()->Lock();
			for (DWORD i=0;i<cm()->GetQueuedFileCount();i++)
			{
				if (cm()->GetQueuedFile(i)->mItem->mMD5.IsEqual(mCurrentPic))
				{
					cm()->CancelQueuedFile(i);
					cm()->Unlock();
					break;
				}
			}
			cm()->Unlock();
		}
		else if (mState == CONTEST_DOWNLOADING)
		{
			//find the download slot, then cancel it
			cm()->Lock();
			for (BYTE i=0;i<cm()->GetDownloadSlotCount();i++)
			{
				if (cm()->GetDownloadSlot(i)->mItem->mMD5.IsEqual(mCurrentPic))
				{
					cm()->CancelDownloadingFile(i);
					cm()->Unlock();
					break;
				}
			}
			cm()->Unlock();
		}

		//clear the completed file
		if (mCF.mBuffer)
		{
			free(mCF.mBuffer);
			mCF.mBuffer = NULL;
			mCF.mBufferSize = 0;
		}

		//start a new query
		CXMQuery *q = new CXMQuery();
		q->mContest = true;
		mCurrentQuery = sm()->QueryBegin(q);
		if (mCurrentQuery == 0)
		{
			SetState(CONTEST_ERROR);
			SetStatus("Error running query.");
			return;
		}

		//we are now 'querying'
		SetState(CONTEST_QUERYING);
	}

	afx_msg LRESULT OnServerMessage(WPARAM wp, LPARAM lp)
	{
		CXMQueryResponse *r;
		
		switch (wp)
		{
		case XM_SMU_QUERY_FINISH:

			//check state
			if ((DWORD)lp != mCurrentQuery ||
				mState != CONTEST_QUERYING)
				break;

			//start downloading the first item
			r = sm()->QueryGetResponse();
			if (r->mFilesCount < 1)
			{
				//no results
				SetState(CONTEST_ERROR);
				SetStatus("Could not find a picture for you to index. Please try again.");
			}
			else
			{
				//get the picture
				CXMQueryResponseItem *qri = r->mFiles[0];
				mCurrentPic = qri->mMD5;

				//do we already have this picture?
				//db()->Lock();
				mFile = db()->FindFile(qri->mMD5.GetValue(), false);
				//db()->Unlock();
				if (mFile)
				{
					//we already have the pic
					mCurrentPic = mFile->GetMD5();

					//get thumb
					CXMDBThumb *thumb = mFile->GetThumb(XMGUI_THUMBWIDTH, XMGUI_THUMBHEIGHT);
					if (!thumb)
					{
						SetState(CONTEST_ERROR);
						SetStatus("Error getting thumbnail for file.");
						break;
					}
					
					//display thumb
					DWORD bufsize = 0;
					BYTE* buf = NULL;
					CJPEGDecoder jpeg;
					CDIBSection dib;
					//CBitmap bmp;
					bufsize = thumb->GetImage(&buf);
					jpeg.MakeBmpFromMemory(buf, bufsize, &dib);
					//bmp.FromHandle((HBITMAP)dib.GetHandle());
					CStatic *prev = (CStatic*)GetDlgItem(IDC_PREVIEW);
					prev->SetBitmap((HBITMAP)dib.GetHandle());
					dib.Detach();
					
					//we are good to go!
					SetState(CONTEST_IDLE);
					break;
				}

				//begin downloading the file
				cm()->Lock();
				if (cm()->EnqueueFile(qri, false, 0, 0) != 0)
				{
					//queued up
					SetState(CONTEST_WAITING);
				}
				else
				{
					//immediate download (or error or something)
					SetState(CONTEST_DOWNLOADING);
				}
				cm()->Unlock();
				
			}
			r->Release();
			break;

		case XM_SMU_QUERY_CANCEL:
		case XM_SMU_QUERY_ERROR:

			SetState(CONTEST_ERROR);
			SetStatus("Error while running query.");
			break;
		}

		return 0;
	}

	afx_msg LRESULT OnClientMessage(WPARAM wp, LPARAM lp)
	{
		//image decoding vars
		CBitmap bmp;
		CDIBSection dib, *thumb;
		CJPEGDecoder jpeg;
		CJPEGEncoder enc;

		//misc
		CXMClientManager::CompletedFile *cf;
		DWORD n;

		//get our tag
		CXMPipelineUpdateTag *tag = (CXMPipelineUpdateTag*)lp;

		//what message?
		switch (wp)
		{
		case XM_CMU_DOWNLOAD_START:

			//do we care?
			if (tag->md5.IsEqual(mCurrentPic))
			{
				SetState(CONTEST_DOWNLOADING);
			}
			break;

		case XM_CMU_DOWNLOAD_REQUESTING:
		case XM_CMU_DOWNLOAD_RECEIVING:
		case XM_CMU_DOWNLOAD_FINISH:
			break;

		case XM_CMU_DOWNLOAD_ERROR:
		case XM_CMU_DOWNLOAD_CANCEL:
			
			//show the error
			if (tag->md5.IsEqual(mCurrentPic))
			{
				SetState(CONTEST_ERROR);
				SetStatus("Error while downloading file.");
			}
			break;

		case XM_CMU_COMPLETED_ADD:

			//our file?
			if (tag->md5.IsEqual(mCurrentPic))
			{
				//get the file
				cm()->Lock();
				n = cm()->FindCompletedFile(tag);
				if (n == -1)
				{
					cm()->Unlock();
					SetState(CONTEST_ERROR);
					SetStatus("Error completing download.");
					tag->Release();
					return 0;
				}
				cf = cm()->GetCompletedFile(n);
				
				//copy cf data to our local var, clear buffer from cf
				memcpy(&mCF, cf, sizeof(CXMClientManager::CompletedFile));
				
				//remove the complete file
				cf->mBuffer = NULL;
				cf->mBufferSize = 0;
				cm()->RemoveCompletedFile(n);
				cm()->Unlock();

				//generate thumbnail
				try
				{
					//decode jpeg
					jpeg.MakeBmpFromMemory(mCF.mBuffer, mCF.mBufferSize, &dib);
					
					//resize
					thumb = FastResize(&dib, XMGUI_THUMBWIDTH, XMGUI_THUMBHEIGHT);
					if (!thumb)
						throw;
					
					//jpeg encode, gen md5, cache file
					CMD5 md5;
					CMemSink mem(0x8000);
					enc.SaveBmp(thumb, &mem);
					md5.FromBuf(mem.GetFullBuffer(), mem.GetDataSize());
					dbman()->CacheFile(
							md5,					//md5 of thumb
							mCurrentPic,			//md5 of tnumb's full pic
							mem.GetFullBuffer(),	//thumbnail buffer
							mem.GetDataSize(),		//size of -^
							TRUE);					//clamp?
					mem.Detach();
				}
				catch(...)
				{	
					SetState(CONTEST_ERROR);
					SetStatus("Error decoding JPEG data.");
					tag->Release();
					return 0;
				}

				//display thumbnail
				CStatic *prev = (CStatic*)GetDlgItem(IDC_PREVIEW);
				prev->SetBitmap((HBITMAP)thumb->GetHandle());
				thumb->Detach();

				//we are now IDLE, waiting for user input
				SetState(CONTEST_IDLE);
			}
			break;
		}
		tag->Release();
		return 0;
	}

	DECLARE_MESSAGE_MAP();
};

BEGIN_MESSAGE_MAP(CContestDialog, CDialog)

	ON_WM_DESTROY()

	ON_BN_CLICKED(IDC_INDEX, OnIndex)
	ON_BN_CLICKED(IDC_SKIP, OnSkip)
	ON_BN_CLICKED(IDC_CONTESTHELP, OnContestHelp)

	ON_MESSAGE(XM_CLIENTMSG, OnClientMessage)
	ON_MESSAGE(XM_SERVERMSG, OnServerMessage)

END_MESSAGE_MAP()

void DoContest(CWnd *pwnd)
{
	CContestDialog dlg(pwnd);
	dlg.DoModal();
}

// ------------------------------------------------------------------------- DOWNLOAD INDEXES

class CUpdateIndexes : public CDialog
{
public:
	CUpdateIndexes(CWnd *pwnd)
		: CDialog(IDD_INDEXER_LISTING, pwnd)
	{
	}
	BOOL OnInitDialog()
	{
		sm()->Lock();
		sm()->RequestListing(m_hWnd);
		sm()->Unlock();
		return FALSE;
	}
	LRESULT OnServerMessage(WPARAM wparam, LPARAM lparam)
	{
		if (wparam==XM_SMU_LISTING_RECEIVE)
		{
			CXMMediaListing *mi = (CXMMediaListing*)lparam;
			mi->Apply();
			mi->Release();
			EndDialog(IDOK);
		}
		return 0;
	}
	DECLARE_MESSAGE_MAP()
};
BEGIN_MESSAGE_MAP(CUpdateIndexes, CDialog)
	ON_MESSAGE(XM_SERVERMSG, OnServerMessage)
END_MESSAGE_MAP()

// -------------------------------------------------------------------------------- DEFINITIONS

class CQueryBuilder : public CDialog
{
public:
	
	CQueryBuilder(CWnd* pParent = NULL);   // standard constructor

	CXMIndex mSearch, mFilter;

	enum { IDD = IDD_QUERY };
	DWORD	mMaxHeight;
	DWORD	mMaxSize;
	DWORD	mMaxWidth;
	DWORD	mMinHeight;
	DWORD	mMinSize;
	DWORD	mMinWidth;

protected:

	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	afx_msg void OnModifySearch();
	afx_msg void OnModifyFilter();
	virtual BOOL OnInitDialog();

	DECLARE_MESSAGE_MAP()
};

class CIndexBuilder : public CPropertySheet
{

// Construction
public:

	CIndexBuilder(int mode, CXMIndex *index, UINT nIDCaption, CWnd* pParentWnd = NULL, UINT iSelectPage = 0);
	CIndexBuilder(int mode, CXMIndex *index, LPCTSTR pszCaption, CWnd* pParentWnd = NULL, UINT iSelectPage = 0);
	virtual ~CIndexBuilder();

	//do the dialog with a preview pane
	int DoModalPreview(CXMDBFile *file)
	{
		m_PreviewEnable = true;
		m_PreviewFile = file;
		return DoModal();
	}

	HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
	{
		//let dialog go first
		HBRUSH hb = CPropertySheet::OnCtlColor(pDC, pWnd, nCtlColor);

		//Everything except username, passwords, and buttons gets
		//black back, white text
		if (pWnd->GetDlgCtrlID()==IDC_SCORE)
		{
			pDC->SetTextColor(RGB(255,0,0));
		}

		return hb;
	}


	//setup the dialog
	CImageViewer m_PreviewCtrl;
	CStatic m_ScoreCtrl;
	#define IMAGEW	300
	#define IMAGEB 4
	int OnCreate(LPCREATESTRUCT lpCreateStruct)
	{
		//property sheet init
		if (CPropertySheet::OnCreate(lpCreateStruct))
			return -1;

		//do we need to display the preview?
		if (m_PreviewEnable)
		{
			//allow maximize
			ModifyStyle(0, WS_MAXIMIZEBOX);

			//expand the window
			CRect r;
			GetWindowRect(r);
			r.right += IMAGEW+IMAGEB*2;
			MoveWindow(r, FALSE);

			//create the image preview
			GetClientRect(r);
			CRect r2(r.right-IMAGEW-IMAGEB, IMAGEB, r.right-IMAGEB, r.bottom-IMAGEB);
			m_PreviewCtrl.Create(
				NULL, NULL, WS_CHILD|WS_VISIBLE,
				r2,
				this, 0);

			//assign the picture
			m_PreviewCtrl.SetFit(true, false);
			m_PreviewCtrl.ShowFile(m_PreviewFile->GetPath());

		}

		if (mIndex->Contest)
		{
			//create the score control
			m_ScoreCtrl.Create("Score: xxx.x", WS_CHILD|WS_VISIBLE,
				CRect(10, 400, 75, 500), this, IDC_SCORE);

			CFont f;
			f.CreateStockObject(ANSI_VAR_FONT);
			m_ScoreCtrl.SetFont(&f, FALSE);

			UpdateScore();
		}

		return 0;
	}

	void OnSize(UINT nType, int cx, int cy)
	{
		CPropertySheet::OnSize(nType, cx, cy);
		
		CTabCtrl *tab = GetTabControl();
		if (m_PreviewCtrl.m_hWnd && tab)
		{
			CRect r;
			tab->GetWindowRect(r);
			ScreenToClient(r);
			CRect r2(r.right+IMAGEB, IMAGEB, cx-IMAGEB, cy-IMAGEB);
			m_PreviewCtrl.MoveWindow(r2, TRUE);
		}
		
	}

	bool CheckLimiter()
	{
		//which mode?
		switch (mMode)
		{
		//check against search limiter
		case XMGUI_INDEXMODE_SEARCH:
			return sm()->LimiterIndex(&mSandbox);
			
		//check against filter limiter
		case XMGUI_INDEXMODE_FILTER:
			return sm()->LimiterFilter(&mSandbox);
			
		//indexes arent checked, of course
		case XMGUI_INDEXMODE_INDEX:
			return true;
		}

		//unknown
		return false;
	}

	void UpdateScore()
	{
		//ignore if not a contest index
		if (mSandbox.Contest)
		{
			if (::IsWindow(m_ScoreCtrl))
			{
				CString str;
				str.Format("Score: %.1f", mSandbox.Score());
				m_ScoreCtrl.SetWindowText(str);
			}		
		}
	}

	afx_msg void OnHelp()
	{
		//is this index for the contest?
		CString url;
		//if (mSandbox.Contest)
		//	url = "http://www.adultmediaswapper.com/contest/";
		//else
			url = "http://www.adultmediaswapper.com/support/software/indexing/";
		
		//open a web browser window with the help
		ShellExecute(
			::GetDesktopWindow(),
			"open",
			"iexplore",
			url,
			NULL,
			SW_SHOWDEFAULT);
	}

//SUB CLASSES FOR EACH PAGE
protected:

	//PREVIEW
	bool m_PreviewEnable;
	CXMDBFile *m_PreviewFile;

	//CATAGORY 1
	class CPageCat1 : public CPropertyPage
	{
	public:
		CIndexBuilder *mParent;
		void DoDataExchange(CDataExchange *pDX);
		void OnOK();

		void OnUpdateScore()
		{			
			UpdateData(TRUE);
			mParent->UpdateScore();
		}
		DECLARE_MESSAGE_MAP();

	} mPageCat1;
	friend class CPageCat1;

	//CATAGORY 2
	class CPageCat2 : public CPropertyPage
	{
	public:
		CIndexBuilder *mParent;
		void DoDataExchange(CDataExchange *pDX);
		void OnOK();

		void OnUpdateScore()
		{	
			UpdateData(TRUE);
			mParent->UpdateScore();
		}
		DECLARE_MESSAGE_MAP();

	} mPageCat2;
	friend class CPageCat2;

	//PICTURE
	class CPagePicture : public CPropertyPage
	{
	public:
		CIndexBuilder *mParent;
		void DoDataExchange(CDataExchange *pDX);
		void OnOK();

		BOOL OnApply()
		{
			//check everything
			if (!mParent->CheckLimiter())
				return FALSE;

			//we are ok, normal apply behavior
			OnOK();
			return TRUE;
		}

		afx_msg void OnClear()
		{
			//reset all the marks for this index
			mParent->mSandbox.Age = 0;
			mParent->mSandbox.Breasts = 0;
			mParent->mSandbox.Build = 0;
			mParent->mSandbox.Butt = 0;
			mParent->mSandbox.Cat1 = 0;
			mParent->mSandbox.Cat2 = 0;
			mParent->mSandbox.Chest = 0;
			mParent->mSandbox.Content = 0;
			mParent->mSandbox.Eyes = 0;
			mParent->mSandbox.FacialHair = 0;
			mParent->mSandbox.FemaleGen = 0;
			mParent->mSandbox.HairColor = 0;
			mParent->mSandbox.HairStyle = 0;
			mParent->mSandbox.Height = 0;
			mParent->mSandbox.Hips = 0;
			mParent->mSandbox.Legs = 0;
			mParent->mSandbox.MaleGen = 0;
			mParent->mSandbox.Nipples = 0;
			mParent->mSandbox.Quality = 0;
			mParent->mSandbox.Quantity = 0;
			mParent->mSandbox.Race = 0;
			mParent->mSandbox.Rating = 0;
			mParent->mSandbox.Setting = 0;
			mParent->mSandbox.Skin = 0;
			
			//update the display
			UpdateData(FALSE);
			mParent->UpdateScore();
		}

		void OnUpdateScore()
		{	
			UpdateData(TRUE);
			mParent->UpdateScore();
		}

		DECLARE_MESSAGE_MAP()

	} mPagePicture;
	friend class CPagePicture;

	//PHYSICAL - GENERAL
	class CPagePhysGen : public CPropertyPage
	{
	public:
		CIndexBuilder *mParent;
		void DoDataExchange(CDataExchange *pDX);
		void OnOK();

		void OnUpdateScore()
		{	
			UpdateData(TRUE);
			mParent->UpdateScore();
		}
		DECLARE_MESSAGE_MAP();

	} mPagePhysGen;
	friend class CPagePhysGen;

	//PHYSICAL - SPECIFIC
	class CPagePhysSpec : public CPropertyPage
	{
	public:
		CIndexBuilder *mParent;
		void DoDataExchange(CDataExchange *pDX);
		void OnOK();

		void OnUpdateScore()
		{	
			UpdateData(TRUE);
			mParent->UpdateScore();
		}
		DECLARE_MESSAGE_MAP();

	} mPagePhysSpec;
	friend class CPagePhysSpec;

	//FEMALE
	class CPageFemale : public CPropertyPage
	{
	public:
		CIndexBuilder *mParent;
		void DoDataExchange(CDataExchange *pDX);
		void OnOK();

		void OnUpdateScore()
		{	
			UpdateData(TRUE);
			mParent->UpdateScore();
		}
		DECLARE_MESSAGE_MAP();

	} mPageFemale;
	friend class CPageFemale;

	//MALE
	class CPageMale : public CPropertyPage
	{
	public:
		CIndexBuilder *mParent;
		void DoDataExchange(CDataExchange *pDX);
		void OnOK();

		void OnUpdateScore()
		{			
			UpdateData(TRUE);
			mParent->UpdateScore();
		}
		DECLARE_MESSAGE_MAP();

	} mPageMale;
	friend class CPageMale;

	int mMode;
	CXMIndex *mIndex;
	CXMIndex mSandbox;
	inline void Construct_Inner(int mode, CXMIndex *index);
	
	DECLARE_MESSAGE_MAP()
};

//-------------------------------------------------------------------------- QUERYBUILDER

CQueryBuilder::CQueryBuilder(CWnd* pParent /*=NULL*/)
	: CDialog(CQueryBuilder::IDD, pParent)
{
	//{{AFX_DATA_INIT(CQueryBuilder)
	mMaxHeight = 0;
	mMaxSize = 0;
	mMaxWidth = 0;
	mMinHeight = 0;
	mMinSize = 0;
	mMinWidth = 0;
	//}}AFX_DATA_INIT
}

#define XM_IMAGESIZE_NONE		0
#define XM_IMAGESIZE_SMALL		1//kb
#define XM_IMAGESIZE_MEDIUM		75//kb
#define XM_IMAGESIZE_LARGE		150//kb


void CQueryBuilder::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	
	/*
	DDX_Text(pDX, IDC_MAXHEIGHT, mMaxHeight);
	DDX_Text(pDX, IDC_MAXSIZE, mMaxSize);
	DDX_Text(pDX, IDC_MAXWIDTH, mMaxWidth);
	DDX_Text(pDX, IDC_MINHEIGHT, mMinHeight);
	DDX_Text(pDX, IDC_MINSIZE, mMinSize);
	DDX_Text(pDX, IDC_MINWIDTH, mMinWidth);
	*/
	if (pDX->m_bSaveAndValidate)
	{
		//set the min image size based on the combo
		int i;
		DDX_CBIndex(pDX, IDC_IMAGESIZE, i);
		switch (i)
		{
		case 0:
			mMinSize = XM_IMAGESIZE_NONE;
			break;
		case 1:
			mMinSize = XM_IMAGESIZE_SMALL;
			break;
		case 2:
			mMinSize = XM_IMAGESIZE_MEDIUM;
			break;
		case 3:
			mMinSize = XM_IMAGESIZE_LARGE;
			break;
		}
		mMaxSize = 0;	//infinite
	}
	else
	{
		int i;
		if (mMinSize >= XM_IMAGESIZE_LARGE)
		{
			//large
			i = 3;
		}
		else if (mMinSize >= XM_IMAGESIZE_MEDIUM)
		{
			//medium
			i = 2;
		}
		else if (mMinSize >= XM_IMAGESIZE_SMALL)
		{
			//small
			i = 1;
		}
		else
		{	
			//0 - not specified
			i = 0; 
		}
		DDX_CBIndex(pDX, IDC_IMAGESIZE, i);
	}
}


BEGIN_MESSAGE_MAP(CQueryBuilder, CDialog)
	//{{AFX_MSG_MAP(CQueryBuilder)
	ON_BN_CLICKED(IDC_QUERY, OnModifySearch)
	ON_BN_CLICKED(IDC_FILTER, OnModifyFilter)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CQueryBuilder message handlers

void CQueryBuilder::OnModifySearch() 
{
	CIndexBuilder dlg(XMGUI_INDEXMODE_SEARCH, &mSearch, "Modify Search Fields");
	dlg.DoModal();
}

void CQueryBuilder::OnModifyFilter() 
{
	CIndexBuilder dlg(XMGUI_INDEXMODE_FILTER, &mFilter, "Modify Filter Fields");
	dlg.DoModal();
}

BOOL CQueryBuilder::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	//set the help text
	SetDlgItemText(IDC_QUERYHELP, CString((LPCSTR)IDC_QUERYHELP));
	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}


//-------------------------------------------------------------------------- INDEXBUILDER

BEGIN_MESSAGE_MAP(CIndexBuilder, CPropertySheet)

	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_BN_CLICKED(IDHELP, OnHelp)

	ON_WM_CTLCOLOR()

END_MESSAGE_MAP()

CIndexBuilder::CIndexBuilder(int mode, CXMIndex* index, UINT nIDCaption, CWnd* pParentWnd, UINT iSelectPage)
	:CPropertySheet(nIDCaption, pParentWnd, iSelectPage)
{
	Construct_Inner(mode, index);
}

CIndexBuilder::CIndexBuilder(int mode, CXMIndex* index, LPCTSTR pszCaption, CWnd* pParentWnd, UINT iSelectPage)
	:CPropertySheet(pszCaption, pParentWnd, iSelectPage)
{
	Construct_Inner(mode, index);
}

//shared construction stuff
void CIndexBuilder::Construct_Inner(int mode, CXMIndex* index)
{
	//setup mode
	mMode = mode;

	//setup preview stuff
	m_PreviewEnable = false;
	m_PreviewFile = NULL;

	//setup index
	mIndex = index;
	memcpy(&mSandbox, index, sizeof(CXMIndex));

	//initialize all the pages
	mPageCat1.Construct(IDD_INDEX_CAT);
	mPageCat2.Construct(IDD_INDEX_CAT2);
	mPagePicture.Construct(IDD_INDEX_PICTURE);
	mPagePhysGen.Construct(IDD_INDEX_PHYSGEN);
	mPagePhysSpec.Construct(IDD_INDEX_PHYSSPEC);
	mPageFemale.Construct(IDD_INDEX_FEMALE);
	mPageMale.Construct(IDD_INDEX_MALE);

	//set each page's ref to us
	mPageCat1.mParent = this;
	mPageCat2.mParent = this;
	mPagePicture.mParent = this;
	mPagePhysGen.mParent = this;
	mPagePhysSpec.mParent = this;
	mPageFemale.mParent = this;
	mPageMale.mParent = this;

	//add each property page
	AddPage(&mPagePicture);
	AddPage(&mPagePhysGen);
	AddPage(&mPagePhysSpec);
	AddPage(&mPageFemale);
	AddPage(&mPageMale);
	AddPage(&mPageCat1);
	AddPage(&mPageCat2);
}

CIndexBuilder::~CIndexBuilder()
{
}



#define IB_GET(_PREFIX, _ITEM) ((IsDlgButtonChecked(_PREFIX##_ITEM)?1:0)<<_ITEM-1)
#define IB_SET(_FIELD, _PREFIX, _ITEM) \
	CheckDlgButton(_PREFIX##_ITEM, (mParent->mSandbox._FIELD&(1<<_ITEM-1))?BST_CHECKED:BST_UNCHECKED);

BEGIN_MESSAGE_MAP(CIndexBuilder::CPageCat1, CPropertyPage)
	
	ON_BN_CLICKED(IDC_CAT1, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT2, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT3, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT4, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT5, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT6, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT7, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT8, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT9, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT10, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT11, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT12, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT13, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT14, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT15, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT16, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT17, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT18, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT19, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT20, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT21, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT22, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT23, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT24, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT25, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT26, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT27, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT28, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT29, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT30, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT31, CPageCat1::OnUpdateScore)

END_MESSAGE_MAP()

void CIndexBuilder::CPageCat1::DoDataExchange(CDataExchange *pDX)
{
	if (pDX->m_bSaveAndValidate)
	{
		mParent->mSandbox.Cat1 = 
			IB_GET(IDC_CAT,  1) | IB_GET(IDC_CAT,  2) | IB_GET(IDC_CAT,  3) |
			IB_GET(IDC_CAT,  4) | IB_GET(IDC_CAT,  5) | IB_GET(IDC_CAT,  6) |
			IB_GET(IDC_CAT,  7) | IB_GET(IDC_CAT,  8) | IB_GET(IDC_CAT,  9) |
			IB_GET(IDC_CAT, 10) | IB_GET(IDC_CAT, 11) | IB_GET(IDC_CAT, 12) |
			IB_GET(IDC_CAT, 13) | IB_GET(IDC_CAT, 14) | IB_GET(IDC_CAT, 15) |
			IB_GET(IDC_CAT, 16) | IB_GET(IDC_CAT, 17) | IB_GET(IDC_CAT, 18) |
			IB_GET(IDC_CAT, 19) | IB_GET(IDC_CAT, 20) | IB_GET(IDC_CAT, 21) |
			IB_GET(IDC_CAT, 22) | IB_GET(IDC_CAT, 23) | IB_GET(IDC_CAT, 24) |
			IB_GET(IDC_CAT, 25) | IB_GET(IDC_CAT, 26) | IB_GET(IDC_CAT, 27) |
			IB_GET(IDC_CAT, 28) | IB_GET(IDC_CAT, 29) | IB_GET(IDC_CAT, 30) |
			IB_GET(IDC_CAT, 31);
	}
	else
	{
		IB_SET(Cat1, IDC_CAT,  1);
		IB_SET(Cat1, IDC_CAT,  2);
		IB_SET(Cat1, IDC_CAT,  3);
		IB_SET(Cat1, IDC_CAT,  4);
		IB_SET(Cat1, IDC_CAT,  5);
		IB_SET(Cat1, IDC_CAT,  6);
		IB_SET(Cat1, IDC_CAT,  7);
		IB_SET(Cat1, IDC_CAT,  8);
		IB_SET(Cat1, IDC_CAT,  9);
		IB_SET(Cat1, IDC_CAT, 10);
		IB_SET(Cat1, IDC_CAT, 11);
		IB_SET(Cat1, IDC_CAT, 12);
		IB_SET(Cat1, IDC_CAT, 13);
		IB_SET(Cat1, IDC_CAT, 14);
		IB_SET(Cat1, IDC_CAT, 15);
		IB_SET(Cat1, IDC_CAT, 16);
		IB_SET(Cat1, IDC_CAT, 17);
		IB_SET(Cat1, IDC_CAT, 18);
		IB_SET(Cat1, IDC_CAT, 19);
		IB_SET(Cat1, IDC_CAT, 20);
		IB_SET(Cat1, IDC_CAT, 21);
		IB_SET(Cat1, IDC_CAT, 22);
		IB_SET(Cat1, IDC_CAT, 23);
		IB_SET(Cat1, IDC_CAT, 24);
		IB_SET(Cat1, IDC_CAT, 25);
		IB_SET(Cat1, IDC_CAT, 26);
		IB_SET(Cat1, IDC_CAT, 27);
		IB_SET(Cat1, IDC_CAT, 28);
		IB_SET(Cat1, IDC_CAT, 29);
		IB_SET(Cat1, IDC_CAT, 30);
		IB_SET(Cat1, IDC_CAT, 31);
	}
}

void CIndexBuilder::CPageCat1::OnOK()
{
	//mParent->mIndex->Cat1 = mParent->mSandbox.Cat1;
}

BEGIN_MESSAGE_MAP(CIndexBuilder::CPageCat2, CPropertyPage)
	
	ON_BN_CLICKED(IDC_CAT1, CPageCat1::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT2, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT3, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT4, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT5, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT6, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT7, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT8, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT9, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT10, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT11, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT12, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT13, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT14, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT15, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT16, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT17, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT18, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT19, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT20, CPageCat2::OnUpdateScore)
	ON_BN_CLICKED(IDC_CAT21, CPageCat2::OnUpdateScore)

END_MESSAGE_MAP()

void CIndexBuilder::CPageCat2::DoDataExchange(CDataExchange *pDX)
{
	if (pDX->m_bSaveAndValidate)
	{
		mParent->mSandbox.Cat2 = 
			IB_GET(IDC_CAT,  1) | IB_GET(IDC_CAT,  2) | IB_GET(IDC_CAT,  3) |
			IB_GET(IDC_CAT,  4) | IB_GET(IDC_CAT,  5) | IB_GET(IDC_CAT,  6) |
			IB_GET(IDC_CAT,  7) | IB_GET(IDC_CAT,  8) | IB_GET(IDC_CAT,  9) |
			IB_GET(IDC_CAT, 10) | IB_GET(IDC_CAT, 11) | IB_GET(IDC_CAT, 12) |
			IB_GET(IDC_CAT, 13) | IB_GET(IDC_CAT, 14) | IB_GET(IDC_CAT, 15) |
			IB_GET(IDC_CAT, 16) | IB_GET(IDC_CAT, 17) | IB_GET(IDC_CAT, 18) |
			IB_GET(IDC_CAT, 19) | IB_GET(IDC_CAT, 20) | IB_GET(IDC_CAT, 21);
	}
	else
	{
		IB_SET(Cat2, IDC_CAT,  1);
		IB_SET(Cat2, IDC_CAT,  2);
		IB_SET(Cat2, IDC_CAT,  3);
		IB_SET(Cat2, IDC_CAT,  4);
		IB_SET(Cat2, IDC_CAT,  5);
		IB_SET(Cat2, IDC_CAT,  6);
		IB_SET(Cat2, IDC_CAT,  7);
		IB_SET(Cat2, IDC_CAT,  8);
		IB_SET(Cat2, IDC_CAT,  9);
		IB_SET(Cat2, IDC_CAT, 10);
		IB_SET(Cat2, IDC_CAT, 11);
		IB_SET(Cat2, IDC_CAT, 12);
		IB_SET(Cat2, IDC_CAT, 13);
		IB_SET(Cat2, IDC_CAT, 14);
		IB_SET(Cat2, IDC_CAT, 15);
		IB_SET(Cat2, IDC_CAT, 16);
		IB_SET(Cat2, IDC_CAT, 17);
		IB_SET(Cat2, IDC_CAT, 18);
		IB_SET(Cat2, IDC_CAT, 19);
		IB_SET(Cat2, IDC_CAT, 20);
		IB_SET(Cat2, IDC_CAT, 21);
	}
}

void CIndexBuilder::CPageCat2::OnOK()
{
	//mParent->mIndex->Cat2 = mParent->mSandbox.Cat2;
}

BEGIN_MESSAGE_MAP(CIndexBuilder::CPagePicture, CPropertyPage)

	ON_BN_CLICKED(IDC_SETTING1, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_SETTING2, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_SETTING3, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_SETTING4, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_SETTING5, CPagePicture::OnUpdateScore)

	ON_BN_CLICKED(IDC_GENDER1, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_GENDER2, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_GENDER3, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_GENDER4, CPagePicture::OnUpdateScore)

	ON_BN_CLICKED(IDC_QUANTITY1, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_QUANTITY2, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_QUANTITY3, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_QUANTITY4, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_QUANTITY5, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_QUANTITY6, CPagePicture::OnUpdateScore)
	
	ON_BN_CLICKED(IDC_QUALITY1, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_QUALITY2, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_QUALITY3, CPagePicture::OnUpdateScore)

	ON_BN_CLICKED(IDC_RATING1, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_RATING2, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_RATING3, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_RATING4, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_RATING5, CPagePicture::OnUpdateScore)
	ON_BN_CLICKED(IDC_RATING6, CPagePicture::OnUpdateScore)

	ON_BN_CLICKED(IDC_CLEAR, CPagePicture::OnClear)

END_MESSAGE_MAP()

void CIndexBuilder::CPagePicture::DoDataExchange(CDataExchange *pDX)
{
	if (pDX->m_bSaveAndValidate)
	{
		mParent->mSandbox.Setting = 
			IB_GET(IDC_SETTING,  1) | IB_GET(IDC_SETTING,  2) | IB_GET(IDC_SETTING,  3) |
			IB_GET(IDC_SETTING,  4) | IB_GET(IDC_SETTING,  5);
	
		mParent->mSandbox.Content = 
			IB_GET(IDC_GENDER,  1) | IB_GET(IDC_GENDER,  2) | IB_GET(IDC_GENDER,  3) |
			IB_GET(IDC_GENDER,  4);
		
		mParent->mSandbox.Quantity = 
			IB_GET(IDC_QUANTITY,  1) | IB_GET(IDC_QUANTITY,  2) | IB_GET(IDC_QUANTITY,  3) |
			IB_GET(IDC_QUANTITY,  4) | IB_GET(IDC_QUANTITY,  5) | IB_GET(IDC_QUANTITY,  6);
		
		mParent->mSandbox.Quality = 
			IB_GET(IDC_QUALITY,  1) | IB_GET(IDC_QUALITY,  2) | IB_GET(IDC_QUALITY,  3);

		mParent->mSandbox.Rating = 
			IB_GET(IDC_RATING,  1) | IB_GET(IDC_RATING,  2) | IB_GET(IDC_RATING,  3) |
			IB_GET(IDC_RATING,  4) | IB_GET(IDC_RATING,  5) | IB_GET(IDC_RATING,  6);
	}
	else
	{
		IB_SET(Setting, IDC_SETTING,  1);
		IB_SET(Setting, IDC_SETTING,  2);
		IB_SET(Setting, IDC_SETTING,  3);
		IB_SET(Setting, IDC_SETTING,  4);
		IB_SET(Setting, IDC_SETTING,  5);
		
		IB_SET(Content, IDC_GENDER,  1);
		IB_SET(Content, IDC_GENDER,  2);
		IB_SET(Content, IDC_GENDER,  3);
		IB_SET(Content, IDC_GENDER,  4);
		
		IB_SET(Quantity, IDC_QUANTITY,  1);
		IB_SET(Quantity, IDC_QUANTITY,  2);
		IB_SET(Quantity, IDC_QUANTITY,  3);
		IB_SET(Quantity, IDC_QUANTITY,  4);
		IB_SET(Quantity, IDC_QUANTITY,  5);
		IB_SET(Quantity, IDC_QUANTITY,  6);

		IB_SET(Quality, IDC_QUALITY,  1);
		IB_SET(Quality, IDC_QUALITY,  2);
		IB_SET(Quality, IDC_QUALITY,  3);

		IB_SET(Rating, IDC_RATING,  1);
		IB_SET(Rating, IDC_RATING,  2);
		IB_SET(Rating, IDC_RATING,  3);
		IB_SET(Rating, IDC_RATING,  4);
		IB_SET(Rating, IDC_RATING,  5);
		IB_SET(Rating, IDC_RATING,  6);

	}
}

void CIndexBuilder::CPagePicture::OnOK()
{
	/*
	mParent->mIndex->Setting = mParent->mSandbox.Setting;
	mParent->mIndex->Content = mParent->mSandbox.Content;
	mParent->mIndex->Quantity = mParent->mSandbox.Quantity;
	mParent->mIndex->Quality = mParent->mSandbox.Quality;
	mParent->mIndex->Rating = mParent->mSandbox.Rating;
	*/
	mParent->mSandbox.CopyTo(mParent->mIndex);
}

BEGIN_MESSAGE_MAP(CIndexBuilder::CPagePhysGen, CPropertyPage)

	ON_BN_CLICKED(IDC_BUILD1, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_BUILD2, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_BUILD3, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_BUILD4, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_BUILD5, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_BUILD6, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_BUILD7, CPagePhysGen::OnUpdateScore)

	ON_BN_CLICKED(IDC_RACE1, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_RACE2, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_RACE3, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_RACE4, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_RACE5, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_RACE6, CPagePhysGen::OnUpdateScore)

	ON_BN_CLICKED(IDC_AGE1, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_AGE2, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_AGE3, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_AGE4, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_AGE5, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_AGE6, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_AGE7, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_AGE8, CPagePhysGen::OnUpdateScore)

	ON_BN_CLICKED(IDC_HEIGHT1, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_HEIGHT2, CPagePhysGen::OnUpdateScore)
	ON_BN_CLICKED(IDC_HEIGHT3, CPagePhysGen::OnUpdateScore)

END_MESSAGE_MAP()

void CIndexBuilder::CPagePhysGen::DoDataExchange(CDataExchange *pDX)
{
	if (pDX->m_bSaveAndValidate)
	{
		mParent->mSandbox.Build = 
			IB_GET(IDC_BUILD,  1) | IB_GET(IDC_BUILD,  2) | IB_GET(IDC_BUILD,  3) |
			IB_GET(IDC_BUILD,  4) | IB_GET(IDC_BUILD,  5) | IB_GET(IDC_BUILD,  6) |
			IB_GET(IDC_BUILD,  7);
		
		mParent->mSandbox.Race = 
			IB_GET(IDC_RACE,  1) | IB_GET(IDC_RACE,  2) | IB_GET(IDC_RACE,  3) |
			IB_GET(IDC_RACE,  4) | IB_GET(IDC_RACE,  5) | IB_GET(IDC_RACE,  6);
		
		mParent->mSandbox.Age = 
			IB_GET(IDC_AGE,  1) | IB_GET(IDC_AGE,  2) | IB_GET(IDC_AGE,  3) |
			IB_GET(IDC_AGE,  4) | IB_GET(IDC_AGE,  5) | IB_GET(IDC_AGE,  6) |
			IB_GET(IDC_AGE,  7) | IB_GET(IDC_AGE,  8);
		
		mParent->mSandbox.Height = 
			IB_GET(IDC_HEIGHT,  1) | IB_GET(IDC_HEIGHT,  2) | IB_GET(IDC_HEIGHT,  3);
	}
	else
	{
		IB_SET(Build, IDC_BUILD,  1);
		IB_SET(Build, IDC_BUILD,  2);
		IB_SET(Build, IDC_BUILD,  3);
		IB_SET(Build, IDC_BUILD,  4);
		IB_SET(Build, IDC_BUILD,  5);
		IB_SET(Build, IDC_BUILD,  6);
		IB_SET(Build, IDC_BUILD,  7);

		IB_SET(Race, IDC_RACE,  1);
		IB_SET(Race, IDC_RACE,  2);
		IB_SET(Race, IDC_RACE,  3);
		IB_SET(Race, IDC_RACE,  4);
		IB_SET(Race, IDC_RACE,  5);
		IB_SET(Race, IDC_RACE,  6);

		IB_SET(Age, IDC_AGE,  1);
		IB_SET(Age, IDC_AGE,  2);
		IB_SET(Age, IDC_AGE,  3);
		IB_SET(Age, IDC_AGE,  4);
		IB_SET(Age, IDC_AGE,  5);
		IB_SET(Age, IDC_AGE,  6);
		IB_SET(Age, IDC_AGE,  7);
		IB_SET(Age, IDC_AGE,  8);

		IB_SET(Height, IDC_HEIGHT,  1);
		IB_SET(Height, IDC_HEIGHT,  2);
		IB_SET(Height, IDC_HEIGHT,  3);
	}
}

void CIndexBuilder::CPagePhysGen::OnOK()
{
	/*
	mParent->mIndex->Build = mParent->mSandbox.Build;
	mParent->mIndex->Race = mParent->mSandbox.Race;
	mParent->mIndex->Age = mParent->mSandbox.Age;
	mParent->mIndex->Height = mParent->mSandbox.Height;
	*/
}

BEGIN_MESSAGE_MAP(CIndexBuilder::CPagePhysSpec, CPropertyPage)

	ON_BN_CLICKED(IDC_HCOLOR1, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_HCOLOR2, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_HCOLOR3, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_HCOLOR4, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_HCOLOR5, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_HCOLOR6, CPagePhysSpec::OnUpdateScore)

	ON_BN_CLICKED(IDC_HSTYLE1, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_HSTYLE2, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_HSTYLE3, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_HSTYLE4, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_HSTYLE5, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_HSTYLE6, CPagePhysSpec::OnUpdateScore)

	ON_BN_CLICKED(IDC_LEGS1, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_LEGS2, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_LEGS3, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_LEGS4, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_LEGS5, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_LEGS6, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_LEGS7, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_LEGS8, CPagePhysSpec::OnUpdateScore)

	ON_BN_CLICKED(IDC_SKIN1, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_SKIN2, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_SKIN3, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_SKIN4, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_SKIN5, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_SKIN6, CPagePhysSpec::OnUpdateScore)

	ON_BN_CLICKED(IDC_BUTT1, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_BUTT2, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_BUTT3, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_BUTT4, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_BUTT5, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_BUTT6, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_BUTT7, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_BUTT8, CPagePhysSpec::OnUpdateScore)

	ON_BN_CLICKED(IDC_EYES1, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_EYES2, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_EYES3, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_EYES4, CPagePhysSpec::OnUpdateScore)
	ON_BN_CLICKED(IDC_EYES5, CPagePhysSpec::OnUpdateScore)

END_MESSAGE_MAP()

void CIndexBuilder::CPagePhysSpec::DoDataExchange(CDataExchange *pDX)
{
	if (pDX->m_bSaveAndValidate)
	{
		mParent->mSandbox.HairColor = 
			IB_GET(IDC_HCOLOR,  1) | IB_GET(IDC_HCOLOR,  2) | IB_GET(IDC_HCOLOR,  3) |
			IB_GET(IDC_HCOLOR,  4) | IB_GET(IDC_HCOLOR,  5) | IB_GET(IDC_HCOLOR,  6);

		mParent->mSandbox.HairStyle = 
			IB_GET(IDC_HSTYLE,  1) | IB_GET(IDC_HSTYLE,  2) | IB_GET(IDC_HSTYLE,  3) |
			IB_GET(IDC_HSTYLE,  4) | IB_GET(IDC_HSTYLE,  5) | IB_GET(IDC_HSTYLE,  6);

		mParent->mSandbox.Legs = 
			IB_GET(IDC_LEGS,  1) | IB_GET(IDC_LEGS,  2) | IB_GET(IDC_LEGS,  3) |
			IB_GET(IDC_LEGS,  4) | IB_GET(IDC_LEGS,  5) | IB_GET(IDC_LEGS,  6) |
			IB_GET(IDC_LEGS,  7) | IB_GET(IDC_LEGS,  8);

		mParent->mSandbox.Skin = 
			IB_GET(IDC_SKIN,  1) | IB_GET(IDC_SKIN,  2) | IB_GET(IDC_SKIN,  3) |
			IB_GET(IDC_SKIN,  4) | IB_GET(IDC_SKIN,  5) | IB_GET(IDC_SKIN,  6);

		mParent->mSandbox.Butt = 
			IB_GET(IDC_BUTT,  1) | IB_GET(IDC_BUTT,  2) | IB_GET(IDC_BUTT,  3) |
			IB_GET(IDC_BUTT,  4) | IB_GET(IDC_BUTT,  5) | IB_GET(IDC_BUTT,  6) |
			IB_GET(IDC_BUTT,  7) | IB_GET(IDC_BUTT,  8);

		mParent->mSandbox.Eyes = 
			IB_GET(IDC_EYES,  1) | IB_GET(IDC_EYES,  2) | IB_GET(IDC_EYES,  3) |
			IB_GET(IDC_EYES,  4) | IB_GET(IDC_EYES,  5);
	}
	else
	{
		IB_SET(HairColor, IDC_HCOLOR,  1);
		IB_SET(HairColor, IDC_HCOLOR,  2);
		IB_SET(HairColor, IDC_HCOLOR,  3);
		IB_SET(HairColor, IDC_HCOLOR,  4);
		IB_SET(HairColor, IDC_HCOLOR,  5);
		IB_SET(HairColor, IDC_HCOLOR,  6);

		IB_SET(HairStyle, IDC_HSTYLE,  1);
		IB_SET(HairStyle, IDC_HSTYLE,  2);
		IB_SET(HairStyle, IDC_HSTYLE,  3);
		IB_SET(HairStyle, IDC_HSTYLE,  4);
		IB_SET(HairStyle, IDC_HSTYLE,  5);
		IB_SET(HairStyle, IDC_HSTYLE,  6);

		IB_SET(Legs, IDC_LEGS,  1);
		IB_SET(Legs, IDC_LEGS,  2);
		IB_SET(Legs, IDC_LEGS,  3);
		IB_SET(Legs, IDC_LEGS,  4);
		IB_SET(Legs, IDC_LEGS,  5);
		IB_SET(Legs, IDC_LEGS,  6);
		IB_SET(Legs, IDC_LEGS,  7);
		IB_SET(Legs, IDC_LEGS,  8);

		IB_SET(Skin, IDC_SKIN,  1);
		IB_SET(Skin, IDC_SKIN,  2);
		IB_SET(Skin, IDC_SKIN,  3);
		IB_SET(Skin, IDC_SKIN,  4);
		IB_SET(Skin, IDC_SKIN,  5);
		IB_SET(Skin, IDC_SKIN,  6);

		IB_SET(Butt, IDC_BUTT,  1);
		IB_SET(Butt, IDC_BUTT,  2);
		IB_SET(Butt, IDC_BUTT,  3);
		IB_SET(Butt, IDC_BUTT,  4);
		IB_SET(Butt, IDC_BUTT,  5);
		IB_SET(Butt, IDC_BUTT,  6);
		IB_SET(Butt, IDC_BUTT,  7);
		IB_SET(Butt, IDC_BUTT,  8);

		IB_SET(Eyes, IDC_EYES,  1);
		IB_SET(Eyes, IDC_EYES,  2);
		IB_SET(Eyes, IDC_EYES,  3);
		IB_SET(Eyes, IDC_EYES,  4);
		IB_SET(Eyes, IDC_EYES,  5);
	}
}

void CIndexBuilder::CPagePhysSpec::OnOK()
{
	/*
	mParent->mIndex->HairColor = mParent->mSandbox.HairColor;
	mParent->mIndex->HairStyle = mParent->mSandbox.HairStyle;
	mParent->mIndex->Legs = mParent->mSandbox.Legs;
	mParent->mIndex->Skin = mParent->mSandbox.Skin;
	mParent->mIndex->Butt = mParent->mSandbox.Butt;
	mParent->mIndex->Eyes = mParent->mSandbox.Eyes;
	*/
}

BEGIN_MESSAGE_MAP(CIndexBuilder::CPageFemale, CPropertyPage)

	ON_BN_CLICKED(IDC_NIPPLES1, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_NIPPLES2, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_NIPPLES3, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_NIPPLES4, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_NIPPLES5, CPageFemale::OnUpdateScore)

	ON_BN_CLICKED(IDC_HIPS1, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_HIPS2, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_HIPS3, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_HIPS4, CPageFemale::OnUpdateScore)

	ON_BN_CLICKED(IDC_BREASTS1, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_BREASTS2, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_BREASTS3, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_BREASTS4, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_BREASTS5, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_BREASTS6, CPageFemale::OnUpdateScore)

	ON_BN_CLICKED(IDC_FEMGEN1, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_FEMGEN2, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_FEMGEN3, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_FEMGEN4, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_FEMGEN5, CPageFemale::OnUpdateScore)
	ON_BN_CLICKED(IDC_FEMGEN6, CPageFemale::OnUpdateScore)

END_MESSAGE_MAP()


void CIndexBuilder::CPageFemale::DoDataExchange(CDataExchange *pDX)
{
	if (pDX->m_bSaveAndValidate)
	{
		mParent->mSandbox.Nipples = 
			IB_GET(IDC_NIPPLES,  1) | IB_GET(IDC_NIPPLES,  2) | IB_GET(IDC_NIPPLES,  3) |
			IB_GET(IDC_NIPPLES,  4) | IB_GET(IDC_NIPPLES,  5);

		mParent->mSandbox.Hips = 
			IB_GET(IDC_HIPS,  1) | IB_GET(IDC_HIPS,  2) | IB_GET(IDC_HIPS,  3) |
			IB_GET(IDC_HIPS,  4);

		mParent->mSandbox.Breasts = 
			IB_GET(IDC_BREASTS,  1) | IB_GET(IDC_BREASTS,  2) | IB_GET(IDC_BREASTS,  3) |
			IB_GET(IDC_BREASTS,  4) | IB_GET(IDC_BREASTS,  5) | IB_GET(IDC_BREASTS,  6);

		mParent->mSandbox.FemaleGen = 
			IB_GET(IDC_FEMGEN,  1) | IB_GET(IDC_FEMGEN,  2) | IB_GET(IDC_FEMGEN,  3) |
			IB_GET(IDC_FEMGEN,  4) | IB_GET(IDC_FEMGEN,  5) | IB_GET(IDC_FEMGEN,  6);
	}
	else
	{
		IB_SET(Nipples, IDC_NIPPLES,  1);
		IB_SET(Nipples, IDC_NIPPLES,  2);
		IB_SET(Nipples, IDC_NIPPLES,  3);
		IB_SET(Nipples, IDC_NIPPLES,  4);
		IB_SET(Nipples, IDC_NIPPLES,  5);
		
		IB_SET(Hips, IDC_HIPS,  1);
		IB_SET(Hips, IDC_HIPS,  2);
		IB_SET(Hips, IDC_HIPS,  3);
		IB_SET(Hips, IDC_HIPS,  4);
		
		IB_SET(Breasts, IDC_BREASTS,  1);
		IB_SET(Breasts, IDC_BREASTS,  2);
		IB_SET(Breasts, IDC_BREASTS,  3);
		IB_SET(Breasts, IDC_BREASTS,  4);
		IB_SET(Breasts, IDC_BREASTS,  5);
		IB_SET(Breasts, IDC_BREASTS,  6);

		IB_SET(FemaleGen, IDC_FEMGEN,  1);
		IB_SET(FemaleGen, IDC_FEMGEN,  2);
		IB_SET(FemaleGen, IDC_FEMGEN,  3);
		IB_SET(FemaleGen, IDC_FEMGEN,  4);
		IB_SET(FemaleGen, IDC_FEMGEN,  5);
		IB_SET(FemaleGen, IDC_FEMGEN,  6);
	}
}

void CIndexBuilder::CPageFemale::OnOK()
{
	/*
	mParent->mIndex->Nipples = mParent->mSandbox.Nipples;
	mParent->mIndex->Hips = mParent->mSandbox.Hips;
	mParent->mIndex->Breasts = mParent->mSandbox.Breasts;
	mParent->mIndex->FemaleGen = mParent->mSandbox.FemaleGen;
	*/
}

BEGIN_MESSAGE_MAP(CIndexBuilder::CPageMale, CPropertyPage)

	ON_BN_CLICKED(IDC_MALEGEN1, CPageMale::OnUpdateScore)
	ON_BN_CLICKED(IDC_MALEGEN2, CPageMale::OnUpdateScore)
	ON_BN_CLICKED(IDC_MALEGEN3, CPageMale::OnUpdateScore)
	ON_BN_CLICKED(IDC_MALEGEN4, CPageMale::OnUpdateScore)
	ON_BN_CLICKED(IDC_MALEGEN5, CPageMale::OnUpdateScore)
	ON_BN_CLICKED(IDC_MALEGEN6, CPageMale::OnUpdateScore)
	ON_BN_CLICKED(IDC_MALEGEN7, CPageMale::OnUpdateScore)

	ON_BN_CLICKED(IDC_FHAIR1, CPageMale::OnUpdateScore)
	ON_BN_CLICKED(IDC_FHAIR2, CPageMale::OnUpdateScore)
	ON_BN_CLICKED(IDC_FHAIR3, CPageMale::OnUpdateScore)
	ON_BN_CLICKED(IDC_FHAIR4, CPageMale::OnUpdateScore)
	ON_BN_CLICKED(IDC_FHAIR5, CPageMale::OnUpdateScore)
	ON_BN_CLICKED(IDC_FHAIR6, CPageMale::OnUpdateScore)

	ON_BN_CLICKED(IDC_CHEST1, CPageMale::OnUpdateScore)
	ON_BN_CLICKED(IDC_CHEST2, CPageMale::OnUpdateScore)
	ON_BN_CLICKED(IDC_CHEST3, CPageMale::OnUpdateScore)
	ON_BN_CLICKED(IDC_CHEST4, CPageMale::OnUpdateScore)
	ON_BN_CLICKED(IDC_CHEST5, CPageMale::OnUpdateScore)

END_MESSAGE_MAP()

void CIndexBuilder::CPageMale::DoDataExchange(CDataExchange *pDX)
{
	if (pDX->m_bSaveAndValidate)
	{
		mParent->mSandbox.MaleGen = 
			IB_GET(IDC_MALEGEN,  1) | IB_GET(IDC_MALEGEN,  2) | IB_GET(IDC_MALEGEN,  3) |
			IB_GET(IDC_MALEGEN,  4) | IB_GET(IDC_MALEGEN,  5) | IB_GET(IDC_MALEGEN,  6) |
			IB_GET(IDC_MALEGEN,  7);
		
		mParent->mSandbox.FacialHair = 
			IB_GET(IDC_FHAIR,  1) | IB_GET(IDC_FHAIR,  2) | IB_GET(IDC_FHAIR,  3) |
			IB_GET(IDC_FHAIR,  4) | IB_GET(IDC_FHAIR,  5) | IB_GET(IDC_FHAIR,  6);
	
		mParent->mSandbox.Chest = 
			IB_GET(IDC_CHEST,  1) | IB_GET(IDC_CHEST,  2) | IB_GET(IDC_CHEST,  3) |
			IB_GET(IDC_CHEST,  4) | IB_GET(IDC_CHEST,  5);
	}
	else
	{
		IB_SET(MaleGen, IDC_MALEGEN,  1);
		IB_SET(MaleGen, IDC_MALEGEN,  2);
		IB_SET(MaleGen, IDC_MALEGEN,  3);
		IB_SET(MaleGen, IDC_MALEGEN,  4);
		IB_SET(MaleGen, IDC_MALEGEN,  5);
		IB_SET(MaleGen, IDC_MALEGEN,  6);
		IB_SET(MaleGen, IDC_MALEGEN,  7);

		IB_SET(FacialHair, IDC_FHAIR,  1);
		IB_SET(FacialHair, IDC_FHAIR,  2);
		IB_SET(FacialHair, IDC_FHAIR,  3);
		IB_SET(FacialHair, IDC_FHAIR,  4);
		IB_SET(FacialHair, IDC_FHAIR,  5);
		IB_SET(FacialHair, IDC_FHAIR,  6);

		IB_SET(Chest, IDC_CHEST,  1);
		IB_SET(Chest, IDC_CHEST,  2);
		IB_SET(Chest, IDC_CHEST,  3);
		IB_SET(Chest, IDC_CHEST,  4);
		IB_SET(Chest, IDC_CHEST,  5);
	}
}

void CIndexBuilder::CPageMale::OnOK()
{
	/*
	mParent->mIndex->MaleGen = mParent->mSandbox.MaleGen;
	mParent->mIndex->FacialHair = mParent->mSandbox.FacialHair;
	mParent->mIndex->Chest = mParent->mSandbox.Chest;
	*/
}

// ----------------------------------------------------------------------------------- FAST INDEXER

//only allow this to be built in internal builds.. building
//the "distro" configuration will exclude this code
#ifdef _INTERNAL

//id's for stuff
#define XBASE				4096
//#define ID_FIELDS_WAIT		XBASE+0
//#define ID_FIELDS_NEXT		XBASE+1
#define ID_FIELDS_CHANGE	XBASE+2
#define ID_FIELDS_REWIND	XBASE+3
#define ID_FIELDS_JUMP		XBASE+4
#define ID_FIELDS_SAVE		XBASE+5
#define IDC_INDEXER_IMAGE	XBASE+16
#define IDC_INDEXER_FIELDS	XBASE+17
#define IDC_INDEXER_BACK	XBASE+18

class CIndexer;

class CIndexer_ChooseField : public CDialog
{
public:
	CIndexer* pi;
	CIndexer_ChooseField(CIndexer *pwnd)
		: CDialog(IDD_INDEXER_CHANGEFIELD, (CWnd*)pwnd)
	{
		pi = pwnd;
	}
	BOOL OnInitDialog();
	void DoDataExchange(CDataExchange *pDX)
	{
		DDX_LBIndex(pDX, IDC_LIST1, x);
	}
	int x;
	DECLARE_MESSAGE_MAP()
};

BEGIN_MESSAGE_MAP(CIndexer_ChooseField, CDialog)
END_MESSAGE_MAP()

class CIndexer_JumpTo : public CDialog
{
public:
	long mJumpTo;
	CIndexer_JumpTo(CWnd *pwnd) :
	  CDialog(IDD_INDEXER_JUMPTO, pwnd)
	  {
	  }
	void DoDataExchange(CDataExchange *pDX)
	{
		DDX_Text(pDX, IDC_EDIT1, mJumpTo);
	}
};

class CIndexer : public CFrameWnd
{
friend class CIndexer_ChooseField;
public:

	//creation
	CIndexer()
	{
		//Create ourself
		PopulateFields();
		CString strWndClass = AfxRegisterWndClass (
									0,
									AfxGetApp()->LoadStandardCursor(IDC_ARROW),
									(HBRUSH)(COLOR_3DFACE+1),
									AfxGetApp()->LoadIcon(IDR_MAINFRAME));
		Create(strWndClass, "Adult Media Swapper - Fast Indexer");

	}
	~CIndexer()
	{
		//free fields
		if (mFields)
		{
			for (int i=0;i<mFieldCount;i++)
				if (mFields[i].value)
					delete mFields[i].value;
			delete mFields;
		}
	}

private:

	//data structure for fields
	void PopulateFields();
	struct field
	{
		char *name;
		int count;
		char **value;
	};
	int mFieldCount;
	field *mFields;

	//state data
	int mdCurrentField;
	DWORD mdCurrentFile;
	DWORD mdLastFile;
	bool mdSetTimer;
	int mdCount;

	//controls
	CStatic mcField, mcStatus, mcChoice, mcThumb;
	CButton mcBack;
	CToolBarCtrl mcToolbar;
	CCheckListBox mcFields;
	CImageViewer mcImage;

	//window creation
	DECLARE_MESSAGE_MAP();
	BOOL PreCreateWindow(CREATESTRUCT &cs)
	{
		if( !CFrameWnd::PreCreateWindow(cs) )
			return FALSE;
		cs.style |= WS_CLIPCHILDREN|WS_CLIPSIBLINGS;
		cs.dwExStyle &= ~WS_EX_CLIENTEDGE;
		return TRUE;
	}
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct)
	{
		//frame window create
		if (CFrameWnd::OnCreate(lpCreateStruct)==-1)
			return -1;

		//set our accelorator
		LoadAccelTable(MAKEINTRESOURCE(IDR_INDEXER));

		//create controls
		mcField.Create("<field>", WS_CHILD|WS_VISIBLE, CRect(0,0,0,0), this, 0);
		mcStatus.Create("<status>", WS_CHILD|WS_VISIBLE, CRect(0,0,0,0), this, 0);
		mcChoice.Create("<choice>", WS_CHILD|WS_VISIBLE, CRect(0,0,0,0), this, 0);
		mcThumb.Create(NULL, WS_CHILD|WS_VISIBLE|SS_BITMAP|SS_CENTERIMAGE, CRect(0,0,0,0), this, 0);
		mcBack.Create("Back", WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON, CRect(0,0,0,0), this, IDC_INDEXER_BACK);
		mcFields.Create(WS_CHILD|WS_VISIBLE|LBS_NOINTEGRALHEIGHT|LBS_HASSTRINGS|LBS_OWNERDRAWFIXED, CRect(0,0,0,0), this, IDC_INDEXER_FIELDS);
		mcImage.Create(NULL, NULL, WS_CHILD|WS_VISIBLE, CRect(0,0,0,0), this, IDC_INDEXER_IMAGE);
	
		//create toolbar
		TBBUTTON tb;
		tb.dwData = 0;
		tb.fsState = TBSTATE_ENABLED;
		tb.fsStyle = TBSTYLE_BUTTON|TBSTYLE_AUTOSIZE;
		tb.iBitmap = -1;
		mcToolbar.Create(WS_CHILD|WS_VISIBLE|CCS_NOPARENTALIGN|TBSTYLE_TOOLTIPS|
				CCS_NORESIZE|CCS_NODIVIDER|TBSTYLE_LIST, CRect(0,0,0,0), this, 0);
		mcToolbar.AddStrings("Wait\0Next\0Change\0Rewind\0Jump\0Save\0");
	
		tb.iString = 0;
		tb.idCommand = ID_FIELDS_WAIT;
		mcToolbar.InsertButton(0, &tb);
	
		tb.iString = 1;
		tb.idCommand = ID_FIELDS_NEXT;
		mcToolbar.InsertButton(1, &tb);

		tb.iString = 2;
		tb.idCommand = ID_FIELDS_CHANGE;
		mcToolbar.InsertButton(2, &tb);

		tb.iString = 3;
		tb.idCommand = ID_FIELDS_REWIND;
		mcToolbar.InsertButton(3, &tb);

		tb.iString = 4;
		tb.idCommand = ID_FIELDS_JUMP;
		mcToolbar.InsertButton(4, &tb);

		tb.iString = 5;
		tb.idCommand = ID_FIELDS_SAVE;
		mcToolbar.InsertButton(5, &tb);

		//set fonts
		CFont font;
		font.CreateStockObject(ANSI_VAR_FONT);
		mcField.SetFont(&font, FALSE);
		mcFields.SetFont(&font, FALSE);
		mcStatus.SetFont(&font, FALSE);
		mcChoice.SetFont(&font, FALSE);
		mcBack.SetFont(&font, FALSE);

		//update our display
		mcImage.SetFit(true, false);
		ShowWindow(SW_MAXIMIZE);
		UpdateWindow();

		//get the latest indexes
		if (!QueryDownloadIndexes(this))
		{
			return -1;
		}
		
		//fields and stuff
		mdCount = 0;
		mdCurrentField = 0;
		mdLastFile = -1;
		mdCurrentFile = -1;

		//get a new field
		OnChangeField();

		//show the first record
		Navigate(0);

		//success
		return 0;
	}

	//windowing messages

	#define W_FIELD		350
	#define H_LABEL		24
	#define H_BUTTON	48
	#define W_CHOICE	200
	#define W_BACK		64
	#define W_BORDER	8

	afx_msg void OnSize(UINT nType, int cx, int cy)
	{
		int x1 = cx-W_FIELD-W_BORDER*2;
		int x2 = cx-x1-W_BORDER;
		int y1 = W_BORDER;
		int y2;

		HDWP h = ::BeginDeferWindowPos(8);
		
		//left column
		h = ::DeferWindowPos(h, mcImage.m_hWnd, NULL, W_BORDER, y1, x1-W_BORDER*2, cy-W_BORDER*2, SWP_NOZORDER);
		
		//right column
		h = ::DeferWindowPos(h, mcField.m_hWnd, NULL, x1, y1, x2, H_LABEL, SWP_NOZORDER);
		y1 += H_LABEL;
		y2 = cy - y1 - W_BORDER*3 - H_LABEL - H_BUTTON - XMGUI_THUMBHEIGHT;
		h = ::DeferWindowPos(h, mcFields.m_hWnd, NULL, x1, y1, x2, y2, SWP_NOZORDER);
		y1 += y2 + W_BORDER;
		h = ::DeferWindowPos(h, mcStatus.m_hWnd, NULL, x1, y1, x2, H_LABEL, SWP_NOZORDER);
		y1 += H_LABEL;
		h = ::DeferWindowPos(h, mcToolbar.m_hWnd, NULL, x1, y1, x2, H_BUTTON, SWP_NOZORDER);

		//bottom strip, right column
		y1 += H_BUTTON+W_BORDER;
		h = ::DeferWindowPos(h, mcThumb.m_hWnd, NULL, x1, y1, XMGUI_THUMBWIDTH, XMGUI_THUMBHEIGHT, SWP_NOZORDER);
		x1 += XMGUI_THUMBWIDTH + W_BORDER;
		x2 -= (XMGUI_THUMBWIDTH+W_BORDER);
		h = ::DeferWindowPos(h, mcChoice.m_hWnd, NULL, x1, y1, x2, H_LABEL, SWP_NOZORDER);
		y1 += H_LABEL;
		h = ::DeferWindowPos(h, mcBack.m_hWnd, NULL, x1, y1, x2, H_BUTTON, SWP_NOZORDER);
		
		::EndDeferWindowPos(h);
	}

	void OnDestroy()
	{
		//save
		OnSave();
	}

	//field commands
	void Navigate(DWORD newIndex)
	{
		//save the data
		db()->Lock();
		CXMDBFile *file = NULL;
		if (mdCurrentFile!=-1)
		{
			//save index
			file = db()->GetFile(mdCurrentFile);

			//compute field value
			BYTE bval;
			DWORD dwval = 0;
			for (int i=0;i<mcFields.GetCount();i++)
				if (mcFields.GetCheck(i))
					dwval |= (1<<i);
			bval = (BYTE)dwval;

			//which field?
			switch (mdCurrentField)
			{
			case 0: //setting
				file->GetIndex()->data.Setting = bval;
				break;
			case 1: //rating
				file->GetIndex()->data.Rating = bval;
				break;
			case 2: //quantity
				file->GetIndex()->data.Quantity = bval;
				break;
			case 3: //contents
				file->GetIndex()->data.Content = bval;
				break;
			case 4: //build
				file->GetIndex()->data.Build = bval;
				break;
			case 5: //haircolor
				file->GetIndex()->data.HairColor = bval;
				break;
			case 6: //hairstyle
				file->GetIndex()->data.HairStyle = bval;
				break;
			case 7: //eyes
				file->GetIndex()->data.Eyes = bval;
				break;
			case 8: //height
				file->GetIndex()->data.Height = bval;
				break;
			case 9: //age
				file->GetIndex()->data.Age = bval;
				break;
			case 10: //breasts
				file->GetIndex()->data.Breasts = bval;
				break;
			case 11: //nipples
				file->GetIndex()->data.Nipples = bval;
				break;
			case 12: //butt
				file->GetIndex()->data.Butt = bval;
				break;
			case 13: //catagory
				file->GetIndex()->data.Cat1 = dwval;
				break;
			case 14: //race
				file->GetIndex()->data.Race = bval;
				break; 
			case 15: //quality
				file->GetIndex()->data.Quality = bval;
				break;
			case 16: //skin
				file->GetIndex()->data.Skin = bval;
				break;
			case 17: //hips
				file->GetIndex()->data.Hips = bval;
				break;
			case 18: //legs
				file->GetIndex()->data.Legs = bval;
				break;
			case 19: //female gen
				file->GetIndex()->data.FemaleGen = bval;
				break;
			case 20: //male gen
				file->GetIndex()->data.MaleGen = bval;
				break;
			case 21: //chest
				file->GetIndex()->data.Chest = bval;
				break;
			case 22: //facial hair
				file->GetIndex()->data.FacialHair = bval;
				break;
			case 23: //catgory 2
				file->GetIndex()->data.Cat2 = dwval;
				break;
			}

			//mark index as dirty
			file->GetIndex()->flags |= DIF_DIRTY;

			//show thumbnail
			CXMDBThumb *thumb = file->GetThumb(XMGUI_THUMBWIDTH, XMGUI_THUMBHEIGHT);
			if (thumb)
			{
				//display
				CJPEGDecoder jpeg;
				CDIBSection dib;
				BYTE* buf;
				DWORD bufcount;
				bufcount = thumb->GetImage(&buf);
				if (bufcount>0)
				{
					try
					{
						jpeg.MakeBmpFromMemory(buf, bufcount, &dib, 32);
						mcThumb.SetBitmap((HBITMAP)dib.GetHandle());
					}
					catch (...)
					{
						AfxMessageBox("Error displaying thumbnail.");
					}
				}
				thumb->FreeImage(&buf);

				//show text
				CString choice, str;
				for (int i=0;i<mcFields.GetCount();i++)
				{
					if (mcFields.GetCheck(i)==1)
					{
						mcFields.GetText(i, str);
						choice += str + " ";
					}
				}
				mcChoice.SetWindowText(choice);

				//set the old index
				mdLastFile = mdCurrentFile;
			}
		}

		//get the next file
		mdCurrentFile = newIndex;
		file = db()->GetFile(newIndex);
		if (file)
		{	
			//has this file been removed?
			if (file->GetFlag(DFF_REMOVED))
			{
				//file does not exist in the file system
				Navigate(mdCurrentFile+1);
			}
			else
			{
				//get the value for the field
				DWORD dwval;
				BYTE bval;
				switch (mdCurrentField)
				{
				case 0: //setting
					bval = file->GetIndex()->data.Setting;
					break;
				case 1: //rating
					bval = file->GetIndex()->data.Rating;
					break;
				case 2: //quantity
					bval = file->GetIndex()->data.Quantity;
					break;
				case 3: //contents
					bval = file->GetIndex()->data.Content;
					break;
				case 4: //build
					bval = file->GetIndex()->data.Build;
					break;
				case 5: //haircolor
					bval = file->GetIndex()->data.HairColor;
					break;
				case 6: //hairstyle
					bval = file->GetIndex()->data.HairStyle;
					break;
				case 7: //eyes
					bval = file->GetIndex()->data.Eyes;
					break;
				case 8: //height
					bval = file->GetIndex()->data.Height;
					break;
				case 9: //age
					bval = file->GetIndex()->data.Age;
					break;
				case 10: //breasts
					bval = file->GetIndex()->data.Breasts;
					break;
				case 11: //nipples
					bval = file->GetIndex()->data.Nipples;
					break;
				case 12: //butt
					bval = file->GetIndex()->data.Butt;
					break;
				case 13: //catagory
					dwval = file->GetIndex()->data.Cat1;
					break;
				case 14: //race
					bval = file->GetIndex()->data.Race;
					break; 
				case 15: //quality
					bval = file->GetIndex()->data.Quality;
					break;
				case 16: //skin
					bval = file->GetIndex()->data.Skin;
					break;
				case 17: //hips
					bval = file->GetIndex()->data.Hips;
					break;
				case 18: //legs
					bval = file->GetIndex()->data.Legs;
					break;
				case 19: //female gen
					bval = file->GetIndex()->data.FemaleGen;
					break;
				case 20: //male gen
					bval = file->GetIndex()->data.MaleGen;
					break;
				case 21: //chest
					bval = file->GetIndex()->data.Chest;
					break;
				case 22: //facial hair
					bval = file->GetIndex()->data.FacialHair;
					break;
				case 23: //catgory 2
					dwval = file->GetIndex()->data.Cat2;
					break;
				}

				//set the check marks
				if (mdCurrentField==13 ||
					mdCurrentField==23)
				{
					//catagory
					for (int i=0;i<32;i++)
						mcFields.SetCheck(i, (dwval&(1<<i))?1:0);
				}
				else
				{
					//non-catagory
					for (int i=0;i<8;i++)
						mcFields.SetCheck(i, (bval&(1<<i))?1:0);
				}

				//decode the picture
				CJPEGDecoder jpeg;
				CDIBSection dib;
				CBitmap* pbmp;
				try
				{
					jpeg.MakeBmpFromFile(file->GetPath(), &dib, 32);
					pbmp = new CBitmap();
					pbmp->Attach(dib.GetHandle());
					dib.Detach();
					mcImage.ShowBitmap(pbmp);
				}
				catch (...)
				{
					AfxMessageBox("Error displaying new picture.");
				}
			} //DFF_REMOVED
		}
		db()->Unlock();

		//incrment the count
		mdCount++;
		if (mdCount>3)
		{
			//OnSave();
		}

		//update the status
		CString str;
		str.Format("[%d]: Idle.", mdCurrentFile);
		mcStatus.SetWindowText(str);

		//reset timer
		KillTimer(4545);
		mdSetTimer = true;
	}
	afx_msg void OnTimer(UINT nIDEvent)
	{
		if (nIDEvent==4545)
		{
			KillTimer(4545);
			//OnNext();
		}
	}
	void OnBack()
	{
		//navigate backward
		if (mdLastFile!=-1)
			Navigate(mdLastFile);
		else
			AfxMessageBox("Nowhere to go!");
	}
	void OnWait()
	{
		//reset timer
		KillTimer(4545);
		mdSetTimer = false;
	
		CString str;
		str.Format("[%d]: Waiting.", mdCurrentFile);
		mcStatus.SetWindowText(str);
	}
	void OnNext()
	{
		//move to the next file
		if ((mdCurrentFile+1)==db()->GetFileCount())
			MessageBox("End of the road.");
		else
			Navigate(mdCurrentFile+1);
	}
	void OnChangeField()
	{
		//request new field
		CIndexer_ChooseField dlg(this);
		if (dlg.DoModal()==IDOK)
		{
			//set the new field
			mdCurrentField = dlg.x;

			//clear listbox
			while(mcFields.GetCount()>0)
				mcFields.DeleteString(0);

			//re-populate the list
			char sz[MAX_PATH];
			for(int i=0;i<mFields[mdCurrentField].count;i++)
			{
				sprintf(sz, "[%d] %s", i+1, mFields[mdCurrentField].value[i]);
				mcFields.AddString(sz);
			}

			//print name in field label
			mcField.SetWindowText(mFields[mdCurrentField].name);
		}
	}
	void OnRewind()
	{
		//move to the first one
		Navigate(0);
	}
	void OnJump()
	{
		//ask user
		CIndexer_JumpTo dlg(this);
		dlg.mJumpTo = 0;
		if (dlg.DoModal()==IDOK)
		{
			Navigate(dlg.mJumpTo);
		}
	}
	void OnSave()
	{
		//build the listing first
		if (!sm()->SendListing(false))
		{
			AfxMessageBox("Error saving!");
			return;
		}
		TRACE("SAVED\n");

		//reset counter
		mdCount = 0;
	}

	//selecting a value
	void OnCheck(int val)
	{
		//make sure its checked
		if(mcFields.GetCheck(val-1)==0)
			mcFields.SetCheck(val-1, 1);
		else
			mcFields.SetCheck(val-1, 0);

		//start the timer
		if (mdSetTimer)
		{
			//reset timer
			KillTimer(4545);
			SetTimer(4545, 1500, NULL);

			CString str;
			str.Format("[%d]: Timer active.", mdCurrentFile);
			mcStatus.SetWindowText(str);
		}
	}
	void OnField1() { OnCheck(1); }
	void OnField2() { OnCheck(2); }
	void OnField3() { OnCheck(3); }
	void OnField4() { OnCheck(4); }
	void OnField5() { OnCheck(5); }
	void OnField6() { OnCheck(6); }
	void OnField7() { OnCheck(7); }
	void OnField8() { OnCheck(8); }
};

BEGIN_MESSAGE_MAP(CIndexer, CFrameWnd)
	
	//windowing
	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_WM_TIMER()
	ON_WM_DESTROY()

	//command button
	ON_BN_CLICKED(IDC_INDEXER_BACK, OnBack)
	ON_COMMAND(ID_FIELDS_WAIT, OnWait)
	ON_COMMAND(ID_FIELDS_NEXT, OnNext)
	ON_COMMAND(ID_FIELDS_CHANGE, OnChangeField)
	ON_COMMAND(ID_FIELDS_REWIND, OnRewind)
	ON_COMMAND(ID_FIELDS_JUMP, OnJump)
	ON_COMMAND(ID_FIELDS_SAVE, OnSave)

	//value selections
	ON_COMMAND(ID_FIELDS_1, OnField1)
	ON_COMMAND(ID_FIELDS_2, OnField2)
	ON_COMMAND(ID_FIELDS_3, OnField3)
	ON_COMMAND(ID_FIELDS_4, OnField4)
	ON_COMMAND(ID_FIELDS_5, OnField5)
	ON_COMMAND(ID_FIELDS_6, OnField6)
	ON_COMMAND(ID_FIELDS_7, OnField7)
	ON_COMMAND(ID_FIELDS_8, OnField8)

END_MESSAGE_MAP()

//generated file contains populates the fields array
#include "buildfields.txt"

BOOL CIndexer_ChooseField::OnInitDialog()
{
	CListBox lb;
	lb.Attach(GetDlgItem(IDC_LIST1)->m_hWnd);
	for(int i=0;i<pi->mFieldCount;i++)
	{
		lb.AddString(pi->mFields[i].name);
	}
	lb.Detach();
	return FALSE;
}

#endif

// --------------------------------------------------------------------------------- GLOBALS

bool QueryDownloadIndexes(CWnd *parent)
{
	CUpdateIndexes dlg(parent);
	return (dlg.DoModal()==IDOK);
}

void QueryFastIndexer()
{
	#ifdef _INTERNAL
	CIndexer *pi = new CIndexer();
	#endif
}

bool QueryBuildSimple(CXMQuery *query)
{
	//show simple dialog
	CQueryBuilder dlg;
	query->mQuery.CopyTo(&dlg.mSearch);
	query->mRejection.CopyTo(&dlg.mFilter);
	dlg.mMinWidth	= query->mMinWidth;
	dlg.mMaxWidth	= query->mMaxWidth;
	dlg.mMinHeight	= query->mMinHeight;
	dlg.mMaxHeight	= query->mMaxHeight;
	dlg.mMinSize	= query->mMinSize;
	dlg.mMaxSize	= query->mMaxSize;
	if (dlg.DoModal()==IDOK)
	{
		//copy data back
		dlg.mSearch.CopyTo(&query->mQuery);
		dlg.mFilter.CopyTo(&query->mRejection);
		query->mMinWidth	= dlg.mMinWidth;
		query->mMaxWidth	= dlg.mMaxWidth;
		query->mMinHeight	= dlg.mMinHeight;
		query->mMaxHeight	= dlg.mMaxHeight;
		query->mMinSize	= dlg.mMinSize;
		query->mMaxSize	= dlg.mMaxSize;
		return true;
	}
	return false;
}


bool QueryBuildIndex(CXMDBFile *file)
{
	//show the dialog
	CIndexBuilder dlg(XMGUI_INDEXMODE_INDEX, &file->GetIndex()->data, "Modify Index");
	if (dlg.DoModalPreview(file)==IDOK)
	{
		//mark the index as modified, and send the update
		file->GetIndex()->flags |= DIF_DIRTY;
		if (!sm()->SendListing(false))
		{
			AfxMessageBox("Failed to send index to server.", MB_ICONERROR);
			return false;
		}
		return true;
	}
	return false;
}

