// XMGUIBROWSING.CPP ------------------------------------------------- XMGUIBROWSING.CPP

#include "stdafx.h"
#include "xmclient.h"
#include "xmnet.h"
#include "xmpipeline.h"
#include "xmgui.h"

// ---------------------------------------------------------------------- Image Viewer

BEGIN_MESSAGE_MAP(CImageViewer, CWnd)

	//windowing
	ON_WM_CREATE()
	ON_WM_PAINT()
	ON_WM_SIZE()

	//mouse activity
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_RBUTTONDOWN()
	ON_WM_CANCELMODE()
	ON_WM_SETCURSOR()
	ON_WM_MOUSEMOVE()
	ON_WM_MOUSEWHEEL()
	ON_WM_HSCROLL()
	ON_WM_VSCROLL()

	//commands
	ON_WM_CONTEXTMENU()
	ON_COMMAND(ID_ZOOMIN, OnZoomIn)
	ON_COMMAND(ID_ZOOMOUT, OnZoomOut)
	//ON_COMMAND(ID_FULLSCREEN, OnFullScreen)

END_MESSAGE_MAP()

// --------------------------------------------------------- Public Interface

void CImageViewer::ShowFile(char *pfile)
{
	CJPEGDecoder jpeg;
	CDIBSection dib;
	CBitmap *bmp;

	//decode bitmap
	try
	{
		jpeg.MakeBmpFromFile(pfile, &dib, 32);
	}
	catch(...)
	{
		return;
	}

	//convert to mfc bitmap
	bmp = new CBitmap();
	bmp->Attach(dib.GetHandle());
	dib.Detach();
	ShowBitmap(bmp);
}

void CImageViewer::ShowBitmap(CBitmap *pbmp)
{	
	//free current bmp?
	if (bmp)
		delete bmp;

	//show this bitmap now
	bmp = pbmp;
	zoom = 2;		//1:1
	if (bmp)
	{
		//set the origin
		BITMAP b;
		bmp->GetBitmap(&b);
		origin.x = b.bmWidth/2;
		origin.y = b.bmHeight/2;
	}
	else
	{
		origin.x = 0;
		origin.y = 0;
	}

	//redraw the window
	RecalcViewport();
	UpdateScrollBars();
	RedrawWindow();
}

// --------------------------------------------------------- Windowing

CImageViewer::CImageViewer()
{
	mIsTracking = false;
	bmp = NULL;
	zoom = 1;
}

CImageViewer::~CImageViewer()
{
	if (bmp)
		delete bmp;
}

BOOL CImageViewer::PreCreateWindow(CREATESTRUCT &cs)
{
	//let cwnd setup the cs
	if (!CWnd::PreCreateWindow(cs))
		return FALSE;

	//set the styles we want
	cs.style |= WS_CHILD|WS_VISIBLE|WS_HSCROLL|WS_VSCROLL;
	cs.dwExStyle |= WS_EX_CLIENTEDGE;
	return TRUE;
}

int CImageViewer::OnCreate(LPCREATESTRUCT lpcs)
{
	//let cwnd create
	if (CWnd::OnCreate(lpcs)!=0)
		return -1;

	//show no bitmap
	ShowBitmap(NULL);

	//success
	return 0;
}

void CImageViewer::OnSize(UINT nType, int cx, int cy)
{
	//update the viewport, display
	RecalcViewport();
	UpdateScrollBars();
	RedrawWindow();
}

// --------------------------------------------------------- UI

