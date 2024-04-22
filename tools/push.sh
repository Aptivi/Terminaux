#!/bin/bash
# This script pushes. Use when you have dotnet installed.
releaseconf=$1
if [ -z $releaseconf ]; then
	releaseconf=Release
fi

# Check for dependencies
dotnetpath=`which dotnet`
if [ ! $? == 0 ]; then
	echo dotnet is not found.
	exit 1
fi

# Push packages
echo Pushing packages...
find .. -type f -path "*/bin/$releaseconf/*.nupkg" -exec dotnet nuget push {} --api-key $NUGET_APIKEY --source "nuget.org" \;
if [ ! $? == 0 ]; then
	echo Push failed.
	exit 1
fi

# Inform success
echo Push successful.
exit 0
