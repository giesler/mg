//------------------------------------------------------------------
//															  DB.CPP
//------------------------------------------------------------------

#include "stdafx.h"
#include "xmclient.h"
#include "xmdb.h"
#include "xmnet.h"
#include <math.h>
#include <io.h>
#include <fcntl.h>
#include <sys/types.h> 
#include <sys/stat.h>

//	------------------------------------------------------------------------- Mem Sink

CMemSink::CMemSink(SIZE_T size)
{	
	//allocate buffer and open
	BYTE* buf = (BYTE*)malloc(size);
	if (!buf) {
		throw;
	};
	Open("CMemSink", buf, size);
};

CMemSink::~CMemSink()
{
	//call our close function
	Close();
}

void CMemSink::Reset()
{
	//reset the current position
	m_nCurPos = 0;
}

void CMemSink::Close()
{
	//free buffer
	if (m_pStartData) {
		free(m_pStartData);
		m_pStartData = NULL;
	}
	CDataSink::Close();
}

BYTE* CMemSink::GetFullBuffer()
{
	return m_pStartData;
}

//helper function for sorting
int __cdecl compareBlobs(const void *elem1, const void *elem2)
{
	//cast into thumbnail pointers
	blob *blob1 = (blob*) elem1;
	blob *blob2 = (blob*) elem2;

	if (blob1->start < blob2->start) {

		//1 before 2
		return -1;
	}
	if (blob1->start == blob2->start) {
	
		//1 same as 2
		return 0;
	}

	//2 before 1 (or 1 greater than 2)
	return 1;
}


//------------------------------------------------------------------
//												 CXMDB Implemenation
//------------------------------------------------------------------



//------------------------------------------------------------------
//										 Initialization, Destruction

CXMDB::CXMDB()
{
	//initialize values
	memset(mPath, 0, MAX_PATH);
	memset(&mDiskHeader, 0, sizeof(mDiskHeader));
	mFile = NULL;
	mFiles = NULL;
	mFileCount = 0;
	mBlobs = NULL;
	mBlobCount = 0;
	mClosing = false;
	
	//setup critical section'
	InitializeCriticalSection(&mSync);
}

CXMDB::~CXMDB()
{
	//call standard free
	mClosing = true;
	if (!Free()) {
		TRACE("CXMDB::Free() failed in CXMDB::~CXMDB().\n");
	}

	//free critical section
	DeleteCriticalSection(&mSync);
}

bool CXMDB::Free()
{
	//close file if it open
	Lock();
	if (IsOpen()) {
		Close();
	}

	//free all files
	for (DWORD i=0;i<mFileCount;i++) {
		delete mFiles[i];
		mFiles[i] = NULL;
	}
	if (mFiles) {
		free(mFiles);
	}
	mFiles = NULL;
	mFileCount = 0;

	//free blobs
	FreeBlobList();

	Unlock();
	return true;
}

//------------------------------------------------------------------
//													  		    Disk

bool CXMDB::New()
{
	//put the database in a blank but valid state
	Lock();

	//must be closed
	if (IsOpen()) {
		Unlock();
		return false;
	}

	//free any existing data
	if (!Free()) {
		Unlock();
		return false;
	}

	//setup disk header structure
	memset(&mDiskHeader, 0, sizeof(mDiskHeader));
	mDiskHeader.filecount = 0;
	mDiskHeader.fileoffset = sizeof(mDiskHeader)+(1024*1024*1);
	mDiskHeader.flags = 0;
	mDiskHeader.version.major = 0;
	mDiskHeader.version.minor = 1;

	//open the file
	int ofshandle = _open(mPath, _O_BINARY | _O_RANDOM | _O_CREAT | _O_RDWR, _S_IREAD | _S_IWRITE);
	if (ofshandle<0) {
		
		//open failed
		Unlock();
		return false;
	}
	mFile = fdopen(ofshandle, "r+b");
	if (!mFile) {

		//could not map ofs handler to crt stream
		mFile = NULL;
		_close(ofshandle);
		Unlock();
		return false;
	}

	//seek forward to create space in the file
	if (fseek(mFile, mDiskHeader.fileoffset, SEEK_SET)) {
		
		//seek failed
		fclose(mFile);
		mFile = NULL;
		Unlock();
		return false;
	}

	//write header
	rewind(mFile);
	if (fwrite(&mDiskHeader, sizeof(mDiskHeader), 1, mFile)!=1) {
		
		//was not written
		fclose(mFile);
		mFile = NULL;
		Unlock();
		return false;
	}

	Unlock();
	return true;
}

