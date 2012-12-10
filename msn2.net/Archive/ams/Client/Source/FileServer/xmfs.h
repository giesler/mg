//
//	Adult Media Swapper File Server
//	(c)2001 Line2 Systems, Inc.
//
//	Author: Nick Nystrom
//
#pragma once

//system includes
#ifdef _DEBUG
#include <afxwin.h>
#include <winsock2.h>
#else
#include <windows.h>
#endif
#include <objbase.h>
#include <stdio.h>
#include <time.h>

//xmlib includes
#include "xmlib.h"
#include "xmdb.h"
#include "xmnet.h"
#include "xmmessage.h"

// ------------------------------------------------------------------------ GLOBALS

//the latest ams version.. be sure to update!
#define AMSVERSION "0.70"

void ShutdownServer();

// ------------------------------------------------------------------------ OUTPUT

#define PRINT	printf

#define LOG		{ \
				char logDate[9]; \
				char logTime[9]; \
				_strdate(logDate); \
				_strtime(logTime); \
				PRINT("%s %s: ", logDate, logTime); \
				} \
				PRINT

#ifdef ERROR
#undef ERROR
#endif
#define ERROR	PRINT("ERROR in \"%s\" on line %d:\n", __FILE__, __LINE__); \
				LOG


// ------------------------------------------------------------------ SERVER HANDLER

class CServerHandler : public IXMSessionHandler
{
public:
	CServerHandler();

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
	UINT m_refCount;
};


// ----------------------------------------------------------------- CLIENT HANDLER

class CClientHandler : public IXMSessionHandler
{
public:
	CClientHandler();

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
	UINT m_refCount;
};