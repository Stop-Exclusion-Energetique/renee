$scriptsFolder = $PSScriptRoot
$projectRoot = Resolve-Path -Path (Join-Path $scriptsFolder "..\..\..\")
$encryptionProjectPath = Resolve-Path -Path (Join-Path $projectRoot "\Renee.MigrationTool")

Write-Host "Encrypting files ..."
dotnet run --project $encryptionProjectPath -- encrypt all
