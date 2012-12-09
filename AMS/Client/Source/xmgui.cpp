// MainFrm.cpp : implementation of the CMainFrame class
#include "stdafx.h"
#include <afxpriv.h>
#include "xmclient.h"
#include "xmnet.h"
#include "xmpipeline.h"
#include "xmgui.h"

const UINT WM_TASKBARCREATED = 
    ::RegisterWindowMessage(_T("TaskbarCreated"));


// --------------------------------------------------------------------- MESSAGE MAP

BEGIN_MESSAGE_MAP(CMainFrame, CFrameWnd)
	
	//creation, destruction
	ON_WM_CREATE()
	ON_WM_CLOSE()
	ON_WM_DESTROY()

	//file tree notifications
	ON_NOTIFY(TVN_DELETEITEM,		IDC_FILES,	OnFilesDeleteItem)
	ON_NOTIFY(TVN_GETDISPINFO,		IDC_FILES,	OnFilesGetDispInfo)
	ON_NOTIFY(TVN_ITEMEXPANDING,	IDC_FILES,	OnFilesItemExpanding)
	ON_NOTIFY(TVN_SELCHANGED,		IDC_FILES,	OnFilesSelChanged)

	//user input
	ON_COMMAND(ID_SHARED, OnSharedFiles)
	ON_COMMAND(ID_ABOUT, OnAbout)
	ON_COMMAND(ID_CONNECT, OnConnect)
	ON_COMMAND(ID_DISCONNECT, OnDisconnect)
	ON_COMMAND(ID_HELP, OnHelp)
	ON_COMMAND(ID_SHRINK, OnShrink)
	ON_CONTROL(STN_CLICKED, IDC_LOGO, OnLogoClick)
	ON_COMMAND(ID_OPTIONS, OnOptions)
	ON_COMMAND(ID_INDEXER, OnFastIndexer)
	ON_COMMAND(ID_EXIT, OnExit)
	ON_COMMAND(ID_CONTEST, OnContest)
	ON_BN_CLICKED(IDC_CONTEST, OnContest)

	//tray input
	ON_MESSAGE(XM_TRAYICON, OnTrayNotification)
	ON_COMMAND(ID_TRAY_SHOW, OnTrayShow)
	ON_COMMAND(ID_TRAY_OPTIONS, OnTrayOptions)
	ON_REGISTERED_MESSAGE(WM_TASKBARCREATED, OnTaskBarCreated)

	//gray out invalid command
	ON_UPDATE_COMMAND_UI(ID_CONNECT, OnUpdateConnect)
	ON_UPDATE_COMMAND_UI(ID_DISCONNECT, OnUpdateConnect)

	//tab notifications
	ON_NOTIFY(TCN_SELCHANGE,		IDC_TABS,	OnTabsSelChange)

	//window sizing
	ON_WM_SIZE()
	ON_WM_SIZING()
	ON_MESSAGE(XM_SPLITMOVE, OnSplitterMove)

	//pipeline updates
	ON_MESSAGE(XM_CLIENTMSG, OnClientMessage)
	ON_MESSAGE(XM_SERVERMSG, OnServerMessage)

END_MESSAGE_MAP()

// -------------------------------------------------------------------------- Sizing

/*
#define XMGUI_LOGOSIZE		60
#define XMGUI_TOPOFADD		3
#define XMGUI_TOPBORDER		(XMGUI_LOGOSIZE+(XMGUI_TOPOFADD*2))	//66
#define XMGUI_SPLITWIDTH	4
#define XMGUI_BORDERRIGHT	1
#define XMGUI_NETSIZE		128
#define XMGUI_BOTTOMBORDER	(XMGUI_NETSIZE+(XMGUI_TOPOFADD*2))	//134
*/

#define XMGUI_GETMIDHEIGHT(cy, sh) ((cy-XMGUI_BOTTOMBORDER-sh)-XMGUI_TOPBORDER)

void CMainFrame::OnSizePrepare(CRect *tabs, long lSizerRight, int cx, int cy)
{
	//get info on the tab control
	int statush = mStatus.IsFolded()?XMGUI_NETSIZEF:XMGUI_NETSIZE;
	*tabs = CRect(lSizerRight, XMGUI_TOPBORDER, cx-XMGUI_BORDERRIGHT, (cy-XMGUI_BOTTOMBORDER-statush));
	mCurrentViewRect = *tabs;
	mTabs.AdjustRect(FALSE, &mCurrentViewRect);
	mCurrentViewRect.DeflateRect(1, 2, 1, 1);
}

void CMainFrame::OnSizeFinish(CRect &rTabs)
{
	//create a region defining the client area of the tab,
	//minus the display region..
	HRGN hr = CreateRectRgn(0,0,1,1);
	HRGN hrBase = CreateRectRgn(0, 0, rTabs.Width(), rTabs.Height());
	HRGN hrDiff = CreateRectRgn(	mCurrentViewRect.left - rTabs.left,
									mCurrentViewRect.top - rTabs.top,
									mCurrentViewRect.right - rTabs.left,
									mCurrentViewRect.bottom - rTabs.top);
	CombineRgn(hr, hrBase, hrDiff, RGN_DIFF);
	mTabs.SetWindowRgn(hr, FALSE);
	//DeleteObject(hr);
	DeleteObject(hrBase);
	DeleteObject(hrDiff);
	
	//now redraw things
	//mTabs.RedrawWindow(NULL, NULL, RDW_UPDATENOW);
	if (mCurrentView) {
		mCurrentView->RedrawWindow(NULL, NULL, RDW_UPDATENOW);
	}
}

void CMainFrame::OnSize(UINT nType, int cx, int cy)
{
	//are we folded?
	bool folded = mShrink.IsFolded();
	int statush = mStatus.IsFolded()?XMGUI_NETSIZEF:XMGUI_NETSIZE;

	//get the sizes for controls
	CRect sizer, rTabs;
	mSplitter.GetWindowRect(&sizer);
	ScreenToClient(&sizer);
	OnSizePrepare(&rTabs, folded?2:sizer.right, cx, cy);

	//resize everything at once
	UINT flags = SWP_NOZORDER;
	UINT wincount = folded?5:7;
	if (mCurrentView)
		wincount++;
	HDWP h = BeginDeferWindowPos(wincount);
	h = DeferWindowPos(h, mAdvert.m_hWnd, NULL, (cx-XMGUI_ADVERTW)/2, XMGUI_TOPOFADD, XMGUI_ADVERTW, XMGUI_LOGOSIZE, flags);
	h = DeferWindowPos(h, mLogo.m_hWnd, NULL, cx-XMGUI_LOGOSIZE-XMGUI_TOPOFADD, XMGUI_TOPOFADD, XMGUI_LOGOSIZE, XMGUI_LOGOSIZE, flags);
	h = DeferWindowPos(h, mShrink.m_hWnd, NULL,
		2, XMGUI_TOPBORDER-XMGUI_FOLDBORDER, XMGUI_FOLDWIDTH, XMGUI_FOLDWIDTH, flags);
	if (!folded)
	{
		h = DeferWindowPos(h, mFiles.m_hWnd, NULL,
				0, XMGUI_TOPBORDER, sizer.left, XMGUI_GETMIDHEIGHT(cy, statush), flags);
		h = DeferWindowPos(h, mSplitter.m_hWnd, NULL, sizer.left, sizer.top, XMGUI_SPLITWIDTH, XMGUI_GETMIDHEIGHT(cy, statush), flags);
	}
	h = DeferWindowPos(h, mTabs.m_hWnd, NULL,
			rTabs.left, rTabs.top, rTabs.Width(), rTabs.Height(), flags);
	h = DeferWindowPos(h, mStatus, NULL, 0, cy-statush, cx, statush, flags);
	if (mCurrentView) {
		h = DeferWindowPos(h, mCurrentView->m_hWnd, NULL,
				mCurrentViewRect.left, mCurrentViewRect.top,
				mCurrentViewRect.Width(), mCurrentViewRect.Height(),
				flags);
	}
	EndDeferWindowPos(h);

	//redraw tab stuff
	OnSizeFinish(rTabs);
}

