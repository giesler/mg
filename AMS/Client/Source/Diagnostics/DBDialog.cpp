// DBDialog.cpp : implementation file
//

#include "stdafx.h"
#include "db.h"
#include "paintlib.h"
#include "XMClient.h"
#include "DBDialog.h"

// CDBDialog dialog

IMPLEMENT_DYNAMIC(CDBDialog, CDialog)
CDBDialog::CDBDialog(CWnd* pParent /*=NULL*/)
	: CDialog(CDBDialog::IDD, pParent)
{
	mDB = NULL;
	mDBMan = NULL;

	//initial values
	mDbPath = "c:\\thumbs.db";
	mSearchPath = "c:\\winnt";
}

CDBDialog::~CDBDialog()
{
	ClearDB();
}

void CDBDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_FILES, mList);
	DDX_Control(pDX, IDC_PIC, mPic);
	DDX_Control(pDX, IDC_LOG, mLog);
	DDX_Text(pDX, IDC_DBPATH, mDbPath);
	DDX_Text(pDX, IDC_PATH2, mSearchPath);
	DDX_Text(pDX, IDC_PICSIZE, mSize);
}

void CDBDialog::OnClickedDbload(void)
{
	//close database if open
	ClearDB();

	//create new objects
	mDB = new CXMDB();
	mDBMan = new CXMDBManager();

	//setup manager
	mDBMan->SetCallback((IXMDBManagerCallback*)this);
	mDBMan->SetDatabase(mDB);

	//try to load database
	UpdateData(TRUE);
	mDB->SetPath(mDbPath);
    if (mDB->Open())
		Log("Database loaded.");
	else {

		//try new database
		Log("FAILED: Load database.");
		if (mDB->New()) 
			Log("New database created.");
		else
			Log("FAILED: Create new database.");
	}

	//draw new files
	DrawFiles();
}

void CDBDialog::OnClickedDbsave(void)
{
	//save the database to the given path
	if (mDB) {
		UpdateData(TRUE);
		if (mDB->Flush())
			Log("Database saved.");
		else
			Log("FAILED: Saving database.");
	}
}

//void CDBDialog::OnItemActivateFiles(NMHDR *pNMHDR, LRESULT *pResult)
//{
//	LPNMITEMACTIVATE pNMIA = reinterpret_cast<LPNMITEMACTIVATE>(pNMHDR);
//	Log("Activiating.");
//	*pResult = 0;
//}

void CDBDialog::OnClickedScan(void)
{
	if (mDB) {
		UpdateData(TRUE);
		mDB->SetSearchPath((const BYTE*)mSearchPath.GetBuffer(0));
		mSearchPath.ReleaseBuffer();
		if (mDBMan->ScanDirectory((char*)mDB->GetSearchPath()))
			Log("Directory scanned.");
		else
			Log("FAILED: Scan directory.");
		DrawFiles();
	}
}

void CDBDialog::ClearDB()
{
	//delete db objects
	if (mDBMan)
		delete mDBMan;
	if (mDB)
		delete mDB;

	//clear ui
	HBITMAP hbmp;
	if (m_hWnd) {
		if (mList.m_hWnd) {
			mList.DeleteAllItems();
			if (mPic.m_hWnd) {
				hbmp = mPic.SetBitmap(NULL);
				if (hbmp) {
					::DeleteObject(hbmp);
				}
			}
		}
	}
}

BEGIN_MESSAGE_MAP(CDBDialog, CDialog)
	ON_BN_CLICKED(IDC_DBLOAD, OnClickedDbload)
	ON_BN_CLICKED(IDC_DBSAVE, OnClickedDbsave)
	//ON_NOTIFY(LVN_ITEMACTIVATE, IDC_FILES, OnItemActivateFiles)
	ON_BN_CLICKED(IDC_SCAN, OnClickedScan)
	ON_NOTIFY(LVN_ITEMCHANGED, IDC_FILES, OnItemchangedFiles)
	//ON_NOTIFY(NM_CLICK, IDC_FILES, OnClickFiles)
END_MESSAGE_MAP()

