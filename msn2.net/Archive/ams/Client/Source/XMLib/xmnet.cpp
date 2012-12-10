//-----------------
// XMSESSION.CPP
//-----------------

#include "stdafx.h"
#include "xmlib.h"
#include "xmnet.h"

#include <objbase.h>
#include <process.h>

//send/receive params
#define SEND_CHUNK		4096
#define RECV_CHUNK		4096

//handler interacce implementation
CXMSessionHWNDHandler::CXMSessionHWNDHandler(HWND hwndWindow)
{
	m_hwndWindow = hwndWindow;
	m_refCount = 1;
}

//IXMSessionHandler Implementation
void CXMSessionHWNDHandler::OnMessageReceived(CXMSession *session)
{
	::PostMessage(m_hwndWindow, XM_MESSAGE, XM_INBOUND, (LPARAM)session);
}
void CXMSessionHWNDHandler::OnMessageSent(CXMSession *session)
{
	::PostMessage(m_hwndWindow, XM_MESSAGE, XM_OUTBOUND, (LPARAM)session);
}
void CXMSessionHWNDHandler::OnStateChanged(CXMSession *session, int oldState, int newState)
{
	::PostMessage(m_hwndWindow, XM_STATE, MAKEWORD(newState, oldState), (LPARAM)session);
}
void CXMSessionHWNDHandler::OnProgress(CXMSession *session)
{
	::PostMessage(m_hwndWindow, XM_PROGRESS, XM_INBOUND, (LPARAM)session);
}

//IUnknown Implementation
void CXMSessionHWNDHandler::AddRef()
{
	m_refCount++;
}
void CXMSessionHWNDHandler::Release()
{
	if (--m_refCount<1)
		delete this;
}


HINSTANCE CXMSession::mhInstance;

//static stuff
CRITICAL_SECTION CXMSession::m_txrxSync;
bool CXMSession::m_txrxSend, CXMSession::m_txrxReceive;

//global session guid
CGUID gSession;

void CXMSession::SetSessionId(CGUID& newSession)
{
	//assign new session id
	gSession = newSession;
}

CGUID& CXMSession::GetSessionId()
{
	//retrieve session id
	return gSession;
}

LRESULT APIENTRY SessionWndProc(
    HWND hWnd, 
    UINT Msg, 
    WPARAM wParam, 
    LPARAM lParam) 
{
	//NOTE: Except for certain messages delivered by
	//windows, any messages sent to the session should
	//be sent with PostThreadMessage
	
	//NEVER perform a SendMessage, it will get passed
	//directly to this function and never reach Alpha's
	//message pump to get processed!

	switch (Msg) {

	case WM_CREATE:
		//nothing to do
		return 0;

	case XM_MESSAGE:
		return 0;

	case XM_SOCKET:
		return 0;

	default:
		return DefWindowProc(hWnd, Msg, wParam, lParam);
	}
}

UINT __stdcall SessionThreadProc(LPVOID lpParameter)
{
	//resolve parameter to a session, then
	//pass control to the session
	CXMSession *session = (CXMSession*)lpParameter;
	DWORD temp = session->Alpha();
	session->Release();
	return (UINT)temp;

	//NOTE: we hold a reference on session until its
	//threadproc exits.. this keeps the socket from
	//getting closed before the thread wants it to
}

bool CXMSession::InitOnce(HINSTANCE hinstance)
{
	mhInstance = hinstance;

	//register window class
	WNDCLASSEX w;
	w.cbSize		= sizeof(WNDCLASSEX);
	w.style			= NULL;
	w.lpfnWndProc	= SessionWndProc;
	w.cbClsExtra	= 0;
	w.cbWndExtra	= 0;
	w.hInstance		= mhInstance;
	w.hIcon			= NULL;
	w.hCursor		= NULL;
	w.hbrBackground	= NULL;
	w.lpszMenuName	= NULL;
	w.lpszClassName	= SESSIONWNDCLASS;
	w.hIconSm		= NULL;

	//create the critical section for tx rx lights
	InitializeCriticalSection(&m_txrxSync);

	return (RegisterClassEx(&w)!=0);
}

