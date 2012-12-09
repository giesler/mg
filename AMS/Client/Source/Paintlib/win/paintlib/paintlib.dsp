# Microsoft Developer Studio Project File - Name="paintlib" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Static Library" 0x0104

CFG=paintlib - Win32 Release
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "paintlib.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "paintlib.mak" CFG="paintlib - Win32 Release"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "paintlib - Win32 Release" (based on "Win32 (x86) Static Library")
!MESSAGE "paintlib - Win32 Debug" (based on "Win32 (x86) Static Library")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""$/AMS/Client", RRFAAAAA"
# PROP Scc_LocalPath "..\..\..\.."
CPP=cl.exe
RSC=rc.exe

!IF  "$(CFG)" == "paintlib - Win32 Release"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir ".\Release"
# PROP BASE Intermediate_Dir ".\Release"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "..\..\..\..\Build\Release\Lib"
# PROP Intermediate_Dir "..\..\..\..\Intermediate\Release\Paintlib"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /YX /c
# ADD CPP /nologo /MT /W3 /GX /O2 /Ob2 /I "." /I "..\..\common" /I "..\..\common\filter" /I "..\libtiff" /I "..\..\..\otherlib\libtiff\libtiff" /I "..\libjpeg" /I "..\..\..\otherlib\libjpeg" /I "..\..\..\otherlib\libpng" /I "..\..\..\otherlib\zlib" /I "..\..\..\Jpeg" /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /Yu"stdpch.h" /c
# ADD BASE RSC /l 0x407
# ADD RSC /l 0x407
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LIB32=link.exe -lib
# ADD BASE LIB32 /nologo
# ADD LIB32 /nologo

!ELSEIF  "$(CFG)" == "paintlib - Win32 Debug"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 1
# PROP BASE Output_Dir ".\Debug"
# PROP BASE Intermediate_Dir ".\Debug"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 1
# PROP Output_Dir "..\..\..\..\Build\Debug\Lib\"
# PROP Intermediate_Dir "..\..\..\..\Intermediate\Debug\Paintlib\"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /GX /Z7 /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /YX /c
# ADD CPP /nologo /MTd /W3 /Gi /GX /ZI /Od /I "." /I "..\..\common" /I "..\..\common\filter" /I "..\libtiff" /I "..\..\..\otherlib\libtiff\libtiff" /I "..\libjpeg" /I "..\..\..\otherlib\libjpeg" /I "..\..\..\otherlib\libpng" /I "..\..\..\otherlib\zlib" /I "..\..\..\Jpeg" /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /FR /Yu"stdpch.h" /Yd /c
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

# Name "paintlib - Win32 Release"
# Name "paintlib - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;hpj;bat;for;f90"
# Begin Source File

SOURCE=..\..\Common\Anybmp.cpp
DEP_CPP_ANYBM=\
	"..\..\Common\anybmp.h"\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\Common\Anydec.cpp
DEP_CPP_ANYDE=\
	"..\..\Common\anydec.h"\
	"..\..\Common\bitmap.h"\
	"..\..\Common\bmpdec.h"\
	"..\..\Common\config.h"\
	"..\..\Common\datasrc.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\Common\jpegdec.h"\
	"..\..\common\Pcx.h"\
	"..\..\common\PcxDec.h"\
	"..\..\common\pgm.h"\
	"..\..\common\pgmdec.h"\
	"..\..\Common\picdec.h"\
	"..\..\Common\pictdec.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\pngdec.h"\
	"..\..\Common\qdraw.h"\
	"..\..\Common\stdpch.h"\
	"..\..\Common\tga.h"\
	"..\..\Common\tgadec.h"\
	"..\..\Common\tiffdec.h"\
	"..\..\Common\windefs.h"\
	".\wemfdec.h"\
	
NODEP_CPP_ANYDE=\
	"..\..\Common\jpeglib.h"\
	"..\..\common\png.h"\
	"..\..\Common\tiff.h"\
	"..\..\common\tiffio.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\Common\Bitmap.cpp
DEP_CPP_BITMA=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\Dither8bit.h"\
	"..\..\Common\except.h"\
	"..\..\common\Filter\FilterCrop.h"\
	"..\..\common\Filter\FilterGetAlpha.h"\
	"..\..\common\Filter\FilterGrayscale.h"\
	"..\..\common\Filter\FilterResize.h"\
	"..\..\common\Filter\FilterResizeBilinear.h"\
	"..\..\common\Filter\FilterResizeBox.h"\
	"..\..\common\Filter\FilterResizeGaussian.h"\
	"..\..\common\Filter\FilterResizeHamming.h"\
	"..\..\common\Filter\FilterRotate.h"\
	"..\..\common\Filter\VideoInvertFilter.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\Common\Bmpdec.cpp
DEP_CPP_BMPDE=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\bmpdec.h"\
	"..\..\Common\config.h"\
	"..\..\Common\datasrc.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\Common\picdec.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	"..\..\Common\windefs.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\Common\datasink.cpp
DEP_CPP_DATAS=\
	"..\..\Common\config.h"\
	"..\..\Common\datasink.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\common\PLObject.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\Common\Datasrc.cpp