bool CXMDB::Open()
{
	//open the file and read list and thumb info
	DWORD skipped = 0;
	DWORD i, j;
	Lock();

	//must be closed
	if (IsOpen()) {
		Unlock();
		return false;
	}

	//free any existing data
	if (!Free()) {
		Unlock();
		return false;
	}

	//open file
	mFile = fopen(mPath, "r+b");
	if (!mFile) goto fail;
	

	//read header
	rewind(mFile);
	if (fread(&mDiskHeader, sizeof(mDiskHeader), 1, mFile)!=1) {

		//could not read the full header, or error
		if (feof(mFile))
			TRACE("ERROR: DB file returned eof.\n");
		else
			TRACE1("ERROR: %d\n", ferror(mFile));
		goto fail;
	}

	//seek to beginning of file records
	if (fseek(mFile, mDiskHeader.fileoffset, SEEK_SET)) {

		//seek failed
		goto fail;
	}

	//allocate memory for files, plus extra
	skipped = 0;
	mFileCount = mDiskHeader.filecount; 
	mFileSize = mDiskHeader.filecount + 64;
	mFiles = (CXMDBFile**)malloc(sizeof(CXMDBFile*)*mFileSize);
	if (!mFiles) goto fail;
	memset(mFiles, 0, sizeof(CXMDBFile*)*mFileSize);

	//read file records
    CXMDBFile *file;
	for (i=0;i<mFileCount;i++) {

		//create new record
		try {
			file = new CXMDBFile(this, mFile);
		}
		catch(...) {
			delete file;
			goto fail;
		}

		//add file to list
		mFiles[i] = file;
	}

	//read thumbnail records
	CXMDBThumb* thumb;
	for (i=0;i<mFileCount;i++) {

		//read each thumbnail record
		file = mFiles[i];
		for (j=0;j<file->GetThumbCount();j++) {

			try {
				thumb = new CXMDBThumb(this, file, mFile);
			}
			catch (...) {
				delete thumb;
				goto fail;
			}

			//add thumbnail to file's record
			file->mThumbs[j] = thumb;
		}
	}

	//should we pre-cache the standard size thumbnail?
	//TODO: precache thumbnails

	Unlock();
	return true;

fail:

	if (mFile) {
		fclose(mFile);
		mFile = NULL;
	}
	if (mFiles) {
		for (i=0;i<mFileSize;i++) {
			if (mFiles[i]) {
				delete mFiles[i];
			}
		}
		free(mFiles);
		mFiles = NULL;
	}
	mFileCount = 0;
	mFileSize = 0;

	Unlock();
	return false;
}

bool CXMDB::Close()
{
	//are we open?
	Lock();
	if (!IsOpen()) {
		Unlock();
		return false;
	}

	//save any data to the disk first
	if (!Flush()) {
		Unlock();
		return false;
	}

	//close the file handle
	if (mFile) {
		fclose(mFile);
		mFile = NULL;
	}

	//free the rest of the memory
	Free();

	Unlock();
	return true;
}

