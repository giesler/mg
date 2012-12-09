// MainFrm.h : interface of the CMainFrame class
//
#pragma once

//GUI MESSAGES
#define XM_GUIBASE		WM_USER+2048

//XM_SPLITMOVE: Splitter bar has moved
#define XM_SPLITMOVE	XM_GUIBASE+0
	//wparam: control id of splitter
	//lparam: LOWORD=left boundry, HIWORD=right boundry
	#define XM_SPLITLEFT(lp)	(long)LOWORD(lp)
	#define XM_SPLITRIGHT(lp)	(long)HIWORD(lp)

//layout parameters
#define XMGUI_LOGOSIZE			80
#define XMGUI_TOPOFADD			3
#define XMGUI_ADVERTW			468
#define XMGUI_TOPBORDER			(XMGUI_LOGOSIZE+(XMGUI_TOPOFADD*2))	//66
#define XMGUI_SPLITWIDTH		4
#define XMGUI_BORDERRIGHT		1
#define XMGUI_NETSIZE			140
#define XMGUI_NETSIZEF			30
#define XMGUI_BOTTOMBORDER		(XMGUI_TOPOFADD*2)	//134
#define XMGUI_VIEWSPLITBORDER	64
#define XMGUI_FOLDWIDTH			24
#define XMGUI_FOLDBORDER		(XMGUI_FOLDWIDTH+XMGUI_SPLITWIDTH)

//thumbnail size
//#define XMGUI_THUMBWIDTH		124	//4:3
//#define XMGUI_THUMBHEIGHT		93

// ---------------------------------------------------------------------------- Query Builders
// xmguiquery.cpp

#define XMGUI_INDEXMODE_SEARCH	0
#define XMGUI_INDEXMODE_FILTER	1
#define XMGUI_INDEXMODE_INDEX	2

bool QueryBuildSimple(CXMQuery* query);
bool QueryBuildIndex(CXMDBFile* file);
void QueryFastIndexer();
bool QueryDownloadIndexes(CWnd *parent);

// ---------------------------------------------------------------------------------- Web Browser
// xmgui.cpp

class CAdvert : public CWnd
{
public:

	//create
	//CAdvert();
	//~CAdvert();
	BOOL Create(
		LPCTSTR lpszClassName, LPCTSTR lpszWindowName,
		DWORD dwStyle, const RECT &rect,
		CWnd *pParentWnd, UINT NID, CCreateContext *pContext = NULL);

	//navigate
	void Navigate(char* location);
	void Refresh();
};

// ------------------------------------------------------------------------------------ Tray Icon
// xmgui.cpp

class CTrayIcon : public CCmdTarget
{
protected:  
	DECLARE_DYNAMIC(CTrayIcon)
	NOTIFYICONDATA m_nid;
	
	// struct for Shell_NotifyIcon args
public:   
	CTrayIcon(UINT uID);
	~CTrayIcon();
	
	// Call this to receive tray notifications
	void SetNotificationWnd(CWnd* pNotifyWnd, UINT uCbMsg);

	// SetIcon functions. To remove icon, call SetIcon(0) 
	BOOL SetIcon(UINT uID);
	BOOL SetIcon(HICON hicon, LPCSTR lpTip);   
	BOOL SetIcon(LPCTSTR lpResName, LPCSTR lpTip)
	{
		return SetIcon(lpResName ?          
			AfxGetApp()->LoadIcon(lpResName) : NULL, lpTip);
	}
	BOOL SetStandardIcon(LPCTSTR lpszIconName, LPCSTR lpTip)
	{
		return SetIcon(::LoadIcon(NULL, lpszIconName), lpTip);
	} 

	virtual LRESULT OnTrayNotification(WPARAM uID, LPARAM lEvent);
};

// ---------------------------------------------------------------------------- Shared Files
// xmguishared.cpp

//everything else is in xmguishared.cpp
void DoSharedFiles(CWnd *parent);

// ---------------------------------------------------------------------------- Image View
// xmguibrowsing.cpp

