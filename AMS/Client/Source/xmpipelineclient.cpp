//----------------------------------------------------------------------------------
// XMPIPLINECLIENT.CPP											XMPIPLINECLIENT.CPP
//----------------------------------------------------------------------------------

#include "stdafx.h"
#include "xmclient.h"
#include "xmnet.h"
#include "xmpipeline.h"
#include "xmdb.h"

// ---------------------------------------------------------------- XMClientManager

CXMClientManager::CXMClientManager()
: CXMPipelineBase()
{
	//setup completed file buffer
	mCompletedCount = 0;
	mCompletedSize = sizeof(CompletedFile)*32;
	mCompleted = (CompletedFile*)malloc(mCompletedSize);
	memset(mCompleted, 0, mCompletedSize);

	//setup queued file buffer
	mQueueCount = 0;
	mQueueSize = sizeof(QueuedFile)*32;
	mQueue = (QueuedFile*)malloc(mQueueSize);
	memset(mQueue, 0, mQueueSize);

	//zero download slots
	mMaxDownloads = 2;
	mMaxThumbnails = 4;
	mDownloadCount = 0;
	mDownloads = NULL;

	//zero upload slots
	mMaxUploads = 4;
	mUploadSlotCount = 0;
	mUploadSlots = NULL;
}

#define XMDATARATE_28k	0
#define XMDATARATE_56k	1
#define XMDATARATE_ISDN	2
#define XMDATARATE_DSL	3
#define XMDATARATE_T1	4

bool CXMClientManager::OnInitialize()
{
	//get download slot counts -- auto?
	if (config()->GetFieldBool(FIELD_PIPELINE_AUTO_DOWNLOAD))
	{
		switch(config()->GetFieldLong(FIELD_NET_DATARATE))
		{
		case XMDATARATE_28k:
			mMaxDownloads = 1;
			mMaxThumbnails = 2;			
			break;
		case XMDATARATE_56k:
			mMaxDownloads = 2;
			mMaxThumbnails = 4;
			break;
		case XMDATARATE_ISDN:
			mMaxDownloads = 2;
			mMaxThumbnails = 8;			
			break;
		case XMDATARATE_DSL:
			mMaxDownloads = 4;
			mMaxThumbnails = 10;			
			break;
		case XMDATARATE_T1:
			mMaxDownloads = 10;
			mMaxThumbnails = 20;
			break;
		default:
			mMaxDownloads = 2;
			mMaxThumbnails = 4;
		}	
	}
	else
	{
		mMaxDownloads = (BYTE)config()->GetFieldLong(FIELD_PIPELINE_MAXFILE);
		mMaxThumbnails = (BYTE)config()->GetFieldLong(FIELD_PIPELINE_MAXTHUMB);
	}

	//setup download slots
	mDownloadCount = mMaxDownloads + mMaxThumbnails;
	mDownloads = (DownloadSlot*)malloc(mDownloadCount*sizeof(DownloadSlot));
	memset(mDownloads, 0, mDownloadCount*sizeof(DownloadSlot));
	for (BYTE i=0;i<mDownloadCount;i++) {
		mDownloads[i].mIsThumb = (i>=mMaxDownloads);
		mDownloads[i].mState = DSS_OPEN;
	}

	//get upload slots -- auto?
	if (config()->GetFieldBool(FIELD_PIPELINE_AUTO_UPLOAD))
	{
		switch(config()->GetFieldLong(FIELD_NET_DATARATE))
		{
		case XMDATARATE_28k:
			mMaxUploads = 1;
			break;
		case XMDATARATE_56k:
			mMaxUploads = 2;
			break;
		case XMDATARATE_ISDN:
			mMaxUploads = 3;
			break;
		case XMDATARATE_DSL:
			mMaxUploads = 4;
			break;
		case XMDATARATE_T1:
			mMaxUploads = 20;
			break;
		default:
			mMaxUploads = 2;
		}	
	}
	else
	{
		mMaxUploads = (BYTE)config()->GetFieldLong(FIELD_PIPELINE_MAXUP);
	}
	
	//setup the upload slots
	mUploadSlotCount = mMaxUploads;
	mUploadSlots = (UploadSlot*)malloc(mUploadSlotCount*sizeof(UploadSlot));
	memset(mUploadSlots, 0, mUploadSlotCount*sizeof(UploadSlot));
	for (i=0;i<mUploadSlotCount;i++)
	{
		mUploadSlots[i].mState = USS_OPEN;
	}

	//start listening
	if (!sessions()->Listen(mhWnd)) {
		return false;
	}

	//start a timer.. we use this to time-out download slots
	SetTimer(mhWnd, 4565, 1000, NULL);

	//success
	return true;
}

