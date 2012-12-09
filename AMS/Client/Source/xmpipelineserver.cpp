//----------------------------------------------------------------------------------
// XMPIPLINESERVER.CPP											XMPIPLINESERVER.CPP
//----------------------------------------------------------------------------------

#include "stdafx.h"
#include "xmclient.h"
#include "xmnet.h"
#include "xmpipeline.h"
#include "xmdb.h"

DWORD WINAPI LoginThreadProc(LPVOID lpParameter);

// ------------------------------------------------------------------------------- utility

#define KB	(1024)
#define MB	(KB*1024)
#define GB	(MB*1024)

void size2str(char *buf, size_t bufsize, DWORD size)
{
	//convert size (in bytes) into a "n bytes/kb/mb/gb"
	//string, store in buf

	//bytes?
	if (size < KB)
	{
		//bytes
		_snprintf(buf, bufsize, "%lu bytes", size);		//%lu = long unsigned int
	}
	else if (size < MB)
	{
		//kb
		_snprintf(buf, bufsize, "%#.3g kb", (double)size/KB);
	}
	else if (size < GB)
	{
		//mb
		_snprintf(buf, bufsize, "%#.3g mb", (double)size/MB);
	}
	else
	{
		//gb
		_snprintf(buf, bufsize, "%#.3g gb", (double)size/GB);
	}
}

// ----------------------------------------------------------------------- XMServerManager

CXMServerManager::CXMServerManager()
: CXMPipelineBase()
{
	//setup server state
	mwndProgress = NULL;
	mServer = NULL;
	mServerShuttingDown = false;

	//setup query
	mQueryRunning = false;
	mQuery = NULL;
	mQueryResponse = NULL;
	
	//setup listings
	mListingWindow = NULL;

	mLoginMsg[0] = '\0';

	//init motd
	mMotdNew = false;
	mMotdType = NULL;
	mMotdMsg = NULL;
	mMotdQuestion = NULL;
	mMotdChoices = NULL;

	//limiters
	mLimiterMaxIndex = 1000;
	mLimiterMaxFilter = 1000;

	//au data
	mAuComplete = false;
	mAuAvailable = false;
	mAuRequired = false;
	mAuVersion[0] = '\0';

	//reconnect data
	mRcAttempts = 0;
	mRcExpectDead = true;
}

CXMServerManager::~CXMServerManager()
{
	//destroy server connection
	if (mServer)
	{
		mServer->Close();
		mServer->Release();
	}

	//free query
	if (mQuery) mQuery->Release();
	if (mQueryResponse) mQueryResponse->Release();

	//free username and such
	if (mUsername)
		free(mUsername);
	if (mPassword)
		free(mPassword);

	//free motd data
	if (mMotdType)
		free(mMotdType);
	if (mMotdMsg)
		free(mMotdMsg);
	if (mMotdQuestion)
		free(mMotdQuestion);
	if (mMotdChoices)
		free(mMotdChoices);
}

bool CXMServerManager::OnInitialize() {
	return true;
}

void CXMServerManager::OnWin32MsgPreview(UINT msg, WPARAM wparam, LPARAM lparam)
{
	//should we forward progress messages?
	if (msg==XM_PROGRESS && mwndProgress)
	{
		::PostMessage(mwndProgress, msg, wparam, lparam);
	}
}

void CXMServerManager::OnWin32MsgReview(UINT msg, WPARAM wparam, LPARAM lparam)
{
	//empty
}

