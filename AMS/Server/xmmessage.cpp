//-----------------
// XMMESSAGE.CPP
//-----------------

#include "stdafx.h"
#include "xmclient.h"
#include "xmsession.h"

CXMMessage::CXMMessage()
{
}

CXMMessage::~CXMMessage()
{
}

CXMMessageQueue::CXMMessageQueue()
{
	//initialize buffer
	mBufferSize = 4;
	mBuffer = (CXMMessage*)malloc(sizeof(CXMMessage*)*mBufferSize);
	mStart = 0;
	mCount = 0;
}

CXMMessageQueue::~CXMMessageQueue()
{
	//free buffer
	CXMMessage* msg;
	if (mBuffer) {
		msg = Pop();
		while(msg) {
			delete msg;
			msg = Pop();
		}
		free(mBuffer);
	}
}

void CXMMessageQueue::Push(CXMMessage* msg)
{
	//do we need to expand?
	if (mCount >= mBufferSize) {
		//try to expand buffer
		if (!ExpandBuffer()) {
			throw(0);
		}
	}

	//copy pointer to new location
	if ((mStart + mCount)!=0) {
		memcpy(mBuffer + ((mBufferSize % (mStart + mCount)) * sizeof(CXMMessage*)), &msg, sizeof(CXMMessage*));
	}
	else {
		//would cause divide by zero in %
		memcpy(mBuffer, &msg, sizeof(msg));
	}
	mCount++;
}

CXMMessage* CXMMessageQueue::Pop()
{
	//is the queue empty?
	if (mCount<1) return NULL;

	//the start pointer is the item we want
	CXMMessage* temp;
	memcpy(&temp, mBuffer + (mStart*sizeof(CXMMessage*)), sizeof(CXMMessage*));

	//adjust state
	mCount--;
	if (mCount>0) {
		//push the start pointer forward, and allow
		//it to wrap around
		mStart = mBufferSize % (++mStart);
	}
	else {
		//rewind the start pointer since we
		//are now empty
		mStart = 0;
	}

	return temp;
}

//WARNING: the queue buffer MUST be completly full
//before calling this function!
bool CXMMessageQueue::ExpandBuffer()
{
	//allocate new buffer
	CXMMessage* buf = (CXMMessage*)malloc((mBufferSize+4)*sizeof(CXMMessage*));
	if (!buf) return false;

	//copy upper block, then lower block, this will place the
	//new buffer in a start=0 state
	memcpy(buf, mBuffer + (mStart*sizeof(CXMMessage*)), (mBufferSize - mStart)*sizeof(CXMMessage*));
	memcpy(buf + (mStart*sizeof(CXMMessage*)), mBuffer, (mBufferSize % (mStart+mCount))*sizeof(CXMMessage*));

	//swap pointers
	free(mBuffer);
	mBuffer = buf;

	//adjust state
	mBufferSize += 4;
	mStart = 0;

	return true;
}


CXMMessageField::CXMMessageField()
{
	mName = NULL;
	mValue = NULL;
}

CXMMessageField::CXMMessageField(char* newName, char* newValue, bool copy)
{
	mName = NULL;
	mValue = NULL;
	Update(newName, newValue, copy);
}

CXMMessageField::~CXMMessageField()
{
	//free string memory
	Free();
}

char* CXMMessageField::GetName(bool copy)
{
	//copy the string by default
	if(copy) {
		return strdup(mName);
	}
	else {
		return mName;
	}
}

char* CXMMessageField::GetValue(bool copy)
{
	//copy the string by default
	if(copy) {
		return strdup(mValue);
	}
	else {
		return mValue;
	}
}

IXMLDOMDocumentFragment* CXMMessageField::GetDocFragment()
{
	//TODO: convert xmmessagefield's contents into xml fragment
	return NULL;
}

void CXMMessageField::Update(char* newName, char* newValue, bool copy)
{
	//free underlying memory if needed
	Free();

	//copy string (or just pointers)
	if (copy) {
		mName = strdup(newName);
		mValue = strdup(newValue);
	}
	else {
		mName = newName;
		mValue = newValue;
	}
}

void CXMMessageField::operator=(const CXMMessageField& field)
{
	//copy from this message into us
	Free();
	mName = strdup(field.mName);
	mValue = strdup(field.mValue);
}

void CXMMessageField::operator =(const char* value)
{
	//assign new value
	if (mValue) {
		free(mValue);
	}
	mValue = strdup(value);
}

CXMMessageField::operator char*()
{
	return strdup(mValue);
}

void CXMMessageField::Free()
{
	//free any memory
	if (mName) {
		free(mName);
	}
	if (mValue) {
		free(mValue);
	}
}

//---------------------------------------------------------------------
//												XMMessage Member Access

#define GETINT(_old) \
	return _old;

#define SETINT(_old, _new) \
	int temp = _old; \
	if (!mLocked) \
		_old = _new; \
	return temp; 

#define GETCHAR(_old) \
	char* temp; \
	if (copy) \
		temp = strdup(_old); \
	else \
		temp = _old; \
	return temp; 

#define SETCHAR(_old, _new) \
	char* temp; \
	if (mLocked) { \
		if (old) \
			temp = strdup(_old); \
		else \
			temp = NULL; \
		return temp; \
	} \
	if (old) \
		temp = _old; \
	else { \
		temp = NULL; \
		if (_old) \
			free(_old); \
	} \
	if (copy) \
		_old = strdup(_new); \
	else \
		_old = _new; \
	return temp; 

#define PROP_IMP_INT(__name, __old) \
	int CXMMessage::Get##__name() { GETINT(__old) } \
	int CXMMessage::Set##__name(int newInt) { SETINT(__old, newInt) }

#define PROP_IMP_STR(__name, __old) \
	char* CXMMessage::Get##__name(bool copy) { GETCHAR(__old) } \
	char* CXMMessage::Set##__name(char* newString, bool copy, bool old) \
	{ SETCHAR(__old, newString) }

PROP_IMP_INT(Sequence, mSequence)
PROP_IMP_INT(Reply, mReply)
PROP_IMP_STR(SessionID, mSessionID)
PROP_IMP_STR(HostIP, mHost.IP)
PROP_IMP_STR(HostSessionID, mHost.SessionID)
PROP_IMP_INT(Type, mType)
PROP_IMP_STR(For, mFor)
PROP_IMP_STR(ContentFormat, mContentFormat)