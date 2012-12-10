!ifdef NO_COMPRESSION
SetCompress off
SetDatablockOptimize off
!endif

!ifdef NO_CRC
CRCCheck off
!endif

Name "NSIS"
Caption "Nullsoft Install System - Setup"
OutFile nsis141.exe

#BGGradient 000000 800000 FFFFFF
#InstallColors FF8080 000000

LicenseText "You must read the following license before installing:"
LicenseData license.txt
ComponentText "This will install the Nullsoft Install System v1.41 on your computer:"
InstType Normal
InstType "Full (w/ Source Code)"
DirShow show
AutoCloseWindow false
ShowInstDetails show
DirText "Please select a location to install NSIS (or use the default):"
SetOverwrite on
SetDateSave on
!ifdef HAVE_UPX
  !packhdr tmp.dat "upx\upx --best --compress-icons=1 tmp.dat"
!endif

InstallDir $PROGRAMFILES\NSIS
InstallDirRegKey HKLM SOFTWARE\NSIS ""

Section "NSIS development system (required)"
  SectionIn 1 2
  SetOutPath $INSTDIR
  File makensis.exe
  File makensis.htm
  File nsisconf.nsi
  File makensis.nsi
  File example1.nsi
  File example2.nsi
  File viewhtml.nsi
  File waplugin.nsi
  File bigtest.nsi
  File uglytest.nsi
  File license.txt
  File bitmap1.bmp
  File bitmap2.bmp
  File main.ico
  File uninst.ico
SectionEnd

Section "NSI Development Shell Extensions"
  SectionIn 1 2

  ; back up old value of .nsi
  ReadRegStr $1 HKCR ".nsi" ""
  StrCmp $1 "" Label1
    StrCmp $1 "NSISFile" Label1
    WriteRegStr HKCR ".nsi" "backup_val" $1
Label1:

  WriteRegStr HKCR ".nsi" "" "NSISFile"
  WriteRegStr HKCR "NSISFile" "" "NSI Script File"
  WriteRegStr HKCR "NSISFile\shell" "" "open"
  WriteRegStr HKCR "NSISFile\DefaultIcon" "" $INSTDIR\makensis.exe,0
  WriteRegStr HKCR "NSISFile\shell\open\command" "" 'notepad.exe "%1"'
  WriteRegStr HKCR "NSISFile\shell\compile" "" "Compile NSI"
  WriteRegStr HKCR "NSISFile\shell\compile\command" "" '"$INSTDIR\makensis.exe" /CD /PAUSE "%1"'
SectionEnd

Section "NSIS Start Menu Group"
  SectionIn 1 2
  SetOutPath $SMPROGRAMS\NSIS
  CreateShortCut "$SMPROGRAMS\NSIS\NSIS Home Page.lnk" \
                 "http://www.nullsoft.com/free/nsis/"
  CreateShortCut "$SMPROGRAMS\NSIS\Uninstall NSIS.lnk" \
                 "$INSTDIR\uninst-nsis.exe"
  CreateShortCut "$SMPROGRAMS\NSIS\NSIS Documentation.lnk" \
                 "$INSTDIR\makensis.htm"
  CreateShortCut "$SMPROGRAMS\NSIS\NSIS Program Directory.lnk" \
                 "$INSTDIR"
  CreateShortCut "$SMPROGRAMS\NSIS\NSI Online Template Generator.lnk" \
                 "http://www.firehose.net/free/nsis/makensitemplate.phtml"
SectionEnd

Section "MakeNSIS Desktop Icon"
  SectionIn 1 2
  SetOutPath $INSTDIR
  CreateShortCut "$DESKTOP\MakeNSIS.lnk" "$INSTDIR\Makensis.exe" '/CD /PAUSE'
SectionEnd

Section "Splash screen support"
  SectionIn 1 2
  SetOutPath $INSTDIR
  File splash.exe
  File splash.txt
  IfFileExists $SMPROGRAMS\NSIS 0 NoShortCuts
    CreateShortCut "$SMPROGRAMS\NSIS\Splash Screen Help.lnk" \
                   "$INSTDIR\splash.txt"
  NoShortCuts:
SectionEnd

Section "ZIP2EXE converter"
  SectionIn 1 2
  SetOutPath $INSTDIR
  File zip2exe.exe
  IfFileExists $SMPROGRAMS\NSIS 0 NoShortCuts
    CreateShortCut "$SMPROGRAMS\NSIS\ZIP2EXE converter.lnk" \
                   "$INSTDIR\zip2exe.exe"
  NoShortCuts:
SectionEnd


!ifndef NO_SOURCE
SectionDivider

