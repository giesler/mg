// XMClient.h : main header file for the XMClient application
//
#pragma once


#include "Resources\resource.h"       // main symbols

//thumbnail size
#define XMGUI_THUMBWIDTH		124	//4:3
#define XMGUI_THUMBHEIGHT		93

// -------------------------------------------------------------------------- MD5

// first, some types:
typedef unsigned      long uint4; // assumes integer is 4 words long
typedef unsigned short int uint2; // assumes short integer is 2 words long
typedef unsigned      char uint1; // assumes char is 1 word long

#define XM_GUIDSTRINGLENGTH		34

//md5 util functions
inline void md5tohex(char* buf, const BYTE* md5, int count=16) {
	for (int i=0; i<count; i++) {
		sprintf(buf+i*2, "%02x", md5[i]);
	}
}

inline void hextomd5(BYTE* md5, const char* buf, int count=16) {
	for (int i=0; i<count; i++) {
		sscanf(buf+i*2, "%2x", md5+i);
	}
}

inline bool md5comp(BYTE* md5a, BYTE* md5b) {
	for (int i=0; i<16; i++) {
		if (md5a[i]!=md5b[i]) {
			return false;
		}
	}
	return true;
}

/////////////////////////////////////////////////////////////////////////////
// CMD5Engine
class CMD5Engine
{
public:
	CMD5Engine()
	{
		init();
	}

public:
// methods for controlled operation:
  //MD5              ();  // simple initializer
  void  update     (unsigned char *input, unsigned long input_length);
  void  update     (FILE *file);
  void  finalize   ();

// methods to acquire finalized result
  unsigned char    *raw_digest ();  // digest as a 16-byte binary array
  char *            hex_digest ();  // digest as a 33-byte ascii-hex string

private:

// next, the private data:
  uint4 state[4];
  uint4 count[2];     // number of *bits*, mod 2^64
  uint1 buffer[64];   // input buffer
  uint1 digest[16];
  uint1 finalized;
  char  mtempchar[33];

// last, the private methods, mostly static:
  void init             ();               // called by all constructors
  void transform        (uint1 *buffer);  // does the real update work.  Note 
                                          // that length is implied to be 64.

  static void encode    (uint1 *dest, uint4 *src, uint4 length);
  static void decode    (uint4 *dest, uint1 *src, uint4 length);

  static inline uint4  rotate_left (uint4 x, uint4 n);
  static inline uint4  F           (uint4 x, uint4 y, uint4 z);
  static inline uint4  G           (uint4 x, uint4 y, uint4 z);
  static inline uint4  H           (uint4 x, uint4 y, uint4 z);
  static inline uint4  I           (uint4 x, uint4 y, uint4 z);
  static inline void   FF  (uint4& a, uint4 b, uint4 c, uint4 d, uint4 x, 
			    uint4 s, uint4 ac);
  static inline void   GG  (uint4& a, uint4 b, uint4 c, uint4 d, uint4 x, 
			    uint4 s, uint4 ac);
  static inline void   HH  (uint4& a, uint4 b, uint4 c, uint4 d, uint4 x, 
			    uint4 s, uint4 ac);
  static inline void   II  (uint4& a, uint4 b, uint4 c, uint4 d, uint4 x, 
			    uint4 s, uint4 ac);

};

//utility classes for md5s and guids
class C16ByteValue
{
public:
	C16ByteValue() {
		mStr[0] = '\0';
	};
	C16ByteValue(const char* val) {
		SetValue(val);
	};
	C16ByteValue(BYTE* val) {
		SetValue(val);
	};
	C16ByteValue(C16ByteValue &val) {
		SetValue(val);
	};
	~C16ByteValue() {};

	//misc
	inline void Random() {
		::UuidCreate((UUID*)mData);
	}
	inline bool IsEqual(C16ByteValue &val)
	{
		return md5comp(mData, val.GetValue());
	}
	inline void Zero() {
		mData[ 0] = 0;
		mData[ 1] = 0;
		mData[ 2] = 0;
		mData[ 3] = 0;
		mData[ 4] = 0;
		mData[ 5] = 0;
		mData[ 6] = 0;
		mData[ 7] = 0;
		mData[ 8] = 0;
		mData[ 9] = 0;
		mData[10] = 0;
		mData[11] = 0;
		mData[12] = 0;
		mData[13] = 0;
		mData[14] = 0;
		mData[15] = 0;
	}

	//get
	inline char* GetString() {
		if (mStr[0]=='\0') {
			md5tohex(mStr, mData);
		}
		return mStr;
	}
	inline BYTE* GetValue() {
		return mData;
	};

