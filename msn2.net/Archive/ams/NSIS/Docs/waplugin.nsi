; waplugin.nsi
;
; This script will generate an installer that installs a Winamp plug-in.
; It also puts a license page on, for shits and giggles.
;
; This installer will automatically alert the user that installation was
; successful, and ask them whether or not they would like to make the 
; plug-in the default and run Winamp.
;

; The name of the installer
Name "TinyVis Plug-in"

; The file to write
OutFile "waplugin.exe"

; License page
; LicenseText "This installer will install the Nullsoft Tiny Visualization 2000 Plug-in for Winamp. Please read the license below."
; use the default makensis license :)
; LicenseData license.txt

; The default installation directory
InstallDir $PROGRAMFILES\Winamp
; detect winamp path from uninstall string if available
InstallDirRegKey HKLM \
                 "Software\Microsoft\Windows\CurrentVersion\Uninstall\Winamp" \
                 "UninstallString"

; The text to prompt the user to enter a directory
DirText "Please select your Winamp path below (you will be able to proceed when Winamp is detected):"

; automatically close the installer when done.
AutoCloseWindow true
; hide the "show details" box
ShowInstDetails nevershow

Function .onVerifyInstDir
  IfFileExists $INSTDIR\Winamp.exe Good
    Abort
  Good:
FunctionEnd

Function QueryWinampVisPath ; sets $1 with vis path
  StrCpy $1 $INSTDIR\Plugins
  ; use DSPDir instead of VISDir to get DSP plugins directory
  ReadINIStr $9 $INSTDIR\winamp.ini Winamp VisDir 
  StrCmp $9 "" End
  IfFileExists $9 0 End
    StrCpy $1 $9 ; update dir
  End: 
FunctionEnd


; The stuff to install
Section "ThisNameIsIgnoredSoWhyBother?"
  Call QueryWinampVisPath
  SetOutPath $1

  ; File to extract
  File "C:\program files\winamp\plugins\vis_nsfs.dll"

  ; prompt user, and if they select no, skip the following 3 instructions.
  MessageBox MB_YESNO|MB_ICONQUESTION \
             "The plug-in was installed. Would you like to run Winamp now with TinyVis as the default plug-in?" \
             IDNO NoWinamp
    WriteINIStr "$INSTDIR\Winamp.ini" "Winamp" "visplugin_name" "vis_nsfs.dll"
    WriteINIStr "$INSTDIR\Winamp.ini" "Winamp" "visplugin_num" "0"
    Exec '"$INSTDIR\Winamp.exe"'
  NoWinamp:
SectionEnd

; eof
