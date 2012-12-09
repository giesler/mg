// XMQuery.cpp: implementation of the CXMQuery class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "xmlib.h"
#include "xmquery.h"
#include "xmdb.h"
//#include <afxpriv.h>
#include <math.h>

void QueryCopy(CXMQuery *source, CXMQuery *dest)
{
	//copy all indexes
	source->mQuery.CopyTo(&dest->mQuery);
	source->mRejection.CopyTo(&dest->mRejection);

	//copy misc fields
	dest->mFilter = source->mFilter;
	dest->mMaxHeight = source->mMaxHeight;
	dest->mMaxSize = source->mMaxSize;
	dest->mMaxWidth = source->mMaxWidth;
	dest->mMinHeight = source->mMinHeight;
	dest->mMinSize = source->mMinSize;
	dest->mMinWidth = source->mMinWidth;

	//copy name
	if (source->mName)
	{
		if (dest->mName)
			free(dest->mName);
		dest->mName = strdup(source->mName);
	}
}

//-------------------------------------------------------------------------- XMLISTING

CXMMediaListing::CXMMediaListing()
{
	mRefCount = 1;

	mItemsCount = 0;
	mItemsSize = 0;
	mItems = NULL;
}

CXMMediaListing::~CXMMediaListing()
{
	//free any items
	for (DWORD i=0;i<mItemsCount;i++)
		delete mItems[i];
	if (mItems)
		free(mItems);
	
}

void CXMMediaListing::AddRef()
{
	mRefCount++;
}

void CXMMediaListing::Release()
{
	if ((--mRefCount)<1)
		delete this;
}

extern _bstr_t bstrMd5;
_bstr_t bstrWidth("width");
_bstr_t bstrHeight("height");
_bstr_t bstrFileSize("filesize");
_bstr_t bstrIndex("index");

bool CXMMediaListing::FromXml(IXMLDOMElement *e)
{
	IXMLDOMNodeList *list = NULL;
	IXMLDOMNodeList *list2 = NULL;
	IXMLDOMElement *f = NULL;
	IXMLDOMNode *n = NULL;
	DOMNodeType t;
	item *i;
	_variant_t v;

	//get "type" attribute
	//TODO: we don't care right now

	//enumerate media items
	COM_SINGLECALL(e->get_childNodes(&list));
	COM_SINGLECALL(list->nextNode(&n));
	while (n)
	{
		//element?
		COM_SINGLECALL(n->get_nodeType(&t));
		if (t==NODE_ELEMENT)
		{
			COM_SINGLECALL(n->QueryInterface(IID_IXMLDOMElement, (void**)&f));

			//convert to item
			i = new item;
			
			//get md5
			COM_SINGLECALL(f->getAttribute(bstrMd5, &v));
			v.ChangeType(VT_BSTR);
			i->md5 = (_bstr_t)v;
			v.Clear();

			//get width, height, size
			COM_SINGLECALL(f->getAttribute(bstrWidth, &v));
			v.ChangeType(VT_UI4);
			i->width = V_UI4(&v);
			v.Clear();
			COM_SINGLECALL(f->getAttribute(bstrHeight, &v));
			v.ChangeType(VT_UI4);
			i->height = V_UI4(&v);
			v.Clear();
			COM_SINGLECALL(f->getAttribute(bstrFileSize, &v));
			v.ChangeType(VT_UI4);
			i->size = V_UI4(&v);
			v.Clear();

			//enumerate children, look for index
			COM_RELEASE(n);
			COM_SINGLECALL(f->get_childNodes(&list2));
			COM_SINGLECALL(list2->nextNode(&n));
			while(n)
			{
				//is element?
				COM_SINGLECALL(n->get_nodeType(&t));
				if (t==NODE_ELEMENT)
				{
					//convert to element
					COM_RELEASE(f);
					COM_SINGLECALL(n->QueryInterface(IID_IXMLDOMElement, (void**)&f));

					//convert to index
					COM_SINGLECALL(i->index.FromXml(f));
					break;
				}
				
				//next child node
				COM_RELEASE(n);
				COM_SINGLECALL(list2->nextNode(&n));
			}

			//free objects
			COM_RELEASE(n);
			COM_RELEASE(list2);
			COM_RELEASE(f);

			//insert i
			if (++mItemsCount > mItemsSize)
			{
				mItemsSize += 128;
				mItems = (item**)realloc(mItems, mItemsSize*sizeof(item*));
			}
			mItems[mItemsCount-1] = i;
		}
		
		//next media item
		COM_RELEASE(n);
		COM_SINGLECALL(list->nextNode(&n));
	}

	
	//success
	return true;

fail:
	COM_RELEASE(list2);
	COM_RELEASE(list);
	COM_RELEASE(n);
	COM_RELEASE(f);
	return false;
}

