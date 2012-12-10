#include <windows.h>
#include <stdlib.h>
#include <stdio.h>

#include "build.h"
#include "tokens.h"

typedef struct 
{
  int id;
  char *name;
  int num_parms; // minimum number of parameters
  int opt_parms; // optional parmaters, usually 0, can be -1 for unlimited.
  char *usage_str;
} tokenType;


static tokenType tokenlist[TOK__LAST] =
{
{TOK_ABORT,"Abort",0,1,"[message]"},
{TOK_AUTOCLOSE,"AutoCloseWindow",1,0,"(false|true)"},
{TOK_BGGRADIENT,"BGGradient",0,3,"(off | [top_color [bottom_color [text_color]]])"},
{TOK_BRINGTOFRONT,"BringToFront",0,0,""},
{TOK_CALL,"Call",1,0,"function_name"},
{TOK_CAPTION,"Caption",1,0,"installer_caption"},
{TOK_CLEARERRORS,"ClearErrors",0,0,""},
{TOK_COMPTEXT,"ComponentText",1,0,"component_page_description"},
{TOK_COMPAREDLLS,"CompareDLLVersions",4,1,"[/STOREFROM] dll1 dll2 goto_if_dll1_newer goto_if_dll2_newer"},
{TOK_COMPAREFILETIMES,"CompareFileTimes",4,1,"[/STOREFROM] file1 file2 goto_if_file1_newer goto_if_file2_newer"},
{TOK_COPYFILES,"CopyFiles",3,0,"source_path destination_path total_size_in_kb"},
{TOK_CRCCHECK,"CRCCheck",1,0,"(on|off)"},
{TOK_CREATEDIR,"CreateDirectory",1,0,"directory_name"},
{TOK_CREATESHORTCUT,"CreateShortCut",2,5,"shortcut_name.lnk shortcut_target [parameters [icon_file [icon index [showmode [hotkey]]]]]\n    showmode=(SW_SHOWNORMAL|SW_SHOWMAXIMIZED|SW_SHOWMINIMIZED)\n    hotkey=(ALT|CONTROL|EXT|SHIFT)|(F1-F24|A-Z)"},
{TOK_DBOPTIMIZE,"SetDatablockOptimize",1,0,"(off|on)"},
{TOK_DELETEINISEC,"DeleteINISec",2,0,"ini_file section_name"},
{TOK_DELETEINISTR,"DeleteINIStr",3,0,"ini_file section_name entry_name"},
{TOK_DELETEREGKEY,"DeleteRegKey",2,0,"root_key subkey\n    root_key=(HKCR|HKLM|HKCU|HKU|HKCC|HKDD|HKPD)"},
{TOK_DELETEREGVALUE,"DeleteRegValue",3,0,"root_key subkey entry_name\n    root_key=(HKCR|HKLM|HKCU|HKU|HKCC|HKDD|HKPD)"},
{TOK_DELETENSPLUG,"DeleteNSPlug",1,0,"netscape_plugin_name.dll"},
{TOK_DELETE,"Delete",1,1,"[/REBOOTOK] filespec"},
{TOK_DETAILPRINT,"DetailPrint",1,0,"message"},
{TOK_DIRTEXT,"DirText",1,0,"directory_page_description"},
{TOK_DIRSHOW,"DirShow",1,0,"(show|hide)"},
{TOK_DISABLEDBITMAP,"DisabledBitmap",1,0,"local_bitmap.bmp"},
{TOK_ENABLEDBITMAP,"EnabledBitmap",1,0,"local_bitmap.bmp"},
{TOK_EXEC,"Exec",1,0,"command_line"},
{TOK_EXECWAIT,"ExecWait",1,0,"command_line"},
{TOK_EXECSHELL,"ExecShell",2,2,"(open|print|etc) command_line [parameters [showmode]]\n   showmode=(SW_SHOWNORMAL|SW_SHOWMAXIMIZED|SW_SHOWMINIMIZED)"},
{TOK_FINDWINDOW,"FindWindow",2,1,"(prompt|close|closeinstant|jump:Label) WindowClass [prompt_text]"},
{TOK_FINDWINDOWBYTITLE,"FindWindowByTitle",2,1,"(prompt|close|closeinstant|jump:Label) WindowTitle [prompt_text]"},
{TOK_FILE,"File",1,-1,"(/r filespec [...]|/oname=outfile one_file_only)"},
{TOK_FUNCTION,"Function",1,0,"function_name"},
{TOK_FUNCTIONEND,"FunctionEnd",0,0,""},
{TOK_GETFULLDLLPATH,"GetFullDLLPath",2,0,"$(0-9) dll_name"},
{TOK_HIDEWINDOW,"HideWindow",0,0,""},
{TOK_ICON,"Icon",1,0,"local_icon.ico"},
{TOK_IFERRORS,"IfErrors",1,1,"label_to_goto_if_errors [label_to_goto_if_no_errors]"},
{TOK_IFFILEEXISTS,"IfFileExists",2,1,"filename label_to_goto_if_file_exists [label_to_goto_otherwise]"},
{TOK_INSTALLDIRREGKEY,"InstallDirRegKey",3,0,"root_key subkey entry_name\n    root_key=(HKCR|HKLM|HKCU|HKU|HKCC|HKDD|HKPD)"},
{TOK_INSTCOLORS,"InstallColors",2,0,"foreground_color background_color"},
{TOK_INSTDIR,"InstallDir",1,0,"default_install_directory"},
{TOK_INSTNSPLUG,"InstNSPlug",1,0,"local_nsplugin.dll"},
{TOK_INSTTYPE,"InstType",1,0,"(/NOCUSTOM|TypeName)"},
{TOK_GOTO,"Goto",1,0,"label"},
{TOK_LICENSEDATA,"LicenseData",1,0,"local_file_that_has_license_text.txt"},
{TOK_LICENSETEXT,"LicenseText",1,0,"license_page_description"},
{TOK_MESSAGEBOX,"MessageBox",2,2,"mode messagebox_text [return_check [label_to_goto_if_equal]]\n    mode=modeflag[|modeflag[|modeflag[...]]]\n    "
                                "modeflag=(MB_ABORTRETRYIGNORE|MB_OK|MB_OKCANCEL|MB_RETRYCANCEL|MB_YESNO|MB_YESNOCANCEL|MB_ICONEXCLAMATION|MB_ICONINFORMATION|MB_ICONQUESTION|MB_ICONSTOP|MB_TOPMOST|MB_SETFOREGROUND|MB_RIGHT"},
{TOK_NOP,"Nop",0,0,""},
{TOK_NAME,"Name",1,0,"installer_name"},
{TOK_OUTFILE,"OutFile",1,0,"install_output.exe"},
{TOK_READINISTR,"ReadINIStr",4,0,"$(0-9|INSTDIR|OUTDIR) ini_file section entry_name"},
{TOK_READREGSTR,"ReadRegStr",4,0,"$(0-9|INSTDIR|OUTDIR) rootkey subkey entry\n   root_key=(HKCR|HKLM|HKCU|HKU|HKCC|HKDD|HKPD)"},
{TOK_REGDLL,"RegDLL",1,1,"dll_path_on_target.dll [entrypoint_symbol]"},
{TOK_RENAME,"Rename",2,1,"[/REBOOTOK] source_file destination_file"},
{TOK_RMDIR,"RMDir",1,1,"[/r] directory_name"},
{TOK_SECTION,"Section",0,1,"[section_name|-section_name]"},
{TOK_SECTIONDIVIDER,"SectionDivider",0,0,""},
{TOK_SECTIONEND,"SectionEnd",0,0,""},
{TOK_SECTIONIN,"SectionIn",1,-1,"InstType [InstType [...]]"},
{TOK_SETCOMPRESS,"SetCompress",1,0,"(off|auto|force)"},
{TOK_SETDATESAVE,"SetDateSave",1,0,"(off|on)"},
{TOK_SETDETAILSVIEW,"SetDetailsView",1,0,"(hide|show)"},
{TOK_SETFILEATTRIBUTES,"SetFileAttributes",2,0,"file attribute[|attribute[...]]\n    attribute=(NORMAL|ARCHIVE|HIDDEN|OFFLINE|READONLY|SYSTEM|TEMPORARY|0)"},
{TOK_SETERRORS,"SetErrors",0,0,""},
{TOK_SETOUTPATH,"SetOutPath",1,0,"output_path"},
{TOK_SETOVERWRITE,"SetOverwrite",1,0,"(on|off|try|ifnewer)"},
{TOK_SHOWDETAILS,"ShowInstDetails",1,0,"(hide|show|nevershow)"},
{TOK_SILENTINST,"SilentInstall",1,0,"(normal|silent|silentlog)"},
{TOK_SLEEP,"Sleep",1,0,"sleep_time_in_ms"},
{TOK_STRCMP,"StrCmp",3,1,"str1 str2 label_to_goto_if_equal [label_to_goto_if_not]"},
{TOK_STRCPY,"StrCpy",2,1,"$(0-9,INSTDIR,OUTDIR) str [maxlen]"},
{TOK_UNINSTALLEXENAME,"UninstallExeName",1,0,"uninstaller_exe_name_to_generate.exe"},
{TOK_UNINSTICON,"UninstallIcon",1,0,"icon_on_local_system.ico"},
{TOK_UNINSTTEXT,"UninstallText",1,0,"Text_to_go_on_uninstall page"},
{TOK_UNREGDLL,"UnRegDLL",1,0,"dll_path_on_target.dll"},
{TOK_WRITEINISTR,"WriteINIStr",4,0,"ini_file section_name entry_name new_value"},
{TOK_WRITEREGBIN,"WriteRegBin",4,0,"rootkey subkey entry_name hex_string_like_12848412AB\n    root_key=(HKCR|HKLM|HKCU|HKU|HKCC|HKDD|HKPD)"},
{TOK_WRITEREGDWORD,"WriteRegDword",4,0,"rootkey subkey entry_name new_value_dword\n    root_key=(HKCR|HKLM|HKCU|HKU|HKCC|HKDD|HKPD)"},
{TOK_WRITEREGSTR,"WriteRegStr",4,0,"rootkey subkey entry_name new_value_string\n    root_key=(HKCR|HKLM|HKCU|HKU|HKCC|HKDD|HKPD)"},
{TOK_P_PACKEXEHEADER,"!packhdr",2,0,"temp_file_name command_line_to_compress_that_temp_file"},
{TOK_P_SYSTEMEXEC,"!system",3,0,"command (<|>|<>|=|ignore) retval"},
{TOK_P_INCLUDE,"!include",1,0,"filename.nsi"},
{TOK_P_CD,"!cd",1,0,"absolute_or_relative_new_directory"},
{TOK_P_IFDEF,"!ifdef",1,-1,"symbol [| symbol2 [& symbol3 [...]]]"},
{TOK_P_IFNDEF,"!ifndef",1,-1,"symbol [| symbol2 [& symbol3 [...]]]"},
{TOK_P_ENDIF,"!endif",0,0,""},
{TOK_P_DEFINE,"!define",1,0,"symbol"},
{TOK_P_ELSE,"!else",0,-1,"[ifdef|ifndef symbol [|symbol2 [& symbol3 [...]]]]"},
{TOK_P_ERROR,"!error",0,1,"[error_message]"},
{TOK_P_WARNING,"!warning",0,1,"[warning_message]"},
{TOK_MUTEXWAIT, "WaitForMutex", 1, 1, "mutex_name"},
{TOK_OPENSTATUS, "OpenStatus", 2, 2, "caption message"},
{TOK_UPDATESTATUS, "UpdateStatus", 1, 1, "message"},
{TOK_CLOSESTATUS, "CloseStatus", 0, 0, ""},
};

void CEXEBuild::print_help(char *commandname)
{
  int x;
  for (x = 0; x < TOK__LAST; x ++)
  {
    if (!commandname || !stricmp(tokenlist[x].name,commandname))
    {
      printf("%s%s %s\n",commandname?"Usage: ":"",tokenlist[x].name,tokenlist[x].usage_str);
      if (commandname) break;
    }
  }
  if (x == TOK__LAST && commandname)\
  {
    printf("Invalid command \"%s\"\n",commandname);
  }

}

int CEXEBuild::get_commandtoken(char *s, int *np, int *op)
{
  int x;
  for (x = 0; x < TOK__LAST; x ++)
    if (!stricmp(tokenlist[x].name,s)) 
    {
      *np=tokenlist[x].num_parms;
      *op=tokenlist[x].opt_parms;
      return tokenlist[x].id;
    }
  return -1;
}