void CImageViewer::OnPaint()
{
	//redraw a part of the bitmap
	CPaintDC dc(this);

	//do we have a bitmap?
	if (!bmp)
	{
		//just fill in with 3dface
		dc.FillSolidRect(&dc.m_ps.rcPaint, GetSysColor(COLOR_3DFACE));
		return;
	}

	//get a dc for the bitmap
	CDC dcBmp;
	if (!dcBmp.CreateCompatibleDC(&dc))
		return;
	dcBmp.SelectObject(bmp);

	//how should we stretch the image?
	if (zoom<2)
	{
		//zoomed out.. need to use halftone, or
		//lots of color data is lost
		dc.SetStretchBltMode(HALFTONE);
	}
	else
	{
		//1:1 or zoomed in.. halftone looks great,
		//but kind of slow
		dc.SetStretchBltMode(COLORONCOLOR);
	}


	//assume accurate viewport
	//TODO: only draw the requested area
	CRect cr;
	GetClientRect(cr);
	if (borderLeft>0)
	{
		dc.FillSolidRect(0, 0, borderLeft, cr.Height(), RGB(0,0,0));
		dc.FillSolidRect(cr.right-borderLeft, 0, borderLeft, cr.Height(), RGB(0,0,0));
	}
	if (borderTop>0)
	{
		dc.FillSolidRect(0, 0, cr.Width(), borderTop, RGB(0,0,0));
		dc.FillSolidRect(0, cr.Height()-borderTop, cr.Width(), borderTop, RGB(0,0,0));
	}
	dc.StretchBlt(
			cr.left, cr.top, cr.Width(), cr.Height(), 
			&dcBmp, viewport.left, viewport.top, viewport.Width(), viewport.Height(),
			SRCCOPY);

	//draw focus rectangle?
	/*
	if (GetFocus()==this)
	{
		cr.DeflateRect(2,2,2,2);
		dc.DrawFocusRect(cr);
	}
	*/
}

//ZOOM FACTORS
float zoomFactor[] = {.25,.5,1,2,3,4,6,8};
#define MINZOOM 0
#define MAXZOOM 7

void CImageViewer::RecalcViewport()
{
	// input:	* source bitmap dimensions
	//			* zoom level
	//			* window client area
	//			* origin
	//
	// using input, update the the viewport member
	// with a value representing the rectangle from
	// the source to blt into the window. then, adjust
	// the origin, so that the entire viewport is within
	// the source bitmap

	//get our client area dimensions, this is the base viewports
	CRect cr;
	GetClientRect(cr);
	viewport = cr;

	//get bitmap dimensions
	BITMAP b;
	if (bmp)
		bmp->GetBitmap(&b);
	else
	{
		b.bmWidth = 0;
		b.bmHeight = 0;
	}
	CRect rectBitmap(0,0,b.bmWidth, b.bmHeight);

	//scale the client area rect to the zoom size
	viewport.right = (long)(viewport.right/zoomFactor[zoom]);
	viewport.bottom = (long)(viewport.bottom/zoomFactor[zoom]);

	//clip this rect to the bitmap size
	if (viewport.right>rectBitmap.right)
	{
		//center the viewport
		borderLeft = 1+(int)(cr.Width() - rectBitmap.Width()*zoomFactor[zoom]) / 2;
		origin.x = rectBitmap.Width()/2;
		viewport.OffsetRect(-1*(viewport.Width()-rectBitmap.Width())/2, 0);
	
	}
	else
	{
		//no border
		borderLeft = 0;

		//check, adjust the origin
		if(origin.x-(viewport.right/2)<0)
			origin.x = viewport.right/2;
		if(origin.x+(viewport.right/2)>rectBitmap.right)
			origin.x = rectBitmap.right-(viewport.right/2);
	
		//position the viewport using origin
		viewport.OffsetRect(origin.x-(viewport.right/2), 0);
	}

	if (viewport.bottom>rectBitmap.bottom)
	{
		//center the viewport
		borderTop = 1+(int)(cr.Height() - rectBitmap.Height()*zoomFactor[zoom]) / 2;
		origin.y = rectBitmap.Height()/2;
		viewport.OffsetRect(0, -1*(viewport.Height()-rectBitmap.Height())/2);
	}
	else
	{
		//no border
		borderTop = 0;

		//check, adjust the origin
		if(origin.y-(viewport.bottom/2)<0)
			origin.y = viewport.bottom/2;
		if(origin.y+(viewport.bottom/2)>rectBitmap.bottom)
			origin.y = rectBitmap.bottom-(viewport.bottom/2);

		//position the viewport using origin
		viewport.OffsetRect(0, origin.y-(viewport.bottom/2));
	}

	
	//done
}