CXMClientManager::~CXMClientManager()
{
	//free the various buffers
	if (mCompleted)
	{
		for (DWORD i=0;i<mCompletedCount;i++)
		{
			if (mCompleted[i].mBuffer)
				free(mCompleted[i].mBuffer);
		}
		free(mCompleted);	
	}
	if (mQueue) {
		for (DWORD i=0;i<mQueueCount;i++) {
			if (mQueue[i].mItem)
				mQueue[i].mItem->Release();
		}
		free(mQueue);
	}
	if (mDownloads) {
		for (DWORD i=0;i<mDownloadCount;i++) {
			if (mDownloads[i].mItem)
				mDownloads[i].mItem->Release();
			if (mDownloads[i].mSession)
			{
				mDownloads[i].mSession->Close();
				mDownloads[i].mSession->Release();
			}
		}
		free(mDownloads);
	}
	if (mUploadSlots) free(mUploadSlots);
}

void CXMClientManager::OnWin32MsgPreview(UINT msg, WPARAM wparam, LPARAM lparam)
{
	//forward messages to the session manager
	sessions()->PreviewMessage(msg, wparam, lparam);

	//timer message?
	if (msg == WM_TIMER)
	{
		//is it for us?
		if (wparam == 4565)
		{
			//increment the statecount property for
			//each download that is closing
			Lock();
			for (BYTE i=0;i<mDownloadCount;i++)
			{
				if (mDownloads[i].mState == DSS_CLOSING)
				{
					//only 5 seconds to close
					if (++(mDownloads[i].mState) > 5)
					{
						//clear out the slot
						ClearDownload(i);
					}
				}
			}
			Unlock();
		}
	}
}

void CXMClientManager::OnWin32MsgReview(UINT msg, WPARAM wparam, LPARAM lparam)
{
	//forward messages to the session manager
	sessions()->ReviewMessage(msg, wparam, lparam);
}

void CXMClientManager::OnMsgReceived(CXMSession *ses, CXMMessage *msg)
{
	try
	{

	//what type of message?
	CXMMessage *response;
	if (strcmp(msg->GetFor(false), XMMSG_PING)==0)
	{
		//respond to ping
		response = msg->CreateReply();
		response->GetField("success")->SetValue("1");
		response->Send();
	}
	else if (strcmp(msg->GetFor(false), XMMSG_FILE)==0)
	{
		if (msg->GetType()==XM_REQUEST)
		{
			//file request
			/// DEBUG {
			try
			{
				OnFileRequest(ses, msg);
			}
			catch(...)
			{
				sm()->FakeMotd("OnFileRequest Exception");
			}
			/// } DEBUG
		}
		else
		{
			//what type of request?
			if (strcmp(msg->GetField("message")->GetValue(false), "file")==0)
			{
				/// DEBUG {
				try
				{
					OnFileReceived(ses, msg);
				}
				catch(...)
				{
					sm()->FakeMotd("OnFileReceived Exception");
				}
				/// } DEBUG
			}
			else
			{
				//assume "busy" or "error"
				/// DEBUG {
				try
				{
					OnFileBusy(ses, msg);
				}
				catch(...)
				{
					sm()->FakeMotd("OnFileBusy Exception");
				}
				/// } DEBUG
			}
		}
	}
	else
	{
		//unknown message type
		ses->Close();
	}

	//always delete message
	delete msg;

	}
	catch(...)
	{
		sm()->FakeMotd("Exception in OnMsgReceived");
	}
}

void CXMClientManager::OnMsgSent(CXMSession *ses, CXMMessage *msg)
{
	//what type of message?
	if (strcmp(msg->GetFor(false), XMMSG_PING)==0)
	{
		//ping sent, close session
		ses->Close();
	}
	else if (strcmp(msg->GetFor(false), XMMSG_FILE)==0)
	{
		if (msg->GetType()==XM_REQUEST)
		{
			//file request sent, now we wait
		}
		else
		{
			//response was sent, close session
			ses->Close();

			//NOTE: the upload slot will be cleared when
			//the session's state changes.
		}
	}
	else
	{
		//unknown message type
		ses->Close();
	}

	//always delete message
	delete msg;
}

