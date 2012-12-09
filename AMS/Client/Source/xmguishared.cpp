// XMGUISTATUS.CPP ---------------------------------------------------- XMGUISTATUS.CPP

#include "stdafx.h"
#include "xmclient.h"
#include "xmdb.h"
#include "xmnet.h"
#include "xmpipeline.h"
#include "xmgui.h"

class CSharedFiles : public CDialog
{
public:

	//construction
	CSharedFiles(CWnd *parent)
		: CDialog(IDD_SHARED, parent)
	{
	}
	~CSharedFiles()
	{
	}

	//setting up the dialog
	BOOL OnInitDialog()
	{
		//add columns to the listview
		CListCtrl ctrl;
		ctrl.Attach(::GetDlgItem(m_hWnd, IDC_FILES));		
		ctrl.InsertColumn(0, "File", LVCFMT_LEFT, 310, 0);
		ctrl.InsertColumn(1, "Size", LVCFMT_CENTER, 65, 1);
		ctrl.InsertColumn(2, "Indexed", LVCFMT_CENTER, 65, 2);

		//turn on full row select
		ctrl.ModifyStyleEx(0, LVS_EX_FULLROWSELECT);
		ctrl.Detach();

		//fill in the list items
		BuildListview();

		//success
		return FALSE;
	}

private:

	//messages
	DECLARE_MESSAGE_MAP();

	#define MAXFILESIZE 32768
	afx_msg void OnAddFiles()
	{
		//setup the data structure
		char files[MAXFILESIZE];
		OPENFILENAME ofn;
		ZeroMemory(&ofn, sizeof(OPENFILENAME));
		ZeroMemory(files, MAXFILESIZE);
		ofn.lStructSize = sizeof(OPENFILENAME);
		ofn.hwndOwner = m_hWnd;
		ofn.lpstrFilter = "JPEG Files (*.jpg,*.jpeg)\0*.jpg;*.jpeg\0All Files (*.*)\0*.*";
		ofn.nFilterIndex = 0;
		ofn.lpstrFile = files;
		ofn.nMaxFile = MAXFILESIZE;
		ofn.lpstrTitle = "Add Files";
		ofn.Flags =
			OFN_ALLOWMULTISELECT | 
			OFN_EXPLORER |
			OFN_FILEMUSTEXIST |
			OFN_HIDEREADONLY |
			OFN_PATHMUSTEXIST;

		//show the open dialog
		if (GetOpenFileName(&ofn)!=TRUE)
			return;

		//build string array
		int scount=0;
		char *s[1024];
		ZeroMemory(s, sizeof(char*)*1024);
		char *ptr = files;
		while (*ptr != '\0')
		{
			s[scount] = ptr;
			ptr += strlen(ptr)+1;
			scount++;
		}
		if (scount < 1)
			return;

		//was there only one file selected?
		CXMDBFile *file;
		CListCtrl ctrl;
		ctrl.Attach(::GetDlgItem(m_hWnd, IDC_FILES));
		if (scount == 1)
		{
			file = db()->AddFile(s[0]);
			if (file)
			{
				//add the record to the listview
				DrawFile(ctrl, file);
			}
			else
			{
				CString str;
				str.Format("Failed to add file:\n%s", s[0]);
				AfxMessageBox(str, MB_ICONERROR);
			}
			ctrl.Detach();
			return;
		}

		//multiple files selected.. append each name to the base dir
		char f[MAX_PATH*2], *g;
		strncpy(f, s[0], MAX_PATH);
		strcat(f, "\\");
		g = f+strlen(s[0])+1;
		for(int i=1;i<scount;i++)
		{
			strncpy(g, s[i], MAX_PATH);
			file = db()->AddFile(f);
			if (file)
			{
				DrawFile(ctrl, file);
			}
			else
			{
				CString str;
				str.Format("Failed to add file:\n%s", f);
				AfxMessageBox(str, MB_ICONERROR);
			}
		}

		//success
		ctrl.Detach();
	}

