#pragma once


// CDBDialog dialog

class CDBDialog : public CDialog, public IXMDBManagerCallback 
{
	DECLARE_DYNAMIC(CDBDialog)

public:
	CDBDialog(CWnd* pParent = NULL);   // standard constructor
	virtual ~CDBDialog();

// Dialog Data
	enum { IDD = IDD_DB };

	//called to indicate beging and end of scan process
	void OnBeginScan();
	void OnEndScan();
	void OnScanDir(char *path);
	void OnProcess();

	//called during scan and watch
	bool OnFileFound(char* path, BYTE* md5);
	bool OnFileRestored(CXMDBFile* file);
	bool OnFileRemoved(CXMDBFile* file);

	//called after an xmfile is either reomved or added
	void AfterFileAdded(CXMDBFile *file);
	void AfterFileRemoved(CXMDBFile *file);
	void OnFileAddError(char* path, BYTE* md5);
	void OnFileRemoveError(CXMDBFile *file);

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	//controls
	CListCtrl mList;
	CStatic mPic;
	CListBox mLog;

	//form data
	CString mDbPath, mSearchPath;
	int mSize;

	//database objects
	CXMDB *mDB;
	CXMDBManager *mDBMan;

	//misc
	void ClearDB();
	void Log(CString str);
	void DrawFiles();

	DECLARE_MESSAGE_MAP()

public:
	void OnClickedDbload(void);
	void OnClickedDbsave(void);
	//void OnItemActivateFiles(NMHDR *pNMHDR, LRESULT *pResult);
	void OnClickedScan(void);
	BOOL OnInitDialog(void);
	void OnItemchangedFiles(NMHDR *pNMHDR, LRESULT *pResult);
	//void OnClickFiles(NMHDR *pNMHDR, LRESULT *pResult);
};
