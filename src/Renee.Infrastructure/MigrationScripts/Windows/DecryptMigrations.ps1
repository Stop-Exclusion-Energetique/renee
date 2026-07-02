$scriptsFolder = $PSScriptRoot

echo $scriptsFolder


$projectRoot = Resolve-Path -Path (Join-Path $scriptsFolder "..\..\..\")
$encryptionProjectPath = Resolve-Path -Path (Join-Path $projectRoot "\Renee.MigrationTool")

Write-Host "Decrypting files ..."

dotnet run --project $encryptionProjectPath -- decrypt all