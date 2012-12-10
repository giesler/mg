// DlgButton.cpp: implementation of the CDlgButton class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "autorun.h"		// for gLog
#include "Button.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CDlgButton::CDlgButton()
{

}

CDlgButton::~CDlgButton()
{

}

bool CDlgButton::Load(CString sButtonId, CString sFileName)
{

	// Initialize variables used to retreive settings
	CString sTemp; int iTemp;
	TCHAR * lpReturnedString;
	DWORD nSize = 1500;
	lpReturnedString = (TCHAR*) malloc(nSize);

	// Save the ID passed in
	mstrId = sButtonId;

	// Get basic settings from INI file
	iTemp				= GetPrivateProfileInt(sButtonId, "ComponentCheck", 0, sFileName);
	mblnComponentCheck	= (iTemp == 1);
	iTemp				= GetPrivateProfileInt(sButtonId, "Default", 0, sFileName);
	mblnDefault			= (iTemp == 1);
	iTemp				= GetPrivateProfileInt(sButtonId, "Cancel", 0, sFileName);
	mblnCancel			= (iTemp == 1);
	mtypDialogAction	= (DialogActionType) GetPrivateProfileInt(sButtonId, "DialogAction", 0, sFileName);
	
	// Load image names
	GetPrivateProfileString(sButtonId, "Standard", "", lpReturnedString, nSize, sFileName);
	mstrStandard	= lpReturnedString;
	GetPrivateProfileString(sButtonId, "MouseOver", "", lpReturnedString, nSize, sFileName);
	mstrMouseOver	= lpReturnedString;
	GetPrivateProfileString(sButtonId, "MouseClick", "", lpReturnedString, nSize, sFileName);
	mstrMouseClick	= lpReturnedString;

	/// Load image positions
	mintTop		= GetPrivateProfileInt(sButtonId, "Top", 0, sFileName);
	mintLeft	= GetPrivateProfileInt(sButtonId, "Left", 0, sFileName);

	// Load button sounds
	GetPrivateProfileString(sButtonId, "MouseEnter", "", lpReturnedString, nSize, sFileName);
	mstrMouseEnter	= lpReturnedString;
	GetPrivateProfileString(sButtonId, "MouseExit", "", lpReturnedString, nSize, sFileName);
	mstrMouseExit	= lpReturnedString;
	GetPrivateProfileString(sButtonId, "MouseUp", "", lpReturnedString, nSize, sFileName);
	mstrMouseUp		= lpReturnedString;
	GetPrivateProfileString(sButtonId, "MouseDown", "", lpReturnedString, nSize, sFileName);
	mstrMouseDown	= lpReturnedString;

	// Get button type
	mtypDlgButtonType = (DlgButtonType) GetPrivateProfileInt(sButtonId, "ButtonType", 0, sFileName);

	// Load settings for the type of button
	switch (mtypDlgButtonType) {
		case DlgButtonTypeRunProgram:
			GetPrivateProfileString(sButtonId, "SetupCommand", "", lpReturnedString, nSize, sFileName);
			mstrSetupCommand = lpReturnedString;
			GetPrivateProfileString(sButtonId, "SetupCommandLine", "", lpReturnedString, nSize, sFileName);
			mstrSetupCommandLine = lpReturnedString;
			iTemp = GetPrivateProfileInt(sButtonId, "RestartPrompt", 0, sFileName);
			mblnRestartPrompt = (iTemp == 1);
			break;
		case DlgButtonTypeLaunchUrl:
			GetPrivateProfileString(sButtonId, "URL", "", lpReturnedString, nSize, sFileName);
			mstrUrl = lpReturnedString;
			break;
		case DlgButtonTypeCancel:
			break;
		case DlgButtonTypeShellExecute:
			GetPrivateProfileString(sButtonId, "File", "", lpReturnedString, nSize, sFileName);
			mstrFile = lpReturnedString;
			break;
		default:
			gLog.LogEvent(mstrId + ": Invalid/no button type specified, ignoring button.");
			free(lpReturnedString);
			return false;
	}

	// Load the images into memory
	mbmpStandard	= (HBITMAP) ::LoadImage(NULL, gUtils.EXEPath() + mstrStandard, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
	mbmpMouseOver	= (HBITMAP) ::LoadImage(NULL, gUtils.EXEPath() + mstrMouseOver, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
	mbmpMouseClick	= (HBITMAP) ::LoadImage(NULL, gUtils.EXEPath() + mstrMouseClick, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);

	//TODO: mbmpStandard.GetWindowRect(&mrect);

	mblnMouseOver  = false;
	mblnMouseClick = false;

	// Cleanup variables
	free(lpReturnedString);
	return true;
}
