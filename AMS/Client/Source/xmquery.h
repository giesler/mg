// XMQuery.h: interface for the CXMQuery class.
//
//////////////////////////////////////////////////////////////////////

#pragma once

struct CXMIndex
{
	DWORD Cat1;
	DWORD Cat2;
	BYTE Setting;
	BYTE Rating;
	BYTE Quantity;
	BYTE Content;
	BYTE Build;
	BYTE HairColor;
	BYTE HairStyle;
	BYTE Eyes;
	BYTE Height;
	BYTE Age;
	BYTE Breasts;
	BYTE Nipples;
	BYTE Butt;
	BYTE Race;
	BYTE Quality;
	BYTE Skin;
	BYTE Hips;
	BYTE Legs;
	BYTE FemaleGen;
	BYTE MaleGen;
	BYTE Chest;
	BYTE FacialHair;

	//conversion
	HRESULT ToXml(IXMLDOMDocument *xml, char *name, IXMLDOMElement** out);
	HRESULT FromXml(IXMLDOMElement *e);

	//operations
	bool IsBlank();
	void CopyTo(CXMIndex *dest);
	int CountFields(bool includeCat);
};

class CXMQuery  
{
public:
	
	//construction
	CXMQuery();
	void AddRef();
	void Release();

	//fields
	char *mName;
	DWORD mMinWidth, mMaxWidth;
	DWORD mMinHeight, mMaxHeight;
	DWORD mMinSize, mMaxSize;
	bool mFilter;
	CXMIndex mQuery;
	CXMIndex mRejection;

	//conversion
	HRESULT ToXmlString(char** out);
	HRESULT ToXml(IXMLDOMDocument *xml, IXMLDOMElement** out, BSTR name = NULL);
	HRESULT FromXml(IXMLDOMElement *e);

private:
	~CXMQuery();
	UINT mRefCount;
};

struct CXMQueryResponseItemHost
{
	char Ip[16];
	BYTE Speed;
	bool IsBusy;
};

class CXMQueryResponseItem
{
public:

	//construction
	CXMQueryResponseItem();
	void AddRef();
	void Release();

	//data
	CMD5 mMD5;
	DWORD mWidth, mHeight, mSize;
	CXMQueryResponseItemHost mHosts[5];
	BYTE mHostsCount;

	//misc
	BYTE FindHost();

	//xml conversions
	HRESULT FromXml(IXMLDOMElement* e);
	HRESULT ToXml(IXMLDOMDocument* xml, IXMLDOMElement** e);

private:
	~CXMQueryResponseItem();
	UINT mRefCount;
};

class CXMQueryResponse
{
public:

	//construction
	CXMQueryResponse();
	void AddRef();
	void Release();

	//data
	CTypedPtrList<CPtrList, CXMQueryResponseItem*> mFiles;

	//xml conversions
	HRESULT FromXml(IXMLDOMElement* e);
	HRESULT ToXml(IXMLDOMDocument* xml, IXMLDOMElement** e);

private:
	~CXMQueryResponse();
	UINT mRefCount;
};

class CXMMediaListing
{
public:

	struct item
	{
		CMD5 md5;
		DWORD width;
		DWORD height;
		DWORD size;
		CXMIndex index;
	};

	//data
	CTypedPtrList<CPtrList, item*> mItems;
	bool mFull;

	//construction
	CXMMediaListing();
	void AddRef();
	void Release();

	//operations
	bool FromXml(IXMLDOMElement *e);
	void Apply();

private:
	//ref counted
	~CXMMediaListing();
	UINT mRefCount;
};

void QueryCopy(CXMQuery *source, CXMQuery *dest);