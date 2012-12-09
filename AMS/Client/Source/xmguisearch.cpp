// XMGUISEARCH.CPP ------------------------------------------------------- XMGUISEARCH.CPP

#include "stdafx.h"
#include "xmclient.h"
#include "xmnet.h"
#include "xmpipeline.h"
#include "xmgui.h"

// ----------------------------------------------------------------------------------- State Stuff

//static data
CXMQuery *CSearchView::mQuery;
CImageList CSearchView::mQueryImages;
CXMGUIQueryItemList CSearchView::mQueryItems;

//imagelist stuff
#define XMGS_DEFIMAGECOUNT		3
#define XMGS_IMAGEWAITING		0
#define XMGS_IMAGEDOWNLOADING	1
#define XMGS_IMAGEERROR			2

//initialize static data
bool CMainFrame::StaticInit()
{
	//create our default query
	CSearchView::mQuery = new CXMQuery();

	//load default images into listview
	CBitmap bmpWaiting, bmpDownloading, bmpError;
	bmpWaiting.LoadBitmap(IDB_THUMBNAIL_WAITING);
	bmpDownloading.LoadBitmap(IDB_THUMBNAIL_DOWNLOADING);
	bmpError.LoadBitmap(IDB_THUMBNAIL_ERROR);
	CSearchView::mQueryImages.Create(XMGUI_THUMBWIDTH, XMGUI_THUMBHEIGHT, ILC_COLOR24, 25, 5);
	CSearchView::mQueryImages.Add(&bmpWaiting, (CBitmap*)NULL);
	CSearchView::mQueryImages.Add(&bmpDownloading, (CBitmap*)NULL);
	CSearchView::mQueryImages.Add(&bmpError, (CBitmap*)NULL);

	//success
	return true;
}

//free any static data
void CMainFrame::StaticClose()
{
	//release the default query
	if (CSearchView::mQuery)
		CSearchView::mQuery->Release();
}

void CMainFrame::ClearQueryItems()
{
	//clear the entire list, and free memory
	CXMGUIQueryItem *pqi;
	POSITION pos = CSearchView::mQueryItems.GetHeadPosition();
	while (pos)
	{
		pqi = CSearchView::mQueryItems.GetNext(pos);
		delete pqi;
	}
	CSearchView::mQueryItems.RemoveAll();

	//empty the imagelist
	while(CSearchView::mQueryImages.GetImageCount()>3)
		CSearchView::mQueryImages.Remove(3);
}

CXMGUIQueryItem* CSearchView::GetQueryItemFromTag(CXMPipelineUpdateTag *tag)
{
	//must pas a good tag
	if (!tag)
		return NULL;

	//look through all the current query item for
	//the one that belongs to tag
	CXMGUIQueryItem *pqi;
	POSITION pos = CSearchView::mQueryItems.GetHeadPosition();
	while (pos)
	{
		pqi = CSearchView::mQueryItems.GetNext(pos);
		if (pqi->mQueryResponseItem)
		{
			if (pqi->mQueryResponseItem->mMD5.IsEqual(tag->md5))
			{
				return pqi;
			}
		}
	}
	return NULL;
}

