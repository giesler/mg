// Log.cpp: implementation of the CLog class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "autorun.h"
#include "Log.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CLog::CLog() {

}


CLog::~CLog()
{
	if (!m_blnLogEvents) return;

	CString strTemp;
	LogEvent("Application Ended.");

}



void CLog::LogEvent(CString p_strEvent)
{
	if (!m_blnLogEvents) return;

	CString strOutput;
	m_File.Open(mstrFilename, CFile::modeWrite);
	m_File.SeekToEnd();

	TCHAR * ptchTemp;
	ptchTemp = (TCHAR*) malloc(1000);

	GetDateFormat(NULL, LOCALE_NOUSEROVERRIDE, NULL, NULL, ptchTemp, MAX_PATH);
	strOutput = "[";
	strOutput += ptchTemp;
	strOutput += " ";

	GetTimeFormat(NULL, LOCALE_NOUSEROVERRIDE, NULL, NULL, ptchTemp, MAX_PATH);
	strOutput += ptchTemp;

	strOutput += "] - ";
	strOutput += p_strEvent;
	strOutput += "\r\n";

	m_File.Write(strOutput, strOutput.GetLength());

	m_File.Close();

	free(ptchTemp);
}


void CLog::Init(bool p_blnLogEvents)
{
	m_blnLogEvents = p_blnLogEvents;
	if (!m_blnLogEvents) return;

	CString strFileName;
	CFileException ex;
	TCHAR * ptchTemp;
	ptchTemp = (TCHAR*) malloc(1000);

	strFileName = "IA Log ";
	GetDateFormat(NULL, LOCALE_NOUSEROVERRIDE, NULL, NULL, ptchTemp, MAX_PATH);
	strFileName += ptchTemp;
	strFileName += " ";

	GetTimeFormat(NULL, LOCALE_NOUSEROVERRIDE, NULL, NULL, ptchTemp, MAX_PATH);
	strFileName += ptchTemp;
	strFileName += ".txt";
	strFileName.Replace("/", "-");
	strFileName.Replace(":", "-");
	strFileName =  gUtils.TempPath() +  "ia.log"; //strFileName;
	mstrFilename = strFileName;

	if (! gUtils.FileExists(strFileName) )
	{
		if (!m_File.Open(strFileName, CFile::modeCreate | CFile::modeWrite, &ex))
			ex.ReportError();
		m_File.Close();
	} 
	else 
	{
		m_File.Open(strFileName, CFile::modeWrite, &ex);
		m_File.SeekToEnd();
		m_File.Write("\r\n", 2);
		m_File.Write("\r\n", 2);
		m_File.Close();
	}

	LogEvent("Application Started - logging initialized.");
	free (ptchTemp);
}
