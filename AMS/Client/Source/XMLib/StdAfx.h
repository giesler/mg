#pragma once

#define WIN32_LEAN_AND_MEAN

#define _WIN32_DCOM			//needed for CoInitEx

#ifdef _INTERNAL
#include <afxwin.h>
#include <afxtempl.h>
#endif


#include <windows.h>
#include <stdio.h>

//import for msxml
#include <comdef.h>
#include <msxml2.h>

//winsock 2 import (mfc defaults to 1.1)
#include <winsock2.h>

//paintlib import
#include "paintlib.h"