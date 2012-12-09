// XMGUISTATUS.CPP ---------------------------------------------------- XMGUISTATUS.CPP

#include "stdafx.h"
#include "xmclient.h"
#include "xmnet.h"
#include "xmpipeline.h"
#include "xmgui.h"

// -------------------------------------------------------------- CXMGUIProgressRibbon

BEGIN_MESSAGE_MAP(CXMGUIProgressRibbon, CWnd)

	ON_WM_PAINT()

END_MESSAGE_MAP()

CXMGUIProgressRibbon::CXMGUIProgressRibbon()
{
	//default values
	mMaxValue = 1;
	mValue = 0;
	mBorder = true;

	//get system colors
	mBackColor = GetSysColor(COLOR_3DFACE);
	mBarColor = GetSysColor(COLOR_HIGHLIGHT);
	mTextColor = GetSysColor(COLOR_HIGHLIGHTTEXT);

	//blank text
	mText[0] = '\0';
}

CXMGUIProgressRibbon::~CXMGUIProgressRibbon()
{
}

void CXMGUIProgressRibbon::OnPaint()
{
	//get a dc to paint with
	CPaintDC dc(this);

	//get our client area
	CRect r;
	GetClientRect(r);

	//draw background
	dc.FillSolidRect(&r, mBackColor);
	
	//draw bar
	CRect rBar(r);
	rBar.right = (mMaxValue>0)?((r.right+mMaxValue)/mMaxValue)*mValue:0;
	dc.FillSolidRect(rBar, mBarColor);

	//draw text overlay
	CFont fontANSI, *poldFont;
	fontANSI.CreateStockObject(ANSI_VAR_FONT);
	poldFont = dc.SelectObject(&fontANSI);
	dc.SetTextColor(mTextColor);
	dc.SetBkColor(mBackColor);
	dc.SetBkMode(TRANSPARENT);
	r.top += 5;
	dc.DrawText(mText, -1, &r, DT_CENTER|DT_VCENTER|DT_END_ELLIPSIS);
	if (poldFont)
		dc.SelectObject(poldFont);

	//draw border
	r.top -= 5;
	if (mBorder)
		dc.DrawEdge(&r, EDGE_SUNKEN, BF_RECT);
}

// ----------------------------------------------------------------- CXMGUIStatusLight

BEGIN_MESSAGE_MAP(CXMGUIStatusLight, CWnd)

	ON_WM_PAINT()

END_MESSAGE_MAP()

CXMGUIStatusLight::CXMGUIStatusLight()
{
	//default stuff
	mBorder = true;
	mDimColor = RGB(16,64,16);		//dark, pale green
	mBrightColor = RGB(64,255,64);	//bright, pale green
	mBackColor = GetSysColor(COLOR_3DFACE);
	mLit = false;
}

CXMGUIStatusLight::~CXMGUIStatusLight()
{
}

void CXMGUIStatusLight::OnPaint()
{
	//get dc to paint with
	CPaintDC dc(this);

	//get our client rect, we always redraw the whole thing
	CRect r;
	GetClientRect(r);

	//draw the background
	dc.FillSolidRect(r, mBackColor);

	//draw either a dim or bright circle
	CBrush light(mLit?mBrightColor:mDimColor);
	CBrush *poldBrush = dc.SelectObject(&light);
	CPen penBlank(PS_SOLID|PS_COSMETIC, 1, GetSysColor(COLOR_3DFACE));
	CPen *poldPen = dc.SelectObject(&penBlank);
	r.DeflateRect(3,3,3,3);
	dc.Ellipse(r);
	if (poldBrush)
		dc.SelectObject(poldBrush);

	//draw the border?
	if (mBorder)
	{
		//setup pens
		CPen penHighlight(PS_SOLID|PS_COSMETIC, 1, GetSysColor(COLOR_3DHILIGHT));
		CPen penShadow(PS_SOLID|PS_COSMETIC, 1, GetSysColor(COLOR_3DSHADOW));

		//draw highlight
		CPoint pntTopRight(r.right, r.top);
		CPoint pntBottomLeft(r.left, r.bottom);
		dc.SelectObject(&penHighlight);
		dc.Arc(r, pntBottomLeft, pntTopRight);
		
		//draw shadow
		dc.SelectObject(&penShadow);
		dc.Arc(r, pntTopRight, pntBottomLeft);
	}

	if (poldPen)
		dc.SelectObject(poldPen);
}

// ---------------------------------------------------------------------- CXMGUIStatus

