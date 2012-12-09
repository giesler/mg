; NOTE: this .NSI script is designed for NSIS v1.3+

Name "Adult Media Swapper Update"
OutFile "AMSUpdate_080.exe"
Icon "amsinst.ico"

Function .onInit

  OpenStatus "AMS Update" "Please wait..."

  ; check to make sure registry entry is set (set by installer)
  ReadRegStr $INSTDIR HKEY_LOCAL_MACHINE "SOFTWARE\AMS" ""
  StrCmp $INSTDIR "" ProgramNotFound
  
  ; wait for AMS to close
  UpdateStatus "Waiting for Adult Media Swapper to close..."
  WaitForMutex "ams_mutex"
  UpdateStatus "Preparing to update..."
  Sleep 3000

  ; update files
  UpdateStatus "Updating program..."
  SetOverwrite "on"
  SetOutPath "$INSTDIR"
  File "AMSClient.exe"

  ; done, launch program
  UpdateStatus "Restarting AMS..."
  Sleep 500
  Exec '"$INSTDIR\AMSClient.exe"'
  CloseStatus
  Delete "$EXEDIR\AMSUpdate_080.exe"
  Abort "Install Complete"

ProgramNotFound:
  MessageBox MB_ICONSTOP|MB_OK "Adult Media Swapper was not found on your system.  Please install Adult Media Swapper from http://www.adultmediaswapper.com."

  Abort "AMS must be installed before running the AMS update program."
  CloseStatus

FunctionEnd

Section "" ; to make compiler happy
SectionEnd