void CXMServerManager::OnMsgReceived(CXMSession *ses, CXMMessage *msg)
{
	//what type of message?
	CXMQueryResponse *resp;
	if (strcmp(msg->GetFor(false), XMMSG_LOGIN)==0
		&& !mLoginCanceled)
	{		
		//was the message a succes, or error message?
		char* success = msg->GetField("success")->GetValue(false);
		if (stricmp(success, "true")!=0)
		{
			//reset reconnect data
			mRcExpectDead = true;

			//login failed, upgrade needed?
			if (stricmp(msg->GetField("au/required")->GetValue(false), "true")==0)
			{
				//needs an upgrade
				mAuAvailable = true;
				mAuRequired = true;
				strncpy(mLoginMsg, msg->GetField("au/version")->GetValue(false), MAX_PATH);
				SendEvent(XM_SERVERMSG, XM_SMU_LOGIN_ERROR_UPGRADE, (LPARAM)mLoginMsg);
			}
			else
			{
				//not a version upgrade error
				strncpy(mLoginMsg, msg->GetField("error")->GetValue(false), MAX_PATH);
				SendEvent(XM_SERVERMSG, XM_SMU_LOGIN_ERROR, (LPARAM)mLoginMsg);
			}
		}
		else
		{
			//reset reconnect data
			mRcExpectDead = false;
			mRcAttempts = 0;

			//login received
			SendEvent(XM_SERVERMSG, XM_SMU_LOGIN_RECEIVED, NULL);

			//update our session based on login message
			CGUID sid = msg->GetField("session")->GetValue(false);
			mServer->SetSessionId(sid);

			//get query limiters
			mLimiterMaxIndex = atoi(msg->GetField("limitindex")->GetValue(false));
			mLimiterMaxFilter = atoi(msg->GetField("limitfilter")->GetValue(false));
			
			//get xml of listing
			bool full = (strcmp(msg->GetField("listing")->GetValue(false), "full")==0)?true:false;
			if (!SendListing(full))
			{
				SendEvent(XM_SERVERMSG, XM_SMU_LOGIN_ERROR, (LPARAM)"Failed to build file listing.");
			}

			//autoupdate?
			if (msg->HasField("au/version"))
			{
				//read fields
				mAuAvailable = true;
				strncpy(mAuVersion, msg->GetField("au/version")->GetValue(false), MAX_PATH);
				mAuRequired =
					(stricmp(msg->GetField("au/required")->GetValue(false), "true")==0);

				//send event
				SendEvent(XM_SERVERMSG, XM_SMU_AU_AVAILABLE, (LPARAM)NULL);
			}
		}
	}
	else if (stricmp(msg->GetFor(false), XMMSG_MOTD)==0)
	{
		//free any existing stuff
		if (mMotdType)
			free(mMotdType);
		if (mMotdMsg)
			free(mMotdType);
		if (mMotdQuestion)
			free(mMotdQuestion);
		if (mMotdChoices)
			free(mMotdChoices);

		//copy into vars
		mMotdType = msg->GetField("type")->GetValue(true);
		mMotdMsg = msg->GetField("message")->GetValue(true);
		mMotdQuestion = msg->GetField("question")->GetValue(true);
		mMotdChoices = msg->GetField("choices")->GetValue(true);

		//send update
		mMotdNew = true;
		SendEvent(XM_SERVERMSG, XM_SMU_MOTD_RECEIVED, NULL);
	}
	else if (stricmp(msg->GetFor(false), XMMSG_LISTING)==0)
	{
		//server has sent us a listing, we only send an update
		//to the window that last asked for a listing.
		Lock();
		if (!mLoginCanceled)
		{
			if (mListingWindow)
			{
				::PostMessage(mListingWindow, XM_SERVERMSG, XM_SMU_LISTING_RECEIVE, (LPARAM)msg->GetMediaListing());
				mListingWindow = NULL;
			}
		}
		Unlock();
	}
	else if (stricmp(msg->GetFor(false), XMMSG_QUERY)==0)
	{
		//were we looking for a query?
		Lock();
		if (mQueryRunning)
		{
			//extract the query response
			resp = msg->GetQueryResponse();
			if (!resp)
			{
				//no response!
				SendEvent(XM_SERVERMSG, XM_SMU_QUERY_ERROR, NULL);
				mQueryRunning = false;
			}
			else
			{
				//assign new response
				if (mQueryResponse) {
					mQueryResponse->Release();
				}
				mQueryResponse = resp;
				mQueryRunning = false;

				//update ui
				SendEvent(XM_SERVERMSG, XM_SMU_QUERY_FINISH, NULL);
			}
		}
		Unlock();
	}
	else if (stricmp(msg->GetFor(false), XMMSG_AU)==0)
	{
		//save the file
		FILE *f = fopen("amsupdate.exe", "wb");
		if (f)
		{
			fwrite(msg->GetBinaryDataBuffer(), msg->GetBinarySize(), 1, f);
			fclose(f);
		}
		f = NULL;

		//spawn new process
		PROCESS_INFORMATION pi;
		STARTUPINFO si;
		ZeroMemory(&si, sizeof(si));
		si.cb = sizeof(si);
		CreateProcess("amsupdate.exe", NULL, NULL, NULL, FALSE, 0, NULL, NULL, &si, &pi);
		if (pi.hProcess)
			CloseHandle(pi.hProcess);
		if (pi.hThread)
			CloseHandle(pi.hThread);

		//au is complete
		mAuComplete = true;
		SendEvent(XM_SERVERMSG, XM_SMU_AU_COMPLETE, NULL);
	}
	else
	{
		//unknown message type
		ses->Close();
	}

	//always delete msg
	delete msg;
}

void CXMServerManager::OnMsgSent(CXMSession *ses, CXMMessage *msg)
{
	//what type of message?
	if (stricmp(msg->GetFor(false), XMMSG_LOGIN)==0)
	{
		//login has been sent
		if (!mLoginCanceled)
		{
			SendEvent(XM_SERVERMSG, XM_SMU_LOGIN_SENT, NULL);
		}
	}
	else if (stricmp(msg->GetFor(false), XMMSG_MOTD)==0)
	{

	}
	else if (stricmp(msg->GetFor(false), XMMSG_QUERY)==0)
	{
		//query message sent.. send ui update
		SendEvent(XM_SERVERMSG, XM_SMU_QUERY_SENT, NULL);
	}
	else if (stricmp(msg->GetFor(false), XMMSG_LISTING)==0)
	{
		//are we logged in yet?
		Lock();
		if (!mLoggedIn)
		{
			//listing was sent.. our login process is finished
			if (!mLoginCanceled)
			{
				mLoggedIn = true;
				mLoginCanceled = false;
				SendEvent(XM_SERVERMSG, XM_SMU_LOGIN_FINISH, NULL);
			}
		}
		Unlock();
	}
	else if (stricmp(msg->GetFor(false), XMMSG_INDEX)==0)
	{

	}
	else if (stricmp(msg->GetFor(false), XMMSG_AU)==0)
	{
		SendEvent(XM_SERVERMSG, XM_SMU_AU_SENT, NULL);
	}
	else
	{
		//unknown message type
		ses->Close();
	}

	//always delete message
	delete msg;
}

void CXMServerManager::OnStateChange(CXMSession *ses, UINT vold, UINT vnew)
{
	//what is the new state?
	switch(vnew)
	{
	case XM_OPEN:
		
		//server is connected
		Lock();
		if (ses==mServer) {
			SendEvent(XM_SERVERMSG, XM_SMU_SERVER_CONNECTED, NULL);
		}
		Unlock();
		break;

	case XM_OPENING:
		
		//server has started connecting
		Lock();
		if (ses==mServer) {
			SendEvent(XM_SERVERMSG, XM_SMU_SERVER_CONNECTING, NULL);
		}
		Unlock();
		break;

	case XM_CLOSING:
		
		//stop all queries, etc
		Lock();
		if (mQueryRunning) {
			SendEvent(XM_SERVERMSG, XM_SMU_QUERY_ERROR, NULL);
			mQueryRunning = false;
		}

		//force logout
		mLoggedIn = false;
		mLoginCanceled = false;

		Unlock();
		break;

	case XM_CLOSED:
		
		//remove our session pointer,
		Lock();
		if (mServer)
		{
			mServer->Release();
			mServer = NULL;
		}
		Unlock();
		
		//server shutdown
		if (mServerShuttingDown)
			SendEvent(XM_SERVERMSG, XM_SMU_SERVER_CLOSED, NULL);
		else
			SendEvent(XM_SERVERMSG, XM_SMU_SERVER_ERROR, NULL);
		mServerShuttingDown = false;
		
		break;
	}
}