BEGIN_MESSAGE_MAP(CXMGUIStatus, CWnd)
	
	//windowing stuff
	ON_WM_CREATE()
	ON_WM_DESTROY()
	ON_WM_SIZE()
	ON_WM_PAINT()
	ON_WM_TIMER()

	//splitter message
	ON_MESSAGE(XM_SPLITMOVE, OnSplitMove)

	//pipeline updates
	ON_MESSAGE(XM_CLIENTMSG, OnClientMessage)
	ON_MESSAGE(XM_SERVERMSG, OnServerMessage)

	//tab switches
	ON_NOTIFY(TCN_SELCHANGE, IDC_TABS, OnTabsSelChange)

	//listview stuff
	ON_NOTIFY(LVN_DELETEITEM, IDC_FILES, OnThumbsDeleteItem)

	//fold button
	ON_COMMAND(ID_SHRINK, OnShrink)

END_MESSAGE_MAP()

CXMGUIStatus::CXMGUIStatus()
{
	mAllControlsCreated = false;
}

CXMGUIStatus::~CXMGUIStatus()
{
}

void CXMGUIStatus::OnPaint()
{
	//paint whatever was requested
	CPaintDC dc(this);
	dc.FillSolidRect(&dc.m_ps.rcPaint, GetSysColor(COLOR_3DFACE));
}

void CXMGUIStatus::OnShrink()
{
	//show, hide the controls
	bool folded = IsFolded();
	mTabs.ShowWindow(folded?SW_HIDE:SW_SHOW);
	mFiles.ShowWindow(folded?SW_HIDE:SW_SHOW);
	mSplitter.ShowWindow(folded?SW_HIDE:SW_SHOW);
	mLightRxLabel.ShowWindow(folded?SW_HIDE:SW_SHOW);
	mLightTxLabel.ShowWindow(folded?SW_HIDE:SW_SHOW);
	mLightServerLabel.ShowWindow(folded?SW_HIDE:SW_SHOW);

	//onsize the parent
	CRect rc;
	CMainFrame *f = (CMainFrame*)GetParent();
	f->GetClientRect(rc);
	f->OnSize(0, rc.Width(), rc.Height());
}

