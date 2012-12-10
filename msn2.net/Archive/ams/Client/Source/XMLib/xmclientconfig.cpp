
// CXMClientConfig Implementation
// (c)2000, 2001 XMedia Inc.

#include "stdafx.h"
#include <shlobj.h>
#include "xmlib.h"

char* CXMClientConfig::GetSharedFilesLocation(bool copy)
{
	//use the saved path?
	if (GetFieldBool(FIELD_DB_SHARE_USESAVED))
	{
		return GetField(FIELD_DB_SAVE_PATH, copy);
	}
	else
	{
		return GetField(FIELD_DB_SHARE_PATH, copy);
	}
}


void CXMClientConfig::Copy(CXMClientConfig *pfrom, CXMClientConfig *pto)
{
	//copy the buffer from one to another
	EnterCriticalSection(&pfrom->mSync);
	EnterCriticalSection(&pto->mSync);

	//free the target's buffer
	if (pto->mFields)
	{
		for(int i=0;i<pto->mFieldCount;i++)
		{
			if (pto->mFields[i].value)
			{
				free(pto->mFields[i].value);
			}
		}
		free(pto->mFields);
		pto->mFields = 0;
		pto->mFieldCount = 0;
		pto->mFieldBufferCount = 0;
	}

	//copy over the buffer
	pto->mFields = (Field*)malloc(sizeof(Field)*pfrom->mFieldCount);
	pto->mFieldCount = pfrom->mFieldCount;
	pto->mFieldBufferCount = pfrom->mFieldCount;
	memcpy(pto->mFields, pfrom->mFields, sizeof(Field)*pto->mFieldCount);

	//copy misc settings
	pto->mbHasCheckedSmoothSplitting = pfrom->mbHasCheckedSmoothSplitting;
	pto->mbSmoothSplitting = pfrom->mbSmoothSplitting;
	pto->mSysID = pfrom->mSysID;

	//copy registry stuff
	if (pto->mRegConfigPath)
	{
		free(pto->mRegConfigPath);
		pto->mRegConfigPath = NULL;
	}
	pto->mRegConfigPath = strdup(pfrom->mRegConfigPath);

	//we need to duplicate all the values
	for (int i=0;i<pto->mFieldCount;i++)
	{
		if (pto->mFields[i].value)
		{
			pto->mFields[i].value = strdup(pto->mFields[i].value);
		}
	}

	//finished
	LeaveCriticalSection(&pfrom->mSync);
	LeaveCriticalSection(&pto->mSync);
}	

// ----------------------------------------------------------------------------------- CLIENT CONFIG

//sync helpers
#define ENTER() EnterCriticalSection(&mSync)
#define EXIT() LeaveCriticalSection(&mSync)

CXMClientConfig::CXMClientConfig()
{
	//reg state
	mRegConfigPath = NULL;
	mRegIsDirty = false;
	mRegKey = NULL;

	//setup list
	mFields = NULL;
	mFieldCount = 0;
	mFieldBufferCount = 0;

	//set persisted queries to clean state
	mMru = NULL;
	mMruCount = 0;
	mMruSize = 0;
	mSaved = NULL;
	mSavedCount = 0;
	mSavedSize = 0;

	//create sync object
	InitializeCriticalSection(&mSync);
}

CXMClientConfig::~CXMClientConfig()
{
	//free fields
	Field *f;
	ENTER();
	if (mFields) {
		for (int x=0;x<mFieldCount;x++)
		{
			f = &mFields[x];
			if (f->value) {
				free(f->value);
			}
		}
		free(mFields);
	}

	//free registry handle
	if (mRegKey) {
		::RegCloseKey(mRegKey);
		mRegKey = NULL;
	}

	//free registry vars
	if (mRegConfigPath) {
		free(mRegConfigPath);
		mRegConfigPath = NULL;
	}

	//release persisted query mem
	QueryFree();

	//release critical section
	EXIT();
	DeleteCriticalSection(&mSync);
}

/*
bool CXMClientConfig::DoPrefs(bool wizard)
{
	CPreferences prefs("AMS Options", this);
	bool retval = prefs.DoPreferences(wizard);
	if (retval)
	{
		//any changes to mru?
		//TODO: reset the mru stuff
	}
	return retval;
}
*/

// --------------------------------------------------------------------------------- Persist Queries