bool CXMDB::Flush()
{
	DWORD i, j, s;
	CXMDBFile *file;
	CXMDBThumb *thumb;
	
	//file must be open
	Lock();
	if (!IsOpen()) {
		Unlock();
		return false;
	}

	//update the disk header
	mDiskHeader.filecount = mFileCount;

	//initialize the blob list--used by
	//findfreechunk()
	BuildBlobList();

	//write any new thumbnails,
	//find where we can start writing file records
	for (i=0;i<mFileCount;i++) {
		file = mFiles[i];
		for (j=0;j<file->mThumbCount;j++) {
			thumb = file->mThumbs[j];

			//do we need to write this thumb's image?
			if (thumb->GetFlag(DTF_NEW)) {
				
				//find a spot, seek to it, and write
				thumb->mDiskThumb.offset = FindFreeChunk(thumb->GetSize());
				if (fseek(mFile, thumb->mDiskThumb.offset, SEEK_SET))
					goto fail;
				if (fwrite(thumb->mRaw, thumb->mRawSize, 1, mFile)!=1)
					goto fail;

				//no longer new
				thumb->SetFlag(DTF_NEW, false);

				//cache the file
				if (!mClosing)
				{
					dbman()->CacheFile(CMD5(thumb->GetMD5()), CMD5(file->GetMD5()), thumb->mRaw, thumb->mRawSize, false);
				}
				else
				{
					free(thumb->mRaw);		//freed by cachefile() if not closing
				}

				//free the "raw" buffer
				thumb->mRaw = NULL;
				thumb->mRawSize = 0;
			}

			//does this one go farther?
			s = thumb->mDiskThumb.offset + thumb->mDiskThumb.size;
			if (s > mDiskHeader.fileoffset) {

				//move file offset forward
				mDiskHeader.fileoffset = s;
			}
		}
	}

	//Release blob list
	FreeBlobList();

	//write file records
	j =  0;
	if (fseek(mFile, mDiskHeader.fileoffset, SEEK_SET))
		goto fail;
	for (i=0;i<mFileCount;i++) {

		//update file disk header
		file = mFiles[i];
		file->mDiskFile.thumbnails = file->mThumbCount;

		//write record if file still exists
		if (!file->GetFlag(DFF_REMOVED))
		{
			j++;
			//TRACE1("Writing file. Known: %d\n", file->GetFlag(DFF_KNOWN));
			if (fwrite(&(file->mDiskFile), sizeof(file->mDiskFile), 1, mFile)!=1)
				goto fail;
		}
	}
	mDiskHeader.filecount = j;	//only count files that aren't removed

	//write thumbnail records
	for (i=0;i<mFileCount;i++) {
		file = mFiles[i];
		if (!file->GetFlag(DFF_REMOVED))
		{
			for (j=0;j<file->mThumbCount;j++)
			{
				thumb = file->mThumbs[j];
				if (fwrite(&(thumb->mDiskThumb), sizeof(thumb->mDiskThumb), 1, mFile)!=1)
					goto fail;
			}
		}
	}

	//write header
	if (fseek(mFile, 0, SEEK_SET))
		goto fail;
	if (fwrite(&mDiskHeader, sizeof(mDiskHeader), 1, mFile)!=1)
		goto fail;

	Unlock();
	return true;

fail:

	Unlock();
	return false;
}

//------------------------------------------------------------------
//												  Thumbnails Storage

void CXMDB::FreeBlobList()
{
	//realease internal blob pointer
	if (mBlobs) {
		free(mBlobs);
		mBlobs = NULL;
	}
}

void CXMDB::BuildBlobList()
{
	//reset the buffer
	if (mBlobs) {
		FreeBlobList();
	}

	//allocate blob buffer
	DWORD i, c=0;
	for (i=0;i<mFileCount;i++) {
		c += mFiles[i]->GetThumbCount();
	}
	if (c<1) {
		mBlobs = NULL;
		return;
	}
	mBlobs = (blob*)malloc(sizeof(blob)*c);
	if (!mBlobs)
		return;

	//fill in start and length
	DWORD j, k=0;
	CXMDBFile *file;
	CXMDBThumb *thumb;
	for (i=0;i<mFileCount;i++) {
		file = mFiles[i];
		for (j=0;j<file->GetThumbCount();j++) {
			thumb = file->GetThumb(j);
			if (!thumb->GetFlag(DTF_NEW)) {
				mBlobs[k].start = thumb->mDiskThumb.offset;
				mBlobs[k].length = thumb->mDiskThumb.size;
				k++;
			}
		}
	}
	mBlobCount = k;

	//where there any?
	if (k<1) {

		//no thumbs are currently on disk
		return;
	}

	//use crt's qsort algorithm
	qsort(mBlobs, k, sizeof(blob), compareBlobs);

	//setup linked list
	for (i=0;i<(k-1);i++) {			//skip last item
		mBlobs[i].next = i+1;
	}
	mBlobs[k-1].next = -1;
}

DWORD CXMDB::InsertBlob(DWORD after, DWORD size)
{
	//insert the given blob data, return start pos
	blob* buf = (blob*)realloc(mBlobs, sizeof(blob)*(mBlobCount+1));
	if (!buf) {
		return sizeof(mDiskHeader);
	}
	mBlobs = buf;
	mBlobCount++;

	//update list
	mBlobs[mBlobCount-1].next = mBlobs[after].next;
	mBlobs[after].next = mBlobCount-1;
	mBlobs[mBlobCount-1].start = mBlobs[after].start+mBlobs[after].length;
	mBlobs[mBlobCount-1].length = size;

	//return the new blob's index
	return mBlobs[mBlobCount-1].start;
}