void CImageViewer::UpdateScrollBars()
{
	//recalculate the scroll bar settings, assume accurate radius
	SCROLLINFO si;
	si.cbSize = sizeof(si);
	si.fMask = SIF_DISABLENOSCROLL|SIF_PAGE|SIF_POS|SIF_RANGE;

	//bitmap?
	if (!bmp)
	{
		si.nMin = 0;
		si.nMax = 0;
		si.nPage = 0;
		si.nPos = 0;
		SetScrollInfo(SB_HORZ, &si, TRUE);
		SetScrollInfo(SB_VERT, &si, TRUE);
		return;
	}
	BITMAP b;
	bmp->GetBitmap(&b);

	//horizontal scroll bar
	si.nMin = 0;
	si.nMax = b.bmWidth;// - viewport.Width();
	si.nPage = viewport.Width();
	si.nPos = origin.x - viewport.Width()/2;
	SetScrollInfo(SB_HORZ, &si, TRUE);

	//vertical scroll bar
	si.nMin = 0;
	si.nMax = b.bmHeight;// - viewport.Height();
	si.nPage = viewport.Height();
	si.nPos = origin.y - viewport.Height()/2;
	SetScrollInfo(SB_VERT, &si, TRUE);
}

// --------------------------------------------------------- Panning

void CImageViewer::OnLButtonDown(UINT nFlags, CPoint pt)
{
	//start tracking
	mIsTracking = true;
	mLastTrackPoint = pt;

	//capture mouse
	SetCapture();
	SetCursor(AfxGetApp()->LoadCursor(IDC_HANDCLOSED));
}

void CImageViewer::OnLButtonUp(UINT nFlags, CPoint pt)
{
	//stop tracking
	mIsTracking = false;
	ReleaseCapture();
	SetCursor(AfxGetApp()->LoadCursor(IDC_HANDOPEN));
}

void CImageViewer::OnRButtonDown(UINT nFlags, CPoint point)
{
	//stop tracking
	mIsTracking = false;
	ReleaseCapture();
	SetCursor(AfxGetApp()->LoadCursor(IDC_HANDOPEN));
}

void CImageViewer::OnCancelMode()
{
	//stop tracking
	mIsTracking = false;
	SetCursor(AfxGetApp()->LoadCursor(IDC_HANDOPEN));
}

void CImageViewer::OnMouseMove(UINT nFlags, CPoint pt)
{
	//give us the focus
	SetFocus();

	//move the origin, save new point
	if (mIsTracking)
	{
		CPoint offset(mLastTrackPoint);
		offset -= pt;
		offset.x = (long)(offset.x/zoomFactor[zoom]);
		offset.y = (long)(offset.y/zoomFactor[zoom]);
		origin.Offset(offset);
		mLastTrackPoint = pt;

		//update display
		RecalcViewport();
		UpdateScrollBars();
		RedrawWindow();
	}
}

BOOL CImageViewer::OnSetCursor(CWnd *pWnd, UINT nHitTest, UINT message)
{
	//client or nc?
	if (nHitTest!=HTCLIENT)
	{
		return CWnd::OnSetCursor(pWnd, nHitTest, message);
	}

	//are we tracking?
	if (mIsTracking)
	{
		SetCursor(AfxGetApp()->LoadCursor(IDC_HANDCLOSED));
	}
	else
	{
		SetCursor(AfxGetApp()->LoadCursor(IDC_HANDOPEN));
	}
	return TRUE;
}

void CImageViewer::OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar)
{
	//set the origin, recalc, redraw
	BITMAP b;
	switch(nSBCode)
	{
	//user moving scroll box
	case SB_THUMBPOSITION:
	case SB_THUMBTRACK:
		origin.x = nPos + (viewport.Width()/2);
		break;

	//handle single clicks
	case SB_LINEUP:
		origin.x--;
		break;
	case SB_LINEDOWN:
		origin.x++;
		break;
	case SB_PAGEUP:
		origin.x -= viewport.Width();
		break;
	case SB_PAGEDOWN:
		origin.x += viewport.Width();
		break;

	//scroll bar was moved to extent
	case SB_TOP:
		origin.x = viewport.Width()/2;
		break;
	case SB_BOTTOM:
		if (bmp)
		{
			bmp->GetBitmap(&b);
			origin.x = b.bmWidth - (viewport.Width()/2);
		}
		break;
	}
	
	//update the scroll bar
	if(nSBCode!=SB_THUMBTRACK)
		SetScrollPos(SB_HORZ, origin.x-viewport.Width()/2);

	//redraw the window
	RecalcViewport();
	RedrawWindow();
}

