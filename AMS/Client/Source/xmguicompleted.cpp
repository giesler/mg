// XMGUICOMPLETED.CPP ------------------------------------------------ XMGUICOMPLETED.CPP

#include "stdafx.h"
#include "xmclient.h"
#include "xmnet.h"
#include "xmpipeline.h"
#include "xmgui.h"

// --------------------------------------------------------------------- Smart Image List

CSmartImageList::CSmartImageList()
{
	//init mask
	mMaskCount = 0;
	mMaskSize = 32;
	mMask = (BYTE*)malloc(mMaskSize);
	memset(mMask, 0, mMaskSize);

	//create the image list
	mImages.Create(XMGUI_THUMBWIDTH, XMGUI_THUMBHEIGHT, ILC_COLOR24, 32, 16);
}
CSmartImageList::~CSmartImageList()
{
	//free mask
	if (mMask)
		free(mMask);
}
int CSmartImageList::SILInsert(CDIBSection *dib)
{
	//passthrough to CBitmap function
	int temp;
	CBitmap bmp;
	bmp.Attach(dib->GetHandle());
	temp = SILInsert(&bmp);
	bmp.Detach();
	return temp;
}
int CSmartImageList::SILInsert(CBitmap *bmp)
{
	//look at the masks first
	int n = -1;
	for (int i=0;i<mMaskCount;i++)
	{
		if (mMask[i] == 0)
		{
			n = i;	
			break;
		}
	}
	if (n == -1)
	{
		//call the standard insert
		n = mImages.Add(bmp, (CBitmap*)NULL);
		if (n == -1)
		{
			//failed
			return -1;
		}

		//enough room in mask?
		if (n+1 > mMaskSize)
		{
			//expand the mask
			mMaskSize = n+16;
			mMask = (BYTE*)realloc(mMask, mMaskSize);
			memset(&mMask[mMaskCount], 0, mMaskSize-mMaskCount);
		}
		mMaskCount = n+1;
		mMask[n] = 1;
	}
	else
	{
		//slot now in use
		mMask[n] = 1;
		
		//copy the image
		if (!mImages.Replace(n, bmp, (CBitmap*)NULL))
		{
			//failed
			return -1;
		}
	}

	//success
	return n;
}

void CSmartImageList::SILDelete(int n)
{
	//mark the slot empty
	ASSERT(n<mMaskCount);
	mMask[n] = 0;
}

const CImageList* CSmartImageList::SILGetImageList()
{
	return &mImages;
}

// --------------------------------------------------------------------- Completed View

BEGIN_MESSAGE_MAP(CCompletedView, CWnd)
	
	ON_WM_SIZE()
	ON_WM_DESTROY()
	ON_WM_PAINT()
	ON_MESSAGE(XM_CLIENTMSG, OnClientMessage)
	ON_MESSAGE(XM_SPLITMOVE, OnSplitterMove)

	//listview notifications
	ON_NOTIFY(LVN_DELETEITEM, IDC_FILES, OnThumbsDeleteItem)
	ON_NOTIFY(NM_CLICK, IDC_FILES, OnThumbsClick)

	//buttons
	ON_BN_CLICKED(IDC_SAVE, OnSave)
	ON_BN_CLICKED(IDC_DELETE, OnDelete)
	
END_MESSAGE_MAP()

CCompletedView::CCompletedView()
{
	m_udRefCount = 1;
}

CCompletedView::~CCompletedView()
{
}

void CCompletedView::AddRef()
{
	//something else is referencing us
	m_udRefCount++;
}

void CCompletedView::Release()
{
	//release reference
	m_udRefCount--;
	if (m_udRefCount<1) {
		delete this;
	}
}

UINT CCompletedView::GetViewType()
{
	//the id of the view
	return XMGUIVIEW_COMPLETED;
}

void CCompletedView::OnPaint()
{
	CPaintDC dc(this);
	dc.FillSolidRect(&dc.m_ps.rcPaint, ::GetSysColor(COLOR_3DFACE));

	mPreview.RedrawWindow();
}