int CXMGUIStatus::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	//let CWnd initialize
	if (CWnd::OnCreate(lpCreateStruct)!=0)
		return -1;

	//load splitter pos
	mSplitFromRight = config()->GetFieldLong(FIELD_GUI_STATUS_SPLIT);

	//create out controls
	if (!mShrink.Create(false, this, IDC_SHRINK, FIELD_GUI_STATUS_FOLDED))
		return -1;
	if (!mTabs.Create(WS_CHILD|WS_VISIBLE|TCS_VERTICAL|TCS_MULTILINE|TCS_FIXEDWIDTH, CRect(0,0,50,50), this, IDC_TABS))
		return -1;
	if (!mFiles.CreateEx(WS_EX_CLIENTEDGE, WC_LISTVIEW, "", WS_CHILD|WS_VISIBLE|WS_BORDER|LVS_AUTOARRANGE, CRect(0,0,0,0), this, IDC_FILES))
		return -1;
	if (!mSplitter.Create(this, 0, CRect(mSplitFromRight,0,mSplitFromRight+XMGUI_SPLITWIDTH,0), 128, 200, GetSysColor(COLOR_3DFACE)))
		return -1;
	if (!mSearchLabel.Create("Search", WS_CHILD|WS_VISIBLE|SS_RIGHT, CRect(0,0,0,0), this, 0))
		return -1;
	if (!mDownloadsLabel.Create("Downloads", WS_CHILD|WS_VISIBLE|SS_RIGHT, CRect(0,0,0,0), this, 0))
		return -1;
	if (!mUploadsLabel.Create("Uploads", WS_CHILD|WS_VISIBLE|SS_RIGHT, CRect(0,0,0,0), this, 0))
		return -1;
	if (!mSearchRibbon.Create(NULL, NULL, WS_CHILD|WS_VISIBLE, CRect(0,0,0,0), this, 0))
		return -1;
	if (!mDownloadsRibbon.Create(NULL, NULL, WS_CHILD|WS_VISIBLE, CRect(0,0,0,0), this, 0))
		return -1;
	if (!mUploadsRibbon.Create(NULL, NULL, WS_CHILD|WS_VISIBLE, CRect(0,0,0,0), this, 0))
		return -1;
	if (!mLightServerLabel.Create("Server", WS_CHILD|WS_VISIBLE, CRect(0,0,0,0), this, 0))
		return -1;
	if (!mLightTxLabel.Create("Tx", WS_CHILD|WS_VISIBLE, CRect(0,0,0,0), this, 0))
		return -1;
	if (!mLightRxLabel.Create("Rx", WS_CHILD|WS_VISIBLE, CRect(0,0,0,0), this, 0))
		return -1;
	if (!mLightServer.Create(NULL, NULL, WS_CHILD|WS_VISIBLE, CRect(0,0,0,0), this, 0))
		return -1;
	if (!mLightTx.Create(NULL, NULL, WS_CHILD|WS_VISIBLE, CRect(0,0,0,0), this, 0))
		return -1;
	if (!mLightRx.Create(NULL, NULL, WS_CHILD|WS_VISIBLE, CRect(0,0,0,0), this, 0))
		return -1;

	//set fonts
	HGDIOBJ h = GetStockObject(ANSI_VAR_FONT);
	::SendMessage(mTabs.m_hWnd, WM_SETFONT, (WPARAM)h, 0);
	::SendMessage(mSearchLabel.m_hWnd, WM_SETFONT, (WPARAM)h, 0);
	::SendMessage(mDownloadsLabel.m_hWnd, WM_SETFONT, (WPARAM)h, 0);
	::SendMessage(mUploadsLabel.m_hWnd, WM_SETFONT, (WPARAM)h, 0);
	::SendMessage(mSearchRibbon.m_hWnd, WM_SETFONT, (WPARAM)h, 0);
	::SendMessage(mDownloadsRibbon.m_hWnd, WM_SETFONT, (WPARAM)h, 0);
	::SendMessage(mUploadsRibbon.m_hWnd, WM_SETFONT, (WPARAM)h, 0);
	::SendMessage(mLightServerLabel.m_hWnd, WM_SETFONT, (WPARAM)h, 0);
	::SendMessage(mLightTxLabel.m_hWnd, WM_SETFONT, (WPARAM)h, 0);
	::SendMessage(mLightRxLabel.m_hWnd, WM_SETFONT, (WPARAM)h, 0);
	::SendMessage(mLightServer.m_hWnd, WM_SETFONT, (WPARAM)h, 0);
	::SendMessage(mLightTx.m_hWnd, WM_SETFONT, (WPARAM)h, 0);
	::SendMessage(mLightRx.m_hWnd, WM_SETFONT, (WPARAM)h, 0);

	//setup tab images
	mTabImages.Create(IDB_TABS, 16, 0, RGB(192,192,192));
	mTabs.SetImageList(&mTabImages);
	mTabs.SetItemSize(CSize(32, 16));

	//create tabs
	TCITEMA tci;
	tci.mask = TCIF_IMAGE|TCIF_TEXT;
	tci.iImage = 1;
	tci.pszText = "";
	mTabs.InsertItem(0, &tci);
	tci.iImage = 0;
	tci.pszText = "";
	mTabs.InsertItem(1, &tci);

	//setup image list for thumbs
	mFiles.SetImageList(const_cast<CImageList*>(mThumbsImages.SILGetImageList()), LVSIL_NORMAL);

	//setup the max values for the progress controls
	mDownloadsRibbon.mMaxValue = cm()->GetDownloadSlotCount()+1;
	mUploadsRibbon.mMaxValue = cm()->GetUploadSlotCount()+1;

	//is the server connected?
	mLightServer.mLit = sm()->ServerIsOpen();
	mLightServer.Invalidate();

	//initialize download/upload ribbons
	UpdateDownloadRibbon();
	UpdateUploadRibbon();

	//Are we folded?
	if (mShrink.IsFolded())
	{
		mTabs.ShowWindow(SW_HIDE);
		mFiles.ShowWindow(SW_HIDE);
		mSplitter.ShowWindow(SW_HIDE);
		mLightRxLabel.ShowWindow(SW_HIDE);
		mLightTxLabel.ShowWindow(SW_HIDE);
		mLightServerLabel.ShowWindow(SW_HIDE);
	}
	
	//show downloads
	mTabState = XMGUI_DL;
	DLRefresh();

	//subscribe to pipeline
	cm()->Subscribe(m_hWnd, true);
	sm()->Subscribe(m_hWnd, true);

	//setup lights
	COLORREF
		dim = RGB(0,80,80),
		bright = RGB(0,255,255);
	mLightRx.mDimColor = dim;
	mLightRx.mBrightColor = bright;
	mLightTx.mDimColor = dim;
	mLightTx.mBrightColor = bright;
	SetTimer(3256, 500, NULL);

	//success
	mAllControlsCreated = true;
	return 1;
}

void CXMGUIStatus::OnDestroy()
{
	//save splitter pos
	config()->SetField(FIELD_GUI_STATUS_SPLIT, mSplitFromRight);

	//unsubscribe
	cm()->UnSubscribe(m_hWnd);
	sm()->UnSubscribe(m_hWnd);
}

// -------------------------------------------------------------------------------------- Sizing

//#define XMGUI_NETSIZE			128
#define XMGUI_RIBBONH			24
#define XMGUI_LIGHTW			XMGUI_RIBBONH
#define XMGUI_LIGHTH			XMGUI_RIBBONH
#define XMGUI_RIBBONTEXTW		70
#define XMGUI_RIBBONTEXTH		XMGUI_RIBBONH
#define XMGUI_LIGHTTEXTW		45
#define XMGUI_LIGHTTEXTH		XMGUI_RIBBONH
#define XMGUI_ROWSPACE			6
#define XMGUI_ROW1T				(XMGUI_ROWSPACE*3)+(XMGUI_RIBBONH*0)
#define XMGUI_ROW2T				(XMGUI_ROWSPACE*4)+(XMGUI_RIBBONH*1)
#define XMGUI_ROW3T				(XMGUI_ROWSPACE*5)+(XMGUI_RIBBONH*2)

