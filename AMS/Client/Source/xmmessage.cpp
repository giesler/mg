//-----------------
// XMMESSAGE.CPP
//-----------------

#include "stdafx.h"
#include "xmclient.h"
#include "xmnet.h"
#include <io.h>

// ---------------------------------------------- UTILITY FUNCTIONS

char* bstr2char(BSTR bstr)
{
	//caller owns return value
	char *ret = (char*)malloc(SysStringLen(bstr)+1);
	char *str = ret;
	BYTE *buf = (BYTE*)bstr;
	do
	{
		//copy character
		*str = *buf;
		
		//skip forward one unicode char
		str += 1;
		buf += 2;
	}
	while (*buf!=0);

	//append null
	*str = '\0';
	return ret;
}

void char2bstr(BSTR *bstr, const char *str)
{
	DWORD dw = strlen(str);
	wchar_t *wcs = (wchar_t*)malloc((dw+1)*2);
	mbstowcs(wcs, str, dw+1);
	*bstr = SysAllocString(wcs);
}

//---------------------------------------------------------------------
//										  XMMessageQueue Implementation

CXMMessageQueue::CXMMessageQueue()
{
	//initialize buffer
	mBufferSize = 4;
	mBuffer = (CXMMessage**)malloc(sizeof(CXMMessage*)*mBufferSize);
	mStart = 0;
	mCount = 0;
	mCountIn = 0;
	mCountOut = 0;

	//initialize critical section
	InitializeCriticalSection(&mSync);
}

CXMMessageQueue::~CXMMessageQueue()
{
	//free buffer
	CXMMessage* msg;
	Lock();
	if (mBuffer) {
		msg = Pop();
		while(msg) {
			delete msg;
			msg = Pop();
		}
		free(mBuffer);
	}
	Unlock();

	//free critical section
	DeleteCriticalSection(&mSync);
}

void CXMMessageQueue::Push(CXMMessage* msg)
{
	Lock();
	
	//do we need to expand?
	if (mCount >= mBufferSize) {
		//try to expand buffer
		if (!ExpandBuffer()) {
			Unlock();
			throw(0);
		}
	}

	//copy pointer to new location
	if ((mStart + mCount)!=0) {
		if (mStart+mCount>=mBufferSize) {
			mBuffer[(mStart + mCount) % mBufferSize] = msg;
		}
		else {
			mBuffer[mStart+mCount] = msg;
		}
	}
	else {
		//would cause divide by zero in %
		mBuffer[0] = msg;
	}
	mCount++;
	mCountIn++;

	Unlock();
}

CXMMessage* CXMMessageQueue::Pop()
{
	Lock();

	//is the queue empty?
	if (mCount<1) {
		Unlock();
		return NULL;
	}

	//the start pointer is the item we want
	CXMMessage* temp;
	temp = mBuffer[mStart];

	//adjust state
	mCount--;
	if (mCount>0) {
		//push the start pointer forward, and allow
		//it to wrap around
		mStart++;
		if (mStart>=mBufferSize) {
			mStart = mStart % mBufferSize;
		}
	}
	else {
		//rewind the start pointer since we
		//are now empty
		mStart = 0;
	}
	mCountOut++;

	
	Unlock();
	return temp;
}

int CXMMessageQueue::GetCountIn()
{
	return mCountIn;
}

int CXMMessageQueue::GetCountOut()
{
	return mCountOut;
}

