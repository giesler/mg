//------------------------------------------------------------------
//																DB.H
//------------------------------------------------------------------
#pragma once

#include "xmquery.h"

//compare function for sorting thumbnails by file offset
int __cdecl compareBlobs(const void *elem1, const void *elem2);

//fast resizer
CDIBSection* FastResize(CBmp *bmpSrc, int width, int height);

//memory sink - thin wrapper of datasink for use in
//encoding jpeg data to memory
class CMemSink : public CDataSink
{
public:

	CMemSink(SIZE_T size);
	~CMemSink();
	void Reset();
	virtual void Close();
	BYTE* GetFullBuffer();
};

//------------------------------------------------------------------
//													 Disk Structures

struct diskheader
{
	struct {
		BYTE major;				//major version number
		BYTE minor;				//minor version number
	} version;
	BYTE		flags;			//flags
	BYTE		userid[16];		//guid of database owner
	DWORD		filecount;		//number of files in database
	DWORD		fileoffset;		//offset to files/thumbs lists
	BYTE		unused[1024];	//upwards compatability
};

//disk header flags
#define DHF_UNUSED1		0x01
#define DHF_UNUSED2		0x02
#define DHF_UNUSED3		0x04
#define DHF_UNUSED4		0x08
#define DHF_UNUSED5		0x10
#define DHF_UNUSED6		0x20
#define DHF_UNUSED7		0x40
#define DHF_UNUSED8		0x80

struct diskindex
{
	BYTE		flags;			//flags
	CXMIndex	data;
	DWORD		unused[4];		//upwards compatability
};

//disk index flags
#define DIF_DIRTY		0x01	//when set, this index is out of sync with
								//the server, and needs to be uploaded
#define DIF_UNUSED2		0x02
#define DIF_UNUSED3		0x04
#define DIF_UNUSED4		0x08
#define DIF_UNUSED5		0x10
#define DIF_UNUSED6		0x20
#define DIF_UNUSED7		0x40
#define DIF_UNUSED8		0x80

struct diskfile
{
	BYTE		md5[16];		//md5 of disk file (not thumbnail!)
	BYTE		path[MAX_PATH];	//path on disk to file
	BYTE		flags;			//file flags
	DWORD		filesize;		//logical size of file
	diskindex	index;			//index data
	DWORD		thumbnails;		//number of thumbnails
	DWORD		width;			//image width (pixels)
	DWORD		height;			//image height (pixels)
	DWORD		unused[4];		//upwards compatability
};

//disk file flags
#define DFF_REMOVED		0x01	//file was moved, renamed, or deleted
#define DFF_SIZEDIFF	0x02	//size was changed
#define DFF_KNOWN		0x04	//server knows about us
#define DFF_UNUSED4		0x08
#define DFF_UNUSED5		0x10
#define DFF_UNUSED6		0x20
#define DFF_UNUSED7		0x40
#define DFF_UNUSED8		0x80

struct diskthumb
{
	BYTE		md5[16];		//md5 of thumbnail (not original image!)
	BYTE		flags;			//thumbnail flags
	DWORD		width;			//thumbnail width, in pixels
	DWORD		height;			//thumbnail height, in pixels
	DWORD		size;			//thumbnail size in bytes (not w*h!)
	DWORD		offset;			//offset in file of thumbnail jpeg
	DWORD		unused[2];		//upwards compatability
};

//disk thumbnail flags
#define DTF_NEW			0x01	//when set, this thumbnail needs to be
								//written to disk
#define DTF_UNUSED2		0x02
#define DTF_UNUSED3		0x04
#define DTF_UNUSED4		0x08
#define DTF_UNUSED5		0x10
#define DTF_UNUSED6		0x20
#define DTF_UNUSED7		0x40
#define DTF_UNUSED8		0x80

//------------------------------------------------------------------
//													 Runtime Objects

class CXMDBThumb;
class CXMDBFile;
class CXMDB;