#define XMGUI_LABELOFFSET		4
#define XMGUI_STATUSBORDER		4

#define XMGUI_RIBBONW(_x, _cx)	_cx-_x-XMGUI_LIGHTW-XMGUI_LIGHTTEXTW-(XMGUI_STATUSBORDER*4)
//x: right edge of splitter
//cx: width of bar

void CXMGUIStatus::OnSize(UINT nType, int cx, int cy)
{
	//make sure we have been created
	if ((!m_hWnd) || (nType!=SIZE_MAXIMIZED && nType!=SIZE_RESTORED) || (!mAllControlsCreated))
	{
		return;
	}

	UINT flags = 0;
	int x, x2;

	if (IsFolded())
	{
		//figure the band width
		int ribw = (cx-(XMGUI_FOLDBORDER)-(XMGUI_RIBBONTEXTW*3)-(XMGUI_LIGHTW*3)-(XMGUI_STATUSBORDER*4))/3;

		HDWP h = ::BeginDeferWindowPos(10);
		h = ::DeferWindowPos(h, mShrink.m_hWnd,				NULL, 2, 0, XMGUI_FOLDWIDTH, XMGUI_FOLDWIDTH, flags);
		h = ::DeferWindowPos(h, mSearchLabel.m_hWnd,		NULL, (x=XMGUI_FOLDBORDER), XMGUI_LABELOFFSET, XMGUI_RIBBONTEXTW, XMGUI_RIBBONTEXTH, flags);
		h = ::DeferWindowPos(h, mSearchRibbon.m_hWnd,		NULL, (x+=XMGUI_RIBBONTEXTW+XMGUI_STATUSBORDER), 0, ribw, XMGUI_RIBBONH, flags);
		h = ::DeferWindowPos(h, mDownloadsLabel.m_hWnd,		NULL, (x+=ribw), XMGUI_LABELOFFSET, XMGUI_RIBBONTEXTW, XMGUI_RIBBONTEXTH, flags);
		h = ::DeferWindowPos(h, mDownloadsRibbon.m_hWnd,	NULL, (x+=XMGUI_RIBBONTEXTW+XMGUI_STATUSBORDER), 0, ribw, XMGUI_RIBBONH, flags);
		h = ::DeferWindowPos(h, mUploadsLabel.m_hWnd,		NULL, (x+=ribw), XMGUI_LABELOFFSET, XMGUI_RIBBONTEXTW, XMGUI_RIBBONTEXTH, flags);
		h = ::DeferWindowPos(h, mUploadsRibbon.m_hWnd,		NULL, (x+=XMGUI_RIBBONTEXTW+XMGUI_STATUSBORDER), 0, ribw, XMGUI_RIBBONH, flags);
		h = ::DeferWindowPos(h, mLightServer.m_hWnd,		NULL, (x+=ribw), 0, XMGUI_LIGHTW, XMGUI_LIGHTH, flags);
		h = ::DeferWindowPos(h, mLightTx.m_hWnd,			NULL, (x+=XMGUI_LIGHTW), 0, XMGUI_LIGHTW, XMGUI_LIGHTH, flags);
		h = ::DeferWindowPos(h, mLightRx.m_hWnd,			NULL, (x+=XMGUI_LIGHTW), 0, XMGUI_LIGHTW, XMGUI_LIGHTH, flags);
		::EndDeferWindowPos(h);
	}
	else
	{
		HDWP h = BeginDeferWindowPos(16);

		//calculate tab rect, and files rect
		CRect rTabs(XMGUI_FOLDBORDER,0,(cx-mSplitFromRight),cy-XMGUI_STATUSBORDER);
		CRect rFiles = rTabs;
		mTabs.AdjustRect(FALSE, &rFiles);
		rFiles.DeflateRect(4,2,2,2);

		h = DeferWindowPos(h, mShrink.m_hWnd			, NULL, 2, 0, XMGUI_FOLDWIDTH, XMGUI_FOLDWIDTH, flags);

		h = DeferWindowPos(h, mTabs.m_hWnd				, NULL, rTabs.left, rTabs.top, rTabs.Width(), rTabs.Height(), flags);
		h = DeferWindowPos(h, mFiles.m_hWnd				, NULL, rFiles.left, rFiles.top, rFiles.Width(), rFiles.Height(), flags);
		h = DeferWindowPos(h, mSplitter.m_hWnd			, NULL, (cx-mSplitFromRight), 0, XMGUI_SPLITWIDTH, cy-XMGUI_STATUSBORDER, flags);

		x = (cx-mSplitFromRight)+XMGUI_SPLITWIDTH;
		h = DeferWindowPos(h, mSearchLabel.m_hWnd		, NULL, x, XMGUI_ROW1T+XMGUI_LABELOFFSET, XMGUI_RIBBONTEXTW, XMGUI_RIBBONTEXTH, flags);
		h = DeferWindowPos(h, mDownloadsLabel.m_hWnd	, NULL, x, XMGUI_ROW2T+XMGUI_LABELOFFSET, XMGUI_RIBBONTEXTW, XMGUI_RIBBONTEXTH, flags);
		h = DeferWindowPos(h, mUploadsLabel.m_hWnd		, NULL, x, XMGUI_ROW3T+XMGUI_LABELOFFSET, XMGUI_RIBBONTEXTW, XMGUI_RIBBONTEXTH, flags);

		x += XMGUI_RIBBONTEXTW + XMGUI_STATUSBORDER;
		x2 = XMGUI_RIBBONW(x,cx);
		h = DeferWindowPos(h, mSearchRibbon.m_hWnd		, NULL, x, XMGUI_ROW1T, x2, XMGUI_RIBBONH, flags);
		h = DeferWindowPos(h, mDownloadsRibbon.m_hWnd	, NULL, x, XMGUI_ROW2T, x2, XMGUI_RIBBONH, flags);
		h = DeferWindowPos(h, mUploadsRibbon.m_hWnd		, NULL, x, XMGUI_ROW3T, x2, XMGUI_RIBBONH, flags);

		x += x2 + (XMGUI_STATUSBORDER*3);
		h = DeferWindowPos(h, mLightServer.m_hWnd		, NULL, x, XMGUI_ROW1T, XMGUI_LIGHTW, XMGUI_LIGHTH, flags);
		h = DeferWindowPos(h, mLightTx.m_hWnd			, NULL, x, XMGUI_ROW2T, XMGUI_LIGHTW, XMGUI_LIGHTH, flags);
		h = DeferWindowPos(h, mLightRx.m_hWnd			, NULL, x, XMGUI_ROW3T, XMGUI_LIGHTW, XMGUI_LIGHTH, flags);

		x += XMGUI_LIGHTW + XMGUI_STATUSBORDER;
		h = DeferWindowPos(h, mLightServerLabel.m_hWnd	, NULL, x, XMGUI_ROW1T+XMGUI_LABELOFFSET, XMGUI_LIGHTTEXTW, XMGUI_LIGHTTEXTH, flags);
		h = DeferWindowPos(h, mLightTxLabel.m_hWnd		, NULL, x, XMGUI_ROW2T+XMGUI_LABELOFFSET, XMGUI_LIGHTTEXTW, XMGUI_LIGHTTEXTH, flags);
		h = DeferWindowPos(h, mLightRxLabel.m_hWnd		, NULL, x, XMGUI_ROW3T+XMGUI_LABELOFFSET, XMGUI_LIGHTTEXTW, XMGUI_LIGHTTEXTH, flags);

		EndDeferWindowPos(h);
	}

	//Redraw ourselves
	//RedrawWindow();
}