void CXMMediaListing::Apply()
{
	//iterate over our media items, and apply the new index
	//to the DBFile.  Automatically overwrites, so should
	//send a Listing to the server first.

	item *i;
	CXMDBFile *f;
	db()->Lock();
	for (DWORD j=0;j<mItemsCount;j++)
	{
		//find db file
		i = mItems[j];
		f = db()->FindFile(i->md5.GetValue(), true);
		if (f)
		{
			//apply index
			i->index.CopyTo(&(f->GetIndex()->data));
			f->SetFlag(DFF_KNOWN, true);
			f->GetIndex()->flags &= ~DIF_DIRTY;
		}
	}
	db()->Unlock();
}

//-------------------------------------------------------------------------- XMINDEX

#define _COUNT32(i32, n) \
for(i=0;i<32;i++)n+=(i32&(1<<i))?1:0

#define _COUNT8(i8, n) \
for(i=0;i<8;i++)n+=(i8&(1<<i))?1:0

float CXMIndex::Score()
{
	// score(n) = 1.2n^(n/100)
	// where n is the number of fields indexed (sans cat1,2)
	int n = CountFields(false);
	float s = (float)pow((float)n, (float)1.2+((float)n/(float)100));

	//catagory: 1st bit = 10 pts, 2nd bit = 5 pts, each additional = 1 pt.
	int i = 0;
	n = 0;
	_COUNT32(Cat1, n);
	_COUNT32(Cat2, n);
	s += ((n--)>0)?10:0;			//add 10 pts if i > 0, decrement i
	s += ((n--)>0)?5:0;			//add 5 pts if i > 0, decrement i
	s += (n>0)?n:0;				//add i pts if i > 0

	return s;
}


int CXMIndex::CountFields(bool includeCat)
{
	int n = 0;
	if (includeCat && (Cat1!=0 || Cat2!=0))
		n++;
	if (Setting!=0) n++;
	if (Rating!=0) n++;
	if (Quantity!=0) n++;
	if (Content!=0) n++;
	if (Build!=0) n++;
	if (HairColor!=0) n++;
	if (HairStyle!=0) n++;
	if (Eyes!=0) n++;
	if (Height!=0) n++;
	if (Age!=0) n++;
	if (Breasts!=0) n++;
	if (Nipples!=0) n++;
	if (Butt!=0) n++;
	if (Race!=0) n++;
	if (Quality!=0) n++;
	if (Skin!=0) n++;
	if (Hips!=0) n++;
	if (Legs!=0) n++;
	if (FemaleGen!=0) n++;
	if (MaleGen!=0) n++;
	if (Chest!=0) n++;
	if (FacialHair!=0) n++;
	return n;
}

int CXMIndex::CountBits(bool includeCat)
{
	int n = 0, i;
	if (includeCat)
	{
		_COUNT32(Cat1, n);
		_COUNT32(Cat2, n);
	}
	_COUNT8(Setting, n);
	_COUNT8(Rating, n);
	_COUNT8(Quantity, n);
	_COUNT8(Content, n);
	_COUNT8(Build, n);
	_COUNT8(HairColor, n);
	_COUNT8(HairStyle, n);
	_COUNT8(Eyes, n);
	_COUNT8(Height, n);
	_COUNT8(Age, n);
	_COUNT8(Breasts, n);
	_COUNT8(Nipples, n);
	_COUNT8(Butt, n);
	_COUNT8(Race, n);
	_COUNT8(Quality, n);
	_COUNT8(Skin, n);
	_COUNT8(Hips, n);
	_COUNT8(Legs, n);
	_COUNT8(FemaleGen, n);
	_COUNT8(MaleGen, n);
	_COUNT8(Chest, n);
	_COUNT8(FacialHair, n);
	return n;
}

bool CXMIndex::IsBlank()
{
	//returns false if anything is non-zero
	return CountFields(true) == 0;
}

