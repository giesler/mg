#include "stdafx.h"
#include "xmlib.h"
#include "xmnet.h"
#include "xmdb.h"
#include <process.h>
#include <io.h>

CXMDBManager::CXMDBManager()
{
	//cache
	mCache = NULL;
	mCacheCount = 0;
	mCacheSize = 0;
	mMaxCacheSize = 1024*1014*1;		//1mb
	mCurrentCacheSize = 0;

	//misc
	mCallback = NULL;
	mDB = NULL;
	InitializeCriticalSection(&mcsLock);
}

CXMDBManager::~CXMDBManager()
{
	//free all cache
	DWORD i;
	for (i=0;i<mCacheCount;i++) {
		free(mCache[i].file);
	}
	if (mCache) {
		free(mCache);
	}
	mCache = NULL;
	mCacheCount = 0;
	mCacheSize = 0;
	mCurrentCacheSize = 0;

	//misc
	mCallback = NULL;
	mDB = NULL;
	DeleteCriticalSection(&mcsLock);
}

void CXMDBManager::Lock()
{
	EnterCriticalSection(&mcsLock);
}

void CXMDBManager::Unlock()
{
	LeaveCriticalSection(&mcsLock);
}

void CXMDBManager::SetDatabase(CXMDB *db)
{
	//assign new db
	mDB = db;
}

CXMDB* CXMDBManager::GetDatabase()
{
	return mDB;
}

void CXMDBManager::SetCallback(IXMDBManagerCallback *callback)
{
	//assign new callback interface
	mCallback = callback;
}

void CXMDBManager::CancelScan()
{
	mCancelFlag = true;
}

void EnsurePathField(const char* field, char* path, const char* defval)
{
	strcpy(path, config()->GetField(field, false));
	if (stricmp(path, "")==0) 
	{
		//path not set
		BuildFilePath(path, defval);
		config()->SetField(field, path);
	}
}

bool CXMDBManager::DatabaseStartupEarly()
{
	//default path settingss
	char dbpath[MAX_PATH+1], sharepath[MAX_PATH+1], savepath[MAX_PATH+1];
	EnsurePathField(FIELD_DB_FILE, dbpath, "ams.db");
	EnsurePathField(FIELD_DB_SHARE_PATH, sharepath, "My Files\\");
	EnsurePathField(FIELD_DB_SAVE_PATH, savepath, "My Files\\");
	return true;
}

bool CXMDBManager::IsDatabaseNew()
{
	return mIsDatabaseNew;
}

bool CXMDBManager::DatabaseStartup()
{
	//db setup
	SetDatabase(db());
	mDB->SetPath(config()->GetField(FIELD_DB_FILE, false));

	//create share, save path
	CreateDirectory(config()->GetField(FIELD_DB_SHARE_PATH, false), NULL);
	CreateDirectory(config()->GetField(FIELD_DB_SAVE_PATH, false), NULL);
	
	//open database
	mIsDatabaseNew = false;
	if (!mDB->Open())
	{
		//database did not exist.. make sure we don't try auto-login
		mIsDatabaseNew = true;	

		//error opening the database, try to create
		if (!mDB->New()) 
		{
			//error creating new database
			char str[MAX_PATH];
			_snprintf(str, MAX_PATH, "Error creating databse:\n%s", config()->GetField(FIELD_DB_FILE, false));
			MessageBox(NULL, str, "Database Error", MB_OK | MB_ICONERROR);
			return false;
		}
	}
	return true;
}


bool CXMDBManager::ScanDirectory(char *path)
{
	//check database
	if (!mDB || !mCallback)
		return false;

	//if path is null, use default path
	if (!path) {
		path = config()->GetSharedFilesLocation(false);
	}

	//walk directory, add any new files
	bool temp = false;
	mCancelFlag = false;
	mCallback->OnBeginScan();
	try {
		temp = _ScanDirectory(path);
	}
	catch (...) {
		//ASSERT(FALSE);
	}

	mCallback->OnEndScan();
	if (!temp) {
		//ASSERT(FALSE);
	}
	return temp;
}