//---------------------------------------------
//										  Thumb

class CXMDBThumb
{
friend class CXMDB;
friend class CXMDBFile;
friend int __cdecl compareBlob(const void *elem1, const void *elem2);
public:

	//thunbnail access
	SIZE_T GetImage(BYTE** buffer);
	void FreeImage(BYTE** buffer);

	//header access
	inline BYTE*	GetMD5();
	inline BYTE		GetFlag(BYTE flag);
	inline DWORD	GetWidth();
	inline DWORD	GetHeight();
	inline DWORD	GetSize();
	inline void		SetFlag(BYTE flag, bool value);

	//sync passthroughs
	inline void Lock();
	inline void Unlock();

private:
	CXMDBThumb(CXMDB *db, CXMDBFile* file, FILE* stream);
	CXMDBThumb(CXMDB *db, CXMDBFile* file, DWORD width, DWORD height);
	CXMDBThumb(CXMDB *db, CXMDBFile* file, CMD5& md5, BYTE* buf, DWORD bufsize, DWORD width, DWORD height);
	~CXMDBThumb();

	//init functions
	inline void InitShared(CXMDB *db, CXMDBFile *file);
	bool InitFromDB(FILE* stream);
	bool InitNewThumb(DWORD width, DWORD height);

	//misc
	CXMDB* mDB;
	CXMDBFile* mFile;
	diskthumb mDiskThumb;

	//thumbnail image buffers
	BYTE* mRaw;
	SIZE_T mRawSize;
};

//---------------------------------------------
//										   File

class CXMDBFile
{
friend class CXMDB;
friend class CXMDBThumb;
public:

	//file header access
	inline BYTE*		GetMD5();
	inline char*		GetPath();
	inline bool			GetFlag(BYTE flag);
	inline DWORD		GetFileSize();
	inline diskindex*	GetIndex();
	inline void			SetFlag(BYTE flag, bool value);
	inline DWORD		GetWidth();
	inline DWORD		GetHeight();

	//thumbnail acces
	CXMDBThumb* GetThumb(DWORD width, DWORD height);
	inline DWORD GetThumbCount() {
		return mThumbCount;
	};
	inline CXMDBThumb* GetThumb(DWORD i) {
		if (i<0 || i>=mThumbCount) 
			return NULL;
		return mThumbs[i];
	};

	//xml stuff
	IXMLDOMElement* GetIndexXml(IXMLDOMDocument* xml);

	//sync passthroughs
	inline void Lock();
	inline void Unlock();

private:
	CXMDBFile(CXMDB *db, FILE* file);		//read info from db
	CXMDBFile(CXMDB *db, const char* path);		//read info from actual file
	CXMDBFile(CXMDB *db, const char* path, CMD5& md5, BYTE* buf, DWORD bufsize, DWORD width, DWORD height);
	~CXMDBFile();

	//init functions
	inline void InitShared(CXMDB *db);
	bool InitFromDB(FILE* file);
	bool InitFromFile(const char* path);

	//misc
	CXMDB* mDB;
	diskfile mDiskFile;

	//thumbnails list
	bool InsertThumb(CXMDBThumb* thumb);
	CXMDBThumb** mThumbs;
	DWORD mThumbCount;
};

//---------------------------------------------
//											 DB
struct blob {
	DWORD start;
	DWORD length;
	DWORD next;							// -1 for last blob
};

class CXMDB
{
friend class CXMDBThumb;
friend class CXMDBFile;
public:
	
	//construction, init
	CXMDB();
	~CXMDB();

	//save and load
	inline void SetPath(const char* newPath);
	inline char* GetPath();
	bool New();					//creates an empty but valid file
	bool Open();				//open handle to file
	bool Close();				//close handle to file
	bool Flush();				//write out any dirty info to disk
	bool Free();				//close and free any memory
	inline bool IsOpen() {
		Lock();
		bool temp = mFile!=NULL;
		Unlock();
		return temp;
	}