class CImageViewer : public CWnd
{
public:

	//construction
	CImageViewer();
	~CImageViewer();

	//misc
	void ShowFile(char *pfile);
	void ShowBitmap(CBitmap *pbmp);

private:

	//state data
	char file[MAX_PATH];		//currently displayed file
	CBitmap *bmp;				//displayed bitmap
	int zoom;					//current zoom level
	CRect viewport;				//rect of the sourcebitmap displayed on the screen
	CPoint origin;				//pixel in the source bitmap that will be displayed
								//in the center of the viewer
	int borderLeft, borderTop;	//black borders at left, top

	//display
	void RecalcViewport();
	void UpdateScrollBars();
	afx_msg void OnPaint();

	//windowing
	BOOL PreCreateWindow(CREATESTRUCT &cs);
	afx_msg int OnCreate(LPCREATESTRUCT lpcs);
	afx_msg void OnSize(UINT nType, int cx, int cy);

	//panning
	DECLARE_MESSAGE_MAP();
	bool mIsTracking;
	CPoint mLastTrackPoint;
	afx_msg void OnLButtonDown(UINT nFlags, CPoint pt);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint pt);
	afx_msg void OnRButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnCancelMode();
	afx_msg void OnMouseMove(UINT nFlags, CPoint pt);
	afx_msg BOOL OnSetCursor(CWnd *pWnd, UINT nHitTest, UINT message);
	
	//scrolling
	afx_msg void OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
	afx_msg void OnVScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);

	//zooming
	afx_msg void OnContextMenu(CWnd* pWnd, CPoint pos);
	afx_msg BOOL OnMouseWheel(UINT nFlags, short zDelta, CPoint pt);
	void Zoom(bool in);
	void OnZoomIn();
	void OnZoomOut();
	void OnFullScreen();
};

// --------------------------------------------------------------------------- File Browser
// xmguibrowsing.cpp

class CFileBrowser : public CListCtrl
{	
public:

	//construction
	CFileBrowser();
	~CFileBrowser();

	//populate the list
	void BrowseFolder(char* path);
	void BrowseFolder(LPITEMIDLIST pidl);
	void UpdateBrowser();

	//get info about the selected item
	const char* GetSelectedPath();
	CBitmap* GetSelectedBitmap();
	CBitmap* GetSelectedThumbnail();

private:

	//misc
	CString mPath;
	CImageList mFilesImages;
	void ClearFileList();

	//file system monitoring
	HANDLE mWaitHandle;
	afx_msg void OnTimer(UINT nID);
	afx_msg void OnDestroy();

	//messages
	DECLARE_MESSAGE_MAP();
	BOOL PreCreateWindow(CREATESTRUCT &cs);
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnFilesDeleteItem(NMHDR* pnmh, LRESULT* pResult);
	afx_msg void OnFilesGetDispInfo(NMHDR* pnmh, LRESULT* pResult);
	afx_msg void OnFilesClick(NMHDR* pnmh, LRESULT* pResult);
	afx_msg LRESULT OnAsyncResize(WPARAM wParam, LPARAM lParam);
	afx_msg void OnContextMenu(CWnd* pWnd, CPoint pos);

	//context menu commands
	afx_msg void OnIndexFile();
	afx_msg void OnShareFile();
	afx_msg void OnUnshareFile();
};

// ------------------------------------------------------------------------------- Splitter
// xmgui.cpp

//our splitter window
class CVerticalSplitter : public CWnd
{
public:

	//contruction
	CVerticalSplitter();
	~CVerticalSplitter();
	virtual bool Create(CWnd* parent, UINT id, CRect &pos, long borderLeft, long borderRight, COLORREF bgcolor);
	HDWP DeferWindowPosAfterSplit(HDWP hWinPosInfo);
	void SetBorders(long borderLeft, long borderRight);

protected:

	//creation
	BOOL OnCreate(LPCREATESTRUCT lpCreateStruct);

