@ECHO OFF

if "%EMULATED%"=="true" GOTO End

SETLOCAL
%~d0
CD "%~dp0"

.\tfs_ObjectModel.exe /Passive
GOTO End

:End