bool CXMSession::Alpha()
{	
	fd_set set;
	timeval timeout = {2, 0};	//2 seconds	

	//from now until connection is complete, we
	//are in "opening" state
	SetState(XM_OPENING);

	//initialize various settings
	if (!InitCOM()) {
		SetState(XM_CLOSED);
		return false;
	}
	if (!InitWindow())	{
		CloseCOM();
		SetState(XM_CLOSED);
		return false;
	}
	if (!InitSocket()) {
		CloseCOM();
		CloseWindow();
		SetState(XM_CLOSING);
		SetState(XM_CLOSED);
		return false;
	}

	//misc variables
	SOCKET sock;

	//enter message loop
	int cont, retval;
	MSG msg;
	cont = GetMessage(&msg, NULL, XM_MINMSG, XM_MAXMSG);
	while(cont!=-1 && cont!=0)
	{
		//proccess msg
		switch (msg.message) {

		case XM_MESSAGE:

			//flush outbound message queue into the
			//outbound network buffer
			FlushOut();

			break;

		case XM_SOCKET:
			
			//was this intended for our socket?
			sock = msg.wParam;
			if (sock==msockMain) {
				
				//process message
				switch(WSAGETSELECTEVENT(msg.lParam)) {

				case FD_ACCEPT:
					break;

				case FD_CONNECT:

					//check errors
					if (WSAGETSELECTERROR(msg.lParam))
					{
						//connection failed, initiate tear-down
						PostQuitMessage(0);
						mbIsConnected = false;
						SetState(XM_CLOSING);
					}
					else
					{
						//a pending connection was completed
						mbIsConnected = true;
						SetState(XM_OPEN);
					}
					break;

				case FD_CLOSE:
					
					//our socket has been closed... if this was
					//not initiated by us, then initiate gracefull
					//shutdown
					if (!mbExpectedShutdown)
					{
						PostMessage(XM_CLOSE, NULL, NULL);

						if (mhostPort == (UINT)config()->GetFieldLong(FIELD_SERVER_PORT))
						{
							TRACE("*** Unexpected FD_CLOSE received from server.\n");
						}
					}

					break;

				case FD_READ:
					//socket is ready for reading
					if (ReceiveChunk()<1)
					{
						//connection is dead
						TRACE("*** FD_READ: Connection is closed.\n");
						PostMessage(XM_CLOSE, NULL, NULL);
					}
					break;

				case FD_WRITE:
					//socket is ready for writing
					if(!SendChunk())
					{
						//send failed.. connectio prolly dead
						TRACE("*** FD_WRITE: Connection is closed.\n");
						PostMessage(XM_CLOSE, NULL, NULL);
					}
					break;

				default:
					break;
				}
				
			}

			break;

		case XM_CLOSE:

			if (!mbExpectedShutdown)
			{
				//from now until we are complelty closed, 
				//we are in "closing" state
				SetState(XM_CLOSING);

				//we are shutting down
				mbExpectedShutdown = true;

				//queue up a gracefull close message
				//TODO: create close xmmessage

				//flush outbound queue into net buffer
				FlushOut();

				//switch to blocking mode
				WSAAsyncSelect(msockMain, mhSelf, 0, 0);

				//flush the outbound net buffer
				while (!netout.IsEmpty()) {
					if (!SendChunk())
						break;
					
					//wait until we can send again
					FD_ZERO(&set);
					FD_SET(msockMain, &set);
					select(NULL, NULL, &set, NULL, &timeout);
				}
				
				//disable further sends, and wait for close
				//readiness
				if (shutdown(msockMain, SD_SEND)!=SOCKET_ERROR)
				{
					//set disconnected state
					mbIsConnected = false;

					//flush the socket's receive buffer when
					//the close event completes
					fd_set set;
					timeval timeout = {2, 0};	//2 seconds
					FD_ZERO(&set);
					FD_SET(msockMain, &set);
					retval = select(0, &set, NULL, NULL, &timeout);
					if (retval==1) {
						do {
							retval = ReceiveChunk();
						} while(retval!=0 && retval!=SOCKET_ERROR);
					}
				}

			}//!mbExpected Shutdown

			//this will cause the pump to break
			//the next time around
			PostQuitMessage(0);	
			
			//hack: for some reason, when a connection is cut postquitmessage seems
			//to have no affect, and GetMessage() will block, so we need to send the
			//close state now
			//SetState(XM_CLOSED);

			break;

		default:
			break;
		}
		
		//retrieve next message
		cont = GetMessage(&msg, NULL, XM_MINMSG, XM_MAXMSG);
	}

	//free per-session resources
	CloseSocket();
	CloseWindow();
	CloseCOM();

	//we are now officially closed for business
	if (GetState()!=XM_CLOSED)
	{
		SetState(XM_CLOSED);
	}

	return true;
}

CXMSession::CXMSession(IXMSessionHandler *owner)
{
	//initialize member variables
	mpOwner = NULL;
	mhSelf = NULL;
	mhThread = NULL;
	mdwThreadID = 0;
	msockMain = NULL;
	mbExpectedShutdown = false;
	mbIsConnected = false;
	mBinary = false;
	mBinaryMessage = NULL;
	mExpectedBinarySize = 0;
	mState = XM_CLOSED;
	mAccept = false;
	mLastSequence = -1;
	mDirection = XM_INBOUND;
	mhostAddress = NULL;

	//store the event handler
	mpOwner = owner;
	mpOwner->AddRef();

	//init the critical section
	InitSync();

	//ref counting
	mRefCount = 1;
}

void CXMSession::AddRef()
{
	//TEMP{
	//Lock();
	mRefCount++;
	//Unlock();
}

void CXMSession::Release()
{
	//Lock();
	if (--mRefCount<1)
	{
		Unlock();
		delete this;
		return;
	}
	//Unlock();
}