LRESULT CMainFrame::OnSplitterMove(WPARAM wParam, LPARAM lParam)
{
	//convert params
	long x = XM_SPLITLEFT(lParam);
	long cx = XM_SPLITRIGHT(lParam);

	//are we folded?
	bool folded = mShrink.IsFolded();
	int statush = mStatus.IsFolded()?XMGUI_NETSIZEF:XMGUI_NETSIZE;

	//prepare sizing stuff
	CRect rTabs, r;
	GetClientRect(&r);
	OnSizePrepare(&rTabs, cx, r.right, r.bottom);

	//move windows
	UINT wincount = 3;
	if (mCurrentView)
		wincount++;
	HDWP h = BeginDeferWindowPos(wincount);
	h = DeferWindowPos(h, mFiles.m_hWnd, NULL,
			0, XMGUI_TOPBORDER, x, XMGUI_GETMIDHEIGHT(r.Height(), statush), SWP_NOZORDER);
	h = mSplitter.DeferWindowPosAfterSplit(h);
	h = DeferWindowPos(h, mTabs.m_hWnd, NULL,
			rTabs.left, rTabs.top, rTabs.Width(), rTabs.Height(), SWP_NOZORDER);
	if (mCurrentView) {
		h = DeferWindowPos(h, mCurrentView->m_hWnd, NULL,
			mCurrentViewRect.left, mCurrentViewRect.top,
			mCurrentViewRect.Width(), mCurrentViewRect.Height(),
			SWP_NOZORDER);
	}
	EndDeferWindowPos(h);

	//redraw tabs and current view
	mFiles.RedrawWindow(NULL, NULL, RDW_UPDATENOW);
	OnSizeFinish(rTabs);

	return 0;
}

void CMainFrame::OnSizing(UINT nSide, LPRECT lpRect)
{
	//keep the window a certain size
	if ((lpRect->right-lpRect->left)<600) {
		lpRect->right = lpRect->left+600;
	}
	if ((lpRect->bottom-lpRect->top)<400) {
		lpRect->bottom = lpRect->top+400;
	}
}

// ----------------------------------------------------------- Construction, Creation

CMainFrame::CMainFrame()
: mTray(IDR_MAINFRAME)
{
	//zero stuff
	mMalloc = NULL;
	mSysImageList = NULL;
	mCurrentView = NULL;
	mCreateDone = false;
	mIgnoreFileClicks = false;
	mClosing = false;
	mWebView = NULL;

	//Create ourself
	CString strWndClass = AfxRegisterWndClass (
								0,
								AfxGetApp()->LoadStandardCursor(IDC_ARROW),
								(HBRUSH)(COLOR_3DFACE+1),
								AfxGetApp()->LoadIcon(IDR_MAINFRAME));
	//TRACE("Entering wait..\n");
	//Sleep(3000);
	//TRACE("Done waiting.\n");
	if (!Create(strWndClass, "Adult Media Swapper"))
	{
		AfxMessageBox("Could not create main window!");
	}

	//default browsing folder
	strcpy(CLocalView::mCurPath, config()->GetSharedFilesLocation(false));
}

CMainFrame::~CMainFrame()
{
	//release stuff
	StaticClose();
	if (mMalloc) {
		mMalloc->Release();
	}
	if (mSysImageList) {
		//NOTE: win9x, this destroys the system image list
		//ImageList_Destroy(mSysImageList);
	}
	if (mCurrentView) {
		mCurrentView->Release();
	}
}

int CMainFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CFrameWnd::OnCreate(lpCreateStruct) == -1)
		return -1;

	//initialize static stuff
	if (!StaticInit()) 
		return -1;

	//get splitter left
	long lSplitX = config()->GetFieldLong(FIELD_GUI_MAIN_SPLIT);

	//create the web browser
	if (!mAdvert.Create(
			NULL,
			"Advert",
			WS_CHILD|WS_VISIBLE,
			CRect(0,0,50,50),
			this,
			IDC_ADVERT,
			NULL))
	{
		return -1;
	}
	mAdvert.Navigate("http://ads.adultmediaswapper.com/request");
	
	//create contest button
	if (!mContest.Create(
			"Contest!",
			WS_CHILD|WS_VISIBLE,
			CRect(XMGUI_TOPOFADD, XMGUI_TOPOFADD, XMGUI_LOGOSIZE, XMGUI_LOGOSIZE-25),
			this,
			IDC_CONTEST))
	{
		return -1;
	}
	CFont f;
	f.CreateStockObject(ANSI_VAR_FONT);
	mContest.SetFont(&f);

	//create splitter
	if (!mSplitter.Create(
			this,
			IDC_SPLITTER,
			CRect(lSplitX, XMGUI_TOPBORDER, lSplitX+XMGUI_SPLITWIDTH, 300+XMGUI_TOPBORDER),
			64, 196,
			GetSysColor(COLOR_3DFACE))) {
		return -1;
	}

	//create file tree
	if (!mFiles.Create(
				WS_CHILD | WS_VISIBLE | WS_BORDER | TVS_HASLINES | TVS_HASBUTTONS | TVS_SHOWSELALWAYS,
				CRect(0,25,50,50),
				this,
				IDC_FILES)) {
		return -1;
	}
	mFiles.ModifyStyleEx(0, WS_EX_CLIENTEDGE);

	//shrink button
	if (!mShrink.Create(true, this, IDC_SHRINK, FIELD_GUI_TREE_FOLDED))
		return -1;

	//create tab control
	if (!mTabs.Create(WS_CHILD|WS_VISIBLE, CRect(0,0,50,50), this, IDC_TABS)) {
		return -1;
	}
	::SendMessage(mTabs.m_hWnd, WM_SETFONT, (WPARAM)GetStockObject(ANSI_VAR_FONT), 0);
	mTabs.InsertItem(TCIF_TEXT | TVIF_PARAM, 0, "Web", 0, XMGUIVIEW_WEB, 0, 0);
	mTabs.InsertItem(TCIF_TEXT | TVIF_PARAM, 1, "Search", 0, XMGUIVIEW_SEARCH, 0, 0);
	mTabs.InsertItem(TCIF_TEXT | TVIF_PARAM, 2, "Completed Downloads", 0, XMGUIVIEW_COMPLETED, 0, 0);
	mTabs.InsertItem(TCIF_TEXT | TVIF_PARAM, 3, "Saved Files", 0, XMGUIVIEW_SAVED, 0, 0);
	//mTabs.InsertItem(TCIF_TEXT | TVIF_PARAM, 3, "Local Files", 0, XMGUIVIEW_LOCAL, 0, 0);

	//create logo
	if (!mLogo.Create(	NULL,
						WS_CHILD | WS_VISIBLE | SS_BITMAP | SS_NOTIFY | SS_CENTERIMAGE,
						CRect(0,0,XMGUI_LOGOSIZE,XMGUI_LOGOSIZE),
						this,
						IDC_LOGO)) {
		return -1;
	}
	mLogo.SetBitmap(LoadBitmap(AfxGetResourceHandle(), MAKEINTRESOURCE(IDB_LOGO_80)));

	//create the status control
	mStatus.Create(NULL, NULL, WS_CHILD|WS_VISIBLE|WS_CLIPCHILDREN|WS_CLIPSIBLINGS, CRect(0,0,0,0), this, 0);

	//set the menu
	CMenu menu;
	if (!menu.LoadMenu(IDR_LOGO))
		return -1;
	#ifndef _INTERNAL
	menu.RemoveMenu(ID_INDEXER, 0);
	#endif
	if (!SetMenu(&menu))
		return -1;
	menu.Detach();

	//folded file tree
	if (mShrink.IsFolded())
	{
		//hide the file tree and splitter
		mFiles.ShowWindow(SW_HIDE);
		mSplitter.ShowWindow(SW_HIDE);
	}

	//initialize shell stuff
	if (!InitializeFileTree()) {
		return -1;
	}

	//show the search pane initially
	DisplayView(XMGUIVIEW_WEB);

	//subscribe to pipeline
	cm()->Subscribe(m_hWnd);
	sm()->Subscribe(m_hWnd);
	
	//done initializing
	mCreateDone = true;

	//draw the window
	UpdateWindow();

	//motd
	if (sm()->MotdIsNew())
	{
		sm()->MotdShow();
	}

	//show the icon
	mTray.SetNotificationWnd(this, XM_TRAYICON);
	mTray.SetIcon(IDR_MAINFRAME);

	return 0;
}

