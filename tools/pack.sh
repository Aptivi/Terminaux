#!/bin/bash

#   Terminaux  Copyright (C) 2023  Aptivi
# 
#   This file is part of Terminaux
# 
#   Terminaux is free software: you can redistribute it and/or modify
#   it under the terms of the GNU General Public License as published by
#   the Free Software Foundation, either version 3 of the License, or
#   (at your option) any later version.
# 
#   Terminaux is distributed in the hope that it will be useful,
#   but WITHOUT ANY WARRANTY; without even the implied warranty of
#   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
#   GNU General Public License for more details.
# 
#   You should have received a copy of the GNU General Public License
#   along with this program.  If not, see <https://www.gnu.org/licenses/>.

# This script builds and packs the artifacts. Use when you have MSBuild installed.
version=$(cat version)
releaseconf=$1
if [ -z $releaseconf ]; then
	releaseconf=Release
fi

# Check for dependencies
zippath=`which zip`
if [ ! $? == 0 ]; then
	echo zip is not found.
	exit 1
fi

# Pack binary
echo Packing binary...
cd "../Terminaux/bin/$releaseconf/netstandard2.0/" && "$zippath" -r /tmp/$version-bin.zip . && cd -
cd "../Terminaux.ResizeListener/bin/$releaseconf/net48/" && "$zippath" -r /tmp/$version-bin-res-48.zip . && cd -
cd "../Terminaux.ResizeListener/bin/$releaseconf/net8.0/" && "$zippath" -r /tmp/$version-bin-res-8.zip . && cd -
if [ ! $? == 0 ]; then
	echo Packing using zip failed.
	exit 1
fi

# Inform success
mv /tmp/$version-bin.zip .
mv /tmp/$version-bin-res-48.zip .
mv /tmp/$version-bin-res-8.zip .
echo Build and pack successful.
exit 0
