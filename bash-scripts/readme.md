# Bash Scripts

This folder is home to the various bash scripts I use to automate development tasks.

## build-zip.sh

Publishes the API as a .NET app to `../pub` and compresses it into `../pub/site.zip`.

## publish-to-azure.sh

Runs Azure CLI commands to deploy `../pub/site.zip` to the Codex app service.

## generate-priv.sh

Generates a folder with boilerplate text files for safely storing private data like passwords and API keys.

## copy-priv.sh

Copies each of the files created by `./generate-priv.sh` to the appropriate ignored path inside this repository.