CXMSession::~CXMSession()
{
	//free any resources
	//NOTE: these should already be freed,
	//but just making sure
	if (msockMain) {
		CloseSocket();
	}
	if (mhSelf) {
		CloseWindow();
	}
	if (mhThread) {
		CloseThread();
	}
	CloseSync();
	if (mhostAddress) {
		free(mhostAddress);
	}
	if (mpOwner)
	{
		mpOwner->Release();
	}

}

bool CXMSession::InitCOM()
{
	//initialize com on this thread
	if (FAILED(CoInitializeEx(NULL, COINIT_MULTITHREADED)))
		return false;
	return true;
}

bool CXMSession::InitSync()
{
	//create critical section
	InitializeCriticalSection(&mSync);
	return true;
}

bool CXMSession::InitWindow()
{
	//create our window
	mhSelf = CreateWindowEx(NULL,				//ex style
							SESSIONWNDCLASS,	//class
							NULL,				//window name
							NULL,				//style
							0,					//x
							0,					//y
							0,					//width
							0,					//height
							NULL,				//TODO: does HWND_MESSAGE crash on pre-win2k?
							NULL,				//menu
							mhInstance,		//instance
							NULL);				//lparam
	if (!mhSelf) {
		return false;
	}
	return true;
}

bool CXMSession::InitThread()
{
	//create thread (suspended)
	mhThread = /*CreateThread*/
	(HANDLE)_beginthreadex(	NULL,				//security attributes
							NULL,				//default stack size (1 page commited, 1mb reserved)
							SessionThreadProc,	//thread function
							(void*)this,		//parameter
							CREATE_SUSPENDED,	//create flags
							&mdwThreadID);		//thread id [out]
	if (!mhThread) {
		return false;
	}
	return true;
}

bool CXMSession::InitSocket()
{
	//open our socket
	sockaddr_in host;
	in_addr a;
	if (!msockMain)
	{
		//socket isn't connected yet, we need
		//to open the connection

		//resolve address.. try ip first
		a.S_un.S_addr  = inet_addr(mhostAddress);
		if (a.S_un.S_addr == INADDR_NONE) {
			
			//try looking up the hostent structure
			hostent *ph = gethostbyname(mhostAddress);
			if (!ph) {
				return false;
			}

			//is this the right kind of address?
			if (ph->h_length != sizeof(in_addr)) {
				return false;
			}

			//was there ANY address?
			if (ph->h_addr_list[0]==NULL) {
				return false;
			}

			//only concerned about the first address
			memcpy(&a, ph->h_addr_list[0], sizeof(in_addr));

			//copy the fqdn
			free(mhostAddress);
			mhostAddress = strdup(ph->h_name);
		}

		//fill out a sockaddr_in
		host.sin_family  = AF_INET;
		host.sin_addr = a;
		host.sin_port = htons(mhostPort);

		//create the socket object
		msockMain = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
		if (msockMain==INVALID_SOCKET) {
			msockMain = NULL;
			return false;
		}

		//subscribe to events
		if (WSAAsyncSelect(msockMain, mhSelf, XM_SOCKET, FD_ALL_EVENTS)!=0) {
			goto fail;
		}

		//open the socket
		if (connect(msockMain, (sockaddr*)&host, sizeof(sockaddr_in))==SOCKET_ERROR) {
			if (WSAGetLastError()!=WSAEWOULDBLOCK) {
				goto fail;
			}
		}
	}
	else {

		//find out ip of other endpoint
		int namelength = sizeof(sockaddr_in);
		if (getpeername(msockMain, (sockaddr*)&host, &namelength)==SOCKET_ERROR) {
			goto fail;
		}
		if (sizeof(sockaddr_in)!=namelength) {
			goto fail;
		}
		mhostPort = host.sin_port;
		if (mhostAddress)
			free(mhostAddress);
		mhostAddress = strdup(inet_ntoa(host.sin_addr));

		//subscribe to events
		if (WSAAsyncSelect(msockMain, mhSelf, XM_SOCKET, FD_ALL_EVENTS)==SOCKET_ERROR) {
			//TRACE1("WSA: %d\n", WSAGetLastError());
			closesocket(msockMain);
			this->CloseWindow();
		}

		//socket is now open
		SetState(XM_OPEN);
		mbIsConnected = true;

	}
	return true;

fail:
	closesocket(msockMain);
	msockMain = NULL;
	return false;
}

void CXMSession::CloseCOM()
{
	CoUninitialize();
}

void CXMSession::CloseThread()
{
	//post a quit message into the thread's message queue
	if (!PostThreadMessage(mdwThreadID, XM_CLOSE, NULL, NULL)) {
		return;
	}

	//close thread
	::CloseHandle(mhThread);
	mhThread = NULL;
	mdwThreadID = 0;
}

void CXMSession::CloseSync()
{
	//release syncronization object
	DeleteCriticalSection(&mSync);
}

void CXMSession::CloseWindow()
{
	//free the window object
	DestroyWindow(mhSelf);
	mhSelf = NULL;
}

void CXMSession::CloseSocket()
{
	//close the socket handle
	closesocket(msockMain);
	msockMain = NULL;
}

