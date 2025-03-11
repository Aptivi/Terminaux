#!/bin/bash

# Repository root
ROOTDIR=$( cd -- "$( dirname -- "$0" )/.." &> /dev/null && pwd )

# Convenience functions
checkerror() {
    if [ $1 != 0 ]
    then
        printf "$2 - Error $1\n" >&2
        exit $1
    fi
}

# This script pushes.
releaseconf=$1
if [ -z $releaseconf ]; then
	releaseconf=Release
fi
nugetsource=$2
if [ -z $nugetsource ]; then
	nugetsource=nuget.org
fi
dotnetpath=`which dotnet`
checkerror $? "dotnet is not found"

# Push packages
echo Pushing packages...
find $ROOTDIR -type f -path "*/bin/$releaseconf/*.nupkg" -exec sh -c "echo {} ; dotnet nuget push {} --api-key $NUGET_APIKEY --source \"$nugetsource\"" \;
checkerror $? "Failed to push"

# Inform success
echo Push successful.
exit 0
