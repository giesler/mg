//----------------------------------------------------------------------------------------
// XMPIPLINE.H																  XMPIPLINE.H
//----------------------------------------------------------------------------------------

/*

SUMMARY: The pipeline manages the state of various
activities that involve sending data accross the
network.  Examples: query processing, file transfer.

The pipeline provides thread-safe, high level interfaces
for higher level modules to manipulate query, file xfer
or other base services.  The pipeline send all upstream
event notification via win32 messaging, and can broadcast
events to multiple targets.

*/

#include "xmdb.h"

/* ----------------------------------------------------------------------- XMPiplineBase

	XMPiplineBase: Abstract class.  Shared code for client
	manager and server manager.  Handles subscriber lists for
	updates, threading, and windowing.

   --------------------------------------------------------------------- */

#define PIPELINEWNDCLASS "CXMPipelineBase_WndClass"
DWORD WINAPI PipelineThreadProc(LPVOID lpParameter);

class CXMPipelineBase
{
friend DWORD WINAPI PipelineThreadProc(LPVOID lpParameter);
public:

	//register wnd class
	static bool InitOnce();			//call during application startup

	//threading interface
	void	Start();				//start the thread and msg pump
	void	Stop();					//stop both thread and msg pump
	HWND	GetHWND();				//return the hwnd of the msg pump
	DWORD	GetThreadID();			//return the threadid of the msg pump thread
	inline void Lock() {
		EnterCriticalSection(&mcsLock);
	};
	inline void Unlock() {
		LeaveCriticalSection(&mcsLock);
	};

	//event subscriber interface
	void Subscribe(HWND target, bool secondary = false);
	void UnSubscribe(HWND target);
	
protected:

	//construction
	CXMPipelineBase();
	virtual ~CXMPipelineBase();

	//subscriber implementation
	CPtrArray mSubscribers;
	CPtrArray mSecondary;
	void SendEvent(UINT msg, WPARAM wp, LPARAM lp);

	//threading implementation
	HWND mhWnd;
	HANDLE mhThread;
	DWORD mdwThreadID;
	CRITICAL_SECTION mcsLock;
	DWORD Alpha();

	//session management
	CXMSession* OpenSession(char* address, UINT port);

	//overrides for implementors
	virtual bool OnInitialize()=0;
	virtual void OnWin32MsgPreview(UINT msg, WPARAM wparam, LPARAM lparam)=0;
	virtual void OnWin32MsgReview(UINT msg, WPARAM wparam, LPARAM lparam)=0;
	virtual void OnMsgReceived(CXMSession *ses, CXMMessage *msg)=0;
	virtual void OnMsgSent(CXMSession *ses, CXMMessage *msg)=0;
	virtual void OnStateChange(CXMSession *ses, UINT vold, UINT vnew)=0;

};

/* ----------------------------------------------------------------------- XMClientManager

	XMClientManager: 1 global instance.  Runs a message pump
	in its own thread, hosts hwnd for session manager.  Receives
	all messages from client connection, inbound or out.  Keeps
	queues of file uploads and downloads, and sends updates to 
	a list of 'subscribers'.  Also, responds to pings.

   --------------------------------------------------------------------- */

//state of each upload slot
#define USS_OPEN			0
#define USS_WAITING			1
#define USS_SENDINGFILE		2
#define USS_SENDINGERROR	3
#define USS_CLOSING			4

//state of each download slot
#define DSS_OPEN			0
#define DSS_WAITING			1
#define DSS_RECEIVING		2
#define DSS_CLOSING			3


//#define XM_CLIENTMSG	XM_MSGBASE+4
//message from the client pipeline
//	wparam: message
//	lparam: pointer to ticket

enum XM_CMU {

	XM_CMU_QUEUE_ADD,
	XM_CMU_QUEUE_REMOVE,

