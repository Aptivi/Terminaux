@echo off

REM This script builds the documentation and packs the artifacts. Use when you have VS installed.
for /f "tokens=* USEBACKQ" %%f in (`type version`) do set ksversion=%%f

echo Finding DocFX...
if exist %USERPROFILE%\.dotnet\tools\docfx.exe goto :build
echo You don't have DocFX installed. Download and install .NET and DocFX.
goto :finished

:build
echo Building the documentation...
%USERPROFILE%\.dotnet\tools\docfx.exe "..\DocGen\docfx.json"
if %errorlevel% == 0 goto :success
echo There was an error trying to build documentation (%errorlevel%).
goto :finished

:success
echo Build and pack successful.
:finished
