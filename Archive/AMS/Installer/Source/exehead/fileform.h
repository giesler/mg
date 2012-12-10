#include "config.h"

#ifndef _FILEFORM_H_
#define _FILEFORM_H_


// stored in file:
// exehead (around 34k)
// firstheader (as of 4/17/01, 32 bytes)
// datablock (at least 512 bytes if CRC enabled)
// CRC (optional - 4 bytes)

// stored in datablock, at firstheader.header_ptr:
// (if install)
//   header
//   sections
//   entries
//   string table
// (if uninstall)
//   uninstall_header
//   entries
//   string table


#define MAX_ENTRY_OFFSETS 5


// maximum install types. note that this should not exceed 31, ever.
#define MAX_INST_TYPES 8


enum
{
  EW_INVALID_OPCODE,    // zero is invalid. useful for catching errors. (otherwise an all zeroes instruction does nothing, which is
                        // easily ignored.
  EW_NOP,               // Nop, do nothing: 1, [advance amount]
  EW_UPDATETEXT,        // Update status text: 1 [update str]
  EW_SETOUTPUTDIR,      // Set output dir: 1,[new directory]
  EW_CREATEDIR,         // 
  EW_EXTRACTFILE,       // File to extract: 4,[overwriteflag, output filename, compressed filedata, filedatetimehigh, filedatetimelow]
                        //  overwriteflag: 0x1 = no. 0x0=force, 0x2=try, 0x3=if date is newer
  EW_EXECUTE,           // Execute program: 2,[complete command line,waitflag]
  EW_SHELLEXEC,         // ShellExecute program: 4, [shell action, complete commandline, parameters, showwindow]
  EW_REGISTERDLL,       // Register DLL: 3,[DLL file name, string ptr of function to call, text to put in display]
  EW_INSTNETSCAPE,      // Install Netscape Plug-in: 5, [DLL file name without path, isuninstall, compressed filedata, filedatetimehigh, filedatetimelow]
  EW_WRITEREG,          // Write Registry value: 5, [RootKey(int),KeyName,ItemName,ItemData,typelen]
                        //  typelen=1 for str, 2 for dword, 3 for binary
  EW_DELREG,            // DeleteRegValue/DeleteRegKey: 4, [root key(int), KeyName, ValueName, delkeyonlyifempty]. ValueName is -1 if delete key
  EW_WRITEINI,          // Write INI String: 4, [Section, Name, Value, INI File]
  EW_CREATESHORTCUT,    // Make Shortcut: 5, [link file, target file, parameters, icon file, iconindex|show mode<<8|hotkey<<16]
  EW_DELETEFILE,        // Delete File: 2, [filename, rebootok]
  EW_FINDWINDOW,        //  FindWindow: 5, [whattodo,window class,string for whattodo, use_title, jumpamount]
                        //  whattodo: 0: close window, and wait. 1: close window, and resume. 
                        //  2: prompt user to close window, and display text in parm 3, 3=jump jumpamount
  EW_MESSAGEBOX,        // MessageBox: 4,[MB_flags,text,retv,moveonretv]
  EW_RMDIR,             // RMDir: 2 [path, recursiveflag]
  EW_COPYFILES,         // CopyFiles: 2 [source mask, destination location]
  EW_SLEEP,             // Sleep: 1 [sleep time in milliseconds]
  EW_BRINGTOFRONT,      // BringToFront: 0
  EW_HIDEWINDOW,        // HideWindow: 0
  EW_IFFILEEXISTS,      // IfFileExists: 3, [file name, jump amount if exists, jump amount if not exists]
  EW_RENAME,            // Rename: 3 [old, new, rebootok]
  EW_SETFILEATTRIBUTES, // SetFileAttributes: 2 [filename, attributes]

  EW_ABORT,             // Abort: 1 [status]
  EW_CHDETAILSVIEW,     // SetDetailsView: 1 [0: hide, 1: show]
  EW_SETWINDOWCLOSE,    // SetWindowClose: 1 [0: no window close at end, 1: window close at end]

  EW_IFERRORS,           // IfErrors: 3 [jump if error, jump if not error, new_erflag]

  EW_ASSIGNVAR,         // Assign: 3 [variable (0-9) to assign, string to assign, maxlen]
  EW_READREGSTR,        // ReadRegStr: 4 [output, rootkey(int), keyname, itemname]
  EW_READINISTR,        // ReadINIStr: 4 [output, section, name, ini_file]
  EW_STRCMP,            // StrCmp: 4 [str1, str2, jump_if_equal, jump_if_not_equal] (case-insensitive)
  EW_GETPARENT,         // GetParentDir 2 [output, input]

  EW_CALL,              // Call: 2 [start offset, end offset]