LRESULT CXMGUIStatus::OnSplitMove(WPARAM wParam, LPARAM lParam)
{
	//move the splitter, dont redraw it
	long x = XM_SPLITLEFT(lParam);
	long cx = XM_SPLITRIGHT(lParam);
	CRect r;
	GetClientRect(r);
	mSplitter.MoveWindow(CRect(x, 0, XMGUI_SPLITWIDTH, r.Height()), FALSE);

	//set the splitfromright prop
	mSplitFromRight = r.right-x;

	//now call the general resize function
	this->OnSize(0, r.Width(), r.Height());

	return 0;
}

// ----------------------------------------------------------------------------------- Pipeline

LRESULT CXMGUIStatus::OnClientMessage(WPARAM wParam, LPARAM lParam)
{
	//get our tag
	CXMPipelineUpdateTag *tag = (CXMPipelineUpdateTag*)lParam;
	CXMGUIQueryItem *pqi = NULL;

	ULTag ult;
	switch (wParam)
	{
	case XM_CMU_QUEUE_ADD:

		//draw the item
		if (!tag->thumb)
		{
			cm()->Lock();
			DWORD i = cm()->FindQueuedFile(tag);
			if (i!=(DWORD)-1)
			{
				DLTagQ tagq = cm()->GetQueuedFile(i);
				DLInsert(tagq);
			}
			cm()->Unlock();
		}

		break;

	case XM_CMU_QUEUE_REMOVE:

		//remove the item
		if (!tag->thumb)
		{
			if (tag->flags & XMPUF_QFLUSH)
			{
				//removed from the queue because it began downloading..
				//handled in DOWNLOAD_START
			}
			else
			{
				//canceled or something
				DLRemove(&tag->md5);
			}
		}		
		break;

	case XM_CMU_DOWNLOAD_FINISH:

		//remove from the download strip
		if (!tag->thumb)
		{
			DLRemove(&tag->md5);
		}

		//update search ribbon
		pqi = CSearchView::GetQueryItemFromTag(tag);
		if (pqi && tag->thumb)
		{
			//increment the count
			mSearchRibbon.mValue++;	
			
			//is it done?
			if (mSearchRibbon.mValue==mSearchRibbon.mMaxValue)
			{
				mSearchRibbon.mBarColor = RGB(0,192,0);
				sprintf(mSearchRibbon.mText, "Finished (%d)", mSearchRibbon.mValue);
			}
			else
			{
				sprintf(mSearchRibbon.mText, "%d/%d", mSearchRibbon.mValue, mSearchRibbon.mMaxValue);
			}
			mSearchRibbon.Invalidate();
		}

		//update download ribbon
		UpdateDownloadRibbon();
		break;

	case XM_CMU_DOWNLOAD_START:

		//update download thumbs
		if (!tag->thumb)
		{
			if (tag->flags & XMPUF_QFLUSH)
			{
				//queued item has started downloading..
				DLText(&tag->md5, "Downloading...");
			}
			else
			{
				//new download started, skipped queue
				cm()->Lock();
				BYTE i = cm()->FindDownloadSlot(tag);
				if (i!=(BYTE)-1)
				{
					DLTagD tagd = cm()->GetDownloadSlot(i);
					DLInsert(tagd);
				}
				cm()->Unlock();
			}
		}

		//update download ribbon
		UpdateDownloadRibbon();
		break;

	case XM_CMU_DOWNLOAD_ERROR:
	case XM_CMU_DOWNLOAD_CANCEL:

		//remove from thumbnail display
		if (!tag->thumb)
		{
			DLRemove(&tag->md5);
		}

		//update download ribbon
		UpdateDownloadRibbon();
		break;

	case XM_CMU_COMPLETED_ADD:
	case XM_CMU_COMPLETED_REMOVE:
		break;

	case XM_CMU_UPLOAD_START:
	case XM_CMU_UPLOAD_FINISH:

		//find the item
		if (mTabState == XMGUI_UL)
		{
			cm()->Lock();
			for (BYTE i=0;i<cm()->GetUploadSlotCount();i++)
			{
				ult = cm()->GetUploadSlot(i);
				if (!ult->mIsThumb)
				{
					if (ult->mId.IsEqual(tag->md5))
					{
						//found.. what op?
						if (wParam == XM_CMU_UPLOAD_START)
							ULInsert(ult);
						else
							ULRemove(ult);
						break;
					}
				}
			}
			cm()->Unlock();
		}

		//update the ribbon
		UpdateUploadRibbon();
		break;
	}
	tag->Release();
	return 1;
}