	//db size stuff
	DWORD CalcWastedSpace();		//ammount of space in images section of
	DWORD CalcUsedSpace();			//file that is used or unused
	bool CompactDatabase();

	//access to header
	inline BYTE	GetMajor();
	inline BYTE	GetMinor();
	inline bool	GetFlag(BYTE flag);
	inline BYTE* GetUserId();
	inline void	SetMajor(BYTE value);
	inline void	SetMinor(BYTE value);
	inline void	SetFlag(BYTE flag, bool value);
	inline void	SetUserId(const BYTE* value);

	//file list access
	CXMDBFile* AddFile(const char* path);
	CXMDBFile* AddFile(const char* path, CMD5& md5, BYTE* buf, DWORD bufsize, DWORD width, DWORD height);
	CXMDBFile* FindFile(BYTE* md5);
	CXMDBFile* FindFile(const char* path);
	inline DWORD GetFileCount() {
		Lock();
		DWORD temp = mFileCount;
		Unlock();
		return temp;
	};
	inline CXMDBFile* GetFile(DWORD i) {
		Lock();
		if (i<0 || i>=mFileCount) {
			Unlock();
			return NULL;
		}
		CXMDBFile* temp = mFiles[i];
		Unlock();
		return temp;
	};

	//syncronization - used by file and thumb
	inline void Lock() {
		EnterCriticalSection(&mSync);
	};
	inline void Unlock() {
		LeaveCriticalSection(&mSync);
	};
	bool mClosing;

private:

	//disk access
	char mPath[MAX_PATH];					//path to .db file
	diskheader mDiskHeader;
	FILE* mFile;							//handle to open db file

	//thumbnail blob ops
	DWORD FindFreeChunk(DWORD size);		//find big enough block
	void BuildBlobList();
	void FreeBlobList();
	DWORD InsertBlob(DWORD after, DWORD size);
	blob *mBlobs;
	DWORD mBlobCount;

	//files list
	bool InsertFile(CXMDBFile *file);
	CXMDBFile** mFiles;
	DWORD mFileCount;
	DWORD mFileSize;
	
	//misc
	CRITICAL_SECTION mSync;
};

//---------------------------------------------
//								   CXMDBManager								  

class IXMDBManagerCallback
{
public:
	//called to indicate beging and end of scan process
	virtual void OnBeginScan() =0;
	virtual void OnEndScan() =0;
	virtual void OnScanDir(const char *path) =0;
	virtual void OnProcess() =0;

	//called during scan and watch
	virtual bool OnFileFound(const char* path, CMD5* md5) =0;
	virtual bool OnFileRestored(CXMDBFile* file) =0;
	virtual bool OnFileRemoved(CXMDBFile* file) =0;

	//called after an xmfile is either reomved or added
	virtual void AfterFileAdded(CXMDBFile *file) =0;
	virtual void AfterFileRemoved(CXMDBFile *file) =0;
	virtual void OnFileAddError(const char* path, CMD5* md5) =0;
	virtual void OnFileRemoveError(CXMDBFile *file) =0;
};

struct cachefile {
	CMD5 md5;
	CMD5 parent;
	DWORD hits;
	SIZE_T size;
	BYTE* file;
	BYTE clamp;
};

class CXMDBManager
{
public:

	//initialization
	CXMDBManager();
	~CXMDBManager();
	bool DatabaseStartup();
	bool DatabaseStartupEarly();

	//cache access
	DWORD FindCachedFileByMD5(CMD5& md5);
	DWORD FindCachedFileByParent(CMD5& parent);
	DWORD GetCachedFileCount();
	cachefile* GetCachedFile(DWORD i);
	void ClampCachedFile(DWORD i);
	void UnclampCachedFile(DWORD i);
	void CacheFile(CMD5& md5, CMD5& parent, BYTE* file, SIZE_T length, BOOL clamp);
	inline SIZE_T GetMaxCacheSize();
	inline void	SetMaxCacheSize(SIZE_T size);

