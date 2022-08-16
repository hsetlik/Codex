#!/bin/bash
cd ..
REMOVEMIGRATION=0
while getopts 'm' opt;
do
    case "${opt}" in

        (m) 
            REMOVEMIGRATION=1
            ;;
        (*)
            echo "No flags passed" 
            exit 1;;
    esac
done

if test REMOVEMIGRATION -gt 0;
then
    dotnet ef migrations remove -p Persistence -s API
fi

dotnet ef database drop -p Persistence -s API

    