bool CXMDBManager::_ScanDirectory(const char* ipath)
{
	//massage path
	char path[MAX_PATH+1];
	strncpy(path, ipath, MAX_PATH);
	if (path[strlen(path)-1]!='\\') {
		
		//append \*.*
		 strncat(path,"\\*.*", MAX_PATH);
	}
	else {

		//only *.* needed
		strncat(path, "*.*", MAX_PATH);
	}

	//let client know what we are scanning
	mCallback->OnScanDir(path);

	//full path stuff
	int fullpathsize;
	char fullpath[MAX_PATH+1];
	fullpathsize = strlen(ipath);
	strncpy(fullpath, ipath, MAX_PATH);
	fullpath[fullpathsize] = '\\';
	fullpathsize++;

	//recursively walk folders from path
	CMD5 md5;
	CXMDBFile *xmfile;
	_finddata_t find;
	long hfile;
	char *t = NULL;
	hfile = _findfirst(path, &find);
	if (hfile)
	{
		while (hfile)
		{	
			//give client a chance to do idle processing
			//most likely a message pump
			mCallback->OnProcess();

			//should we cancel?
			if (mCancelFlag)
				return false;

			//is it dots?
			if (find.name[strlen(find.name)-1] == '.')
				goto nextfile;

			//build the enture fully qualified path
			strncpy(fullpath+fullpathsize, find.name, MAX_PATH-fullpathsize);

			//is it sub-folder?
			if (find.attrib & _A_SUBDIR)
			{
				//search path
				if (!_ScanDirectory(fullpath))
				{
					return false;
				}
				goto nextfile;
			}

			//is jpeg?
			if (strlen(find.name) < 5)
				continue; //cant be a.jpg or a.jpeg (4 and 5 chars respectivly)
			t = find.name + strlen(find.name) - 4;
			if (stricmp(t, ".jpg") != 0)
			{
				if (strlen(find.name) < 6)
					goto nextfile;	//cant be a.jpeg
				t--;
				if (stricmp(t, ".jpeg") != 0)
					goto nextfile;
			}

			//in the db already?
			xmfile = mDB->FindFile(fullpath, true);
			if (xmfile)
			{
				//file sizes must match
				if (find.size != xmfile->GetFileSize())
				{
					//file size changed since last run
					mCallback->OnFileAddError(find.name, &CMD5(xmfile->GetMD5()));
				}
				else
				{
					//if the file is both removed and known, this is a condition that
					//can only occur if it loaded from the db file and detected as a 
					//duplicate there. that means we should ignreo the file, and keep
					//the removed flag
					if (xmfile->GetFlag(DFF_REMOVED) && xmfile->GetFlag(DFF_REMOVED))
						goto nextfile;

					//restore file
					if (mCallback->OnFileRestored(xmfile))
					{
						//set flag
						xmfile->SetFlag(DFF_REMOVED, false);
						mCallback->AfterFileAdded(xmfile);
					}
				}
			}
			else
			{    
				//file not in db
				md5.FromFile(fullpath);
				if (mCallback->OnFileFound(fullpath, &md5))
				{
					//add
					xmfile = mDB->AddFile(fullpath);
					if (xmfile)
						mCallback->AfterFileAdded(xmfile);
					else
					{
						mCallback->OnFileAddError(fullpath, &md5);

						#ifdef _INTERNAL
						ErrorFiles.AddTail(fullpath);
						#endif
					}
				}
			}
			
			//next file
nextfile:
			if (-1 == _findnext(hfile, &find))
				break;
		}

		_findclose(hfile);
	}

	return true;
}

#define COMFREE(pi) if (pi) pi->Release(); pi = NULL
#define COMCALL(call) if (FAILED(call)) throw

_bstr_t bflListing("listing");
_bstr_t bflMedia("media");
_bstr_t bflMd5("md5");
_bstr_t bflWidth("width");
_bstr_t bflHeight("height");
_bstr_t bflFilesize("filesize");
_bstr_t bflAction("action");
_variant_t bflInsert("insert");
_variant_t bflRemove("remove");
_variant_t bflUpdate("update");
_bstr_t bflType("type");