DEP_CPP_DATASR=\
	"..\..\Common\config.h"\
	"..\..\Common\datasrc.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\common\PLObject.h"\
	"..\..\Common\prognot.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=.\dibsect.cpp
DEP_CPP_DIBSE=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	".\dibsect.h"\
	".\winbmp.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\Dither8bit.cpp
DEP_CPP_DITHE=\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\Dither8bit.h"\
	"..\..\common\Neuquant.h"\
	"..\..\common\PLObject.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\Common\Except.cpp
DEP_CPP_EXCEP=\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\common\PLObject.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\Common\filesink.cpp
DEP_CPP_FILES=\
	"..\..\Common\config.h"\
	"..\..\Common\datasink.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\Common\filesink.h"\
	"..\..\common\PLObject.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\Common\Filesrc.cpp
DEP_CPP_FILESR=\
	"..\..\Common\config.h"\
	"..\..\Common\datasrc.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\Common\filesrc.h"\
	"..\..\common\PLObject.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\Filter.cpp
DEP_CPP_FILTE=\
	"..\..\Common\anybmp.h"\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterCrop.cpp
DEP_CPP_FILTER=\
	"..\..\Common\anybmp.h"\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\Filter\FilterCrop.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterGetAlpha.cpp
DEP_CPP_FILTERG=\
	"..\..\Common\anybmp.h"\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\Filter\FilterGetAlpha.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterGrayscale.cpp
DEP_CPP_FILTERGR=\
	"..\..\Common\anybmp.h"\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\Filter\FilterGrayscale.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterResize.cpp
DEP_CPP_FILTERR=\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\Filter\FilterResize.h"\
	"..\..\common\PLObject.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterResizeBilinear.cpp
DEP_CPP_FILTERRE=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\Filter\2PassScale.h"\
	"..\..\common\Filter\2PSFilters.h"\
	"..\..\common\Filter\FilterResize.h"\
	"..\..\common\Filter\FilterResizeBilinear.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterResizeBox.cpp
DEP_CPP_FILTERRES=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\Filter\2PassScale.h"\
	"..\..\common\Filter\2PSFilters.h"\
	"..\..\common\Filter\FilterResize.h"\
	"..\..\common\Filter\FilterResizeBox.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterResizeGaussian.cpp
DEP_CPP_FILTERRESI=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\Filter\2PassScale.h"\
	"..\..\common\Filter\2PSFilters.h"\
	"..\..\common\Filter\FilterResize.h"\
	"..\..\common\Filter\FilterResizeGaussian.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterResizeHamming.cpp
DEP_CPP_FILTERRESIZ=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\Filter\2PassScale.h"\
	"..\..\common\Filter\2PSFilters.h"\
	"..\..\common\Filter\FilterResize.h"\
	"..\..\common\Filter\FilterResizeHamming.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterRotate.cpp
DEP_CPP_FILTERRO=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\Filter\FilterRotate.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\jmemdest.cpp
DEP_CPP_JMEMD=\
	"..\..\Common\config.h"\
	"..\..\Common\datasink.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\common\jmemdest.h"\
	"..\..\common\PLObject.h"\
	"..\..\Common\stdpch.h"\
	
NODEP_CPP_JMEMD=\
	"..\..\Common\jerror.h"\
	"..\..\Common\jinclude.h"\
	"..\..\Common\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\Common\Jmemsrc.c
DEP_CPP_JMEMS=\
	"..\..\Common\jmemsrc.h"\
	
NODEP_CPP_JMEMS=\
	"..\..\Common\jerror.h"\
	"..\..\Common\jinclude.h"\
	"..\..\Common\jpeglib.h"\
	
# SUBTRACT CPP /YX /Yc /Yu
# End Source File
# Begin Source File

SOURCE=..\..\Common\Jpegdec.cpp
DEP_CPP_JPEGD=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\Common\datasrc.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\Common\jmemsrc.h"\
	"..\..\Common\jpegdec.h"\
	"..\..\Common\picdec.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
NODEP_CPP_JPEGD=\
	"..\..\Common\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\jpegenc.cpp
DEP_CPP_JPEGE=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\Common\datasink.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\common\jmemdest.h"\
	"..\..\common\jpegenc.h"\
	"..\..\Common\Picenc.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
NODEP_CPP_JPEGE=\
	"..\..\Common\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\memsrc.cpp
DEP_CPP_MEMSR=\
	"..\..\Common\config.h"\
	"..\..\Common\datasrc.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\common\memsrc.h"\
	"..\..\common\PLObject.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\Neuquant.c
DEP_CPP_NEUQU=\
	"..\..\common\Neuquant.h"\
	
# SUBTRACT CPP /YX /Yc /Yu
# End Source File
# Begin Source File

SOURCE=..\..\common\PcxDec.cpp
DEP_CPP_PCXDE=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\Common\datasrc.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\common\Pcx.h"\
	"..\..\common\PcxDec.h"\
	"..\..\Common\picdec.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\pgmdec.cpp