void CXMSession::SetOwner(IXMSessionHandler *owner)
{
	//allow changes in the window we send
	//updates to.
	Lock();

	//set the new owner
	if (mpOwner)
	{
		mpOwner->Release();
	}
	mpOwner = owner;
	mpOwner->AddRef();

	//send a friendly reminder of our state
	PostState(mState);

	Unlock();
}

bool CXMSession::PostMessage(UINT Msg, WPARAM wParam, LPARAM lParam)
{
	//post the message to our thread
	return PostThreadMessage(mdwThreadID, Msg, wParam, lParam) ? true : false;
}

bool CXMSession::OpenInner()
{
	//initialize win32 objects
	if (!InitThread()) {
		CloseSync();
		return false;	
	}

	//let the new thread loose
	AddRef();
	if (::ResumeThread(mhThread)==-1) {
		CloseSync();
		CloseThread();
		Release();
		return false;	
	}

	return true;
}

bool CXMSession::Open(char* address, unsigned int port)
{
	//check state
	if (mState!=XM_CLOSED) return false;

	//store endpoint info
	if (mhostAddress)
		free(mhostAddress);
	mhostAddress = strdup(address);
	mhostPort = port;

	//connection is outbound
	mDirection = XM_OUTBOUND;

	return OpenInner();
}

bool CXMSession::Open()
{
	//check state
	if (mState!=XM_CLOSED) return false;

	//pass forward config's server data
	if (mhostAddress)
		free(mhostAddress);
	mhostAddress = config()->GetServerAddress();
	mhostPort = config()->GetServerPort();

	//connection is outbound
	mDirection = XM_OUTBOUND;

	return OpenInner();
}

bool CXMSession::Accept(SOCKET newSocket)
{
	//check state
	if (mState!=XM_CLOSED) return false;

	//open session on existing socket
	msockMain = newSocket;
	mAccept = true;

	//connection is inbound
	mDirection = XM_INBOUND;

	return OpenInner();
}

bool CXMSession::Close(DWORD wait)
{
	//post our close message
	if (!PostMessage(XM_CLOSE, NULL, NULL))
		return false;

	//wait the given amount of time
	WaitForSingleObject(mhThread, wait);
	return true;
}

bool CXMSession::Send(CXMMessage* msg)
{
	//safely place a single message on the 
	//outgoing queue, and send notification
	//into the thread
	Lock();
	out.Push(msg);
	Unlock();
	return PostMessage(XM_MESSAGE, NULL, NULL);
}

CXMMessage* CXMSession::Receive()
{
	//safely read a single message
	//of the incoming queue
	CXMMessage *msg;
	Lock();
	msg = in.Pop();
	Unlock();
	return msg;
}

CXMMessage* CXMSession::Sent()
{
	CXMMessage *msg;
	Lock();
	msg = sent.Pop();
	Unlock();
	return msg;
}

void CXMSession::Lock(DWORD timeout)
{
	//wait for ownership of this object
	//TEMP {
	EnterCriticalSection(&mSync);
	//} TEMP
}

void CXMSession::Unlock()
{
	//release ownership of this object
	LeaveCriticalSection(&mSync);
}

void CXMSession::FlushOut()
{
	CXMMessage *pmsg;
	char* buf;
	int binsize;
	void* bin;

	//flush the outbound message queue into
	//the outbound net buffer
	Lock();
	while (out.Size()>0)
	{	
		//get the next message
		pmsg = out.Pop();

		//set standard message fields
		pmsg->SetSequence(++mLastSequence);
		pmsg->SetSessionID(gSession.GetString());

		//send message body over the wire
		buf = pmsg->ToString();
		if (buf)
		{
			//send the message
			netout.Push(buf, strlen(buf)+1); //add 1 for the null char
			free(buf);

			//binary data to send?
			binsize = pmsg->GetBinarySize();
			if (binsize>0)
			{
				bin = malloc(binsize);
				if (pmsg->GetBinaryData(bin, binsize)==(DWORD)binsize)
				{
					netout.Push(bin, binsize);
				}
				free(bin);
			}

			//place message on the sent queue
			sent.Push(pmsg);
		}
		else
		{
			//TRACE("Failed to send message!\n");
			//ASSERT(FALSE);
		}

		//inform owner window
		//::PostMessage(mhOwner, XM_MESSAGE, XM_OUTBOUND, (LPARAM)this);
		mpOwner->OnMessageSent(this);
	}
	Unlock();

	//begin sending (even if this blocks, it will
	//get winsock to start pumping us messages
	if (!netout.IsEmpty())
	{
		SendChunk();
	}
}

CXMNetBuffer::CXMNetBuffer()
{
	//initialize contents
	mBuffer = NULL;
	mBufferSize = 0;
	mStart = 0;
	mLockCount = 0;
	mLastPop = 0;
}