	//state pointers
	void SetDatabase(CXMDB *db);
	void SetCallback(IXMDBManagerCallback *callback);
	CXMDB* GetDatabase();

	//scan functions
	bool ScanDirectory(char* path);
	void CancelScan();

	//misc
	void Lock();
	void Unlock();
	char* BuildFileListing(bool full);

protected:

	//cache data
	cachefile* mCache;
	DWORD mCacheCount;			//number of valid items
	DWORD mCacheSize;			//size of buffer in cachefiles
	SIZE_T mMaxCacheSize;		//max size (in bytes) of thumb data
	SIZE_T mCurrentCacheSize;	//current size (in bytes) of thumb data

	CRITICAL_SECTION mcsLock;
	bool _ScanDirectory(CString path);
	IXMDBManagerCallback *mCallback;
	CXMDB *mDB;
	bool mCancelFlag;
};

// -------------------------------------------------------------------------- Async Resize

//resize operations
#define XMAROP_JPG	0x1			//re-encode the image
#define XMAROP_MD5	0x2			//calc and store the md5
#define XMAROP_DIB	0x4			//save the dib section

//#define XM_ASYNCRESIZE	XM_MSGBASE+6
//async resize op complete
//	wparam: op
//	lparam: work item id

class CXMAsyncResizer
{
friend DWORD WINAPI AsyncResizerThreadProc(LPVOID lpParameter);

public:
	struct WorkItem
	{
		DWORD dwId;
		HWND hWnd;
		char szPath[MAX_PATH];
		UINT uiOp;
		DWORD dwWidth;
		DWORD dwHeight;
	};

	struct CompletedItem
	{
		WorkItem mWorkItem;
		BYTE* mBuf;
		DWORD mBufCount;
		CMD5 mMd5;
		CDIBSection *mpDib;
	};

	//construction
	CXMAsyncResizer();
	~CXMAsyncResizer();

	//work items
	WorkItem* QueueItem();
	void CancelItem(DWORD id);
	void CancelWindow(HWND hWnd);
	void FlushQueue();

	//completed items
	CompletedItem* GetCompletedItem(DWORD id);
	void RemoveCompletedItem(DWORD id);

	//sync
	void Stop();
	void Lock();
	void Unlock();

private:

	//work function
	DWORD Alpha();

	//queue
	WorkItem *mpWorkItems;
	DWORD mWorkItemCount;
	DWORD mWorkItemSize;
	DWORD mLastItem;

	//completed items
	CompletedItem *mpCompletedItems;
	DWORD mCompletedItemCount;
	DWORD mCompletedItemSize;

	//threading
	CRITICAL_SECTION mcsLock;
	HANDLE mhThread;
	DWORD mdwThreadId;
	bool mbKill;

};

//------------------------------------------------------------------
//									  Thumbnail Database File Format

// |<- 24 ->|<- oo ->|<- sizeof(file)*i ->|<- sizeof(thumb)*n ->|
//   header   images	file list				thumb list

// the thumbnail list is in the order of the file list.. i.e. if
// there are 3 files each with 3, 1, and 2 thumbnails respectivly,
// then the thumbnail list looks like this:
// <f1t1><f1t2><f1t3><f2t1><f2t1><f2t2>

//------------------------------------------------------------------
//												  Inlines - Database

inline void CXMDB::SetPath(const char* newPath) {
	Lock();
	strncpy(mPath, newPath, MAX_PATH);
	Unlock();
}

inline char* CXMDB::GetPath() {
	Lock();
	char* temp = mPath;
	Unlock();
	return temp;
}

inline BYTE	CXMDB::GetMajor() {
	Lock();
	BYTE temp = mDiskHeader.version.major;
	Unlock();
	return temp;
}

inline BYTE	CXMDB::GetMinor() {
	Lock();
	BYTE temp = mDiskHeader.version.minor;
	Unlock();
	return temp;
}