LRESULT CMainFrame::OnClientMessage(WPARAM wParam, LPARAM lParam)
{
	//image decoding vars
	CBitmap bmp;
	CDIBSection dib;
	CJPEGDecoder jpeg;

	//misc
	CXMClientManager::CompletedFile *pcf;
	DWORD dwCompletedFile;

	//get our tag
	CXMPipelineUpdateTag *tag = (CXMPipelineUpdateTag*)lParam;
	CXMGUIQueryItem *pqi = CSearchView::GetQueryItemFromTag(tag);

	//what message?
	switch (wParam)
	{
	case XM_CMU_QUEUE_ADD:
		
		//get the query item
		if (pqi)
		{
			if (tag->thumb) 
			{
				//set the image and state
				pqi->mImage = XMGS_IMAGEWAITING;
				pqi->mState = XMGUISIS_THUMBQUEUED;
			}
			else
			{
				//set the state
				pqi->mState = XMGUISIS_FILEQUEUED;
			}
		}
		break;

	case XM_CMU_QUEUE_REMOVE:
		break;

	case XM_CMU_DOWNLOAD_START:

		//update the image and state
		if (pqi)
		{
			if (tag->thumb)
			{
				pqi->mImage = XMGS_IMAGEDOWNLOADING;
				pqi->mState = XMGUISIS_THUMBDOWNLOADING;
			}
			else
			{
				pqi->mState = XMGUISIS_FILEDOWNLOADING;
			}
		}
		break;

	case XM_CMU_DOWNLOAD_REQUESTING:
	case XM_CMU_DOWNLOAD_RECEIVING:
	case XM_CMU_DOWNLOAD_FINISH:
		break;

	case XM_CMU_DOWNLOAD_ERROR:
	case XM_CMU_DOWNLOAD_CANCEL:
		
		//show the error
		if (pqi)
		{
			pqi->mImage = XMGS_IMAGEERROR;
			pqi->mState = XMGUISIS_ERROR;
		}
		break;

	case XM_CMU_COMPLETED_ADD:
		
		//picture finished
		if (pqi)
		{
			if (tag->thumb)
			{
				//decode image
				cm()->Lock();
				dwCompletedFile = cm()->FindCompletedFile(tag);
				pcf = cm()->GetCompletedFile(dwCompletedFile);
				jpeg.MakeBmpFromMemory(pcf->mBuffer, pcf->mBufferSize, &dib, 0);
				bmp.Attach(dib.GetHandle());
				dib.Detach();

				//cache the jpeg data
				pqi->mThumbMD5.FromBuf(pcf->mBuffer, pcf->mBufferSize);
				dbman()->CacheFile(pqi->mThumbMD5, pcf->mMD5, pcf->mBuffer, pcf->mBufferSize, TRUE);
				pcf->mBuffer = NULL;
				pcf->mBufferSize = 0;
				cm()->RemoveCompletedFile(dwCompletedFile);

				//update state
				pqi->mImage = CSearchView::mQueryImages.Add(&bmp, (CBitmap*)NULL);
				pqi->mState = XMGUISIS_THUMBCOMPLETE;
				cm()->Unlock();
			}
			else
			{
				//update state
				pqi->mState = XMGUISIS_FILECOMPLETE;
			}
		}
		else
		{
		}
		break;

	case XM_CMU_COMPLETED_REMOVE:
		break;
	}
	tag->Release();
	return 0;
}

LRESULT CMainFrame::OnServerMessage(WPARAM wParam, LPARAM lParam)
{
	POSITION pos;
	CXMQueryResponse *r;
	CXMGUIQueryItem *pqi;

	switch (wParam)
	{
	case XM_SMU_QUERY_SENT:
	case XM_SMU_QUERY_BEGIN:
		break;

	case XM_SMU_QUERY_FINISH:

		//refresh the CSearhView::mQueryItems collection
		r = sm()->QueryGetResponse();
		ClearQueryItems();
		pos = r->mFiles.GetHeadPosition();
		while (pos)
		{
			//fill out the query item
			pqi = new CXMGUIQueryItem();
			pqi->mImage = XMGS_IMAGEWAITING;
			pqi->mState = XMGUISIS_WAITING;
			pqi->mQueryResponseItem = r->mFiles.GetNext(pos);
			pqi->mQueryResponseItem->AddRef();
			CSearchView::mQueryItems.AddTail(pqi);

			//begin downloading the thumbnail
			cm()->Lock();
			cm()->EnqueueFile(pqi->mQueryResponseItem, true, XMGUI_THUMBWIDTH, XMGUI_THUMBHEIGHT);
			cm()->Unlock();
		}
		r->Release();

	case XM_SMU_QUERY_CANCEL:
	case XM_SMU_QUERY_ERROR:
		break;

	case XM_SMU_MOTD_RECEIVED:

		//show the motd if not new (should be in xmgui.cpp)
		if (sm()->MotdIsNew())
		{
			sm()->MotdShow();
		}
		break;
	}
	return 0;
}

