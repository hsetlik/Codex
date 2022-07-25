#!/bin/bash

cd CodexBackend
dotnet publish API -o ../pub 
cd ../pub
zip -r site.zip *