#!/bin/bash

scriptsFolder=$(dirname "$0")
projectRoot=$(realpath "$scriptsFolder/../../../")

encryptionProjectPath=$(realpath "$projectRoot/Renee.MigrationTool")
echo "Encrypting files ..."

dotnet run --project "$encryptionProjectPath" -- encrypt all