void CXMClientConfig::QueryFree()
{
	//free any memory allocated for persisted queries
	if (mMru)
	{
		//release allocated queries
		for(int i=0;i<mMruCount;i++)
		{
			mMru[i]->Release();
		}
		
		//free buffer
		free(mMru);
		mMru = NULL;
		mMruCount = 0;
		mMruSize = 0;
	}

	//saved queries
	if (mSaved)
	{
		//relase allocated queries
		for(int i=0;i<mSavedCount;i++)
		{
			mSaved[i]->Release();
		}

		//free buffer
		free(mSaved);
		mSaved = NULL;
		mSavedCount = 0;
		mSavedSize = 0;
	}
}

void CXMClientConfig::QueryAlloc()
{
	//allocate buffers and set initial state for all
	//persisted queries based on config settings
	QueryFree();

	//allocate mrus
	if (GetFieldBool(FIELD_SEARCH_AUTOSAVE_ENABLE))
	{
		mMruSize = GetFieldLong(FIELD_SEARCH_AUTOSAVE_COUNT);
		mMru = (CXMQuery**)malloc(mMruSize*sizeof(CXMQuery*));
	}
}

void CXMClientConfig::FilterSet(CXMIndex *filter)
{
	//set our internal filter
	filter->CopyTo(&mFilter);
}

void CXMClientConfig::FilterGet(CXMIndex *filter)
{
	//copy our internal filter
	if (filter)
		mFilter.CopyTo(filter);
}

void CXMClientConfig::FilterPersist(IXMLDOMElement *root)
{
	IXMLDOMDocument *xml = NULL;
	IXMLDOMElement *e = NULL;
	IXMLDOMNode *n = NULL;

	COM_SINGLECALL(root->get_ownerDocument(&xml));
	COM_SINGLECALL(mFilter.ToXml(xml, "filter", &e));
	COM_SINGLECALL(root->appendChild(e, &n));

	COM_RELEASE(xml);
	COM_RELEASE(e);
	COM_RELEASE(n);
	return;

fail:
	COM_RELEASE(xml);
	COM_RELEASE(e);
	COM_RELEASE(n);
}

void CXMClientConfig::FilterLoad(IXMLDOMElement *root)
{
	IXMLDOMNodeList *list = NULL;
	IXMLDOMElement *e = NULL;
	IXMLDOMNode *n = NULL;
	DOMNodeType t;

	//find children named "filter"
	COM_SINGLECALL(root->getElementsByTagName(_bstr_t("filter"), &list));
	COM_SINGLECALL(list->nextNode(&n));
	while (n)
	{
		//element?
		COM_SINGLECALL(n->get_nodeType(&t));
		if (t==NODE_ELEMENT)
		{
			//convert to index
			COM_SINGLECALL(n->QueryInterface(IID_IXMLDOMElement, (void**)&e));
			COM_SINGLECALL(mFilter.FromXml(e));
			COM_RELEASE(e);
		}

		//next node
		COM_RELEASE(n);
		COM_SINGLECALL(list->nextNode(&n));
	}
	COM_RELEASE(list);	

	//success
	return;

fail:
	COM_RELEASE(list);
	COM_RELEASE(e);
	COM_RELEASE(n);
}

int CXMClientConfig::QueryGetCount()
{
	//count of all persisted queries
	return mSavedCount + mMruCount;
}

void CXMClientConfig::QueryGetItem(int i, CXMQuery** query)
{
	//mru?
	if (i<mMruCount)
	{
		//mru
		*query = mMru[i];
	}
	else
	{
		//saved query
		*query = mSaved[i-mMruCount];
	}
}

void CXMClientConfig::QueryDeleteItem(int i)
{
	//mru?
	if (i<mMruCount)
	{
		//mru
		return;
	}
	else
	{
		//release the item
		mSaved[i-mMruCount]->Release();

		//saved query
		memmove(
			&(mSaved[i-mMruCount]),
			&(mSaved[i-mMruCount+1]), 
			sizeof(CXMQuery*)*(mSavedCount-(i-mMruCount+1)));
		mSavedCount--;
	}
}