void CImageViewer::OnVScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar)
{
	//set the origin, recalc, redraw
	BITMAP b;
	switch(nSBCode)
	{
	//user moving scroll box
	case SB_THUMBPOSITION:
	case SB_THUMBTRACK:
		origin.y = nPos + (viewport.Height()/2);
		break;

	//handle single clicks
	case SB_LINEUP:
		origin.y--;
		break;
	case SB_LINEDOWN:
		origin.y++;
		break;
	case SB_PAGEUP:
		origin.y -= viewport.Height();
		break;
	case SB_PAGEDOWN:
		origin.y += viewport.Height();
		break;

	//scroll bar was moved to extent
	case SB_TOP:
		origin.y = viewport.Height()/2;
		break;
	case SB_BOTTOM:
		if (bmp)
		{
			bmp->GetBitmap(&b);
			origin.y = b.bmHeight - (viewport.Height()/2);
		}
		break;
	}
	
	//update the scroll bar
	if(nSBCode!=SB_THUMBTRACK)
		SetScrollPos(SB_VERT, origin.y-viewport.Height()/2);

	//redraw the window
	RecalcViewport();
	RedrawWindow();
}

// --------------------------------------------------------- Zooming

void CImageViewer::OnContextMenu(CWnd* pWnd, CPoint pos)
{
	//show the menu
	CMenu menu;
	menu.CreatePopupMenu();
	menu.AppendMenu(MF_POPUP, (UINT)LoadMenu(AfxGetResourceHandle(), (LPCSTR)IDR_IMAGE));
	menu.GetSubMenu(0)->TrackPopupMenu(TPM_LEFTALIGN|TPM_TOPALIGN, pos.x, pos.y, this, 0);
}

void CImageViewer::OnFullScreen()
{
	//context menu command
}

void CImageViewer::OnZoomIn()
{
	//context menu command
	Zoom(true);
}

void CImageViewer::OnZoomOut()
{
	//context menu command
	Zoom(false);
}


BOOL CImageViewer::OnMouseWheel(UINT nFlags, short zDelta, CPoint pt)
{
	//up or down?
	if (zDelta > 0)
	while (zDelta >= WHEEL_DELTA)
	{
		zDelta -= WHEEL_DELTA;
		Zoom(true);
	}
	else
	while (zDelta < 0)
	{
		zDelta += WHEEL_DELTA;
		Zoom(false);
	}
	return 0;
}

void CImageViewer::Zoom(bool in)
{
	//modify the zoom level
	zoom += in?1:-1;
	if (zoom<MINZOOM) zoom = MINZOOM;
	if (zoom>MAXZOOM) zoom = MAXZOOM;

	//update the display
	RecalcViewport();
	UpdateScrollBars();
	RedrawWindow();
}


// ---------------------------------------------------------------------- File Browse Tag

struct XMFileBrowseTag
{
	//just the name!
	XMFileBrowseTag(CString newName, CString newPath)
	{
		mName = newName;
		mPath = newPath;
		mdwWorkItemId = 0xFFFFFFFF;
		mStillExists = true;
		mShared = false;
	}
	CString mName;
	CString mPath;
	DWORD mdwWorkItemId;
	bool mStillExists;
	bool mShared;
	CMD5 mMD5;
};

// ---------------------------------------------------------------------- File Browser

