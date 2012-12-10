@echo off
cls
@echo off
echo Removing old copies...
del /F %windir%\msn2.* > NUL
del /F %windir%\msn2picture* > NUL
del /F %windir%\system32\msn2.* > NUL
del /F %windir%\system32\msn2picture* > NUL
copy /Y \\sp\data\software\msn2.net\PictureScreenSaver\msn2* %windir%
rename %windir%\msn2PicturesScreenSaver.exe msn2PicturesScreenSaver.scr