	//set
	inline void SetValue(C16ByteValue &val) {
		memcpy(mData, val.GetValue(), 16);
		mStr[0] = '\0';
	};
	inline void SetValue(BYTE* val) {
		memcpy(mData, val, 16);
		mStr[0] = '\0';
	};
	inline void SetValue(const char* val) {
		strncpy(mStr, val, 32);
		hextomd5(mData, mStr);
	};
	inline C16ByteValue& operator =(C16ByteValue &val) {
		SetValue(val);
		return *this;
	};
	inline C16ByteValue& operator =(BYTE* val) {
		SetValue(val);
		return *this;
	};
	inline C16ByteValue& operator =(const char* val) {
		SetValue(val);
		return *this;
	};

	//md5engine passthroughs
	inline void FromFile(FILE* file) {
		CMD5Engine e;
		e.update(file);
		e.finalize();
		SetValue(e.raw_digest());
	};
	inline void FromFile(const char* path) {
		FILE* file = fopen(path, "rb");
		if (file) {
			FromFile(file);
		}
	};
	inline void FromBuf(BYTE* buf, DWORD length) {
		CMD5Engine e;
		e.update(buf, length);
		e.finalize();
		SetValue(e.raw_digest());
	};

protected:

	BYTE mData[16];
	char mStr[33];
};

typedef C16ByteValue CGUID;
typedef C16ByteValue CMD5;

// ---------------------------------------------------------------------- Win App

class CXMClientApp : public CWinApp
{
public:
	CXMClientApp();

	char* Version();
	bool m_bHasInit;
	HANDLE m_hMutex;

//Overrides
public:
	virtual BOOL InitInstance();
	virtual int ExitInstance();

// Implementation
public:
	DECLARE_MESSAGE_MAP()
};

// ---------------------------------------------------------------------- Config

#include "xmquery.h"


//root of our registry key
#define XM_REGKEY						"SOFTWARE\\AMS\\Client"
#define XM_REGCONFIGPATH				"ConfigPath"
#define XM_REGSYSID						"SystemID"

//named fields

#define FIELD_HASRUN					"hasrun"

#define FIELD_DB_SHARE_ENABLE			"db/sharing/enable"
#define FIELD_DB_FILE					"db/file"
#define FIELD_DB_SHARE_PATH				"db/share/path"
#define FIELD_DB_SHARE_USESAVED			"db/share/usesaved"
#define FIELD_DB_SAVE_PATH				"db/save/path"

#define FIELD_SERVER_ADDRESS			"server/address"
#define FIELD_SERVER_PORT				"server/port"

#define FIELD_LOGIN_LOGINID				"login/loginid"
#define FIELD_LOGIN_LAST				"login/last"

#define FIELD_LOGIN_PROTECT_ENABLE		"login/protect/enable"
#define FIELD_LOGIN_PROTECT_PASSWORD	"login/protect/password"
#define FIELD_LOGIN_AUTO_ENABLE			"login/auto/enable"
#define FIELD_LOGIN_AUTO_USERNAME		"login/auto/username"
#define FIELD_LOGIN_AUTO_PASSWORD		"login/auto/password"

#define FIELD_SEARCH_AUTOSAVE_ENABLE	"search/autosave/enable"
#define FIELD_SEARCH_AUTOSAVE_COUNT		"search/autosave/count"

#define FIELD_NET_DATARATE				"net/datarate"
#define FIELD_NET_CLIENTPORT			"net/clientport"
#define FIELD_NET_RECONNECT_ENABLE		"net/reconnect/enable"
#define FIELD_NET_RECONNECT_DELAY		"net/reconnect/delay"

#define FIELD_PIPELINE_AUTO_UPLOAD		"pipeline/auto/upload"
#define FIELD_PIPELINE_AUTO_DOWNLOAD	"pipeline/auto/download"
#define FIELD_PIPELINE_MAXFILE			"pipeline/maxfile"
#define FIELD_PIPELINE_MAXTHUMB			"pipeline/maxthumb"
#define FIELD_PIPELINE_MAXUP			"pipeline/maxup"

#define FIELD_GUI_MAIN_SPLIT			"gui/main/split"
#define FIELD_GUI_MAIN_WND_X			"gui/main/wnd/x"
#define FIELD_GUI_MAIN_WND_Y			"gui/main/wnd/y"
#define FIELD_GUI_MAIN_WND_CX			"gui/main/wnd/cx"
#define FIELD_GUI_MAIN_WND_CY			"gui/main/wnd/cy"
#define FIELD_GUI_MAIN_WND_MODE			"gui/main/wnd/mode"
#define FIELD_GUI_TREE_FOLDED			"gui/tree/folded"
#define FIELD_GUI_LOCAL_SPLIT			"gui/local/split"
#define FIELD_GUI_COMPLETED_SPLIT		"gui/completed/split"
#define FIELD_GUI_STATUS_SPLIT			"gui/status/split"
#define FIELD_GUI_STATUS_FOLDED			"gui/status/folded"

class CXMClientConfig : public CObject
{
friend class CXMClientConfig;
friend class CPreferences;
public:
    CXMClientConfig();
	~CXMClientConfig();

