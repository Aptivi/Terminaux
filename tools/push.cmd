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

set apikey=%1

REM This script pushes. Use when you have VS installed.
echo Pushing...
cmd /C "forfiles /s /m *.nupkg /p ..\ /C "cmd /c dotnet nuget push @path --api-key %apikey% --source nuget.org""
if %errorlevel% == 0 goto :success
echo There was an error trying to push (%errorlevel%).
goto :finished

:success
echo Push successful.
:finished
