#!/bin/bash

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