BEGIN_MESSAGE_MAP(CFileBrowser, CListCtrl)

	//windowing
	ON_WM_CREATE()
	ON_WM_DESTROY()
	ON_WM_TIMER()
	ON_WM_CONTEXTMENU()

	//listctrl notifications
	ON_NOTIFY_REFLECT(LVN_DELETEITEM, OnFilesDeleteItem)
	ON_NOTIFY_REFLECT(LVN_GETDISPINFO, OnFilesGetDispInfo)

	//asycn resizing
	ON_MESSAGE(XM_ASYNCRESIZE, OnAsyncResize)

	//context menu functions
	ON_COMMAND(ID_SHAREFILE, OnShareFile)
	ON_COMMAND(ID_UNSHAREFILE, OnUnshareFile)
	ON_COMMAND(ID_INDEXFILE, OnIndexFile)

END_MESSAGE_MAP()

CFileBrowser::CFileBrowser()
{
	mWaitHandle = NULL;
}

CFileBrowser::~CFileBrowser()
{
	//close the file monitoring handle
	if (mWaitHandle)
	{
		FindCloseChangeNotification(mWaitHandle);
		mWaitHandle = NULL;
	}
}

BOOL CFileBrowser::PreCreateWindow(CREATESTRUCT &cs)
{
	//call base class
	if (!CListCtrl::PreCreateWindow(cs))
		return FALSE;

	//setup styles
	cs.dwExStyle |= WS_EX_CLIENTEDGE;
	cs.style |= WS_CHILD|WS_VISIBLE|LVS_AUTOARRANGE;

	return TRUE;
}

int CFileBrowser::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	//let listctrl have first shot
	if (CListCtrl::OnCreate(lpCreateStruct)!=0)
		return -1;

	//create the imagelist
	if (!mFilesImages.Create(XMGUI_THUMBWIDTH, XMGUI_THUMBHEIGHT, ILC_COLOR24, 0, 10)) {
		return false;
	}
	CBitmap bmp, bmp2;
	bmp.LoadBitmap(IDB_THUMBNAIL_WAITING);
	bmp2.LoadBitmap(IDB_THUMBNAIL_ERROR);
	mFilesImages.Add(&bmp, (CBitmap*)NULL);
	mFilesImages.Add(&bmp2, (CBitmap*)NULL);

	//assign to us
	SetImageList(&mFilesImages, LVSIL_NORMAL);

	//start timer.. we check for changed files
	//every 5 seconds
	SetTimer(1, 5*1000, NULL);

	//success
	return 0;
}

void CFileBrowser::OnDestroy()
{
	//pass to list control
	CListCtrl::OnDestroy();

	//stop the timer
	KillTimer(1);
}

void CFileBrowser::OnContextMenu(CWnd* pWnd, CPoint pos)
{
	//create context menu
	CMenu menu;
	menu.CreatePopupMenu();
	menu.AppendMenu(MF_POPUP, (UINT)LoadMenu(AfxGetResourceHandle(), (LPCSTR)IDR_LOCAL));
	
	//which menu?
	int i = GetNextItem(-1, LVNI_SELECTED);
	if (i == -1)
		return;
	XMFileBrowseTag *ptag = (XMFileBrowseTag*)GetItemData(i);	
	i = ptag->mShared?0:1;

	//show menu
	menu.GetSubMenu(0)->GetSubMenu(i)->TrackPopupMenu(TPM_LEFTALIGN|TPM_TOPALIGN, pos.x, pos.y, this, 0);
}

void CFileBrowser::OnIndexFile()
{
	//get first selected item
	int i = GetNextItem(-1, LVNI_SELECTED);
	if (i == -1)
		return;
	XMFileBrowseTag *tag = (XMFileBrowseTag*)GetItemData(i);
	if (!tag->mShared)
		return;

	//get file
	db()->Lock();
	CXMDBFile *file = db()->FindFile(tag->mMD5.GetValue());
	db()->Unlock();
	if (file)
	{
		QueryBuildIndex(file);
	}

	//HACK: db should stay locked the whole time, but that would cause lots
	//of other threads to block while the window is up.
}