BOOL CMainFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	if( !CFrameWnd::PreCreateWindow(cs) )
		return FALSE;
	cs.style |= WS_CLIPCHILDREN;
	cs.dwExStyle &= ~WS_EX_CLIENTEDGE;

	//position the window
	cs.x = config()->GetFieldLong(FIELD_GUI_MAIN_WND_X);
	cs.y = config()->GetFieldLong(FIELD_GUI_MAIN_WND_Y);
	cs.cx = config()->GetFieldLong(FIELD_GUI_MAIN_WND_CX) - cs.x;
	cs.cy = config()->GetFieldLong(FIELD_GUI_MAIN_WND_CY) - cs.y;

	return TRUE;
}

void CMainFrame::OnDestroy()
{
	//destroy the view
	if (mCurrentView)
	{
		mCurrentView->DestroyWindow();
		mCurrentView->Release();
		mCurrentView = NULL;
	}

	//save our splitter position
	CRect r;
	mSplitter.GetWindowRect(&r);
	ScreenToClient(&r);
	config()->SetField(FIELD_GUI_MAIN_SPLIT, r.left);

	//save the window position
	WINDOWPLACEMENT wp;
	GetWindowPlacement(&wp);
	config()->SetField(FIELD_GUI_MAIN_WND_X, wp.rcNormalPosition.left);
	config()->SetField(FIELD_GUI_MAIN_WND_Y, wp.rcNormalPosition.top);
	config()->SetField(FIELD_GUI_MAIN_WND_CX, wp.rcNormalPosition.right);
	config()->SetField(FIELD_GUI_MAIN_WND_CY, wp.rcNormalPosition.bottom);
	config()->SetField(FIELD_GUI_MAIN_WND_MODE, wp.showCmd);

	//free query memory
	ClearQueryItems();
	
	//unsubscribe from pipeline
	cm()->UnSubscribe(m_hWnd);
	sm()->UnSubscribe(m_hWnd);

	//delete all sessions
	sessions()->FreeAll();

	//call base onclose
	CFrameWnd::OnDestroy();
}

// ---------------------------------------------------------------------------------- User Input

void CMainFrame::OnContest()
{
	DoContest(this);
}

void CMainFrame::OnExit()
{
	mClosing = true;
	PostMessage(WM_CLOSE);
}

void CMainFrame::OnSharedFiles()
{
	//no other action required
	DoSharedFiles(this);
}

void CMainFrame::OnHelp()
{
	//open a web browser window with the help
	ShellExecute(
		::GetDesktopWindow(),
		"open",
		"iexplore",
		"http://www.adultmediaswapper.com/support/software/",
		NULL,
		SW_SHOWDEFAULT);
}

void CMainFrame::OnConnect()
{
	//sm()->Lock();
	if (!sm()->LoginIsLoggedIn())
	{
		sm()->LoginUI();
	}
	//sm()->Unlock();
}

void CMainFrame::OnDisconnect()
{
	sm()->Lock();
	if (sm()->ServerIsOpen())
	{
		sm()->ServerClose();
	}
	sm()->Unlock();
}

void CMainFrame::OnAbout()
{
	DoAbout(this);
}

void CMainFrame::OnLogoClick()
{
	//create context menu
	DWORD dw = ::GetMessagePos();
	POINTS pos = MAKEPOINTS(dw);
	CMenu menu;
	menu.CreatePopupMenu();
	menu.AppendMenu(MF_POPUP, (UINT)LoadMenu(AfxGetResourceHandle(), (LPCSTR)IDR_LOGO));

	//modify menu
	#ifndef _INTERNAL
	menu.RemoveMenu(ID_INDEXER, 0);
	#endif
	
	//show menu
	menu.GetSubMenu(0)->TrackPopupMenu(TPM_LEFTALIGN|TPM_TOPALIGN, pos.x, pos.y, this, 0);
}
void CMainFrame::OnOptions()
{
	//show options
	config()->DoPrefs(false);
}
void CMainFrame::OnFastIndexer()
{
	//display the indexing window
	#ifdef _INTERNAL
	QueryFastIndexer();
	#endif
}

void CMainFrame::OnShrink()
{
	//toggle the shrink var
	bool folded = mShrink.IsFolded();
	
	//show/hide controls
	mFiles.ShowWindow(folded?SW_HIDE:SW_SHOW);
	mSplitter.ShowWindow(folded?SW_HIDE:SW_SHOW);

	//redraw everything
	CRect rc;
	GetClientRect(rc);
	OnSize(0, rc.Width(), rc.Height());
}

void CMainFrame::OnUpdateConnect(CCmdUI* pCmdUI)
{
	pCmdUI->Enable(
		sm()->LoginIsLoggedIn()?
		(pCmdUI->m_nID==ID_DISCONNECT):
		(pCmdUI->m_nID==ID_CONNECT));
}

// ---------------------------------------------------------------------------------- File Tree

struct XMTVItemData
{
	bool desktop;
	LPSHELLFOLDER folder;
	LPITEMIDLIST pidl;
};

void CMainFrame::OnFilesSelChanged(NMHDR *pnmh, LRESULT *pResult)
{
	//ignore this if we haven't fully
	//initialized yet
	if (!mCreateDone)
		return;
	if (mIgnoreFileClicks)
		return;

	//get the pidl of the selected node
	LPITEMIDLIST pidl;
	LPNMTREEVIEWA pnmtv = (LPNMTREEVIEW)pnmh;
	XMTVItemData *ptvi = (XMTVItemData*)pnmtv->itemNew.lParam;
	pidl = GetFullyQualifiedPIDL(ptvi->folder, ptvi->pidl);
	if (!pidl)
		return;
	
	//make sure we are in the local file browser
	DisplayView(XMGUIVIEW_LOCAL);
	CLocalView *plv = (CLocalView*)mCurrentView;
	if(!plv)
		return;

	//move to that folder
	plv->BrowseFolder(pidl);
	mMalloc->Free(pidl);
}

