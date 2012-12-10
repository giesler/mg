// UpdateWarning.cpp : implementation file
//

#include "stdafx.h"
#include "autorun.h"
#include "UpdateWarning.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CUpdateWarning dialog


CUpdateWarning::CUpdateWarning(CWnd* pParent /*=NULL*/)
	: CDialog(CUpdateWarning::IDD, pParent)
{
	//{{AFX_DATA_INIT(CUpdateWarning)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}


void CUpdateWarning::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CUpdateWarning)
		// NOTE: the ClassWizard will add DDX and DDV calls here
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CUpdateWarning, CDialog)
	//{{AFX_MSG_MAP(CUpdateWarning)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CUpdateWarning message handlers