CXMNetBuffer::~CXMNetBuffer()
{
	//free buffer?
	if (mBufferSize>0)
	{
		free(mBuffer);
	}
}
DWORD CXMNetBuffer::Push(void *buf, DWORD size)
{
	//the buffer must be unlocked
	if (mLockCount>0)
	{
		return -1;
	}

	//allocate new buffer
	if (mBufferSize==0)
	{
		mBuffer = malloc(size);
		if (!mBuffer)
		{
			return -1;
		}
	}
	else
	{
		if (!(mBuffer = realloc(mBuffer, mBufferSize+size)))
		{
			return -1;
		}
	}

	//copy new data
	memcpy((char*)mBuffer+mBufferSize, buf, size);
	mBufferSize += size;

	return size;
}

DWORD CXMNetBuffer::Pop(void **pbuf, DWORD size)
{
	//increment the lock count
	mLockCount++;

	//is there any data to read?
	if (mBufferSize==mStart)
	{
		mLastPop = 0;
		return 0;
	}

	//set pointer
	*pbuf = ((char*)mBuffer+mStart);

	//figure out size
	DWORD temp = (mBufferSize - mStart) > size ? size : mBufferSize - mStart;
	mStart += temp;
	mLastPop = temp;
	return temp;
}

void CXMNetBuffer::Pack()
{
	//decrement lock count
	mLockCount--;

	//we dont need pop size, just reset it
	mLastPop = 0;

	//should the buffer be emptied?
	if (mStart==mBufferSize)
	{
		//free buffer
		if (mBufferSize>0)
		{
			free(mBuffer);
			mBufferSize = 0;
			mStart = 0;
		}
		return;
	}

	//if there there is less than 16k to free, or what is freed
	//is less than 1/3 of the buffer, dont bother
	if (mStart < (16*1024) ||
		mStart < (mBufferSize/3))
	{
		return;
	}

	//go ahead and pack buffer
	void *buf = malloc(mBufferSize - mStart);
	memcpy(buf,(char*) mBuffer + mStart, mBufferSize - mStart);

	//swap in new buffer
	free(mBuffer);
	mBuffer = buf;

	//adjust state
	mBufferSize = mBufferSize - mStart;
	mStart = 0;
}

void CXMNetBuffer::Rewind()
{
	//undoes the last pop.. mutually exclusive to Pack()!
	mLockCount--;

	//rewind
	mStart -= mLastPop;
	mLastPop = 0;
}

bool CXMSession::SendChunk()
{
	//only send if we are connected
	if (!mbIsConnected)
	{
		return false;
	}

	//read chunk
	void *chunk = NULL;
	DWORD chunksize = 0;
	while (true)
	{
		//get next chunk
		chunksize = netout.Pop(&chunk, SEND_CHUNK);

		//was there anything?
		if (chunksize<1)
		{
			netout.Rewind();
			return true;
		}

		//attempt send
		int retval = send(msockMain, (char*)chunk, chunksize, 0);
		if (retval==SOCKET_ERROR)
		{
			//nothing was sent--probobly the socket would
			//have blocked
			netout.Rewind();

			//check error
			if (WSAGetLastError()==WSAEWOULDBLOCK)
			{
				return true;
			}

			return false;
		}

		//was the full chunk sent?
		if (chunksize==(DWORD)retval)
		{
			netout.Pack();
		}
		else
		{
			//rewind, and then pop out just the right amount
			netout.Rewind();
			chunksize = netout.Pop(&chunk, retval);
			if (chunksize==(DWORD)retval)
			{
				netout.Pack();
			}
			else
			{
				//buffer error
				netout.Rewind();
				PostMessage(XM_CLOSE, NULL, NULL);
				return false;
			}
		}

		//set the tx light
		TxRxSend();

	}

	//never gets here
	return true;
}

int CXMSession::ReceiveChunk()
{
	//receive from the socket
	void *chunk = malloc(RECV_CHUNK);
	DWORD chunksize = RECV_CHUNK;
	int retval = recv(msockMain, (char*)chunk, chunksize, NULL);
	if (retval==SOCKET_ERROR)
	{
		//was there only partial message delivery?
		if (WSAGetLastError()!=WSAEMSGSIZE)
		{
			free(chunk);
			return retval;
		}
	}
	if (retval==0)
	{
		//connection was gracefully closed
		free(chunk);
		return retval;
	}

	//scan data for interest
	ScanChunk(chunk, retval);
	free(chunk);

	//set the rx light
	TxRxReceive();

	return retval;
}

