// QueryTester.cpp : implementation file
//

#include "stdafx.h"
#include "XMClient.h"
#include "QueryTester.h"
#include "xmsession.h"
#include "xmpipeline.h"
#include "xmquery.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

CXMQuery* gpquery;

/////////////////////////////////////////////////////////////////////////////
// CQueryTester dialog


CQueryTester::CQueryTester(CWnd* pParent /*=NULL*/)
	: CDialog(CQueryTester::IDD, pParent)
{
	//{{AFX_DATA_INIT(CQueryTester)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	gpquery = new CXMQuery();
}


void CQueryTester::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CQueryTester)
	DDX_Control(pDX, IDC_STATUS, mStatus);
	DDX_Control(pDX, IDC_RESULTS, mResults);
	DDX_Control(pDX, IDC_PREVIEW, mPreview);
	DDX_Control(pDX, IDC_HOSTS, mHosts);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CQueryTester, CDialog)
	//{{AFX_MSG_MAP(CQueryTester)
	ON_BN_CLICKED(IDC_RUNQUERY, OnRunquery)
	ON_LBN_SELCHANGE(IDC_RESULTS, OnSelchangeResults)
	ON_BN_CLICKED(IDC_QUERY, OnQuery)
	ON_BN_CLICKED(IDC_GETFILE, OnGetfile)
	ON_BN_CLICKED(IDC_GETTHUMB, OnGetthumb)
	ON_WM_DESTROY()
	//}}AFX_MSG_MAP
	ON_MESSAGE(XM_CLIENTMSG, OnClientMessage)
	ON_MESSAGE(XM_SERVERMSG, OnServerMessage)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CQueryTester message handlers

void CQueryTester::OnRunquery() 
{
	sm()->QueryBegin(gpquery);	
}

void CQueryTester::OnQuery() 
{
	gpquery->BuildSimple();	
}

void CQueryTester::OnSelchangeResults() 
{
	//fill the hosts listbox
	CString str;
	CXMQueryResponseItem *ri =
		(CXMQueryResponseItem*)mResults.GetItemDataPtr(mResults.GetCurSel());
	sm()->Lock();
	mHosts.ResetContent();
	for (int i=0;i<ri->mHostsCount;i++) {
		str.Format("%s, speed: %d", ri->mHosts[i].Ip, ri->mHosts[i].Speed);
		mHosts.AddString(str);
	}
	sm()->Unlock();
}

void CQueryTester::OnGetfile() 
{
	CXMQueryResponseItem *ri = ((CXMQueryResponseItem*)mResults.GetItemDataPtr(mResults.GetCurSel()))->Clone();
	cm()->EnqueueFile(ri, false, ri->mWidth, ri->mHeight);
	cm()->FlushQueue();
}

void CQueryTester::OnGetthumb() 
{
	CXMQueryResponseItem *ri = ((CXMQueryResponseItem*)mResults.GetItemDataPtr(mResults.GetCurSel()))->Clone();
	cm()->EnqueueFile(ri, true, 150, 150);
	cm()->FlushQueue();
}

BOOL CQueryTester::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	//subscribe to events
	cm()->Subscribe(m_hWnd);
	sm()->Subscribe(m_hWnd);

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CQueryTester::OnDestroy() 
{
	//unsubscribe to events
	cm()->UnSubscribe(m_hWnd);
	sm()->UnSubscribe(m_hWnd);

	CDialog::OnDestroy();
}

// -------------------------------------------------------------------------- Client

