@echo off

REM    Terminaux  Copyright (C) 2023-2025  Aptivi
REM
REM    This file is part of Terminaux
REM
REM    Terminaux is free software: you can redistribute it and/or modify
REM    it under the terms of the GNU General Public License as published by
REM    the Free Software Foundation, either version 3 of the License, or
REM    (at your option) any later version.
REM
REM    Terminaux is distributed in the hope that it will be useful,
REM    but WITHOUT ANY WARRANTY; without even the implied warranty of
REM    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
REM    GNU General Public License for more details.
REM
REM    You should have received a copy of the GNU General Public License
REM    along with this program.  If not, see <https://www.gnu.org/licenses/>.

REM This script builds and packs the artifacts. Use when you have VS installed.
set releaseconfig=%1
if "%releaseconfig%" == "" set releaseconfig=Release

:download
echo Downloading packages...
"%ProgramFiles%\dotnet\dotnet.exe" restore "..\Terminaux.sln" -p:Configuration=%releaseconfig%
if %errorlevel% == 0 goto :build
echo There was an error trying to download packages (%errorlevel%).
goto :finished

:build
echo Building Terminaux...
"%ProgramFiles%\dotnet\dotnet.exe" build "..\Terminaux.sln" -p:Configuration=%releaseconfig%
if %errorlevel% == 0 goto :success
echo There was an error trying to build (%errorlevel%).
goto :finished

:success
echo Build successful.
:finished