// --------------------------------------------------------------------------- Query Item


CXMGUIQueryItem::CXMGUIQueryItem()
{
	mQueryResponseItem = NULL;
}

CXMGUIQueryItem::~CXMGUIQueryItem()
{
	//unclamp the cache
	dbman()->Lock();
	DWORD i = dbman()->FindCachedFileByMD5(mThumbMD5);
	if (i!=-1)
	{
		dbman()->UnclampCachedFile(i);
	}
	dbman()->Unlock();

	//release the response item
	if (mQueryResponseItem)
		mQueryResponseItem->Release();
}
	

// ------------------------------------------------------------------------------------- Search View

BEGIN_MESSAGE_MAP(CSearchView, CWnd)

	ON_WM_SIZE()
	ON_WM_PAINT()
	ON_WM_DESTROY()

	ON_MESSAGE(XM_CLIENTMSG, OnClientMessage)
	ON_MESSAGE(XM_SERVERMSG, OnServerMessage)

	ON_COMMAND(ID_SEARCH_SEARCH,		OnSearchSearch)	
	ON_COMMAND(ID_SEARCH_EDIT,			OnSearchEdit)
	ON_COMMAND(ID_SEARCH_SAVE,			OnSearchSave)
	ON_COMMAND(ID_SEARCH_SAVEDSEARCH,	OnSavedSearch)
	ON_COMMAND(ID_SEARCH_SAVEDEDIT,		OnSavedEdit)
	ON_COMMAND(ID_SEARCH_SAVEDDELETE,	OnSavedDelete)

	ON_COMMAND(ID_DOWNLOAD, OnDownload)

	ON_NOTIFY(LVN_DELETEITEM, IDC_SEARCH_THUMBS, OnThumbsDeleteItem)
	ON_NOTIFY(NM_DBLCLK, IDC_SEARCH_THUMBS, OnThumbsDoubleClick)
	ON_WM_CONTEXTMENU()

	ON_NOTIFY(TBN_GETINFOTIP, IDC_SEARCH_TOOLS, OnGetInfoTip)
	ON_NOTIFY(TBN_GETINFOTIP, IDC_SEARCH_SAVEDTOOLS, OnGetInfoTip)

END_MESSAGE_MAP()

CSearchView::CSearchView()
{
	m_udRefCount = 1;
}

CSearchView::~CSearchView()
{
	//save the filter
	config()->FilterSet(&mQuery->mRejection);
}

void CSearchView::AddRef()
{
	//something else is referencing us
	m_udRefCount++;
}

void CSearchView::Release()
{
	//release reference
	m_udRefCount--;
	if (m_udRefCount<1) {
		delete this;
	}
}

void CSearchView::OnDestroy()
{
	//unsubscribe
	cm()->UnSubscribe(m_hWnd);
	sm()->UnSubscribe(m_hWnd);
}

void CSearchView::OnPaint()
{
	CPaintDC dc(this);
	dc.FillSolidRect(&dc.m_ps.rcPaint, GetSysColor(COLOR_3DFACE));
}

void CSearchView::OnGetInfoTip(NMHDR *pnmh, LRESULT* pResult)
{
	LPNMTBGETINFOTIP git = (LPNMTBGETINFOTIP) pnmh;
	switch (git->iItem)
	{
	case ID_SEARCH_SEARCH:
	case ID_SEARCH_SAVEDSEARCH:
		strncpy(git->pszText, "Search", git->cchTextMax);
		break;

	case ID_SEARCH_EDIT:
	case ID_SEARCH_SAVEDEDIT:
		strncpy(git->pszText, "Edit Search", git->cchTextMax);
		break;

	case ID_SEARCH_SAVE:
		strncpy(git->pszText, "Save Search", git->cchTextMax);
		break;

	case ID_SEARCH_SAVEDDELETE:
		strncpy(git->pszText, "Delete Search", git->cchTextMax);
		break;
	}
}

