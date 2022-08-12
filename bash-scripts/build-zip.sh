#!/bin/bash

cd ../codex-client
echo "Creating React production build. . ."
npm run build
echo "Finished React production build"
cd ../CodexBackend
# Make sure pub folder exists
if [-d "../pub"]
then
    echo "pub folder exists"
else
    mkdir ../pub
fi
echo "Publishing .NET application. . ."
dotnet publish API -o ../pub 
cd ../pub
echo "Compressing .NET application to zip package. . ."
zip -r site.zip *
echo "Zip package created"