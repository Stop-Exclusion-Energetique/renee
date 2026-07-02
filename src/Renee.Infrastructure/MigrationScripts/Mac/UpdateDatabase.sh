#!/bin/bash

scriptsFolder=$(dirname "$0")
projectRoot=$(realpath "$scriptsFolder/../../../")
startupProjectPath=$(realpath "$projectRoot/Renee.UI")
dbProjectPath=$(realpath "$projectRoot/Renee.Infrastructure")
encryptionProjectPath=$(realpath "$projectRoot/Renee.MigrationTool")

echo "Decrypting files ..."

dotnet run --project "$encryptionProjectPath" -- decrypt all
dotnet-ef database update --project "$dbProjectPath" --startup-project "$startupProjectPath"
dotnet run --project "$encryptionProjectPath" -- clear all