void CXMClientConfig::QueryMru(CXMQuery* query)
{
	//do we save mru's?
	if (!GetFieldBool(FIELD_SEARCH_AUTOSAVE_ENABLE) ||
		mMruSize==0)
		return;

	//new item? or overwrite?
	if (mMruCount < mMruSize)
	{
		//open up the first slot
		memmove(
			&(mMru[1]),
			&(mMru[0]),
			sizeof(CXMQuery*)*mMruCount);
		mMruCount++;

		//copy query open slot
		mMru[0] = new CXMQuery();
		QueryCopy(query, mMru[0]);
	}
	else
	{
		//shift everything down, move the last first
		CXMQuery *temp = mMru[mMruCount-1];
		memmove(
			&(mMru[1]),
			&(mMru[0]),
			sizeof(CXMQuery*)*(mMruCount-1));
		mMru[0] = temp;

		//copy query into first slot
		QueryCopy(query, temp);
	}

	//set the names
	char name[MAX_PATH];
	for(int i=0;i<mMruCount;i++)
	{
		//release current name
		if (mMru[i]->mName)
			free(mMru[i]->mName);
		
		//new name
		sprintf(name, "Recent Search (%d)", i+1);
		mMru[i]->mName = strdup(name);
	}
}

void CXMClientConfig::QuerySave(CXMQuery* query)
{
	//expand the buffer?
	if (mSavedCount >= mSavedSize)
	{
		//enlarge buffer
		CXMQuery** buf = (CXMQuery**)realloc(mSaved, sizeof(CXMQuery*)*(mSavedSize+4));
		if (!buf)
			return;

		//update vars
		mSaved = buf;
		mSavedSize += 4;
	}

	//copy new item
	mSaved[mSavedCount] = new CXMQuery();
	QueryCopy(query, mSaved[mSavedCount]);
	mSavedCount++;
}

void CXMClientConfig::QueryPersist(IXMLDOMElement *root)
{
	IXMLDOMDocument *xml = NULL;
	IXMLDOMElement *e = NULL;
	IXMLDOMNode *n = NULL;
	int i;
	BSTR saved = SysAllocString(L"saved");

	//get the doc object
	COM_SINGLECALL(root->get_ownerDocument(&xml));

	//create query elements
	for (i=0;i<mSavedCount;i++)
	{
		//create node, append to root
		COM_SINGLECALL(mSaved[i]->ToXml(xml, &e, saved));
		COM_SINGLECALL(root->appendChild(e, &n));

		//release objects
		COM_RELEASE(e);
		COM_RELEASE(n);
	}
	
	//success
	COM_RELEASE(xml);
	SysFreeString(saved);
	return;

fail:
	COM_RELEASE(xml);
	COM_RELEASE(e);
	COM_RELEASE(n);
	SysFreeString(saved);
	return;
}

void CXMClientConfig::QueryLoad(IXMLDOMElement *root)
{
	IXMLDOMNodeList *list = NULL;
	IXMLDOMNode *n = NULL;
	IXMLDOMElement *e = NULL;
	DOMNodeType t;
	CXMQuery* q = new CXMQuery();
	
	//enumerate "saved" nodes
	COM_SINGLECALL(root->getElementsByTagName(_bstr_t("saved"), &list));
	COM_SINGLECALL(list->nextNode(&n));
	while (n)
	{
		//convert to element
		COM_SINGLECALL(n->get_nodeType(&t));
		if (t==NODE_ELEMENT)
		{	
			//load a new query
			COM_SINGLECALL(n->QueryInterface(IID_IXMLDOMElement, (void**)&e));
			COM_SINGLECALL(q->FromXml(e));

			//insert the new query
			QuerySave(q);
			COM_RELEASE(e);
		}

		//next item
		COM_RELEASE(n);
		COM_SINGLECALL(list->nextNode(&n));
	}
	COM_RELEASE(list);

	//create the mru queries
	mMruSize = GetFieldLong(FIELD_SEARCH_AUTOSAVE_COUNT);
	
	//success
	q->Release();
	return;

fail:
	COM_RELEASE(list);
	COM_RELEASE(n);
	COM_RELEASE(e);
	q->Release();
}

//--------------------------------------------------------------------------------------------------
//																					 Registry Access

bool CXMClientConfig::RegGetSmoothSplit()
{
	//do we need to load the value?
	if (!mbHasCheckedSmoothSplitting)
	{
		BOOL b;
		SystemParametersInfo(SPI_GETDRAGFULLWINDOWS, 0, &b, 0);
		mbSmoothSplitting = b?true:false;
		mbHasCheckedSmoothSplitting = true;
	}

	//already loaded the value
	return mbSmoothSplitting;
}