void CCompletedView::OnDestroy()
{
	//save splitter pos
	config()->SetField(FIELD_GUI_COMPLETED_SPLIT, mLastSplitPos);

	//unregister
	cm()->UnSubscribe(m_hWnd);
}

void CCompletedView::OnThumbsClick(NMHDR* pnmh, LRESULT* pResult)
{
	//get selected tag
	int i = mThumbs.GetNextItem(-1, LVNI_SELECTED);
	if (i==-1)
		return;
	tag t = (tag)mThumbs.GetItemData(i);

	//decode jpeg data
	CJPEGDecoder jpeg;
	CDIBSection dib;
	try 
	{
		jpeg.MakeBmpFromMemory(t->mBuffer, t->mBufferSize, &dib, 32);
	}
	catch (...)
	{
		ASSERT(FALSE);
		return;
	}

	//show the bitmap
	CBitmap *pbmp = new CBitmap();
	pbmp->Attach(dib.GetHandle());
	dib.Detach();
	mPreview.ShowBitmap(pbmp);
}

void CCompletedView::OnThumbsDeleteItem(NMHDR *pnmh, LRESULT* pResult)
{
	//release image
	NMLISTVIEW *pnmlv = (NMLISTVIEW*)pnmh;
	LVITEM lvi;
	lvi.mask = LVIF_IMAGE;
	lvi.iItem = pnmlv->iItem;
	lvi.iSubItem = 0;
	if (mThumbs.GetItem(&lvi))
	{
		mThumbsImages.SILDelete(lvi.iImage);
	}

	//free tag
	delete (tag)pnmlv->lParam;
}

bool CCompletedView::InsertItem(tag t)
{
	LVITEMA lvi;
	cachefile *cf;
	DWORD cacheindex;
	CDIBSection *dib;
	CJPEGDecoder jpeg;
	int n;
	char sz[MAX_PATH];
	tag t2;

	//don't do thumbnails
	if (t->mIsThumb)
		return false;

	//decode jpeg
	dbman()->Lock();
	try 
	{
		//get the thumbnail from cache
		cacheindex = dbman()->FindCachedFileByParent(t->mMD5);
		if (cacheindex!=-1)
		{
			//decode
			//AfxMessageBox("Cached file found.");
			cf = dbman()->GetCachedFile(cacheindex);
			dib = new CDIBSection();
			jpeg.MakeBmpFromMemory(cf->file, cf->size, dib, 32);
		}
		else
		{
			//decode full image
			//AfxMessageBox("Resizing new file.");
			CDIBSection dibsrc;
			jpeg.MakeBmpFromMemory(t->mBuffer, t->mBufferSize, &dibsrc, 32);

			//resize to thumbnail
			dib = FastResize(&dibsrc, XMGUI_THUMBWIDTH, XMGUI_THUMBHEIGHT);
		}
		//AfxMessageBox("Succesfully decoded...");
	}
	catch (...)
	{
		AfxMessageBox("Error while decoding JPEG data.");
		//ASSERT(FALSE);
		if (dib)
		{
			//delete dib;
			//AfxMessageBox("Done deleting.");
			dib = NULL;
		}
		//AfxMessageBox("Done deleting 2.");
	}
	dbman()->Unlock();
	
	//insert into listview
	if (dib)
	{
		//insert thumbnail
		n = mThumbsImages.SILInsert(dib);
		if (n==-1)
		{
			AfxMessageBox("Image insert failed.");
			ASSERT(FALSE);
			return false;
		}
		delete dib;
		dib = NULL;

		//create text
		sprintf(sz, "%d x %d", t->mWidth, t->mHeight);

		//copy tag
		t2 = (tag)malloc(sizeof(CXMClientManager::CompletedFile));
		memcpy(t2, t, sizeof(CXMClientManager::CompletedFile));
		
		//insert listview item
		lvi.mask = LVIF_IMAGE|LVIF_PARAM|LVIF_TEXT;
		lvi.iImage = n;
		lvi.lParam = (LPARAM)t2;
		lvi.pszText = sz;
		lvi.iItem = mThumbs.GetItemCount();
		lvi.iSubItem = 0;
		mThumbs.InsertItem(&lvi);
	}
	return true;
}