void CXMClientManager::OnStateChange(CXMSession *ses, UINT vold, UINT vnew)
{
	//what is the new state?
	BYTE i;
	CXMMessage *msg;
	switch(vnew)
	{
	case XM_OPEN:

		//is this a download connection?
		Lock();
		for(i=0;i<mDownloadCount;i++)
		{
			if ((mDownloads[i].mSession==ses) && 
				(mDownloads[i].mState==DSS_WAITING))
			{
				//we can go ahead and send the request now
				msg = new CXMMessage(mDownloads[i].mSession);
				msg->SetFor("file");
				msg->SetContentFormat("text/xml");
				msg->SetType(XM_REQUEST);
				msg->GetField("md5")->SetValue(mDownloads[i].mItem->mMD5.GetString());
				msg->GetField("width")->SetValue(mDownloads[i].mWidth);
				msg->GetField("height")->SetValue(mDownloads[i].mHeight);
				msg->Send();

				//send client update
				SendUpdate(	XM_CMU_DOWNLOAD_REQUESTING,
							mDownloads[i].mItem->mMD5,
							mDownloads[i].mWidth,
							mDownloads[i].mHeight,
							mDownloads[i].mIsThumb,
							0);

				//set new state
				mDownloads[i].mState = DSS_RECEIVING;
				break;
			}
		}
		Unlock();
		break;

	case XM_OPENING:
		break;

	case XM_CLOSING:
		break;

	case XM_CLOSED:
		
		//if this session was active in any slot, we need
		//to clear that slot for others
		Lock();

			//has this session been uploading?
			for(i=0;i<mUploadSlotCount;i++)
			{
				if ((mUploadSlots[i].mSession == ses) && 
					(mUploadSlots[i].mState != USS_OPEN))
				{
					//free up this slot
					mUploadSlots[i].mState = USS_OPEN;
					if (mUploadSlots[i].mSession)
					{
						mUploadSlots[i].mSession->Release();
						mUploadSlots[i].mSession = NULL;
					}

					//send the update
					SendUpdate(XM_CMU_UPLOAD_FINISH, mUploadSlots[i].mId, 0, 0, mUploadSlots[i].mIsThumb, 0);
				}
			}
			
			//has this session been downloading files?
			for(i=0;i<mDownloadCount;i++)
			{
				if(mDownloads[i].mSession==ses)
				{
					//if the dl_finish hasn't already been
					//received, then this is an error
					if(mDownloads[i].mState!=DSS_CLOSING)
					{
						//mark the current host busy
						mDownloads[i].mItem->mHosts[mDownloads[i].mCurrentHost].IsBusy = true;

						//try again
						mDownloads[i].mState = DSS_WAITING;
						BeginDownload(i);
					}
					else
					{
						ClearDownload(i);
					}
				}
			}
			
		Unlock();
		break;
	}
}

void CXMClientManager::OnFileReceived(CXMSession *ses, CXMMessage *msg)
{
	//extract data from message
	CMD5 md5(msg->GetField("md5")->GetValue(false));
	DWORD width = atol(msg->GetField("width")->GetValue(false));
	DWORD height = atol(msg->GetField("height")->GetValue(false));
	
	//are we looking for this file?
	Lock();
	DWORD x;
	for (int i=0;i<mDownloadCount;i++)
	{
		if ((mDownloads[i].mSession==ses) &&
			(mDownloads[i].mItem->mMD5.IsEqual(md5)) &&
			(mDownloads[i].mWidth==width) &&
			(mDownloads[i].mHeight==height) &&
			(mDownloads[i].mState!=DSS_OPEN))
		{
			//move to completed list
			x = AddCompletedFile();
			mCompleted[x].mIsThumb = mDownloads[i].mIsThumb;
			mCompleted[x].mMD5 = md5;
			mCompleted[x].mWidth = width;
			mCompleted[x].mHeight = height;
			mCompleted[x].mBufferSize = msg->GetBinarySize();
			mCompleted[x].mBuffer = (BYTE*)malloc(msg->GetBinarySize());
			msg->GetBinaryData(	mCompleted[x].mBuffer,
								mCompleted[x].mBufferSize);

			//send ui update
			SendUpdate(	XM_CMU_DOWNLOAD_FINISH,
						mDownloads[i].mItem->mMD5,
						mDownloads[i].mWidth,
						mDownloads[i].mHeight,
						mDownloads[i].mIsThumb,
						0);
			SendUpdate(	XM_CMU_COMPLETED_ADD,
						mCompleted[x].mMD5,
						mCompleted[x].mWidth,
						mCompleted[x].mHeight,
						mCompleted[x].mIsThumb,
						0);

			//free slot
			ClearDownload(i);	
		}
	}
	Unlock();

	//we dont need the session anymore
	ses->Close();
}

