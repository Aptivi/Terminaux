@echo off

REM    Terminaux  Copyright (C) 2018-2021  Aptivi
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

for /f "tokens=* USEBACKQ" %%f in (`type version`) do set version=%%f
set releaseconfig=%1
if "%releaseconfig%" == "" set releaseconfig=Release

:packbin
echo Packing binary...
"%ProgramFiles%\WinRAR\rar.exe" a -ep1 -r -m5 %temp%/%version%-bin.rar "..\Terminaux\bin\%releaseconfig%\netstandard2.0\"
"%ProgramFiles%\WinRAR\rar.exe" a -ep1 -r -m5 %temp%/%version%-demo.rar "..\Terminaux.Console\bin\%releaseconfig%\net6.0\"
if %errorlevel% == 0 goto :complete
echo There was an error trying to pack binary (%errorlevel%).
goto :finished

:complete
move %temp%\%version%-bin.rar
move %temp%\%version%-demo.rar

echo Pack successful.
:finished
