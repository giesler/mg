Function .onInit
  MessageBox MB_YESNO "Would you like to install the Ugly Test?" IDYES NoAbort
    MessageBox MB_OK "Install aborted."
    Abort
  NoAbort: 
FunctionEnd

Function .onVerifyInstDir 
; if you want to use this, check $INSTDIR to make sure it is valid, and Abort
; if it is not. (i.e. for a winamp plug-in, you could do: 
; IfFileExists $INSTDIR\Winamp.exe Exists
;   Abort
; Exists:
FunctionEnd

Function .onUserAbort
  MessageBox MB_YESNO "Abort install?" IDYES NoAbort
    Abort ; this causes the installer to not cancel itself 
          ; (the user aborted the "quit" process)
  NoAbort:
FunctionEnd

Function .onInstSuccess
  MessageBox MB_OK "Yay for you, the installer worked. Don't break your computer now, bitch."
FunctionEnd

Function .onInstFailed
MessageBox MB_OK "Your computer sucks, so try again later."
FunctionEnd

Name "Ugly Test"
Caption "Ugly Test Setup Installer"
OutFile "uglytest.exe"
BGGradient 0000FF FFFF00 FFFFFF

DirText "This will install the ugly test on your computer. Choose a directory."
InstallDir $PROGRAMFILES\UglyTest
InstallDirRegKey HKLM Software\UglyTest ""

Function SetupRegKeys
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\UglyTest" "DisplayName" "UglyTest (remove only)"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\UglyTest" "UninstallString" '"$INSTDIR\uninst.exe"'
  WriteRegStr HKLM Software\UglyTest "" $INSTDIR
FunctionEnd

Section ; default section
  SetOutPath $INSTDIR
  Call SetupRegKeys
  MessageBox MB_YESNO "Would you like to abort the install?" IDNO NoAbort
    Abort "Install aborted by user"
  NoAbort:
SectionEnd ; end the section

UninstallEXEName "uninst.exe"
UninstallText "This will uninstall the Ugly Test from your system."

Section Uninstall
  DeleteRegKey HKLM Software\UglyTest
  Delete $INSTDIR\uninst.exe
  RMDir $INSTDIR
SectionEnd

; eof
