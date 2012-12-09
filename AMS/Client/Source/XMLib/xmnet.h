//------------------
// XMSession.H
//------------------
#pragma once

class CXMSession;
class CXMNetBuffer;
class CXMSessionManager;

#include "xmmessage.h"

//#define XM_WELLKNOWNPORT	38492

//WINDOW MESSAGES
#define XM_MSGBASE		WM_USER+1024

#define XM_MESSAGE		XM_MSGBASE+0
//xmmessage is waiting
//	wparam: direction
//	lparam: session pointer
#define XM_INBOUND	0	//new message is waiting
#define XM_OUTBOUND	1	//message was just sent

#define XM_SOCKET		XM_MSGBASE+1
//windows socket notificatin
//	wparam: SOCKET
//	lparam:
//		HIWORD: error
//		LOWORD: notification
//NOTE: internal to session

#define XM_STATE		XM_MSGBASE+2
//xmsession state has changed
//	wparam:
//		HIBYTE: old state
//		LOBYTE: new state
//	lparam: session pointer
#define XM_OLDSTATE(_lp) HIBYTE(_lp)
#define XM_NEWSTATE(_lp) LOBYTE(_lp)

#define XM_CLOSE		XM_MSGBASE+3
//instruct a session to close
//	wparam: empty
//	lparam: empty

#define XM_CLIENTMSG	XM_MSGBASE+4
//message from the client pipeline
//	wparam: message
//	lparam: pointer to ticket
//see xmpipeline.h for more

#define XM_SERVERMSG	XM_MSGBASE+5
//message from the server pipeline
//	wparam: message
//	lparam: <msg defined>

#define XM_ASYNCRESIZE	XM_MSGBASE+6
//async resize op complete
//	wparam: op
//	lparam: work item id

#define XM_STATUSENTRY	XM_MSGBASE+7
//login dialog should add entry
//	wparam: BOOL, detail
//	lparam: strdup'd message

#define XM_TRAYICON		XM_MSGBASE+8
//msg sent by tray icon
//	wparam: see win32 docs
//	lparam:

#define XM_PROGRESS		XM_MSGBASE+9
//sent by the network layer when
//binary data arrives
//	wparam: direction (XM_INBOUND, XM_OUTBOUND)
//	lparam: session pointer

#define XM_MINMSG	XM_MSGBASE+0	//used to filter msgs
#define XM_MAXMSG	XM_MSGBASE+9	//

//SESSION STATES
#define XM_CLOSED		0
#define XM_OPENING		1
#define XM_OPEN			2
#define XM_CLOSING		3

//WINDOW CLASS
#define SESSIONWNDCLASS "CXMSession_WndClass"

//LIBRARY INCLUDES
#pragma comment(lib, "ws2_32.lib")

// ------------------------------------------------------------------------- NET BUFFER

class CXMNetBuffer
{
public:
	CXMNetBuffer();
	~CXMNetBuffer();

	inline DWORD Size() { return mBufferSize - mStart; };
	bool IsEmpty() { return mBufferSize==0; };
	DWORD Push(void *buf, DWORD size);		//push data onto the buffer
	DWORD Pop(void **pbuf, DWORD size);		//pop data from the buffer
	void Rewind();							//undoes last pop
	void Pack();							//packs the contents of the buffer,
											//call after each pop, after you
											//have used the buffer

	//WARING: Pop returns an address into the internal
	//buffer! DO NOT FREE OR WRITE THIS MEMORY! The buffer
	//returned can only be garunteed until the next call
	//to Pack, since Pack often re-allocates the buffer.

	//WARNING: Rewind and Pack are mutaually exclusive!
	//Do not call them both for the same Pop().  Rewind
	//undoes the pop, while Pack() commits it.

private:
	void *mBuffer;			//actual buffer
	DWORD mStart;			//start location
	DWORD mBufferSize;		//allocated size of buffer
	DWORD mLastPop;			//amount of data that was pop'd
	int mLockCount;			//tracks locking
};


// ---------------------------------------------------------------------- SESSION HANDLER

class IXMSessionHandler
{
public:
	virtual void OnMessageReceived(CXMSession *session)=0;
	virtual void OnMessageSent(CXMSession *session)=0;
	virtual void OnStateChanged(CXMSession *session, int oldState, int newState)=0;
	virtual void OnProgress(CXMSession *session)=0;
    virtual void AddRef()=0;
    virtual void Release()=0;
};

class IXMSessionHandlerFactory
{
public:
	virtual IXMSessionHandler*	CreateHandler()=0;
	virtual void AddRef()=0;
	virtual void Release()=0;
};

//handler interface implementation
class CXMSessionHWNDHandler : public IXMSessionHandler
{
public:
	CXMSessionHWNDHandler(HWND hwndWindow);

	//IXMSessionHandler Implementation
	virtual void OnMessageReceived(CXMSession *session);
	virtual void OnMessageSent(CXMSession *session);
	virtual void OnStateChanged(CXMSession *session, int oldState, int newState);
	virtual void OnProgress(CXMSession *session);

	//IUnknown Implementation
    virtual void AddRef();
    virtual void Release();

//data
protected:
	HWND m_hwndWindow;
	UINT m_refCount;
};

