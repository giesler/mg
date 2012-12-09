; NOTE: this .NSI script is designed for NSIS v1.3+

Name "Adult Media Swapper"
OutFile "AMSInstall.exe"
Icon "amsinst.ico"
UninstallIcon "amsuninst.ico"

Function .onInit
  Call CheckReqs
FunctionEnd

Function .onInstSuccess
  Exec '"$INSTDIR\AMSClient.exe"'
FunctionEnd

; Some default compiler settings (uncomment and change at will):
; SetCompress auto ; (can be off or force)
; SetDatablockOptimize on ; (can be off)
; CRCCheck on ; (can be off)
; AutoCloseWindow false ; (can be true for the window go away automatically at end)
; ShowInstDetails hide ; (can be show to have them shown, or nevershow to disable)
; SetDateSave off ; (can be on to have files restored to their orginal date)

LicenseText "You must agree to this license before installing."
LicenseData "license.txt"

InstallDir "$PROGRAMFILES\AMS"
InstallDirRegKey HKEY_LOCAL_MACHINE "SOFTWARE\Line 2 Systems\Adult Media Swapper" ""
DirShow show ; (make this hide to not let the user change it)
DirText "Select the directory to install AMS in"

Section "" ; (default section)
SetOutPath "$INSTDIR"
  ; add files / whatever that need to be installed here.
  WriteRegStr HKEY_LOCAL_MACHINE "SOFTWARE\Line 2 Systems\Adult Media Swapper" "" "$INSTDIR"
  WriteRegStr HKEY_LOCAL_MACHINE "Software\Microsoft\Windows\CurrentVersion\Uninstall\Adult Media Swapper" "DisplayName" "AMS (remove only)"
  WriteRegStr HKEY_LOCAL_MACHINE "Software\Microsoft\Windows\CurrentVersion\Uninstall\Adult Media Swapper" "UninstallString" '"$INSTDIR\uninstams.exe"'
  ; Copy files
  SetOverwrite "on"
  File "AMSClient.exe"
  ; Install MSXML files  
  Call InstallMSXML
  ; Start menu stuff
  SetOverwrite "on"
  SetOutPath "$INSTDIR"		; make sure current directory is this for shortcut stuff
  CreateDirectory "$SMPROGRAMS\AMS"
  CreateShortCut "$SMPROGRAMS\AMS\AMS.lnk" "$INSTDIR\AMSClient.exe" "" "$INSTDIR\AMSClient.exe" 0
  CreateShortCut "$DESKTOP\AMS.lnk" "$INSTDIR\AMSClient.exe" "" "$INSTDIR\AMSClient.exe" 0
SectionEnd ; end of default section

; Installs MSXML3 files if needed and registers them
Function InstallMSXML

  SetOutPath "$SYSDIR"

  ; check if we already have it on the system
  IfFileExists "$SYSDIR\msxml3.dll" MSXMLCheckVersion MSXMLCopyFiles

  MSXMLCopyFiles:
    DetailPrint "Copying MSXML3 install files..."
    SetOverwrite ifnewer
    File "msxml3.dll"
    File "msxml3r.dll"
    File "msxml3a.dll"
  
  MSXMLReg:
    DetailPrint "Registering MSXML3..."
    RegDLL "msxml3.dll"
    Goto MSXMLDone

  MSXMLCheckVersion:		; see if the version is old - if so go back to copy and register, otherwise end
    DetailPrint "Checking MSXML3 file version..."
    CompareDLLVersions /STOREFROM "msxml3.dll" "$SYSDIR\msxml3.dll" MSXMLCopyFiles MSXMLDone

  MSXMLDone:
    DetailPrint "MSXML3 installation complete."

FunctionEnd


Function CheckReqs
  StrCpy $1 "AMS requires the following system component(s) before installing.  Please upgrade these components before rerunning setup.$\n$\n"
  StrCpy $2 "0"

  IfFileExists "$SYSDIR\wininet.dll" IEVersionCheck IEVersionOld

  ; we need version 4.72.2106.5 - from IE 4.01
  IEVersionCheck:
    CompareDLLVersions /STOREFROM "wininet_fake_dll.dll" "$SYSDIR\wininet.dll" IEVersionOld RASVersionCheck

  IEVersionOld:
    StrCpy $1 "$1Microsoft Internet Explorer must be version 4.01 or newer.  Upgrade at http://www.microsoft.com/ie$\n"
    StrCpy $2 "1"

  RASVersionCheck:
    IfFileExists "$SYSDIR\ws2_32.dll" RASVersionDone RASVersionMissing

  RASVersionMissing:
    StrCpy $1 "$1Winsock 2.0 is required.  Upgrade at http://www.microsoft.com/windows95/downloads/contents/wuadmintools/s_wunetworkingtools/w95sockets2/default.asp"
    StrCpy $2 "1"

  RASVersionDone:

  ; see if we found any missing reqs
  StrCmp $2 "0" CheckReqsDone MissingReqFound

  MissingReqFound:
    MessageBox MB_ICONSTOP|MB_OK $1
    Abort "Your system does not meet the AMS system requirements."

  CheckReqsDone:

FunctionEnd

; ---------------------------------------------------------------------------------------

; begin uninstall settings/section
UninstallText "This will uninstall AMS for your system."
UninstallExeName "uninstams.exe"

Section Uninstall
; add delete commands to delete whatever files/registry keys/etc you installed here.
  Delete "$INSTDIR\uninstams.exe"
  Delete "$INSTDIR\AMSClient.exe"
  Delete "$INSTDIR\thumbs.db"
  DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\Line 2 Systems\Adult Media Swapper"
  DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Adult Media Swapper"
  RMDir "$INSTDIR"
  Delete "$SMPROGRAMS\AMS\AMS.lnk"
  RMDir "$SMPROGRAMS\AMS"
  Delete "$DESKTOP\AMS.lnk"
SectionEnd ; end of uninstall section

; eof

