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

bool CDlgButton::Load(CString sButtonId)
{

	// Save the ID passed in
	mstrId = sButtonId;

	// Get basic settings from INI file
	mblnComponentCheck	= gUtils.GetINIBool(sButtonId, "ComponentCheck", 0);
	mblnDefault			= gUtils.GetINIBool(sButtonId, "Default", 0);
	mblnCancel			= gUtils.GetINIBool(sButtonId, "Cancel", 0);
	mtypDialogAction	= (DialogActionType) gUtils.GetINIInt(sButtonId, "DialogAction", 0);

	// Load image names
	mstrStandard		= gUtils.GetINIString(sButtonId, "Standard", "");
	mstrMouseOver		= gUtils.GetINIString(sButtonId, "MouseOver", "");
	mstrMouseClick		= gUtils.GetINIString(sButtonId, "MouseClick", "");

	/// Load image positions
	mintTop				= gUtils.GetINIInt(sButtonId, "Top", 0);
	mintLeft			= gUtils.GetINIInt(sButtonId, "Left", 0);

	// Load button sounds
	mstrMouseEnter		= gUtils.GetINIString(sButtonId, "MouseEnter", "");
	mstrMouseExit		= gUtils.GetINIString(sButtonId, "MouseExit", "");
	mstrMouseUp			= gUtils.GetINIString(sButtonId, "MouseUp", "");
	mstrMouseDown		= gUtils.GetINIString(sButtonId, "MouseDown", "");

	// Get button type
	mtypDlgButtonType	= (DlgButtonType) gUtils.GetINIInt(sButtonId, "ButtonType", 0);

	// Load settings for the type of button
	switch (mtypDlgButtonType) {
		case DlgButtonTypeRunProgram:
			mstrSetupCommand		= gUtils.GetINIString(sButtonId, "SetupCommand", "");
			mstrSetupCommandLine	= gUtils.GetINIString(sButtonId, "SetupCommandLine", "");
			mblnRestartPrompt		= gUtils.GetINIBool(sButtonId, "RestartPrompt", false);
			break;
		case DlgButtonTypeLaunchUrl:
			mstrUrl					= gUtils.GetINIString(sButtonId, "URL", "");
			break;
		case DlgButtonTypeCancel:
			break;
		case DlgButtonTypeShellExecute:
			mstrFile				= gUtils.GetINIString(sButtonId, "File", "");
			break;
		default:
			gLog.LogEvent(mstrId + ": Invalid/no button type specified, ignoring button.");
			return false;
	}

	// Load the images into memory
	mbmpStandard	= (HBITMAP) ::LoadImage(NULL, mstrStandard, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
	mbmpMouseOver	= (HBITMAP) ::LoadImage(NULL, mstrMouseOver, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
	mbmpMouseClick	= (HBITMAP) ::LoadImage(NULL, mstrMouseClick, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);

	// Load the cursor if we can
	mhMouseCursor = NULL;
	mstrMouseCursor = gUtils.GetINIString(sButtonId, "MouseCursor", "");
	if (mstrMouseCursor != "") {
		HANDLE hCursor = LoadImage(NULL, mstrMouseCursor, IMAGE_CURSOR, 0, 0, LR_LOADFROMFILE);
		if (hCursor != NULL)
			mhMouseCursor = (HCURSOR) hCursor;
	}

	mblnMouseOver  = false;
	mblnMouseClick = false;

	// Cleanup variables
	return true;
}