	XM_CMU_DOWNLOAD_START,
	XM_CMU_DOWNLOAD_REQUESTING,
	XM_CMU_DOWNLOAD_RECEIVING,
	XM_CMU_DOWNLOAD_ERROR,
	XM_CMU_DOWNLOAD_CANCEL,
	XM_CMU_DOWNLOAD_FINISH,

	XM_CMU_COMPLETED_ADD,
	XM_CMU_COMPLETED_REMOVE,

	XM_CMU_UPLOAD_START,
	XM_CMU_UPLOAD_FINISH

};

class CXMPipelineUpdateTag;

//actual manaager class
class CXMClientManager : public CXMPipelineBase
{
public:

	//construction
	CXMClientManager();
	virtual ~CXMClientManager();

	//various tickets
	struct QueuedFile
	{
		bool mIsThumb;
		DWORD mWidth, mHeight;
		CXMQueryResponseItem *mItem;
	};
	struct CompletedFile
	{
		CMD5 mMD5;
		DWORD mWidth, mHeight;
		BYTE* mBuffer;
		DWORD mBufferSize;
		bool mIsThumb;
		bool mIsError;
		char* mErrorMsg;
	};
	struct DownloadSlot
	{
		bool mIsThumb;
		BYTE mState;
		CXMSession *mSession;
		DWORD mWidth, mHeight;
		BYTE mCurrentHost;
		CXMQueryResponseItem *mItem;
	};
	struct UploadSlot
	{
		BYTE mState;
		CMD5 mId;
		CMD5 mMD5;
		bool mIsThumb;
		CXMSession *mSession;
	};

	//WARNING: The following functions ARE NOT THREADSAFE!
	//Call Lock() and Unlock() manually

	//queue access
	DWORD FindQueuedFile(CXMPipelineUpdateTag* tag);
	DWORD GetQueuedFileCount();
	QueuedFile* GetQueuedFile(DWORD i);
	QueuedFile* GetQueuedFileBuffer();

	//download slot access
	BYTE FindDownloadSlot(CXMPipelineUpdateTag* tag);
	BYTE GetDownloadSlotCount();
	CXMClientManager::DownloadSlot* GetDownloadSlot(BYTE i);
	CXMClientManager::DownloadSlot* GetDownloadSlotBuffer();

	//completed files acces
	DWORD FindCompletedFile(CXMPipelineUpdateTag* tag);
	DWORD GetCompletedFileCount();
	CXMClientManager::CompletedFile* GetCompletedFile(DWORD i);
	CXMClientManager::CompletedFile* GetCompletedFileBuffer();

	//upload slot access
	BYTE FindUploadSlot(CXMPipelineUpdateTag* tag);
	BYTE GetUploadSlotCount();
	CXMClientManager::UploadSlot* GetUploadSlot(BYTE i);
	CXMClientManager::UploadSlot* GetUploadSlotBuffer();

	//operations
	DWORD EnqueueFile(CXMQueryResponseItem* item, bool thumb, DWORD width, DWORD height);
	void FlushQueue();
	void CancelQueuedFile(DWORD i);
	void CancelDownloadingFile(DWORD i);
	void ClearThumbnailDownloads();
	void RemoveCompletedFile(DWORD i);

protected:

	//Misc
	void SendUpdate(WPARAM update, CMD5& md5, DWORD width, DWORD height, bool thumb, BYTE flags);

	//Track Completed Files
	CompletedFile *mCompleted;
	DWORD mCompletedCount;
	DWORD mCompletedSize;
	DWORD AddCompletedFile();

	//Manage Queued Downloads
	QueuedFile *mQueue;
	DWORD mQueueCount;
	DWORD mQueueSize;
	DWORD AddQueuedFile();

	//Manage Downloads
	BYTE mMaxDownloads;
	BYTE mMaxThumbnails;
	DownloadSlot *mDownloads;
	BYTE mDownloadCount;
	void ClearDownload(DWORD i);
	void BeginDownload(DWORD i);
	void OnFileReceived(CXMSession *ses, CXMMessage *msg);
	void OnFileBusy(CXMSession *ses, CXMMessage *msg);
	