BOOL CXMClientConfig::RegLoad()
{
	//attempt to load registry branch
	HKEY hX;
	if (::RegOpenKeyEx(HKEY_LOCAL_MACHINE, XM_REGKEY, 0, KEY_ALL_ACCESS, &hX)
		==ERROR_SUCCESS)
	{
		//load registry settings
		mRegKey = hX;
		return RegLoadInner();
	}
	else
	{
		//create registry settings
		return RegCreateInner();
	}
}

BOOL CXMClientConfig::RegLoadInner()
{
	//test registry key
	if (!mRegKey) {
		return FALSE;
	}

	//load config path
	DWORD dwLen;
	if (mRegConfigPath) {
		free(mRegConfigPath);
	}
	if (::RegQueryValueEx(mRegKey, XM_REGCONFIGPATH, NULL, NULL,
		NULL, &dwLen)!=ERROR_SUCCESS) {
		return FALSE;
	}
	mRegConfigPath = (char*)malloc(dwLen);
	if (::RegQueryValueEx(mRegKey, XM_REGCONFIGPATH, NULL, NULL,
		(LPBYTE)mRegConfigPath, &dwLen)!=ERROR_SUCCESS) {
		free(mRegConfigPath);
		mRegConfigPath = NULL;
		return FALSE;
	}

	//load system id
	dwLen = 16;
	if (::RegQueryValueEx(mRegKey, XM_REGSYSID, NULL, NULL,
		mSysID.GetValue(), &dwLen)!=ERROR_SUCCESS) {
		return FALSE;
	}

	return TRUE;
}

BOOL CXMClientConfig::RegCreateInner()
{
	//create registry key first
	if (::RegCreateKeyEx(HKEY_LOCAL_MACHINE, XM_REGKEY, 0, NULL,
						 REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, NULL,
						 &mRegKey, NULL)!=ERROR_SUCCESS)
	{
		//failed
		mRegKey = NULL;
		return FALSE;
	}

	//path to config.xml
	char fullpath[MAX_PATH+1];
	BuildFilePath(fullpath, "config.xml");
	mRegConfigPath = strdup(fullpath);

	//default saved path
	BuildFilePath(fullpath, "My Files\\");
	SetField(FIELD_DB_SAVE_PATH, fullpath);

	//default shared path
	BuildFilePath(fullpath, "My Files\\");
	SetField(FIELD_DB_SHARE_PATH, fullpath);

	//setup system id
	mSysID.Random();

	//save values to registry
	return RegSave();
}

BOOL CXMClientConfig::RegSave()
{
	//test our reg key
	if (!mRegKey) {
		return FALSE;
	}

	//save config path
	if (mRegConfigPath) {
		if (::RegSetValueEx(mRegKey, XM_REGCONFIGPATH, 0, REG_SZ, 
			(LPBYTE)mRegConfigPath, strlen(mRegConfigPath)+1)!=ERROR_SUCCESS) {
			return FALSE;
		}
	}

	//save the system id
	if (::RegSetValueEx(mRegKey, XM_REGSYSID, 0, REG_BINARY, 
		mSysID.GetValue(), 16)!=ERROR_SUCCESS) {
		return FALSE;
	}

	return TRUE;
}

char* CXMClientConfig::RegGetConfigPath()
{
	return mRegConfigPath;
}

void CXMClientConfig::RegSetConfigPath(const char* newConfigPath)
{
	if (mRegConfigPath) free(mRegConfigPath);
	mRegConfigPath = strdup(newConfigPath);
	mRegIsDirty = true;
}

CGUID& CXMClientConfig::RegGetSystemID()
{
	return mSysID;
}

//-----------------------------------------------------------------------------
//																  Config Access