void CFileBrowser::OnShareFile()
{
	//enum selected files
	XMFileBrowseTag *tag;
	CXMDBFile* file;
	int i = GetNextItem(-1, LVNI_SELECTED);
	while (i != -1)
	{
		//unshare the file
		tag = (XMFileBrowseTag*)GetItemData(i);
		if (!tag->mShared)
		{
			file = db()->AddFile(tag->mPath);
			if (file)
			{
				CString str;
				str.Format("%s - Shared", tag->mName);
				SetItemText(i, 0, str);
				tag->mShared = true;
				tag->mMD5 = file->GetMD5();	
			}
		}

		//next selected item
		i = GetNextItem(i, LVNI_SELECTED);
	}
}

void CFileBrowser::OnUnshareFile()
{
	//enum selected files
	XMFileBrowseTag *tag;
	CXMDBFile* file;
	int i = GetNextItem(-1, LVNI_SELECTED);
	while (i != -1)
	{
		//unshare the file
		tag = (XMFileBrowseTag*)GetItemData(i);
		if (tag->mShared)
		{
			file = db()->FindFile(tag->mMD5.GetValue());
			file->SetFlag(DFF_REMOVED, true);

			CString str;
			str.Format("%s - Not Shared", tag->mName);
			SetItemText(i, 0, str);
			tag->mShared = false;
		}

		//next selected item
		i = GetNextItem(i, LVNI_SELECTED);
	}
}

const char* CFileBrowser::GetSelectedPath()
{
	//return the path
	int i = GetNextItem(-1, LVNI_SELECTED);
	if (i<0)
		return NULL;
	XMFileBrowseTag *ptag = (XMFileBrowseTag*)GetItemData(i);
	if (!ptag)
		return NULL;

	//NOTE: don't store pointer without copying it!
	return ptag->mPath;
}

CBitmap* CFileBrowser::GetSelectedBitmap()
{
	//decode the selected item's jpeg file
	int i = GetNextItem(-1, LVNI_SELECTED);
	if (i<0)
		return NULL;
	XMFileBrowseTag *ptag = (XMFileBrowseTag*)GetItemData(i);
	if (!ptag)
		return NULL;

	//decode
	CJPEGDecoder jpeg;
	CDIBSection dib;
	CBitmap *pbmp = new CBitmap();
	try {
		jpeg.MakeBmpFromFile(ptag->mPath, &dib, 32);
		pbmp->Attach(dib.GetHandle());
		dib.Detach();
	}
	catch(...)
	{
		pbmp->LoadBitmap(IDB_THUMBNAIL_ERROR);
	}
	return pbmp;
}

CBitmap* CFileBrowser::GetSelectedThumbnail()
{
	//extract the thumbnail for the selected item
	int i = GetNextItem(-1, LVNI_SELECTED);
	if (i<0)
		return NULL;

	CBitmap *pbmp = new CBitmap();
	IMAGEINFO ii;
	mFilesImages.GetImageInfo(i, &ii);
	pbmp->Attach(ii.hbmImage);
	return pbmp;
}

void CFileBrowser::OnFilesDeleteItem(NMHDR* pnmh, LRESULT* pResult)
{
	//just call delete against the lparam
	LPNMLISTVIEW pnmlv = (LPNMLISTVIEW)pnmh;
	XMFileBrowseTag *ptag = (XMFileBrowseTag*)pnmlv->lParam;
	if (ptag->mdwWorkItemId!=0xFFFFFFFF)
		ar()->CancelItem(ptag->mdwWorkItemId);
	delete ptag;
}

LRESULT CFileBrowser::OnAsyncResize(WPARAM wParam, LPARAM lParam)
{
	//an async resize was completed
	DWORD id = (DWORD)lParam;

	//get the item
	ar()->Lock();
	CXMAsyncResizer::CompletedItem *pci = ar()->GetCompletedItem(id);
	if (!pci)
	{
		ar()->Unlock();
		return 1;
	}

	//add the image the imagelist
	int i = 0;
	CBitmap bmp;
	if (pci->mpDib)
	{
		bmp.Attach(pci->mpDib->GetHandle());
		pci->mpDib->Detach();
		i = mFilesImages.Add(&bmp, (CBitmap*)NULL);
	}
	else
	{
		//error resizing
		i = 1;
	}

	//we are now done with the resizer
	ar()->RemoveCompletedItem(id);
	ar()->Unlock();

	//update the listview
	LVITEMA lvi;
	XMFileBrowseTag *ptag;
	for (int j=0;j<GetItemCount();j++)
	{
		//is this the item?
		ptag = (XMFileBrowseTag*)GetItemData(j);
		if (ptag->mdwWorkItemId == id)
		{
			//update
			lvi.mask = LVIF_IMAGE;
			lvi.iItem = j;
			lvi.iSubItem = 0;
			lvi.iImage = i;
			SetItem(&lvi);
			return 1;
		}
	}
	return 1;
}

