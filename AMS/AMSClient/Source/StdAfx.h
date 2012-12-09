// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently
#pragma once

#define VC_EXTRALEAN		// Exclude rarely-used stuff from Windows headers

// Modify the following defines if you have to target a platform prior to the ones specified below.
#ifndef WINVER				// Allow use of features specific to Win 2K or later
//#define WINVER 0x0500
#endif

#ifndef _WIN32_WINNT		// Allow use of features specific to Win 2K or later
//#define _WIN32_WINNT 0x0500
#endif

#ifndef _WIN32_WINDOWS		// Allow use of features specific to Win 98 or later
//#define _WIN32_WINDOWS 0x0410
#endif

#ifndef _WIN32_IE			// Allow use of features specific to IE 5.01 or later
//#define _WIN32_IE 0x0501
#endif

//my defines
#define _WIN32_DCOM			//needed for CoInitEx

// turns off MFC's hiding of some common and often safely ignored warning messages
#define _AFX_ALL_WARNINGS

//mfc includes
#include <afxwin.h>         // MFC core and standard components
#include <afxext.h>         // MFC extensions
#include <afxdtctl.h>		// MFC support for Internet Explorer 4 Common Controls
#include <afxcmn.h>			// MFC support for Windows Common Controls
#include <afxtempl.h>

//import for msxml
#include <comdef.h>
#include <msxml2.h>

//winsock 2 import (mfc defaults to 1.1)
#include <winsock2.h>

//paintlib import
#include "paintlib.h"		//custom paintlib header with everything we use