// ------------------------------------------------------------------------------------------- Saved

class CSaveSearch : public CDialog
{
public:
	CSaveSearch(CWnd *parent)
		: CDialog(IDD_SAVESEARCH, parent)
	{
	}
	CString mName;
	void DoDataExchange(CDataExchange *pDX)
	{
		DDX_Text(pDX, IDC_NAME, mName);
		DDV_MaxChars(pDX, mName, MAX_PATH);
	}
};

void CSearchView::OnSearchSave()
{
	//take the current query and save it
	CSaveSearch dlg(this);
	if (dlg.DoModal()==IDOK)
	{
		//save
		if (mQuery->mName)
			free(mQuery->mName);
		mQuery->mName = strdup(dlg.mName);
		config()->QuerySave(mQuery);
		RefreshSaved();
	}
}

void CSearchView::OnSavedSearch()
{
	//get the query
	CXMQuery *q = GetSavedQuery();
	if (!q) return;

	//check state
	if (sm()->QueryIsRunning())
		return;

	//begin query
	if (!sm()->QueryBegin(q)) {
		AfxMessageBox("Error running search.");
	}

	//refresh the advert
	((CMainFrame*)GetParent())->OnSearch(q);
}

void CSearchView::OnSavedEdit()
{
	//get the query
	CXMQuery *q = GetSavedQuery();
	if (!q) return;

	//edit it
	QueryBuildSimple(q);
}

void CSearchView::OnSavedDelete()
{
	//delete the selected item
	int i = mSavedList.GetCurSel();
	if (i==LB_ERR)
		return;

	//kill it
	config()->QueryDeleteItem(i);
	RefreshSaved();
}

void CSearchView::RefreshSaved()
{
	//remember the selection
	CString sel;
	int x = mSavedList.GetCurSel();
	if (x != -1)
		mSavedList.GetLBText(x, sel);

	//delete all the current list items
	mSavedList.ResetContent();

	//just add the name to the listbox
	CXMQuery *q;
	char *s;
	for (int i=0;i<config()->QueryGetCount();i++)
	{
		config()->QueryGetItem(i, &q);
		s = q->mName;
		if (!s)
			s = "<empty>";
		mSavedList.AddString(s);
	}

	//restore selection
	if (x != -1)
		mSavedList.SelectString(-1, sel);
}

CXMQuery* CSearchView::GetSavedQuery()
{
	//anything selected?
	int i = mSavedList.GetCurSel();
	if (i==LB_ERR)
		return NULL;
	CXMQuery *q;
	config()->QueryGetItem(i, &q);
	return q;
}

// --------------------------------------------------------------------------------------- Searching

void CSearchView::OnThumbsDeleteItem(NMHDR *pnmh, LRESULT* pResult)
{
	//free custom data
	NMLISTVIEW *pnmlv = (NMLISTVIEW*)pnmh;
	CXMGUIQueryItem *pqi;
	if (pnmlv->lParam)
	{
		pqi = (CXMGUIQueryItem*)pnmlv->lParam;
		//delete pqi;
	}
}

void CSearchView::OnThumbsDoubleClick(NMHDR *pnmh, LRESULT* pResult)
{
	//start download
	OnDownload();
}

void CSearchView::OnContextMenu(CWnd* pWnd, CPoint pos)
{
	//was it in the listview?
	if (*pWnd!=mThumbs)
		return;

	//create menu
	CMenu menu;
	menu.CreatePopupMenu();
	menu.AppendMenu(MF_POPUP, (UINT)LoadMenu(AfxGetResourceHandle(), (LPCSTR)IDR_THUMBNAIL));
	menu.GetSubMenu(0)->TrackPopupMenu(TPM_LEFTALIGN|TPM_TOPALIGN, pos.x, pos.y, this, 0);
}