	//graphics
	HCURSOR mCursor;
	COLORREF mBackgroundColor;
	void OnPaint();
	void DrawInvertRect(CRect &r);

	//mouse tracking
	void OnLButtonDown(UINT nFlags, CPoint pt);
	void OnLButtonUp(UINT nFlags, CPoint pt);
	void OnCancelMode();
	void OnMouseMove(UINT nFlags, CPoint pt);
	BOOL OnSetCursor(CWnd *pWnd, UINT nHitTest, UINT message);

	//internal tracking
	bool mIsTracking;
	CRect mCurRect;
	long mStartX;
	long mBorderLeft, mBorderRight;

	DECLARE_MESSAGE_MAP()
};

// ----------------------------------------------------------------------- FoldButton
// xmgui.cpp

class CFoldButton : public CToolBarCtrl
{
public:

	//construction
	CFoldButton();
	~CFoldButton();
	bool Create(bool left, CWnd *parent, UINT idc, char* field);
	bool IsFolded(); 

private:
	DECLARE_MESSAGE_MAP();
	CImageList mImage;
	afx_msg void OnDestroy();
	char* mField;
};


// ----------------------------------------------------------------------- IXMGUIView

#define XMGUIVIEW_SEARCH	0x00
#define XMGUIVIEW_COMPLETED 0x01
#define XMGUIVIEW_SAVED		0x02
#define XMGUIVIEW_LOCAL		0x04

class IXMGUIView : public CWnd
{
public:

	//lifetime management
	virtual void AddRef()=0;
	virtual void Release()=0;
	virtual UINT GetViewType()=0;
	virtual bool Create(CWnd *hwParent, CRect &rect)=0;
};

// --------------------------------------------------------------------- Search View
// xmguisearch.cpp

//track the state of each item in the gui
#define XMGUISIS_THUMBQUEUED		0
#define XMGUISIS_THUMBDOWNLOADING	1
#define XMGUISIS_THUMBCOMPLETE		2
#define XMGUISIS_FILEQUEUED			3
#define XMGUISIS_FILEDOWNLOADING	4
#define XMGUISIS_FILECOMPLETE		5
#define XMGUISIS_ERROR				6
#define XMGUISIS_WAITING			7

struct CXMGUIQueryItem
{
	CXMGUIQueryItem();
	~CXMGUIQueryItem();
	CMD5 mThumbMD5;
	int mState;
	int mImage;
	int mItem;
	CXMQueryResponseItem *mQueryResponseItem;
};
typedef CTypedPtrList<CPtrList, CXMGUIQueryItem*> CXMGUIQueryItemList;

class CSearchView : public IXMGUIView
{
public:
	CSearchView();
	
	//IMovableControlGroup
	void AddRef();
	void Release();
	UINT GetViewType();
	bool Create(CWnd *hwParent, CRect &rect);

	//static query data
	static CXMQuery *mQuery;
	static CImageList mQueryImages;
	static CXMGUIQueryItemList mQueryItems;
	static CXMGUIQueryItem* GetQueryItemFromTag(CXMPipelineUpdateTag *tag);

protected:

	//misc
	void ShowResults();

	//messages
	DECLARE_MESSAGE_MAP();
	afx_msg void OnDestroy();
	afx_msg void OnPaint();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg LRESULT OnClientMessage(WPARAM wParam, LPARAM lParam);
	afx_msg LRESULT OnServerMessage(WPARAM wParam, LPARAM lParam);
	afx_msg void OnThumbsDeleteItem(NMHDR* pnmh, LRESULT* pResult);
	afx_msg void OnThumbsDoubleClick(NMHDR *pnmh, LRESULT* pResult);
	afx_msg void OnContextMenu(CWnd* pWnd, CPoint pos);
	afx_msg void OnDownload();
	afx_msg void OnGetInfoTip(NMHDR *pnmh, LRESULT* pResult);