void CDBDialog::Log(CString str)
{
	//append to log list box
	mLog.SetCurSel(mLog.InsertString(mLog.GetCount(), str));
}

//called to indicate beging and end of scan process
void CDBDialog::OnBeginScan()
{
	Log("Beginning scan...");
}

void CDBDialog::OnEndScan()
{
	Log("Ending scan.");
}

void CDBDialog::OnScanDir(char *path)
{
	CString str;
	str.Format("Scanning path: %s", path);
	Log(str);
}

void CDBDialog::OnProcess()
{
}


//called during scan and watch
bool CDBDialog::OnFileFound(char* path, BYTE* md5)
{
	CString str;
	str.Format("Found New File: %s", path);
	Log(str);
	return true;
}

bool CDBDialog::OnFileRestored(CXMDBFile* file)
{
	CString str;
	str.Format("Found Restored File: %s", file->GetPath());
	Log(str);
	return true;
}

bool CDBDialog::OnFileRemoved(CXMDBFile* file)
{
	CString str;
	str.Format("Found File Removed: %s", file->GetPath());
	Log(str);
	return true;
}


//called after an xmfile is either reomved or added
void CDBDialog::AfterFileAdded(CXMDBFile *file)
{
	CString str;
	str.Format("File Added: %s", file->GetPath());
	Log(str);
}

void CDBDialog::AfterFileRemoved(CXMDBFile *file)
{
	CString str;
	str.Format("File Removed: %s", file->GetPath());
	Log(str);
}

void CDBDialog::OnFileAddError(char* path, BYTE* md5)
{
	CString str;
	str.Format("Error Adding File: %s", path);
	Log(str);
}

void CDBDialog::OnFileRemoveError(CXMDBFile *file)
{
	CString str;
	str.Format("Error Removing File: %s", file->GetPath());
	Log(str);
}

BOOL CDBDialog::OnInitDialog(void)
{
	CDialog::OnInitDialog();

	//fill combo box
	HWND hwnd;
	CComboBox box;
	GetDlgItem(IDC_PICSIZE, &hwnd);
	box.Attach(hwnd);
	box.AddString("50x50");
	box.AddString("100x100");
	box.AddString("150x150");
	box.SetCurSel(0);
	box.Detach();

	//setup listview
	RECT r;
	mList.GetClientRect(&r);
	mList.InsertColumn(0, "Path", LVCFMT_LEFT, r.right);

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CDBDialog::DrawFiles()
{
	//clear current list
	mList.DeleteAllItems();

	//start drawing files
	DWORD i;
	int item;
	CXMDBFile *file;
	for (i=0;i<mDB->GetFileCount();i++)
	{
		file = mDB->GetFile(i);
		if (file) { 
			item = mList.InsertItem(mList.GetItemCount(), file->GetPath());
			mList.SetItemData(item, i);
		}
		else
			Log("FAILED: CXMDB::GetFile() returned NULL!");
	}
}

void CDBDialog::OnItemchangedFiles(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMLISTVIEW pNMLV = reinterpret_cast<LPNMLISTVIEW>(pNMHDR);
	if (pNMLV->uNewState==3) {

		//get file pointer
		CXMDBFile *file;
		file = mDB->GetFile(mList.GetItemData(pNMLV->iItem));

		//find thumbnail
		UpdateData(TRUE);
		DWORD s = mSize;//(mSize+1)*50;
		CXMDBThumb *thumb;
		thumb = file->GetThumb(s, s);

		//if valid, display
		if (!thumb) {
			
			//failed
			Log("FAILED: CXMDBFile::GetThumb().");
		}
		else  {

			//decode jpeg
			BYTE *buf;
			DWORD size;
			CDIBSection bmp;
			CJPEGDecoder jpeg;
			size = thumb->GetImage(&buf);
			jpeg.MakeBmpFromMemory(buf, size, &bmp);
			thumb->FreeImage(&buf);

			//assign picture
			
			HBITMAP h = (HBITMAP)bmp.GetHandle();
			bmp.Detach();
			mPic.SetBitmap(h);
			if (h) {
				::DeleteObject(h);
			}
		}
	}
	*pResult = 0;
}












