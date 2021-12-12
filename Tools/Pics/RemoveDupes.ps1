
$baseFolder = "\\nas0\pictures\Store\"

Write-Host "Finding edited files..."

$editedFiles = Get-ChildItem $baseFolder -Recurse -Include "*-edited*"

Write-Host "Checking for unedited instances..."

$noMatch = [System.Collections.ArrayList]::new()

$editedFiles | ForEach-Object {
    $uneditedName = ($_.FullName).Replace("-edited", "")

    if (-not (Test-Path $uneditedName))
    {
        $uneditedName = ($_.FullName).Replace("-edited-2", "")
        if (-not (Test-Path $uneditedName))
        {
            $noMatch.Add($_.FullName)
        }
    }
}

Write-Host "Edited count $($editedFiles.Length), unmatched base files $($noMatch.Count)"

$noMatch | ForEach-Object {
    $destFile = $_.Replace($baseFolder, "")
    $destFile = $destFile.Replace("\", " - ").Trim()
    $destFile = Join-Path "\\nas0\pictures\noUneditedMatches\" $destFile
    Write-Host "Copy $_ to $destFile"
#    Copy-Item $_ $destFile -Verbose
#    Remove-Item $_ -WhatIf
}

Write-Host "Edited count $($editedFiles.Length), unmatched base files $($noMatch.Count)"
Write-Host "Done"