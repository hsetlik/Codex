#!/bin/bash
cd ../pub
az webapp config appsettings set --resource-group codex-azure_group --name codex-azure --settings WEBSITE_RUN_FROM_PACKAGE="1"
az webapp deployment source config-zip --src site.zip --resource-group codex-azure_group --name codex-azure
echo "Published to Azure"



