// UpdateWarning.cpp : implementation file
//

#include "stdafx.h"
#include "autorun.h"
#include "UpdateWarning.h"
#include "component.h"

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
	m_Message = _T("");
	//}}AFX_DATA_INIT

	m_Message.LoadString(IDS_UPDATEWARNING);

}


void CUpdateWarning::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CUpdateWarning)
	DDX_Control(pDX, IDOK, m_OKButton);
	DDX_Control(pDX, IDCANCEL, m_CancelButton);
	DDX_Control(pDX, IDC_COMPONENTLIST, m_ComponentList);
	DDX_Text(pDX, IDC_MESSAGE, m_Message);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CUpdateWarning, CDialog)
	//{{AFX_MSG_MAP(CUpdateWarning)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CUpdateWarning message handlers


void CUpdateWarning::SetComponents(CList<CComponent*, CComponent*> &lstComps)
{
	// loop through the list, adding to list
	POSITION pos; int i;
	CComponent * pobjComp;
	
	// Count total time
	pos = lstComps.GetHeadPosition();
	for (i = 0; i < lstComps.GetCount(); i++) 
	{
		pobjComp = lstComps.GetNext(pos);
		mlstComps.AddTail(pobjComp);
	}
}

BOOL CUpdateWarning::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	// Set text
	CString s;
	s.LoadString(IDS_OK);
	m_OKButton.SetWindowText(s);
	s.LoadString(IDS_CANCEL);
	m_CancelButton.SetWindowText(s);

	s.LoadString(IDS_DEFAULTPROGRAMNAME);
	s = gUtils.GetINIString("Settings", "ProgramName", s);
	SetWindowText(s);

	// Set full row select
	m_ComponentList.SetExtendedStyle(LVS_EX_FULLROWSELECT);

	// Add column to listview
	CRect rect;
	m_ComponentList.GetClientRect(&rect);
	m_ComponentList.InsertColumn(0, "Component", LVCFMT_LEFT, rect.Width()-GetSystemMetrics(SM_CXVSCROLL));

	// Create image lists for the restart icons
	HIMAGELIST hList = ImageList_Create(16, 16, ILC_COLOR8 | ILC_MASK, 8, 1);
	m_ImageList.Attach(hList);

	// add restart icon
	m_ImageList.Add(AfxGetApp()->LoadIcon(IDI_RESTARTICON));
	m_ImageList.Add(AfxGetApp()->LoadIcon(IDI_BLANKICON));

	// Tie image list to list view
	m_ComponentList.SetImageList(&m_ImageList, LVSIL_SMALL);

	// loop through the list, adding to list
	POSITION pos; int i; int index = 0;
	CComponent * pobjComp;

	// Reset 'listed' on all components
	pos = mlstComps.GetHeadPosition();
	for (i = 0; i < mlstComps.GetCount(); i++) 
	{
		pobjComp = mlstComps.GetNext(pos);
		pobjComp->mblnListed = false;
	}

	// Loop through list until everything is added
	bool allAdded = false;
	int loopDetect = 1000;
	while (!allAdded)
	{
		// Loop through the list
		int lastBatchItem = -1;
		CString lastBatchItemName = "";
		pos = mlstComps.GetHeadPosition();
		for (i = 0; i < mlstComps.GetCount(); i++) 
		{
			pobjComp = mlstComps.GetNext(pos);

			// Check if we already have taken care of this item
			if (!pobjComp->mblnListed && pobjComp->mblnInstall)
			{
				LVITEM lvi;
				lvi.mask = LVIF_IMAGE | LVIF_TEXT;

				// Now check if any depends need to be installed first
				if (DependsListed(pobjComp))
				{
					// if an immediate reboot, then we don't want to continue with current loop
					if (pobjComp->mtypReboot == ImmediateReboot)
					{
						lvi.iImage = 0;
						lvi.iItem = index;
						lvi.iSubItem = 0;
						lvi.pszText		= (LPTSTR) (LPCTSTR) pobjComp->mstrName;
						m_ComponentList.InsertItem(&lvi);
						pobjComp->mblnListed = true;
						RemoveIncludes(pobjComp);
						index++;
						break;		// start over in list
					}

					// we want to add this item
					lvi.iImage = 1;
					lvi.iItem = index;
					lvi.iSubItem = 0;
					lvi.pszText		= (LPTSTR) (LPCTSTR) pobjComp->mstrName;
					m_ComponentList.InsertItem(&lvi);
					RemoveIncludes(pobjComp);

					// We may need to add a reboot icon later
					if (pobjComp->mtypReboot == BatchReboot)
					{
						lastBatchItem = index;
						lastBatchItemName = pobjComp->mstrName;
					}
					index++;

					pobjComp->mblnListed = true;

				}		// depends installed

			} // listed

		}	// end for loop, look at next component

		// Check if the last added item needs to have a reboot icon
		if (lastBatchItem != -1)
		{
			LVITEM lvUpdate;
			lvUpdate.iItem = lastBatchItem;
			lvUpdate.iSubItem = 0;
			lvUpdate.mask = LVIF_IMAGE | LVIF_TEXT;
			lvUpdate.pszText = (LPTSTR) (LPCTSTR) lastBatchItemName;
			m_ComponentList.GetItem(&lvUpdate);
			lvUpdate.iImage = 0;
			m_ComponentList.SetItem(&lvUpdate);
		}

		
		// Check if we added everything yet
		allAdded = true;
		pos = mlstComps.GetHeadPosition();
		for (i = 0; i < mlstComps.GetCount(); i++) 
		{
			pobjComp = mlstComps.GetNext(pos);
			if (!pobjComp->mblnListed && pobjComp->mblnInstall)
				allAdded = false;
		}

		// Make sure not in an infinite loop
		loopDetect--;
		if (loopDetect == 0)
		{
			AfxMessageBox("Setup has detected a loop in the component list.  The list of components that need to be installed may be slightly incorrect, however, you may continue.");
			allAdded = true;
		}
	}

	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

