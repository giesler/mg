//------------------
// XMMessage.H
//------------------
#pragma once

class CXMMessage;
class CXMMessageField;
class CXMMessageQueue;

#include "xmnet.h"
#include "xmquery.h"

//message types
#define XM_REQUEST				1
#define XM_RESPONSE				2
#define XM_UPDATE				3

//content formats
#define CONTENT_XML				"text/xml"

//helper macros for properties
#define PROP_DEF_INT(_name) \
	int Get##_name(); \
	int Set##_name(int newInt)

#define PROP_DEF_STR(_name) \
	char* Get##_name(bool copy = true); \
	char* Set##_name(char* newString, bool copy = true, bool old = false)

//defines for all the "for" fields possible
#define XMMSG_LOGIN			"login"
#define XMMSG_PING			"ping"
#define XMMSG_MOTD			"motd"
#define XMMSG_LISTING		"listing"
#define XMMSG_INDEX			"index"
#define XMMSG_QUERY			"query"
#define XMMSG_FILE			"file"
#define XMMSG_AU			"au"

class CXMMessageField
{
public:

	//construction
	CXMMessageField();
	CXMMessageField(char* newName, char* newValue, bool copy = true);
	~CXMMessageField();

	//member access
	char* GetName(bool copy = true);
	char* GetValue(bool copy = true);
	void SetValue(char* newValue, bool copy = true);
	void SetValue(DWORD newValue);
	void SetXml(char* newXml, bool copy = true);
	IXMLDOMDocumentFragment* GetDocFragment();
	void Update(char* newName, char* newValue, bool copy = true);
	bool IsXml();

	//assignment
	void operator=(const CXMMessageField& field);
	void operator=(const char* value);

	//cast
	operator char*();

private:
	bool mIsXml;
	char* mName;
	char* mValue;

	void Free();
};

class CXMMessage
{
public:

	//construction
	CXMMessage(CXMSession *newSession);
	~CXMMessage();

	//member access
	PROP_DEF_INT(Sequence);
	PROP_DEF_INT(Reply);
	PROP_DEF_STR(SessionID);
	PROP_DEF_STR(HostIP);
	PROP_DEF_STR(HostSessionID);
	PROP_DEF_INT(Type);
	PROP_DEF_STR(For);
	PROP_DEF_STR(ContentFormat);
	PROP_DEF_STR(SystemID);
	PROP_DEF_STR(HostSystemID);

	//tag
	DWORD tag;

	//field access
	int GetFieldCount();
	CXMMessageField* GetField(int index);
	CXMMessageField* GetField(char* index);
	bool HasField(char *index);
	bool CompareField(char* index, char* value);

	//binary access
	DWORD GetBinarySize();
	DWORD GetBinaryData(void *buf, DWORD bufsize);
	DWORD SetBinaryData(void *buf, DWORD bufsize);
	DWORD GetExpectedBinarySize();
	BYTE* GetBinaryMD5();
	void* GetBinaryDataBuffer();
	bool AttachFile(char *pfilename);
	bool AttachFile(FILE *pfile);

	//initialization, extraction
	char*				ToString();
	IXMLDOMDocument*	ToXml();
	bool				FromString(char* string);
	bool				FromXml(IXMLDOMDocument* xml);

	//misc
	bool Send();
	CXMMessage* CreateReply();

	//return message specific data.. only works
	//once, client must free memory
	CXMQueryResponse* GetQueryResponse();
	CXMMediaListing* GetMediaListing();

private:

	//misc data.. not part of message
	CXMSession* mSession;		//associated session
	bool mLocked;				//true for incoming messages

	//general header info
	int mSequence;				//this message's sequence number
	int mReply;					//sequence this message replies to
	char* mSessionID;			//string guid of our session id
	char* mSystemID;			//system guid of our computer
	struct {
		char* IP;				//dotted ip of host that sent msg
		char* SessionID;		//string guid of host's session
		char* SystemID;			//system guid of host's computer
	} mHost;
	int mType;					//message type (see above for #defs)
	char* mFor;					//service this message is for (request, update)

	//content
	char* mContentFormat;		//mime format.. should be "text/xml"
	CXMMessageField** mContent;	//array of message fields
	int mContentSize;			//allocated size of content buffer
	DWORD mBinarySize;			//size of binary buffer
	void* mBinaryBuf;			//binary buffer pointer
	DWORD mExpectedBinarySize;	//incoming only, size from <binary> tag
	bool AllocBuf(DWORD size);	//realloc the binary buffer, freeing if needed
	void FreeBuf();				//empty binary buf
	bool UpdateMD5();			//recalculate the md5 of our buffer

	//in interal builds, we don't read the md5 from the
	//file, so client manager needs to set it manually
#ifdef _INTERNAL
	public:
#endif
	BYTE mBinaryMD5[16];		//MD5 of buffer
#ifdef _INTERNAL
	private:
#endif

	//specific content
	CXMQueryResponse *mQueryResponse;
	CXMMediaListing *mMediaListing;
};

class CXMMessageQueue
{
public:
	CXMMessageQueue();
	~CXMMessageQueue();

	inline int Size() { return mCount; };
	void Push(CXMMessage* msg);
	CXMMessage* Pop();

	int GetCountIn();
	int GetCountOut();

private:
	CXMMessage** mBuffer;	//actual buffer
	int mStart;				//where the oldest item is (in messages)
	int mBufferSize;		//actual size (in messages) of buffer
	int mCount;				//number of messages in queue
	int mCountIn;			//track all messages into the queue
	int mCountOut;			//track all message out of the queue

	//CRITICAL_SECTION mSync;
	
	bool ExpandBuffer();	//expand buffer, re-pack the queue
	void Lock() {
		//EnterCriticalSection(&mSync);
	};
	void Unlock()
	{
		//LeaveCriticalSection(&mSync);
	};
};