// ---------------------------------------------------------------- MOTD

bool CXMServerManager::MotdIsNew()
{
	return mMotdNew;
}

void CXMServerManager::MotdShow()
{
	if(	!mMotdType ||
		!mMotdMsg)
		return;

	//we only support type=="none"
	if (stricmp(mMotdType, "none")==0)
	{
		//just show a msg box
		MessageBox(NULL, mMotdMsg, "Message of the Day", MB_ICONINFORMATION);
	}
	else if (stricmp(mMotdType, "single")==0)
	{

	}
	else if (stricmp(mMotdType, "multi")==0)
	{

	}
	else if (stricmp(mMotdType, "open")==0)
	{

	}
	mMotdNew = false;
}

// ---------------------------------------------------------------- Listing Interface

bool CXMServerManager::SendListing(bool full)
{
	//server must be open
	if (!mServer)
		return false;
	if (mServer->GetState()!=XM_OPEN)
		return false;

	//get xml of listing
	char* xml = dbman()->BuildFileListing(full);
	if (!xml)
	{
		return false;
	}
	else
	{
		//send the listing message
		CXMMessage *msg = new CXMMessage(mServer);
		msg->SetType(XM_REQUEST);
		msg->SetFor("listing");
		msg->SetContentFormat("text/xml");
		msg->GetField("listing")->SetXml(xml, false);
		msg->Send();
	}
	return true;
}

bool CXMServerManager::RequestListing(HWND hwnd)
{
	//server must be open
	if (!mServer)
		return false;
	if (mServer->GetState()!=XM_OPEN)
		return false;

	//store the hwnd
	mListingWindow = hwnd;

	//request all of our media
	CXMMessage *msg = new CXMMessage(mServer);
	msg->SetType(XM_REQUEST);
	msg->SetFor(XMMSG_INDEX);
	msg->SetContentFormat("text/xml");
	msg->GetField("all")->SetValue("true");
	msg->Send();

	return true;
}

bool CXMServerManager::RequestListing(CMD5 md5, HWND hwnd)
{
	//server must be open
	if (!mServer)
		return false;
	if (mServer->GetState()!=XM_OPEN)
		return false;

	//store the hwnd
	mListingWindow = hwnd;

	//request a particular md5
	CXMMessage *msg = new CXMMessage(mServer);
	msg->SetType(XM_REQUEST);
	msg->SetFor(XMMSG_INDEX);
	msg->SetContentFormat("text/xml");
	msg->GetField("md5")->SetValue(md5.GetString());
	msg->Send();

	return true;
}

// ---------------------------------------------------------------- Server Interface

bool CXMServerManager::ServerOpen()
{
	//check state, free current connect
	Lock();
	if (mServer)
	{
		//must be closed
		if (mServer->GetState!=XM_CLOSED) {
			Unlock();
			return false;
		}
	}
	else
	{
		//create new session
		mServer = new CXMSession(mhWnd);
	}

	//start the connection
	if (!mServer->Open())
	{
		//failed to open
		mServer->Release();
		mServer = NULL;
		Unlock();
		return false;
	}

	//updates will be sent from OnStateChange()
	Unlock();
	return true;
}

bool CXMServerManager::ServerClose()
{
	//connection must be open(ing)
	Lock();
	if (mServer)
	{
		if(	(mServer->GetState()==XM_OPEN) ||
			(mServer->GetState()==XM_OPENING))
		{
			//close.. updates handled by state change
			mServer->Close();
			Unlock();
			return true;
		}
	}
	Unlock();
	return false;
}

bool CXMServerManager::ServerIsOpen(bool reconnect)
{
	//check to see if the server is connected
	Lock();
	bool temp;
	if (mServer) 
	{
		temp = ((mServer->GetState()==XM_OPEN));
	}
	else
	{
		temp = false;
	}
	Unlock();

	//if we arent open, and the reconnect flag is set, we
	//should try to auto-reconnect
	if (!temp && reconnect)
	{
		ReconnectTry();
	}

	//done
	return temp;
}

// ---------------------------------------------------------------- Query Interface

bool CXMServerManager::LimiterIndex(CXMIndex *index)
{
	//if limiter is zero, query is open
	if (mLimiterMaxIndex<1)
		return true;

	//check the index against max index
	bool retval = (index->CountFields(true)<=mLimiterMaxIndex);
	if (!retval)
	{
		CString str;
		str.Format(
			"There are too many fields in this query.  You may "
			"specify up to %d fields.  Modify the query and try "
			"again.", mLimiterMaxIndex);
		AfxMessageBox(str, MB_ICONERROR);
	}
	return retval;
}

bool CXMServerManager::LimiterFilter(CXMIndex *filter)
{
	//if limiter is zero, query is open
	if (mLimiterMaxFilter<1)
		return true;

	//check the filter against max filter
	bool retval = (filter->CountFields(false)<=mLimiterMaxFilter);
	if (!retval)
	{
		CString str;
		str.Format(
			"There are too many fields in the filter.  You may "
			"specify up to %d fields, NOT INCLUDING THE CATAGORY. "
			"You may filter by as many fields in the catagory as you "
			"like.  Modify the query and try again.", mLimiterMaxFilter);
		AfxMessageBox(str, MB_ICONERROR);
	}
	return retval;
}