bool CUpdateWarning::DependsListed(CComponent *pobjComp)
{

	POSITION lstPos; CString strDependId; CComponent * pobjTemp;

	// go through the depends for object pobjComp
	POSITION pos = pobjComp->mlstDepends.GetHeadPosition();
	for (int i = 0; i < pobjComp->mlstDepends.GetCount(); i++) 
	{
		strDependId = pobjComp->mlstDepends.GetNext(pos);

		// look through list to see if any depends are in list of to install stuff
 		lstPos = mlstComps.GetHeadPosition();
		for (int j = 0; j < mlstComps.GetCount(); j++) 
		{
			pobjTemp = mlstComps.GetNext(lstPos);
			if (pobjTemp->mblnInstall && pobjTemp->mstrId.CompareNoCase(strDependId) == 0
				&& !pobjTemp->mblnListed) 
			{
				// we have a depend in the list that is not installed
				return false;
			}
		}	// end looping through install list
	}		// end looping through depends list

	return true;	// all depends are installed
}

void CUpdateWarning::RemoveIncludes(CComponent *pobjComp)
{
	POSITION lstPos; CString strIncludeId; CComponent * pobjTemp;

	// go through the includes for object pobjComp
	POSITION pos = pobjComp->mlstIncludes.GetHeadPosition();
	for (int i = 0; i < pobjComp->mlstIncludes.GetCount(); i++) 
	{
		strIncludeId = pobjComp->mlstIncludes.GetNext(pos);

		// look through list to see if this comp is already included by another component
 		lstPos = mlstComps.GetHeadPosition();
		for (int j = 0; j < mlstComps.GetCount(); j++) 
		{
			pobjTemp = mlstComps.GetNext(lstPos);
			if (pobjTemp->mblnInstall && pobjTemp->mstrId.CompareNoCase(strIncludeId) == 0)
			{
				// we have an include in the list that is already handled
				pobjTemp->mblnListed = true;
			}
		}	// end looping through install list
	}		// end looping through include list


}