void CFileBrowser::OnFilesGetDispInfo(NMHDR* pnmh, LRESULT* pResult)
{
	//get disp info, tag
	LPNMLVDISPINFOA pdi = (LPNMLVDISPINFOA)pnmh;
	LPLVITEMA plvi = &pdi->item;
	XMFileBrowseTag *ptag = (XMFileBrowseTag*)plvi->lParam;

	//if we have a work item already, ignore this
	if (ptag->mdwWorkItemId!=0xFFFFFFFF)
		return;

	//search files in db for this path
	db()->Lock();
	CXMDBFile *pfile = NULL;
	CXMDBThumb *pthumb = NULL;
	CJPEGDecoder jpeg;
	CBitmap bmp;
	CDIBSection dib;
	BYTE* buf = NULL;
	SIZE_T bufcount;
	static char text[MAX_PATH];

	pfile = db()->FindFile(ptag->mPath);
	if (pfile)
	{
		if (!pfile->GetFlag(DFF_REMOVED))
		{
			//decode the image
			if (plvi->mask&LVIF_IMAGE)
			{
				pthumb = pfile->GetThumb(XMGUI_THUMBWIDTH, XMGUI_THUMBHEIGHT);
				if (!pthumb)
				{
					db()->Unlock();
					return;
				}
				bufcount = pthumb->GetImage(&buf);
				jpeg.MakeBmpFromMemory(buf, bufcount, &dib);
				bmp.Attach(dib.GetHandle());
				dib.Detach();
				
				//add to imagelist
				plvi->iImage = mFilesImages.Add(&bmp, (CBitmap*)NULL);
				bmp.DeleteObject();
				pthumb->FreeImage(&buf);
			}

			//set the text
			if (plvi->mask&LVIF_TEXT)
			{
				sprintf(text, "%s - Shared", ptag->mName);
				plvi->pszText = text;
			}

			//success
			ptag->mShared = true;
			ptag->mMD5 = pfile->GetMD5();
			plvi->mask |= LVIF_DI_SETITEM;
			db()->Unlock();
			return;
		}
	}
	db()->Unlock();

	//start async resize
	if (plvi->mask & LVIF_IMAGE)
	{
		ar()->Lock();
		CXMAsyncResizer::WorkItem* pwi = ar()->QueueItem();
		strcpy(pwi->szPath, ptag->mPath);
		pwi->hWnd = m_hWnd;
		pwi->uiOp |= XMAROP_DIB;
		pwi->dwWidth = XMGUI_THUMBWIDTH;
		pwi->dwHeight = XMGUI_THUMBHEIGHT;
		ar()->FlushQueue();

		//store the work id
		ptag->mdwWorkItemId = pwi->dwId;
		ar()->Unlock();

		//setup the image to "waiting"
		plvi->iImage = 0;
	}

	//text
	if (plvi->mask & LVIF_TEXT)
	{
		sprintf(text, "%s - Not Shared", ptag->mName);
		plvi->pszText = text;
	}

	//success
	ptag->mShared = false;
	plvi->mask |= LVIF_DI_SETITEM;
}

void CFileBrowser::ClearFileList()
{
	//remove all items from the listview.. they will
	//free their own lParams
	DeleteAllItems();

	//clear the image list
	while(mFilesImages.GetImageCount()>2)
		mFilesImages.Remove(2);
}