char* CXMDBManager::BuildFileListing(bool full)
{
	//create xml document
	IXMLDOMDocument *xml = NULL;
	IXMLDOMDocumentFragment *fragment = NULL;
	IXMLDOMElement	*listing = NULL,
					*media = NULL,
					*index = NULL,
					*indexfield = NULL;
	IXMLDOMNode *node = NULL;

	//create document
	xml = CreateXmlDocument();
	if (!xml) return NULL;

	DWORD count, i;
	CXMDBFile *file;
	bool inlock = false;
	char *str = NULL;
	char *a;
	_bstr_t bstr;
	BSTR tempstr;
	try {

		//create fragment
		COMCALL(xml->createDocumentFragment(&fragment));

		//create listing element
		COMCALL(xml->createElement(bflListing, &listing));
		COMCALL(listing->setAttribute(bflType, full ? _variant_t("full") : _variant_t("partial")));
		COMCALL(fragment->appendChild(listing, &node));

		//walk file list
		mDB->Lock();
		inlock = true;
		count = mDB->GetFileCount();
		char szMD5[33];
		int x = 0;
		for (i=0;i<count;i++)
		{
			//if this is a partial listing, we ignore files
			//the server already knows about
			file = mDB->GetFile(i);
			if (full ||
				!file->GetFlag(DFF_KNOWN) ||
				file->GetIndex()->flags&DIF_DIRTY)
			{
				
				md5tohex(szMD5, file->GetMD5());
				COMCALL(xml->createElement(bflMedia, &media));
				COMCALL(media->setAttribute(bflMd5, _variant_t(szMD5)));
				COMCALL(media->setAttribute(bflWidth, _variant_t((long)file->GetWidth())));
				COMCALL(media->setAttribute(bflHeight, _variant_t((long)file->GetHeight())));
				COMCALL(media->setAttribute(bflFilesize, _variant_t((long)file->GetFileSize())));

				//what type of entry is this?
				if (file->GetFlag(DFF_REMOVED))
				{
					COMCALL(media->setAttribute(bflAction, bflRemove));
				}
				else if (!file->GetFlag(DFF_KNOWN))
				{
					COMCALL(media->setAttribute(bflAction, bflInsert));
				}
				else
				{
					COMCALL(media->setAttribute(bflAction, bflUpdate));
				}

				//setup index
				if (file->GetIndex()->flags&DIF_DIRTY)
				{
					COMCALL(file->GetIndex()->data.ToXml(xml, "index", &index));
					COMCALL(media->appendChild(index, &node));			
				}

				//append media
				COMFREE(node);
				COMCALL(listing->appendChild(media, &node));

				//if it wasnt known before, it is now
				file->SetFlag(DFF_KNOWN, true);
				file->GetIndex()->flags &= ~DIF_DIRTY;

				//reset the contest flag
				file->GetIndex()->data.Contest = false;

				//free com objects
				COMFREE(node);
				COMFREE(media);
				COMFREE(index);
				x++;
			}
		}

		//TRACE("*** sending files: %d\n", x);

		inlock = false;
		mDB->Unlock();

		//convert to string
		COMCALL(fragment->get_xml(&tempstr));
		a = _com_util::ConvertBSTRToString(tempstr);
		str = strdup(a);
		free(a);
		SysFreeString(tempstr);
	}
	catch (...)
	{
		if (inlock)
			mDB->Unlock();
	}

	//free memory
	COMFREE(xml);
	COMFREE(fragment);
	COMFREE(listing);
	COMFREE(media);
	COMFREE(index);
	COMFREE(indexfield);
	COMFREE(node);

	//successs
	//TRACE1("Listing: %s\n", str);
	return str;
}

//----------------------------------------------------------------------------------------------------
//																					Thumbnail Caching

SIZE_T CXMDBManager::GetMaxCacheSize()
{
	Lock();
	SIZE_T temp = mMaxCacheSize;
	Unlock();
	return temp;
}

void CXMDBManager::SetMaxCacheSize(SIZE_T size)
{
	Lock();
	mMaxCacheSize = size;
	Unlock();
}

DWORD CXMDBManager::FindCachedFileByMD5(CMD5& md5)
{
	//look for this md5
	for(DWORD i=0;i<mCacheCount;i++)
		if (mCache[i].md5.IsEqual(md5))
			return i;
	return -1;
}

DWORD CXMDBManager::FindCachedFileByParent(CMD5& parent)
{
	//look for this parent md5
	for(DWORD i=0;i<mCacheCount;i++)
		if(mCache[i].parent.IsEqual(parent))
			return i;
	return -1;
}

DWORD CXMDBManager::GetCachedFileCount()
{
	return mCacheCount;
}

void CXMDBManager::ClampCachedFile(DWORD i)
{
	//increment the clamp count
	mCache[i].clamp++;
}

