@echo off

set ROOTDIR=%~dp0..

echo Cleaning up...
for %%f in (bin, obj, docs) do forfiles /s /m %%f /p %ROOTDIR%\ /C "cmd /c echo @path && rd /s /q @path"
