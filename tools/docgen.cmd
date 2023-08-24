@echo off

REM    Terminaux  Copyright (C) 2018-2022  Aptivi
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

REM This script builds documentation and packs the artifacts.

echo Finding DocFX...
if exist %ProgramData%\chocolatey\bin\docfx.exe goto :build
echo You don't have DocFX installed. Download and install Chocolatey and DocFX.
goto :finished

:build
echo Building Documentation...
%ProgramData%\chocolatey\bin\docfx.exe "..\DocGen\docfx.json"
if %errorlevel% == 0 goto :success
echo There was an error trying to build documentation (%errorlevel%).
goto :finished

:success
echo Build and pack successful.
:finished