void CXMDBManager::UnclampCachedFile(DWORD i)
{
	//remove a clamp count
	mCache[i].clamp--;

	//should we delete this?
	if (mCurrentCacheSize>mMaxCacheSize &&		//oversize
		mCache[i].clamp<1)						//no longer clamped
	{
		//free memory
		mCurrentCacheSize -= mCache[i].size;
		if (mCache[i].file)
			free(mCache[i].file);
	
		//push down
		memmove(&mCache[i], &mCache[i+1], mCacheCount-i-1);
		mCacheCount--;
	}
}

cachefile* CXMDBManager::GetCachedFile(DWORD i)
{
	return &mCache[i];
}

void CXMDBManager::CacheFile(CMD5& md5, CMD5& parent, BYTE* file, SIZE_T length, BOOL clamp)
{
	//1: Determine whether to cache or not
	//2: Will new file push over limit?
	//		a: If yes, determine which file to remove
	//		b: Remove file
	//		c: Repeat (2)
	//3: Copy new file
	Lock();

	//would the file be too big to cache, even if it
	//was the only one?
	if (length > mMaxCacheSize && !clamp) {
		Unlock();
		free(file);
		return;
	}

	//see if this file is already cached
	for (DWORD i=0;i<mCacheCount;i++) {
		if (md5.IsEqual(mCache[i].md5)) {

			//add a clamp?
			if (clamp)
				mCache[i].clamp++;
			Unlock();
			free(file);
			return;
		}
	}

	//file is new, remove old files until we have room
	DWORD loser;
	mCurrentCacheSize += length;
	while (mCurrentCacheSize>mMaxCacheSize)
	{
		//search for the fewist hits
		loser = -1;
		for (i=0;i<mCacheCount;i++) {
			if (mCache[i].hits < mCache[loser].hits) {
				if (mCache[i].clamp<1) {
					loser = i;				//only allow unclamped
				}
			}
		}
		
		//was there anything?
		if (loser==-1)
		{
			//only proceed if the new item is clamped
			if (clamp)
				break;
			else
			{
				Unlock();
				free(file);
				return;
			}
		}

		//free loser's memory
		if (mCache[loser].file) {
			free(mCache[loser].file);
		}
		mCurrentCacheSize -= mCache[loser].size;

		//push the rest of the items down
		memmove(&mCache[loser], &mCache[loser+1],
				sizeof(cachefile) * (mCacheCount-loser-1) );
		mCacheCount--;
	}
	//there is now size enough to fit the 
	//new file and stay under-limit.

	//expand buffer if needed
	if (mCacheCount >= mCacheSize) {
		
		//no room for another record
		DWORD s = mCacheCount+16;
		cachefile* buf = (cachefile*)realloc(mCache,
							sizeof(cachefile) * s);

		//test buffer
		if (!buf) {
			
			//out of memory?
			mCurrentCacheSize -= length;
			Unlock();
			return;
		}
		mCacheSize = s;
		mCache = buf;
	}

	//copy new item
	cachefile *cf = &mCache[mCacheCount];
	cf->md5 = md5;
	cf->parent = parent;
	cf->hits = 0;
	cf->size = length;
	cf->file = file;
	cf->clamp = clamp?1:0;
	//cf->file = (BYTE*)malloc(cf->size);
	//memcpy(cf->file, file, length);
	
	//success!
	mCacheCount++;
	Unlock();
}

// ----------------------------------------------------------------------------------------------
//																					Async Resizer

UINT __stdcall AsyncResizerThreadProc(LPVOID lpParameter)
{
	//call the alpha function
	CXMAsyncResizer* par = (CXMAsyncResizer*)lpParameter;
	return (UINT)par->Alpha();
}

CXMAsyncResizer::CXMAsyncResizer()
{
	//create critical section
	InitializeCriticalSection(&mcsLock);

	//initialize work item list
	mWorkItemCount = 0;
	mLastItem = 0;
	mWorkItemSize = 16;
	mpWorkItems = (WorkItem*)malloc(sizeof(WorkItem)*mWorkItemSize);

	//initialize completed item list
	mCompletedItemCount = 0;
	mCompletedItemSize = 16;
	mpCompletedItems = (CompletedItem*)malloc(sizeof(CompletedItem)*mCompletedItemSize);
	
	//start thread
	mbKill = false;
	mhThread = /* CreateThread */
	(HANDLE)_beginthreadex(	NULL,
							0,
							AsyncResizerThreadProc,
							(LPVOID)this,
							0,
							&mdwThreadId);
	SetThreadPriority(mhThread, THREAD_PRIORITY_BELOW_NORMAL);
}