bool CXMServerManager::QueryBegin(CXMQuery *query)
{
	//check state
	Lock();
	if (mQueryRunning) {
		Unlock();
		return false;
	}
	if (!mServer) {
		Unlock();
		return false;
	}

	//check the new query
	if (!LimiterIndex(&query->mQuery) ||
		!LimiterIndex(&query->mRejection))
	{
		return false;
	}

	//store our new query
	query->AddRef();
	if (mQuery)
		mQuery->Release();
	mQuery = query;

	//NOTE: addref and release MUST BE IN THAT ORDER, since they might be
	//the same object, if you release first, it could get deleted

	//send the query message
	char* str = NULL;
	if (FAILED(query->ToXmlString(&str))) {
		mQuery = NULL;
		Unlock();
		return false;
	}
	CXMMessage *msg = new CXMMessage(mServer);
	msg->SetType(XM_REQUEST);
	msg->SetFor("query");
	msg->SetContentFormat("text/xml");
	msg->GetField("query")->SetXml(str, false);
	msg->Send();	

	//send update
	SendEvent(XM_SERVERMSG, XM_SMU_QUERY_BEGIN, NULL);

	//clear all current thumbnail downloads.. i.e., the last query
	cm()->ClearThumbnailDownloads();

	//success
	mQueryRunning = true;
	Unlock();
	return true;
}

bool CXMServerManager::QueryCancel()
{
	//check state
	Lock();
	if (!mQueryRunning) {
		Unlock();
		return false;
	}

	//todo: send cancel msg to server
	mQueryRunning = false;

	//send ui update
	SendEvent(XM_SERVERMSG, XM_SMU_QUERY_CANCEL, NULL);

	Unlock();
	return true;
}

bool CXMServerManager::QueryIsRunning()
{
	Lock();
	bool temp = mQueryRunning;
	Unlock();
	return temp;
}

CXMQuery* CXMServerManager::QueryGet()
{
	//NOT THREADSAFE
	mQuery->AddRef();
	return mQuery;
}

CXMQueryResponse* CXMServerManager::QueryGetResponse()
{
	//NOT THREADSAFE
	mQueryResponse->AddRef();
	return mQueryResponse;
}

// ---------------------------------------------------------------- Login Interface

bool CXMServerManager::LoginUI()
{
	//true: logon succesfull
	//false: error, or canceled before login
	return (CLoginDialog().DoModal()==IDOK);
}

bool CXMServerManager::Login(const char *username, const char *password)
{
	//must not be logged in
	Lock();
	if (mLoggedIn) {
		Unlock();
		return false;
	}

	//server must be up
	if (!mServer) {
		Unlock();
		return false;
	}
	if (mServer->GetState()!=XM_OPEN) {
		Unlock();
		return false;
	}

	//store the username and password
	if (mUsername) {
		free(mUsername);
	}
	if (mPassword) {
		free(mPassword);
	}
	mUsername = strdup(username);
	mPassword = strdup(password);

	//send the login message
	CXMMessage *msg = new CXMMessage(mServer);
	msg->SetType(XM_REQUEST);
	msg->SetFor("login");
	msg->SetContentFormat("text/xml");
	msg->GetField("datarate")->SetValue(config()->GetField(FIELD_NET_DATARATE), false);
	msg->GetField("system")->SetValue(config()->RegGetSystemID().GetString(), true);
	msg->GetField("username")->SetValue(mUsername, true);
	msg->GetField("password")->SetValue(mPassword, true);
	msg->GetField("version")->SetValue(app()->Version(), true);
	if (!msg->Send()) {
		delete msg;
		Unlock();
		return false;
	}

	//login has begun
	SendEvent(XM_SERVERMSG, XM_SMU_LOGIN_BEGIN, NULL);

	Unlock();
	return true;
}

bool CXMServerManager::LoginCancel()
{
	//set canceled flag
	mLoginCanceled = true;		

	//kill server connection
	mServer->Close();
	return true;
}

bool CXMServerManager::LoginIsLoggedIn()
{
	Lock();
	bool temp = mLoggedIn;
	Unlock();
	return temp;
}

CMD5 CXMServerManager::LoginGetSession()
{
	Lock();
	CMD5 temp = mSessionID;
	Unlock();
	return temp;
}

char* CXMServerManager::LoginGetUsername()
{
	Lock();
	char* temp = mUsername;
	Unlock();
	return temp;
}

char* CXMServerManager::LoginGetPassword()
{
	Lock();
	char* temp = mPassword;
	Unlock();
	return temp;
}

// -------------------------------------------------------------------------------- Auto Update

class CAutoUpdate : public CDialog
{
public:

	//construction
	CAutoUpdate(CWnd* pParent = NULL) :
	  CDialog(IDD_AU, pParent)
	{
		  mVersion = "";
		  mRequired = false;
	}

	BOOL OnInitDialog()
	{
		//get ctrl refs
		UpdateData(false);

		//hook into server manager
		sm()->Subscribe(m_hWnd);
		sm()->HookProgress(m_hWnd);

		//set initial control states
		mStatus.SetWindowText("");
		mRate.SetWindowText("");
		mProgress.SetRange(0, 1);
		mProgress.SetPos(0);

		//ask the server for the file
		if (!sm()->AuRequest())
		{
			AfxMessageBox("Error requesting auto-update file.", MB_ICONERROR);
			EndDialog(IDCANCEL);
			return FALSE;
		}

		//record time
		mtimeStart = CTime::GetCurrentTime();
		
		return FALSE;
	}

	//parameters
	CString mVersion;
	bool mRequired;

private:
	
	//misc
	CTime mtimeStart;

	//controls
	CStatic mStatus;
	CStatic mRate;
	CProgressCtrl mProgress;
	CButton mOK;
	CButton mCancel;
	void DoDataExchange(CDataExchange *pDX)
	{
		DDX_Control(pDX, IDC_STATUS, mStatus);
		DDX_Control(pDX, IDC_RATE, mRate);
		DDX_Control(pDX, IDC_PROGRESS, mProgress);
		DDX_Control(pDX, IDOK, mOK);
		DDX_Control(pDX, IDCANCEL, mCancel);
	}