bool CMainFrame::InitializeFileTree()
{
	IShellFolder *pdesktop = NULL;
	LPITEMIDLIST pidl = NULL;
	SHFILEINFO fi;
	HTREEITEM hti = NULL;
	TVINSERTSTRUCT tvi;
	XMTVItemData *ptvid = NULL;
	
	//create malloc
	if (FAILED(SHGetMalloc(&mMalloc))) return false;
	
	//retrieve desktop location
	if (FAILED(SHGetSpecialFolderLocation(m_hWnd, CSIDL_DESKTOP, &pidl))) return false;

	//setup system image list
	mSysImageList = (HIMAGELIST)SHGetFileInfo(
								(LPCTSTR)pidl,
								NULL,
								&fi,
								sizeof(fi),
								SHGFI_PIDL|SHGFI_DISPLAYNAME|SHGFI_SYSICONINDEX|SHGFI_SMALLICON);
	if (!mSysImageList) return false;
	TreeView_SetImageList(mFiles.m_hWnd, mSysImageList, TVSIL_NORMAL);

	//setup our tag
	SHGetDesktopFolder(&pdesktop);
	ptvid = (XMTVItemData*)mMalloc->Alloc(sizeof(XMTVItemData));
	ptvid->folder = pdesktop;
	ptvid->pidl = pidl;
	ptvid->desktop = true;
	//	mMalloc->Free(pidl);


	//draw the desktop item
	tvi.hParent = TVI_ROOT;
	tvi.hInsertAfter = TVI_LAST;
	tvi.item.mask = TVIF_IMAGE | TVIF_SELECTEDIMAGE |
					TVIF_PARAM | TVIF_CHILDREN | TVIF_TEXT;
	tvi.item.cChildren = 1;
	tvi.item.iImage = fi.iIcon;
	tvi.item.iSelectedImage = fi.iIcon;
	tvi.item.lParam = (LPARAM)ptvid;
	tvi.item.pszText = fi.szDisplayName;
	hti = mFiles.InsertItem(&tvi);

	//expand the desktop node
	if (PopulateFileBranch(hti))
	{
		//show the current database path folder
		hti = GarunteeFolderVisible(config()->GetSharedFilesLocation(false));
		if (!hti)
			return false;
		mFiles.SelectItem(hti);
	}
	else
	{
		AfxMessageBox("Error populating tree.  Non-fatal.");
	}

	//success
	return true;
}

HTREEITEM CMainFrame::GarunteeFolderVisible(char* path)
{
	//get desktop shell folder
	LPSHELLFOLDER pshell = NULL;
	SHGetDesktopFolder(&pshell);
	if (!pshell)
		return NULL;

	//convert name to unicode
	OLECHAR wstr[MAX_PATH];
	MultiByteToWideChar(CP_ACP,
					MB_PRECOMPOSED,
					path,
					-1,
					wstr,
					MAX_PATH);

	//get the pidl of the folder
	LPITEMIDLIST pidl = NULL;
	pshell->ParseDisplayName(m_hWnd, NULL, wstr, NULL, &pidl, NULL);
	if (!pidl)
		return NULL;

	//start with the drive list
	LPITEMIDLIST pidlMyComp = NULL;
	SHGetSpecialFolderLocation(m_hWnd, CSIDL_DRIVES, &pidlMyComp);
	if (!pidlMyComp)
		return NULL;

	//make sure my comp is visible
	HTREEITEM hroot = mFiles.GetRootItem();
	HTREEITEM hti;
	mFiles.Expand(hroot, TVE_EXPAND);	

	//expand branches till we walk entire pidl
	LPITEMIDLIST pidlW = pidl;
	XMTVItemData *data;
	while(pidlW->mkid.cb)
	{
		//expand the branch with the current pidl
		hti = mFiles.GetChildItem(hroot);
		while (hti)
		{
			//test the data
			data = (XMTVItemData*)mFiles.GetItemData(hti);
			if (memcmp(data->pidl, pidlW, pidlW->mkid.cb)==0)
			{
				//match
				mFiles.Expand(hti, TVE_EXPAND);
				mFiles.EnsureVisible(hti);
				break;
			}
			hti = mFiles.GetNextSiblingItem(hti);
		}

		//get the next pidle
		hroot = hti;
		pidlW = GetPIDLNext(pidlW);
	}

	if (pidl) mMalloc->Free(pidl);
	if (pshell) pshell->Release();
	return hti;
}

bool CMainFrame::PopulateFileBranch(HTREEITEM hti)
{
	TVINSERTSTRUCTA tvi;
	LPSHELLFOLDER psh = NULL;
	LPITEMIDLIST pidl = NULL;
	LPENUMIDLIST penum = NULL;
	XMTVItemData *data = (XMTVItemData*)mFiles.GetItemData(hti);
	XMTVItemData *tvid = NULL;

	//fill out tvi data that stays the same
	tvi.hParent = hti;
	tvi.hInsertAfter = TVI_LAST;
	tvi.item.mask = TVIF_IMAGE | TVIF_SELECTEDIMAGE |
					TVIF_PARAM | TVIF_CHILDREN | TVIF_TEXT;
	tvi.item.pszText = LPSTR_TEXTCALLBACK;
	tvi.item.cChildren = I_CHILDRENCALLBACK;
	tvi.item.iImage = I_IMAGECALLBACK;
	tvi.item.iSelectedImage = I_IMAGECALLBACK;

	//get new object for this node
	if (data->desktop)
	{
		//this is the root node, don't try bindtoobject
		psh = data->folder;
		psh->AddRef();
	}
	else
	{
		//its not the root, go ahead and get child folder
		if (FAILED(data->folder->BindToObject(data->pidl, NULL, IID_IShellFolder, (void**)&psh)))
			return false;
	}
	
	//enumerate the children of this folder
	MSG msg;
	if (SUCCEEDED(psh->EnumObjects(m_hWnd, SHCONTF_FOLDERS, &penum)))
	{
		while(penum->Next(1, &pidl, NULL)==S_OK)
		{
			//setup the tag structure
			tvid = (XMTVItemData*)mMalloc->Alloc(sizeof(XMTVItemData));
			tvid->folder = psh;
			tvid->pidl = pidl;
			tvid->desktop = false;
			psh->AddRef();

			//insert the new item
			tvi.item.lParam = (LPARAM)tvid;
			mFiles.InsertItem(&tvi);
		}
		penum->Release();

		//pump messages
		while (::PeekMessage(&msg, NULL, 0, 0, PM_NOREMOVE))
		{
			if (!AfxGetApp()->PumpMessage())
				PostQuitMessage(0);
		}
	}

	//success
	psh->Release();
	return true;
}

void CMainFrame::OnFilesDeleteItem(NMHDR* pnmh, LRESULT* pResult)
{
	//free memory associated with this item
	LPNMTREEVIEW ptv = (LPNMTREEVIEW)pnmh;
	XMTVItemData *data = (XMTVItemData*)ptv->itemOld.lParam;
	if (data) {
		if (data->folder)
			data->folder->Release();
		if (data->pidl)
			mMalloc->Free(data->pidl);
		mMalloc->Free(data);
	}
}

void CMainFrame::OnFilesGetDispInfo(NMHDR* pnmh, LRESULT* pResult)
{
	//provide whatever information is asked for
	LPNMTVDISPINFO pdi = (LPNMTVDISPINFO)pnmh;
	XMTVItemData *data = (XMTVItemData*)pdi->item.lParam;

	if (pdi->item.mask & (TVIF_IMAGE|TVIF_SELECTEDIMAGE)) {
		
		//set image fields
		int i = GetChildIcon(pdi->item.hItem);
		pdi->item.iImage = i;
		pdi->item.iSelectedImage = i;
	}
	if (pdi->item.mask & TVIF_TEXT) {
		
		//set text field
		GetChildName(data->folder, data->pidl, SHGDN_INFOLDER, pdi->item.pszText);
	}
	if (pdi->item.mask & TVIF_CHILDREN) {

		//retreive the attributes for this item from the folder
		ULONG attr = SFGAO_HASSUBFOLDER;
		data->folder->GetAttributesOf(1, (LPCITEMIDLIST*)&data->pidl, &attr);
		pdi->item.cChildren = (attr&SFGAO_HASSUBFOLDER)?1:0;
	}

	//dont ask us for this again
	pdi->item.mask |= TVIF_DI_SETITEM;
}

void CMainFrame::OnFilesItemExpanding(NMHDR* pnmh, LRESULT* pResult)
{
	//clear any children
	LPNMTREEVIEW ptv = (LPNMTREEVIEW)pnmh;
	HTREEITEM hti;
	while (hti = mFiles.GetChildItem(ptv->itemNew.hItem))
		mFiles.DeleteItem(hti);
	
	//populate branch if expanding
	if (ptv->action & TVE_EXPAND)
		PopulateFileBranch(ptv->itemNew.hItem);
	if (ptv->action & TVE_COLLAPSE) {
		TVITEM tvi;
		tvi.hItem = ptv->itemNew.hItem;
		tvi.cChildren = 1;
		tvi.mask = TVIF_STATE | TVIF_CHILDREN;
		tvi.state = 0;
		tvi.stateMask = TVIS_EXPANDEDONCE | TVIS_EXPANDED;
		mFiles.SetItem(&tvi);
	}
}