LRESULT CXMGUIStatus::OnServerMessage(WPARAM wParam, LPARAM lParam)
{
	CXMQueryResponse *pResponse = NULL;
	switch (wParam)
	{
	case XM_SMU_QUERY_BEGIN:

		//blank progress bar
		mSearchRibbon.mValue = 0;
		strcpy(mSearchRibbon.mText, "Searching...");
		mSearchRibbon.Invalidate();
		break;
	
	case XM_SMU_QUERY_CANCEL:
	case XM_SMU_QUERY_ERROR:

		//blank progress bar
		mSearchRibbon.mValue = 0;
		strcpy(mSearchRibbon.mText, "Search Error.");
		mSearchRibbon.Invalidate();
		break;

	case XM_SMU_QUERY_FINISH:

		//set max value
		mSearchRibbon.mMaxValue = CSearchView::mQueryItems.GetCount();
		
		//set initial value to 0
		mSearchRibbon.mValue = 0;
		sprintf(mSearchRibbon.mText, "%d/%d", 0, mSearchRibbon.mMaxValue);

		//set color to highlight
		mSearchRibbon.mBarColor = GetSysColor(COLOR_HIGHLIGHT);
		mSearchRibbon.Invalidate();
		break;

	case XM_SMU_SERVER_CONNECTED:

		//server connected
		mLightServer.mLit = true;
		mLightServer.Invalidate();
		break;

	case XM_SMU_SERVER_ERROR:
	case XM_SMU_SERVER_CLOSED:

		//server disconnected
		mLightServer.mLit = false;
		mLightServer.Invalidate();
		break;
	}
	return 1;
}

void CXMGUIStatus::UpdateDownloadRibbon()
{
	//count number of current downloads
	cm()->Lock();
	int x = 0;
	for(BYTE i=0;i<cm()->GetDownloadSlotCount();i++)
	{
		if (cm()->GetDownloadSlot(i)->mState!=DSS_OPEN)
			x++;
	}
	cm()->Unlock();

	//update download ribbon
	mDownloadsRibbon.mValue = x;
	sprintf(mDownloadsRibbon.mText, "%d/%d", x, mDownloadsRibbon.mMaxValue);
	mDownloadsRibbon.Invalidate();
}