void CSearchView::OnDownload()
{
	//begin downloading each of the selected thumbnails
	CXMGUIQueryItem *pqi;
	int i = -1;
	cm()->Lock();
	while ((i=mThumbs.GetNextItem(i, LVNI_SELECTED))!=-1)
	{
		//start the download
		pqi = (CXMGUIQueryItem*)mThumbs.GetItemData(i);
		cm()->EnqueueFile(pqi->mQueryResponseItem, false, 0, 0);
	}
	cm()->Unlock();
}

UINT CSearchView::GetViewType()
{
	//the id of the view
	return XMGUIVIEW_SEARCH;
}

#define CSVGCL_BORDER			4
#define CSVGCL_CTRLTOP			16
#define CSVGCL_CURRENTWIDTH		100
#define CSVGCL_CURRENTHEIGHT	35
#define CSVGCL_SFWIDTH			300
#define CSVGCL_SFHEIGHT			53
#define CSVGCL_SLWIDTH			175
#define CSVGCL_SLHEIGHT			125 //25
#define CSVGCL_STWIDTH			100
#define CSVGCL_STHEIGHT			35
#define CSVGCL_THUMBTOP			64

bool CSearchView::Create(CWnd *hwParent, CRect &rect)
{
	//calculate control positions
	CRect rct, rst, rsf, rsl, rt;
	rct.left	= 0				+ CSVGCL_BORDER;
	rct.top		= 0				+ CSVGCL_CTRLTOP;
	rct.right	= rct.left		+ CSVGCL_CURRENTWIDTH;
	rct.bottom	= rct.top		+ CSVGCL_CURRENTHEIGHT;
	rsf.left	= rct.right		+ CSVGCL_BORDER;
	rsf.top		= 0				+ CSVGCL_BORDER; 
	rsf.right	= rsf.left		+ CSVGCL_SFWIDTH;
	rsf.bottom	= rsf.top		+ CSVGCL_SFHEIGHT;
	rsl.left	= rsf.left		+ CSVGCL_BORDER*2;
	rsl.top		= 0				+ CSVGCL_CTRLTOP + 5; 
	rsl.right	= rsl.left		+ CSVGCL_SLWIDTH;
	rsl.bottom	= rsl.top		+ CSVGCL_SLHEIGHT;
	rst.left	= rsl.right		+ CSVGCL_BORDER*2;
	rst.top		= 0				+ CSVGCL_CTRLTOP;
	rst.right	= rst.left		+ CSVGCL_STWIDTH;
	rst.bottom	= rst.top		+ CSVGCL_STHEIGHT;
	rt.left		= 0				+ 0; 
	rt.top		= 0				+ CSVGCL_THUMBTOP; 
	rt.right	= rect.Width()	- 0;
	rt.bottom	= rect.Height()	- 0;

	//create ourself
	if (!CWnd::Create(
			NULL,
			"CSearchView_Wnd",
			WS_CHILD|WS_VISIBLE,
			rect,
			hwParent,
			0,
			NULL)) {
		return false;
	}
	
	//create main toolbar
	mCurrentTools.Create(
				WS_CHILD|WS_VISIBLE|CCS_NOPARENTALIGN|TBSTYLE_TOOLTIPS|
				CCS_NORESIZE|CCS_NODIVIDER|TBSTYLE_FLAT,
				rct, this, IDC_SEARCH_TOOLS);
	mSavedFrame.Create(
				"Saved Searches",
				WS_CHILD|WS_VISIBLE|BS_GROUPBOX,
				rsf, this, IDC_SEARCH_SAVEDFRAME);
	mSavedList.Create(
				WS_CHILD|WS_VISIBLE|CBS_DROPDOWNLIST|CBS_NOINTEGRALHEIGHT|CBS_AUTOHSCROLL|WS_VSCROLL,
				rsl, this, IDC_SEARCH_SAVEDLIST);
	mSavedTools.Create(
				WS_CHILD|WS_VISIBLE|CCS_NOPARENTALIGN|TBSTYLE_TOOLTIPS|
				CCS_NORESIZE|CCS_NODIVIDER|TBSTYLE_FLAT,
				rst, this, IDC_SEARCH_SAVEDTOOLS);
	mThumbs.CreateEx(
				WS_EX_CLIENTEDGE,
				WC_LISTVIEW,
				NULL,
				WS_CHILD|WS_VISIBLE|WS_BORDER|LVS_AUTOARRANGE,
				rt, this, IDC_SEARCH_THUMBS);
	
	//set the font on the group box
	CFont font;
	font.CreateStockObject(ANSI_VAR_FONT);
	mSavedFrame.SetFont(&font, FALSE);
	mSavedList.SetFont(&font, FALSE);

	//add bitmaps and strings to toolbars
	mImages.Create(IDB_SEARCHTOOLS, 24, 0, RGB(255,0,255));
	mCurrentTools.SetImageList(&mImages);
	mSavedTools.SetImageList(&mImages);
	/*
	mCurrentTools.AddStrings("Search\0Modify Search\0Save Search\0");
	mSavedTools.AddStrings("Search\0Modify Search\0Delete Search\0");
	*/
	
	//add buttons to the current toolbar
	TBBUTTON tb[3];
	tb[0].iBitmap = 0;
	tb[0].idCommand = ID_SEARCH_SEARCH;
	tb[0].fsStyle = TBSTYLE_BUTTON;
	tb[0].fsState = TBSTATE_ENABLED;
	tb[0].iString = 0;
	tb[1].iBitmap = 1;
	tb[1].idCommand = ID_SEARCH_EDIT;
	tb[1].fsStyle = TBSTYLE_BUTTON;
	tb[1].fsState = TBSTATE_ENABLED;
	tb[1].iString = 1;
	tb[2].iBitmap = 2;
	tb[2].idCommand = ID_SEARCH_SAVE;
	tb[2].fsStyle = TBSTYLE_BUTTON;
	tb[2].fsState = TBSTATE_ENABLED;
	tb[2].iString = 2;
	mCurrentTools.AddButtons(3, tb);

	//add buttons to the saved toolbar
	tb[0].idCommand = ID_SEARCH_SAVEDSEARCH;
	tb[1].idCommand = ID_SEARCH_SAVEDEDIT;
	tb[2].iBitmap = 3;
	tb[2].idCommand = ID_SEARCH_SAVEDDELETE;
	tb[2].iString = 3;
	mSavedTools.AddButtons(3, tb);

	//set enabled state of execute buttons
	mCurrentTools.EnableButton(ID_SEARCH_SEARCH, !sm()->QueryIsRunning());
	mSavedTools.EnableButton(ID_SEARCH_SAVEDSEARCH, !sm()->QueryIsRunning());

	//update our listview
	mThumbs.SetImageList(&mQueryImages, LVSIL_NORMAL);
	ShowResults();

	//Load the saved queries
	RefreshSaved();

	//load the filter
	config()->FilterGet(&mQuery->mRejection);

	//hook us into the updates
	cm()->Subscribe(m_hWnd, TRUE);
	sm()->Subscribe(m_hWnd, TRUE);
		
	return true;
}