DWORD CXMDB::FindFreeChunk(DWORD size)
{
	//find enough free space in the file to hold
	//<size> bytes of data

	//check the blob list
	if (!mBlobs) {

		//return offset of first byte after header
		return sizeof(mDiskHeader);
	}

	//check gap between blobs
	DWORD i=0;
	if (mBlobCount>0) {
		while (mBlobs[i].next != -1) {
			if ((mBlobs[mBlobs[i].next].start - 
				(mBlobs[i].start+mBlobs[i].length)) >=size)
			{
				//found
				return InsertBlob(i, size);
			}
			i = mBlobs[i].next;
		}
	}
	else {
		
		//blob list was empty.. insert item
		//at the front
		blob* buf = (blob*)realloc(mBlobs, sizeof(blob));
		if (!buf) {
			return sizeof(mDiskHeader);
		}
		mBlobs = buf;
		mBlobs[0].start = sizeof(mDiskHeader);
		mBlobs[0].length = size;
		mBlobs[0].next = -1;
		mBlobCount = 1;
		return sizeof(mDiskHeader);
	}

	//couldn't find a gap, insert after the
	//last blob
	return InsertBlob(i, size);
}

DWORD CXMDB::CalcWastedSpace()
{
	//file must be open
	Lock();
	if (!IsOpen()) {
		Unlock();
		return -1;
	}

	//make sorted thumbnail buffer
	BuildBlobList();
	if (!mBlobs) {
		Unlock();
		return 0;
	}

	//walk the sorted list, noting the difference
	//between (i->offset+i->size) and (i+1->offset)
	DWORD s=0, i=0;
	while (mBlobs[i].next!=-1) {
		s += (mBlobs[mBlobs[i].next].start - 
			 (mBlobs[i].start+mBlobs[i].length));
		i++;
	}

	//for the last thumb, we find the difference
	//between (offset+size) and the file list offset
	s += mDiskHeader.fileoffset -
		 (mBlobs[i].start+mBlobs[i].length);

	//success, free buffer and return count
	FreeBlobList();
	Unlock();
	return s;
}

DWORD CXMDB::CalcUsedSpace()
{
	//file must be open
	Lock();
	if (!IsOpen()) {
		Unlock();
		return -1;
	}

	//total up the size of each thumb
	DWORD s = 0, i, j;
	for (i=0;i<mFileCount;i++) {
		for (j=0;j<mFiles[i]->mThumbCount;j++) {
			s += mFiles[i]->mThumbs[j]->mDiskThumb.size;
		}
	}

	Unlock();
	return s;
}

bool CXMDB::CompactDatabase()
{
	//NOTE: this can require LOTS of memory!
	Lock();

	//file must be open
	Lock();
	if (!IsOpen()) {
		Unlock();
		return false;
	}

	//first, load the raw thumb data OUT of the file
	DWORD i, j;
	CXMDBFile *file;
	CXMDBThumb *thumb;
	for (i=0;i<mFileCount;i++) {
		file = mFiles[i];
		for (j=0;j<file->mThumbCount;j++) {
			
			//read data if needed
			thumb = file->mThumbs[j];
			if (!thumb->mRaw) {

				//allocate memory
				thumb->mRaw = (BYTE*)malloc(thumb->mDiskThumb.size);
				thumb->mRawSize = thumb->mDiskThumb.size;

				//seek, then read
				fseek(mFile, thumb->mDiskThumb.offset, SEEK_SET);
				fread(thumb->mRaw, thumb->mRawSize, 1, mFile);
			}

			//thumbnail is now 'new'
			thumb->SetFlag(DTF_NEW, true);
		}
	}

	//all thumbnails are loaded, set the file offset
	//just past the header, then flush
	mDiskHeader.fileoffset = sizeof(mDiskHeader);
	bool retval = Flush();

	//free up all the in memory thumbs
	for (i=0;i<mFileCount;i++) {
		file = mFiles[i];
		for (j=0;j<file->mThumbCount;j++) {
			thumb = file->mThumbs[j];
			if (thumb->mRaw) {
				free(thumb->mRaw);
				thumb->mRaw = NULL;
				thumb->mRawSize = 0;
			}
		}
	}

	Unlock();
	return retval;
}

//------------------------------------------------------------------
//														 File Access

bool CXMDB::InsertFile(CXMDBFile *file)
{
	//expand buffer?
	if (mFileCount>=mFileSize) {

		//allocate more mem
		DWORD s = mFileCount + 16;
		CXMDBFile **buf = (CXMDBFile**)realloc(mFiles, sizeof(CXMDBFile*)*s);
		if (!buf) {

			//not enough mem?
			return false;
		}
		mFiles = buf;
		mFileSize = s;
	}

	//copy new file pointer
	mFiles[mFileCount] = file;
	mFileCount++;
	return true;
}