	//messages
	DECLARE_MESSAGE_MAP();
	LRESULT OnServerMessage(WPARAM wParam, LPARAM lParam)
	{
		static int osmState = 0;
		switch(wParam)
		{
		case XM_SMU_AU_SENDING:
			mStatus.SetWindowText("Requesting new version...");
			break;
		
		case XM_SMU_AU_SENT:
			mStatus.SetWindowText("Waiting for response...");
			break;
		
		case XM_SMU_AU_RECEIVING:
			if (osmState!=XM_SMU_AU_RECEIVING)
				mStatus.SetWindowText("Receiving file...");
			break;
		
		case XM_SMU_AU_COMPLETE:
			
			//ok the dialog
			OnOK();
			break;
		}
		osmState = wParam;
		return 0;
	}
	LRESULT OnProgress(WPARAM wParam, LPARAM lParam)
	{
		//modify the progress bar and rate text
		DWORD dwTotal, dwCurrent;
		CXMSession *ses = (CXMSession*)lParam;
		ses->GetBinaryProgress(&dwTotal, &dwCurrent);
		return 0;
	}

	//dialog overrides
	void OnOK()
	{
		//unhook from server manager
		sm()->UnSubscribe(m_hWnd);
		sm()->HookProgress(NULL);

		CDialog::OnOK();
	}
	void OnCancel()
	{
		//unhook from server manager
		sm()->UnSubscribe(m_hWnd);
		sm()->HookProgress(NULL);

		CDialog::OnCancel();
	}
};

BEGIN_MESSAGE_MAP(CAutoUpdate, CDialog)
	ON_MESSAGE(XM_SERVERMSG, OnServerMessage)
	ON_MESSAGE(XM_PROGRESS, OnProgress)
END_MESSAGE_MAP()

void CXMServerManager::HookProgress(HWND hWnd)
{
	mwndProgress = hWnd;
}

bool CXMServerManager::AuRequest()
{
	//server open?
	if (!mServer || mServer->GetState()!=XM_OPEN)
		return false;

	//create message
	CXMMessage *msg = new CXMMessage(mServer);
	msg->SetContentFormat("text/xml");
	msg->SetFor(XMMSG_AU);
	msg->SetType(XM_REQUEST);

	//send the msg
	SendEvent(XM_SERVERMSG, XM_SMU_AU_SENDING, NULL);
	return msg->Send();
}

bool CXMServerManager::AuAvailable()
{
	return mAuAvailable;
}

bool CXMServerManager::AuComplete()
{
	return mAuComplete;
}

bool CXMServerManager::AuGo(CWnd *wnd)
{
	//build message
	char msg[MAX_PATH+1];
	if (mAuRequired)
	{
		_snprintf(msg, MAX_PATH, "You must upgrade to the latest version of AMS (%s) before you can log in.  Automatically upgrade now?", mLoginMsg);
	}
	else
	{
		_snprintf(msg, MAX_PATH, "There is a new version of AMS available (%s), would you like to upgrade now?", mLoginMsg);
	}

	//ask if we want to upgrade
	if (::MessageBox(wnd->m_hWnd, msg, "Auto Upgrade", MB_ICONQUESTION | MB_YESNO) == IDNO)
	{
		return false;
	}
	
	//show the dialog
	CAutoUpdate dlg(wnd);
	mAuComplete = (dlg.DoModal()==IDOK);
	return true;
}

// -------------------------------------------------------------------------------- Login Dialog


//login state constants
#define LDS_INPUT			 0
#define LDS_CONNECT			10
#define LDS_LOGINSEND		20
#define LDS_LOGINWAIT		25
#define LDS_LISTINGBUILD	30
#define LDS_LISTINGSEND		35
#define LDS_END				40
#define LDS_FAIL			100

BEGIN_MESSAGE_MAP(CLoginDialog, CDialog)
	ON_BN_CLICKED(IDC_LOGINBUTTON, OnClickedLoginbutton)
	ON_WM_TIMER()
	ON_WM_DESTROY()
	ON_WM_PAINT()
	ON_MESSAGE(XM_SERVERMSG, OnServerMsg)
	ON_MESSAGE(XM_STATUSENTRY, OnStatusEntry)
	ON_WM_CTLCOLOR()
	ON_COMMAND(ID_OPTIONS, OnOptions)
	ON_COMMAND(ID_DETAILS, OnDetails)
	ON_CONTROL(STN_CLICKED, IDC_SIGNUP, OnSignup)
END_MESSAGE_MAP()

CLoginDialog::CLoginDialog(CWnd* pParent /*=NULL*/)
	: CDialog(IDD_LOGIN, (CWnd*)pParent)
{
	m_pSession = NULL;
	m_nState = LDS_INPUT;
	m_bShowingQuestion = false;

	m_bWaitingForLogin = false;
	m_bWorkerDone = false;
	m_bWorkerFail = false;
	m_bCancel = false;
	m_bDetails = false;

	m_hThread = NULL;
	m_dwThreadId = 0;
	InitializeCriticalSection(&m_cs);
}

void CLoginDialog::OnSignup()
{
	//open a web browser window with the help
	UINT retval = (UINT)ShellExecute(
							::GetDesktopWindow(),
							"open",
							"iexplore",
							"http://www.adultmediaswapper.com/download/new.ams",
							"",
							SW_SHOWDEFAULT);
	if (retval<=32)
	{
		//error
		ASSERT(FALSE);
	}
}

CLoginDialog::~CLoginDialog()
{
	//release the thread handle?
	if (m_hThread)
	{
		TerminateThread(m_hThread, 1);
		CloseHandle(m_hThread);
	}
	DeleteCriticalSection(&m_cs);
}

BOOL CLoginDialog::PreCreateWindow(CREATESTRUCT &cs)
{
	if (!CDialog::PreCreateWindow(cs))
		return FALSE;
	cs.cy = 250;
	return TRUE;
}

