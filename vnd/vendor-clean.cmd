@echo off

set ROOTDIR=%~dp0..

echo Cleaning up...
for %%f in (bin, obj, docs, Generated) do forfiles /s /m %%f /p "%ROOTDIR%" /c "cmd /c if @isdir==TRUE (echo @path && rd /s /q @path)" 2>nul
