; bigtest.nsi
;
; This script attempts to test most of the functionality of NSIS.
;
!ifdef HAVE_UPX
!packhdr tmp.dat "upx\upx -9 tmp.dat"
!endif

!ifdef NOCOMPRESS
SetCompress off
!endif

SetDateSave on
SetDatablockOptimize on
CRCCheck on
SilentInstall normal
BGGradient 000000 800000 FFFFFF
InstallColors FF8080 000030


Name "BigNSISTest"
Caption "NSIS Big Test"
Icon "main.ico"
OutFile "bigtest.exe"

LicenseText "make sure this license text is all there (it's about 28k last I checked). The last line is a simple '}'."
LicenseData "source\exehead\ui.c"

InstallDir "$PROGRAMFILES\NSISCrap\BigNSISTest"
InstallDirRegKey HKLM SOFTWARE\NSISCrap\BigNSISTest "Install_Dir"
DirText "Choose a directory to install in to:"
DirShow show

; uninstall stuff
UninstallText "This will uninstall example2. Hit next to continue."
UninstallExeName "bt-uninst.exe"
UninstallIcon "uninst.ico"

ComponentText "This will install the test on your computer. Select which optional things you want installed."
EnabledBitmap bitmap1.bmp
DisabledBitmap bitmap2.bmp
!ifndef NOINSTTYPES ; only if not defined
InstType "Most"
InstType "Full"
InstType "More"
InstType "Base"
;InstType /NOCUSTOM
!endif

AutoCloseWindow false
ShowInstDetails show


Section "" ; empty string makes it hidden, so would starting with -
; write reg crap
StrCpy $1 "POOOOOOOOOOOP"
DetailPrint "I like to f*ck sheep $1"
WriteRegStr HKLM SOFTWARE\NSISCrap\BigNSISTest "Install_Dir" "$INSTDIR"
; write uninstall strings
WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\BigNSISTest" "DisplayName" "BigNSISTest (remove only)"
WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\BigNSISTest" "UninstallString" '"$INSTDIR\bt-uninst.exe"'
File /oname=poop.exe C:\windows\notepad.exe

SetOutPath $INSTDIR
File "source\exehead\bin2h.exe"
CreateDirectory "$INSTDIR\shiz\crap" ; 2 recursively create a directory for fun.
Nop ; for fun
SectionEnd

Section "TempTest"
SectionIn 123
  Start: MessageBox MB_OK "Start:"

  MessageBox MB_YESNO "Goto Poop" IDYES Poop

  MessageBox MB_OK "Right before Poop:"

  Poop: MessageBox MB_OK "Poop:"
  
  MessageBox MB_OK "Right after Poop:"

  MessageBox MB_YESNO "Goto Start:?" IDYES Start

SectionEnd




Function poopTest
ReadINIStr $1 "$INSTDIR\test.ini" "MySectionShit" "Value1"
StrCmp $1 $8 NoFailedMsg
  MessageBox MB_OK "WriteINIStr failed"
NoFailedMsg: FunctionEnd 


Section "Test Registry/INI functions"
SectionIn 123
WriteRegStr HKLM SOFTWARE\NSISCrap\BigNSISTest "StrTest_INSTDIR" "$INSTDIR"
WriteRegDword HKLM SOFTWARE\NSISCrap\BigNSISTest "DwordTest_0xDEADBEEF" 0xdeadbeef
WriteRegDword HKLM SOFTWARE\NSISCrap\BigNSISTest "DwordTest_123456" 123456
WriteRegDword HKLM SOFTWARE\NSISCrap\BigNSISTest "DwordTest_0123" 0123
WriteRegBin HKLM SOFTWARE\NSISCrap\BigNSISTest "BinTest_deadbeef01f00dbeef" "DEADBEEF01F00DBEEF"
StrCpy $8 "$SYSDIR\Poop"
WriteINIStr "$INSTDIR\test.ini"  "MySection" "Value1" $8
WriteINIStr "$INSTDIR\test.ini"  "MySectionShit" "Value1" $8
WriteINIStr "$INSTDIR\test.ini"  "MySectionShit" "Value2" $8
WriteINIStr "$INSTDIR\test.ini"  "POOPon" "Value1" $8
Call poopTest

DeleteINIStr "$INSTDIR\test.ini" "POOPon" "Value1"
DeleteINISec "$INSTDIR\test.ini" "MySectionShit"

ReadINIStr $1 "$INSTDIR\test.ini" "MySectionShit" "Value1"
StrCmp $1 "" INIDelSuccess
  MessageBox MB_OK "DeleteINISec failed"
INIDelSuccess:

ClearErrors
ReadRegStr $1 HKCR "software\microsoft" shit
IfErrors 0 NoError
  MessageBox MB_OK "could not read from HKCR\software\microsoft\shit"
  Goto ErrorYay
NoError:
  MessageBox MB_OK "read '$1' from HKCR\software\microsoft\shit"