CXMAsyncResizer::~CXMAsyncResizer()
{
	//free work item list
	Lock();
	if (mpWorkItems)
	{
		free(mpWorkItems);
		mpWorkItems = NULL;
		mWorkItemSize = 0;
		mWorkItemCount = 0;
	}
	
	//kill the thread
	mbKill = true;
	FlushQueue();

	//free comlpeted item list
	if (mpCompletedItems)
	{
		for (DWORD i=0;i<mCompletedItemCount;i++)
		{
			if (mpCompletedItems[i].mBuf)
				free(mpCompletedItems[i].mBuf);
			if (mpCompletedItems[i].mpDib)
				delete mpCompletedItems[i].mpDib;
		}
		free(mpCompletedItems);
		mpCompletedItems = NULL;
		mCompletedItemCount = 0;
		mCompletedItemSize = 0;
	}

	//wait for thread to exit
	Sleep(0);
	WaitForSingleObject(mhThread, 100);
	CloseHandle(mhThread);
	Unlock();

	//release critical section
	DeleteCriticalSection(&mcsLock);
}

void CXMAsyncResizer::Stop()
{
	//set the kill signal
	mbKill = true;
	FlushQueue();
}

CXMAsyncResizer::WorkItem* CXMAsyncResizer::QueueItem()
{
	//return the address of a new item.. not
	//threadsafe, user must Lock(), Unlock()

	//expand buffer?
	if (mWorkItemCount >= mWorkItemSize)
	{
		mWorkItemSize += 16;
		mpWorkItems = (WorkItem*)realloc(mpWorkItems, sizeof(WorkItem)*mWorkItemSize);
		if (!mpWorkItems)
			return NULL;
	}

	//get ptr to item
	WorkItem *pwi = &mpWorkItems[mWorkItemCount++];

	//setup the work item
	pwi->uiOp = 0;
	pwi->dwId = (++mLastItem);
	pwi->szPath[0] = '\0';
	pwi->hWnd = NULL;
	return pwi;
}

void CXMAsyncResizer::FlushQueue()
{
	//resume the thread if it was suspended
	Lock();
	ResumeThread(mhThread);
	Unlock();
}

void CXMAsyncResizer::CancelItem(DWORD id)
{
	//remove an item from the work queue
	Lock();
	for (DWORD i=0;i<mWorkItemCount;i++)
	{
		if(mpWorkItems[i].dwId==id)
		{
			//bump everything down
			mWorkItemCount--;
			memmove(&mpWorkItems[i], &mpWorkItems[i+1], sizeof(WorkItem)*(mWorkItemCount-i));
			break;
		}
	}
	Unlock();
}

CXMAsyncResizer::CompletedItem* CXMAsyncResizer::GetCompletedItem(DWORD id)
{
	//Not threadsafe, enclose in Lock(), Unlock()
	for (DWORD i=0;i<mCompletedItemCount;i++)
		if (mpCompletedItems[i].mWorkItem.dwId==id)
			return &mpCompletedItems[i];
	return NULL;
}

void CXMAsyncResizer::RemoveCompletedItem(DWORD id)
{
	//Not threadsafe, enclose in Lock(), Unlock()
	for (DWORD i=0;i<mCompletedItemCount;i++)
	{
		if (mpCompletedItems[i].mWorkItem.dwId==id)
		{
			//free any mem allocated for this item
			if (mpCompletedItems[i].mBuf)
				free(mpCompletedItems[i].mBuf);
			if (mpCompletedItems[i].mpDib)
				delete mpCompletedItems[i].mpDib;

			//push everything down
			mCompletedItemCount--;
			memmove(
				&mpCompletedItems[i],
				&mpCompletedItems[i+1],
				sizeof(CompletedItem)*(mCompletedItemCount-i));
			return;
		}
	}
}

void CXMAsyncResizer::CancelWindow(HWND hWnd)
{
	//cancel all downloads from a given hwnd
	Lock();
	for (DWORD i=mWorkItemCount-1;i>=0;i--)
	{
		//is this item from the right hwnd?
		if (mpWorkItems[i].hWnd==hWnd)
		{
			CancelItem(mpWorkItems[i].dwId);
		}
	}
	Unlock();
}

