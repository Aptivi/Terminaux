#!/bin/bash

build() {
    # Check for dependencies
    dotnetpath=`which dotnet`
    checkerror $? "dotnet is not found"

    # Turn off telemetry and logo
    export DOTNET_CLI_TELEMETRY_OPTOUT=1
    export DOTNET_NOLOGO=1
    
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
    packages=()
    while IFS= read -r pkg; do
        packages+=("$pkg")
    done < <(find "$ROOTDIR" -type f -path "*/bin/$releaseconf/*.nupkg")
    for pkg in "${packages[@]}"; do
        echo "$pkg"
        dotnet nuget push "$pkg" --api-key "$NUGET_APIKEY" --source "$nugetsource"
        push_result=$?
        if [ $push_result -ne 0 ]; then
            checkvendorerror $push_result
            return $push_result
        fi
    done
    
    checkvendorerror $?
}

increment() {
    # Check the versions
    OLDVER=$1
    NEWVER=$2
    if [ -z $OLDVER ]; then
        printf "Old version must be specified.\n"
        exit 1
    fi
    if [ -z $NEWVER ]; then
        printf "New version must be specified to replace old version $OLDVER.\n"
        exit 1
    fi

    # Populate some of the files needed to replace the old version with the new version
    FILES=(
        "$ROOTDIR/Directory.Build.props"
        "$ROOTDIR/CHANGES.TITLE"
    )
    for FILE in "${FILES[@]}"; do
        printf "Processing $FILE...\n"
        sed -b -i "s/$OLDVER/$NEWVER/g" "$FILE"
        result=$?
        if [ $result -ne 0 ]; then
            checkvendorerror $result
            return $result
        fi
    done
}

clean() {
    OUTPUTS=(
        '-name "bin" -or'
        '-name "obj" -or'
        '-name "docs"'
    )
    find "$ROOTDIR" -type d \( ${OUTPUTS[@]} \) -print -exec rm -rf "{}" +
}