void CFileBrowser::BrowseFolder(char* path)
{
	//use the default path?
	ASSERT(path);

	//add *.* to the end of the path
	CString fullPath(path);
	if (fullPath.Right(1)!="\\")
		fullPath += "\\";

	//is this the same as our current folder?
	if (fullPath!=mPath)
	{
		//empty the list
		ClearFileList();
	}

	//call the update function
	mPath = fullPath;
	UpdateBrowser();

	//release current wait handle
	if (mWaitHandle)
		FindCloseChangeNotification(mWaitHandle);

	//get new wait handle
	mWaitHandle = FindFirstChangeNotification(
						mPath,
						FALSE,
						FILE_NOTIFY_CHANGE_FILE_NAME);
}

void CFileBrowser::UpdateBrowser()
{
	CString filepath;
	int x;
	DWORD i=0;
	CFileFind find;
	XMFileBrowseTag *ptag;
	bool found;

	//turn off the "still exists" flag in all
	//existing items
	for (x=0;x<GetItemCount();x++)
	{
		ptag = (XMFileBrowseTag*)GetItemData(x);
		ptag->mStillExists = false;
	}

	//fill out a generic lvitem
	LVITEMA lvi;
	lvi.mask = LVIF_IMAGE|LVIF_PARAM|LVIF_TEXT;
	lvi.iImage = I_IMAGECALLBACK;
	lvi.pszText = LPSTR_TEXTCALLBACK;
	lvi.iSubItem = 0;

	//enumerate the list of files in the folder
	if (!find.FindFile(mPath+"*.*", 0))
		return;
	BOOL cont = find.FindNextFile();
	BOOL islast = FALSE;
	while (cont)
	{
		//if this is the last one, we don't continue
		if (islast)
			cont = FALSE;

		//is this a directory?
		if (!find.IsDirectory())
		{
			//is it a jpeg?
			filepath = find.GetFileName();
			x = filepath.ReverseFind('.');
			if (x!=-1)
			{
				//compare the extension
				if (filepath.Mid(x+1)=="jpg" ||
					filepath.Mid(x+1)=="jpeg")
				{
					//try to find this file
					found = false;
					for(x=0;x<GetItemCount();x++)
					{
						ptag = (XMFileBrowseTag*)GetItemData(x);
						if (!ptag->mStillExists)
						{
							if (ptag->mName==filepath)
							{
								found = true;
								ptag->mStillExists = true;
								break;
							}
						}
					}

					//add the file, the image fill be fetched
					//when the listitem is actually displayed
					if (!found)
					{
						lvi.lParam = (LPARAM)new XMFileBrowseTag(filepath, find.GetFilePath());
						lvi.iItem = GetItemCount();
						InsertItem(&lvi);
					}
				}
			}

			//clean out the message queue
			MSG msg;
			if ((++i%4)==0)
			{
				while (PeekMessage(&msg, NULL, NULL, NULL, PM_NOREMOVE))
					if (!AfxGetApp()->PumpMessage())
						AfxPostQuitMessage(0);
			}
			if (i%128==1)	//all items less than 128 get redrawn immediatly..
			{
				//only redraw the list every 128 items
				SetRedraw(TRUE);
				RedrawWindow();
				SetRedraw(FALSE);
			}
		}

		//next item
		if (cont)
			islast = !find.FindNextFile();
	}
	SetRedraw(TRUE);

	//any items that did not get their "still exists" flag
	//turned back on have been deleted
	for (x=GetItemCount()-1;x>=0;x--)
	{
		ptag = (XMFileBrowseTag*)GetItemData(x);
		if (!ptag->mStillExists)
		{	
			DeleteItem(x);
		}
	}
}

void CFileBrowser::BrowseFolder(LPITEMIDLIST pidl)
{
	//translate the pidle to a path
	char path[MAX_PATH];
	if (!SHGetPathFromIDList(pidl, path))
		return;

	//call browsefolder witht he path
	BrowseFolder(path);
}

void CFileBrowser::OnTimer(UINT nID)
{
	//check the wait handle
	if (nID!=1)
		return;
	if (WaitForSingleObject(mWaitHandle, 0)==
		WAIT_OBJECT_0)
	{
		//update the display
		UpdateBrowser();

		//reset the handle
		if (!FindNextChangeNotification(mWaitHandle))
			mWaitHandle = NULL;
	}
}