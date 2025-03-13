#!/bin/bash

build() {
    # Determine the release configuration
    releaseconf=$1
    if [ -z $releaseconf ]; then
	    releaseconf=Release
    fi

    # Now, build.
    echo Building with configuration $releaseconf...
    "$dotnetpath" build "$ROOTDIR/Terminaux.sln" -p:Configuration=$releaseconf ${@:2}
    checkvendorerror $?
}

docpack() {
    # Get the project version
    version=$(grep "<Version>" $ROOTDIR/Directory.Build.props | cut -d "<" -f 2 | cut -d ">" -f 2)
    checkerror $? "Failed to get version. Check to make sure that the version is specified correctly in D.B.props"

    # Check for dependencies
    zippath=`which zip`
    checkerror $? "zip is not found"

    # Pack documentation
    echo Packing documentation...
    cd "$ROOTDIR/docs/" && "$zippath" -r /tmp/$version-doc.zip . && cd -
    checkvendorerror $?

    # Clean things up
    rm -rf "$ROOTDIR/DocGen/api"
    checkvendorerror $?
    rm -rf "$ROOTDIR/DocGen/obj"
    checkvendorerror $?
    rm -rf "$ROOTDIR/docs"
    checkvendorerror $?
    mv /tmp/$version-doc.zip "$ROOTDIR/tools"
    checkvendorerror $?
}

docgenerate() {
    # Check for dependencies
    docfxpath=`which docfx`
    checkerror $? "docfx is not found"

    # Turn off telemetry and logo
    export DOTNET_CLI_TELEMETRY_OPTOUT=1
    export DOTNET_NOLOGO=1

    # Build docs
    echo Building documentation...
    "$docfxpath" $ROOTDIR/DocGen/docfx.json
    checkvendorerror $?
}

pushall() {
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
    echo Pushing packages with configuration $releaseconf to $nugetsource...
    find $ROOTDIR -type f -path "*/bin/$releaseconf/*.nupkg" -exec sh -c "echo {} ; dotnet nuget push {} --api-key $NUGET_APIKEY --source \"$nugetsource\"" \;
    checkvendorerror $?
}