	afx_msg void OnRemoveFiles()
	{
		//mark all the selected items
		CListCtrl ctrl;
		ctrl.Attach(::GetDlgItem(m_hWnd, IDC_FILES));
		POSITION pos = ctrl.GetFirstSelectedItemPosition();
		int i, j[1024], k=0;
		CXMDBFile *file;
		while (pos)
		{
			i = ctrl.GetNextSelectedItem(pos);
			file = db()->FindFile((BYTE*)ctrl.GetItemData(i), true);
			if (file)
			{
				file->SetFlag(DFF_REMOVED, true);
				file->SetFlag(DFF_KNOWN, false);
			}
			j[k] = i;
			k++;
		}

		//remove from list
		while(k>0)
		{
			ctrl.DeleteItem(j[k-1]);
			k--;
		}
		ctrl.Detach();
	}

	afx_msg void OnIndexFile()
	{
		//index the selected file
		CListCtrl *ctrl = (CListCtrl*)GetDlgItem(IDC_FILES);
		int i = ctrl->GetNextItem(-1, LVNI_SELECTED);
		if (i != -1)
		{
			db()->Lock();
			CXMDBFile *file = db()->FindFile((BYTE*)ctrl->GetItemData(i), false);
			if (file)
			{
				if (QueryBuildIndex(file))
				{
					//reset 'x' flag in list ctrl
					ctrl->SetItemText(i, 2, file->GetIndex()->data.IsBlank()?"":"x");
				}
			}
			db()->Unlock();
		}
	}
	
	afx_msg void OnDownloadIndexes()
	{
		QueryDownloadIndexes(this);
	}

	//drawing the listview
	void DrawFile(CListCtrl &ctrl, CXMDBFile *file)
	{
		//add the initial item
		LVITEMA lvi;
		lvi.mask = LVIF_PARAM|LVIF_TEXT;
		lvi.iItem = ctrl.GetItemCount();
		lvi.iSubItem = 0;
		lvi.lParam = (LPARAM)file->GetMD5();
		lvi.pszText = file->GetPath();
		ctrl.InsertItem(&lvi);

		//insert the dimensions
		char size[MAX_PATH];
		sprintf(size, "%dx%d", file->GetWidth(), file->GetHeight());
		lvi.mask = LVIF_TEXT;
		lvi.iSubItem = 1;
		lvi.lParam = NULL;
		lvi.pszText = size;
		ctrl.SetItem(&lvi);

		//insert the indexed flag
		strcpy(size, file->GetIndex()->data.IsBlank()?"":"x");
		lvi.mask = LVIF_TEXT;
		lvi.iSubItem = 2;
		lvi.lParam = NULL;
		lvi.pszText = size;
		ctrl.SetItem(&lvi);

	}
	void BuildListview()
	{
		//get the cotnrol
		CListCtrl ctrl;
		ctrl.Attach(::GetDlgItem(m_hWnd, IDC_FILES));

		//clear everything
		ctrl.DeleteAllItems();

		//loop through the database
		db()->Lock();
		CXMDBFile *file;
		for (DWORD i=0;i<db()->GetFileCount();i++)
		{
			//skip removed files
			file = db()->GetFile(i);
			if (file->GetFlag(DFF_REMOVED))
				continue;

			//add the listview
			DrawFile(ctrl, file);
		}
		db()->Unlock();
		ctrl.Detach();
	}

};

BEGIN_MESSAGE_MAP(CSharedFiles, CDialog)

	//buttons
	ON_BN_CLICKED(IDC_ADD, OnAddFiles)
	ON_BN_CLICKED(IDC_REMOVE, OnRemoveFiles)
	ON_BN_CLICKED(IDC_INDEX, OnIndexFile)
	ON_BN_CLICKED(IDC_DOWNLOAD, OnDownloadIndexes)

END_MESSAGE_MAP()

void DoSharedFiles(CWnd *parent)
{
	//just show the dialog
	CSharedFiles dlg(parent);
	dlg.DoModal();
}