void CLoginDialog::OnOptions()
{
	config()->DoPrefs(false);
}

void CLoginDialog::OnDestroy()
{
	//remove server man hook
	sm()->UnSubscribe(m_hWnd);
}

void CLoginDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LOGIN, m_editLogin);
	DDX_Control(pDX, IDC_PASSWORD, m_editPassword);
	DDX_Control(pDX, IDC_STATUS, m_editStatus);
	DDX_Control(pDX, IDC_LOGINBUTTON, m_buttonOK);
	DDX_Control(pDX, IDCANCEL, m_buttonCancel);
	DDX_Control(pDX, ID_OPTIONS, m_buttonOptions);
	DDX_Control(pDX, ID_DETAILS, m_buttonDetails);
	DDX_Control(pDX, IDC_DETAILS, m_listDetails);
	DDX_Text(pDX, IDC_LOGIN, m_strUsername);
	DDX_Text(pDX, IDC_PASSWORD, m_strPassword);
}

LRESULT CLoginDialog::OnServerMsg(WPARAM wParam, LPARAM lParam)
{
	switch (wParam)
	{
	case XM_SMU_LOGIN_BEGIN:
		SetState(LDS_LOGINSEND);
		break;

	case XM_SMU_LOGIN_SENT:
		SetState(LDS_LOGINWAIT);
		break;

	case XM_SMU_LOGIN_RECEIVED:
		SetState(LDS_LISTINGSEND);
		break;

	case XM_SMU_LOGIN_LISTING:
		break;

	case XM_SMU_LOGIN_FINISH:
		SetState(LDS_END);
		break;

	case XM_SMU_LOGIN_CANCEL:
		SetState(LDS_INPUT);
		break;

	case XM_SMU_LOGIN_ERROR:
	case XM_SMU_LOGIN_ERROR_UPGRADE:

		//kill any timers, otherwise "server hasn't responded" messages
		//will pop up while the error dialog is up
		KillTimer(m_nState);

		//if its a upgrade error, AuGo will handle it
		if (wParam == XM_SMU_LOGIN_ERROR)
		{
			::MessageBox(m_hWnd, (char*)lParam, "Login Error", MB_ICONERROR|MB_OK);
			SetState(LDS_FAIL);
		}
		else
		{
			SetState(LDS_END);
		}
		break;

	case XM_SMU_SERVER_CONNECTING:
		SetState(LDS_CONNECT);
		break;

	case XM_SMU_SERVER_CONNECTED:

		//begin actual login
		DoLogin();
		break;

	case XM_SMU_SERVER_ERROR:
		AfxMessageBox("Error connecting to server.");
		SetState(LDS_FAIL);
		break;

	case XM_SMU_SERVER_CLOSED:
		
		//are we waiting for this?
		if (m_bCancel)
		{
			CDialog::OnCancel();
		}
		break;

	};

	return 0;
}

BOOL CLoginDialog::OnInitDialog(void)
{
	CDialog::OnInitDialog();

	//load the last login
	m_strUsername = config()->GetField(FIELD_LOGIN_LOGINID, false);
	m_strPassword = "";
	UpdateData(FALSE);

	//is there a username already?
	if (m_strUsername.GetLength()>0)
		m_editPassword.SetFocus();
	else
		m_editLogin.SetFocus();

	//hook us into the session manager
	sm()->Subscribe(m_hWnd);

	//set the size
	CRect rw;
	GetWindowRect(rw);
	rw.bottom = rw.top + 250;
	MoveWindow(rw);

	//create underlined font
	CFont font, ansi;
	LOGFONTA lfa;
	ansi.CreateStockObject(ANSI_VAR_FONT);
	ansi.GetLogFont(&lfa);
	lfa.lfUnderline = TRUE;
	lfa.lfWeight = FW_BOLD;
	font.CreateFontIndirect(&lfa);
	GetDlgItem(IDC_SIGNUP)->SetFont(&font);
	font.Detach();

	//are we doing auto-login?
	if (config()->GetFieldBool(FIELD_LOGIN_AUTO_ENABLE))
	{
		//read the username and password
		m_strUsername = config()->GetField(FIELD_LOGIN_AUTO_USERNAME, false);
		m_strPassword = config()->GetField(FIELD_LOGIN_AUTO_PASSWORD, false);

		//start login
		DoLogin();
	}

	//start the worker thread
	m_hThread = CreateThread(NULL, NULL, LoginThreadProc, (LPVOID)this, NULL, &m_dwThreadId);
	if (!m_hThread)
	{
		AfxMessageBox("Error starting worker thread!");
		EndDialog(FALSE);
		return FALSE;
	}

	return FALSE;  // return TRUE unless you set the focus to a control
}

void CLoginDialog::OnOK()
{
	CDialog::OnOK();
}

void CLoginDialog::OnCancel()
{
	//are we loggging in?
	m_bCancel = true;
	if(m_nState==LDS_INPUT)
	{
		//we are idly waiting for login, exit
		//as soon as the worker thread exits normally
		if (m_hThread)
		{
			WaitForSingleObject(m_hThread, 1000);
		}

		CDialog::OnCancel();
	}
	else
	{
		//worker thread has already exited.. kill the connection
		//on the server, we will exit when the server connection closes
		if (sm()->ServerIsOpen())
		{
			if (!sm()->LoginCancel()) {
				CDialog::OnCancel();
			}
		}
		else
		{
			CDialog::OnCancel();
		}
	}
}

void CLoginDialog::OnClickedLoginbutton()
{
	//get data from ui
	UpdateData(TRUE);

	//start the login
	DoLogin();
}