CXMDBFile* CXMDB::AddFile(const char* path, CMD5& md5, BYTE* buf, DWORD bufsize, DWORD width, DWORD height)
{
	//create a new file from the given buffer
	Lock();
	CXMDBFile *file = NULL;
	try {
		file = new CXMDBFile(this, path, md5, buf, bufsize, width, height);
	}
	catch (...)
	{
		delete file;
		Unlock();
		return NULL;
	}
	
	//add file to array
	if (!InsertFile(file))
	{
		delete file;
		Unlock();
		return NULL;
	}

	//success
	Unlock();
	return file;
}

CXMDBFile* CXMDB::AddFile(const char* path)
{
	//create new file object from path
	Lock();

	//create file
	CXMDBFile *file = NULL;
	try {
		file = new CXMDBFile(this, path);
	}
	catch (...) {
		
		//failed to create
		//ASSERT(FALSE);
		delete file;
		Unlock();
		return NULL;
	}

	//add file to array
	if (!InsertFile(file))
	{
		delete file;
		Unlock();
		return NULL;
	}

	//success
	Unlock();
	return file;
}

CXMDBFile* CXMDB::FindFile(const char* path)
{
	//search for the given file
	Lock();
	for(DWORD i=0;i<mFileCount;i++)
	{
		if ((_stricmp(mFiles[i]->GetPath(), path)==0) &&
			!mFiles[i]->GetFlag(DFF_REMOVED))
		{
			Unlock();
			return mFiles[i];
		}
	}
	//TRACE1("CXMDB::FindFile() - File not found: %s\n", path);
	Unlock();
	return NULL;
}

CXMDBFile* CXMDB::FindFile(BYTE* md5)
{
	//search file list for the given md5
	Lock();
	
	CXMDBFile* temp;
	for (DWORD i=0;i<mFileCount;i++) {

		//compare
		if ((md5comp(mFiles[i]->mDiskFile.md5, md5)) &&
			!mFiles[i]->GetFlag(DFF_REMOVED)) {

			//match
			temp = mFiles[i];
			Unlock();
			return temp;
		}
	}

	//no match
	Unlock();
	return NULL;
}

//-------------------------------------------------------------------------------------------
//																	 CXMDBFile Implemenation

CXMDBFile::CXMDBFile(CXMDB *db, FILE* file)
{
	InitShared(db);
	InitFromDB(file);
}

CXMDBFile::CXMDBFile(CXMDB *db, const char* path)
{
	InitShared(db);
	if (!InitFromFile(path))
	{
		throw 0;
	}
}

CXMDBFile::CXMDBFile(CXMDB *db, const char* path, CMD5& md5, BYTE* buf, DWORD bufsize, DWORD width, DWORD height)
{
	//basic init
	InitShared(db);

	//does this file already exist
	CXMDBFile *f = db->FindFile(path);
	if (!f)
		f = db->FindFile(md5.GetValue());
	if (f)
		throw 0;
	
	//copy path, filesize, md5, widht, and height
	strncpy((char*)mDiskFile.path, path, MAX_PATH);
	mDiskFile.filesize = bufsize;
	memcpy(mDiskFile.md5, md5.GetValue(), 16);
	mDiskFile.width = width;
	mDiskFile.height = height;

	//get thumbnail from cache
	dbman()->Lock();
	DWORD i = dbman()->FindCachedFileByParent(md5);
	if (i != (DWORD)-1)
	{
		cachefile *cf = dbman()->GetCachedFile(i);
		if (cf)
		{
			//add the thumbnail record
			CXMDBThumb *thumb = NULL;
			try
			{
				thumb = new CXMDBThumb(db, this, cf->md5, cf->file, cf->size, XMGUI_THUMBWIDTH, XMGUI_THUMBHEIGHT);	
			}
			catch (...)
			{
				delete thumb;
				dbman()->Unlock();
				throw 0;
			}
			if (!InsertThumb(thumb))
			{
				delete thumb;
				dbman()->Unlock();
				throw 0;
			}
		}
	}
	dbman()->Unlock();
	//If not cached, it will get generated later
}

CXMDBFile::~CXMDBFile()
{
	//free thumbnails
	Lock();
	for (DWORD i=0;i<mThumbCount;i++) {
		delete mThumbs[i];
	}
	if (mThumbs) {
		free(mThumbs);
		mThumbs = NULL;
	}
	Unlock();
}

