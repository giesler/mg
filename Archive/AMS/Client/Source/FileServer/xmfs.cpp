//
//	Adult Media Swapper File Server
//	(c)2001 Line2 Systems, Inc.
//
//	Author: Nick Nystrom
//
#include "xmfs.h"

HWND ghMainWindow = NULL;
CXMSession* gpServer = NULL;
bool gbShuttingDown = false;

void ShutdownServer()
{
	gbShuttingDown = true;
	//DestroyWindow(ghMainWindow);
	PostMessage(ghMainWindow, WM_QUIT, 0, 0);
}

BOOL WINAPI ConsoleHandler(DWORD signal)
{
	char *signame;
	switch(signal)
	{
	case CTRL_C_EVENT:
		signame = "<ctrl+c> pressed";
		break;
	case CTRL_BREAK_EVENT:
		signame = "<ctrl+break> pressed";
		break;
	case CTRL_CLOSE_EVENT:
		signame = "console closed";
		break;
	case CTRL_LOGOFF_EVENT:
		signame = "user logging off";
		break;
	case CTRL_SHUTDOWN_EVENT:
		signame = "system shutting down";
		break;
	default:
		signame = "<unknown>";
	}
	LOG("Server shutting down: %s.\n", signame);
	ShutdownServer();
	return TRUE;
}

LRESULT APIENTRY MainWndProc(
    HWND hWnd, 
    UINT Msg, 
    WPARAM wParam, 
    LPARAM lParam) 
{
	LRESULT retval = 0;
	sessions()->PreviewMessage(Msg, wParam, lParam);
	switch (Msg) {

	case WM_DESTROY:
		PostQuitMessage(0);
		break;

	default:
		retval = DefWindowProc(hWnd, Msg, wParam, lParam);
	}
	sessions()->ReviewMessage(Msg, wParam, lParam);
	return retval;
}

class CScanCallback : public IXMDBManagerCallback
{
public:
	//called to indicate beging and end of scan process
	virtual void OnBeginScan() {};
	virtual void OnEndScan() {};
	virtual void OnScanDir(const char *path)
	{
		LOG(" Scanning folder: %s\n", path);
	}
	virtual void OnProcess()
	{
	}

	//called during scan and watch
	virtual bool OnFileFound(const char* path, CMD5* md5)
	{
		LOG(" Added file: %s\n", path);
		return true;
	};
	virtual bool OnFileRestored(CXMDBFile* file) { return true; };
	virtual bool OnFileRemoved(CXMDBFile* file)
	{
		LOG(" Removed file: %s\n", file->GetPath());
		return true; 
	};

	//called after an xmfile is either reomved or added
	virtual void AfterFileAdded(CXMDBFile *file) {}
	virtual void AfterFileRemoved(CXMDBFile *file) { }
	virtual void OnFileAddError(const char* path, CMD5* md5)
	{
		LOG(" Error adding file: %s\n", path);
	}
	virtual void OnFileRemoveError(CXMDBFile *file)
	{
		LOG(" Error removing file: %s\n", file->GetPath());
	}
};

class CInlineHandlerFactory : public IXMSessionHandlerFactory
{
public:
	CInlineHandlerFactory()
	{
		m_nRefCount = 1;
	}
	virtual IXMSessionHandler* CreateHandler()
	{
		return new CClientHandler();
	}
	virtual void AddRef()
	{
		m_nRefCount++;
	}
	virtual void Release()
	{
		if (--m_nRefCount<1)
			delete this;
	}
protected:
	UINT m_nRefCount;
};

void Cleanup(int level)
{
	//cleanup.. depending on what level
	LOG("Cleaning up...");
	switch (level)
	{
	case 10:
	case  9:
	case  8:
		gpServer->Release();
	case  7:
		sessions()->Stop();
	case  6:
		//no scan cleanup
	case  5:
		db()->Close();
	case  4:
		//no config cleanup
	case  3:
		DestroyWindow(ghMainWindow);
	case  2:
		WSACleanup();
	case  1:
		CoUninitialize();
	}
	//cleanup--opposite order as startup
	PRINT("done.\n");
}