void CXMClientManager::OnFileBusy(CXMSession *ses, CXMMessage *msg)
{
	//server could not send us the file because
	//it was too busy... we should try another server
	CMD5 md5(msg->GetField("md5")->GetValue(false));
	DWORD width = atol(msg->GetField("width")->GetValue(false));
	DWORD height = atol(msg->GetField("height")->GetValue(false));

	//find the download slot
	Lock();
	CXMQueryResponseItem* item;
	for (DWORD i=0;i<mDownloadCount;i++)
	{
		if ((mDownloads[i].mSession==ses) &&
			(mDownloads[i].mItem->mMD5.IsEqual(md5)) /* &&
			(mDownloads[i].mWidth==width) &&
			(mDownloads[i].mHeight==height) &&
			(mDownloads[i].mState!=DSS_OPEN)*/)
		{
			//mark the current host busy
			item = mDownloads[i].mItem;
			item->mHosts[mDownloads[i].mCurrentHost].IsBusy = true;

			//close the current session
			ses->Close();
			//ses->Release();

			//try again
			mDownloads[i].mState = DSS_WAITING;
			BeginDownload(i);

			Unlock();
			return;
		}
	}
	Unlock();
}

void CXMClientManager::OnFileRequest(CXMSession *ses, CXMMessage *msg)
{
	//extract data from message
	
	CMD5 md5(msg->GetField("md5")->GetValue(false));
	DWORD width = atol(msg->GetField("width")->GetValue(false));
	DWORD height = atol(msg->GetField("height")->GetValue(false));

	//try to find this file
	Lock();
	db()->Lock();
	CXMMessage *reply;
	CXMDBFile *f = db()->FindFile(md5.GetValue(), false);
	db()->Unlock();

	/// DEBUG {
	try {

	if (!f)
	{
		//we dont have it
		reply = msg->CreateReply();
		reply->GetField("message")->SetValue("error");
		reply->GetField("error")->SetValue("File not found.");
		reply->GetField("md5")->SetValue(md5.GetString());
		reply->Send();
		ses->Close();
		Unlock();
		return;
	}

	//is it shared?
	if (f->GetFlag(DFF_REMOVED))
	{
		//we dont have it
		reply = msg->CreateReply();
		reply->GetField("message")->SetValue("error");
		reply->GetField("error")->SetValue("File not found.");
		reply->GetField("md5")->SetValue(md5.GetString());
		reply->Send();
		ses->Close();
		Unlock();
		return;
	}
	
	
	}
	catch(...)
	{
		Unlock();
		sm()->FakeMotd("Exception in OnFileRequest Setup");
		return;
	}
	/// } DEBUG

	//do we have an open spot?
	for(BYTE i=0;i<mUploadSlotCount;i++)
	{
		if (mUploadSlots[i].mState == USS_OPEN)
		{
			//create reply message
			
			/// DEBUG {
			try {
			
			reply = msg->CreateReply();
			reply->GetField("md5")->SetValue(CMD5(f->GetMD5()).GetString());
			reply->GetField("message")->SetValue("file");

			//is this a thumbnail request?
			mUploadSlots[i].mIsThumb =	((f->GetWidth() != width) ||
										 (f->GetHeight() != height)) &&
										(width!=0 && height!=0);

			}
			catch(...)
			{
				Unlock();
				sm()->FakeMotd("Exception in OnFileRequest: Create Reply");
				return;
			}
			/// } DEBUG

			if (mUploadSlots[i].mIsThumb)
			{
				//send as thumbnail
				/// DEBUG {
				try {

				CXMDBThumb *t = f->GetThumb(width, height);
				if (!t) 
				{
					//could not convert
					delete reply;
					reply = msg->CreateReply();
					reply->GetField("message")->SetValue("error");
					reply->GetField("error")->SetValue("Unable to create thumbnail.");
					reply->GetField("md5")->SetValue(md5.GetString());
					reply->Send();
					ses->Close();
					Unlock();
					return;
				}

				//attach thumbnail
				BYTE* buf;
				DWORD bufsize = t->GetImage(&buf);
				reply->SetBinaryData(buf, bufsize);
				t->FreeImage(&buf);

				//set misc fields
				reply->GetField("width")->SetValue(t->GetWidth());
				reply->GetField("height")->SetValue(t->GetHeight());
				reply->GetField("size")->SetValue(t->GetSize());

				//send 
				reply->Send();

				}
				catch(...)
				{
					Unlock();
					sm()->FakeMotd("Exception in OnFileRequest: Thumbnail");
					return;
				}
				/// } DEBUG
			}
			else
			{
				//send full size

				/// DEBUG {
				try
				{

				reply->GetField("width")->SetValue(f->GetWidth());
				reply->GetField("height")->SetValue(f->GetHeight());
				reply->GetField("size")->SetValue(f->GetFileSize());

				//attach file
				if (!reply->AttachFile(f->GetPath()))
				{
					//file deleted?
					delete reply;
					reply = msg->CreateReply();
					reply->GetField("message")->SetValue("error");
					reply->GetField("error")->SetValue("Error sending file.");
					reply->GetField("md5")->SetValue(md5.GetString());
					reply->Send();
					Unlock();
					return;
				}

				//if this is an internal build, we need to manually
				//set the md5 of the file.. we just hope that it is
				//what it should be!
				#ifdef _INTERNAL
				memcpy(reply->mBinaryMD5, md5.GetValue(), 16);
				#endif

				//check the md5 of the file against what we expect
				if(!md5comp(reply->GetBinaryMD5(), md5.GetValue()))
				{
					//don't match.  let the client know
					delete reply;
					reply = msg->CreateReply();
					reply->GetField("message")->SetValue("error");
					reply->GetField("error")->SetValue("Bad MD5 on disk.");
					reply->GetField("md5")->SetValue(md5.GetString());
					reply->Send();
					Unlock();
					return;
				}

				//send file
				reply->Send();

				}
				catch(...)
				{
					Unlock();
					sm()->FakeMotd("Esception in OnFileRequest: File");
					return;
				}
				/// } DEBUG
			}

			/// DEBUG {
			try
			{

			//do we need to clear the session currently
			//in this slot?
			if (mUploadSlots[i].mSession)
			{
				mUploadSlots[i].mSession->Release();
			}

			//setup our slot's state
			mUploadSlots[i].mState = USS_SENDINGFILE;
			mUploadSlots[i].mSession = ses;
			mUploadSlots[i].mMD5 = md5;
			mUploadSlots[i].mId.Random();
			ses->AddRef();

			//update ui
			SendUpdate(XM_CMU_UPLOAD_START, mUploadSlots[i].mId, 0, 0, mUploadSlots[i].mIsThumb, 0);

			//file sent, exit
			Unlock();
			return;

			}
			catch(...)
			{
				Unlock();
				sm()->FakeMotd("Esception in OnFileRequest: Finish");
				return;
			}
			/// } DEBUG

		}
	}
	Unlock();

	/// DEBUG {
	try
	{

	//no open slots
	reply = msg->CreateReply();
	reply->GetField("message")->SetValue("busy");
	reply->GetField("md5")->SetValue(md5.GetString());
	reply->Send();
	ses->Close();

	}
	catch(...)
	{
		sm()->FakeMotd("Esception in OnFileRequest: Busy");
		return;
	}
	/// } DEBUG
}