	//Manage Uploads
	BYTE mMaxUploads;
	UploadSlot *mUploadSlots;
	BYTE mUploadSlotCount;
	void OnFileRequest(CXMSession *ses, CXMMessage *msg);
	
	//hooks called by the pipeline
	virtual bool OnInitialize();
	virtual void OnWin32MsgPreview(UINT msg, WPARAM wparam, LPARAM lparam);
	virtual void OnWin32MsgReview(UINT msg, WPARAM wparam, LPARAM lparam);
	virtual void OnMsgReceived(CXMSession *ses, CXMMessage *msg);
	virtual void OnMsgSent(CXMSession *ses, CXMMessage *msg);
	virtual void OnStateChange(CXMSession *ses, UINT vold, UINT vnew);
};

//pipeline update flags
#define XMPUF_QFLUSH	1<<0	//item is being removed from the queue because it is about
								//to start downloading
#define XMPUF_RESERVED2	1<<1	
#define XMPUF_RESERVED3	1<<2
#define XMPUF_RESERVED4	1<<3
#define XMPUF_RESERVED5	1<<4
#define XMPUF_RESERVED6	1<<5
#define XMPUF_RESERVED7	1<<6
#define XMPUF_RESERVED8	1<<7

class CXMPipelineUpdateTag
{
friend bool CXMPipelineBase::InitOnce();
friend void CXMClientManager::SendUpdate(WPARAM update, CMD5& md5, DWORD width, DWORD height, bool thumb, BYTE flags);
public:

	//construction
	void AddRef();
	void Release();
	
	//data
	CMD5 md5;
	DWORD width, height;
	bool thumb;
	BYTE flags;

protected:
	CXMPipelineUpdateTag(BYTE RefCount);
	~CXMPipelineUpdateTag();
	BYTE mRefCount;
	static CRITICAL_SECTION mcsSync;
};

/* ----------------------------------------------------------------------- XMServerManager

	XMServerManager: 1 global instance.  Runs a message pump
	in its own thread, handles all incoming and outgoing
	messages to the server.  Manages state for query processing,
	and sends query process updates to the ui.  Also handles
	MOTD messages and corresponding ui updates, as well as
	sending index updates to the server.

   ---------------------------------------------------------------------*/

//#define XM_SERVERMSG	XM_MSGBASE+5
//message from the server pipeline
//	wparam: message
//	lparam: NULL

enum XM_SMU_MESSAGE {
	
XM_SMU_QUERY_BEGIN,
XM_SMU_QUERY_SENT,
XM_SMU_QUERY_CANCEL,
XM_SMU_QUERY_ERROR,
XM_SMU_QUERY_FINISH,

XM_SMU_LOGIN_BEGIN,
XM_SMU_LOGIN_SENT,
XM_SMU_LOGIN_RECEIVED,
XM_SMU_LOGIN_LISTING,
XM_SMU_LOGIN_FINISH,
XM_SMU_LOGIN_CANCEL,
XM_SMU_LOGIN_ERROR,

XM_SMU_LISTING_RECEIVE,

XM_SMU_SERVER_CONNECTING,
XM_SMU_SERVER_CONNECTED,
XM_SMU_SERVER_ERROR,
XM_SMU_SERVER_CLOSED,

XM_SMU_MOTD_RECEIVED,
XM_SMU_MOTD_SENT

};

class CXMServerManager : public CXMPipelineBase
{
public:

	//construction
	CXMServerManager();
	virtual ~CXMServerManager();

	//server session management
	bool ServerOpen();
	bool ServerClose();
	bool ServerIsOpen();
	
	//Login
	bool LoginUI();
	bool Login(const char *username, const char *password);
	bool LoginCancel();
	bool LoginIsLoggedIn();
	CMD5 LoginGetSession();
	char* LoginGetUsername();
	char* LoginGetPassword();