//WARNING: the queue buffer MUST be completly full
//before calling this function!
bool CXMMessageQueue::ExpandBuffer()
{
	//allocate new buffer
	CXMMessage** buf = (CXMMessage**)malloc((mBufferSize+4)*sizeof(CXMMessage*));
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

//---------------------------------------------------------------------
//										  XMMessageField Implementation


CXMMessageField::CXMMessageField()
{
	mName = NULL;
	mValue = NULL;
	mIsXml = false;
}

CXMMessageField::CXMMessageField(char* newName, char* newValue, bool copy)
{
	mName = NULL;
	mValue = NULL;
	mIsXml = false;
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

void CXMMessageField::SetValue(char* newValue, bool copy)
{
	//copy new value
	if (mValue) free(mValue);
	if (copy)
		mValue = strdup(newValue);
	else
		mValue = newValue;	
	mIsXml = false;
}

void CXMMessageField::SetValue(DWORD newValue)
{
	//convert to string, then store
	char *buf = (char*)malloc(11);
	ltoa(newValue, buf, 10);
	SetValue(buf, false);
}

void CXMMessageField::SetXml(char* newXml, bool copy)
{
	//copy the value, set the xml flag
	SetValue(newXml, copy);
	mIsXml = true;
}

bool CXMMessageField::IsXml()
{
	return mIsXml;
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
//												 XMMessage Construction

CXMMessage::CXMMessage(CXMSession *newSession)
{

	//initialize members
	mSequence = -1;
	mReply = -1;
	mSessionID = strdup("0");
	mHost.IP = NULL;
	mHost.SessionID = NULL;
	mType = -1;
	mFor = NULL;
	mContentFormat = NULL;
	mContent = NULL;
	mContentSize = 0;
	mBinaryBuf = NULL;
	mBinarySize = 0;
	mExpectedBinarySize = 0;

	mSession = newSession;
	mLocked = false;

	mQueryResponse = NULL;
	mMediaListing = NULL;
}

CXMMessage::~CXMMessage()
{

	//free members
	if (mSessionID) free(mSessionID);
	if (mHost.IP) free(mHost.IP);
	if (mHost.SessionID) free(mHost.SessionID);
	if (mFor) free(mFor);
	if (mContentFormat) free(mContentFormat);
	if (mContent) {
		CXMMessageField *f;
		for (int x=0;x<mContentSize;x++) {
			f = mContent[x];
			delete f;
		}	
		free(mContent);
	}
	if (mBinaryBuf) free(mBinaryBuf);
	if (mQueryResponse) mQueryResponse->Release();
	if (mMediaListing) mMediaListing->Release();
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
PROP_IMP_STR(SystemID, mSystemID);
PROP_IMP_STR(HostSystemID, mHost.SystemID);

//---------------------------------------------------------------------
//												 XMMessage Field Access

int CXMMessage::GetFieldCount()
{
	return mContentSize;
}

CXMMessageField* CXMMessage::GetField(int index)
{
	//check index
	if (index >= mContentSize) return NULL;

	//return field
	return mContent[index];
}

CXMMessageField* CXMMessage::GetField(char* index)
{
	//try to find field
	CXMMessageField *f;
	for (int x=0;x<mContentSize;x++) {
		f = mContent[x];
		if (stricmp(f->GetName(false), index)==0) {
			return f;
		}
	}

	//could not find field, allocate memory for it
	CXMMessageField **buf = 
		(CXMMessageField**)realloc(mContent, sizeof(CXMMessageField*)*(mContentSize+1));
	if (!buf) {
		return NULL;
	}
	mContent = buf;

	//setup new field
	f = new CXMMessageField(index, "");
	mContent[mContentSize] = f;
	mContentSize++;
	return f;
}

bool CXMMessage::HasField(char *index)
{
	//look for <index> in the fields collection
	CXMMessageField *f;
	for (int i=0;i<mContentSize;i++)
	{
		f = mContent[i];
		if (stricmp(f->GetName(false), index)==0)
		{
			//found
			return true;
		}
	}

	//not found
	return false;
}

//---------------------------------------------------------------------
//									XMMessage Initialization/Extraction

char* CXMMessage::ToString()
{
	//wrapper around ToXml()
	IXMLDOMDocument* xml = ToXml();
	if (!xml) 
	{
		//failed to convert
		return NULL;
	}

	//output to text
	BSTR tempstr = NULL;
	char *a;
	COM_SINGLECALL(xml->get_xml(&tempstr));
	if (tempstr)
		a = bstr2char(tempstr);
	
	//success
	if (tempstr)
		SysFreeString(tempstr);
	COM_RELEASE(xml);
	return a;

fail:
	COM_RELEASE(xml);
	return NULL;
}

_bstr_t bstrMessage("message");
_bstr_t bstrSequence("sequence");
_bstr_t bstrReply("reply");
_bstr_t bstrFrom("from");
_bstr_t bstrHost("host");
_bstr_t bstrSession("session");
_bstr_t bstrRequest("request");
_bstr_t bstrResponse("response");
_bstr_t bstrFor("for");
_bstr_t bstrUpdate("update");
_bstr_t bstrContent("content");
_bstr_t bstrFormat("format");
_bstr_t bstrName("name");
_bstr_t bstrField("field");
_bstr_t bstrBinary("binary");
_bstr_t bstrMd5("md5");
_bstr_t bstrSize("size");
_bstr_t bstrResults("results");
_bstr_t bstrListing("listing");

IXMLDOMDocument* CXMMessage::ToXml()
{
	//create xml document
	IXMLDOMDocument* xml = CreateXmlDocument();
	IXMLDOMDocument* xml2 = NULL;
	IXMLDOMElement* el = NULL;
	IXMLDOMElement* root = NULL;
	IXMLDOMElement* el2 = NULL;
	IXMLDOMElement* content = NULL;
	IXMLDOMNode* node = NULL;
	IXMLDOMNode* node2 = NULL;
	BSTR bstrTemp = NULL;
	char* temp = NULL;
	int x=0;
	_variant_t var;
	if (!xml) return false;

	//create root
	COM_SINGLECALL(xml->createElement(bstrMessage, &root));
	COM_SINGLECALL(root->setAttribute(bstrSequence, _variant_t((short)mSequence)));
	COM_SINGLECALL(root->setAttribute(bstrReply, _variant_t((short)mReply)));
	COM_SINGLECALL(xml->putref_documentElement(root));

	//read our local ip
	temp = mSession->LocalIP();
	if (!temp) {
		temp = strdup("0.0.0.0");
	}

	//create "from" tag
	COM_SINGLECALL(xml->createElement(bstrFrom, &el));
	COM_SINGLECALL(el->setAttribute(bstrHost, _variant_t(temp)));	
	COM_SINGLECALL(el->setAttribute(bstrSession, _variant_t(mSessionID)));
	free(temp);
	temp = NULL;

	//append to root
	COM_SINGLECALL(el->QueryInterface(IID_IXMLDOMNode, (void**)&node));
	COM_SINGLECALL(root->appendChild(node, &node2));
	if (!node2) goto fail;
	COM_RELEASE(node2);
	COM_RELEASE(node);
	COM_RELEASE(el);

	//create request tag
	if (mType==XM_REQUEST || mType==XM_RESPONSE) 
	{
		//make sure it says "request" or "response"
		COM_SINGLECALL(xml->createElement(
			(mType==XM_REQUEST)?bstrRequest:bstrResponse,
			&el));
		COM_SINGLECALL(el->setAttribute(bstrFor, _variant_t(mFor)));
        COM_SINGLECALL(el->QueryInterface(IID_IXMLDOMNode, (void**)&node));
		COM_SINGLECALL(root->appendChild(node, &node2));
		if (!node2) goto fail;
		COM_RELEASE(node2);
		COM_RELEASE(node);
		COM_RELEASE(el);
	}

	//create update tag
	if (mType==XM_UPDATE) 
	{
		COM_SINGLECALL(xml->createElement(bstrUpdate, &el));
		COM_SINGLECALL(el->setAttribute(bstrFor, _variant_t(mFor)));
        COM_SINGLECALL(el->QueryInterface(IID_IXMLDOMNode, (void**)&node));
		COM_SINGLECALL(root->appendChild(node, &node2));
		if (!node2) goto fail;
		COM_RELEASE(node2);
		COM_RELEASE(node);
		COM_RELEASE(el);
	}

	//create content tag
	COM_SINGLECALL(xml->createElement(bstrContent, &content));
	COM_SINGLECALL(content->setAttribute(bstrFormat, _variant_t(mContentFormat)));
    COM_SINGLECALL(content->QueryInterface(IID_IXMLDOMNode, (void**)&node));
	COM_SINGLECALL(root->appendChild(node, &node2));
	if (!node2) goto fail;
	COM_RELEASE(node2);
	COM_RELEASE(node);

	//create field tags
	CXMMessageField *f;
	VARIANT_BOOL b;
	for (x=0;x<mContentSize;x++) 
	{
		f = mContent[x];
		COM_SINGLECALL(xml->createElement(bstrField, &el));
		COM_SINGLECALL(el->setAttribute(bstrName, _variant_t(f->GetName(false))));
		if (f->IsXml())
		{
			//convert back into structured xml
			xml2 = CreateXmlDocument();
			char2bstr(&bstrTemp, f->GetValue(false));
			COM_SINGLECALL(xml2->put_validateOnParse(-1));
			COM_SINGLECALL(xml2->loadXML(bstrTemp, &b));
			//COM_SINGLECALL(xml2->loadXML(_bstr_t(f->GetValue(false)), &b));
			if (b)
			{
				COM_SINGLECALL(xml2->get_documentElement(&el2));
				COM_SINGLECALL(el2->QueryInterface(IID_IXMLDOMNode, (void**)&node));
				COM_SINGLECALL(el->appendChild(node, &node2));
			}
			else
			{
				IErrorInfo *ei = NULL;
				BSTR bstrErr = NULL;
				GetErrorInfo(NULL, &ei);
				ei->GetDescription(&bstrErr);
				ASSERT(FALSE);
				COM_RELEASE(ei);
				if (bstrErr)
					SysFreeString(bstrErr);

			}
			COM_RELEASE(xml2);
			COM_RELEASE(el2);
			COM_RELEASE(node);
			COM_RELEASE(node2);
			if (bstrTemp)
			{
				SysFreeString(bstrTemp);
				bstrTemp = NULL;
			}
		}
		else
		{
			//simple text field
			COM_SINGLECALL(el->put_text(_bstr_t(f->GetValue(false))));
		}
		COM_SINGLECALL(el->QueryInterface(IID_IXMLDOMNode, (void**)&node));
		COM_SINGLECALL(content->appendChild(node, &node2));
		if (!node2) goto fail;
		COM_RELEASE(node2);
		COM_RELEASE(node);
		COM_RELEASE(el);
	}

	//create binary tag
	if (mBinarySize>0) {

		//get our md5 into string
		char md5[33];	//16 bytes * 2 chars per byte + null char
		md5tohex(md5, mBinaryMD5);

		var.Clear();
		V_VT(&var) = VT_UI4;
		V_UI4(&var) = mBinarySize;
		COM_SINGLECALL(xml->createElement(bstrBinary, &el));
		COM_SINGLECALL(el->setAttribute(bstrSize, var));
		V_VT(&var) = VT_BSTR;
		V_BSTR(&var) = _bstr_t(md5).copy();
		COM_SINGLECALL(el->setAttribute(bstrMd5, var));
		COM_SINGLECALL(el->QueryInterface(IID_IXMLDOMNode, (void**)&node));
		COM_SINGLECALL(root->appendChild(node, &node2));
		if (!node2) goto fail;
		COM_RELEASE(node2);
		COM_RELEASE(node);
		COM_RELEASE(el);
	}

	//success
	COM_RELEASE(xml2);
	COM_RELEASE(el2);
	COM_RELEASE(node2);
	COM_RELEASE(node);
	COM_RELEASE(el);
	COM_RELEASE(content);
	COM_RELEASE(root);
	return xml;

fail:
	COM_RELEASE(xml2);
	COM_RELEASE(el2);
	COM_RELEASE(node2);
	COM_RELEASE(node);
	COM_RELEASE(el);
	COM_RELEASE(content);
	COM_RELEASE(root);
	COM_RELEASE(xml);
	if (temp) free(temp);
	return NULL;
}

bool CXMMessage::FromString(char* string)
{
	//convert to xml
	IXMLDOMDocument* xml = CreateXmlDocument();
	if (!xml) return false;

	//push in text
	size_t l = strlen(string)+1;	//+1 = null char
	OLECHAR *ole = (OLECHAR*)malloc(l*2);
	MultiByteToWideChar(CP_ACP, 0, string, -1, ole, l);
	BSTR bstr = SysAllocString(ole);
	/*
	TRACE1("char*: %d\n", l);
	TRACE1("OLECHAR*: %d\n", wcslen(ole));
	TRACE1("BSTR: %d\n", SysStringLen(bstr));
	*/
	VARIANT_BOOL retval;
	COM_SINGLECALL(xml->loadXML(bstr, &retval));
	SysFreeString(bstr);
	free(ole);
	if (retval==VARIANT_FALSE) goto fail;
	
	//wrap around FromXml()
	if (!FromXml(xml)) goto fail;

	//success
	COM_RELEASE(xml);
	return true;

fail:
	COM_RELEASE(xml);
	return false;
}

bool CXMMessage::FromXml(IXMLDOMDocument* xml)
{
	IXMLDOMElement* root = NULL;
	IXMLDOMElement* el = NULL;
	IXMLDOMElement* el2 = NULL;
	IXMLDOMElement* content = NULL;
	IXMLDOMNodeList* list = NULL;
	IXMLDOMNodeList* list2 = NULL;
	IXMLDOMNode* node = NULL;
	HRESULT retval;
	_variant_t var;
	_bstr_t str;
	BSTR tempstr;
	char* a;
	CXMMessageField *field;
	DOMNodeType nodeType;

	bool hasFrom = false;
	bool hasRequest = false;
	bool hasResponse = false;
	bool hasUpdate = false;
	bool hasContent = false;
	bool hasBinary = false;

	//get root
	xml->AddRef();
	COM_SINGLECALL(xml->get_documentElement(&root));

	//get sequence number
	COM_SINGLECALL(root->getAttribute(bstrSequence, &var));
	if (V_VT(&var)==VT_EMPTY ||
		V_VT(&var)==VT_NULL) {
		goto fail;
	}
	mSequence = V_I2(&var);

	//get reply sequence number (optioanl, can't use macro)
	retval = root->getAttribute(bstrReply, &var);
	if (FAILED(retval)) {
		if (retval!=S_FALSE)
			goto fail;
		else
			mReply = -1;
	}
	else {
		if (V_VT(&var)!=VT_EMPTY &&
			V_VT(&var)!=VT_NULL) {
			mReply = V_I2(&var);
		}
	}

	//enumerate children of root node

	COM_SINGLECALL(root->get_childNodes(&list));
	COM_SINGLECALL(list->nextNode(&node));
	while(node)
	{
		//only proceed if node is element
		COM_SINGLECALL(node->get_nodeType(&nodeType));
		if (nodeType==NODE_ELEMENT)
		{
			//convert node->element
			COM_SINGLECALL(node->QueryInterface(IID_IXMLDOMElement, (void**)&el));

			//get tag name
			COM_SINGLECALL(el->get_tagName(&tempstr));
			a = _com_util::ConvertBSTRToString(tempstr);

			//from field
			if (stricmp(a, "from")==0) {
				
				//test for inclusion.. only 1 "from" allowable
				if (hasFrom) goto fail;
				hasFrom = true;

				//get host (ip) field
				COM_SINGLECALL(el->getAttribute(bstrHost, &var));
				if (V_VT(&var)!=VT_EMPTY &&	V_VT(&var)!=VT_NULL)
					mHost.IP = strdup(_bstr_t(var));
				
				//get host session id
				COM_SINGLECALL(el->getAttribute(bstrSession, &var));
				if (V_VT(&var)!=VT_EMPTY && V_VT(&var)!=VT_NULL)
					mHost.SessionID = strdup(_bstr_t(var));
			}

			//request field
			if (stricmp(a, "request")==0) {

				//test for inclusion
				if (hasRequest || hasResponse || hasUpdate) goto fail;
				hasRequest = true;

				//get for field
				COM_SINGLECALL(el->getAttribute(bstrFor, &var));
				if (V_VT(&var)==VT_EMPTY || V_VT(&var)==VT_NULL) goto fail;
				mFor = strdup(_bstr_t(var));

				//set type
				mType = XM_REQUEST;
			}

			//response field
			if (stricmp(a, "response")==0) {

				//test for inclusion
				if (hasRequest || hasResponse || hasUpdate) goto fail;
				hasResponse = true;

				//get for field
				COM_SINGLECALL(el->getAttribute(bstrFor, &var));
				if (V_VT(&var)==VT_EMPTY || V_VT(&var)==VT_NULL) goto fail;
				mFor = strdup(_bstr_t(var));

				//set type
				mType = XM_RESPONSE;
			}

			//update field
			if (stricmp(a, "update")==0) {

				//test for inclusion
				if (hasRequest || hasResponse || hasUpdate) goto fail;
				hasUpdate = true;

				//get for field
				COM_SINGLECALL(el->getAttribute(bstrFor, &var));
				if (V_VT(&var)==VT_EMPTY || V_VT(&var)==VT_NULL) goto fail;
				mFor = strdup(_bstr_t(var));

				//set type
				mType = XM_UPDATE;
			}

			//content field
			if (stricmp(a, "content")==0) {

				//test for inclusion
				if (hasContent) goto fail;
				hasContent = true;

				//store the content element
				content = el;
				content->AddRef();

			}

			//binary field
			if (stricmp(a, "binary")==0) {

				//test for inclusion
				if (hasBinary) goto fail;
				hasBinary = true;

				//get size
				COM_SINGLECALL(el->getAttribute(bstrSize, &var));
				if (V_VT(&var)==VT_EMPTY || V_VT(&var)==VT_NULL) goto fail;
				var.ChangeType(VT_UI4);
				mExpectedBinarySize = V_UI4(&var);

				//get md5
				char* md5;
				COM_SINGLECALL(el->getAttribute(bstrMd5, &var));
				if (V_VT(&var)==VT_EMPTY || V_VT(&var)==VT_NULL) goto fail;
				md5 = strdup(_bstr_t(var));
				if (strlen(md5)<16) goto fail;
				hextomd5(mBinaryMD5, md5);
				free(md5);
			}

			free(a);
			a = NULL;
			SysFreeString(tempstr);
		}
		COM_RELEASE(el);
		COM_RELEASE(node);
		COM_SINGLECALL(list->nextNode(&node));
	}
	COM_RELEASE(list);

	//were the appropriate fields picked up?
	if (!hasFrom ||
		!(hasRequest || hasResponse || hasUpdate))
		goto fail;

	//was there any contents?
	if (hasContent) {

		//get format field
		COM_SINGLECALL(content->getAttribute(bstrFormat, &var));
		if (V_VT(&var)==VT_EMPTY || V_VT(&var)==VT_NULL) goto fail;
		mContentFormat = strdup(_bstr_t(var));

		//enumerate child nodes of content
		COM_SINGLECALL(content->get_childNodes(&list));
		COM_SINGLECALL(list->nextNode(&node));
		while(node)
		{
			//test for element
			COM_SINGLECALL(node->get_nodeType(&nodeType));
			if (nodeType==NODE_ELEMENT) {

				//convert to element
				COM_SINGLECALL(node->QueryInterface(IID_IXMLDOMElement, (void**)&el));
				COM_RELEASE(node);

				//anything but field is invalid
				COM_SINGLECALL(el->get_nodeName(&tempstr));
				if (_bstr_t(tempstr, false)!=bstrField) goto fail;
				
				//get name
				COM_SINGLECALL(el->getAttribute(bstrName, &var));
				if (V_VT(&var)==VT_EMPTY || V_VT(&var)==VT_NULL) goto fail;
				str = var;

				//insert new fields
				field = GetField(str);
				if (!field) goto fail;

				//set the field's data
				if (strcmp(field->GetName(false), "results")==0)
				{
					//get result element
					COM_SINGLECALL(el->getElementsByTagName(bstrResults, &list2));
					COM_SINGLECALL(list2->nextNode(&node));
					while (node) {
						
						//convert to element
						COM_SINGLECALL(node->QueryInterface(IID_IXMLDOMElement, (void**)&el2));
						
						//decode
						if (mQueryResponse) mQueryResponse->Release();
						mQueryResponse = new CXMQueryResponse();
						if (FAILED(mQueryResponse->FromXml(el2))) {
							mQueryResponse->Release();
							mQueryResponse = NULL;
						}

						//next node (should not happen)
						COM_RELEASE(el2);
						COM_RELEASE(node);
						COM_SINGLECALL(list2->nextNode(&node));
					}
					COM_RELEASE(list2);
					COM_RELEASE(node);
				}
				else if (strcmp(field->GetName(false), "listing")==0)
				{
					//get the listing field
					COM_SINGLECALL(el->getElementsByTagName(bstrListing, &list2));
					COM_SINGLECALL(list2->nextNode(&node));
					if (node)
					{
						//convert to element
						COM_SINGLECALL(node->QueryInterface(IID_IXMLDOMElement, (void**)&el2));
						
						//convert to listing
						mMediaListing = new CXMMediaListing();
						COM_SINGLECALL(mMediaListing->FromXml(el2));
						COM_RELEASE(el2);
					}
					COM_RELEASE(list2);
					COM_RELEASE(node);
				}
				else
				{
					//not a special field.. simply set value
					//to the inner text of the element
					COM_SINGLECALL(el->get_text(&tempstr));
					a = _com_util::ConvertBSTRToString(tempstr);
					field->SetValue(a);
					free(a);
					a = NULL;
					SysFreeString(tempstr);
				}
			}

			COM_RELEASE(el);
			COM_RELEASE(node);
			COM_SINGLECALL(list->nextNode(&node));
		}
		COM_RELEASE(list);
	}

	//success
	COM_RELEASE(node);
	COM_RELEASE(el);
	COM_RELEASE(list);
	COM_RELEASE(content);
	COM_RELEASE(root);
	COM_RELEASE(xml);
	return true;

fail:
	COM_RELEASE(node);
	COM_RELEASE(el);
	COM_RELEASE(el2);
	COM_RELEASE(list);
	COM_RELEASE(list2);
	COM_RELEASE(content);
	COM_RELEASE(root);
	COM_RELEASE(xml);
	return false;
}

//---------------------------------------------------------------------
//											    XMMessage Binary Access

DWORD CXMMessage::GetBinarySize()
{
	return mBinarySize;
}

DWORD CXMMessage::GetExpectedBinarySize()
{
	return mExpectedBinarySize;
}

bool CXMMessage::AllocBuf(DWORD size)
{
	//is our buffer already allocated?
	void* temp;
	if (mBinaryBuf) {

		//attempt realloc
		temp = realloc(mBinaryBuf, size);
		if (!temp) {
			FreeBuf();
			return false;
		}
		mBinaryBuf = temp;
	}
	else {

		//allocate new buffer
		mBinaryBuf = malloc(size);
		if (!mBinaryBuf) {
			FreeBuf();
			return false;
		}
	}

	//success
	mBinarySize = size;
	return true;
}

void CXMMessage::FreeBuf()
{
	//free buffer, reset size
	if (mBinaryBuf) {
		free(mBinaryBuf);
		mBinaryBuf = NULL;
	}
	mBinarySize = 0;
}

DWORD CXMMessage::GetBinaryData(void *buf, DWORD bufsize)
{
	//is our buffer valid?
	if (!mBinaryBuf) return 0;

	//copy data into given buffer
	DWORD copysize = (bufsize>mBinarySize) ? mBinarySize : bufsize;
	memcpy(buf, mBinaryBuf, copysize);
	return copysize;
}

void* CXMMessage::GetBinaryDataBuffer()
{
	//return pointer to buffer
	return mBinaryBuf;
}

DWORD CXMMessage::SetBinaryData(void *buf, DWORD bufsize)
{
	//reallocate buffer, copy data, recalc checksum
	if (!AllocBuf(bufsize)) return -1;
	memcpy(mBinaryBuf, buf, bufsize);
	if (!UpdateMD5()) return -1;
	return bufsize;
}

bool CXMMessage::AttachFile(char *pfilename)
{
	//open the file and pass control the other overload
	FILE* pf = fopen(pfilename, "rb");

	bool retval;
	if (pf) {
			retval = AttachFile(pf);
			fclose(pf);
	}
	else {
		retval = false;
	}
	return retval;
}

bool CXMMessage::AttachFile(FILE *pfile)
{
	//reallocate our buffer
	int hfile = _fileno(pfile);
	if (!AllocBuf(_filelength(hfile))) {
		return false;
	}

	//read file
	BYTE *buf = (BYTE*)mBinaryBuf;
	DWORD pos = 0, remaining = mBinarySize;
	size_t retval;
	rewind(pfile);
	while(!feof(pfile) && pos<mBinarySize) {

		//read another block from the file
		remaining = mBinarySize - pos;
		if (remaining>4096) remaining = 4096;
		retval = fread(buf, sizeof(BYTE), remaining, pfile);
		if (retval!=(remaining*sizeof(BYTE))) {
			if (ferror(pfile)!=0) {
				
				//read error
				FreeBuf();
				return false;
			}
		}

		//reposition buffer pointer for next read
		buf += retval;
		pos += retval;
	}

	//Calculate MD5 -- only in distribution builds!
	#ifdef _DISTRO
	if (!UpdateMD5()) {
		FreeBuf();
		return false;
	}
	#endif

	//success
	return true;
}

bool CXMMessage::UpdateMD5()
{
	//calculate md5
	CMD5Engine md5;
	md5.update((BYTE*)mBinaryBuf, mBinarySize);
	md5.finalize();

	//copy value
	memcpy(mBinaryMD5, md5.raw_digest(), 16);

	return true;
}

BYTE* CXMMessage::GetBinaryMD5()
{
	return mBinaryMD5;
}

//---------------------------------------------------------------------
//											   XMMessage Misc Functions

bool CXMMessage::Send()
{
	//pass to session
	return mSession->Send(this);
}

CXMMessage* CXMMessage::CreateReply()
{
	//create a new message that will
	//reply to this one
	CXMMessage *msg = new CXMMessage(mSession);
	msg->SetFor(mFor);
	msg->SetReply(mSequence);
	msg->SetType(XM_RESPONSE);
	msg->SetContentFormat("text/xml");

	return msg;
}

CXMQueryResponse* CXMMessage::GetQueryResponse()
{
	if (mQueryResponse)
		mQueryResponse->AddRef();
	return mQueryResponse;
}

CXMMediaListing* CXMMessage::GetMediaListing()
{
	if (mMediaListing)
		mMediaListing->AddRef();
	return mMediaListing;
}