Section "NSIS Source Code"
  SectionIn 2
  SetOutPath $INSTDIR\Source
  File Source\*.cpp
  File Source\*.h
  File Source\script1.rc
  File Source\makenssi.dsp
  File Source\makenssi.dsw
  File Source\icon.ico
  SetOutPath $INSTDIR\Source\zlib
  File Source\zlib\*.*
  SetOutPath $INSTDIR\Source\exehead
  File Source\exehead\*.c
  File Source\exehead\*.h
  File Source\exehead\resource.rc
  File Source\exehead\exehead.dsp
  File Source\exehead\exehead.dsw
  File Source\exehead\nsis.ico
  File Source\exehead\uninst.ico
  File Source\exehead\bitmap1.bmp
  File Source\exehead\bitmap2.bmp
  File Source\exehead\bin2h.exe
  SetOutPath $INSTDIR\Source\Splash
  File Source\Splash\splash.c
  File Source\Splash\splash.dsp
  File Source\Splash\splash.dsw
  SetOutPath $INSTDIR\Source\zip2exe
  File Source\zip2exe\*.cpp
  File Source\zip2exe\*.ico
  File Source\zip2exe\*.h
  File Source\zip2exe\*.rc
  File Source\zip2exe\*.dsw
  File Source\zip2exe\*.dsp
  SetOutPath $INSTDIR\Source\zip2exe\zlib
  File Source\zip2exe\zlib\*.*
  IfFileExists $SMPROGRAMS\NSIS 0 NoSourceShortCuts
    CreateShortCut "$SMPROGRAMS\NSIS\MakeNSIS project workspace.lnk" \
                   "$INSTDIR\source\makenssi.dsw"
    CreateShortCut "$SMPROGRAMS\NSIS\ZIP2EXE project workspace.lnk" \
                   "$INSTDIR\source\zip2exe\zip2exe.dsw"
    CreateShortCut "$SMPROGRAMS\NSIS\Splash project workspace.lnk" \
                   "$INSTDIR\source\splash\splash.dsw"
  NoSourceShortCuts:
SectionEnd

!endif

Section -post
  SetOutPath $INSTDIR

  ; since the installer is now created last (in 1.2+), this makes sure 
  ; that any old installer that is readonly is overwritten.
  Delete $INSTDIR\uninst-nsis.exe 

  WriteRegStr HKLM SOFTWARE\NSIS "" $INSTDIR
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\NSIS" \
                   "DisplayName" "NSIS Development Kit (remove only)"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\NSIS" \
                   "UninstallString" '"$INSTDIR\uninst-nsis.exe"'
  ExecShell open '$INSTDIR'
  Sleep 500
  BringToFront
SectionEnd

Function .onInstSuccess
  MessageBox MB_YESNO|MB_ICONQUESTION \
             "Setup has completed. View readme file now?" \
             IDNO NoReadme
    ExecShell open '$INSTDIR\makensis.htm'
  NoReadme:
FunctionEnd

!ifndef NO_UNINST
UninstallText "This will uninstall NSIS from your system:"
UninstallExeName uninst-nsis.exe

Section Uninstall
  ReadRegStr $1 HKCR ".nsi" ""
  StrCmp $1 "NSISFile" 0 NoOwn ; only do this if we own it
    ReadRegStr $1 HKCR ".nsi" "backup_val"
    StrCmp $1 "" 0 RestoreBackup ; if backup == "" then delete the whole key
      DeleteRegKey HKCR ".nsi"
    Goto NoOwn
    RestoreBackup:
      WriteRegStr HKCR ".nsi" "" $1
      DeleteRegValue HKCR ".nsi" "backup_val"
  NoOwn:

  DeleteRegKey HKCR "NSISFile"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\NSIS"
  DeleteRegKey HKLM SOFTWARE\NSIS
  Delete $SMPROGRAMS\NSIS\*.lnk
  RMDir $SMPROGRAMS\NSIS
  Delete $DESKTOP\MakeNSIS.lnk
  Delete $INSTDIR\makensis.exe
  Delete $INSTDIR\zip2exe.exe
  Delete $INSTDIR\splash.txt
  Delete $INSTDIR\splash.exe
  Delete $INSTDIR\makensis.htm
  Delete $INSTDIR\makensis.rtf
  Delete $INSTDIR\uninst-nsis.exe
  Delete $INSTDIR\nsisconf.nsi
  Delete $INSTDIR\makensis.nsi
  Delete $INSTDIR\example1.nsi
  Delete $INSTDIR\example2.nsi
  Delete $INSTDIR\waplugin.nsi
  Delete $INSTDIR\viewhtml.nsi
  Delete $INSTDIR\bigtest.nsi
  Delete $INSTDIR\uglytest.nsi
  Delete $INSTDIR\spin.nsi
  Delete $INSTDIR\wafull.nsi
  Delete $INSTDIR\main.ico
  Delete $INSTDIR\makensis-license.txt
  Delete $INSTDIR\license.txt
  Delete $INSTDIR\uninst.ico
  Delete $INSTDIR\bitmap1.bmp
  Delete $INSTDIR\bitmap2.bmp
  RMDir /r $INSTDIR\Source
  RMDir $INSTDIR

  ; if $INSTDIR was removed, skip these next ones
  IfFileExists $INSTDIR 0 Removed 
    MessageBox MB_YESNO|MB_ICONQUESTION \
      "Remove all files in your NSIS directory? (If you have anything\
 you created that you want to keep, click No)" IDNO Removed
    Delete $INSTDIR\*.* ; this would be skipped if the user hits no
    RMDir /r $INSTDIR
    IfFileExists $INSTDIR 0 Removed 
      MessageBox MB_OK|MB_ICONEXCLAMATION \
                 "Note: $INSTDIR could not be removed."
  Removed:
SectionEnd

!endif