void CSearchView::OnSize(UINT nType, int cx, int cy)
{
	//resize the thumbnail listview
	if (mThumbs.m_hWnd) 
	{
		mThumbs.MoveWindow(0, CSVGCL_THUMBTOP, cx, cy - CSVGCL_THUMBTOP, TRUE);
	}
}

LRESULT CSearchView::OnClientMessage(WPARAM wParam, LPARAM lParam)
{
	LVITEMA lvi;
	/*
	int x;
	POSITION pos;
	CXMGUIQueryItem *pqi2;
	*/

	//get the item this refers to
	CXMPipelineUpdateTag *tag = (CXMPipelineUpdateTag*)lParam;
	CXMGUIQueryItem *pqi = GetQueryItemFromTag(tag);

	switch (wParam)
	{		
	case XM_CMU_QUEUE_REMOVE:
	case XM_CMU_DOWNLOAD_FINISH:
	case XM_CMU_DOWNLOAD_REQUESTING:
	case XM_CMU_DOWNLOAD_RECEIVING:
	case XM_CMU_COMPLETED_REMOVE:

		//all ignored
		break;

	case XM_CMU_QUEUE_ADD:
	case XM_CMU_DOWNLOAD_START:
	case XM_CMU_DOWNLOAD_ERROR:
	case XM_CMU_DOWNLOAD_CANCEL:
	case XM_CMU_COMPLETED_ADD:

		//update the picture
		if (pqi) 
		{
			lvi.iItem = pqi->mItem;
			lvi.iSubItem = 0;
			lvi.mask = LVIF_IMAGE;
			lvi.iImage = pqi->mImage;
			mThumbs.SetItem(&lvi);

			//if everything is in, start another download
			/*
			x = 0;
			pos = mQueryItems.GetHeadPosition();
			while (pos)
			{
				pqi2 = mQueryItems.GetNext(pos);
				if ((pqi2->mState==XMGUISIS_THUMBCOMPLETE) ||
					(pqi2->mState==XMGUISIS_ERROR))
				{
					x++;
				}
			}
			if (x==mQueryItems.GetCount())
			{
				OnSearchSearch();
			}
			*/
		}
		break;

	}
	tag->Release();
	return 0;
}