bool CCompletedView::Create(CWnd *hwParent, CRect &rect)
{	
	//create self
	if (!CWnd::Create(
			NULL,
			"CCompletedView_Wnd",
			WS_CHILD|WS_VISIBLE|WS_CLIPCHILDREN,
			rect,
			hwParent,
			0,
			NULL)) {
		return false;
	}

	//load splitter position
	mLastSplitPos = config()->GetFieldLong(FIELD_GUI_COMPLETED_SPLIT);

	//create controls
	if (!mThumbs.CreateEx(
			WS_EX_CLIENTEDGE, WC_LISTVIEW,
			NULL,
			WS_CHILD|WS_VISIBLE|LVS_AUTOARRANGE,
			CRect(0,0,0,0),
			this,
			IDC_FILES))
		return false;

	if (!mSave.Create(
			"Save",
			WS_CHILD|WS_VISIBLE,
			CRect(0,0,0,0),
			this,
			IDC_SAVE))
		return false;

	if (!mDelete.Create(
			"Delete",
			WS_CHILD|WS_VISIBLE,
			CRect(0,0,0,0),
			this,
			IDC_DELETE))
		return false;

	if (!mSplitter.Create(
			this,
			0,
			CRect(0,0,0,0),
			XMGUI_VIEWSPLITBORDER,
			XMGUI_VIEWSPLITBORDER,
			GetSysColor(COLOR_3DFACE)))
		return false;

	if (!mPreview.Create(
			NULL,
			NULL,
			WS_CHILD|WS_VISIBLE,
			CRect(0,0,0,0),
			this,
			0))
		return false;
	
	//setup image list
	mThumbs.SetImageList(const_cast<CImageList*>(mThumbsImages.SILGetImageList()), LVSIL_NORMAL);

	//set fonts
	CFont f;
	f.CreateStockObject(ANSI_VAR_FONT);
	mSave.SetFont(&f);
	mDelete.SetFont(&f);

	//size the controls
	OnSize(0, rect.Width(), rect.Height());

	//Populate list control
	cm()->Lock();
	tag t;
	for (DWORD i=0;i<cm()->GetCompletedFileCount();i++)
	{
		t = cm()->GetCompletedFile(i);
		if (!t->mIsThumb)
			InsertItem(t);
	}
	cm()->Unlock();

	//register for updates
	cm()->Subscribe(m_hWnd, true);

	return true;
}

void CCompletedView::OnSize(UINT nType, int cx, int cy)
{
	int nButtonH = 75;
	int nButtonW1 = (mLastSplitPos - XMGUI_SPLITWIDTH)/2;
	int nButtonW2 = mLastSplitPos - XMGUI_SPLITWIDTH - nButtonW1;
	HDWP h = ::BeginDeferWindowPos(5);
	h = ::DeferWindowPos(h, mThumbs.m_hWnd, NULL, 0, 0, mLastSplitPos, cy-nButtonH-XMGUI_SPLITWIDTH, SWP_NOZORDER);
	h = ::DeferWindowPos(h, mSave.m_hWnd, NULL, 0, cy-nButtonH, nButtonW1, nButtonH, SWP_NOZORDER);
	h = ::DeferWindowPos(h, mDelete.m_hWnd, NULL, mLastSplitPos-nButtonW2, cy-nButtonH, nButtonW2, nButtonH, SWP_NOZORDER);
	h = ::DeferWindowPos(h, mPreview.m_hWnd, NULL, mLastSplitPos+XMGUI_SPLITWIDTH, 0, cx-mLastSplitPos-XMGUI_SPLITWIDTH, cy, SWP_NOZORDER);
	h = ::DeferWindowPos(h, mSplitter.m_hWnd, HWND_TOP, mLastSplitPos, 0, XMGUI_SPLITWIDTH, cy, SWP_SHOWWINDOW);
	::EndDeferWindowPos(h);
}

