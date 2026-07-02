$scriptsFolder = $PSScriptRoot
$projectRoot = Resolve-Path -Path (Join-Path $scriptsFolder "..\..\..\")
$startupProjectPath = Resolve-Path -Path (Join-Path $projectRoot "\Renee.UI")
$dbProjectPath = Resolve-Path -Path (Join-Path $projectRoot "\Renee.Infrastructure")
$encryptionProjectPath = Resolve-Path -Path (Join-Path $projectRoot "\Renee.MigrationTool")

Write-Host "Decrypting files ..."

dotnet run --project "$encryptionProjectPath" -- decrypt all
dotnet-ef database update --project "$dbProjectPath" --startup-project "$startupProjectPath"
dotnet run --project "$encryptionProjectPath" -- clear all