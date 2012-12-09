// StatusObj.h: interface for the StatusObj class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_STATUSOBJ_H__61F228C6_D848_49D2_B53A_1F9A782BB761__INCLUDED_)
#define AFX_STATUSOBJ_H__61F228C6_D848_49D2_B53A_1F9A782BB761__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class StatusObj  
{
public:
	StatusObj();
	void AddRef();

private:
	~StatusObj();
	int refcount;
};

#endif // !defined(AFX_STATUSOBJ_H__61F228C6_D848_49D2_B53A_1F9A782BB761__INCLUDED_)