inline void CXMDBFile::InitShared(CXMDB *db)
{
	//basic init
	mDB = db;
	mThumbCount = 0;
	mThumbs = 0;
	memset(&mDiskFile, 0, sizeof(mDiskFile));
}

bool CXMDBFile::InitFromDB(FILE* file)
{
	//read file data from stream
	if (fread(&mDiskFile, sizeof(mDiskFile), 1, file)!=1)
		throw;

	//are we already removed?
	if (!GetFlag(DFF_REMOVED))
	{
		//check for the existance of the file
		struct _stat s;
		if (_stat((char*)mDiskFile.path, &s) != 0) {

			//file not found
			SetFlag(DFF_REMOVED, true);
			SetFlag(DFF_KNOWN, false);
			SetFlag(DFF_SIZEDIFF, false);
		}
		else {

			//has the file changed size?
			SetFlag(DFF_REMOVED, false);
			if (mDiskFile.filesize != (DWORD)s.st_size) {

				//size changed
				SetFlag(DFF_SIZEDIFF, true);
			}
			else {

				//file exists, and hasn't changed
				SetFlag(DFF_SIZEDIFF, false);
			}
		}
		//TRACE1("Loading file. Known: %d\n", GetFlag(DFF_KNOWN));
	}

	//prepare for thumbnail entry
	mThumbCount = mDiskFile.thumbnails;
	mThumbs = (CXMDBThumb**)malloc(mThumbCount*sizeof(CXMDBThumb*));
	if (!mThumbs)
		throw;

	return true;
}

bool CXMDBFile::InitFromFile(const char* path)
{
	//does this file already exist
	CXMDBFile *f = db()->FindFile(path);
	if (f)
	{
		return false;
	}

	//copy path
	strncpy((char*)mDiskFile.path, path, MAX_PATH);

	//setup disk file from the physical image file
	struct _stat s;
	if (_stat((char*)mDiskFile.path, &s) != 0) {
		
		//file not found!
		SetFlag(DFF_REMOVED, true);
		return false;
	}

	//read info from stats
	mDiskFile.filesize = s.st_size;

	//read md5
	CMD5Engine md5;
	FILE* file = fopen(path, "rb");
	if (!file) {
		return false;
	}
	md5.update(file);
	md5.finalize();
	fclose(file);
	memcpy(mDiskFile.md5, md5.raw_digest(), 16);

	//use lib jpeg to get width and height
	CJPEGDecoder jpeg;
	if (!jpeg.GetDimensions(const_cast<char*>(path), &mDiskFile.width, &mDiskFile.height))
	{
		return false;
	}

	//create standard thumbnails
	//TODO: auto-gen standard thumbs

	return true;
}

IXMLDOMElement* CXMDBFile::GetIndexXml(IXMLDOMDocument* xml)
{
	//convert this file's index into xml
	ASSERT(FALSE);
	return NULL;
}

CXMDBThumb* CXMDBFile::GetThumb(DWORD width, DWORD height)
{
	//find the given thumbnail size
	Lock();
	CXMDBThumb *thumb = NULL;
	for (DWORD i=0;i<mThumbCount;i++) {
		if ( (mThumbs[i]->GetWidth()==width) &&
			(mThumbs[i]->GetHeight()==height) ) {
			
			//match
			thumb = mThumbs[i];
			Unlock();
			return thumb;
		}
	}

	//thumbnail not found, create new one
	try {
		thumb = new CXMDBThumb(mDB, this, width, height);
	}
	catch (...) {
		
		//failed to create thumbnail
		delete thumb;
		Unlock();
		return NULL;
	}
	
	//insert the thumbnail
	if (!InsertThumb(thumb))
	{
		delete thumb;
		Unlock();
		return NULL;
	}

	//success
	Unlock();
	return thumb;
}

bool CXMDBFile::InsertThumb(CXMDBThumb *thumb)
{
	//expand buffer
	CXMDBThumb **buf = (CXMDBThumb**)realloc(mThumbs,
						sizeof(CXMDBThumb*)*(mThumbCount+1));
	if (!buf) {
	
		//no memory
		return false;
	}
	mThumbs = buf;
	mThumbCount++;

	//copy pointer
	mThumbs[mThumbCount-1] = thumb;
	return true;
}

//----------------------------------------------------------------------------------------------
//																		CXMDBThumb Implemenation