bool CXMClientConfig::New()
{
	ENTER();

	//version 0.50
	SetField(FIELD_HASRUN, "false");
	SetField(FIELD_SERVER_ADDRESS, "query1.adultmediaswapper.com");
	SetField(FIELD_SERVER_PORT, "25346");
	SetField(FIELD_LOGIN_LOGINID, "");
	SetField(FIELD_LOGIN_PROTECT_ENABLE, "false");
	SetField(FIELD_LOGIN_PROTECT_PASSWORD, "");
	SetField(FIELD_LOGIN_AUTO_ENABLE, "false");
	SetField(FIELD_LOGIN_AUTO_USERNAME, "");
	SetField(FIELD_LOGIN_AUTO_PASSWORD, "");
	SetField(FIELD_NET_DATARATE, "1");
	SetField(FIELD_NET_CLIENTPORT, "25347");
    SetField(FIELD_DB_FILE, "");
	SetField(FIELD_DB_SHARE_ENABLE, "true");
	SetField(FIELD_DB_SHARE_PATH, "");
	SetField(FIELD_DB_SHARE_USESAVED, "true");
	SetField(FIELD_DB_SAVE_PATH, "");
	SetField(FIELD_PIPELINE_AUTO_UPLOAD, "true");
	SetField(FIELD_PIPELINE_AUTO_DOWNLOAD, "true");
	SetField(FIELD_PIPELINE_MAXFILE, "3");
	SetField(FIELD_PIPELINE_MAXTHUMB, "4");
	SetField(FIELD_PIPELINE_MAXUP, "4");
	SetField(FIELD_SEARCH_AUTOSAVE_ENABLE, "true");
	SetField(FIELD_SEARCH_AUTOSAVE_COUNT, "4");
	SetField(FIELD_GUI_MAIN_SPLIT, "150");
	SetField(FIELD_GUI_MAIN_WND_X, "25");
	SetField(FIELD_GUI_MAIN_WND_Y, "25");
	SetField(FIELD_GUI_MAIN_WND_CX, "600");
	SetField(FIELD_GUI_MAIN_WND_CY, "420");
	SetField(FIELD_GUI_MAIN_WND_MODE, "3");	//maximized
	SetField(FIELD_GUI_LOCAL_SPLIT, "196");	
	SetField(FIELD_GUI_COMPLETED_SPLIT, "196");
	SetField(FIELD_GUI_STATUS_SPLIT, "196");
	SetField(FIELD_GUI_TREE_FOLDED, "false");
	SetField(FIELD_GUI_STATUS_FOLDED, "false");

	//version 0.70
	SetField(FIELD_NET_RECONNECT_ENABLE, "true");
	SetField(FIELD_NET_RECONNECT_DELAY, "15");

	EXIT();
	return true;
}

bool CXMClientConfig::Load(IXMLDOMDocument* xml, char* version)
{
	xml->AddRef();

	//interfaces used
	IXMLDOMNodeList *nodes = NULL;
	IXMLDOMNode *field = NULL;
	IXMLDOMElement *field2 = NULL;
	IXMLDOMElement *root = NULL;
	_variant_t var;
	_bstr_t str;
	BSTR tempstr;
	char* a;

	//upgrade stuff
	char *sz;
	bool upgrade = false;

	//dont allow access to the config during
	//this operations
	ENTER();
	
	//get root node
	COM_SINGLECALL(xml->get_documentElement(&root));

	//get version
	COM_SINGLECALL(root->getAttribute(_bstr_t("version"), &var));
	if (stricmp(_bstr_t(var.bstrVal), version))
	{
		//versions are different
		//HACK: do better checking
		upgrade = true;
	}

	//load all field nodes
	COM_SINGLECALL(xml->getElementsByTagName(_bstr_t("field"), &nodes));

	//search through nodes
	DOMNodeType nodeType;
	COM_SINGLECALL(nodes->nextNode(&field));
	while (field) {
	COM_TRY()

		//add the field if it is element
		COM_CALL(field->get_nodeType(&nodeType));
		if (nodeType==NODE_ELEMENT) 
		{
			COM_CALL(field->QueryInterface(IID_IXMLDOMElement, (void**)&field2));
			COM_CALL(field2->getAttribute(_bstr_t("name"), &var));
			COM_CALL(field2->get_text(&tempstr));
			a = _com_util::ConvertBSTRToString(tempstr);
			SetField(_bstr_t(var.bstrVal), a);
			free(a);
			SysFreeString(tempstr);
		}

		//release node
		COM_RELEASE(field);
		COM_RELEASE(field2);

		//get the next node
		COM_CALL(nodes->nextNode(&field));

	COM_CATCH()
	}

	//initialize mrus
	QueryAlloc();

	//load the persisted queries
	QueryLoad(root);
	FilterLoad(root);

	COM_RELEASE(root);
	COM_RELEASE(nodes);
	COM_RELEASE(xml);

	//upgrade?
	if (upgrade)
	{
		//convert any auto-login password to an md5
		CMD5 md5;
		sz = GetField(FIELD_LOGIN_AUTO_PASSWORD, false);
		if (strlen(sz) > 0)
		{
			md5.FromBuf((BYTE*)sz, strlen(sz));
			SetField(FIELD_LOGIN_AUTO_PASSWORD, md5.GetString());
		}
		sz = NULL;

		//convert password protect password to md5
		sz = GetField(FIELD_LOGIN_PROTECT_PASSWORD, false);
		if (strlen(sz) > 0)
		{
			md5.FromBuf((BYTE*)sz, strlen(sz));
			SetField(FIELD_LOGIN_PROTECT_PASSWORD, md5.GetString());
		}
		sz = NULL;
	}

	EXIT();
	return true;

fail:
	COM_RELEASE(xml);
	COM_RELEASE(nodes);
	COM_RELEASE(field);
	COM_RELEASE(field2);
	COM_RELEASE(root);
	EXIT();
	return false;
}