void CMainFrame::GetChildName(LPSHELLFOLDER folder, LPITEMIDLIST item, DWORD flags, char *out)
{
	//get info on item
	STRRET name;
	folder->GetDisplayNameOf(item, flags, &name);
	if(name.uType==STRRET_CSTR) {
		memcpy(out, name.cStr, MAX_PATH);
	}
	if(name.uType==STRRET_WSTR) {
		WideCharToMultiByte(
				CP_ACP,
				0,
				name.pOleStr,
				-1,
				out,
				MAX_PATH,
				NULL,
				NULL);
		mMalloc->Free(name.pOleStr);
	}
	if(name.uType==STRRET_OFFSET) {
		strncpy(out, (char*)item+name.uOffset, MAX_PATH);
	}
}

LPITEMIDLIST CMainFrame::GetFullyQualifiedPIDL(LPSHELLFOLDER folder, LPITEMIDLIST item)
{
	//get the parsed name
	char str[MAX_PATH];
	OLECHAR wstr[MAX_PATH];
	GetChildName(folder, item, SHGDN_FORPARSING, str);

	//convert to ole str
	MultiByteToWideChar(CP_ACP,
						MB_PRECOMPOSED,
						str,
						-1,
						wstr,
						MAX_PATH);

	//convert back to pidl
	LPSHELLFOLDER pdesktop;
	LPITEMIDLIST pidl;
	SHGetDesktopFolder(&pdesktop);
	if (FAILED(pdesktop->ParseDisplayName(NULL, NULL, wstr, NULL, &pidl, NULL)))
	{
		pdesktop->Release();
		return NULL;
	}
	
	//sucess
	pdesktop->Release();
	return pidl;
}

int CMainFrame::GetChildIcon(HTREEITEM hti /*LPSHELLFOLDER folder, LPITEMIDLIST item*/)
{
	//get data
	XMTVItemData *tvi = (XMTVItemData*)mFiles.GetItemData(hti);
	LPSHELLFOLDER folder = tvi->folder;
	LPITEMIDLIST item = tvi->pidl;

	//try shell icon first
	bool bdofq = false;

	/*
	int icon;
	LPSHELLICON shicon = NULL;
	if (SUCCEEDED(folder->QueryInterface(IID_IShellIcon, (void**)&shicon)))
	{
		if (SUCCEEDED(shicon->GetIconOf(item, 0, &icon)))
		{
			//is the icon good?
			if (icon>0) {
				shicon->Release();
				return icon;
			}
		}	
		shicon->Release();
	}
	*/
	//NOTE: ^-- this code returns bogus results in release builds

	//walk up the tree, create a reverse ordered list of relative pidls
	XMTVItemData *pidlListData;
	LPITEMIDLIST pidlList[64];		//only 64 available
	HTREEITEM p = hti;
	int pidlListCount = 0;
	DWORD pidlSize = 0;
	while (p)
	{
		//get the item
		pidlListData = (XMTVItemData*)mFiles.GetItemData(p);
		if (!pidlListData->desktop)
		{
			//fill in the item
			pidlList[pidlListCount] = pidlListData->pidl;
			pidlListCount++;

			//keep track of how much room we need
			pidlSize += pidlListData->pidl->mkid.cb;
		}

		//get parent
		p = mFiles.GetParentItem(p);
	}
	pidlSize += sizeof(pidlList[0]->mkid.cb);
	if (pidlListCount<1)
		return -1;

	//walk the list we just made in reverse order (parent first),
	//building a fully qualified pidl
	LPITEMIDLIST pidl = (LPITEMIDLIST)mMalloc->Alloc(pidlSize);
	LPITEMIDLIST pidlPtr = pidl;
	LPITEMIDLIST pidlTemp;
	while (pidlListCount>0)
	{
		//copy the pidl
		pidlTemp = pidlList[pidlListCount-1];
		memcpy(pidlPtr, pidlTemp, pidlTemp->mkid.cb);
		pidlPtr = (LPITEMIDLIST)(((BYTE*)pidlPtr) + pidlTemp->mkid.cb);
		
		//next item
		pidlListCount--;
	}
	pidlPtr->mkid.cb = 0;	//terminating null
	
	 /*
	LPITEMIDLIST pidl = NULL;
	::SHGetSpecialFolderLocation(NULL, CSIDL_DESKTOP, &pidl);
	*/

	SHFILEINFO fi;
	if (!pidl) {
		return -1;
	}
	if (!SHGetFileInfo ((LPCTSTR)pidl, 0, &fi, sizeof(fi), SHGFI_PIDL|SHGFI_SYSICONINDEX|SHGFI_SMALLICON))
	{
		mMalloc->Free(pidl);
		return -1;
	}

	//success
	mMalloc->Free(pidl);
	return fi.iIcon;
}

LPITEMIDLIST CMainFrame::GetPIDLNext(LPITEMIDLIST pidl)
{
   LPSTR lpMem=(LPSTR)pidl;
   lpMem+=pidl->mkid.cb;
   return (LPITEMIDLIST)lpMem;
}

UINT CMainFrame::GetPIDLSize(LPITEMIDLIST pidl)
{
	//return the size needed to store the pidl
    UINT cbTotal = 0;
    if (pidl)
    {
        cbTotal += sizeof(pidl->mkid.cb);       // Null terminator
        while (pidl->mkid.cb)
        {
            cbTotal += pidl->mkid.cb;
            pidl = GetPIDLNext(pidl);
        }
    }
    return cbTotal;
}

LPITEMIDLIST CMainFrame::ClonePIDL(LPITEMIDLIST pidl)
{
	//create a copy of the pidl
	UINT size = GetPIDLSize(pidl);
	LPITEMIDLIST retval = (LPITEMIDLIST)mMalloc->Alloc(size);
	memcpy(retval, pidl, size);
	return retval;
}

// ----------------------------------------------------------------------------------- Splitter

BEGIN_MESSAGE_MAP(CVerticalSplitter, CWnd)
	
	//creation
	ON_WM_CREATE()

	//mouse tracking
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_WM_CANCELMODE()
	ON_WM_SETCURSOR()

	//graphics
	ON_WM_PAINT()

END_MESSAGE_MAP()

CVerticalSplitter::CVerticalSplitter()
{
	mIsTracking = false;
	mBorderLeft = 64;
	mBorderRight = 64;
	mCursor = NULL;
	mBackgroundColor = RGB(255,255,255);
}

CVerticalSplitter::~CVerticalSplitter()
{
	//free the cursor
	if (mCursor) {
		DeleteObject(mCursor);
	}
}

void CVerticalSplitter::SetBorders(long borderLeft, long borderRight)
{
	//just assign the borders
	mBorderLeft = borderLeft;
	mBorderRight = borderRight;
}

bool CVerticalSplitter::Create(CWnd* parent, UINT id, CRect &pos, long borderLeft, long borderRight, COLORREF bgcolor)
{
	mBorderLeft = borderLeft;
	mBorderRight = borderRight;
	mBackgroundColor = bgcolor;
	return CWnd::Create(
			NULL,
			"VerticalSplitter",
			WS_CHILD | WS_VISIBLE,
			pos,
			parent,
			id,
			NULL) == TRUE;
}

BOOL CVerticalSplitter::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	//base class
	if (!CWnd::OnCreate(lpCreateStruct)) {
		return FALSE;
	}

	//success
	return TRUE;
}

HDWP CVerticalSplitter::DeferWindowPosAfterSplit(HDWP hWinPosInfo)
{
	return DeferWindowPos(	hWinPosInfo,
							m_hWnd,
							NULL,
							mCurRect.left,
							mCurRect.top,
							mCurRect.Width(), 
							mCurRect.Height(),
							SWP_NOZORDER);
}

