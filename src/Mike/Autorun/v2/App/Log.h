// Log.h: interface for the CLog class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_LOG_H__4477C14F_4485_4D75_B8F9_9F42CEF58600__INCLUDED_)
#define AFX_LOG_H__4477C14F_4485_4D75_B8F9_9F42CEF58600__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class CLog  
{
public:
	void Init(bool p_blnLogEvents);
	void LogEvent(CString p_strEvent);
	CLog();
	~CLog();

private:
	bool m_blnLogEvents;
	CFile m_File;
};

#endif // !defined(AFX_LOG_H__4477C14F_4485_4D75_B8F9_9F42CEF58600__INCLUDED_)