LRESULT CCompletedView::OnSplitterMove(WPARAM wParam, LPARAM lParam)
{
	//resize cotnrols
	CRect rc;
	GetClientRect(rc);
	long x = XM_SPLITLEFT(lParam);
	long cx = XM_SPLITRIGHT(lParam);
	mLastSplitPos = x;
	OnSize(0, rc.Width(), rc.Height());
	return 0;
}

LRESULT CCompletedView::OnClientMessage(WPARAM wParam, LPARAM lParam)
{
	//get our tag
	CXMPipelineUpdateTag *utag = (CXMPipelineUpdateTag*)lParam;
	tag t;
	DWORD dw;
	//CString str;
	
	switch (wParam)
	{
	case XM_CMU_COMPLETED_ADD:

		//get completed file
		cm()->Lock();
		dw = cm()->FindCompletedFile(utag);
		if (dw!=-1)
		{
			//str.Format("Completed file #%d.", dw);
			//AfxMessageBox(str);
			t = cm()->GetCompletedFile(dw);
			if (t)
			{
				//insert
				InsertItem(t);
			}
		}
		cm()->Unlock();
		break;

	case XM_CMU_COMPLETED_REMOVE:
		
		//ignore thumbs
		if (!utag->thumb)
		{
			//remove from listview
			for (int i=0;i<mThumbs.GetItemCount();i++)
			{
				t = (tag)mThumbs.GetItemData(i);
				if (t->mMD5.IsEqual(utag->md5))
				{
					mThumbs.DeleteItem(i);
					break;
				}
			}
		}
		break;
	}

	//done
	utag->Release();
	return 0;
}

void CCompletedView::OnSave()
{
	//prefix files with path and today's date

	//loop through selected listviw items
	CString fname;
	
	int i = mThumbs.GetNextItem(-1, LVNI_SELECTED);
	tag t;
	while (i != -1)
	{
		//last part of file is first 4 bytes of the md5
		t = (tag)mThumbs.GetItemData(i);
		fname = BuildSavedFilename(t->mMD5.GetValue());
		
		//open the file
		FILE *f = fopen(fname, "wb");
		if (f)
		{
			fwrite(t->mBuffer, 1, t->mBufferSize, f);
			fclose(f);
		}
		else
		{
			CString err;
			err.Format("Could not open file for writing:\n%s", fname);
			AfxMessageBox(err, MB_OK | MB_ICONERROR);
			return;
		}

		//share the file
		db()->AddFile(
				fname,				//filename
				t->mMD5,			//md5
				t->mBuffer,			//data
				t->mBufferSize,		//data size
				t->mWidth,			//width
				t->mHeight);		//height
					
		//remove the item
		cm()->Lock();
		tag t2;
		for (DWORD j=0;j<cm()->GetCompletedFileCount();j++)
		{
			t2 = cm()->GetCompletedFile(j);
			if (t2->mMD5.IsEqual(t->mMD5))
			{
				cm()->RemoveCompletedFile(j);
				break;
			}
		}
		cm()->Unlock();

		//get next selected item
		i = mThumbs.GetNextItem(i, LVNI_SELECTED);
	}
}

void CCompletedView::OnDelete()
{
	int i;
	DWORD j;
	tag t, t2;

	//cycle through selected list items
	cm()->Lock();
	i = mThumbs.GetNextItem(-1, LVNI_SELECTED);
	while (i!=-1)
	{
		//look for the completed file index
		t = (tag)mThumbs.GetItemData(i);
		for (j=0;j<cm()->GetCompletedFileCount();j++)
		{	
			t2 = cm()->GetCompletedFile(j);
			if (!t2->mIsThumb &&
				t2->mMD5.IsEqual(t->mMD5))
			{
				//remove from client manager.. will get delete from
				//lsitview automatically
				cm()->RemoveCompletedFile(j);
				break;
			}
		}	
		i = mThumbs.GetNextItem(i, LVNI_SELECTED);
	}
	cm()->Unlock();
}