	//controls
	CImageList mImages;
	CToolBarCtrl mCurrentTools;		//IDC_SEARCH_TOOLS
	CToolBarCtrl mSavedTools;		//IDC_SEARCH_SAVEDTOOLS
	CButton mSavedFrame;			//IDC_SEARCH_SAVEDFRAME
	CComboBox mSavedList;			//IDC_SEARCH_SAVEDLIST
	CListCtrl mThumbs;				//IDC_SEARCH_THUMBS

	//toolbar buttons
	afx_msg void OnSearchSearch();
	afx_msg void OnSearchEdit();
	afx_msg void OnSearchSave();
	afx_msg void OnSavedSearch();
	afx_msg void OnSavedEdit();
	afx_msg void OnSavedDelete();
	void RefreshSaved();
	CXMQuery* GetSavedQuery();

	~CSearchView();
	UINT m_udRefCount;
};

// ------------------------------------------------------------------- Local Browser View
// xmguilocal.cpp

class CLocalView : public IXMGUIView
{
public:
	CLocalView();
	
	//IMovableControlGroup
	void AddRef();
	void Release();
	UINT GetViewType();
	bool Create(CWnd *hwParent, CRect &rect);

	//external control
	void BrowseFolder(char* path, bool save = true);
	void BrowseFolder(LPITEMIDLIST pidl, bool save = true);
	static char mCurPath[MAX_PATH];

protected:

	//messages
	DECLARE_MESSAGE_MAP();
	afx_msg void OnDestroy();
	afx_msg void OnPaint();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg LRESULT OnSplitterMove(WPARAM wParam, LPARAM lParam);
	afx_msg void OnFilesClick(NMHDR* pnmh, LRESULT* pResult);

	//controls
	CFileBrowser mFiles;
	CVerticalSplitter mSplitter;
	CImageViewer mPreview;
	CEdit mPathLabel;

	~CLocalView();
	UINT m_udRefCount;
	UINT m_nLastSplitterPos;
};

// ------------------------------------------------------------------- Smart Image List
// xmguicompleted.cpp

class CSmartImageList
{
public:

	CSmartImageList();
	~CSmartImageList();
	int SILInsert(CDIBSection *dib);
	int SILInsert(CBitmap *bmp);
	void SILDelete(int n);
	const CImageList* SILGetImageList();

private:

	//mask stuff
	BYTE *mMask;
	int mMaskCount;
	int mMaskSize;

	//our internal imagelist
	CImageList mImages;
};

// ------------------------------------------------------------------- Completed File View
// xmguicompleted.cpp

class CCompletedView : public IXMGUIView
{
public:
	CCompletedView();
	
	//IXMGUIView
	void AddRef();
	void Release();
	UINT GetViewType();
	bool Create(CWnd *hwParent, CRect &rect);

protected:

	//thumbnail stuff
	typedef CXMClientManager::CompletedFile* tag;
	bool InsertItem(tag t);
	afx_msg void OnThumbsDeleteItem(NMHDR *pnmh, LRESULT* pResult);
	afx_msg void OnThumbsClick(NMHDR* pnmh, LRESULT* pResult);

	//buttons
	afx_msg void OnSave();
	afx_msg void OnDelete();

	//misc
	UINT mLastSplitPos;

	//controls
	CImageViewer mPreview;
	CListCtrl mThumbs;
	CSmartImageList mThumbsImages;
	CVerticalSplitter mSplitter;
	CButton mSave, mDelete;

	//messages
	DECLARE_MESSAGE_MAP();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg LRESULT OnClientMessage(WPARAM wParam, LPARAM lParam);
	afx_msg void OnDestroy();
	afx_msg void OnPaint();
	afx_msg LRESULT OnSplitterMove(WPARAM wParam, LPARAM lParam);

	//internal
	~CCompletedView();
	UINT m_udRefCount;
};

// ------------------------------------------------------------------- Progress Ribbon
// xmguistatus.cpp

//progress bar w/ overlaid text
class CXMGUIProgressRibbon : public CWnd
{
public:

	//construction
	CXMGUIProgressRibbon();
	~CXMGUIProgressRibbon();