DWORD CXMClientManager::AddCompletedFile()
{
	//is there allocated room?
	Lock();
	DWORD temp;
	CompletedFile *buf;
	DWORD bufsize;
	mCompletedCount++;
	if (mCompletedCount*sizeof(CompletedFile) >= mCompletedSize)
	{
		//need to reallocate the buffer
		bufsize = (mCompletedCount+8)*sizeof(CompletedFile);
		buf = (CompletedFile*)realloc(mCompleted, bufsize);
		if (!buf) {
			//bad
			Unlock();
			return 0;
		}
		mCompletedSize = bufsize;
		mCompleted = buf;
	}

	//zero the last item, and return
	memset(&mCompleted[mCompletedCount-1], 0, sizeof(CompletedFile));
	temp = mCompletedCount-1;
	Unlock();
	return temp;
}

void CXMClientManager::BeginDownload(DWORD i)
{
	//is this a valid slot to download in?
	Lock();
	if (mDownloads[i].mState != DSS_WAITING)
	{
		Unlock();
		return;
	}

	//find a new host
	if ((mDownloads[i].mCurrentHost=
		mDownloads[i].mItem->FindHost())
		== (BYTE)-1)
	{
		//error, update ui
		SendUpdate(	XM_CMU_DOWNLOAD_ERROR,
					mDownloads[i].mItem->mMD5,
					mDownloads[i].mWidth,
					mDownloads[i].mHeight,
					mDownloads[i].mIsThumb,
					0);

		//all hosts busy
		ClearDownload(i);
		Unlock();
		return;
	}

	//open a new session, the download request will
	//begin when the session is connected
	mDownloads[i].mSession = OpenSession(mDownloads[i].mItem->
		mHosts[mDownloads[i].mCurrentHost].Ip, config()->GetFieldLong(FIELD_NET_CLIENTPORT));

	TRACE("Downloading from host: %s (%d of %d)\n",
			mDownloads[i].mItem->mHosts[mDownloads[i].mCurrentHost].Ip,
			mDownloads[i].mCurrentHost+1,
			mDownloads[i].mItem->mHostsCount);

	//clamp the thumbnail
	if (!mDownloads[i].mIsThumb)
	{
		dbman()->Lock();
		DWORD x = dbman()->FindCachedFileByParent(mDownloads[i].mItem->mMD5);
		if (x!=-1)
		{
			dbman()->ClampCachedFile(x);
		}
		dbman()->Unlock();
	}

	Unlock();
}

