#!/bin/bash

cd ../codex-client
npm run build
cd ../CodexBackend
# Make sure pub folder exists
if [-d "../pub"]
then
    echo "pub folder exists"
else
    mkdir ../pub
fi
dotnet publish API -o ../pub 
cd ../pub
zip -r site.zip *
echo "Zip package created"