LRESULT CQueryTester::OnClientMessage(WPARAM wParam, LPARAM lParam)
{
	CXMClientManager::CompletedFile *cf;
	CDIBSection bmp;
	CJPEGDecoder dec;
	HANDLE h;

	switch (wParam) {

	case XM_CMU_QUEUE_ADD:
	case XM_CMU_QUEUE_REMOVE:
		TRACE("QUEUE ACTIVITY\n");
		break;

	case XM_CMU_DOWNLOAD_START:
		mStatus.SetWindowText("Connecting...");
		break;

	case XM_CMU_DOWNLOAD_REQUESTING:
		mStatus.SetWindowText("Requesting file...");
		break;

	case XM_CMU_DOWNLOAD_RECEIVING:
		mStatus.SetWindowText("Receiving file...");
		break;

	case XM_CMU_DOWNLOAD_ERROR:
	case XM_CMU_DOWNLOAD_CANCEL:
		mStatus.SetWindowText("Error retreiving file.");
		break;

	case XM_CMU_DOWNLOAD_FINISH:
		mStatus.SetWindowText("Finish downloading file.");
		break;

	case XM_CMU_COMPLETED_ADD:

		//new file waiting
		cm()->Lock();
		if (cm()->GetCompletedFileCount()<1) {
			mStatus.SetWindowText("No completed files waiting!");
			break;
		}
		cf = cm()->GetCompletedFile(0);

		//convert to image
		dec.MakeBmpFromMemory(cf->mBuffer, cf->mBufferSize, &bmp);
		
		//display in preview
		h = mPreview.SetBitmap((HBITMAP)bmp.GetHandle());
		bmp.Detach();
		if (h) DeleteObject(h);

		//finished
		cm()->RemoveCompletedFile(0);
		cm()->Unlock();
		break;

	case XM_CMU_COMPLETED_REMOVE:
		break;

	case XM_CMU_UPLOAD_START:
		mStatus.SetWindowText("Beginning upload...");
		break;

	case XM_CMU_UPLOAD_FINISH:
		mStatus.SetWindowText("Upload complete.");
		break;
	}

	return 0;
}

// -------------------------------------------------------------------------- Server

LRESULT CQueryTester::OnServerMessage(WPARAM wParam, LPARAM lParam)
{
	CString str;
	CXMQueryResponse *resp;
	CXMQueryResponseItem *ri;
	POSITION pos;
	
	switch (wParam) {

	//QUERIES
	case XM_SMU_QUERY_BEGIN:
		mStatus.SetWindowText("Sending query...");
		break;

	case XM_SMU_QUERY_SENT:
		mStatus.SetWindowText("Query sent, waiting for response...");
		break;

	case XM_SMU_QUERY_CANCEL:
	case XM_SMU_QUERY_ERROR:
		mStatus.SetWindowText("Query failed!");
		break;

	case XM_SMU_QUERY_FINISH:
		
		//get query response
		sm()->Lock();
		resp = sm()->QueryGetResponse();
		if (!resp) {
			AfxMessageBox("Could not retrieve query response.");
			break;
		}

		//clear current ui stuff
		ClearUI();
		
		//fill the results listbox
		pos = resp->mFiles.GetHeadPosition();
		while (pos)
		{
			ri = resp->mFiles.GetNext(pos);
			mResults.SetItemDataPtr(mResults.AddString(ri->mMD5.GetString()), (void*)ri);
		}

		//done
		str.Format("Query finished. %d results.", resp->mFiles.GetCount());
		mStatus.SetWindowText(str);
		sm()->Unlock();
		break;

	//SERVER CONNECTION
	case XM_SMU_SERVER_CONNECTING:
		break;

	case XM_SMU_SERVER_CONNECTED:
		mStatus.SetWindowText("Connected to server.");
		break;

	case XM_SMU_SERVER_ERROR:
	case XM_SMU_SERVER_CLOSED:
		mStatus.SetWindowText("Connection to server lost!");
		break;

	//LOGIN MESSAGES
	case XM_SMU_LOGIN_BEGIN:
	case XM_SMU_LOGIN_SENT:
	case XM_SMU_LOGIN_RECEIVED:
	case XM_SMU_LOGIN_LISTING:
	case XM_SMU_LOGIN_FINISH:
		mStatus.SetWindowText("Login complete.");
		break;

	case XM_SMU_LOGIN_CANCEL:
	case XM_SMU_LOGIN_ERROR:
		mStatus.SetWindowText("Login failed.");
		break;
	}

	return 0;
}

void CQueryTester::ClearUI()
{
	//delete all the CMD5 pointers
	/*
	void* pv;
	for (int i=0;i<mResults.GetCount();i++) {
		pv = mResults.GetItemDataPtr(i);
		if (pv) {
			delete (CMD5*)pv;
		}
	}
	*/
	
	//clear listboxes
	mResults.ResetContent();
	mHosts.ResetContent();
}