void CLoginDialog::DoLogin()
{
	//has the worker thread exited?
	if (!m_bWorkerDone)
	{
		//wait until login is finished
		m_bWaitingForLogin = true;
		StatusEntry("Waiting for database to finish loading...", false);
		return;
	}

	//start login
	if(sm()->ServerIsOpen())
	{
		//send the login message
		SetState(LDS_LOGINSEND);
		if (!sm()->Login(m_strUsername, m_strPassword)) {
			AfxMessageBox("Error starting login.");
		}
	}
	else
	{
		//open connection first.. login will be initiated when
		//state change is detected
		if (!sm()->ServerOpen()) {
			AfxMessageBox("Error connecting to server.");
		}
	}
}

void CLoginDialog::SetState(BYTE newstate)
{
	//kill current timer
	KillTimer(m_nState);

	//proccessing before moving out of a state
	switch(m_nState) {
		case LDS_INPUT:

			m_buttonOK.EnableWindow(FALSE);
			m_editLogin.EnableWindow(FALSE);
			m_editPassword.EnableWindow(FALSE);
			break;

		default:
			break;
	}

	//store new state, set timer if needed, set message
	m_nState = newstate;
	switch(newstate) {
		case LDS_INPUT:
			m_buttonOK.EnableWindow(TRUE);
			m_editLogin.EnableWindow(TRUE);
			m_editPassword.EnableWindow(TRUE);
			StatusEntry("Enter your login id and password.", false);
			break;
		case LDS_CONNECT:
			StatusEntry("Connecting to sever...", false);
			this->SetTimer(LDS_CONNECT, 1000*10, NULL);
			break;
		case LDS_LOGINSEND:
			StatusEntry("Logging in...", false);
			this->SetTimer(LDS_LOGINSEND, 1000*10, NULL);
			break;
		case LDS_LOGINWAIT:
			StatusEntry("Waiting for response...", false);
			this->SetTimer(LDS_LOGINWAIT, 1000*10, NULL);
			break;
		/*
		case LDS_LISTINGBUILD:
			m_editStatus.SetWindowText("Building file list...");
			break;
		*/
		case LDS_LISTINGSEND:
			StatusEntry("Sending file list...", false);
			break;
		case LDS_END:
			StatusEntry("Login complete.", false);
			break;
		case LDS_FAIL:
			StatusEntry("Login failed.", false);
			SetState(LDS_INPUT);
			break;
		default:
			StatusEntry("Unkown state.", false);
	}

	//if end, tear down dialog
	if (newstate==LDS_END)
	{
		//login successfull.. check for updates
		if (sm()->AuAvailable())
		{
			if (!sm()->AuGo(this))
			{
				//failed, use cancel instead
				OnCancel();
				return;
			}
		}
		//NOTE: cwinapp::initapp() will know if autocomplete needs to restsart

		OnOK();
	}
}

void CLoginDialog::OnTimer(UINT nIDEvent)
{
	//only show one dialog at a time
	if (m_bShowingQuestion)
		return;

	//turn off current timer
	KillTimer(nIDEvent);

	//timeout occured--ask user for next step
	char* str = NULL;
	switch (nIDEvent) {

	case LDS_CONNECT:		//connect
		str = "The server has not connected.  Do you wish to continue waiting?";
		break;

	case LDS_LOGINSEND:
		str = "The login request has not been sent.  Do you wish to continue waiting?";
		break;

	case LDS_LOGINWAIT:		//login
		str = "The server has not responded to the login request.  Do you wish to continue waiting?";
		break;

	case LDS_LISTINGSEND:		//listing
		str = "The server has not responded.  Do you wish to continue waiting?";
		break;

	default:
		break;		
	}
	int retval;
	if (str) {
		
		retval = AfxMessageBox(str, MB_YESNO | MB_ICONQUESTION, 0);
		if (retval == IDNO) 
			OnCancel();
	}

	//restart timer
	if (nIDEvent==LDS_CONNECT || nIDEvent==LDS_LOGINSEND || nIDEvent==LDS_LOGINWAIT)
	{
		SetTimer(nIDEvent, 1000*20, NULL);
	}

	CDialog::OnTimer(nIDEvent);
}

HBRUSH CLoginDialog::OnCtlColor(CDC *pDC, CWnd *pWnd, UINT nCtlColor)
{
	//let dialog go first
	HBRUSH hb = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);

	//Everything except username, passwords, and buttons gets
	//black back, white text
	if (	(nCtlColor==CTLCOLOR_STATIC) ||
			(nCtlColor==CTLCOLOR_DLG) ||
			(pWnd->GetDlgCtrlID()==IDC_STATUS))
	{
		pDC->SetTextColor(RGB(255,255,255));
		pDC->SetBkColor(RGB(0,0,0));
		pDC->SetBkMode(TRANSPARENT);
		hb = (HBRUSH)::GetStockObject(BLACK_BRUSH);
	}

	if (pWnd->GetDlgCtrlID()==IDC_SIGNUP)
	{
		pDC->SetTextColor(RGB(0,0,255));
	}


	return hb;
}

void CLoginDialog::OnDetails()
{
	CRect rw;
	GetWindowRect(rw);
	m_bDetails = !m_bDetails;
	if (m_bDetails)
	{
		rw.bottom = rw.top + 371;
		MoveWindow(rw);
		m_buttonDetails.SetWindowText("Details <<");
	}
	else
	{
		rw.bottom = rw.top + 250;
		MoveWindow(rw);
		m_buttonDetails.SetWindowText("Details >>");
	}
}

void CLoginDialog::StatusEntry(const char* text, bool detail)
{
	//send message to ourself
	PostMessage(XM_STATUSENTRY, detail?TRUE:FALSE, (LPARAM)strdup(text));
}

LRESULT CLoginDialog::OnStatusEntry(WPARAM wp, LPARAM lp)
{
	char *text = (char*)lp;
	bool detail = wp?true:false;
	if (m_editStatus.m_hWnd)
	{
		if (!detail)
		{
			m_editStatus.SetWindowText(text);
		}
		m_listDetails.SetCurSel(
				m_listDetails.InsertString(
					m_listDetails.GetCount(),
					text));
	}
	free(text);
	return 0;
}