CXMDBThumb::CXMDBThumb(CXMDB *db, CXMDBFile* file, FILE* stream)
{
	InitShared(db, file);
	InitFromDB(stream);
}

CXMDBThumb::CXMDBThumb(CXMDB *db, CXMDBFile* file, DWORD width, DWORD height)
{
	InitShared(db, file);
	InitNewThumb(width, height);
}

CXMDBThumb::CXMDBThumb(CXMDB *db, CXMDBFile* file, CMD5& md5, BYTE* buf, DWORD bufsize, DWORD width, DWORD height)
{
	//standard init
	InitShared(db, file);

	//copy the image to our raw buffer
	if (mRaw) {
		free(mRaw);
	}
	mRawSize = bufsize;
	mRaw = (BYTE*)malloc(mRawSize);
	if (!mRaw) {
		throw 0;
	}
	memcpy(mRaw, buf, mRawSize);

	//build md5
	memcpy(mDiskThumb.md5, md5.GetValue(), 16);

	//set 'new' flag
	SetFlag(DTF_NEW, true);

	//setup rest of header
	mDiskThumb.width = width;
	mDiskThumb.height = height;
	mDiskThumb.offset = -1;
	mDiskThumb.size = mRawSize;
}

CXMDBThumb::~CXMDBThumb()
{
	//free any buffered image data
	if (mRaw) {
		free(mRaw);
		mRaw = NULL;
		mRawSize = 0;
	}
}

inline void CXMDBThumb::InitShared(CXMDB *db, CXMDBFile *file)
{
	mRaw = NULL;
	mRawSize = 0;
	mDB = db;
	mFile = file;
	memset(&mDiskThumb, 0, sizeof(mDiskThumb));
}

bool CXMDBThumb::InitFromDB(FILE* stream)
{
	//load thumbnail info from db
	if (fread(&mDiskThumb, sizeof(mDiskThumb), 1, stream)!=1)
		throw;

	return true;
}

bool CXMDBThumb::InitNewThumb(DWORD width, DWORD height)
{
	//decode the original file
	CWinBmp source;
	CDIBSection *dest;
	CJPEGDecoder dec;
	dec.MakeBmpFromFile(mFile->GetPath(), &source, 32);

	//billinear resize
	//CFilterResizeBilinear filter(width, height);
	//filter.ApplyInPlace(&source);
	dest = FastResize(&source, width, height);
	if (!dest)
		return false;

	//encode to local memory buffer
	CJPEGEncoder enc;
	CMemSink sink(0x8000);	//32k thumb size
	enc.SaveBmp(dest, &sink);
	delete dest;

	//copy the image to our raw buffer
	if (mRaw) {
		free(mRaw);
	}
	mRawSize = sink.GetDataSize();
	mRaw = (BYTE*)malloc(mRawSize);
	if (!mRaw) {
		return false;
	}
	memcpy(mRaw, sink.GetFullBuffer(), mRawSize);

	//build md5
	CMD5Engine md5;
	md5.update(mRaw, mRawSize);
	md5.finalize();
	memcpy(mDiskThumb.md5, md5.raw_digest(), 16);

	//set 'new' flag
	SetFlag(DTF_NEW, true);

	//setup rest of header
	mDiskThumb.width = width;
	mDiskThumb.height = height;
	mDiskThumb.offset = -1;
	mDiskThumb.size = mRawSize;
	
	return true;
}

SIZE_T CXMDBThumb::GetImage(BYTE** buffer)
{
	Lock();

	//is the file in our local buffer?
	DWORD s = -1;
	if (mRaw) {

		//use this buffer
		*buffer = mRaw;
		s = mRawSize;
	}
	else {

		//allocate memory for thumbnail
		s = mDiskThumb.size;
		*buffer = (BYTE*)malloc(s);
		if (!*buffer)
		{
			s = -1;
		}
		else
		{
			//we need to try a cache lookup next
			dbman()->Lock();
			DWORD i = dbman()->FindCachedFileByMD5(CMD5(mDiskThumb.md5));
			if (i!=-1)
			{	
				//cache hit
				cachefile *cf = dbman()->GetCachedFile(i);
				memcpy(*buffer, cf->file, s);
				dbman()->Unlock();
				Unlock();
				return s;
			}
			dbman()->Unlock();

			//cache lookup failed, we need to load the image
			//from the disk

			//database must be open
			if (!(mDB->mFile)) {

				//database closed
				free(*buffer);
				*buffer = NULL;
				s = -1;
			}
			else
			{
				fseek(mDB->mFile, mDiskThumb.offset, SEEK_SET);
				fread(*buffer, s, 1, mDB->mFile);
			}

			//update thumbnail cache
			if (!mDB->mClosing)
			{
				BYTE *tempbuf = (BYTE*)malloc(s);
				memcpy(tempbuf, *buffer, s);
				dbman()->CacheFile(CMD5(mDiskThumb.md5), CMD5(mFile->GetMD5()), tempbuf, s, FALSE);
			}
		}
	}

	Unlock();
	return s;
}