	//query interface
	bool QueryBegin(CXMQuery *query);
	bool QueryCancel();
	bool QueryIsRunning();
	CXMQuery* QueryGet();						//NOT THREADSAFE
	CXMQueryResponse* QueryGetResponse();		//NOT THREADSAFE

	//query limiter
	bool LimiterIndex(CXMIndex *index);
	bool LimiterFilter(CXMIndex *filter);

	//listing interface
	bool SendListing(bool full);
	bool RequestListing(HWND hwnd);
	bool RequestListing(CMD5 md5, HWND hwnd);

	//MOTD
	bool MotdIsNew();
	void MotdShow();

protected:
	
	//server session state
	CXMSession *mServer;
	bool mServerShuttingDown;

	//login data
	char mLoginMsg[MAX_PATH];
	bool mLoggedIn;
	bool mLoginCanceled;
	CMD5 mSessionID;
	char *mUsername, *mPassword;

	//query data
	bool mQueryRunning;
	CXMQuery *mQuery;
	CXMQueryResponse *mQueryResponse;
	int mLimiterMaxIndex;
	int mLimiterMaxFilter;

	//listing data
	HWND mListingWindow;

	//motd data
	bool mMotdNew;
	char *mMotdType;
	char *mMotdMsg;
	char *mMotdQuestion;
	char *mMotdChoices;

	//hooks called by the pipeline
	virtual bool OnInitialize();
	virtual void OnWin32MsgPreview(UINT msg, WPARAM wparam, LPARAM lparam);
	virtual void OnWin32MsgReview(UINT msg, WPARAM wparam, LPARAM lparam);
	virtual void OnMsgReceived(CXMSession *ses, CXMMessage *msg);
	virtual void OnMsgSent(CXMSession *ses, CXMMessage *msg);
	virtual void OnStateChange(CXMSession *ses, UINT vold, UINT vnew);
};

// ------------------------------------------------------------------ Login Dialog


class CLoginDialog : public CDialog
{
public:
	CLoginDialog(CWnd* pParent = NULL);   // standard constructor
	virtual ~CLoginDialog();

	//status interface (thread safe)
	void StatusEntry(const char* text, bool detail);
	bool StatusGetCanceled();
	void StatusSetWorkerDone();
	void StatusSetWorkerFail();

private:

	//pipeline
	void DoLogin();
	void SetState(BYTE newstate);
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg LRESULT OnServerMsg(WPARAM wParam, LPARAM lParam);

	//pipeline data
	CXMSession *m_pSession;
	BYTE m_nState;
	CString m_strUsername;
	CString m_strPassword;
	bool m_bShowingQuestion;

	//status interface data
	afx_msg LRESULT OnStatusEntry(WPARAM, LPARAM);
	bool m_bWaitingForLogin;
	bool m_bWorkerDone;
	bool m_bWorkerFail;
	HANDLE m_hThread;
	DWORD m_dwThreadId;
	CRITICAL_SECTION m_cs;

	//controls
	CEdit m_editLogin;
	CEdit m_editPassword;
	CEdit m_editStatus;
	CButton m_buttonOK;
	CButton m_buttonCancel;
	CButton m_buttonDetails;
	CButton m_buttonOptions;
	CListBox m_listDetails;

	//user input
	bool m_bCancel;
	bool m_bDetails;
	afx_msg void OnClickedLoginbutton();
	afx_msg void OnOK();
	afx_msg void OnCancel();
	afx_msg void OnDetails();
	afx_msg void OnOptions();
	afx_msg void OnSignup();

	//windowing
	DECLARE_MESSAGE_MAP()
	BOOL PreCreateWindow(CREATESTRUCT &cs);
	BOOL OnInitDialog(void);
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	afx_msg HBRUSH OnCtlColor(CDC *pDC, CWnd *pWnd, UINT nCtlColor);
	afx_msg void OnDestroy();
};