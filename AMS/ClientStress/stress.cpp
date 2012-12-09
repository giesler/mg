#include <winsock2.h>
#include <windows.h>
#include <stdio.h>
#include <string.h>
#include <time.h>
#include <process.h>

bool cont = true;

// --------------------------------------------------------------------------------- MEDIA

struct media
{
	char md5[33];
	DWORD width;
	DWORD height;
	DWORD size;

	media *next;

	static media *first;
	static int count;

	static void load(const char* path)
	{
		//open the file
		FILE* f = fopen(path, "rt");

		//read the first line
		first = new media;
		fscanf(f, "%s %u %u %u\n", first->md5, &first->width, &first->height, &first->size);
		strlwr(first->md5);
		
		//read rest of file
		count = 1;
		media *last = first;
		while(true)
		{
			//read line
			media *m = new media;
			if (EOF == fscanf(f, "%s %u %u %u\n", m->md5, &m->width, &m->height, &m->size))
			{
				delete m;
				break;
			}
			strlwr(m->md5);

			//insert into linked list
			last->next = m;
			last = m;
			count++;
		}

		//loop the list
		last->next = first;

		//close file
		fclose(f);
	}
};

media *media::first;
int media::count;

// --------------------------------------------------------------------------------- CONNECTION

namespace stats 
{
	CRITICAL_SECTION _lock;
	void lock() { EnterCriticalSection(&_lock); }
	void unlock() { LeaveCriticalSection(&_lock); }

	DWORD files = 0;
	DWORD failed = 0;
}

class connection
{
public:
	
	media *media;

	static void lock() { EnterCriticalSection(&_lock); }
	static void unlock() { LeaveCriticalSection(&_lock); }

	void free()
	{
		lock();
		__try
		{
			media = NULL;
		}
		__finally
		{
			unlock();
		}
	}

	static int count;
	static CRITICAL_SECTION _lock;
	static connection *all;
	static sockaddr_in client;

	void start()
	{
		//start the thread
		HANDLE h = (HANDLE)_beginthreadex(NULL, 0, alpha, this, 0, NULL);
		CloseHandle(h);
	}

	static UINT __stdcall alpha(LPVOID p)
	{
		//get a pointer to our connection
		connection *c = (connection*)p;
		
		//guard the entire thread, so that we can
		//free up the slot if there is an error
		__try
		{
			//connect the socket
			SOCKET s = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
			if (connect(s, (sockaddr*)&client, sizeof(client)) == SOCKET_ERROR)
			{
				//printf("Error connecting socket: %d.\n", WSAGetLastError());
				stats::lock();
				stats::failed++;
				stats::unlock();
				closesocket(s);
				return -1;
			}

			//what width should we use?
			DWORD w, h;
			if ( (c->media->width & 0xF) < 0x8 )
			{
				//full size 25%
				w = c->media->width;
				h = c->media->height;
			}
			else
			{
				//thumbnail 75%
				w = 124;
				h = 93;
			}
		
			//send the message
			char msg[1024];
			_snprintf(msg, 1023,
				"<message sequence=\"0\" reply=\"0\">\n\
				<from/>\n\
				<request for=\"file\"/>\n\
				<content format=\"text/xml\">\n\
					<field name=\"md5\">%s</field>\n\
					<field name=\"width\">%u</field>\n\
					<field name=\"height\">%u</field>\n\
				</content>\n\
				</message>\n",
				c->media->md5,
				w,
				h);
			//printf(msg);
			if (SOCKET_ERROR == send(s, msg, strlen(msg)+1, 0))
			{
				//printf("Error sending message: %d.\n", WSAGetLastError());
				closesocket(s);
				stats::lock();
				stats::failed++;
				stats::unlock();
				return -1;
			}

			//read response
			while (recv(s, msg, sizeof(msg), 0) > 0);

			shutdown(s, SD_SEND);
			closesocket(s);

			//success
			stats::lock();
			stats::files++;
			stats::unlock();
		}
		__finally
		{
			//we free no matter what
			c->free();
		}
		

		return 0;
	}

	static bool loadip(const char *ip)
	{
		//called by main
		client.sin_addr.s_addr = inet_addr(ip);
		if (client.sin_addr.s_addr == INADDR_NONE)
			return false;
		client.sin_port = htons(25347);
		client.sin_family = AF_INET;
		return true;
	}

	static bool loadall(int c)
	{
		//called by main
		if (c==0)
			return false;

		count = c;
		all = new connection[count];
		memset(all, 0, sizeof(all));
		InitializeCriticalSection(&_lock);
		InitializeCriticalSection(&stats::_lock);
		return true;
	}
};

connection*			connection::all;
CRITICAL_SECTION	connection::_lock;
int					connection::count;
sockaddr_in			connection::client;


// --------------------------------------------------------------------------------- CONTROLLER

class controller
{
public:

	void start()
	{
		//record the current time
		begin = time(NULL);

		//start the thread
		thread = (HANDLE)_beginthreadex(NULL, 0, alpha, NULL, 0, NULL);
	}