LRESULT CSearchView::OnServerMessage(WPARAM wParam, LPARAM lParam)
{
	switch (wParam)
	{
	case XM_SMU_QUERY_SENT:
	case XM_SMU_QUERY_BEGIN:

		//disable the execute buttons
		mCurrentTools.EnableButton(ID_SEARCH_SEARCH, FALSE);
		mSavedTools.EnableButton(ID_SEARCH_SAVEDSEARCH, FALSE);
		break;

	case XM_SMU_QUERY_FINISH:

		//show results in the listview
		ShowResults();

	case XM_SMU_QUERY_CANCEL:
	case XM_SMU_QUERY_ERROR:

		//enable the execute buttons
		mCurrentTools.EnableButton(ID_SEARCH_SEARCH, TRUE);
		mSavedTools.EnableButton(ID_SEARCH_SAVEDSEARCH, TRUE);
		break;
	}
	return 0;
}

void CSearchView::ShowResults()
{
	//ensure listview is empty
	mThumbs.DeleteAllItems();

	//add item for each query item
	char buf[MAX_PATH];
	LVITEMA lvi;
	CXMGUIQueryItem *pqi;
	POSITION pos = mQueryItems.GetHeadPosition();
	while (pos)
	{	
		//get the item
		pqi = mQueryItems.GetNext(pos);

		//format text
		sprintf(buf, "%dx%d",
			pqi->mQueryResponseItem->mWidth,
			pqi->mQueryResponseItem->mHeight);

		//insert into listview
		lvi.mask = LVIF_TEXT|LVIF_IMAGE|LVIF_PARAM;
		lvi.iItem = mThumbs.GetItemCount();
		lvi.iSubItem = 0;
		lvi.iImage = pqi->mImage;
		lvi.lParam = (LPARAM)pqi;
		lvi.pszText = buf;
		pqi->mItem = mThumbs.InsertItem(&lvi);
	}
}

void CSearchView::OnSearchSearch()
{
	//check state
	if (sm()->QueryIsRunning())
		return;

	//begin query
	if (!sm()->QueryBegin(mQuery)) {
		AfxMessageBox("Error running search.");
	}

	//add to mru
	config()->QueryMru(mQuery);
	RefreshSaved();

	//refresh the advert
	((CMainFrame*)GetParent())->OnSearch(mQuery);
}

void CSearchView::OnSearchEdit()
{
	//modify the current query
	QueryBuildSimple(mQuery);
}