ErrorYay:
SectionEnd


Function "CSCTest"
CreateDirectory "$SMPROGRAMS\Big NSIS Test"
SetOutPath $INSTDIR ; for working directory
CreateShortCut "$SMPROGRAMS\Big NSIS Test\Uninstall BIG NSIS Test.lnk" "$INSTDIR\bt-uninst.exe" ; use defaults for parameters, icon, etc.
; this one will use notepad's icon, start it minimized, and give it a hotkey (of Ctrl+Shift+Q)
CreateShortCut "$SMPROGRAMS\Big NSIS Test\bin2h.exe.lnk" "$INSTDIR\bin2h.exe" "" "$WINDIR\notepad.exe" 0 SW_SHOWMINIMIZED CONTROL|SHIFT|Q
CreateShortCut "$SMPROGRAMS\Big NSIS Test\TheDir.lnk" "$INSTDIR\" "" "" 0 SW_SHOWMAXIMIZED CONTROL|SHIFT|Z
FunctionEnd

Section "Test CreateShortCut"
SectionIn 123
Call CSCTest
SectionEnd

Function myfunc
StrCpy $2 "poop=$1"
MessageBox MB_OK "myfunc: $2"
FunctionEnd

Section "Test Branching"
BeginTestSection:
SectionIn 123
SetOutPath $INSTDIR
IfFileExists "$INSTDIR\bin2h.c" 0 BranchTest69
  MessageBox MB_YESNO|MB_ICONQUESTION "Would you like to overwrite $INSTDIR\bin2h.c?" IDNO NoOverwrite ; skipped if file doesn't exist
BranchTest69:
  SetOverwrite ifnewer ; NOT AN INSTRUCTION, NOT COUNTED IN SKIPPINGS
NoOverwrite:
File "source\exehead\bin2h.c" ; skipped if answered no
SetOverwrite try ; NOT AN INSTRUCTION, NOT COUNTED IN SKIPPINGS
MessageBox MB_YESNO|MB_ICONQUESTION "Would you like to skip the rest of this section?" IDYES EndTestBranch
MessageBox MB_YESNO|MB_ICONQUESTION "Would you like to go back to the beginning of this section?" IDYES BeginTestSection
MessageBox MB_YESNO|MB_ICONQUESTION "Would you like to hide the installer and wait five seconds?" IDNO NoHide
HideWindow
Sleep 5000
BringToFront
NoHide:
MessageBox MB_YESNO|MB_ICONQUESTION "Would you like to call the function 5 times?" IDNO NoRecurse
StrCpy $1 "x"
LoopPoop: 
  Call myfunc
  StrCpy $1 "x$1"
  StrCmp $1 "xxxxxx" 0 LoopPoop
NoRecurse:

EndTestBranch:

SectionEnd

Section "Test CopyFiles"
SectionIn 123
SetOutPath $INSTDIR\cpdest
CopyFiles "$WINDIR\*.ini" "$INSTDIR\cpdest" 0
SectionEnd

Section "Test Exec functions"
SectionIn 123
GetFullDLLPath $1 notepad.exe
MessageBox MB_OK "notepad.exe=$1"
Exec '"$1"'
ExecShell "open" '"$INSTDIR"'
Sleep 500
BringToFront
SectionEnd

Section "Test Netscape Plug-in Installing"
SectionIn 2
; I don't have a real netscape plug-in, so I'll just have it put, uhh, makensis.exe into those directories.
InstNSPlug "makensis.exe"
SectionEnd

Section "Test ActiveX control registration"
SectionIn 2
UnRegDLL "$SYSDIR\spin32.ocx"
Sleep 1000
RegDLL "$SYSDIR\spin32.ocx"
Sleep 1000
SectionEnd

; special uninstall section.
Section "Uninstall"
DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\BigNSISTest"
DeleteRegKey HKLM "SOFTWARE\NSISCrap\BigNSISTest"
DeleteNSPlug "makensis.exe"
Delete "$INSTDIR\bin2h.exe"
Delete "$INSTDIR\bin2h.c"
Delete "$INSTDIR\bt-uninst.exe"
Delete "$INSTDIR\test.ini"
Delete "$SMPROGRAMS\Big NSIS Test\*.*"
RMDir "$SMPROGRAMS\BiG NSIS Test"
MessageBox MB_YESNO|MB_ICONQUESTION "Would you like to remove the directory $INSTDIR\cpdest?" IDNO NoDelete
Delete "$INSTDIR\cpdest\*.*"
RMDir "$INSTDIR\cpdest" ; skipped if no
NoDelete:
RMDir "$INSTDIR\shiz\crap"
RMDir "$INSTDIR\shiz"
RMDir "$INSTDIR"
IfFileExists "$INSTDIR" 0 NoErrorMsg
MessageBox MB_OK "Note: $INSTDIR could not be removed!" IDOK 0 ; skipped if file doesn't exist
NoErrorMsg:
SectionEnd

; eof