void CVerticalSplitter::OnPaint()
{
	//get painting context
	CPaintDC *pdc = new CPaintDC(this);
	CRect r;
	GetClientRect(&r);

	pdc->FillSolidRect(&r, mBackgroundColor);
	
	//release context
	delete pdc;
}

void CVerticalSplitter::DrawInvertRect(CRect &r)
{
	//setup gdi stuff
	CBrush* pBrush = CDC::GetHalftoneBrush();
	CClientDC *pdc = new CClientDC(GetParent());
	HBRUSH hOldBrush = NULL;
	if (pBrush != NULL)
		hOldBrush = (HBRUSH)SelectObject(pdc->m_hDC, pBrush->m_hObject);
	
	//erase old pattern
	pdc->PatBlt(r.left, r.top, r.Width(), r.Height(), PATINVERT);

	//clear gdi stuff
	if (hOldBrush != NULL)
		SelectObject(pdc->m_hDC, hOldBrush);
	delete pdc;
}

void CVerticalSplitter::OnLButtonDown(UINT nFlags, CPoint pt)
{
	//nothing to do if already tracking
	if (mIsTracking) return;
	mIsTracking = true;

	//capture the mouse
	SetCapture();

	//turn off clipchildren in our parent
	CWnd *pwnd = GetParent();
	pwnd->ModifyStyle(WS_CLIPCHILDREN, 0);

	//record our current position
	GetWindowRect(&mCurRect);
	pwnd->ScreenToClient(&mCurRect);
	mStartX = pt.x;

	//draw the splitter
	if(!config()->RegGetSmoothSplit())
		DrawInvertRect(mCurRect);
}

void CVerticalSplitter::OnLButtonUp(UINT nFlags, CPoint pt)
{
	//nothing to do if not tracking
	if (!mIsTracking) return;
	mIsTracking = false;

	//release the mouse
	ReleaseCapture();

	//turn on clipchildren
	CWnd *pwnd = GetParent();
	pwnd->ModifyStyle(0, WS_CLIPCHILDREN);

	//erase splitter
	if(!config()->RegGetSmoothSplit())
		DrawInvertRect(mCurRect);
	
	//inform our parent
	pwnd->SendNotifyMessage(
						XM_SPLITMOVE,
						(WPARAM)GetDlgCtrlID(), 
						(LPARAM)MAKELONG((WORD)mCurRect.left, (WORD)mCurRect.right));
}

void CVerticalSplitter::OnCancelMode()
{
	//cancel tracking
	if (mIsTracking) {

		//turn off tracking
		mIsTracking = false;
		ReleaseCapture();
		DrawInvertRect(mCurRect);

		//turn on clipchildren
		CWnd *pwnd = GetParent();
		pwnd->ModifyStyle(0, WS_CLIPCHILDREN);
	}
}

void CVerticalSplitter::OnMouseMove(UINT nFlags, CPoint pt)
{
	//are we tracking?
	if (mIsTracking) {

		//get parent window rect
		CRect r, rprime;
		GetParent()->GetClientRect(&r);
		
		//calculate new position
		GetWindowRect(&rprime);
		GetParent()->ScreenToClient(&rprime);
		rprime.OffsetRect(pt.x - mStartX, 0);

		//bounds check the new position
		if (rprime.right >= (r.right-mBorderRight))
		{
			//move as far right as allowed
			rprime.OffsetRect((r.right-mBorderRight)-rprime.right, 0);
		}
		if (rprime.left <= mBorderLeft)
		{
			//move as far left as we are allowed
			rprime.OffsetRect(mBorderLeft - rprime.left, 0);
		}

		//erase old, draw new pattern
		if(config()->RegGetSmoothSplit())
		{
			//record to position
			mCurRect = rprime;

			//inform our parent
			GetParent()->SendNotifyMessage(
						XM_SPLITMOVE,
						(WPARAM)GetDlgCtrlID(), 
						(LPARAM)MAKELONG((WORD)rprime.left, (WORD)rprime.right));

		}
		else
		{
			//move the split bar
			DrawInvertRect(mCurRect);
			DrawInvertRect(rprime);

			//record to position
			mCurRect = rprime;

		}
	}
}

BOOL CVerticalSplitter::OnSetCursor(CWnd *pWnd, UINT nHitTest, UINT message)
{
	//always use the horizontal splitter icon
	if (mCursor == NULL)
	{
		//load the splitter cursor
		mCursor = LoadCursor(AfxGetResourceHandle(), MAKEINTRESOURCE(IDC_VSPLIT));
	}

	//set the cursor
	SetCursor(mCursor);
	return TRUE;
}

// ----------------------------------------------------------------------------------- Tabs

void CMainFrame::OnTabsSelChange(NMHDR* pnmh, LRESULT* pResult)
{
	//switch to whatever view tab was clicked on
	switch (mTabs.GetCurSel())
	{
	case 0:
		DisplayView(XMGUIVIEW_WEB);
		break;
	case 1:
		DisplayView(XMGUIVIEW_SEARCH);
		break;
	case 2:
		DisplayView(XMGUIVIEW_COMPLETED);
		break;
	case 3:
		DisplayView(XMGUIVIEW_SAVED);
		break;
	/*
	case 3:
		DisplayView(XMGUIVIEW_LOCAL);
		break;
	*/
	}
}

void CMainFrame::DisplayView(UINT newView)
{
	//switch to particular view type
	if (mCurrentView)
	{
		//is this already the right view?
		if (mCurrentView->GetViewType() & newView)
		{
			switch (newView)
			{
			case XMGUIVIEW_SAVED:

				//saved files folder, show the folder, but dont record it
				mIgnoreFileClicks = true;
				((CLocalView*)mCurrentView)->BrowseFolder(config()->GetField(FIELD_DB_SAVE_PATH), false);
				mFiles.Select(GarunteeFolderVisible(config()->GetField(FIELD_DB_SAVE_PATH, false)), TVGN_CARET);
				mTabs.SetCurSel(2);
				mIgnoreFileClicks = false;
				break;
			
			case XMGUIVIEW_LOCAL:

				//local folder, show the last folder
				((CLocalView*)mCurrentView)->BrowseFolder((char*)NULL);
				mTabs.SetCurSel(-1);
				break;
			}
			return;
		}

		//release the curent view, unless web
		if (mCurrentView->GetViewType() != XMGUIVIEW_WEB)
		{
			mCurrentView->DestroyWindow();
			mCurrentView->Release();
		}
		else
		{
			//we just hide the view
			mCurrentView->ShowWindow(SW_HIDE);
			mCurrentView = NULL;
		}
	}

	//instansiate the new view
	switch (newView)
	{
	case XMGUIVIEW_WEB:
		
		//do we already have a web view created?
		if(mWebView)
		{
			//just assign to current view, and show it
			mCurrentView = mWebView;
		}
		else
		{
			//create new one
			mWebView = new CXMWebView();
			mCurrentView = mWebView;
		}
		mTabs.SetCurSel(0);
		break;

	case XMGUIVIEW_SEARCH:
		mCurrentView = (IXMGUIView*)new CSearchView();
		mTabs.SetCurSel(1);
		break;
	case XMGUIVIEW_COMPLETED:
		mCurrentView = (IXMGUIView*)new CCompletedView();
		mTabs.SetCurSel(2);
		break;
	case XMGUIVIEW_SAVED:
		mCurrentView = (IXMGUIView*)new CLocalView();
		mTabs.SetCurSel(3);
		break;
	case XMGUIVIEW_LOCAL:
		mCurrentView = (IXMGUIView*)new CLocalView();
		mTabs.SetCurSel(-1);
		break;
	}

	//create the window (web view will ignore multiple calls)
	if (mCurrentView->Create(this, mCurrentViewRect))
	{

		//post create stuff
		switch (newView)
		{
		case XMGUIVIEW_SAVED:
			mIgnoreFileClicks = true;
			((CLocalView*)mCurrentView)->BrowseFolder(config()->GetField(FIELD_DB_SAVE_PATH), false);
			mFiles.Select(GarunteeFolderVisible(config()->GetField(FIELD_DB_SAVE_PATH, false)), TVGN_CARET);
			mIgnoreFileClicks = false;
			break;
		case XMGUIVIEW_LOCAL:
			((CLocalView*)mCurrentView)->BrowseFolder((char*)NULL);
			break;
		}
	}
	else
	{
		ASSERT(FALSE);
	}
}