bool CXMSession::ScanChunk(void *buf, DWORD size)
{
	void* nullchar = NULL;
	void* temp = NULL;
	char* str = NULL;
	CXMMessage* msg = NULL;
	DWORD retval, remaining, origsize;

	if(mBinary) {

		//if the size of this chunk, plus what is in the
		//buffer meets or exceeds the expected size, then
		//pop off a binary message
		if ((netin.Size() + size) >= mExpectedBinarySize) {

			//figure out how much of the buffer
			//will be left over
			remaining = size - (mExpectedBinarySize - netin.Size());

			//pop out the net buffer
			origsize = netin.Size();
			retval = netin.Pop(&nullchar, netin.Size());
			if (retval!=origsize) {
				netin.Rewind();
				return false;
			}

			//recreate full buffer
			temp = malloc(mExpectedBinarySize);
			if (!temp) return false;
			memcpy(temp, nullchar, retval);
			memcpy((char*)temp+retval, buf, mExpectedBinarySize - retval);

			//done with data from buffer
			netin.Pack();

			//check md5 of buffer against what the
			//message is expecting
			CMD5Engine md5;
			md5.update((BYTE*)temp, mExpectedBinarySize);
			md5.finalize();
			if (!md5comp(mBinaryMessage->GetBinaryMD5(), md5.raw_digest())) {
	
				//they dont match!
				free(temp);
				delete mBinaryMessage;
				
				//TRACE("Non-matching MD5 in binary message!");
			}
			else {

				//copy buffer into message
				retval = mBinaryMessage->SetBinaryData(temp, mExpectedBinarySize);
				free(temp);
				if (retval!=mExpectedBinarySize) return false;

				//push message and inform system
				Lock();
				in.Push(mBinaryMessage);
				//::PostMessage(mhOwner, XM_MESSAGE, XM_INBOUND, (LPARAM)this);
				mpOwner->OnMessageReceived(this);
				Unlock();

				//message complete, send progress update
				mCurrentBinarySize = mExpectedBinarySize;
				//::PostMessage(mhOwner, XM_PROGRESS, XM_INBOUND, (LPARAM)this);
				mpOwner->OnProgress(this);
			}

			//turn off binary mode
			mBinary = false;
			mBinaryMessage = NULL;
			//mExpectedBinarySize = 0;	//NOTE: this will kill getbinaryprogress

			//proccesss rest of message
			if (remaining>0) {
				return ScanChunk((char*)buf+(size-remaining), remaining);
			}
			return true;
		}
		else {

			//not big enough yet
			netin.Push(buf, size);

			//send progress update
			mCurrentBinarySize = netin.Size();
			//::PostMessage(mhOwner, XM_PROGRESS, XM_INBOUND, (LPARAM)this);
			mpOwner->OnProgress(this);
		}
	}
	else {

		//standard message, scan for null char
		nullchar = memchr(buf, NULL, size);
		if (!nullchar) {

			//message not finished
			netin.Push(buf, size);
			return true;
		}
		else {

			//build full string
			origsize = netin.Size();
			retval = netin.Pop(&temp, netin.Size());
			if (retval!=origsize) {

				//couldnt pop entire buffer, msg would
				//be mangled.. abort
				netin.Rewind();
				netin.Push(buf, size);
				return false;
			}

			//build full string
			str = (char*)malloc(retval+((char*)nullchar-(char*)buf+1));
			memcpy(str, temp, retval);
			memcpy(str+retval, buf, ((char*)nullchar-(char*)buf+1));

			//cleanup
			netin.Pack();
			temp = NULL;
			
			//create message
			msg = new CXMMessage(this);
			if (!msg->FromString(str)) {

				//bad xml maybe?
				free(str);
				delete msg;
			
				//handle rest of buffer
				if (size-((char*)nullchar-(char*)buf+1)) {
					return ScanChunk(((char*)nullchar)+1, size - ((char*)nullchar-(char*)buf+1));
				}
				return false;
			}
			free(str);

			//is message binary?
			if (msg->GetExpectedBinarySize()>0) {

				//setup for binary mode
				mBinary = true;
				if(mBinaryMessage) {
					delete mBinaryMessage;
				}
				mBinaryMessage = msg;
				mExpectedBinarySize = msg->GetExpectedBinarySize();
				mCurrentBinarySize = 0;

				//let everyone know we are entering binary mode
				//::PostMessage(mhOwner, XM_PROGRESS, XM_INBOUND, (LPARAM)this);
				mpOwner->OnProgress(this);
			}
			else {
			
				//not binary, push onto stack
				Lock();
				in.Push(msg);
				//::PostMessage(mhOwner, XM_MESSAGE, XM_INBOUND, (LPARAM)this);
				mpOwner->OnMessageReceived(this);
				Unlock();
			}

			//scan the rest of the chunk
			if (size-((char*)nullchar-(char*)buf+1)) {
				return ScanChunk(((char*)nullchar)+1, size - ((char*)nullchar-(char*)buf+1));
			}
			
		}
	}
	return false;
}

char* CXMSession::LocalIP()
{
	//get our local ip, threadsafe
	char* temp;
	Lock();

	//is the socket open?
	if (!msockMain) {
		Unlock();
		return NULL;
	}

	//perform winsock calls
	sockaddr_in addr;
	int size = sizeof(sockaddr_in);
	if (getsockname(msockMain, (sockaddr*)&addr, &size)==SOCKET_ERROR) {
		//TRACE1("In CXMSessioN::LocalIP()...\n\tgetsockname() returned: %d\n", WSAGetLastError());
		Unlock();
		return NULL;
	}
	if (size!=sizeof(sockaddr_in)) {
		Unlock();
		return NULL;
	}

	//convert to string
	temp = inet_ntoa(addr.sin_addr);

	//success
	Unlock();
	return strdup(temp);
}

int CXMSession::GetState()
{
	//TEMP{
	//Lock();
	int temp = mState;
	//Unlock();
	return temp;
}

