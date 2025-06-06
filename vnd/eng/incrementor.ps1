$targetFile = $args[0]
$oldVersion = $args[1]
$newVersion = $args[2]

$contents = [System.IO.File]::ReadAllText($targetFile).Replace($oldVersion, $newVersion)
[System.IO.File]::WriteAllText($targetFile, $contents)