	//registry access
	BOOL RegLoad();
	BOOL RegSave();
	char* RegGetConfigPath();
	void RegSetConfigPath(const char* newConfigPath);
	CGUID& RegGetSystemID();
	bool RegGetSmoothSplit();

	//field structure
	struct Field
	{
		char name[32];
		char *value;
	};

	//query persistence
	void FilterSet(CXMIndex *filter);
	void FilterGet(CXMIndex *filter);
	int QueryGetCount();
	void QueryGetItem(int i, CXMQuery** query);
	void QueryDeleteItem(int i);
	void QueryMru(CXMQuery* query);
	void QuerySave(CXMQuery* query);

	//load & save
	bool New();
	bool Load(IXMLDOMDocument* xml);
	bool LoadFromFile(char* pFile);
	bool Save(IXMLDOMDocument** xml);
	bool SaveToFile(char* pFile);
	bool SaveToDefaultFile();

	//preferences
	bool DoPrefs(bool wizard);

	//public interface to fields
	inline int   GetFieldCount() { return mFieldCount; };
	char* GetField(const char* index, bool copy = true);
	char* GetField(const int   index, bool copy = true);
	char* SetField(const char* index, const char* value, const bool freemem = true);
	char* SetField(const int   index, const char* value, const bool freemem = true);
	char* SetField(const char* index, const long value, const bool freemem = true);
	char* SetField(const int   index, const long value, const bool freemem = true);
	char* operator[](const char* index) { return GetField(index); };
	char* operator[](const int   index) { return GetField(index); };

	//named field helper functions
	inline char* GetServerAddress() {
		return GetField(FIELD_SERVER_ADDRESS, true);
	};
	inline unsigned int GetServerPort() {
		return (unsigned int)GetFieldLong(FIELD_SERVER_PORT);
	};
	inline long GetFieldLong(const char* index) {
		char* temp = GetField(index, false);
		if (!temp) throw(0);
		return atol(temp);
	};
	inline bool GetFieldBool(const char* index) {
		char* temp = GetField(index, false);
		if (!temp) throw(0);
		return (stricmp(temp, "true")==0)?true:false;
	};

	//helper functions with logic
	char* GetSharedFilesLocation(bool copy = true);

private:
	
	//copy the entire config from one object to another
	static void Copy(CXMClientConfig *pfrom, CXMClientConfig *pto);

	//registry access
	BOOL RegLoadInner();				//load registry data
	BOOL RegCreateInner();				//create registry data
	HKEY mRegKey;						//current registry key
	bool mRegIsDirty;					//track state of registry vars
	char* mRegConfigPath;				//path to our config.xml file
	CGUID mSysID;						//our system id

	//smooth scrolling
	bool mbHasCheckedSmoothSplitting;
	bool mbSmoothSplitting;

	//queries
	void FilterPersist(IXMLDOMElement *root);
	void FilterLoad(IXMLDOMElement *root);
	void QueryFree();
	void QueryAlloc();
	void QueryPersist(IXMLDOMElement *root);
	void QueryLoad(IXMLDOMElement *root);
	CXMIndex mFilter;
	CXMQuery** mSaved;
	CXMQuery** mMru;
	int mSavedCount;
	int mSavedSize;
	int mMruCount;
	int mMruSize;

	//fields
	CRITICAL_SECTION mSync;				//sync object
	Field* mFields;						//pointer to array of fields
	int mFieldCount;					//logical count of fields
	int mFieldBufferCount;				//physical count of allocated fields
	bool NewField(const char* index, \
				  const char* value);	//create a new field
};

// ---------------------------------------------------------------------- Utils

//about box
void DoAbout(CWnd *parent);

bool DoPasswordCheck();

//utility function
IXMLDOMDocument* CreateXmlDocument();
void BuildFilePath(char* buf, const char* filename);

//COM helpers
inline void COM_CALL(HRESULT _hr) {
	if (FAILED(_hr)) {
		throw(_hr);
	}
}
#define COM_TRY() try {
#define COM_CATCH() } catch(...) { goto fail; }
#define COM_SINGLECALL(_call) if (FAILED(_call)) goto fail
#define COM_RELEASE(_var) if (_var) { \
							_var->Release(); \
							_var = NULL; \
						  }

// ---------------------------------------------------------------------- Globals

class CXMSessionManager;
class CXMDBManager;
class CXMClientManager;
class CXMServerManager;
class CXMDB;
class CXMAsyncResizer;

//app helper function
inline CXMClientApp* app() {
	return (CXMClientApp*)AfxGetApp();
}
extern inline CXMSessionManager* sessions();
extern inline CXMClientConfig* config();
extern inline CXMDB* db();
extern inline CXMDBManager* dbman();
extern inline CXMClientManager* cm();
extern inline CXMServerManager* sm();
extern inline CXMAsyncResizer* ar();