int main(int argc, char* argv[])
{
	//startup message
	PRINT("Adult Media Swapper File Server\n");
	PRINT("(c)2001 Line2 Systems, Inc.\n");
	PRINT("-------------------------------\n");
	PRINT("XMLIB Version: %s\n", XMLIB_VERSION);
	PRINT("Build Date:    %s\n", __TIMESTAMP__);
	PRINT("-------------------------------\n");

	//install console handler
	SetConsoleCtrlHandler(ConsoleHandler, TRUE);
	
	//initialize com
	LOG("Initializing COM...");
	CoInitializeEx(NULL, COINIT_MULTITHREADED);	
	PRINT("done.\n");

	//initialize sockets
	LOG("Initializing network...");
	WORD wsockVer = MAKEWORD(2,2);
	WSADATA wsockData;
	if (WSAStartup(wsockVer, &wsockData)!=0)
	{
		PRINT("failed.\n");
		ERROR("Failed to initialize Winsock2.\n");
		Cleanup(1);
		return -1;
	}
	if (HIBYTE(wsockData.wVersion) != 2 ||
		LOBYTE(wsockData.wVersion) != 2)
	{
		PRINT("warning.\n");
		LOG("Unknown Winsock version.\n");
		LOG("Continuing...");
	}
	if (!CXMSession::InitOnce(NULL))
	{
		PRINT("failed.\n");
		ERROR("Failed to initialize the network layer.\n");
		Cleanup(1);
		return FALSE;
	}
	PRINT("done.\n");
	
	//start out window
	LOG("Initializing main window...");
	WNDCLASSEX w;
	w.cbSize		= sizeof(WNDCLASSEX);
	w.style			= NULL;
	w.lpfnWndProc	= MainWndProc;
	w.cbClsExtra	= 0;
	w.cbWndExtra	= 0;
	w.hInstance		= NULL;
	w.hIcon			= NULL;
	w.hCursor		= NULL;
	w.hbrBackground	= NULL;
	w.lpszMenuName	= NULL;
	w.lpszClassName	= "XMFileServer_Main";
	w.hIconSm		= NULL;
	if (!RegisterClassEx(&w))
	{
		Cleanup(2);
		ERROR("\nFailed to register window class.\n");
		return -1;
	}
	ghMainWindow = CreateWindowEx(
							NULL,				//ex style
							w.lpszClassName,	//class
							NULL,				//window name
							NULL,				//style
							0,					//x
							0,					//y
							0,					//width
							0,					//height
							NULL,				//parent window
							NULL,				//menu
							NULL,				//instance
							NULL);				//lparam
	if (!ghMainWindow)
	{
		ERROR("\nFailed to create main window.\n");
		Cleanup(2);
		return -1;
	}
	PRINT("done.\n");

	//load the configuration data
	LOG("Loading configuration data...");
	if (!config()->RegLoad())
	{
		PRINT("failed.\n");
		ERROR("Failed to load configuration data from registry.\n");
		Cleanup(3);
		return -1;
	}
	if (!config()->LoadFromFile(config()->RegGetConfigPath(), AMSVERSION)) {
		PRINT("failed.\n");
		ERROR("Failed to load configuration data from file: %s\n",
				config()->RegGetConfigPath());
		Cleanup(3);
		return -1;
	}
	if (!config()->GetFieldBool(FIELD_LOGIN_AUTO_ENABLE))
	{
		PRINT("failed.\n");
		LOG("You MUST have auto-login enabled to use the dedicated server.\n");
		LOG("Run the client and turn on auto-login, then reload the server.\n");
		Cleanup(3);
		return -1;
	}
	PRINT("done.\n");
	LOG("Read %d entries from file:\n", config()->GetFieldCount());
	LOG("\t%s\n", config()->RegGetConfigPath());

	//load database
	LOG("Loading database: %s...", config()->GetField(FIELD_DB_FILE, false));
	if (!dbman()->DatabaseStartupEarly())
	{
		PRINT("failed.\n");
		ERROR("Error creating database paths.\n");
		Cleanup(4);
		return -1;
	}
	if (!dbman()->DatabaseStartup())
	{
		PRINT("failed.\n");
		ERROR("Error loading database.\n");
		Cleanup(4);
		return -1;
	}
	PRINT("done.\n");
	LOG("\t%u entries in database.\n", db()->GetFileCount());

	//scan for new files
	LOG("Scanning folders...\n");
	CScanCallback callback;
	dbman()->SetCallback(&callback);
	if (!dbman()->ScanDirectory(NULL))
	{	
		Cleanup(5);
		ERROR("Failed to scan folders.\n");
	}
	LOG("Done scanning folders.\n");

	//begin listening
	LOG("Listening on port: %d...", config()->GetFieldLong(FIELD_NET_CLIENTPORT));
	CInlineHandlerFactory* factory = new CInlineHandlerFactory();
	sessions()->SetSessionHandlerFactory(factory);
	factory->Release();
	if (!sessions()->Listen(ghMainWindow))
	{
		PRINT("failed.\n");
		ERROR("Could not start listener socket.\n");
		Cleanup(6);
		return -1;
	}
	PRINT("done.\n");

	//open server connection
	LOG("Connecting to server: %s:%d...\n",
		config()->GetField(FIELD_SERVER_ADDRESS, false),
		config()->GetFieldLong(FIELD_SERVER_PORT));
	CServerHandler *handler = new CServerHandler();
	gpServer = new CXMSession(handler);
	handler->Release();
	if (!gpServer->Open())
	{
		ERROR("Could not create connection to the server.\n");
		Cleanup(7);
		return -1;
	}

	//enter our message loop
	MSG msg;
	BOOL retval;
	try
	{
		while ((retval = GetMessage(&msg, ghMainWindow, 0, 0)) != 0)
		{
			if (retval == -1)
			{
				//windows error
				ERROR("Error in message loop. GetMessage() returned -1.\n");
				break;
			}
			else
			{
				TranslateMessage(&msg);
				DispatchMessage(&msg);
			}
		}
	}
	catch(...)
	{
		//exception.. doesent really matter what
		LOG("Exception in message loop, exiting.\n");
	}

	//cleanup
	LOG("Closing server connection...\n");
	gpServer->Close();
	Cleanup(10);

	//exit program
	LOG("Exiting.\n");
	return 0;
}