//alpha proc
DWORD CXMAsyncResizer::Alpha()
{
	//loop until we get the kill signal
	bool bFailed = false;
	WorkItem wi;
	WorkItem *pwi = NULL;
	CompletedItem ci;
	CompletedItem *pci = NULL;
	CJPEGDecoder jpeg;
	jpeg.SetFast(FALSE);
	CJPEGEncoder enc;
	CFilterResizeBilinear filter(0,0);
	CDIBSection bmpOriginal;
	CDIBSection *pBmp = NULL;
	CMemSink sink(0x8000);
	CMD5Engine *pMd5 = NULL;
	while (!mbKill)
	{
		//process an item
		Lock();
		if (mWorkItemCount>0)
		{
			//get first item
			pwi = &mpWorkItems[mWorkItemCount-1];
			memcpy(&wi, pwi, sizeof(WorkItem));
			pwi = NULL;

			//delete work item
			mWorkItemCount--;
			//memmove(&mpWorkItems[0], &mpWorkItems[1], sizeof(WorkItem)*mWorkItemCount);

			//let other threads go
			Unlock();

			//setup completed item
			ci.mBuf = NULL;
			ci.mBufCount = 0;
			ci.mpDib = NULL;

			//decode, resize
			bFailed = false;
			try 
			{
				//pBmp = new CDIBSection();
				jpeg.MakeBmpFromFile(wi.szPath, &bmpOriginal, 32);
				
				//filter.SetNewSize((int)wi.dwWidth, (int)wi.dwHeight);
				//filter.ApplyInPlace(pBmp);
				pBmp = FastResize(&bmpOriginal, wi.dwWidth, wi.dwHeight);
				if (!pBmp)
					throw;

				//encode to local memory buffer
				if (wi.uiOp & XMAROP_JPG)
				{
					sink.Reset();
					enc.SaveBmp(pBmp, &sink);

					//copy the image to our raw buffer
					ci.mBufCount = sink.GetDataSize();
					ci.mBuf = (BYTE*)malloc(ci.mBufCount);
					if (!ci.mBuf)
						return -1;
					memcpy(ci.mBuf, sink.GetFullBuffer(), ci.mBufCount);
					
					//build md5
					if (wi.uiOp & XMAROP_MD5)
					{
						pMd5 = new CMD5Engine();
						pMd5->update(ci.mBuf, ci.mBufCount);
						pMd5->finalize();			
						ci.mMd5 = pMd5->raw_digest();
					}
				}

				//store dib, or delete it?
				if (wi.uiOp & XMAROP_DIB)
					ci.mpDib = pBmp;
				else
					delete pBmp;
				pBmp = NULL;

			}
			catch(...)
			{
				//error
				bFailed = true;
				//TRACE1("Resize failed (%d).\n", wi.szPath);

				//free memory if we can
				if (ci.mBuf)
					free(ci.mBuf);
				ci.mBuf = NULL;
				ci.mBufCount = 0;
				ci.mMd5.Zero();
			}

			//create completed item
			Lock();
			mCompletedItemCount++;
			if (mCompletedItemCount>mCompletedItemSize)
			{
				mCompletedItemSize += 16;
				mpCompletedItems = (CompletedItem*)realloc(
										mpCompletedItems,
										sizeof(CompletedItem)*mCompletedItemSize);
				if (!mpCompletedItems)
					return -1;
			}
			pci = &mpCompletedItems[mCompletedItemCount-1];
			memcpy(&pci->mWorkItem, &wi, sizeof(WorkItem));
			pci->mBuf = ci.mBuf;
			pci->mBufCount = ci.mBufCount;
			pci->mMd5 = ci.mMd5;
			pci->mpDib = ci.mpDib;

			//send message
			PostMessage(wi.hWnd, XM_ASYNCRESIZE, (WPARAM)wi.uiOp, (LPARAM)wi.dwId);

			//done
			Unlock();	
		}
		else
		{
			//nothing to do.. sleep
			Unlock();
			SuspendThread(mhThread);
		}
	}

	return 1;
}

//sync
void CXMAsyncResizer::Lock()
{
	EnterCriticalSection(&mcsLock);
}

void CXMAsyncResizer::Unlock()
{
	LeaveCriticalSection(&mcsLock);
}
