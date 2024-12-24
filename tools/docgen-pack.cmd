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

REM This script builds documentation and packs the artifacts. Use when you have VS installed.
for /f "tokens=*" %%g in ('findstr "<Version>" ..\Directory.Build.props') do (set MIDVER=%%g)
for /f "tokens=1 delims=<" %%a in ("%MIDVER:~9%") do (set ksversion=%%a)

:pack
echo Packing documentation...
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%ksversion%-doc.zip "..\docs\*"
if %errorlevel% == 0 goto :finalize
echo There was an error trying to pack documentation (%errorlevel%).
goto :finished

:finalize
rmdir /S /Q "..\DocGen\api\"
rmdir /S /Q "..\DocGen\obj\"
rmdir /S /Q "..\docs\"
move %temp%\%ksversion%-doc.zip
echo Build and pack successful.
goto :finished

:finished