	//display data
	bool mBorder;
	BYTE mValue, mMaxValue;
	COLORREF mBarColor, mBackColor, mTextColor;
	char mText[MAX_PATH];

private:

	//windowing
	DECLARE_MESSAGE_MAP();
	afx_msg void OnPaint();
};

// ------------------------------------------------------------------- Status Light
// xmguistatus.cpp

class CXMGUIStatusLight : public CWnd
{
public:

	//construction
	CXMGUIStatusLight();
	~CXMGUIStatusLight();

	//display data
	bool mBorder;
	COLORREF mDimColor, mBrightColor, mBackColor;
	bool mLit;

private:

	//windowing
	DECLARE_MESSAGE_MAP();
	afx_msg void OnPaint();
};

// ----------------------------------------------------------------------- Status Pane
// xmguistatus.cpp

class CXMGUIStatus : public CWnd
{
public:

	//construction
	CXMGUIStatus();
	~CXMGUIStatus();

	inline bool IsFolded()
	{
		return mShrink.IsFolded();
	}

private:
	
	//tabs
	typedef enum
	{
		XMGUI_UL,
		XMGUI_DL
	} STATE;
	STATE mTabState;
	afx_msg void OnTabsSelChange(NMHDR* pnmh, LRESULT* pResult);
	afx_msg void OnThumbsDeleteItem(NMHDR *pnmh, LRESULT* pResult);

	//uploads
	typedef CXMClientManager::UploadSlot* ULTag;
	void ULRefresh();
	void ULInsert(ULTag tag);
	void ULRemove(ULTag tag);
	void ULClear();

	//downloads
	typedef CMD5* DLTag;
	typedef CXMClientManager::QueuedFile* DLTagQ;
	typedef CXMClientManager::DownloadSlot* DLTagD;
	void DLRefresh();
	void DLInsert(DLTagQ tag);
	void DLInsert(DLTagD tag);
	void DLInsert(DLTag tag, const char* text);
	void DLText(DLTag tag, const char* text);
	void DLRemove(DLTag tag);
	void DLClear();
	void DLRefreshProgress();

	//pipeline
	afx_msg LRESULT OnClientMessage(WPARAM wParam, LPARAM lParam);
	afx_msg LRESULT OnServerMessage(WPARAM wParam, LPARAM lParam);
	void UpdateDownloadRibbon();
	void UpdateUploadRibbon();

	//windowing messages
	DECLARE_MESSAGE_MAP();
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnDestroy();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnPaint();
	afx_msg void OnTimer(UINT nIDEvent);
	bool mAllControlsCreated;

	//splitter
	afx_msg void OnShrink();
	afx_msg LRESULT OnSplitMove(WPARAM wParam, LPARAM lParam);
	int mSplitFromRight;

	//thumbnails
	CFoldButton mShrink;
	CVerticalSplitter mSplitter;
	CTabCtrl mTabs;
	CListCtrl mFiles;
	CSmartImageList mThumbsImages;
	CImageList mTabImages;
	
	//progress bars
	CStatic mSearchLabel;
	CStatic mDownloadsLabel;
	CStatic mUploadsLabel;
	CXMGUIProgressRibbon mSearchRibbon;
	CXMGUIProgressRibbon mDownloadsRibbon;
	CXMGUIProgressRibbon mUploadsRibbon;

	//lights
	CStatic mLightServerLabel;
	CStatic mLightTxLabel;
	CStatic mLightRxLabel;
	CXMGUIStatusLight mLightServer;
	CXMGUIStatusLight mLightTx;
	CXMGUIStatusLight mLightRx;
};

// --------------------------------------------------------------------------- Main Frame
// xmgui.cpp

class CMainFrame : public CFrameWnd
{
public:

	//construction
	CMainFrame();
	virtual ~CMainFrame();

	//public for GUIStatus, so it force a resize when
	//it gets folded
	afx_msg void	OnSize(UINT nType, int cx, int cy);

	//event called by search
	void OnSearch(CXMQuery* query);

protected:
	
