// Button.h: interface for the CButton class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_BUTTON_H__BF3C81F1_8E99_4395_909E_29D1C2D5DCB0__INCLUDED_)
#define AFX_BUTTON_H__BF3C81F1_8E99_4395_909E_29D1C2D5DCB0__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

enum DlgButtonType {
	DlgButtonTypeRunProgram = 0, 
	DlgButtonTypeLaunchUrl,
	DlgButtonTypeCancel,
	DlgButtonTypeShellExecute
};

enum DialogActionType {
	DialogActionDoNotShowDialog = 0,
	DialogActionShowImmediately,
	DialogActionShowWhenActionComplete
};

class CDlgButton  
{
public:
	bool Load(CString sButtonId);
	CDlgButton();
	virtual ~CDlgButton();

	CString mstrId;
	bool mblnComponentCheck;
	bool mblnDefault;
	bool mblnCancel;
	DialogActionType mtypDialogAction;
	DlgButtonType mtypDlgButtonType;
	
	// Images
	CString mstrStandard;
	HBITMAP mbmpStandard;
	CString mstrMouseOver;
	HBITMAP mbmpMouseOver;
	CString mstrMouseClick;
	HBITMAP mbmpMouseClick;

	CString mstrMouseCursor;
	HCURSOR mhMouseCursor;
	
	// Dialog info
	CRect mrect;
	bool mblnMouseOver;
	bool mblnMouseClick;
	CStatic * mstatic;

	// Positioning
	int mintLeft;
	int mintTop;

	// Sounds
	CString mstrMouseEnter;
	CString mstrMouseExit;
	CString mstrMouseUp;
	CString mstrMouseDown;
	
	// TYPE: RunProgram
	CString mstrSetupCommand;
	CString mstrSetupCommandLine;
	bool mblnRestartPrompt;

	// TYPE: LaunchUrl
	CString mstrUrl;

	// TYPE: ShellExecute
	CString mstrFile;
};

#endif // !defined(AFX_BUTTON_H__BF3C81F1_8E99_4395_909E_29D1C2D5DCB0__INCLUDED_)
