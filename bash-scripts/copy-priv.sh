#!/bin/bash

# This copies local credential files to git-ignored paths inside the project. Useful for some git operations.
KEY_SRC="../../priv/api-keys.json"
KEY_DEST="../CodexBackend/ApiKeys"

CRED_SRC="../../priv/PrivateData.cs"
CRED_DEST="../CodexBackend/Domain/PrivateData"

mkdir -p $KEY_DEST
cp $KEY_SRC $KEY_DEST

mkdir -p $CRED_DEST
cp $CRED_SRC $CRED_DEST