// ------------------------------------------------------------------------ Fold Button

BEGIN_MESSAGE_MAP(CFoldButton, CToolBarCtrl)
	ON_WM_DESTROY()
END_MESSAGE_MAP()

CFoldButton::CFoldButton()
{
	mField = NULL;
}

CFoldButton::~CFoldButton()
{
}

void CFoldButton::OnDestroy()
{
	//save our value
	if (mField)
	{
		config()->SetField(mField, IsFolded()?"true":"false");
	}

	//call base class destroy
	CToolBarCtrl::OnDestroy();
}

bool CFoldButton::IsFolded()
{
	//check the state of the first button
	return IsButtonChecked(ID_SHRINK)?true:false;
}

bool CFoldButton::Create(bool left, CWnd *parent, UINT idc, char* field)
{
	//create the toolbar
	if (!CToolBarCtrl::Create(
			WS_CHILD|WS_VISIBLE|CCS_NOPARENTALIGN|TBSTYLE_TOOLTIPS|
				CCS_NORESIZE|CCS_NODIVIDER|TBSTYLE_FLAT,
			CRect(0,0,XMGUI_FOLDWIDTH,XMGUI_FOLDWIDTH),
			parent,
			idc))
		return false;

	//create the imagelist
	if (!mImage.Create(
			left?IDB_SHRINKLEFT:IDB_SHRINKDOWN,
			16,
			0,
			RGB(255,0,255)))
		return false;
	SetImageList(&mImage);

	//add the toolbar button
	TBBUTTON b;
	b.dwData = 0;
	b.iBitmap = 0;
	b.idCommand = ID_SHRINK;
	b.iString = -1;
	b.fsState = TBSTATE_ENABLED;
	b.fsStyle = TBSTYLE_CHECK;
	if (!InsertButton(0, &b))
		return FALSE;

	//set its check state
	mField = field;
	if (mField)
	{
		if (config()->GetFieldBool(field))
		{
			CheckButton(ID_SHRINK, TRUE);
		}
	}

	//success
	return TRUE;
}

// -------------------------------------------------------------------------------------------- WEB

void CMainFrame::OnSearch(CXMQuery* query)
{
	//TODO: set post data detailing the query
	//TEMP:BEGIN
	//mAdvert.Refresh();
	//TEMP:END
}

BOOL CAdvert::Create(
	LPCTSTR lpszClassName, LPCTSTR lpszWindowName,
	DWORD dwStyle, const RECT &rect,
	CWnd *pParentWnd, UINT NID, CCreateContext *pContext)
{
	VARIANT_BOOL bsilent = TRUE;

	//new web browser control
	if (!CreateControl(
		CLSID_WebBrowser,
		"Advert",
		dwStyle|WS_CHILD|WS_VISIBLE,
		rect,
		pParentWnd,
		NID,
		NULL, FALSE, NULL))
		return false;

	//get the web browser interface
	IWebBrowser2 *web = NULL;
	IUnknown *i = GetControlUnknown();
	COM_SINGLECALL(i->QueryInterface(IID_IWebBrowser2, (void**)&web));

	//make silent
	COM_SINGLECALL(web->put_Silent(bsilent));

	//success
	COM_RELEASE(web);
	return TRUE;

fail:
	COM_RELEASE(web);
	return FALSE;
}

//navigate
void CAdvert::Navigate(const char* location)
{
	//get the web browser interface
	IWebBrowser2 *web = NULL;
	IUnknown *i = GetControlUnknown();
	COM_SINGLECALL(i->QueryInterface(IID_IWebBrowser2, (void**)&web));

	//navigate
	COM_SINGLECALL(web->Navigate(
		_bstr_t(location), 
		&_variant_t((short)navNoHistory),
		NULL, NULL, NULL));

	//success
	COM_RELEASE(web);
	return;

fail:
	COM_RELEASE(web);
	TRACE("CAdvert::Navigate(\"%s\") failed.\n", location);
}

void CAdvert::Refresh()
{
	//get the web browser interface
	IWebBrowser2 *web = NULL;
	IUnknown *i = GetControlUnknown();
	COM_SINGLECALL(i->QueryInterface(IID_IWebBrowser2, (void**)&web));

	//refresh
	COM_SINGLECALL(web->Refresh());

	//success
	COM_RELEASE(web);
	return;

fail:
	COM_RELEASE(web);
	TRACE("CAdvert::Refresh() failed.\n");
}

void CAdvert::Next()
{
	IWebBrowser2 *web = NULL;
	IUnknown *i = GetControlUnknown();
	COM_SINGLECALL(i->QueryInterface(IID_IWebBrowser2, (void**)&web));

	//refresh
	COM_SINGLECALL(web->GoForward());

	//success
	COM_RELEASE(web);
	return;

fail:
	COM_RELEASE(web);
	TRACE("CAdvert::Next() failed.\n");
}

void CAdvert::Back()
{
	IWebBrowser2 *web = NULL;
	IUnknown *i = GetControlUnknown();
	COM_SINGLECALL(i->QueryInterface(IID_IWebBrowser2, (void**)&web));

	//refresh
	COM_SINGLECALL(web->GoBack());

	//success
	COM_RELEASE(web);
	return;

fail:
	COM_RELEASE(web);
	TRACE("CAdvert::Back() failed.\n");
}

// ------------------------------------------------------------------------------------ WEB VIEW

BEGIN_MESSAGE_MAP(CXMWebView, CWnd)
	
	ON_WM_SIZE()

END_MESSAGE_MAP()

CXMWebView::CXMWebView()
{
	//set ref count
	m_udRefCount = 1;

	//variable init
	mIsValid = false;
}

CXMWebView::~CXMWebView()
{

}

bool CXMWebView::Create(CWnd *hwParent, CRect &rect)
{
	//have we already created ourselves?
	if (mIsValid)
	{
		//move ourself to the rectangle
		MoveWindow(&rect, TRUE);

		//show us
		ShowWindow(SW_SHOW);
	}
	else
	{
		//create this
		if(!CWnd::Create(NULL, "CXMWebView_Wnd", WS_CHILD|WS_VISIBLE,
			rect, hwParent, 0, NULL))
		{
			return false;
		}

		//create the web browser wnd
		if (!mWeb.Create(NULL, "WebCtrl", WS_CHILD|WS_VISIBLE,
			CRect(0,0,rect.Width(),rect.Height()), this, 0, NULL))

		{
			return false;
		}

		//browse to ams.com, ?sid=<sessionid>
		GoHome();

		//success
		mIsValid = true;
	}

	//success
	return true;
}

void CXMWebView::OnSize(UINT nType, int cx, int cy)
{
	//browser control fills window
	if (::IsWindow(mWeb.m_hWnd))
		mWeb.MoveWindow(0,0,cx,cy, TRUE);
}

void CXMWebView::AddRef()
{
	m_udRefCount++;
}

void CXMWebView::Release()
{
	m_udRefCount--;
	if (m_udRefCount < 1)
	{
		delete this;
	}
}

UINT CXMWebView::GetViewType()
{
	return XMGUIVIEW_WEB;
}

void CXMWebView::GoHome()
{
	//check mweb
	if (!IsWindow(mWeb.m_hWnd))
		return;

	//ams.com?sid=<sessionid>
	CString str;
	str.Format("http://welcome.adultmediaswapper.com/?at=%s", sm()->LoginGetSession().GetString());
	mWeb.Navigate(str);
}

