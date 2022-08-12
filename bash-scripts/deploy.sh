#!/bin/bash
#NOTE: this script must be run w sudo
./build-zip.sh
./publish-to-azure.sh
echo "Deployment succeded!"