void CXMGUIStatus::UpdateUploadRibbon()
{
	//count number of current downloads
	cm()->Lock();
	int x = 0;
	for(BYTE i=0;i<cm()->GetUploadSlotCount();i++)
	{
		if (cm()->GetUploadSlot(i)->mState!=USS_OPEN)
			x++;
	}
	cm()->Unlock();

	//update download ribbon
	mUploadsRibbon.mValue = x;
	sprintf(mUploadsRibbon.mText, "%d/%d", x, mUploadsRibbon.mMaxValue);
	mUploadsRibbon.Invalidate();
}

void CXMGUIStatus::OnTabsSelChange(NMHDR* pnmh, LRESULT* pResult)
{
	//switch to whatever view tab was clicked on
	switch (mTabs.GetCurSel())
	{
	case 0:

		//switch to download if needed
		if (mTabState == XMGUI_UL)
		{
			ULClear();
			mTabState = XMGUI_DL;
			DLRefresh();
		}

		break;

	case 1:

		//switch to upload if needed
		if (mTabState == XMGUI_DL)
		{
			DLClear();
			mTabState = XMGUI_UL;
			ULRefresh();
		}
		break;
	}
}

void CXMGUIStatus::OnThumbsDeleteItem(NMHDR *pnmh, LRESULT* pResult)
{
	//what tab are showing?
	NMLISTVIEW *pnmlv = (NMLISTVIEW*)pnmh;
	if (mTabState == XMGUI_DL)
	{
		//download
		DLTag tag;
		tag = (DLTag)pnmlv->lParam;
		if (tag)
		{
			delete tag;
		}
	}
	else
	{
		//upload
		ULTag tag;
		tag = (ULTag)pnmlv->lParam;
		if (tag)
		{
			delete tag;
		}
	}

	//either way, clear the image list of our index
	LVITEM lvi;
	lvi.mask = LVIF_IMAGE;
	lvi.iItem = pnmlv->iItem;
	lvi.iSubItem = 0;
	if (mFiles.GetItem(&lvi))
	{
		mThumbsImages.SILDelete(lvi.iImage);
	}
}

// ------------------------------------------------------------------------------------ UPLOADS

void CXMGUIStatus::ULRefresh()
{
	//add every non-thumb upload slot
	ULTag tag;
	cm()->Lock();
	for (BYTE i=0;i<cm()->GetUploadSlotCount();i++)
	{
		tag = cm()->GetUploadSlot(i);
		if (!tag->mIsThumb &&
			tag->mState != USS_OPEN &&
			tag->mState != USS_CLOSING)
		{
			//insert the file
			ULInsert(tag);
		}	
	}
	cm()->Unlock();
}

void CXMGUIStatus::ULInsert(ULTag tag)
{
	//check state
	if (mTabState != XMGUI_UL)
		return;

	//get the thumbnail
	db()->Lock();
	CXMDBFile *f = db()->FindFile(tag->mMD5.GetValue());
	if (!f)
	{
		db()->Unlock();
		return;
	}
	CXMDBThumb *t = f->GetThumb(XMGUI_THUMBWIDTH, XMGUI_THUMBHEIGHT);
	if (!t)
	{
		db()->Unlock();
		return;
	}

	//copy the filename
	char text[MAX_PATH];
	sprintf(text, "%d x %d", f->GetWidth(), f->GetHeight());

	//decode the image
	BYTE *buf;
	DWORD bufsize = t->GetImage(&buf);
	CJPEGDecoder jpeg;
	CDIBSection dib;
	jpeg.MakeBmpFromMemory(buf, bufsize, &dib, 32);
	t->FreeImage(&buf);
	db()->Unlock();

	//copy the tag
	ULTag tag2 = (ULTag)malloc(sizeof(CXMClientManager::UploadSlot));
	memcpy(tag2, tag, sizeof(CXMClientManager::UploadSlot));

	//insert the item
	LVITEMA lvi;
	lvi.mask = LVIF_TEXT|LVIF_PARAM|LVIF_IMAGE;
	lvi.iItem = mFiles.GetItemCount();
	lvi.iSubItem = 0;
	lvi.iImage = mThumbsImages.SILInsert(&dib);
	lvi.lParam = (LPARAM)tag2;
	lvi.pszText = text;
	mFiles.InsertItem(&lvi);
}

void CXMGUIStatus::ULRemove(ULTag tag)
{
	//check state
	if (mTabState != XMGUI_UL)
		return;

	//find the item
	ULTag tag2;
	for (int i=0;i<mFiles.GetItemCount();i++)
	{
		tag2 = (ULTag)mFiles.GetItemData(i);
		if (tag2->mMD5.IsEqual(tag->mMD5))
		{
			//remove the item
			mFiles.DeleteItem(i);
			break;
		}
	}
}