void CXMSession::SetState(int newState)
{
	//different?
	//Lock();
	int oldstate = mState;
	if (newState!=mState) {
		
		//update state
		mState = newState;		
	
		//inform owner
		PostState(oldstate);
	}
	//Unlock();
}

void CXMSession::PostState(int oldstate)
{
	//TEMP{
	//Lock();
	//::PostMessage(mhOwner, XM_STATE, MAKEWORD(mState, oldstate), (LPARAM)this);
	mpOwner->OnStateChanged(this, oldstate, mState);
	//Unlock();
}

DWORD CXMSession::GetThreadID()
{
	Lock();
	DWORD temp = mdwThreadID;
	Unlock();
	return temp;
}

int CXMSession::GetDirection()
{
	Lock();
	int temp = mDirection;
	Unlock();
	return temp;
}

int CXMSession::GetMessagesIn()
{
	Lock();
	int temp = in.GetCountIn();
	Unlock();
	return temp;
}

int CXMSession::GetMessagesOut()
{
	Lock();
	int temp = out.GetCountOut();
	Unlock();
	return temp;
}

// -------------------------------------------------------------------------------- TX RX LIGHTS

void CXMSession::TxRxSend()
{
	//set the tx flag
	EnterCriticalSection(&m_txrxSync);
	m_txrxSend = true;
	LeaveCriticalSection(&m_txrxSync);
}

void CXMSession::TxRxReceive()
{
	//set the rx flag
	EnterCriticalSection(&m_txrxSync);
	m_txrxReceive = true;
	LeaveCriticalSection(&m_txrxSync);
}

void CXMSession::TxRxReset(bool &tx, bool &rx)
{
	//retrieve the tx and rx flags, then reset them
	EnterCriticalSection(&m_txrxSync);
	tx = m_txrxSend;
	rx = m_txrxReceive;
	m_txrxSend = false;
	m_txrxReceive = false;
	LeaveCriticalSection(&m_txrxSync);
}

void CXMSession::GetBinaryProgress(DWORD *dwTotal, DWORD *dwCurrent)
{
	*dwTotal = mExpectedBinarySize;
	*dwCurrent = mCurrentBinarySize;
}

//-------------------------------------------------------------------------------
//																  Session Manager

CXMSessionManager::CXMSessionManager()
{
	mhOwner = NULL;
	msockListener = NULL;
	mList = NULL;
	mListCount = 0;
	mHandlerFactory = this;
	InitializeCriticalSection(&mcsSync);
}

CXMSessionManager::~CXMSessionManager()
{
	//free sessions
	FreeAll();

	if (msockListener)
		Stop();
	if (mList)
		free(mList);
	if (mHandlerFactory)
		mHandlerFactory->Release();

	DeleteCriticalSection(&mcsSync);
}

void CXMSessionManager::Lock()
{
	EnterCriticalSection(&mcsSync);
}

void CXMSessionManager::Unlock()
{
	LeaveCriticalSection(&mcsSync);
}

bool CXMSessionManager::Listen(HWND hOwner)
{
	//open socket in listen mode
	Lock();
	if (msockListener) {
		Unlock();
		return false;
	}
	mhOwner = hOwner;

	//create socket
	msockListener = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (msockListener==INVALID_SOCKET) {
		msockListener = NULL;
		Unlock();
		return false;
	}

	//bind to port
	sockaddr_in host;
	host.sin_addr.S_un.S_addr = INADDR_ANY;
	host.sin_family = AF_INET;
	host.sin_port = htons((UINT)config()->GetFieldLong(FIELD_NET_CLIENTPORT));
	if (bind(msockListener, (sockaddr*)&host, sizeof(host))==SOCKET_ERROR)
		goto fail;

	//turn on listen mode
	if (listen(msockListener, 255)==SOCKET_ERROR)	//maximum backlog!
		goto fail;

	//listen for events
	if (WSAAsyncSelect(msockListener, hOwner, XM_SOCKET, FD_ACCEPT))
	{
		//TRACE1("WSA: %d\n", WSAGetLastError());
		goto fail;
	}

	Unlock();
	return true;

fail:
	closesocket(msockListener);
	msockListener = NULL;
	Unlock();
	return false;
}

bool CXMSessionManager::Stop()
{
	//close socket
	Lock();
	if (!msockListener) {
		Unlock();
		return false;
	}
	closesocket(msockListener);
	msockListener = NULL;
	Unlock();
	return true;
}

bool CXMSessionManager::PreviewMessage(UINT msg, WPARAM wParam, LPARAM lParam)
{
	//do anything that must be down BEFORE 
	//our owner processes the message
	return true;
}