void CXMWebView::GoNext()
{
	//check mweb
	if (!IsWindow(mWeb.m_hWnd))
		return;

	//next
	mWeb.Next();
}

void CXMWebView::GoBack()
{
	//check mweb
	if (!IsWindow(mWeb.m_hWnd))
		return;

	//back
	mWeb.Back();
}

void CXMWebView::GoRefresh()
{
	//check mweb
	if (!IsWindow(mWeb.m_hWnd))
		return;

	//refresh
	mWeb.Refresh();
}


// ------------------------------------------------------------------------------------ TRAY ICON

void CMainFrame::OnClose()
{
	//test the closing var
	if(mClosing)
	{
		//are there any unsaved pictures?
		cm()->Lock();
		if (cm()->GetCompletedFileCount()>0)
		{
			//prompt user
			char sz[MAX_PATH+1];
			_snprintf(sz, MAX_PATH, "There are %d unsaved files. Are you sure you want to close AMS before you save them?", cm()->GetCompletedFileCount());
			int retval = ::MessageBox(m_hWnd, sz, "Save", MB_YESNO | MB_ICONQUESTION);
			if (retval==IDNO)
			{	
				//reset the closing flag, otherwise the x button
				//will try to close the program
				mClosing = false;

				//cancel the close op
				cm()->Unlock();
				return;
			}
		}
		cm()->Unlock();

		//no unsaved pics, or they pressed 'yes'
		DestroyWindow();
	}
	else
	{
		//unless we set the closing flag, just hide
		//the window
		ShowWindow(SW_HIDE);
	}
}

LRESULT CMainFrame::OnTaskBarCreated(WPARAM wp, LPARAM lp)
{
	Sleep(2000);
	mTray.SetNotificationWnd(this, XM_TRAYICON);
	mTray.SetIcon(IDR_MAINFRAME);
	return 0;
}

LRESULT CMainFrame::OnTrayNotification(WPARAM uID, LPARAM lEvent)
{  
	return mTray.OnTrayNotification(uID, lEvent);
}

void CMainFrame::OnTrayShow()
{
	//do we need a password?
	if (config()->GetFieldBool(FIELD_LOGIN_PROTECT_ENABLE))
	{
		if (!IsWindowVisible())
		{
			if (!DoPasswordCheck())
				return;
		}
	}
	
	WINDOWPLACEMENT wp;
	wp.length = sizeof(wp);
	wp.flags = 0;
	GetWindowPlacement(&wp);

	int nshow;
	if (wp.showCmd==SW_SHOWMINIMIZED)
		nshow = SW_RESTORE;
	else
		nshow = SW_SHOW;
	ShowWindow(nshow);
}

void CMainFrame::OnTrayOptions()
{
	//do we need a password?
	if (config()->GetFieldBool(FIELD_LOGIN_PROTECT_ENABLE))
	{
		if (!IsWindowVisible())
		{
			if (!DoPasswordCheck())
				return;
		}
	}
	config()->DoPrefs(false);
}

//////////////////////////////////////////////////////
// CTrayIcon Copyright 1996 Microsoft Systems Journal.
//
// If this code works, it was written by Paul DiLascia.
// If not, I don't know who wrote it.

IMPLEMENT_DYNAMIC(CTrayIcon, CCmdTarget)

CTrayIcon::CTrayIcon(UINT uID)
{
	// Initialize NOTIFYICONDATA
	memset(&m_nid, 0 , sizeof(m_nid));
	m_nid.cbSize = sizeof(m_nid);
	m_nid.uID = uID; // never changes after construction
	
	// Use resource string as tip if there is one
	AfxLoadString(uID, m_nid.szTip, sizeof(m_nid.szTip));
}

CTrayIcon::~CTrayIcon()
{
	SetIcon(0); // remove icon from system tray
}

//////////////////
// Set notification window. It must created already.
//
void CTrayIcon::SetNotificationWnd(CWnd* pNotifyWnd, UINT uCbMsg)
{
	// If the following assert fails, you're p obably
	// calling me before you created your window. Oops.
	ASSERT(pNotifyWnd==NULL || ::IsWindow(pNotifyWnd->GetSafeHwnd()));
	m_nid.hWnd = pNotifyWnd->GetSafeHwnd();
	
	ASSERT(uCbMsg==0 || uCbMsg>=WM_USER);
	m_nid.uCallbackMessage = uCbMsg;
}

//////////////////
// This is the main variant for setting the icon.
// Sets both the icon and tooltip from resource ID
// To remove the icon, call SetIcon(0)
//
BOOL CTrayIcon::SetIcon(UINT uID)
{ 
	HICON hicon=NULL;
	if (uID) {
		AfxLoadString(uID, m_nid.szTip, sizeof(m_nid.szTip));
		hicon = AfxGetApp()->LoadIcon(uID);
	}
	return SetIcon(hicon, NULL);
}

//////////////////
// Common SetIcon for all overloads. 
//
BOOL CTrayIcon::SetIcon(HICON hicon, LPCSTR lpTip) 
{
	UINT msg;
	m_nid.uFlags = 0;
	
	// Set the icon
	if (hicon) {
		// Add or replace icon in system tray
		msg = m_nid.hIcon ? NIM_MODIFY : NIM_ADD;
		m_nid.hIcon = hicon;
		m_nid.uFlags |= NIF_ICON;
	} else { // remove icon from tray
		if (m_nid.hIcon==NULL)
			return TRUE; // already deleted
		msg = NIM_DELETE;
	}
	
	// Use the tip, if any
	if (lpTip)
		strncpy(m_nid.szTip, lpTip, sizeof(m_nid.szTip));
	if (m_nid.szTip[0])
		m_nid.uFlags |= NIF_TIP;
	
	// Use callback if any
	if (m_nid.uCallbackMessage && m_nid.hWnd)
		m_nid.uFlags |= NIF_MESSAGE;
	
	// Do it
	BOOL bRet = Shell_NotifyIcon(msg, &m_nid);
	if (msg==NIM_DELETE || !bRet)
		m_nid.hIcon = NULL; // failed
	return bRet;
}

/////////////////
// Default event handler handles right-menu and doubleclick.
// Call this function from your own notification handler.
//
LRESULT CTrayIcon::OnTrayNotification(WPARAM wID, LPARAM lEvent)
{
	if (wID!=m_nid.uID ||
		(lEvent!=WM_RBUTTONUP && lEvent!=WM_LBUTTONUP))
		return 0;
	
	// If there's a resource menu with the same ID as the icon, use it as 
	// the right-button popup menu. CTrayIcon will interprets the first
	// item in the menu as the default command for WM_LBUTTONDBLCLK
	// 
	CMenu menu;
	if (!menu.LoadMenu(m_nid.uID))
		return 0;
	CMenu* pSubMenu = menu.GetSubMenu(0);
	if (!pSubMenu) 
		return 0;
	
	if (lEvent==WM_RBUTTONUP) {
		
		// Make first menu item the default (bold font)
		::SetMenuDefaultItem(pSubMenu->m_hMenu, 0, TRUE);
		
		// Display the menu at the current mouse location. There's a "bug"
		// (Microsoft calls it a feature) in Windows 95 that requires calling
		// SetForegroundWindow. To find out more, search for Q135788 in MSDN.
		//
		CPoint mouse;
		GetCursorPos(&mouse);
		::SetForegroundWindow(m_nid.hWnd); 
		::TrackPopupMenu(pSubMenu->m_hMenu, 0, mouse.x, mouse.y, 0,
			m_nid.hWnd, NULL);
		
	} else // double click: execute first menu item
		::SendMessage(m_nid.hWnd, WM_COMMAND, pSubMenu->GetMenuItemID(0), 0);
	
	return 1; // handled
} 