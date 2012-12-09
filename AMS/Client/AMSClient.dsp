# Microsoft Developer Studio Project File - Name="AMSClient" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Application" 0x0101

CFG=AMSClient - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "AMSClient.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "AMSClient.mak" CFG="AMSClient - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "AMSClient - Win32 Release" (based on "Win32 (x86) Application")
!MESSAGE "AMSClient - Win32 Debug" (based on "Win32 (x86) Application")
!MESSAGE "AMSClient - Win32 Distro" (based on "Win32 (x86) Application")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""$/AMS/Client", RRFAAAAA"
# PROP Scc_LocalPath "."
CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "AMSClient - Win32 Release"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Release"
# PROP BASE Intermediate_Dir "Release"
# PROP BASE Target_Dir ""
# PROP Use_MFC 1
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Build\Release"
# PROP Intermediate_Dir "Intermediate\Release\Client"
# PROP Ignore_Export_Lib 0
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /YX /FD /c
# ADD CPP /nologo /MT /W3 /GX /O2 /I "Source\Jpeg" /I "Source\xmlib" /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /D "_INTERNAL" /FR /YX /FD /c
# ADD BASE MTL /nologo /D "NDEBUG" /mktyplib203 /win32
# ADD MTL /nologo /D "NDEBUG" /mktyplib203 /win32
# ADD BASE RSC /l 0x409 /d "NDEBUG"
# ADD RSC /l 0x409 /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:windows /machine:I386
# ADD LINK32 paintlib.lib libjpeg.lib msxml2.lib rpcrt4.lib xmlib.lib /nologo /subsystem:windows /machine:I386 /libpath:"Build\Release\Lib"

!ELSEIF  "$(CFG)" == "AMSClient - Win32 Debug"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 1
# PROP BASE Output_Dir "Debug"
# PROP BASE Intermediate_Dir "Debug"
# PROP BASE Target_Dir ""
# PROP Use_MFC 1
# PROP Use_Debug_Libraries 1
# PROP Output_Dir "Build\Debug\"
# PROP Intermediate_Dir "Intermediate\Debug\Client"
# PROP Ignore_Export_Lib 0
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /YX /FD /GZ /c
# ADD CPP /nologo /MTd /W3 /Gm /GX /ZI /Od /I "Source\Jpeg" /I "Source\xmlib" /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /D "_INTERNAL" /FR /YX /FD /GZ /c
# ADD BASE MTL /nologo /D "_DEBUG" /mktyplib203 /win32
# ADD MTL /nologo /D "_DEBUG" /mktyplib203 /win32
# ADD BASE RSC /l 0x409 /d "_DEBUG"
# ADD RSC /l 0x409 /d "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:windows /debug /machine:I386 /pdbtype:sept
# ADD LINK32 paintlib.lib libjpeg.lib msxml2.lib rpcrt4.lib xmlib.lib /nologo /subsystem:windows /debug /machine:I386 /libpath:"Build\Debug\Lib"
# SUBTRACT LINK32 /profile

!ELSEIF  "$(CFG)" == "AMSClient - Win32 Distro"

# PROP BASE Use_MFC 1
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "AMSClient___Win32_Distro"
# PROP BASE Intermediate_Dir "AMSClient___Win32_Distro"
# PROP BASE Ignore_Export_Lib 0
# PROP BASE Target_Dir ""
# PROP Use_MFC 1
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Build\Distro"
# PROP Intermediate_Dir "Intermediate\Distro\Client"
# PROP Ignore_Export_Lib 0
# PROP Target_Dir ""
# ADD BASE CPP /nologo /MT /W3 /GX /O2 /I "Source\Jpeg" /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /YX /FD /c
# ADD CPP /nologo /MT /W3 /GX /O2 /I "Source\Jpeg" /I "Source\xmlib" /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /D "_DISTRO" /FR /YX /FD /c
# ADD BASE MTL /nologo /D "NDEBUG" /mktyplib203 /win32
# ADD MTL /nologo /D "NDEBUG" /mktyplib203 /win32
# ADD BASE RSC /l 0x409 /d "NDEBUG"
# ADD RSC /l 0x409 /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 paintlib.lib libjpeg.lib msxml2.lib rpcrt4.lib /nologo /subsystem:windows /machine:I386 /libpath:"Build\Release\Lib"
# ADD LINK32 paintlib.lib libjpeg.lib msxml2.lib rpcrt4.lib xmlib.lib /nologo /subsystem:windows /machine:I386 /libpath:"Build\Release\Lib"

!ENDIF 

# Begin Target

# Name "AMSClient - Win32 Release"
# Name "AMSClient - Win32 Debug"
# Name "AMSClient - Win32 Distro"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Source File

SOURCE=.\Source\StdAfx.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\xmclient.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\xmgui.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\xmguibrowsing.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\xmguicompleted.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\xmguilocal.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\xmguiquery.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\xmguisearch.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\xmguishared.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\xmguistatus.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\xmpipeline.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\xmpipelineclient.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\xmpipelineserver.cpp
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# Begin Source File

SOURCE=.\Source\paintlib.h
# End Source File
# Begin Source File

SOURCE=.\Source\StdAfx.h
# End Source File
# Begin Source File

SOURCE=.\Source\xmclient.h
# End Source File
# Begin Source File

SOURCE=.\Source\xmgui.h
# End Source File
# Begin Source File

SOURCE=.\Source\xmpipeline.h
# End Source File
# End Group
# Begin Group "Resource Files"

# PROP Default_Filter "ico;cur;bmp;dlg;rc2;rct;bin;rgs;gif;jpg;jpeg;jpe"
# Begin Source File

SOURCE=.\Source\Resources\handclose.cur
# End Source File
# Begin Source File

SOURCE=.\Source\Resources\HandOpen.cur
# End Source File
# Begin Source File

SOURCE=.\Source\Resources\Logo_170x158.bmp
# End Source File
# Begin Source File

SOURCE=.\Source\Resources\Logo_60x60.bmp
# End Source File
# Begin Source File

SOURCE=.\Source\Resources\Logo_80x80.bmp
# End Source File
# Begin Source File

SOURCE=.\Source\Resources\resource.h
# End Source File
# Begin Source File

SOURCE=.\Source\Resources\search_tools.bmp
# End Source File
# Begin Source File

SOURCE=.\Source\Resources\Shrink_Down.bmp
# End Source File
# Begin Source File

SOURCE=.\Source\Resources\Shrink_Left.bmp
# End Source File
# Begin Source File

SOURCE=.\Source\Resources\Tabs.bmp
# End Source File
# Begin Source File

SOURCE=.\Source\Resources\Thumbnail_Downloading.bmp
# End Source File
# Begin Source File

SOURCE=.\Source\Resources\Thumbnail_Error.bmp
# End Source File
# Begin Source File

SOURCE=.\Source\Resources\Thumbnail_Waiting.bmp
# End Source File
# Begin Source File

SOURCE=.\Source\Resources\vsplit.cur
# End Source File
# Begin Source File

SOURCE=.\Source\Resources\XMClient.ico
# End Source File
# Begin Source File

SOURCE=.\Source\Resources\XMClient.rc
# End Source File
# End Group
# End Target
# End Project
