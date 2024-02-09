##########################################################
# Script that generates known terminfo capabilities
##########################################################

$Output = Join-Path $PSScriptRoot ".temp"
$Source = Join-Path $PSScriptRoot "/../src/Terminaux.TermInfo"

if(!(Test-Path $Output -PathType Container)) {
    New-Item -ItemType Directory -Path $Output | Out-Null
}

# Generate the files
Push-Location (Join-Path $PSScriptRoot "/../src/Terminaux.TermInfo.Cli")
&dotnet run generate "$Output" --input $Output
if(!$?) { 
    Pop-Location
    Throw "An error occured when generating code."
}
Pop-Location

# Copy the files to the correct location
Copy-Item  (Join-Path "$Output" "TermInfoCaps.cs") -Destination "$Source/TermInfoCaps.cs"
Copy-Item  (Join-Path "$Output" "TermInfoDesc.Generated.cs") -Destination "$Source/TermInfoDesc.Generated.cs"