DEP_CPP_PGMDE=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\Common\datasrc.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\common\pgm.h"\
	"..\..\common\pgmdec.h"\
	"..\..\Common\picdec.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\Common\Picdec.cpp
DEP_CPP_PICDE=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\Common\datasrc.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\Common\filesrc.h"\
	"..\..\common\memsrc.h"\
	"..\..\Common\picdec.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	".\ressrc.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\Common\picenc.cpp
DEP_CPP_PICEN=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\Common\datasink.h"\
	"..\..\Common\datasrc.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\Common\filesink.h"\
	"..\..\Common\picdec.h"\
	"..\..\Common\Picenc.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\Common\Pictdec.cpp
DEP_CPP_PICTD=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\Common\datasrc.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\Common\jpegdec.h"\
	"..\..\Common\optable.h"\
	"..\..\Common\picdec.h"\
	"..\..\Common\pictdec.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\qdraw.h"\
	"..\..\Common\stdpch.h"\
	
NODEP_CPP_PICTD=\
	"..\..\Common\jpeglib.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\PLObject.cpp
DEP_CPP_PLOBJ=\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\PLObject.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=.\ressrc.cpp
DEP_CPP_RESSR=\
	"..\..\Common\config.h"\
	"..\..\Common\datasrc.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\common\PLObject.h"\
	"..\..\Common\stdpch.h"\
	".\ressrc.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\Common\Stdpch.cpp
DEP_CPP_STDPC=\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\PLObject.h"\
	"..\..\Common\stdpch.h"\
	
# ADD CPP /Yc"stdpch.h"
# End Source File
# Begin Source File

SOURCE=..\..\Common\Tgadec.cpp
DEP_CPP_TGADE=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\Common\datasrc.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\Common\picdec.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	"..\..\Common\tga.h"\
	"..\..\Common\tgadec.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\Common\Trace.cpp
DEP_CPP_TRACE=\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\PLObject.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\VideoInvertFilter.cpp
DEP_CPP_VIDEO=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\common\Filter\VideoInvertFilter.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	
# End Source File
# Begin Source File

SOURCE=.\wemfdec.cpp
DEP_CPP_WEMFD=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\Common\datasrc.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\Common\filesrc.h"\
	"..\..\Common\picdec.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	".\wemfdec.h"\
	".\winbmp.h"\
	
# End Source File
# Begin Source File

SOURCE=.\winbmp.cpp
DEP_CPP_WINBM=\
	"..\..\Common\bitmap.h"\
	"..\..\Common\config.h"\
	"..\..\common\debug.h"\
	"..\..\Common\except.h"\
	"..\..\common\PLObject.h"\
	"..\..\common\plpoint.h"\
	"..\..\Common\stdpch.h"\
	".\winbmp.h"\
	
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl;fi;fd"
# Begin Source File

SOURCE=..\..\common\Filter\2PassScale.h
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\2PSFilters.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\anybmp.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\anydec.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\bitmap.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\bmpdec.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\config.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\datasink.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\datasrc.h
# End Source File
# Begin Source File

SOURCE=..\..\common\debug.h
# End Source File
# Begin Source File

SOURCE=.\dibsect.h
# End Source File
# Begin Source File

SOURCE=..\..\common\Dither8bit.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\except.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\filesink.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\filesrc.h
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\Filter.h
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterCrop.h
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterGetAlpha.h
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterGrayscale.h
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterResize.h
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterResizeBilinear.h
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterResizeBox.h
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterResizeGaussian.h
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterResizeHamming.h
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\FilterRotate.h
# End Source File
# Begin Source File

SOURCE=..\..\common\jmemdest.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\jmemsrc.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\jpegdec.h
# End Source File
# Begin Source File

SOURCE=..\..\common\jpegenc.h
# End Source File
# Begin Source File

SOURCE=..\..\common\memsrc.h
# End Source File
# Begin Source File

SOURCE=..\..\common\Neuquant.h
# End Source File
# Begin Source File

SOURCE=..\..\common\Pcx.h
# End Source File
# Begin Source File

SOURCE=..\..\common\PcxDec.h
# End Source File
# Begin Source File

SOURCE=..\..\common\pgm.h
# End Source File
# Begin Source File

SOURCE=..\..\common\pgmdec.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\picdec.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\Picenc.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\pictdec.h
# End Source File
# Begin Source File

SOURCE=..\..\common\PLObject.h
# End Source File
# Begin Source File

SOURCE=..\..\common\plpoint.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\prognot.h
# End Source File
# Begin Source File

SOURCE=.\ressrc.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\stdpch.h
# End Source File
# Begin Source File

SOURCE=..\..\Common\tgadec.h
# End Source File
# Begin Source File

SOURCE=..\..\common\Filter\VideoInvertFilter.h
# End Source File
# Begin Source File

SOURCE=.\wemfdec.h
# End Source File
# Begin Source File

SOURCE=.\winbmp.h
# End Source File
# End Group
# Begin Group "Resource Files"

# PROP Default_Filter "ico;cur;bmp;dlg;rc2;rct;bin;cnt;rtf;gif;jpg;jpeg;jpe"
# End Group
# End Target
# End Project
