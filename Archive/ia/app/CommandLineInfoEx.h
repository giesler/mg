// CommandLineInfoEx.h: interface for the CCommandLineInfoEx class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_COMMANDLINEINFOEX_H__EEF4D517_976F_4A69_9195_51F0AE62B92F__INCLUDED_)
#define AFX_COMMANDLINEINFOEX_H__EEF4D517_976F_4A69_9195_51F0AE62B92F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

//////////////////
// Improved CCommandLineInfo parses arbitrary switches.
// Use instead of CCommandLineInfo:
//
//    CCommandLineInfoEx cmdinfo;
//    ParseCommandLine(cmdinfo); // (from app object)
//
// After parsing, you can call GetOption to get the value of any switch. Eg:
//
//    if (cmdinfo.GetOption("nologo")) {
//       // handle it
//    }
//
// to get the value of a string option, type
//
//    CString filename;
//    if (cmdinfo.GetOption("f")) {
//       // now filename is string following -f option
//    }
//
class CCommandLineInfoEx : public CCommandLineInfo {
public:
   bool GetOption(LPCTSTR option, CString& val);
   bool GetOption(LPCTSTR option) {
      return GetOption(option, CString());
   }

protected:
   CMapStringToString m_options; // hash of options
   CString  m_sLastOption;       // last option encountered
   virtual void ParseParam(const TCHAR* pszParam, BOOL bFlag, BOOL bLast);
};


#endif // !defined(AFX_COMMANDLINEINFOEX_H__EEF4D517_976F_4A69_9195_51F0AE62B92F__INCLUDED_)