inline bool	CXMDB::GetFlag(BYTE flag) {
	Lock();
	bool temp = (mDiskHeader.flags && flag);
	Unlock();
	return temp;
}

inline BYTE* CXMDB::GetUserId() {
	Lock();
	BYTE* temp = mDiskHeader.userid;
	Unlock();
	return temp;
}

inline void	CXMDB::SetMajor(BYTE value) {
	Lock();
	mDiskHeader.version.major = value;
	Unlock();
}

inline void	CXMDB::SetMinor(BYTE value) {
	Lock();
	mDiskHeader.version.minor = value;
	Unlock();
}

inline void	CXMDB::SetFlag(BYTE flag, bool value) {
	Lock();
	if (value) {
		mDiskHeader.flags |= flag;
	}
	else {
		mDiskHeader.flags &= ~flag;
	}
	Unlock();
}

inline void	CXMDB::SetUserId(const BYTE* value) {
	Lock();
	memcpy(mDiskHeader.userid, value, 16);
	Unlock();
}

//------------------------------------------------------------------
//													  Inlines - File

inline BYTE* CXMDBFile::GetMD5() {
	Lock();
	BYTE* temp = mDiskFile.md5;
	Unlock();
	return temp;
}

inline char* CXMDBFile::GetPath() {
	Lock();
	char* temp = (char*)mDiskFile.path;
	Unlock();
	return temp;
}

inline bool CXMDBFile::GetFlag(BYTE flag) {
	Lock();
	bool temp = (mDiskFile.flags & flag)?true:false;
	Unlock();
	return temp;
}

inline DWORD CXMDBFile::GetFileSize() {
	Lock();
	DWORD temp = mDiskFile.filesize;
	Unlock();
	return temp;
}

inline diskindex* CXMDBFile::GetIndex() {
	Lock();
	diskindex* temp = &mDiskFile.index;
	Unlock();
	return temp;
}

inline DWORD CXMDBFile::GetWidth() {
	Lock();
	DWORD temp = mDiskFile.width;
	Unlock();
	return temp;
}

inline DWORD CXMDBFile::GetHeight() {
	Lock();
	DWORD temp = mDiskFile.height;
	Unlock();
	return temp;
}

inline void CXMDBFile::SetFlag(BYTE flag, bool value) {
	Lock();
	if (value) {
		mDiskFile.flags |= flag;
	}
	else  {
		mDiskFile.flags &= ~flag;
	}
	Unlock();
}

inline void CXMDBFile::Lock() {
	mDB->Lock();
}

inline void CXMDBFile::Unlock() {
	mDB->Unlock();
}


//------------------------------------------------------------------
//													 Inlines - Thumb

inline BYTE* CXMDBThumb::GetMD5() {
	Lock();
	BYTE* temp = mDiskThumb.md5;
	Unlock();
	return temp;
}

inline BYTE	CXMDBThumb::GetFlag(BYTE flag) {
	Lock();
	BYTE temp = (mDiskThumb.flags && flag);
	Unlock();
	return temp;
}

inline DWORD CXMDBThumb::GetWidth() {
	Lock();
	DWORD temp = mDiskThumb.width;
	Unlock();
	return temp;
}

inline DWORD CXMDBThumb::GetHeight() {
	Lock();
	DWORD temp = mDiskThumb.height;
	Unlock();
	return temp;
}

inline DWORD CXMDBThumb::GetSize() {
	Lock();
	DWORD temp = mDiskThumb.size;
	Unlock();
	return temp;
}

inline void	CXMDBThumb::SetFlag(BYTE flag, bool value) {
	Lock();
	if (value) {
		mDiskThumb.flags |= flag;
	}
	else {
		mDiskThumb.flags &= ~flag;
	}
	Unlock();
}

inline void CXMDBThumb::Lock() {
	mDB->Lock();
}

inline void CXMDBThumb::Unlock() {
	mDB->Unlock();
}