void CXMClientManager::ClearDownload(DWORD i)
{
	//reset the slot, then give the queue algorithm
	//a chance to start a new download
	Lock();
	if (mDownloads[i].mItem) {
		mDownloads[i].mItem->Release();
		mDownloads[i].mItem = NULL;
	}
	mDownloads[i].mState = DSS_OPEN;
	if (mDownloads[i].mSession)
	{
		mDownloads[i].mSession->Release();
		mDownloads[i].mSession = NULL;
	}
	FlushQueue();
	Unlock();
}

DWORD CXMClientManager::AddQueuedFile()
{
	//is there allocated room?
	Lock();
	DWORD temp;
	QueuedFile *buf;
	DWORD bufsize;
	mQueueCount++;
	if (mQueueCount*sizeof(QueuedFile) >= mQueueSize)
	{
		//need to reallocate the buffer
		bufsize = (mQueueCount+8)*sizeof(QueuedFile);
		buf = (QueuedFile*)realloc(mQueue, bufsize);
		if (!buf) {
			//bad
			Unlock();
			return 0;
		}
		mQueueSize = bufsize;
		mQueue = buf;
	}	

	//zero the last item, and return
	memset(&mQueue[mQueueCount-1], 0, sizeof(QueuedFile));
	temp = mQueueCount-1;
	Unlock();
	return temp;
}

// ---------------------------------------------------------------- Public Interface


DWORD CXMClientManager::EnqueueFile(CXMQueryResponseItem* item,
									bool thumb,
									DWORD width,
									DWORD height)
{
	//do we already have this item?
	db()->Lock();
	CXMDBFile *file = db()->FindFile(item->mMD5.GetValue(), false);
	if (file)
	{
		//if its a thumbnail, just pretend like we downloaded it
		if (thumb)
		{
			//get the proper sized thumbnail
			CXMDBThumb *t = file->GetThumb(width, height);
			if (t)
			{
				//get thumbnail data
				BYTE *buf;
				DWORD bufsize;
				bufsize = t->GetImage(&buf);

				//insert into completed
				DWORD i = AddCompletedFile();
				mCompleted[i].mIsThumb = thumb;
				mCompleted[i].mMD5 = item->mMD5;
				mCompleted[i].mWidth = width;
				mCompleted[i].mHeight = height;

				//copy data
				mCompleted[i].mBufferSize = bufsize;
				mCompleted[i].mBuffer = (BYTE*)malloc(bufsize);
				memcpy(mCompleted[i].mBuffer, buf, bufsize);
				t->FreeImage(&buf);

				//send ui update
				SendUpdate(	XM_CMU_COMPLETED_ADD,
							mCompleted[i].mMD5,
							mCompleted[i].mWidth,
							mCompleted[i].mHeight,
							mCompleted[i].mIsThumb,
							0);				

				//done
				db()->Unlock();
				return -1;
			}
		}
		else
		{
			//not a thumbnail, just ignore the request
			db()->Unlock();
			return -1;
		}
	}
	db()->Unlock();

	//can we simply put it into a download slot?
	DWORD i;
	for (i=0;i<mDownloadCount;i++)
	{
		if(	(mDownloads[i].mIsThumb==thumb) &&
			(mDownloads[i].mState==DSS_OPEN))
		{
			//put right into slot
			item->AddRef();
			mDownloads[i].mItem = item;
			if (thumb)
			{
				mDownloads[i].mWidth = width;
				mDownloads[i].mHeight = height;
			}	
			else
			{	
				mDownloads[i].mWidth = item->mWidth;
				mDownloads[i].mHeight = item->mHeight;
			}
			mDownloads[i].mSession = NULL;
			mDownloads[i].mCurrentHost = 0;

			//update ui
			SendUpdate(XM_CMU_DOWNLOAD_START, item->mMD5, width, height, thumb, 0);

			//begin downloading
			mDownloads[i].mState = DSS_WAITING;
			BeginDownload(i);
			return -1;
		}
	}

	//create new record
	i = AddQueuedFile();
	if (i==-1) {
		return i;
	}

	//fill out ticket
	item->AddRef();
	mQueue[i].mIsThumb = thumb;
	mQueue[i].mItem = item;
	if (thumb) {
		mQueue[i].mWidth = width;
		mQueue[i].mHeight = height;
	}
	else {
		mQueue[i].mWidth = item->mWidth;
		mQueue[i].mHeight = item->mHeight;
	}

	//update ui
	SendUpdate(	XM_CMU_QUEUE_ADD,
				item->mMD5,
				width,
				height,
				thumb,
				0);

	//return our new index
	return i;
}

