// XMGUILOCAL.CPP ------------------------------------------------------- XMGUILOCAL.CPP

#define XMGUILV_EDITTOP		4
#define XMGUILV_EDITHEIGHT	18
#define XMGUILV_FILESTOP	22

#include "stdafx.h"
#include "xmclient.h"
#include "xmnet.h"
#include "xmpipeline.h"
#include "xmgui.h"

// --------------------------------------------------------------------------- Local View

char CLocalView::mCurPath[MAX_PATH];

//message map
BEGIN_MESSAGE_MAP(CLocalView, CWnd)

	//windowing
	ON_WM_DESTROY()
	ON_WM_PAINT()

	//sizing
	ON_WM_SIZE()
	ON_MESSAGE(XM_SPLITMOVE, OnSplitterMove)

	//listview notifys
	ON_WM_CONTEXTMENU()
	ON_NOTIFY(NM_CLICK, IDC_FILES, OnFilesClick)

END_MESSAGE_MAP()

CLocalView::CLocalView()
{
	m_udRefCount = 1;
}

CLocalView::~CLocalView()
{
}

void CLocalView::AddRef()
{
	//something else is referencing us
	m_udRefCount++;
}

void CLocalView::Release()
{
	//release reference
	m_udRefCount--;
	if (m_udRefCount<1) {
		delete this;
	}
}

void CLocalView::OnPaint()
{
	CPaintDC dc(this);
	dc.FillSolidRect(&dc.m_ps.rcPaint, GetSysColor(COLOR_3DFACE));

	//paint the preview
	//mPreview.RedrawWindow();
}

UINT CLocalView::GetViewType()
{
	//the id of the view
	return XMGUIVIEW_LOCAL | XMGUIVIEW_SAVED;
}



bool CLocalView::Create(CWnd *hwParent, CRect &rect)
{
	//where does the splitter go?
	long lSplitX = config()->GetFieldLong(FIELD_GUI_LOCAL_SPLIT);

	//create this window
	if (!CWnd::Create(
			NULL,
			"CLocalView_Holder",
			WS_CHILD|WS_VISIBLE/*|WS_CLIPCHILDREN*/,
			rect,
			hwParent,
			0,
			NULL)) {
		return false;
	}

	//create path edit label
	if (!mPathLabel.Create(
			ES_READONLY|WS_CHILD|WS_VISIBLE,
			CRect(0,XMGUILV_EDITTOP,lSplitX,XMGUILV_EDITHEIGHT),
			this,
			0)) {
		return false;
	}

	//create files
	if (!mFiles.Create(
			NULL,
			CRect(0,XMGUILV_FILESTOP,lSplitX,rect.Height()),
			this,
			IDC_FILES)) {
		return false;
	}

	//create splitter
	m_nLastSplitterPos = lSplitX;
	if (!mSplitter.Create(
			this,
			0,
			CRect(lSplitX, 0, lSplitX+XMGUI_SPLITWIDTH, rect.Height()),
			XMGUI_VIEWSPLITBORDER,
			XMGUI_VIEWSPLITBORDER,
			GetSysColor(COLOR_3DFACE))) {
		return false;
	}

	//create previe
	if (!mPreview.Create(
			NULL,
			NULL,
			WS_CHILD|WS_VISIBLE,
			CRect(lSplitX+XMGUI_SPLITWIDTH, 0, rect.Width(), rect.Height()),
			this,
			0)) {
		return false;
	}
	CBitmap *pbmp = new CBitmap();
	pbmp->LoadBitmap(MAKEINTRESOURCE(IDB_LOGO_170));
	mPreview.ShowBitmap(pbmp);
	
	//set the font for controls
	CFont font;
	font.CreateStockObject(ANSI_VAR_FONT);
	mPathLabel.SetFont(&font);

	//browse the default folder
	//BrowseFolder((char*)NULL);

	//success
	return true;
}

void CLocalView::OnDestroy()
{
	//save the splitter position
	config()->SetField(FIELD_GUI_LOCAL_SPLIT, m_nLastSplitterPos);
}

void CLocalView::OnSize(UINT nType, int cx, int cy)
{
	//resize each of our controls
	if (mFiles.m_hWnd && mPreview.m_hWnd && mSplitter.m_hWnd) {
		UINT uFlags = SWP_NOZORDER;
		HDWP h = BeginDeferWindowPos(4);
		h = DeferWindowPos(h, mPathLabel.m_hWnd, NULL,
			0, XMGUILV_EDITTOP, m_nLastSplitterPos, XMGUILV_EDITHEIGHT, uFlags);
		h = DeferWindowPos(h, mFiles.m_hWnd, NULL,
			0, XMGUILV_FILESTOP, m_nLastSplitterPos, cy-XMGUILV_FILESTOP, uFlags);
		h = DeferWindowPos(h, mSplitter.m_hWnd, NULL,
			m_nLastSplitterPos, 0, XMGUI_SPLITWIDTH, cy, uFlags);
		h = DeferWindowPos(h, mPreview.m_hWnd, NULL,
			m_nLastSplitterPos+XMGUI_SPLITWIDTH, 0,
			cx-m_nLastSplitterPos-XMGUI_SPLITWIDTH, cy, uFlags);
		EndDeferWindowPos(h);
	}
}

LRESULT CLocalView::OnSplitterMove(WPARAM wParam, LPARAM lParam)
{
	//convert params
	long x = XM_SPLITLEFT(lParam);
	long cx = XM_SPLITRIGHT(lParam);

	//store splitter position
	m_nLastSplitterPos = x;

	//get size of window
	CRect r;
	GetClientRect(r);

	//resize everything
	UINT uFlags = SWP_NOZORDER;
	HDWP h = BeginDeferWindowPos(4);
	h = DeferWindowPos(h, mPathLabel.m_hWnd, NULL,
		0, XMGUILV_EDITTOP, x, XMGUILV_EDITHEIGHT, uFlags);
	h = DeferWindowPos(h, mFiles.m_hWnd, NULL,
		0, XMGUILV_FILESTOP, x, r.Height()-XMGUILV_FILESTOP, uFlags);
	h = DeferWindowPos(h, mSplitter.m_hWnd, NULL,
		x, 0, XMGUI_SPLITWIDTH, r.Height(), uFlags);
	h = DeferWindowPos(h, mPreview.m_hWnd, NULL,
		cx, 0, r.Width()-cx, r.Height(), uFlags);
	EndDeferWindowPos(h);

	//redraw windows
	mFiles.RedrawWindow();
	mSplitter.RedrawWindow();
	mPreview.RedrawWindow();
	return 0;
}

// ---------------------------------------------------------------------- File Browsing

void CLocalView::OnFilesClick(NMHDR* pnmh, LRESULT* pResult)
{
	//read the bitmap of the selected file
	CBitmap* pbmp = mFiles.GetSelectedBitmap();
	if (pbmp)
	{
		mPreview.ShowBitmap(pbmp);
	}
}

void CLocalView::BrowseFolder(char* path, bool save)
{
	//use the default path?
	if (!path) {
		path = mCurPath;
	} else {
		//save this path
		if (save)
			strncpy(mCurPath, path, MAX_PATH);
	}
	
	//set the path label
	mPathLabel.SetWindowText(path);

	//pass control to browser
	if (mFiles.m_hWnd)
		mFiles.BrowseFolder(path);
}

void CLocalView::BrowseFolder(LPITEMIDLIST pidl, bool save)
{
	//translate the pidle to a path
	char path[MAX_PATH];
	if (!SHGetPathFromIDList(pidl, path))
		return;

	//call browsefolder witht he path
	BrowseFolder(path, save);
}