bool CXMClientConfig::Save(IXMLDOMDocument** pXml, char* version)
{
	//declare interfaces
	IXMLDOMDocument *xml = *pXml;
	IXMLDOMElement *root = NULL;
	IXMLDOMElement *field = NULL;
	IXMLDOMNode *fieldout = NULL;
	int x=0;

	//do we need to creat the doc?
	if (xml==NULL)
	{
		//create doc
		xml = CreateXmlDocument();
		if (xml==NULL)
			return false;
		*pXml = xml;
	}
	else {
		xml = *pXml;
	}
	xml->AddRef();

	//lock access
	ENTER();

	//initialize the document with root node and version number
	VARIANT_BOOL retval;
	COM_TRY()
	COM_CALL(xml->loadXML(_bstr_t("<xmconfig></xmconfig>"), &retval));
	if (retval==VARIANT_FALSE) goto fail;
	COM_CALL(xml->get_documentElement(&root));
	COM_CALL(root->setAttribute(_bstr_t("version"), _variant_t(version)));
	COM_CATCH()

	//read each setting, and add an element
	//to the xml document
	for (x=0;x<mFieldCount;x++)
	{
		COM_TRY()
			COM_CALL(xml->createElement(_bstr_t("field"), &field));
			COM_CALL(field->setAttribute(_bstr_t("name"), _variant_t(mFields[x].name)));
			COM_CALL(field->put_text(_bstr_t(mFields[x].value)));
			COM_CALL(root->appendChild(field, &fieldout));
			if (!fieldout) goto fail;
			COM_RELEASE(field);
			COM_RELEASE(fieldout);
		COM_CATCH()
	}

	//persist all saved queries
	QueryPersist(root);
	FilterPersist(root);
	
	COM_RELEASE(root);
	COM_RELEASE(xml);
	EXIT();
	return true;

fail:
	COM_RELEASE(root);
	COM_RELEASE(xml);
	COM_RELEASE(field);
	COM_RELEASE(fieldout);
	EXIT();
	return false;
}

bool CXMClientConfig::LoadFromFile(char* pFile, char* version)
{	
	IXMLDOMDocument *xml = CreateXmlDocument();
	VARIANT_BOOL bSuccess;
	if (xml==NULL) {
		goto fail;
	}
	COM_SINGLECALL(xml->load(_variant_t(pFile), &bSuccess));
	if (bSuccess==VARIANT_FALSE)
	{
		//dont need xml anymore
		COM_RELEASE(xml);

		//try to create blank config
		if (!this->New()) {
			goto fail;
		}

		//save it to a file
		if (!this->SaveToFile(pFile, version)) {
			goto fail;
		}

		//new() loads all properties, we don't
		//need to go any further
		return true;
	}

	//must first call New, this will ensure that all
	//settings REQUIRED are set
	if (!this->New()) {
		goto fail;
	}

	//garunteed a valid xml doc, load
	//properties from it
	if (!this->Load(xml, version)) {
		goto fail;
	}

	COM_RELEASE(xml);
	return true;

fail:
	COM_RELEASE(xml);
	return false;
}

bool CXMClientConfig::SaveToFile(char* pFile, char* version)
{
	//get an xml doc
	IXMLDOMDocument *xml = NULL;
	if (!Save(&xml, version)) {
		return false;
	}

	//write to disk
	if (FAILED(xml->save(_variant_t(pFile)))) {
		xml->Release();
		return false;
	}

	xml->Release();
	return true;
}

bool CXMClientConfig::SaveToDefaultFile(char* version)
{
	//save to the file we loaded from
	if (mRegConfigPath)
	{
		return SaveToFile(mRegConfigPath, version);
	}
	return false;
}