bool CLoginDialog::StatusGetCanceled()
{
	return m_bCancel;
}

void CLoginDialog::StatusSetWorkerDone()
{
	m_bWorkerDone = true;
	CloseHandle(m_hThread);
	m_hThread = NULL;
	m_dwThreadId = 0;

	//were we waiting for this?
	if (m_bWaitingForLogin)
		DoLogin();
	else
		StatusEntry("Enter login id and password.", false);

	//NOTE: DoLogin DOES work from other threads.
}

void CLoginDialog::StatusSetWorkerFail()
{
	m_bWorkerFail = true;
	CloseHandle(m_hThread);
	m_hThread = NULL;
	m_dwThreadId = 0;
}

// ----------------------------------------------------------------- LOGIN WORKER

class CXMLoginWorker : public IXMDBManagerCallback
{
public:

	DWORD Alpha(CLoginDialog *dlg)
	{
		//have we already initialied?
		if (app()->m_bHasInit)
		{
			dlg->StatusSetWorkerDone();
			return 0;
		}

		//load the database
		mdlg = dlg;
		mdlg->StatusEntry("Loading database...", false);
		if (!dbman()->DatabaseStartup())
		{
			mdlg->StatusEntry("Error loading database file.", false);
			mdlg->StatusSetWorkerFail();
			return 0;
		}

		//scan for files
		if (config()->GetFieldBool(FIELD_DB_SHARE_ENABLE))
		{
			dbman()->SetCallback((IXMDBManagerCallback*)this);
			if (!dbman()->ScanDirectory(NULL))
			{
				mdlg->StatusEntry("Error scanning shared folder.", false);
				mdlg->StatusSetWorkerFail();
				return 0;
			}
		}

		//complete
		mdlg->StatusSetWorkerDone();
		app()->m_bHasInit = true;
		return 0;
	}

public:	//dbman callback
	
	void OnBeginScan()
	{
		mdlg->StatusEntry("Scanning for new files...", false);
	}
	void OnEndScan()
	{
	}
	void OnScanDir(const char *path)
	{
		CString str;
		str.Format("Scanning folder: %s", path);
		mdlg->StatusEntry(str, true);
	}
	void OnProcess()
	{
		if (mdlg->StatusGetCanceled()) {
			dbman()->CancelScan();
		}
	}

	//called during scan and watch
	virtual bool OnFileFound(const char* path, CMD5* md5)
	{
		CString str;
		str.Format("Added file: %s", path);
		mdlg->StatusEntry(str, true);
		return true;
	}
	virtual bool OnFileRestored(CXMDBFile* file)
	{
		return true;
	}
	virtual bool OnFileRemoved(CXMDBFile* file)
	{
		CString str;
		str.Format("Removed file: %s", file->GetPath());
		mdlg->StatusEntry(str, true);
		return true;
	}

	//called after an xmfile is either reomved or added
	virtual void AfterFileAdded(CXMDBFile *file)
	{
	}
	virtual void AfterFileRemoved(CXMDBFile *file)
	{
	}
	virtual void OnFileAddError(const char* path, CMD5* md5)
	{
		CString str;
		str.Format("Error Adding File: %s", path);
		mdlg->StatusEntry(str, true);
	}
	virtual void OnFileRemoveError(CXMDBFile *file)
	{
		CString str;
		str.Format("Error Removing File: %s", file->GetPath());
		mdlg->StatusEntry(str, true);
	}

private:
	CLoginDialog *mdlg;
};

DWORD WINAPI LoginThreadProc(LPVOID lpParameter)
{
	//cast parameter to login dialog
	CLoginDialog *dlg = (CLoginDialog*)lpParameter;

	//start the worker
	CXMLoginWorker worker;
	return worker.Alpha(dlg);
}


// ------------------------------------------------------------------ Reconnect Implementation

class CReconnectDialog : public CDialog
{
public:

	//construciton
	CReconnectDialog(CWnd* pwndParent)
		: CDialog(IDD_RECONNECT, pwndParent)
	{
		mConnected = false;
	}
	~CReconnectDialog()
	{
	}

	//init
	BOOL OnInitDialog()
	{
		//begin the connection
		if (!sm()->ServerOpen())
		{
			CDialog::OnCancel();
		}

		//success
		return FALSE;
	}

	//did we succesfully connect?
	bool mConnected;

private:

	//client pipeline message
	LRESULT OnServerMessage(WPARAM wp, LPARAM lp)
	{
		switch (wp)
		{
		case XM_SMU_SERVER_ERROR:
		case XM_SMU_SERVER_CLOSED:
		case XM_SMU_LOGIN_CANCEL:
		case XM_SMU_LOGIN_ERROR:
		case XM_SMU_LOGIN_ERROR_UPGRADE:
			
			//failure
			mConnected = false;
			CDialog::OnCancel();
			break;

		case XM_SMU_LOGIN_FINISH:
			
			//success
			mConnected = true;
			CDialog::OnOK();
			break;

		case XM_SMU_SERVER_CONNECTED:

			//next stage..
			sm()->Login(NULL, NULL);
			break;
		}

		return 0;
	}

	//cancel
	void OnCancel()
	{
		//cancel the login
		if (!sm()->LoginCancel())
		{
			CDialog::OnCancel();
		}
	}

	DECLARE_MESSAGE_MAP()
};

BEGIN_MESSAGE_MAP(CReconnectDialog, CDialog)
	ON_MESSAGE(XM_SERVERMSG, OnServerMessage)
END_MESSAGE_MAP()

//return val: if true, connection now open
bool CXMServerManager::ReconnectTry()
{
	//were we expecting to be down?
	if (mRcExpectDead)
		return false;

	//have we already tried?
	if (mRcAttempts > 0)
		return false;

	//start the reconnect
	CReconnectDialog dlg(NULL);
	dlg.DoModal();
	mRcAttempts += dlg.mConnected?0:1;

	//success
	return dlg.mConnected;
}

