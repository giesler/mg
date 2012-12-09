# Microsoft Developer Studio Project File - Name="exehead" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Application" 0x0101

CFG=exehead - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "exehead.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "exehead.mak" CFG="exehead - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "exehead - Win32 Release" (based on "Win32 (x86) Application")
!MESSAGE "exehead - Win32 Debug" (based on "Win32 (x86) Application")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""
# PROP Scc_LocalPath ""
CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "exehead - Win32 Release"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Release"
# PROP BASE Intermediate_Dir "Release"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Release"
# PROP Intermediate_Dir "Release"
# PROP Ignore_Export_Lib 0
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /YX /FD /c
# ADD CPP /nologo /W3 /GX /O1 /Oy /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /D "EXEHEAD" /FD /c
# SUBTRACT CPP /YX
# ADD BASE MTL /nologo /D "NDEBUG" /mktyplib203 /win32
# ADD MTL /nologo /D "NDEBUG" /mktyplib203 /win32
# ADD BASE RSC /l 0x409 /d "NDEBUG"
# ADD RSC /l 0x409 /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:windows /machine:I386
# ADD LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib version.lib /nologo /entry:"WinMain" /subsystem:windows /pdb:none /machine:I386 /nodefaultlib /opt:nowin98
# Begin Special Build Tool
SOURCE="$(InputPath)"
PostBuild_Desc=generating include file for makenssi
PostBuild_Cmds=bin2h Release\exehead.exe Release\exehead.h header_data	bin2h bitmap1.bmp Release\bitmap1.h bitmap1_data	bin2h bitmap2.bmp Release\bitmap2.h bitmap2_data	bin2h nsis.ico Release\icon.h icon_data	bin2h uninst.ico Release\unicon.h unicon_data
# End Special Build Tool

!ELSEIF  "$(CFG)" == "exehead - Win32 Debug"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 1
# PROP BASE Output_Dir "Debug"
# PROP BASE Intermediate_Dir "Debug"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 1
# PROP Output_Dir "Debug"
# PROP Intermediate_Dir "Debug"
# PROP Ignore_Export_Lib 0
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /YX /FD /GZ /c
# ADD CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /YX /FD /GZ /c
# ADD BASE MTL /nologo /D "_DEBUG" /mktyplib203 /win32
# ADD MTL /nologo /D "_DEBUG" /mktyplib203 /win32
# ADD BASE RSC /l 0x409 /d "_DEBUG"
# ADD RSC /l 0x409 /d "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:windows /debug /machine:I386 /pdbtype:sept
# ADD LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib  version.lib /nologo /subsystem:windows /debug /machine:I386 /pdbtype:sept

!ENDIF 

# Begin Target

# Name "exehead - Win32 Release"
# Name "exehead - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Group "zlib"

# PROP Default_Filter ""
# Begin Group "headers"

# PROP Default_Filter ""
# Begin Source File

SOURCE=..\zlib\Infblock.h
# End Source File
# Begin Source File

SOURCE=..\zlib\Infcodes.h
# End Source File
# Begin Source File

SOURCE=..\zlib\Inftrees.h
# End Source File
# Begin Source File

SOURCE=..\zlib\Infutil.h
# End Source File
# Begin Source File

SOURCE=..\zlib\Zconf.h
# End Source File
# Begin Source File

SOURCE=..\zlib\Zlib.h
# End Source File
# End Group
# Begin Source File

SOURCE=..\zlib\ADLER32.C
# End Source File
# Begin Source File

SOURCE=..\zlib\INFBLOCK.C
# End Source File
# Begin Source File

SOURCE=..\zlib\INFCODES.C
# End Source File
# Begin Source File

SOURCE=..\zlib\INFLATE.C
# End Source File
# Begin Source File

SOURCE=..\zlib\INFTREES.C
# End Source File
# Begin Source File

SOURCE=..\zlib\INFUTIL.C
# End Source File
# End Group
# Begin Source File

SOURCE=.\bgbg.c
# End Source File
# Begin Source File

SOURCE=.\exec.c
# End Source File
# Begin Source File

SOURCE=.\fileform.c
# End Source File
# Begin Source File

SOURCE=.\libc.c

!IF  "$(CFG)" == "exehead - Win32 Release"

# ADD CPP /GB /Zp8

!ELSEIF  "$(CFG)" == "exehead - Win32 Debug"

!ENDIF 

# End Source File
# Begin Source File

SOURCE=.\Main.c
# End Source File
# Begin Source File

SOURCE=.\Ui.c
# End Source File
# Begin Source File

SOURCE=.\util.c
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# Begin Source File

SOURCE=.\config.h
# End Source File
# Begin Source File

SOURCE=.\exec.h
# End Source File
# Begin Source File

SOURCE=.\fileform.h
# End Source File
# Begin Source File

SOURCE=.\libc.h
# End Source File
# Begin Source File

SOURCE=.\state.h
# End Source File
# Begin Source File

SOURCE=.\ui.h
# End Source File
# Begin Source File

SOURCE=.\util.h
# End Source File
# End Group
# Begin Group "Resource Files"

# PROP Default_Filter "ico;cur;bmp;dlg;rc2;rct;bin;rgs;gif;jpg;jpeg;jpe"
# Begin Source File

SOURCE=.\bitmap1.bmp
# End Source File
# Begin Source File

SOURCE=.\bitmap2.bmp
# End Source File
# Begin Source File

SOURCE=.\icon3.ico
# End Source File
# Begin Source File

SOURCE=.\nsis.ico
# End Source File
# Begin Source File

SOURCE=.\nullsoft.ico
# End Source File
# Begin Source File

SOURCE=.\resource.h
# End Source File
# Begin Source File

SOURCE=.\resource.rc
# End Source File
# Begin Source File

SOURCE=.\uninst.ico
# End Source File
# End Group
# End Target
# End Project