	void stop()
	{
		//mark the global continue var
		cont = false;

		//wait for the thread to exit
		if (WAIT_TIMEOUT == WaitForSingleObject(thread, 3000))
		{
			TerminateThread(thread, -1);
		}
		CloseHandle(thread);

		//wait for all other threads to exit
		bool allclosed = false;
		int c = 0;
		while (!allclosed && ++c < 30)
		{
			connection::lock();
			allclosed = true;
			for (int i=0;i<connection::count;i++)
			{
				if (connection::all[i].media)
				{
					allclosed = false;
					break;
				}
			}
			connection::unlock();
			Sleep(100);
		}
	}
		
	HANDLE thread;

	static time_t	begin;

	static UINT __stdcall alpha(LPVOID p)
	{
		media *m = media::first;
		while(cont)
		{
			//check for empty connection slots
			connection::lock();
			for (int i=0;i<connection::count;i++)
			{
				if (!connection::all[i].media)
				{
					//start a download
					connection::all[i].media = m;
					connection::all[i].start();
					Sleep(0);
				}

				//do this even for full slots, mixes up the order
				m = m->next;
			}
			connection::unlock();
			Sleep(0);
		}
		return 0;
	}
};

time_t controller::begin;

// --------------------------------------------------------------------------------- CONSOLE

class console
{
public:

	static void cmd_media()
	{
		int i = 0;
		printf("\n\tMD5\t\t\t\t\tWidth\tHeight\tSize\n");
		printf(  "\t--------------------------------\t-----\t------\t----\n");
		media *m = media::first;
		do
		{
			printf("%d:\t%s\t%u\t%u\t%u\n", ++i, m->md5, m->width, m->height, m->size);
			m = m->next;
		} while(m != media::first);
	}

	static void cmd_help()
	{
		printf("\nAvailable commands:\n");
		printf("HELP	This screen.\n");
		printf("EXIT	Quits the program.\n");
		printf("MEDIA	Displays a table of all the pictures.\n");
		printf("STATS	Display statistics for the program.\n");
	}

	static void cmd_stats()
	{
		//calculate
		printf("Calculating...");
		stats::lock();
		time_t now = time(NULL);
		double elapsed = difftime(now, controller::begin);
		double tps = stats::files / elapsed;
		stats::unlock();
		printf("done.\n");

		//print
		printf("Begin:              %s", ctime(&controller::begin));
		printf("End:                %s", ctime(&now));
		printf("Total Transactions: %u\n", stats::files);
		printf("Total Errors:       %u\n", stats::failed);
		printf("Transactions/Sec:   %.1f\n", tps);
	}

	static void loop()
	{
		char cmd[MAX_PATH];
		while(cont)
		{
			printf("\n>");
			gets(cmd);

			if (stricmp("exit", cmd)==0)
				cont = false;
			else if (stricmp("media", cmd)==0)
				cmd_media();
			else if (stricmp("help", cmd)==0)
				cmd_help();
			else if (stricmp("stats", cmd)==0)
				cmd_stats();
			else
			{
				printf("Unknown command: %s\nType 'help' for a list of commands.\n", cmd);
			}
		}
	}

};

// --------------------------------------------------------------------------------- MAIN

int main(int argc, char* argv[])
{
	//print logo
	printf("AMS Client Stress Utility\n(c)2001 Line 2 Systems, Inc.\n\n");

	//check command line
	if (argc < 4)
	{
		printf("USAGE: 'clientstress <filelist> <ip> <connections>'\n\n");
		printf("   <filelist>: Path to the the list of files.\n");
		printf("               Format is '<md5> <width> <height> <size>'. With one\n");
		printf("               file per line.\n\n");
		printf("         <ip>: Dotted ip address of the client to connect to.\n");
		printf("<connections>: Number of connections to keep open.\n");
		getchar();
		return -1;
	}

	//load media
	printf("Loading media set: %s...", argv[1]);
	media::load(argv[1]);
	printf("%d files, done.\n", media::count);

	//load winsock
	WORD wVersionRequested = MAKEWORD( 2, 2 );
	WSADATA wsaData;
	if (0 != WSAStartup(wVersionRequested, &wsaData))
	{
		printf("\nFailed to initialize Winsock 2.\n");
		return -1;
	}

	//load the ip addresss
	if (!connection::loadip(argv[2]))
	{
		printf("\nBad ip address: %s\n", argv[2]);
		getchar();
		return -1;
	}
	printf("Using client address: %s.\n", inet_ntoa(connection::client.sin_addr));

	//load the conenction count
	if (!connection::loadall(atoi(argv[3])))
	{
		printf("Invalid number of connections: %s.\n", argv[3]);
		getchar();
		return -1;
	}
	printf("Using at most %d simultaneous connections.\n", connection::count);

	//start the controller thread
	printf("Starting...");
	controller c;
	c.start();
	printf("done.\nTest is now running.\n");

	//enter the console input loop
	console::loop();
	
	//wait for the controller to exit
	c.stop();

	//cleanup
	DeleteCriticalSection(&connection::_lock);
	DeleteCriticalSection(&stats::_lock);
	WSACleanup();
	return 0;
}
