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
	m_File.Close();
}



void CLog::LogEvent(CString p_strEvent)
{
	if (!m_blnLogEvents) return;

	CString strOutput;

	TCHAR * ptchTemp;
	ptchTemp = (TCHAR*) malloc(1000);

	GetDateFormat(NULL, LOCALE_NOUSEROVERRIDE, NULL, NULL, ptchTemp, MAX_PATH);
	strOutput += ptchTemp;
	strOutput += " ";

	GetTimeFormat(NULL, LOCALE_NOUSEROVERRIDE, NULL, NULL, ptchTemp, MAX_PATH);
	strOutput += ptchTemp;

	strOutput += " - ";
	strOutput += p_strEvent;
	strOutput += "\r\n";

	m_File.Write(strOutput, strOutput.GetLength());

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

	strFileName = "VBSW Log ";
	GetDateFormat(NULL, LOCALE_NOUSEROVERRIDE, NULL, NULL, ptchTemp, MAX_PATH);
	strFileName += ptchTemp;
	strFileName += " ";

	GetTimeFormat(NULL, LOCALE_NOUSEROVERRIDE, NULL, NULL, ptchTemp, MAX_PATH);
	strFileName += ptchTemp;
	strFileName += ".txt";
	strFileName.Replace("/", "-");
	strFileName.Replace(":", "-");
	strFileName =  gUtils.TempPath() + strFileName;

	if (!m_File.Open(strFileName, CFile::modeCreate | CFile::modeWrite, &ex))
		ex.ReportError();

	LogEvent("Application Started - logging initialized.");
	free (ptchTemp);
}
