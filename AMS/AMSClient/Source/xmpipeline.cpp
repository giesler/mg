//----------------------------------------------------------------------------------------
// XMPIPLINE.CPP															XMPIPLINE.CPP
//----------------------------------------------------------------------------------------

#include "stdafx.h"
#include "xmclient.h"
#include "xmnet.h"
#include "xmpipeline.h"
#include "xmdb.h"

// ----------------------------------------------------------------------- XMPiplineBase

//NOTE: do you even need this?
LRESULT APIENTRY PipelineWndProc(
    HWND hWnd, 
    UINT Msg, 
    WPARAM wParam, 
    LPARAM lParam) 
{
	return DefWindowProc(hWnd, Msg, wParam, lParam);
}

//Simply pass control to the pipeline
DWORD WINAPI PipelineThreadProc(LPVOID lpParameter)
{
	CXMPipelineBase *pl = (CXMPipelineBase*)lpParameter;
	return pl->Alpha();
}

bool CXMPipelineBase::InitOnce()
{
	//register window class
	WNDCLASSEX w;
	w.cbSize		= sizeof(WNDCLASSEX);
	w.style			= NULL;
	w.lpfnWndProc	= PipelineWndProc;
	w.cbClsExtra	= 0;
	w.cbWndExtra	= 0;
	w.hInstance		= app()->m_hInstance;
	w.hIcon			= NULL;
	w.hCursor		= NULL;
	w.hbrBackground	= NULL;
	w.lpszMenuName	= NULL;
	w.lpszClassName	= PIPELINEWNDCLASS;
	w.hIconSm		= NULL;

	//create the pipeline's update lock
	InitializeCriticalSection(&CXMPipelineUpdateTag::mcsSync);

	return (RegisterClassEx(&w)!=0);
}

CXMPipelineBase::CXMPipelineBase()
{
	//setup sync objects
	InitializeCriticalSection(&mcsLock);

	//initialize handles to null
	mhWnd = NULL;
	mhThread = NULL;
	mdwThreadID = 0;

	//setup subscriber list
	//mSubscribers.SetSize(
}

CXMPipelineBase::~CXMPipelineBase()
{
	//are we running?
	Lock();
	if (mhThread)
	{
		Stop();
	}
	Unlock();

	//release critical section
	DeleteCriticalSection(&mcsLock);
}

void CXMPipelineBase::Start()
{
	//are we already running?
	Lock();
	if (!mhThread)
	{
		//Open up the thread
		mhThread = CreateThread(NULL,				//security attributes
								NULL,				//default stack size (1 page commited, 1mb reserved)
								PipelineThreadProc,	//thread function
								(void*)this,		//parameter
								CREATE_SUSPENDED,	//create flags
								&mdwThreadID);		//thread id [out]
		ResumeThread(mhThread);
	}
	Unlock();
}

void CXMPipelineBase::Stop()
{
	//are we even running?
	Lock();
	if (mhThread)
	{
		//send kill signal
		PostThreadMessage(mdwThreadID, WM_QUIT, 0, 0);
		
		//wait for thread to exit
		if (WaitForSingleObject(mhThread, 100)==
			WAIT_TIMEOUT)
		{
			//kill the thread
			TerminateThread(mhThread, 0);
		}
		CloseHandle(mhThread);
		mhThread = NULL;
		mdwThreadID = 0;
	}
	Unlock();
}

HWND CXMPipelineBase::GetHWND()
{
	//threadsafe
	HWND temp;
	Lock();
	temp = mhWnd;
	Unlock();
	return temp;
}

DWORD CXMPipelineBase::GetThreadID()
{
	//threadsafe
	DWORD temp;
	Lock();
	temp = mdwThreadID;
	Unlock();
	return temp;
}

void CXMPipelineBase::Subscribe(HWND target, bool secondary)
{
	//insert target into collection
	Lock();
	if (secondary)
		mSecondary.Add((void*)target);
	else
		mSubscribers.Add((void*)target);
	Unlock();
}

void CXMPipelineBase::UnSubscribe(HWND target)
{
	//remove target from collection
	int i;
	Lock();
	for(i=0;i<mSubscribers.GetSize();i++)
	{
		if (mSubscribers[i]==(void*)target)
		{
			mSubscribers.RemoveAt(i);
			break;
		}
	}
	for(i=0;i<mSecondary.GetSize();i++)
	{
		if (mSecondary[i]==(void*)target)
		{
			mSecondary.RemoveAt(i);
			break;
		}
	}
	Unlock();
}

void CXMPipelineBase::SendEvent(UINT msg, WPARAM wp, LPARAM lp)
{
	//send the msg to all subscribers
	Lock();
	for (int i=0;i<mSubscribers.GetSize();i++)
	{
		::PostMessage((HWND)mSubscribers[i], msg, wp, lp);
	}
	for (i=0;i<mSecondary.GetSize();i++)
	{
		::PostMessage((HWND)mSecondary[i], msg, wp, lp);
	}
	Unlock();
}

DWORD CXMPipelineBase::Alpha()
{
	//declares
	int cont;
	MSG msg;
	CXMSession *xmses = NULL;
	
	//create our message pump window
	Lock();
	mhWnd = CreateWindowEx(	NULL,				//ex style
							PIPELINEWNDCLASS,	//class
							NULL,				//window name
							NULL,				//style
							0,					//x
							0,					//y
							0,					//width
							0,					//height
							NULL,				//TODO: does HWND_MESSAGE crash on pre-win2k?
							NULL,				//menu
							app()->m_hInstance,	//instance
							NULL);				//lparam
	if (!mhWnd) {
		Unlock();
		goto end;
	}
	Unlock();

	//done initializing
	if (!OnInitialize()) {
		goto end;
	}

	//start message pump
	cont = GetMessage(&msg, NULL, 0, 0);
	while(cont!=-1 && cont!=0)
	{
		//every msg invokes a standard handler
		OnWin32MsgPreview(msg.message, msg.wParam, msg.lParam);

		//our message handler
		switch (msg.message)
		{
		case XM_MESSAGE:

			//its a new xm message.. inbound or out?
			xmses = (CXMSession*)msg.lParam;
			if (msg.wParam == XM_INBOUND)
				OnMsgReceived(xmses, xmses->Receive());
			else
				OnMsgSent(xmses, xmses->Sent());

			break;

		case XM_STATE:

			//its a state change
			xmses = (CXMSession*)msg.lParam;
			OnStateChange(xmses, XM_OLDSTATE(msg.wParam), XM_NEWSTATE(msg.wParam));

			break;
		}

		//every msg invokes a standard handler
		OnWin32MsgReview(msg.message, msg.wParam, msg.lParam);

		//retrieve next message
		cont = GetMessage(&msg, NULL, 0, 0);
	}
	
end:
	//close window
	if (mhWnd)
	{
		DestroyWindow(mhWnd);
		mhWnd = NULL;
	}
	
	return 0;
}

CXMSession* CXMPipelineBase::OpenSession(char* address, UINT port)
{
	//create a new session that
	//sends messages to us
	CXMSession *ses = new CXMSession(GetHWND());
	sessions()->Attach(ses);
	ses->Open(address, port);
	return ses;
}
