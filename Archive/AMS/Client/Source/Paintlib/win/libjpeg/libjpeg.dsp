# Microsoft Developer Studio Project File - Name="libjpeg" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Static Library" 0x0104

CFG=libjpeg - Win32 Release
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "libjpeg.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "libjpeg.mak" CFG="libjpeg - Win32 Release"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "libjpeg - Win32 Release" (based on "Win32 (x86) Static Library")
!MESSAGE "libjpeg - Win32 Debug" (based on "Win32 (x86) Static Library")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""$/AMS/Client", RRFAAAAA"
# PROP Scc_LocalPath "..\..\..\.."
CPP=cl.exe
RSC=rc.exe

!IF  "$(CFG)" == "libjpeg - Win32 Release"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir ".\Release"
# PROP BASE Intermediate_Dir ".\Release"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "..\..\..\..\Build\Release\Lib"
# PROP Intermediate_Dir "..\..\..\..\Intermediate\Release\Jpeg"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /YX /c
# ADD CPP /nologo /MT /W3 /GX /O2 /Ob2 /I "." /I "..\..\..\otherlib\libjpeg" /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /YX /c
# ADD BASE RSC /l 0x407
# ADD RSC /l 0x407
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LIB32=link.exe -lib
# ADD BASE LIB32 /nologo
# ADD LIB32 /nologo

!ELSEIF  "$(CFG)" == "libjpeg - Win32 Debug"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 1
# PROP BASE Output_Dir ".\Debug"
# PROP BASE Intermediate_Dir ".\Debug"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 1
# PROP Output_Dir "..\..\..\..\Build\Debug\Lib\"
# PROP Intermediate_Dir "..\..\..\..\Intermediate\Debug\Jpeg\"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /GX /Z7 /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /YX /c
# ADD CPP /nologo /MTd /W3 /GX /Z7 /Od /I "." /I "..\..\..\otherlib\libjpeg" /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /FR /YX /c
# ADD BASE RSC /l 0x407
# ADD RSC /l 0x407
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LIB32=link.exe -lib
# ADD BASE LIB32 /nologo
# ADD LIB32 /nologo

!ENDIF 

# Begin Target

# Name "libjpeg - Win32 Release"
# Name "libjpeg - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;hpj;bat;for;f90"
# Begin Source File

SOURCE=..\..\..\Jpeg\jcapimin.c
DEP_CPP_JCAPI=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jcapistd.c
DEP_CPP_JCAPIS=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jccoefct.c
DEP_CPP_JCCOE=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jccolor.c
DEP_CPP_JCCOL=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jcdctmgr.c
DEP_CPP_JCDCT=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jdct.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jchuff.c
DEP_CPP_JCHUF=\
	"..\..\..\Jpeg\jchuff.h"\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jcinit.c
DEP_CPP_JCINI=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jcmainct.c
DEP_CPP_JCMAI=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jcmarker.c
DEP_CPP_JCMAR=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jcmaster.c
DEP_CPP_JCMAS=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jcomapi.c
DEP_CPP_JCOMA=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jcparam.c
DEP_CPP_JCPAR=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jcphuff.c
DEP_CPP_JCPHU=\
	"..\..\..\Jpeg\jchuff.h"\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jcprepct.c
DEP_CPP_JCPRE=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jcsample.c
DEP_CPP_JCSAM=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jctrans.c
DEP_CPP_JCTRA=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdapimin.c
DEP_CPP_JDAPI=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdapistd.c
DEP_CPP_JDAPIS=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdatadst.c
DEP_CPP_JDATA=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdatasrc.c
DEP_CPP_JDATAS=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdcoefct.c
DEP_CPP_JDCOE=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdcolor.c
DEP_CPP_JDCOL=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jddctmgr.c
DEP_CPP_JDDCT=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jdct.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdhuff.c
DEP_CPP_JDHUF=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jdhuff.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdinput.c
DEP_CPP_JDINP=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdmainct.c
DEP_CPP_JDMAI=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdmarker.c
DEP_CPP_JDMAR=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdmaster.c
DEP_CPP_JDMAS=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdmerge.c
DEP_CPP_JDMER=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdphuff.c
DEP_CPP_JDPHU=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jdhuff.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdpostct.c
DEP_CPP_JDPOS=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdsample.c
DEP_CPP_JDSAM=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdtrans.c
DEP_CPP_JDTRA=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jerror.c
DEP_CPP_JERRO=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	"..\..\..\Jpeg\jversion.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jfdctflt.c
DEP_CPP_JFDCT=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jdct.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jfdctfst.c
DEP_CPP_JFDCTF=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jdct.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jfdctint.c
DEP_CPP_JFDCTI=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jdct.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jidctflt.c
DEP_CPP_JIDCT=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jdct.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jidctfst.c
DEP_CPP_JIDCTF=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jdct.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jidctint.c
DEP_CPP_JIDCTI=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jdct.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jidctred.c
DEP_CPP_JIDCTR=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jdct.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jmemmgr.c
DEP_CPP_JMEMM=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmemsys.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jmemnobs.c
DEP_CPP_JMEMN=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmemsys.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jquant1.c
DEP_CPP_JQUAN=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jquant2.c
DEP_CPP_JQUANT=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jutils.c
DEP_CPP_JUTIL=\
	"..\..\..\Jpeg\jconfig.h"\
	"..\..\..\Jpeg\jerror.h"\
	"..\..\..\Jpeg\jinclude.h"\
	"..\..\..\Jpeg\jmorecfg.h"\
	"..\..\..\Jpeg\jpegint.h"\
	"..\..\..\Jpeg\jpeglib.h"\
	
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl;fi;fd"
# Begin Source File

SOURCE=..\..\..\Jpeg\cderror.h
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\cdjpeg.h
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jchuff.h
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jconfig.h
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdct.h
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jdhuff.h
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jerror.h
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jinclude.h
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jmemsys.h
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jmorecfg.h
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jpegint.h
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jpeglib.h
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\jversion.h
# End Source File
# Begin Source File

SOURCE=..\..\..\Jpeg\transupp.h
# End Source File
# End Group
# Begin Group "Resource Files"

# PROP Default_Filter "ico;cur;bmp;dlg;rc2;rct;bin;cnt;rtf;gif;jpg;jpeg;jpe"
# End Group
# End Target
# End Project
