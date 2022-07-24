#!/bin/bash
cd ../pub
az webapp deployment source config-zip --src site.zip --resource-group codex-azure_group --name codex-azure
echo "Published to Azure"