	//user input
	afx_msg void OnSharedFiles();
	afx_msg void OnConnect();
	afx_msg void OnDisconnect();
	afx_msg void OnAbout();
	afx_msg void OnHelp();
	afx_msg void OnLogoClick();
	afx_msg void OnOptions();
	afx_msg void OnFastIndexer();
	afx_msg void OnShrink();
	afx_msg void OnUpdateConnect(CCmdUI* pCmdUI);
	afx_msg void OnExit();

	//tray
	bool mClosing;
	afx_msg void OnClose();
	afx_msg LRESULT OnTrayNotification(WPARAM wp, LPARAM lp);
	afx_msg void OnTrayShow();
	afx_msg void OnTrayOptions();
	LRESULT OnTaskBarCreated(WPARAM wp, LPARAM lp);

	//updates from pipeline
	bool StaticInit();
	void StaticClose();
	void ClearQueryItems();
	afx_msg LRESULT OnClientMessage(WPARAM wParam, LPARAM lParam);
	afx_msg LRESULT OnServerMessage(WPARAM wParam, LPARAM lParam);

	//controls
	CFoldButton mShrink;			//IDC_SHRINK
	CVerticalSplitter mSplitter;	//IDC_SPLITTER
	CTreeCtrl mFiles;				//IDC_FILES
	CTabCtrl mTabs;					//IDC_TABS
	CStatic mLogo;					//IDC_LOGO
	CXMGUIStatus mStatus;
	CAdvert mAdvert;				//IDC_ADVERT
	CTrayIcon mTray;

	//IDC_NETMON
	//IDC_BANNER

	//window creation, destruction
	bool mCreateDone;
	virtual BOOL	PreCreateWindow(CREATESTRUCT& cs);
	afx_msg int		OnCreate(LPCREATESTRUCT lpCreateStruct);	
	afx_msg void	OnDestroy();

	//sizing
	afx_msg void	OnSizing(UINT nSide, LPRECT lpRect);
	afx_msg LRESULT OnSplitterMove(WPARAM wParam, LPARAM lParam);

	//tabs
	IXMGUIView *mCurrentView;
	CRect mCurrentViewRect;
	afx_msg HBRUSH OnCtlColor(CDC *pDC, CWnd *pWnd, UINT nCtlColor);
	afx_msg void OnTabsSelChange(NMHDR* pnmh, LRESULT* pResult);
	void DisplayView(UINT newView);
	void OnSizePrepare(CRect *tabs, long lSizerRight, int cx, int cy);
	void OnSizeFinish(CRect &rTabs);
	void UpdateCompletedDownloadsTab();

	//file tree
	bool mIgnoreFileClicks;
	IMalloc *mMalloc;
	HIMAGELIST mSysImageList;
	bool InitializeFileTree();
	bool PopulateFileBranch(HTREEITEM hti);
	HTREEITEM GarunteeFolderVisible(char* path);
	afx_msg void OnFilesSelChanged(NMHDR *pnmh, LRESULT *pResult);
	afx_msg void OnFilesDeleteItem(NMHDR* pnmh, LRESULT* pResult);
	afx_msg void OnFilesGetDispInfo(NMHDR* pnmh, LRESULT* pResult);
	afx_msg void OnFilesItemExpanding(NMHDR* pnmh, LRESULT* pResult);

	//shell32 utilities
	LPITEMIDLIST GetFullyQualifiedPIDL(LPSHELLFOLDER folder, LPITEMIDLIST item);
	void GetChildName(LPSHELLFOLDER folder, LPITEMIDLIST item, DWORD flags, char* out);
	int GetChildIcon(HTREEITEM hti /*LPSHELLFOLDER folder, LPITEMIDLIST item*/);
	LPITEMIDLIST GetPIDLNext(LPITEMIDLIST pidl);
	UINT GetPIDLSize(LPITEMIDLIST pidl);
	LPITEMIDLIST ClonePIDL(LPITEMIDLIST pidl);

	DECLARE_MESSAGE_MAP()
};