  EW_GETFULLDLLPATH,    // GetFullDllPath: 2 [output, dll name]
  EW_COMPAREDLLS,       // CompareDLLVersions 5 [dll1, dll2, jumpif1newer, jumpif2newer, [ver_low]]
 // if dll2&0x80000000 then dll1 is the low dword of version, [4] is the high dword
  EW_COMPAREFILETIMES,  // CompareFileTImes 5 [file1, file2, jumpif1newer, jumpif2newer, [time_low]]
 // if file2&0x80000000 then file1 is the low dword of time, [4] is the high dword

  // mpg mod start
  EW_MUTEXWAIT,			// wait for an EXE to finish execution - will look for process based on name passed
  EW_OPENSTATUS,		// open a status dialog, set caption to [1], msg to [2]
  EW_UPDATESTATUS,		// update status dialog with [1]
  EW_CLOSESTATUS		// close status dialog
  // mpg mod end

};

typedef struct
{
  int flags; // &1=CRC, &2=uninstall, &4=silent 
  int siginfo;  // FH_SIG

  int nsinst[3]; // FH_INT1,FH_INT2,FH_INT3 (That's "NullSoftInst")

  // these point to the header+sections+entries+stringtable in the datablock
  int length_of_header;
  int header_ptr;

  // this specifies the length of all the data (including the firstheader and CRC)
  int length_of_all_following_data;
} firstheader;


typedef struct
{
  // all these _ptr ones are in the string table.
  int name_ptr; // name of installer
  int caption_ptr; // name of installer + " Setup" or whatever.
  int text_ptr; // directory page text
  int componenttext_ptr; // component page text
  int licensetext_ptr; // license page text
  int licensedata_ptr; // license text
  int install_directory_ptr; // default install dir.
  
  int install_reg_rootkey, install_reg_key_ptr, install_reg_value_ptr; 

  int install_types_ptr[MAX_INST_TYPES]; // -1 if not used. can describe as lite, normal, full, etc.
  int no_custom_instmode_flag;

  int num_sections; // total number of sections

  int num_entries; // total number of entries

#ifdef NSIS_CONFIG_UNINSTALL_SUPPORT
  int uninstall_exe_name_ptr; // names executable for uninstaller
  int uninstdata_offset; // -1 if no uninst data.
  int uninstexehead_iconoffset; // offset in exe head that the icon is in (for replacing)
#endif

#ifdef NSIS_SUPPORT_BGBG 
  int bg_color1, bg_color2, bg_textcolor;
#endif
  int lb_bg, lb_fg;

  // .on* calls
  int code_onInit[2];
  int code_onInstSuccess[2];
  int code_onInstFailed[2];
  int code_onUserAbort[2];
  int code_onVerifyInstDir[2];

  // additional flags
  char silent_install, auto_close, show_details, no_show_dirpage;

} header;

typedef struct 
{
  int name_ptr;
  int uninstalltext_ptr;

  int num_entries; // entries used

  int code[2];
#ifdef NSIS_SUPPORT_BGBG 
  int bg_color1, bg_color2, bg_textcolor;
#endif
  int lb_bg, lb_fg;
} uninstall_header;

typedef struct 
{
  int name_ptr; // '' for non-optional components
  int default_state; // bits 0-3 set for each of the different install_types, if any.
  int code[2];
  int size_kb;
} section;


typedef struct 
{
  int which;
  int offsets[MAX_ENTRY_OFFSETS];	// count and meaning of offsets depend on 'which'
} entry;


#define FH_FLAGS_MASK 7
#define FH_FLAGS_CRC 1
#define FH_FLAGS_UNINSTALL 2
#define FH_FLAGS_SILENT 4

#define FH_SIG 0xDEADBEEF

// neato surprise signature that goes in firstheader. :)
#define FH_INT1 0x6C6C754E
#define FH_INT2 0x74666F53
#define FH_INT3 0x74736E49


// amount of bmp/ico in resource to skip (apparently the first 20/22 bytes can be different)
#define BMP_HDRSKIP 20
#define ICO_HDRSKIP 22

// the following are only used/implemented in exehead, not makensis.

int isheader(firstheader *h); // returns 0 on not header, length_of_datablock on success

// returns nonzero on error
// returns 0 on success
// on success, m_header will be set to a pointer that should eventually be GlobalFree()'d.
// (or m_uninstheader)
int loadHeaders(void);

extern HANDLE g_db_hFile;
extern int g_db_offset;

char *GetStringFromStringTab(int offs);
int GetCompressedDataFromDataBlock(int offset, HANDLE hFileOut);
int GetCompressedDataFromDataBlockToMemory(int offset, char *out, int out_len);

#endif //_FILEFORM_H_