void CXMClientManager::FlushQueue()
{
	//search for open download slots
	Lock();
	for (BYTE i=0;(i<mDownloadCount)&&(mQueueCount>0);i++)
	{
		//is empty?
		if (mDownloads[i].mState==DSS_OPEN)
		{
			//find a queue item that matches (thumb/file)
			for (DWORD j=0;j<mQueueCount;j++)
			{
				if (mDownloads[i].mIsThumb==mQueue[j].mIsThumb)
				{
					//fill out download ticket
					mDownloads[i].mItem = mQueue[j].mItem;
					mDownloads[i].mWidth = mQueue[j].mWidth;
					mDownloads[i].mHeight = mQueue[j].mHeight;
					mDownloads[i].mSession = NULL;
					mDownloads[i].mCurrentHost = 0;

					//beging downloading
					mDownloads[i].mState = DSS_WAITING;
					BeginDownload(i);

					//update ui
					SendUpdate( XM_CMU_QUEUE_REMOVE,
								mQueue[j].mItem->mMD5,
								mQueue[j].mWidth,
								mQueue[j].mHeight,
								mQueue[j].mIsThumb,
								XMPUF_QFLUSH);
					SendUpdate(	XM_CMU_DOWNLOAD_START,
								mDownloads[i].mItem->mMD5,
								mDownloads[i].mWidth,
								mDownloads[i].mHeight,
								mDownloads[i].mIsThumb,
								XMPUF_QFLUSH);

					//QFLUSH lets the ui know that the item is being removed from the
					//queue because it is beginning download.

					//remove item from the queue
					memmove(&mQueue[j], &mQueue[j+1], (mQueueCount-(j+1))*sizeof(QueuedFile));
					mQueueCount--;
					break;
				}
			}
		}
	}
	Unlock();
}

void CXMClientManager::CancelQueuedFile(DWORD i)
{
	//release the thumbnail clamp
	Lock();
	if (!mQueue[i].mIsThumb)
	{
		dbman()->Lock();
		DWORD x = dbman()->FindCachedFileByParent(mQueue[i].mItem->mMD5);
		if (x != -1)
		{
			dbman()->UnclampCachedFile(x);
		}
		dbman()->Unlock();
	}

	//update the ui
	SendUpdate(	XM_CMU_QUEUE_REMOVE,
				mQueue[i].mItem->mMD5,
				mQueue[i].mWidth,
				mQueue[i].mHeight,
				mQueue[i].mIsThumb,
				0);
	mQueue[i].mItem->Release();

	//remove from the queue
	memmove(&mQueue[i], &mQueue[i+1], (mQueueCount-(i+1))*sizeof(QueuedFile));
	mQueueCount--;
	Unlock();
}

void CXMClientManager::CancelDownloadingFile(DWORD i)
{
	//simply close the session.. let the
	//state change handle the rest
	Lock();
	if (mDownloads[i].mState==DSS_RECEIVING ||
		mDownloads[i].mState==DSS_WAITING)
	{
		//unclamp if its a thumb
		if (mDownloads[i].mIsThumb)
		{
			dbman()->Lock();
			DWORD x = dbman()->FindCachedFileByParent(mDownloads[i].mItem->mMD5);
			if (x != -1)
			{
				dbman()->UnclampCachedFile(x);
			}
			dbman()->Unlock();
		}

		//cancel the download
		SendUpdate(	XM_CMU_DOWNLOAD_CANCEL,
					mDownloads[i].mItem->mMD5,
					mDownloads[i].mWidth,
					mDownloads[i].mHeight,
					mDownloads[i].mIsThumb,
					0);
		mDownloads[i].mSession->Close();
		mDownloads[i].mState = DSS_CLOSING;
		mDownloads[i].mStateCount = 0;
	}
	Unlock();
}

void CXMClientManager::ClearThumbnailDownloads()
{
	//remove all thumbnails from the queue first
	Lock();
	for (DWORD i=(mQueueCount-1);i!=-1;i--)
	{
		if (mQueue[i].mIsThumb)
			CancelQueuedFile(i);
	}
	
	//cancel all current downloads
	for (i=0;i<mDownloadCount;i++)
	{
		if (mDownloads[i].mIsThumb && mDownloads[i].mState!=DSS_OPEN)
			CancelDownloadingFile(i);
	}
	Unlock();
}

