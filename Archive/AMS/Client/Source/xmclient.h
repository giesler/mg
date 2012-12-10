// XMClient.h : main header file for the XMClient application
//
#pragma once


#include "Resources\resource.h"       // main symbols

#include "xmlib.h"

// ---------------------------------------------------------------------- Win App

class CXMClientApp : public CWinApp
{
public:
	CXMClientApp();

	char* Version();
	bool m_bHasInit;
	HANDLE m_hMutex;

//Overrides
public:
	virtual BOOL InitInstance();
	virtual int ExitInstance();

// Implementation
public:
	DECLARE_MESSAGE_MAP()
};


#include "xmquery.h"


// ---------------------------------------------------------------------- Utils

//about box
void DoAbout(CWnd *parent);
bool DoPasswordCheck();

//prefs
void DoPrefs();

//utility function
CString BuildSavedFilename(CMD5 md5);

// ---------------------------------------------------------------------- Globals

class CXMClientManager;
class CXMServerManager;
class CXMAsyncResizer;


//app helper function
inline CXMClientApp* app() {
	return (CXMClientApp*)AfxGetApp();
}
extern inline CXMClientManager* cm();
extern inline CXMServerManager* sm();
extern inline CXMAsyncResizer* ar();