#define INDEX_TOXML_HELPER(_size, _field) \
	if (_field != 0) \
	{ \
		VariantInit(&v); \
		V_VT(&v) = VT_UI##_size; \
		V_UI##_size(&v) = _field; \
		COM_SINGLECALL(xml->createElement(_bstr_t(#_field), &f)); \
		COM_SINGLECALL(f->setAttribute(bstrVal, v)); \
		COM_SINGLECALL(e->appendChild(f, &n)); \
		VariantClear(&v); \
		COM_RELEASE(f); \
		COM_RELEASE(n); \
	}

HRESULT CXMIndex::ToXml(IXMLDOMDocument *xml, char *name, IXMLDOMElement** out)
{
	//declares
	IXMLDOMElement *e = NULL, *f = NULL;
	IXMLDOMNode *n = NULL;
	_bstr_t bstrVal("value");
	VARIANT v;
	VariantInit(&v);

	//create base element
	COM_SINGLECALL(xml->createElement(_bstr_t(name), &e));

	//create each child element
	INDEX_TOXML_HELPER(4, Cat1)
	INDEX_TOXML_HELPER(4, Cat2)
	INDEX_TOXML_HELPER(1, Setting)
	INDEX_TOXML_HELPER(1, Rating)
	INDEX_TOXML_HELPER(1, Quantity)
	INDEX_TOXML_HELPER(1, Content)
	INDEX_TOXML_HELPER(1, Build)
	INDEX_TOXML_HELPER(1, HairColor)
	INDEX_TOXML_HELPER(1, HairStyle)
	INDEX_TOXML_HELPER(1, Eyes)
	INDEX_TOXML_HELPER(1, Height)
	INDEX_TOXML_HELPER(1, Age)
	INDEX_TOXML_HELPER(1, Breasts)
	INDEX_TOXML_HELPER(1, Nipples)
	INDEX_TOXML_HELPER(1, Butt)
	INDEX_TOXML_HELPER(1, Race)
	INDEX_TOXML_HELPER(1, Quality)
	INDEX_TOXML_HELPER(1, Skin)
	INDEX_TOXML_HELPER(1, Legs)
	INDEX_TOXML_HELPER(1, FemaleGen)
	INDEX_TOXML_HELPER(1, MaleGen)
	INDEX_TOXML_HELPER(1, Chest)
	INDEX_TOXML_HELPER(1, FacialHair)
	INDEX_TOXML_HELPER(1, Hips)

	//add the source field?
	if (Contest)
	{
		COM_SINGLECALL(e->setAttribute(_bstr_t("source"), _variant_t("contest")));
	}

	//success
	VariantClear(&v);
	COM_RELEASE(f);
	COM_RELEASE(n);
	*out = e;
	return S_OK;

fail:
	VariantClear(&v);
	COM_RELEASE(e);
	COM_RELEASE(f);
	COM_RELEASE(n);
	return S_FALSE;
}

#define INDEX_FROMXML_HELPER(_size, _field) \
	if (stricmp(strname, #_field) == 0) { \
		COM_SINGLECALL(VariantChangeType(&v, &v, 0, VT_UI##_size)); \
		_field = V_UI##_size(&v);  \
	}

HRESULT CXMIndex::FromXml(IXMLDOMElement *index)
{
	//declares
	IXMLDOMNodeList *list = NULL;
	IXMLDOMNode *n = NULL;
	IXMLDOMElement *e = NULL;
	DOMNodeType t;
	BSTR name = NULL;
	char* strname = NULL;
	_bstr_t bstrVal("value");
	VARIANT v;
	VariantInit(&v);

	//init all fields to 0
	Cat1 = 0;
	Cat2 = 0;
	Setting = 0;
	Rating = 0;
	Quantity = 0;
	Content = 0;
	Build = 0;
	HairColor = 0;
	HairStyle = 0;
	Eyes = 0;
	Height = 0;
	Age = 0;
	Breasts = 0;
	Nipples = 0;
	Butt = 0;
	Race = 0;
	Quality = 0;
	Skin = 0;
	Legs = 0;
	FemaleGen = 0;
	MaleGen = 0;
	Chest = 0;
	FacialHair = 0;
	Hips = 0;

	//loop through child nodes
	COM_SINGLECALL(index->get_childNodes(&list));
	COM_SINGLECALL(list->nextNode(&n));
	while(n)
	{
		//is it an element?
		COM_SINGLECALL(n->get_nodeType(&t));
		if (t==NODE_ELEMENT)
		{
			//convert to element
			COM_SINGLECALL(n->QueryInterface(IID_IXMLDOMElement, (void**)&e));

			//read name and value
			COM_SINGLECALL(e->get_nodeName(&name));
			COM_SINGLECALL(e->getAttribute(bstrVal, &v));
			//AfxBSTR2CString(strname, name);
			if (strname)
				free(strname);
			strname = strdup(_bstr_t(name));
			
			//switch on name
			INDEX_FROMXML_HELPER(4, Cat1)
			INDEX_FROMXML_HELPER(4, Cat2)
			INDEX_FROMXML_HELPER(1, Setting)
			INDEX_FROMXML_HELPER(1, Rating)
			INDEX_FROMXML_HELPER(1, Quantity)
			INDEX_FROMXML_HELPER(1, Content)
			INDEX_FROMXML_HELPER(1, Build)
			INDEX_FROMXML_HELPER(1, HairColor)
			INDEX_FROMXML_HELPER(1, HairStyle)
			INDEX_FROMXML_HELPER(1, Eyes)
			INDEX_FROMXML_HELPER(1, Height)
			INDEX_FROMXML_HELPER(1, Age)
			INDEX_FROMXML_HELPER(1, Breasts)
			INDEX_FROMXML_HELPER(1, Nipples)
			INDEX_FROMXML_HELPER(1, Butt)
			INDEX_FROMXML_HELPER(1, Race)
			INDEX_FROMXML_HELPER(1, Quality)
			INDEX_FROMXML_HELPER(1, Skin)
			INDEX_FROMXML_HELPER(1, Legs)
			INDEX_FROMXML_HELPER(1, FemaleGen)
			INDEX_FROMXML_HELPER(1, MaleGen)
			INDEX_FROMXML_HELPER(1, Chest)
			INDEX_FROMXML_HELPER(1, FacialHair)
			INDEX_FROMXML_HELPER(1, Hips)

			//free name and value
			SysFreeString(name);
			name = NULL;
			VariantClear(&v);
			COM_RELEASE(e);
		}

		//get next node
		COM_RELEASE(n);
		COM_SINGLECALL(list->nextNode(&n));
	}

	//default contest value
	Contest = false;

	//look for the source attribute
	::VariantClear(&v);
	COM_SINGLECALL(index->getAttribute(_bstr_t("source"), &v));
	if (V_VT(&v) != VT_NULL)
	{
		//convert to string
		COM_SINGLECALL(::VariantChangeType(&v, &v, 0, VT_BSTR));
		//AfxBSTR2CString(&strname, V_BSTR(&v));
		if (strname)
			free(strname);
		strname = strdup(_bstr_t(V_BSTR(&v)));

		//is it contest?
		if (stricmp(strname, "contest")==0)
		{
			Contest = true;
		}
	}
	
	//success
	if (strname)
		free(strname);
	COM_RELEASE(list);
	VariantClear(&v);
	SysFreeString(name);
	return S_OK;

fail:
	COM_RELEASE(list);
	COM_RELEASE(n);
	COM_RELEASE(e);
	VariantClear(&v);
	SysFreeString(name);
	if (strname)
		free(strname);
	return S_FALSE;
}

void CXMIndex::CopyTo(CXMIndex *dest)
{
	memcpy(dest, this, sizeof(CXMIndex));
}

//-------------------------------------------------------------------------- XMQUERY

CXMQuery::CXMQuery()
{
	mMinWidth = 0;
	mMaxWidth = 0;
	mMinHeight = 0;
	mMaxHeight = 0;
	mMinSize = 0;
	mMaxSize = 0;
	mFilter = true;
	memset(&mQuery, 0, sizeof(CXMIndex));
	memset(&mRejection, 0, sizeof(CXMIndex));
	mRefCount = 1;
	mName = NULL;
	mContest = false;
	mTag = 0;
}

CXMQuery::~CXMQuery()
{
	if (mName)
		free(mName);
}

void CXMQuery::AddRef()
{
	mRefCount++;
}

void CXMQuery::Release()
{
	mRefCount--;
	if (mRefCount<1)
		delete this;
}

_bstr_t bstrQuery("query");
_bstr_t bstrMinWidth("minwidth");
_bstr_t bstrMaxWidth("maxwidth");
_bstr_t bstrMinHeight("minheight");
_bstr_t bstrMaxHeight("maxheight");
_bstr_t bstrMinSize("minsize");
_bstr_t bstrMaxSize("maxsize");
_bstr_t bstrFilter("filter");
extern _bstr_t bstrName;

HRESULT CXMQuery::ToXml(IXMLDOMDocument *xml, IXMLDOMElement** out, BSTR name)
{
	//declares
	IXMLDOMElement *e = NULL;
	IXMLDOMElement *f = NULL;
	IXMLDOMNode *n = NULL;
	VARIANT v;
	VariantInit(&v);

	//create query element
	if (!name)
	{
		name = bstrQuery;
	}
	COM_SINGLECALL(xml->createElement(name, &e));

	//is this a contest?
	if (mContest)
	{
		COM_SINGLECALL(e->setAttribute(_bstr_t("for"), _variant_t("contest")));
	}
	else
	{
		//add bounds checks
		V_VT(&v) = VT_UI4;
		V_UI4(&v) = mMinWidth;
		COM_SINGLECALL(e->setAttribute(bstrMinWidth, v));
		V_UI4(&v) = mMaxWidth;
		COM_SINGLECALL(e->setAttribute(bstrMaxWidth, v));
		V_UI4(&v) = mMinHeight;
		COM_SINGLECALL(e->setAttribute(bstrMinHeight, v));
		V_UI4(&v) = mMaxHeight;
		COM_SINGLECALL(e->setAttribute(bstrMaxHeight, v));
		V_UI4(&v) = mMinSize;
		COM_SINGLECALL(e->setAttribute(bstrMinSize, v));
		V_UI4(&v) = mMaxSize;
		COM_SINGLECALL(e->setAttribute(bstrMaxSize, v));
		VariantClear(&v);

		//add name
		if (mName)
		{
			COM_SINGLECALL(e->setAttribute(bstrName, _variant_t(_bstr_t(mName))));
		}

		//turn off filtering?
		if (!mFilter)
		{
			COM_SINGLECALL(e->setAttribute(bstrFilter, _variant_t("none")));
		}

		//append query index
		COM_SINGLECALL(mQuery.ToXml(xml, "queryindex", &f));
		COM_SINGLECALL(e->appendChild(f, &n));
		COM_RELEASE(n);
		COM_RELEASE(f);

		//append rejection index
		COM_SINGLECALL(mRejection.ToXml(xml, "rejectionindex", &f));
		COM_SINGLECALL(e->appendChild(f, &n));
		COM_RELEASE(n);
		COM_RELEASE(f);
	}
	
	//success
	*out = e;
	return S_OK;

fail:
	COM_RELEASE(e);
	COM_RELEASE(f);
	COM_RELEASE(n);
	VariantClear(&v);
	return S_FALSE;
}

HRESULT CXMQuery::ToXmlString(char** out)
{
	//document and fragment
	IXMLDOMDocument *xml = NULL;
	IXMLDOMDocumentFragment *fragment = NULL;
	IXMLDOMElement *e = NULL;
	IXMLDOMNode *n = NULL;
	BSTR bstrXml = NULL;

	//create doc and fragment
	xml = CreateXmlDocument();
	if (!xml) goto fail;
	COM_SINGLECALL(xml->createDocumentFragment(&fragment));

	//convert query to xml
	COM_SINGLECALL(ToXml(xml, &e));
	COM_SINGLECALL(fragment->appendChild(e, &n));
	COM_RELEASE(n);
	COM_RELEASE(e);

	//copy string
	COM_SINGLECALL(fragment->get_xml(&bstrXml));
	*out = _com_util::ConvertBSTRToString(bstrXml);
	SysFreeString(bstrXml);
	
	//success
	COM_RELEASE(xml);
	COM_RELEASE(fragment);
	return S_OK;

fail:
	COM_RELEASE(xml);
	COM_RELEASE(fragment);
	COM_RELEASE(e);
	COM_RELEASE(n);
	SysFreeString(bstrXml);
	*out = NULL;
	return S_FALSE;
}

HRESULT CXMQuery::FromXml(IXMLDOMElement *query)
{
	//declares
	IXMLDOMNamedNodeMap *map = NULL;
	IXMLDOMNode *n = NULL;
	IXMLDOMElement *e = NULL;
	IXMLDOMNodeList *list = NULL;
	VARIANT v;
	BSTR name = NULL;
	char* strname = NULL;
	DOMNodeType t;
	VariantInit(&v);

	//is this for the contest?
	::VariantClear(&v);
	COM_SINGLECALL(query->getAttribute(_bstr_t("for"), &v));
	if (V_VT(&v) != VT_NULL)
	{
		//convert to string
		COM_SINGLECALL(::VariantChangeType(&v, &v, 0, VT_BSTR));
		//AfxBSTR2CString(&strname, V_BSTR(&v));
		if (strname)
			free(strname);
		strname = strdup(_bstr_t(V_BSTR(&v)));

		//is it contest?
		if (stricmp(strname, "contest")==0)
		{
			mContest = true;
		}
	}

	//only do the rest if it ISNT contest
	if (!mContest)
	{
		//set bound checks
		COM_SINGLECALL(query->get_attributes(&map));
		COM_SINGLECALL(map->nextNode(&n));
		while(n)
		{
			//retrieve name and value
			COM_SINGLECALL(n->get_nodeName(&name));
			COM_SINGLECALL(n->get_nodeValue(&v));
			//AfxBSTR2CString(&strname, name);
			if (strname)
				free(strname);
			strname = strdup(_bstr_t(name));
			
			//which attribute?
			if (stricmp(strname, "minwidth")==0)
			{
				COM_SINGLECALL(VariantChangeType(&v, &v, 0, VT_UI4));
				mMinWidth = V_UI4(&v);
			}
			else if (stricmp(strname, "maxwidth")==0)
			{
				COM_SINGLECALL(VariantChangeType(&v, &v, 0, VT_UI4));
				mMaxWidth = V_UI4(&v);
			}
			else if (stricmp(strname, "minheight")==0)
			{
				COM_SINGLECALL(VariantChangeType(&v, &v, 0, VT_UI4));
				mMinHeight = V_UI4(&v);
			}
			else if (stricmp(strname, "maxheight")==0)
			{
				COM_SINGLECALL(VariantChangeType(&v, &v, 0, VT_UI4));
				mMaxHeight = V_UI4(&v);
			}
			else if (stricmp(strname, "minsize")==0)
			{
				COM_SINGLECALL(VariantChangeType(&v, &v, 0, VT_UI4));
				mMinSize = V_UI4(&v);
			}
			else if (stricmp(strname, "maxsize")==0)
			{
				COM_SINGLECALL(VariantChangeType(&v, &v, 0, VT_UI4));
				mMaxSize = V_UI4(&v);
			}
			else if (stricmp(strname, "name")==0)
			{
				COM_SINGLECALL(VariantChangeType(&v, &v, 0, VT_BSTR));
				mName = (char*)malloc((wcslen(V_BSTR(&v))*2)+1);
				wcstombs(mName, V_BSTR(&v), MAX_PATH);
			}
			
			//next item
			SysFreeString(name);
			name = NULL;
			VariantClear(&v);
			COM_RELEASE(n);
			COM_SINGLECALL(map->nextNode(&n));
		}
		COM_RELEASE(map);

		//load query index
		COM_SINGLECALL(query->get_childNodes(&list));
		COM_SINGLECALL(list->nextNode(&n));
		while(n)
		{
			//can convert to element?
			COM_SINGLECALL(n->get_nodeType(&t));
			if (t==NODE_ELEMENT)
			{
				//convert to element
				COM_SINGLECALL(n->QueryInterface(IID_IXMLDOMElement, (void**)&e));

				//test name, execute proper read method
				COM_SINGLECALL(e->get_nodeName(&name));
				//AfxBSTR2CString(&strname, name);
				if (strname)
					free(strname);
				strname = strdup(_bstr_t(name));
				if (stricmp(strname, "queryindex")==0)
				{
					COM_SINGLECALL(mQuery.FromXml(e));
				}
				if (stricmp(strname, "rejectionindex")==0)
				{
					COM_SINGLECALL(mRejection.FromXml(e));
				}
				COM_RELEASE(e);
				SysFreeString(name);
				name = NULL;
			}

			//next item
			COM_RELEASE(n);
			COM_SINGLECALL(list->nextNode(&n));
		}
		COM_RELEASE(list);
	} //if !mContest

	//success
	if (strname)
		free(strname);
	VariantClear(&v);
	SysFreeString(name);
	return S_OK;

fail:
	COM_RELEASE(map);
	COM_RELEASE(n);
	COM_RELEASE(list);
	VariantClear(&v);
	SysFreeString(name);
	if (strname)
		free(strname);
	return S_FALSE;
}

//-------------------------------------------------------------------------- QUERYRESPONSE

CXMQueryResponseItem::CXMQueryResponseItem()
{
	mWidth = 0;
	mHeight = 0;
	mSize = 0;
	mSponsor = XMSPONSOR_NONE;

	//zero out hosts
	for(int i=0;i<5;i++) {
		mHosts[i].Ip[0] = 0;
		mHosts[i].Speed = 0;
		mHosts[i].IsBusy = false;
	}
	mHostsCount = 0;

	mRefCount = 1;
}

CXMQueryResponseItem::~CXMQueryResponseItem()
{
}

void CXMQueryResponseItem::AddRef()
{
	mRefCount++;
}

void CXMQueryResponseItem::Release()
{
	mRefCount--;
	if (mRefCount<1)
		delete this;
}

CXMQueryResponse::CXMQueryResponse()
{
	mRefCount = 1;
	mFilesCount = 0;
	mFilesSize = 0;
	mFiles = NULL;
}

CXMQueryResponse::~CXMQueryResponse()
{
	//release each item
	for(DWORD i=0;i<mFilesCount;i++)
		mFiles[i]->Release();

	//release the array
	if (mFiles)
		free(mFiles);
}

void CXMQueryResponse::AddRef()
{
	mRefCount++;
}

void CXMQueryResponse::Release()
{
	mRefCount--;
	if (mRefCount<1)
		delete this;
}

BYTE CXMQueryResponseItem::FindHost()
{
	//build list of eligable hosts
	BYTE h[5] = {0,0,0,0,0};
	BYTE hcount = 0;
	for (BYTE i=0;i<mHostsCount;i++)
	{
		if (!mHosts[i].IsBusy)
		{
			h[hcount] = i;
			hcount++;
		}
	}
	
	//did we find anything?
	if (hcount==0){
		return -1;
	}

	//add up the speeds of everyone
	BYTE s=0;
	for (i=0;i<hcount;i++)
		s+=mHosts[h[i]].Speed;

	//pick a random number
	int r = ((rand()*s/RAND_MAX))+1;

	//walk the list until r runs out
	for (i=0;i<hcount;i++)
	{
		r-=mHosts[h[i]].Speed;
		if (r<=0)
		{
			return h[i];
		}
	}

	//we should not get here
	return h[0];
}

HRESULT CXMQueryResponseItem::FromXml(IXMLDOMElement* e)
{
	//declares
	IXMLDOMNamedNodeMap *attributes = NULL;
	IXMLDOMNodeList *list = NULL;
	IXMLDOMNode *n = NULL;
	IXMLDOMElement *f = NULL;
	BSTR bstrName = NULL;
	VARIANT v, v2;
	VariantInit(&v);
	VariantInit(&v2);
	char* a = NULL;
	char b[MAX_PATH+1];
	DOMNodeType t;

	//read attributes
	COM_SINGLECALL(e->get_attributes(&attributes));
	COM_SINGLECALL(attributes->nextNode(&n));
	while (n)
	{
		//get name and value
		COM_SINGLECALL(n->get_nodeName(&bstrName));
		COM_SINGLECALL(n->get_nodeValue(&v));

		//convert to ansi
		wcstombs(b, bstrName, MAX_PATH);
		
		if (_stricmp(b, "md5")==0)
		{
			COM_SINGLECALL(VariantChangeType(&v, &v, 0, VT_BSTR));
			a = _com_util::ConvertBSTRToString(V_BSTR(&v));
			mMD5.SetValue(a);
			free(a);
			a = NULL;
		}
		else if (_stricmp(b, "width")==0)
		{
			COM_SINGLECALL(VariantChangeType(&v, &v, 0, VT_UI4));
			mWidth = V_UI4(&v);
		}
		else if (_stricmp(b, "height")==0)
		{
			COM_SINGLECALL(VariantChangeType(&v, &v, 0, VT_UI4));
			mHeight = V_UI4(&v);
		}
		else if (_stricmp(b, "size")==0)
		{
			COM_SINGLECALL(VariantChangeType(&v, &v, 0, VT_UI4));
			mSize = V_UI4(&v);
		}
		else if (_stricmp(b, "sponsor")==0)
		{
			COM_SINGLECALL(VariantChangeType(&v, &v, 0, VT_I4));
			mSponsor = V_I4(&v);
		}

		//next attribute
		VariantClear(&v);
		SysFreeString(bstrName);
		COM_RELEASE(n);
		COM_SINGLECALL(attributes->nextNode(&n));
	}
	COM_RELEASE(attributes);

	//read all the hosts
	mHostsCount = 0;
	COM_SINGLECALL(e->get_childNodes(&list));
	COM_SINGLECALL(list->nextNode(&n));
	while (n)
	{
		//is it an element?
		COM_SINGLECALL(n->get_nodeType(&t));
		if (t==NODE_ELEMENT)
		{
			//convert to element
			COM_SINGLECALL(n->QueryInterface(IID_IXMLDOMElement, (void**)&f));
			
			//does it have an "ip" attribute?
			if(	!FAILED(f->getAttribute(_bstr_t("ip"), &v)) &&
				!FAILED(f->getAttribute(_bstr_t("speed"), &v2))	)
			{
				//add to host collection
				COM_SINGLECALL(VariantChangeType(&v, &v, 0, VT_BSTR));
				COM_SINGLECALL(VariantChangeType(&v2, &v2, 0, VT_UI1));
				a = _com_util::ConvertBSTRToString(V_BSTR(&v));
				strncpy(mHosts[mHostsCount].Ip, a, 16);
				mHosts[mHostsCount].Speed = V_UI1(&v2);

				//free memory
				free(a);
				a = NULL;
				VariantClear(&v);
				VariantClear(&v2);

				//increment counter
				mHostsCount++;
			}

			//free stuff
			COM_RELEASE(f);
		}

		//next node
		COM_RELEASE(n);
		COM_SINGLECALL(list->nextNode(&n));
	}
	COM_RELEASE(list);

	//we look into our current database and see
	//if this file is already local
	if (db()->FindFile(mMD5.GetValue(), false))
		mAlreadyGotIt = TRUE;
	else
		mAlreadyGotIt = FALSE;

	//success
	return S_OK;

fail:
	COM_RELEASE(attributes);
	COM_RELEASE(list);
	COM_RELEASE(n);
	COM_RELEASE(f);
	SysFreeString(bstrName);
	VariantClear(&v);
	VariantClear(&v2);
	return S_FALSE;
}

HRESULT CXMQueryResponseItem::ToXml(IXMLDOMDocument* xml, IXMLDOMElement** e)
{
	//declares
	IXMLDOMElement *f = NULL, *g = NULL;
	IXMLDOMNode *n = NULL;
	VARIANT v;
	VariantInit(&v);
	int i=0;

	//create our element
	COM_SINGLECALL(xml->createElement(_bstr_t("item"), &f));

	//add ui4 attributes
	V_VT(&v) = VT_UI4;
	V_UI4(&v) = mWidth;
	COM_SINGLECALL(f->setAttribute(_bstr_t("width"), v));
	V_UI4(&v) = mHeight;
	COM_SINGLECALL(f->setAttribute(_bstr_t("height"), v));
	V_UI4(&v) = mSize;
	COM_SINGLECALL(f->setAttribute(_bstr_t("size"), v));

	//add bstr attributes
	V_VT(&v) = VT_BSTR;
	V_BSTR(&v) = _com_util::ConvertStringToBSTR(mMD5.GetString());
	COM_SINGLECALL(f->setAttribute(_bstr_t("md5"), v));
	VariantClear(&v);

	//add each host entry
	for(i=0;i<mHostsCount;i++)
	{
		//convert current string into host entry
		COM_SINGLECALL(xml->createElement(_bstr_t("host"), &g));
		V_VT(&v) = VT_UI1;
		V_UI1(&v) = mHosts[i].Speed;
		COM_SINGLECALL(g->setAttribute(_bstr_t("speed"), v));
		V_VT(&v) = VT_BSTR;
		V_BSTR(&v) = _com_util::ConvertStringToBSTR(mHosts[i].Ip);
		COM_SINGLECALL(g->setAttribute(_bstr_t("ip"), v));
		COM_SINGLECALL(f->appendChild(g, &n));

		//free
		VariantClear(&v);
		COM_RELEASE(g);
		COM_RELEASE(n);
	}

	//success
	*e = f;
	return S_OK;

fail:
	COM_RELEASE(f);
	COM_RELEASE(g);
	COM_RELEASE(n);
	VariantClear(&v);
	return S_FALSE;
}

HRESULT CXMQueryResponse::FromXml(IXMLDOMElement* e)
{
	//declares
	IXMLDOMNodeList *list = NULL;
	IXMLDOMNode *n = NULL;
	IXMLDOMElement *f = NULL;
	DOMNodeType t;

	//loop through all response item nodes
	CXMQueryResponseItem *i = NULL;
	COM_SINGLECALL(e->get_childNodes(&list));
	COM_SINGLECALL(list->nextNode(&n));
	while(n)
	{
		//is it an element?
		COM_SINGLECALL(n->get_nodeType(&t));
		if (t==NODE_ELEMENT)
		{
			//convert element into query response item
			i = new CXMQueryResponseItem();
			COM_SINGLECALL(n->QueryInterface(IID_IXMLDOMElement, (void**)&f));
			COM_SINGLECALL(i->FromXml(f));

			//store the item
			if (++mFilesCount > mFilesSize)
			{
				mFilesSize += 20;
				mFiles = (CXMQueryResponseItem**)realloc(mFiles, mFilesSize*sizeof(CXMQueryResponseItem*));
			}
			mFiles[mFilesCount-1] = i;
			i = NULL;

			//free
			COM_RELEASE(f);
		}

		//next node
		COM_RELEASE(n);
		COM_SINGLECALL(list->nextNode(&n));
	}
	COM_RELEASE(list);

	//success
	return S_OK;

fail:
	COM_RELEASE(list);
	COM_RELEASE(n);
	COM_RELEASE(f);
	return S_FALSE;
}

HRESULT CXMQueryResponse::ToXml(IXMLDOMDocument* xml, IXMLDOMElement** e)
{
	//declares
	IXMLDOMElement *f = NULL, *g = NULL;
	IXMLDOMNode *n = NULL;
	DWORD i;

	//create our element
	COM_SINGLECALL(xml->createElement(_bstr_t("results"), &f));

	//convert each item into xml
	for(i=0;i<mFilesCount;i++)
	{
		COM_SINGLECALL(mFiles[i]->ToXml(xml, &g));
		COM_SINGLECALL(f->appendChild(g, &n));
		COM_RELEASE(g);
		COM_RELEASE(n);
	}
	
	//success
	*e = f;
	return S_OK;

fail:
	COM_RELEASE(f);
	COM_RELEASE(g);
	COM_RELEASE(n);
	return S_FALSE;
}