bool CXMSessionManager::ReviewMessage(UINT msg, WPARAM wParam, LPARAM lParam)
{
	//NOTE: function is not thread safe, and
	//relies on Attach and Detach to be
	//syncronized internally.

	//do standard processing, AFTER the
	//owner does
	CXMSession* session;
	SOCKET sock, newSock;
	switch(msg) {

		case XM_MESSAGE:
			//message arrived or sent
			session = (CXMSession*)lParam;

			break;

		case XM_SOCKET:
			//incoming connection request
			sock = (SOCKET)wParam;
			if (sock==msockListener) {
				if (!WSAGETSELECTERROR(lParam)) {
					if (WSAGETSELECTEVENT(lParam)==FD_ACCEPT) {
						
						//accept the conection
						sockaddr addr;
						int addrlen = sizeof(addr);
						newSock = accept(sock, &addr, &addrlen);
						if (newSock!=INVALID_SOCKET) {

							//setup new session
							IXMSessionHandler *handler = mHandlerFactory->CreateHandler();
							session = new CXMSession(handler);
							handler->Release();
							if (session->Accept(newSock)) {
								if (!Attach(session)) {
									session->Close(2000);
									closesocket(newSock);
								}
							}
							else {
								closesocket(newSock);
							}
							session->Release();
						}
					}
				}
			}
			break;

		case XM_STATE:
			//socket state changed
			session = (CXMSession*)lParam;
			switch(XM_NEWSTATE(wParam)) {
				
				case XM_CLOSED:
					//session was closed
					Detach(FindByPointer(session));
					break;

				case XM_CLOSING:
					//session is beginning teardown
					break;
				
				case XM_OPENING:
					//session is beginning connect
					break;

				case XM_OPEN:
					//session connect complete
					break;
			}

			break;

		default:
			break;
	}

	return true;
}

bool CXMSessionManager::Attach(CXMSession* session)
{
	//search for session
	Lock();
	if (FindByPointer(session)!=-1) {
		Unlock();
		return true;
	}

	//increase buffer size
	CXMSession** buf = (CXMSession**)realloc(mList, (++mListCount)*sizeof(CXMSession*));
	if (!buf) {
		mListCount--;
		Unlock();
		return false;
	}
	mList = buf;

	//store pointer
	mList[mListCount-1] = session;
	session->AddRef();
	Unlock();
	return true;
}

CXMSession* CXMSessionManager::Detach(int index)
{
	//check index
	Lock();
	if ((index>=mListCount)||(index<0)) {
		Unlock();
		return false;
	}

	//release our hold
	CXMSession* temp = mList[index];
	temp->Release();

	//bump memory down to fill gap
	memmove(&mList[index],			//destination
			&mList[index+1],		//source
			sizeof(CXMSession*)*(mListCount-(index+1)));	//count
	mListCount--;
	Unlock();
	return temp;
}

bool CXMSessionManager::DetachAll()
{
	//release every session
	for(int i=0;i<mListCount;i++)
		mList[i]->Release();

	//simply free the buffer and restet list count
	Lock();
	if (mList) {
		free(mList);
		mList = NULL;
		mListCount = 0;
	}
	Unlock();
	return true;
}

int CXMSessionManager::GetSessionCount()
{
	//threadsafe
	int temp;
	Lock();
	temp = mListCount;
	Unlock();
	return temp;
}

int CXMSessionManager::FindByPointer(CXMSession* session)
{
	//search for the given pointer
	Lock();
	for (int i=0;i<mListCount;i++) {
		if (mList[i]==session) {
			Unlock();
			return i;
		}
	}

	//not found
	Unlock();
	return -1;
}

int CXMSessionManager::FindByThreadID(DWORD dw)
{
	//search for the given thread id
	Lock();
	for (int i=0;i<mListCount;i++) {
		if (mList[i]->GetThreadID()==dw) {
			Unlock();
			return i;
		}
	}

	//not found
	Unlock();
	return -1;
}

CXMSession* CXMSessionManager::GetSession(int i)
{
	//return the session pionte at i
	CXMSession* temp;
	Lock();
	if (i>=mListCount) {
		Unlock();
		return NULL;
	}
	temp = mList[i];
	temp->AddRef();
	Unlock();
	return temp;
}

bool CXMSessionManager::Free(int index)
{
	///bounds check index
	Lock();
	if (index>=mListCount) {
		Unlock();
		return false;
	}

	//free the session
	CXMSession* session = GetSession(index);
	session->Close(2000);
	session->Release();

	//detach the pointer
	Detach(index);
	Unlock();
	return true;
}

bool CXMSessionManager::FreeAll()
{
	//close sessions
	bool temp;
	Lock();
	for (int i=0;i<mListCount;i++)
		mList[i]->Close();
	mListCount = 0;

	//detach
	temp = DetachAll();
	Unlock();
	return temp;
}

void CXMSessionManager::SetSessionHandlerFactory(IXMSessionHandlerFactory *factory)
{
	//release our current factory?
	Lock();
	if (mHandlerFactory)
		mHandlerFactory->Release();

	//assign new factory
	mHandlerFactory = factory;
	mHandlerFactory->AddRef();
	Unlock();
}

IXMSessionHandler* CXMSessionManager::CreateHandler()
{
	return new CXMSessionHWNDHandler(mhOwner);
}

void CXMSessionManager::AddRef()
{
}

void CXMSessionManager::Release()
{
}

