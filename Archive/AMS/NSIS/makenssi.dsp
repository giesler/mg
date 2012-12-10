# Microsoft Developer Studio Project File - Name="makenssi" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Console Application" 0x0103

CFG=makenssi - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "makenssi.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "makenssi.mak" CFG="makenssi - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "makenssi - Win32 Release" (based on "Win32 (x86) Console Application")
!MESSAGE "makenssi - Win32 Debug" (based on "Win32 (x86) Console Application")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""$/AMS/NSIS", YJGAAAAA"
# PROP Scc_LocalPath "."
CPP=cl.exe
RSC=rc.exe

!IF  "$(CFG)" == "makenssi - Win32 Release"

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
# ADD BASE CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_CONSOLE" /D "_MBCS" /YX /FD /c
# ADD CPP /nologo /W3 /O2 /Ob2 /D "WIN32" /D "NDEBUG" /D "_CONSOLE" /D "_MBCS" /FR /FD /c
# SUBTRACT CPP /YX
# ADD BASE RSC /l 0x409 /d "NDEBUG"
# ADD RSC /l 0x409 /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:console /machine:I386
# ADD LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib version.lib /nologo /subsystem:console /pdb:none /machine:I386 /out:"Release\makensis.exe" /opt:nowin98

!ELSEIF  "$(CFG)" == "makenssi - Win32 Debug"

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
# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_CONSOLE" /D "_MBCS" /YX /FD /GZ /c
# ADD CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_CONSOLE" /D "_MBCS" /YX /FD /GZ /c
# ADD BASE RSC /l 0x409 /d "_DEBUG"
# ADD RSC /l 0x409 /d "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:console /debug /machine:I386 /pdbtype:sept
# ADD LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib version.lib /nologo /subsystem:console /debug /machine:I386 /pdbtype:sept

!ENDIF 

# Begin Target

# Name "makenssi - Win32 Release"
# Name "makenssi - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Group "zlib"

# PROP Default_Filter ""
# Begin Group "headers"

# PROP Default_Filter ""
# Begin Source File

SOURCE=.\zlib\Deflate.h
# End Source File
# Begin Source File

SOURCE=.\zlib\Infblock.h
# End Source File
# Begin Source File

SOURCE=.\zlib\Infcodes.h
# End Source File
# Begin Source File

SOURCE=.\zlib\Inftrees.h
# End Source File
# Begin Source File

SOURCE=.\zlib\Infutil.h
# End Source File
# Begin Source File

SOURCE=.\zlib\Zconf.h
# End Source File
# Begin Source File

SOURCE=.\zlib\Zlib.h
# End Source File
# Begin Source File

SOURCE=.\zlib\Zutil.h
# End Source File
# End Group
# Begin Source File

SOURCE=.\zlib\Adler32.c

!IF  "$(CFG)" == "makenssi - Win32 Release"

# ADD CPP /G6

!ELSEIF  "$(CFG)" == "makenssi - Win32 Debug"

!ENDIF 

# End Source File
# Begin Source File

SOURCE=.\zlib\deflate.c

!IF  "$(CFG)" == "makenssi - Win32 Release"

# ADD CPP /G6

!ELSEIF  "$(CFG)" == "makenssi - Win32 Debug"

!ENDIF 

# End Source File
# Begin Source File

SOURCE=.\zlib\trees.c

!IF  "$(CFG)" == "makenssi - Win32 Release"

# ADD CPP /G6

!ELSEIF  "$(CFG)" == "makenssi - Win32 Debug"

!ENDIF 

# End Source File
# End Group
# Begin Source File

SOURCE=.\build.cpp

!IF  "$(CFG)" == "makenssi - Win32 Release"

# ADD CPP /G6

!ELSEIF  "$(CFG)" == "makenssi - Win32 Debug"

!ENDIF 

# End Source File
# Begin Source File

SOURCE=.\exedata.cpp

!IF  "$(CFG)" == "makenssi - Win32 Release"

# ADD CPP /G6

!ELSEIF  "$(CFG)" == "makenssi - Win32 Debug"

!ENDIF 

# End Source File
# Begin Source File

SOURCE=.\makenssi.cpp

!IF  "$(CFG)" == "makenssi - Win32 Release"

# ADD CPP /G6

!ELSEIF  "$(CFG)" == "makenssi - Win32 Debug"

!ENDIF 

# End Source File
# Begin Source File

SOURCE=.\script.cpp

!IF  "$(CFG)" == "makenssi - Win32 Release"

# ADD CPP /Ot /Ow /Oy
# SUBTRACT CPP /Og

!ELSEIF  "$(CFG)" == "makenssi - Win32 Debug"

!ENDIF 

# End Source File
# Begin Source File

SOURCE=.\tokens.cpp
# End Source File
# Begin Source File

SOURCE=.\util.cpp

!IF  "$(CFG)" == "makenssi - Win32 Release"

# ADD CPP /G6

!ELSEIF  "$(CFG)" == "makenssi - Win32 Debug"

!ENDIF 

# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# Begin Source File

SOURCE=.\build.h
# End Source File
# Begin Source File

SOURCE=.\exedata.h
# End Source File
# Begin Source File

SOURCE=.\lineparse.h
# End Source File
# Begin Source File

SOURCE=.\strlist.h
# End Source File
# Begin Source File

SOURCE=.\tokens.h
# End Source File
# Begin Source File

SOURCE=.\util.h
# End Source File
# End Group
# Begin Group "Resource Files"

# PROP Default_Filter "ico;cur;bmp;dlg;rc2;rct;bin;rgs;gif;jpg;jpeg;jpe"
# Begin Source File

SOURCE=.\icon.ico
# End Source File
# Begin Source File

SOURCE=.\Script1.rc
# End Source File
# End Group
# Begin Source File

SOURCE=..\makensis.htm
# End Source File
# Begin Source File

SOURCE=..\makensis.nsi
# End Source File
# End Target
# End Project
