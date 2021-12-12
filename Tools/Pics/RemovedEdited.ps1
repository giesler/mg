$baseFolder = "\\nas0\pictures\Store\"

Write-Host "Finding edited files..."

$editedFiles = Get-ChildItem $baseFolder -Recurse -Include "*-edited*"

Write-Host "Removing $($editedFiles.Count) files..."

$editedFiles | ForEach-Object {
    Remove-Item -Path $_ -Force -Verbose
}