#!/bin/bash

KEY_SRC="../../codex-priv/codextranslation-69b55f865606.json"
KEY_DEST="../CodexBackend/ApiKeys"

CRED_SRC="../../codex-priv/PrivateData.cs"
CRED_DEST="../CodexBackend/API/PrivateData"

mkdir -p $KEY_DEST
cp $KEY_SRC $KEY_DEST

mkdir -p $CRED_DEST
cp $CRED_SRC $CRED_DEST