void CXMDBThumb::FreeImage(BYTE** buffer)
{
	//dont free if our internal buffer
	Lock();
	if (mRaw==*buffer) {
		*buffer = NULL;
	}
	else {
		free(*buffer);
		*buffer = NULL;
	}
	Unlock();
}

// ----------------------------------------------------------------------------------- Fast Resize

//custom resizing function
CDIBSection* FastResize(CBmp *bmpSrc, int width, int height)
{
	//verify source
	if (!bmpSrc)
	{
		ASSERT(FALSE);
		return NULL;
	}
	if (bmpSrc->GetBitsPerPixel()!=32)
	{
		ASSERT(FALSE);
		return NULL;
	}

	//calc the actual rect we resize into.. aspect ratio will match
	//the source.. we will add blank pixels/rows to fill in
	int x1 = bmpSrc->GetWidth(), y1 = bmpSrc->GetHeight();
	if (x1<1||y1<1) return NULL;
	int x2, y2;
	int topBorder = 0;
	int leftBorder = 0;
	if (((float)x1/(float)y1)>((float)width/(float)height))
	{
		x2 = width;		//width is known
		y2 = (x2*y1)/x1;
		topBorder = (height-y2)/2;
	}
	else
	{
		y2 = height;
		x2 = (y2*x1)/y1;
		leftBorder = ((width-x2)/2)*PIXEL_SIZE;
	}

	ASSERT(x2<=width);
	ASSERT(y2<=height);
	ASSERT(x2+leftBorder*2<=(width*PIXEL_SIZE));
	ASSERT(y2+topBorder*2<=height);

	//calculate samples - number of pixels in source that
	//scales to one pixel in destination
	float hSamples = (float)x1/(float)x2;
	float vSamples = (float)y1/(float)y2;

	//how far apart should we supersample?
	long supersampledistance;
	if (x2>x1)
		supersampledistance = 1;	//super sample a little when scaling up
	else if (x2*2>x1)
		supersampledistance = 0;
	else if (x2*3>x1)
		supersampledistance = 1;
	else if (x2*4>x1)
		supersampledistance = 2;
	else
		supersampledistance = 3;
	supersampledistance *= PIXEL_SIZE;

	//setup the destination bitmap
	CDIBSection *dib = new CDIBSection();
	dib->Create(width, height, 32, FALSE);

	//get a pointer to the data in the src and dst bitmaps
	long srcrowsize = bmpSrc->GetBytesPerLine();
	long destrowsize = dib->GetBytesPerLine();
	BYTE** src = bmpSrc->GetLineArray();
	BYTE** dest = dib->GetLineArray();

	//fill in the destinatio bitmap pixel by pixel
	BYTE *row;
	BYTE *srcrow;
	long srcrowcount=0;
	long destcol;
	long srccol;
	for (long destrow=topBorder;destrow<y2;destrow++)
	{
		row = dest[destrow];
		srcrow = src[srcrowcount];
		srccol = supersampledistance;
		for (destcol=0;destcol<x2*(PIXEL_SIZE);destcol+=PIXEL_SIZE)
		{
			//supersample pattern: o--o--o
			row[leftBorder+destcol+0] = (srcrow[srccol+0]+srcrow[srccol+0-supersampledistance]+srcrow[srccol+0+supersampledistance])/3;
			row[leftBorder+destcol+1] = (srcrow[srccol+1]+srcrow[srccol+1-supersampledistance]+srcrow[srccol+1+supersampledistance])/3;
			row[leftBorder+destcol+2] = (srcrow[srccol+2]+srcrow[srccol+2-supersampledistance]+srcrow[srccol+2+supersampledistance])/3;
			srccol = supersampledistance+(long)(destcol*hSamples);
			srccol -= srccol%PIXEL_SIZE;		//align on pixel

		}
		srcrowcount = (long)(destrow*vSamples);
	}

	return dib;
}