void CXMGUIStatus::ULClear()
{
	//clear the imagelist.. memory is automagically freed
	mFiles.DeleteAllItems();
}

// ------------------------------------------------------------------------------------ DOWNLOADS

void CXMGUIStatus::DLRefresh()
{
	//first all queued items
	cm()->Lock();
	for (DWORD i=0;i<cm()->GetQueuedFileCount();i++)
	{
		DLTagQ tag = cm()->GetQueuedFile(i);
		if (!tag->mIsThumb)
		{
			DLInsert(tag);
		}
	}

	//now all downloading items
	for (BYTE j=0;j<cm()->GetDownloadSlotCount();j++)
	{
		DLTagD tag = cm()->GetDownloadSlot(j);
		if (!tag->mIsThumb &&
			(tag->mState == DSS_WAITING ||
			 tag->mState == DSS_RECEIVING))
		{
			DLInsert(tag);
		}
	}
	cm()->Unlock();
}

void CXMGUIStatus::DLClear()
{
	//clear the imagelist.. memory is automagically freed
	mFiles.DeleteAllItems();
}

void CXMGUIStatus::DLInsert(DLTagQ tag)
{
	//check state
	if (mTabState != XMGUI_DL)
		return;

	//insert
	cm()->Lock();
	DLInsert(&tag->mItem->mMD5, "Queued.");
	cm()->Unlock();
}

void CXMGUIStatus::DLInsert(DLTagD tag)
{
	//check state
	if (mTabState != XMGUI_DL)
		return;

	//insert
	cm()->Lock();
	DLInsert(&tag->mItem->mMD5, "Downloading...");
	cm()->Unlock();
}

void CXMGUIStatus::DLInsert(DLTag tag, const char* text)
{
	//check state
	if (mTabState != XMGUI_DL)
		return;

	
	//try the cache
	dbman()->Lock();
	CJPEGDecoder jpeg;
	CDIBSection dib;
	DWORD i = dbman()->FindCachedFileByParent(*tag);
	int x = -1;
	if (i != -1)
	{
		//get the cache record
		cachefile *pcf = dbman()->GetCachedFile(i);
		jpeg.MakeBmpFromMemory(pcf->file, pcf->size, &dib, 32);
		
		//insert into image list
		x = mThumbsImages.SILInsert(&dib);
	}
	dbman()->Unlock();

	//if its not in the cache, then we don't have it
	//here on the client. probobly they tried to download
	//before the thumbnail came in.. in the future, we
	//should display the thumbnail here when it becomes avail

	//insert into listview
	LVITEMA lvi;
	lvi.mask = LVIF_TEXT|LVIF_PARAM|LVIF_IMAGE;
	lvi.iItem = mFiles.GetItemCount();
	lvi.iSubItem = 0;
	lvi.iImage = x;
	lvi.lParam = (LPARAM)new CMD5(*tag);
	lvi.pszText = const_cast<char*>(text);
	mFiles.InsertItem(&lvi);
}

void CXMGUIStatus::DLText(DLTag tag, const char* text)
{
	//check state
	if (mTabState != XMGUI_DL)
		return;
	
	//find the list item
	DLTag t;
	for (int i=0;i<mFiles.GetItemCount();i++)
	{
		t = (DLTag)mFiles.GetItemData(i);
		if (t->IsEqual(*tag))
		{
			mFiles.SetItemText(i, 0, text);
		}
	}
}

void CXMGUIStatus::DLRemove(DLTag tag)
{
	//check state
	if (mTabState != XMGUI_DL)
		return;

	//find the list item
	cm()->Lock();
	DLTag t;
	for (int i=0;i<mFiles.GetItemCount();i++)
	{
		t = (DLTag)mFiles.GetItemData(i);
		if (t->IsEqual(*tag))
		{
			mFiles.DeleteItem(i);
			break;
		}
	}
	cm()->Unlock();
}

// ---------------------------------------------------------------------------------- TIMER

void CXMGUIStatus::OnTimer(UINT nIDEvent)
{
	//track the last values, to see if we should redraw
	static bool 
		tx2 = false,
		rx2 = false;

	//cwnd may be interested
	CWnd::OnTimer(nIDEvent);

	//only concerned about this event
	if (nIDEvent==3256)
	{
		//get new values
		bool tx, rx;
		CXMSession::TxRxReset(tx, rx);

		//set the new values
		if (tx != tx2)
		{
			mLightTx.mLit = tx;
			mLightTx.RedrawWindow();
			tx2 = tx;
		}
		if (rx != rx2)
		{
			mLightRx.mLit = rx;
			mLightRx.RedrawWindow();
			rx2 = rx;
		}
	}
}