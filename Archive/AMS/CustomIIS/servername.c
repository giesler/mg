/* ServerName - ISAPI Filter to redefine the name of your webserver

A simple ISAPI filter that allows redefinition of the server name
that is reported in the HTTP headers.  Normally this identifies
the server as IIS, but by using this filter, you can select any
name for your server.  Just set

HKEY_LOCAL_MACHINE\SOFTWARE\WindowsDevelopersJournal\ServerName

to the name you want.  Given below is a sample REG file for the
settings.  This is the ONLY configuration item for this filter.

NOTE: This filter MUST be installed at Machine Level. 

REGEDIT4

[HKEY_LOCAL_MACHINE\SOFTWARE\WindowsDevelopersJournal]

[HKEY_LOCAL_MACHINE\SOFTWARE\WindowsDevelopersJournal\ServerName]
"name"="Server Name Here"

*/
#define WIN32_LEAN_AN_MEAN
#include <windows.h>
#include <httpfilt.h>
#include <stdio.h>

/* Defines */

//#define FILTER_DESCRIPTION "Server Name"
#define BUFF_MAX 64
//#define REG_NAME "SOFTWARE\\WindowsDevelopersJournal\\ServerName"

/* Private Prototypes */
DWORD HandleEvent_ReadRawData(HTTP_FILTER_CONTEXT *pFC,
    PHTTP_FILTER_RAW_DATA pReadData);
DWORD HandleEvent_SendRawData(HTTP_FILTER_CONTEXT *pFC,
    PHTTP_FILTER_RAW_DATA pSendData);
DWORD HandleEvent_EndOfNetSession(HTTP_FILTER_CONTEXT *pFC);

typedef struct {
	BOOL header;
} REQ_CONTEXT,*PREQ_CONTEXT;

char gServerName[80];



BOOL WINAPI GetFilterVersion(HTTP_FILTER_VERSION *pVersion)
/* Entry point for ISAPI filter DLL */
{

	/* Set version */
	pVersion->dwFilterVersion = HTTP_FILTER_REVISION;

	/* Provide a short description of the filter */
	lstrcpyn(pVersion->lpszFilterDesc, "Custom IIS Filter",
        SF_MAX_FILTER_DESC_LEN);

	/* Set event we want to be notified about and the
     * notification priority. SF_NOTIFY_ORDER_LOW is used so we
     * don't get in the way of ASP
     */
	pVersion->dwFlags = (SF_NOTIFY_ORDER_LOW |
						 SF_NOTIFY_READ_RAW_DATA |
                         SF_NOTIFY_SEND_RAW_DATA 
						 );

	/* Set the server name. */
	strcpy(gServerName, "AMS Server v1.0");

	return TRUE;
}

/* Entry point for ISAPI filter DLL - this function is where
 * the work is really done
 */

DWORD WINAPI HttpFilterProc(HTTP_FILTER_CONTEXT *pFC,
                DWORD dwNotificationType, VOID *pvData)
{
	DWORD dwRet = SF_STATUS_REQ_NEXT_NOTIFICATION;

	switch (dwNotificationType) {
        case SF_NOTIFY_READ_RAW_DATA:
			dwRet = HandleEvent_ReadRawData(pFC,
                        (PHTTP_FILTER_RAW_DATA)pvData);
            break;
        case SF_NOTIFY_SEND_RAW_DATA:
			dwRet = HandleEvent_SendRawData(pFC,
                        (PHTTP_FILTER_RAW_DATA)pvData);
            break;
	}

	return dwRet;
}


DWORD HandleEvent_ReadRawData(HTTP_FILTER_CONTEXT *pFC,
            PHTTP_FILTER_RAW_DATA pReadData)
{
    /*	Here we scan the body of raw request */

    DWORD           dwRet = SF_STATUS_REQ_NEXT_NOTIFICATION;
    PREQ_CONTEXT		pContext;

    /* Allocate space to save our request context information */
    pFC->pFilterContext=pFC->AllocMem(pFC, sizeof(REQ_CONTEXT), 0);

    if (pFC->pFilterContext == NULL)
        dwRet = SF_STATUS_REQ_ERROR;
    else {
        /*  Initialize our request information to be saved
         *  across notifications for this request. */
        pContext = (PREQ_CONTEXT)pFC->pFilterContext;
        pContext->header=TRUE;
        }
    return dwRet;
}

/* Parse through headers and replace old server name with new */
void ReplaceServerName(HTTP_FILTER_CONTEXT *pFC,
            PHTTP_FILTER_RAW_DATA pSendData)
{
	char *start,*end;
	long length;
	BOOL newline=TRUE;
	char token[]="SERVER:";
	char buffer[sizeof(token)+1];
	char *text;
	long orglen;

	text = pSendData->pvInData;
	start=text;
	orglen = length = pSendData->cbInData;

	while(length--)
	{
		if(newline)
		{
			memcpy(buffer,start,sizeof(token));
			buffer[sizeof(token)-1]=0;
			if(!lstrcmpi(buffer,token))
			{
				start+=sizeof(token);
				end=start;
				while( (*end!='\r') && (*end!='\n')
                        && ((DWORD)(end-text)<pSendData->cbInData))
					end++;
				
				pSendData->cbInData-=(end-start);
				pSendData->cbInData+=strlen(gServerName);
				pSendData->pvInData=(char*)pFC->AllocMem(pFC,
                        pSendData->cbInData, 0);
				memcpy(((char*)pSendData->pvInData),text,
                    start-text);
				memcpy(((char*)pSendData->pvInData)+(start-text),
                    gServerName,strlen(gServerName));
				memcpy(((char*)pSendData->pvInData)+(start-text)
                    +strlen(gServerName),end,orglen-(end-text));
			}
			newline=FALSE;
		}

		if( (*start=='\n') || (*start=='\r') )
			newline=TRUE;
		start++;

	}

}

DWORD HandleEvent_SendRawData(HTTP_FILTER_CONTEXT *pFC,
        PHTTP_FILTER_RAW_DATA pSendData)
{
    DWORD   dwRet = SF_STATUS_REQ_NEXT_NOTIFICATION;
	PREQ_CONTEXT pContext;


	pContext = (PREQ_CONTEXT)pFC->pFilterContext;

	if(pContext->header==TRUE)
		ReplaceServerName(pFC,pSendData);

	pContext->header=FALSE;
	return dwRet;
}


