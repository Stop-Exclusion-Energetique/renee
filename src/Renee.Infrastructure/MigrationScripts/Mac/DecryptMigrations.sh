#!/bin/bash

scriptsFolder=$(dirname "$0")
projectRoot=$(realpath "$scriptsFolder/../../../")

encryptionProjectPath=$(realpath "$projectRoot/Renee.MigrationTool")
echo "Decrypting files ..."

dotnet run --project "$encryptionProjectPath" -- decrypt all