char* CXMClientConfig::GetField(const char* index, bool copy)
{
	//cant allow modification while
	//we search
	ENTER();

	//search for this item
	Field* f = NULL;
	for(int x=0;x<mFieldCount;x++)
	{
		f = &mFields[x];
		if (_stricmp(index, f->name)==0) {

			//copy & return value
			char *temp;
			if (copy) {
				temp = strdup(f->value);
			}
			else {
				temp = f->value;
			}
			EXIT();
			return temp;
		}
	}
	EXIT();

	//inform user
	char str[MAX_PATH];
	_snprintf(str, MAX_PATH, "Unknown configuration setting requested: %s", index);
	MessageBox(NULL, str, "Configuation Error", MB_OK | MB_ICONERROR);

	throw(0);
	return NULL;
}

char* CXMClientConfig::GetField(const int   index, bool copy)
{
	//bounds-check index
	ENTER();
	if (index >= mFieldCount) {
		EXIT();
		throw(0);
	}

	//return the item
	char* temp;
	if (copy) {
		temp = strdup(mFields[index].value);
	}
	else {
		temp = mFields[index].value;
	}
	EXIT();
	return temp;
}


char* CXMClientConfig::SetField(const char* index, const char* value, const bool freemem)
{
	//sync access
	ENTER();

	//get the field
	Field* f = NULL;
	for(int x=0;x<mFieldCount;x++) {
		if (stricmp(mFields[x].name, index)==0) {
			f = &mFields[x];
			break;
		}
	}

	//did we get a field?
	if (!f) {
		//create new field
		if (!NewField(index, value)) {
			EXIT();
			throw(0);
		}
		EXIT();
		return NULL;
	}

	//update the field, return old val
	char* temp = f->value;
	f->value = strdup(value);
	if (freemem) {
		free(temp);
		temp = NULL;
	}
	EXIT();
	return temp;
}

char* CXMClientConfig::SetField(const int   index, const char* value, const bool freemem)
{
	//sync access
	ENTER();

	//get the field
	Field* f = NULL;
	if (index>=mFieldCount) {
		EXIT();
		throw(0);
	}
	f = &mFields[index];

	//update the field, return old val
	char* temp = f->value;
	f->value = strdup(value);
	if (freemem) {
		free(temp);
		temp = NULL;
	}
	EXIT();
	return temp;
}

char* CXMClientConfig::SetField(const int index, const long value, const bool freemem)
{
	//convert val to char
	char buf[MAX_PATH];
	ltoa(value, buf, 10);
	return SetField(index, buf, freemem);
}

char* CXMClientConfig::SetField(const char* index, const long value, const bool freemem)
{
	//convert val to char
	char buf[MAX_PATH];
	ltoa(value, buf, 10);
	return SetField(index, buf, freemem);
}

bool CXMClientConfig::NewField(const char *index, const char *value)
{
	//sync access
	ENTER();

	//test memory first
	if (mFieldCount<mFieldBufferCount)
	{
		//we have enough room
		memset(&mFields[mFieldCount], NULL, sizeof(Field));
		strncpy(mFields[mFieldCount].name, index, 32);
		mFields[mFieldCount].value = strdup(value);
		if (!mFields[mFieldCount].value) {
			EXIT();
			return false;
		}
		mFieldCount++;
		EXIT();
		return true;
	}

	//reallocate larger buffer
	mFieldBufferCount += 8;
	Field* buf = (Field*)realloc(mFields, sizeof(Field)*mFieldBufferCount);
	if (!buf) {
		mFieldBufferCount -= 8;
		EXIT();
		return false;
	}
	mFields = buf;
	buf = NULL;

	//assign new value
	strncpy(mFields[mFieldCount].name, index, 32);
	mFields[mFieldCount].value = strdup(value);
	if (!mFields[mFieldCount].value) {
		EXIT();
		return false;
	}
	mFieldCount++;
	EXIT();
	return true;
}

void BuildFilePath(char* buf, const char* filename)
{

	//read current directory path
	char path[MAX_PATH+1];
	DWORD retval = ::GetCurrentDirectory(MAX_PATH, path);
	if (retval==0 || retval > MAX_PATH) {
		return;
	}

	//               12345678901         1234567890
	//append either \\config.xml or just config.xml
	if (path[retval-1]=='\\') {
		strcpy(buf, path);
		strcat(buf, filename);
	}
	else {
		strcpy(buf, path);
		strcat(buf, "\\");
		strcat(buf, filename);
	}
}