void CXMClientManager::RemoveCompletedFile(DWORD i)
{
	//free any memory from this item
	Lock();
	if (mCompleted[i].mBuffer)
		free(mCompleted[i].mBuffer);

	//update the ui
	SendUpdate(	XM_CMU_COMPLETED_REMOVE,
				mCompleted[i].mMD5,
				mCompleted[i].mWidth,
				mCompleted[i].mHeight,
				mCompleted[i].mIsThumb,
				0);

	//unclamp the thumb
	if (!mCompleted[i].mIsThumb)
	{
		dbman()->Lock();
		DWORD x = dbman()->FindCachedFileByParent(mCompleted[i].mMD5);
		if (x != -1)
		{
			dbman()->UnclampCachedFile(x);
		}
		dbman()->Unlock();
	}

	//bump everything down a notch
	memmove(&mCompleted[i], &mCompleted[i+1], (mCompletedCount-(i+1))*sizeof(CompletedFile));
	mCompletedCount--;
	Unlock();
}

//queue access
DWORD CXMClientManager::FindQueuedFile(CXMPipelineUpdateTag* tag) {
	for(DWORD i=0;i<mQueueCount;i++)
		if( (mQueue[i].mItem->mMD5.IsEqual(tag->md5)) &&
			mQueue[i].mIsThumb==tag->thumb)
			return i;
	return -1;
}
DWORD CXMClientManager::GetQueuedFileCount() {	return mQueueCount; }
CXMClientManager::QueuedFile* CXMClientManager::GetQueuedFile(DWORD i) { return &mQueue[i]; }
CXMClientManager::QueuedFile* CXMClientManager::GetQueuedFileBuffer() { return mQueue; }

//download slot access
BYTE CXMClientManager::FindDownloadSlot(CXMPipelineUpdateTag* tag) {
	for(BYTE i=0;i<mDownloadCount;i++)
		if (mDownloads[i].mState != DSS_OPEN &&
			mDownloads[i].mItem)
			if(	(mDownloads[i].mItem->mMD5.IsEqual(tag->md5)) &&
				(mDownloads[i].mIsThumb==tag->thumb))
				return i;
	return -1;
}
BYTE CXMClientManager::GetDownloadSlotCount() { return mDownloadCount; }
CXMClientManager::DownloadSlot* CXMClientManager::GetDownloadSlot(BYTE i) { return &mDownloads[i]; }
CXMClientManager::DownloadSlot* CXMClientManager::GetDownloadSlotBuffer() { return mDownloads; }

//completed files acces
DWORD CXMClientManager::FindCompletedFile(CXMPipelineUpdateTag* tag) {
	for(DWORD i=0;i<mCompletedCount;i++)
		if(	(mCompleted[i].mMD5.IsEqual(tag->md5)) &&
			(mCompleted[i].mWidth==tag->width) &&
			(mCompleted[i].mHeight==tag->height))
			return i;
	return -1;
}
DWORD CXMClientManager::GetCompletedFileCount() { return mCompletedCount; }
CXMClientManager::CompletedFile* CXMClientManager::GetCompletedFile(DWORD i) { return &mCompleted[i]; }
CXMClientManager::CompletedFile* CXMClientManager::GetCompletedFileBuffer() { return mCompleted; }

//upload slot access
BYTE CXMClientManager::FindUploadSlot(CXMPipelineUpdateTag *tag) {
	for(BYTE i=0;i<mUploadSlotCount;i++)
		if (mUploadSlots[i].mId.IsEqual(tag->md5))
			return i;
	return -1;
}
BYTE CXMClientManager::GetUploadSlotCount() { return mUploadSlotCount; }
CXMClientManager::UploadSlot* CXMClientManager::GetUploadSlot(BYTE i) { return &mUploadSlots[i]; }
CXMClientManager::UploadSlot* CXMClientManager::GetUploadSlotBuffer() { return mUploadSlots; }


// --------------------------------------------------------------------- Event Distro

void CXMClientManager::SendUpdate(WPARAM update, CMD5& md5, DWORD width, DWORD height, bool thumb, BYTE flags)
{
	int s = mSubscribers.GetSize() + mSecondary.GetSize();
	CXMPipelineUpdateTag *tag = new CXMPipelineUpdateTag(s);
	tag->md5 = md5;
	tag->width = width;
	tag->height = height;
	tag->thumb = thumb;
	tag->flags = flags;
	SendEvent(XM_CLIENTMSG, update, (LPARAM)tag);
}

CRITICAL_SECTION CXMPipelineUpdateTag::mcsSync;
CXMPipelineUpdateTag::CXMPipelineUpdateTag(BYTE RefCount)
{
	mRefCount = RefCount;
}

CXMPipelineUpdateTag::~CXMPipelineUpdateTag()
{
}

void CXMPipelineUpdateTag::AddRef()
{
	EnterCriticalSection(&mcsSync);
	mRefCount++;
	LeaveCriticalSection(&mcsSync);
}

void CXMPipelineUpdateTag::Release()
{
	EnterCriticalSection(&mcsSync);
	mRefCount--;
	if (mRefCount < 1)
	{
		delete this;
	}
	LeaveCriticalSection(&mcsSync);
}