// ------------------------------------------------------------------ SESSION

class CXMSession
{
public:

	//construction
	void AddRef();
	void Release();
	CXMSession(IXMSessionHandler *owner);			//must supply awindow for messages
	static bool InitOnce(
		HINSTANCE HINSTANCE);		//register our wndcls
	bool Alpha();					//threadproc
	
	//state modifiers
	bool Open(char* address,
			  unsigned int port);	//connect to given address
	bool Open();					//defaults to server
	bool Accept(SOCKET newSocket);	//opens the session from a socket
	bool Close(DWORD wait = 0);		//milliseconds to wait
	int GetState();					//return current state

	//xml message queue access
	bool Send(CXMMessage* msg);		//push a message onto the out queue
	CXMMessage* Receive();			//pop a message from the in queue
	CXMMessage* Sent();				//pop the last message sent
	int GetMessagesOut();			//messages actually sent
	int GetMessagesIn();			//messages actually received

	//txrx lights
	static void TxRxReset(bool &tx, bool &rx);

	//misc
	void SetOwner(IXMSessionHandler *owner);
	bool PostMessage(UINT Msg, WPARAM wParam, LPARAM lParam);
	char* LocalIP();
	DWORD GetThreadID();
	int GetDirection();
	void SetSessionId(CGUID& newSession);
	CGUID& GetSessionId();
	void GetBinaryProgress(DWORD *dwTotal, DWORD *dwCurrent);

private:

	//session is now ref counted
	~CXMSession();					//to be sent to
	UINT mRefCount;

	//win32 threadings
	static HINSTANCE mhInstance;
	IXMSessionHandler *mpOwner;		//interface to event handler
	HWND mhSelf;					//window, and our window
	HANDLE mhThread;				//handle to our thread
	UINT mdwThreadID;				//threadid of our thread

	//windows message helper functions
	void PostState(int oldstate);

	//xml message queue
	CXMMessageQueue in;				//incoming queue
	CXMMessageQueue out;			//outgoing queue
	CXMMessageQueue sent;			//sent queue
	void FlushOut();				//flush out queue into net buffer
	
	//initialization functions
	bool OpenInner();				//called open and accept
	bool InitCOM();
	bool InitThread();
	bool InitSync();
	bool InitWindow();
	bool InitSocket();

	//termination functions
	void CloseCOM();
	void CloseThread();
	void CloseSync();
	void CloseWindow();
	void CloseSocket();

	//network state
	SOCKET msockMain;				//socket handle
	bool mAccept;					//handles a request
	int mDirection;					//did we connect or accept?
	char* mhostAddress;				//endpoint's address..
	unsigned int mhostPort;			//and port
	bool mbExpectedShutdown;		//indicates that shutdown is
									//in-progress
	bool mbIsConnected;				//not set until the connection
									//has completed
	int mState;						//current state
	void SetState(int newState);	//update state, send msg

	//network access
	bool SendChunk();				//send and receive small
	int ReceiveChunk();				//chunks from buffers
	bool ScanChunk(void* buf,
				   DWORD size);		//scans buf and dispatchs msgs
	CXMNetBuffer netin, netout;		//send/recv buffers
	long mLastSequence;				//last sequence number sent

	//binary messages
	CXMMessage *mBinaryMessage;		//pointer to the last message
	bool mBinary;					//true if receiving binary data
	DWORD mExpectedBinarySize;		//size of binary chunk
	DWORD mCurrentBinarySize;		//amounnt of binary already downloaded

	//sync helpers
	CRITICAL_SECTION mSync;			//sync access to queues
	void Lock(DWORD timeout = 0);	//
	void Unlock();					//

	//tx rx lights
	static CRITICAL_SECTION m_txrxSync;
	static bool m_txrxSend, m_txrxReceive;
	static void TxRxSend();
	static void TxRxReceive();
};

// ------------------------------------------------------------------ SESSION MANAGER

class CXMSessionManager : IXMSessionHandlerFactory
{
public:
	CXMSessionManager();
	~CXMSessionManager();

	//syncronization ops
	void Lock();
	void Unlock();

	//listen operations
	void SetSessionHandlerFactory(IXMSessionHandlerFactory *factory);
	bool Listen(HWND hOwner);
	bool Stop();

	//message handling
	bool PreviewMessage(UINT msg, WPARAM wParam, LPARAM lParam);
	bool ReviewMessage(UINT msg, WPARAM wParam, LPARAM lParam);

	//session list operations
	bool Attach(CXMSession* session);
	CXMSession* Detach(int index);
	bool DetachAll();
	bool Free(int index);
	bool FreeAll();

	//session list access
	int GetSessionCount();
	int FindByPointer(CXMSession* session);
	int FindByThreadID(DWORD dw);
	CXMSession* GetSession(int i);

	//handler factory imp
	IXMSessionHandler* CreateHandler();
	void AddRef();
	void Release();

private:
	CRITICAL_SECTION mcsSync;	//sync object
	HWND mhOwner;				//owner handle
	SOCKET msockListener;		//listener socket
	CXMSession** mList;			//buffer for sessions
	int mListCount;				//number of sessions

	IXMSessionHandlerFactory *mHandlerFactory;
};