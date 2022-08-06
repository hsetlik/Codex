#!/bin/bash
echo "Generating private files. . ."
#========================================================================================
# IMPORTANT: FOLLOW INSTRUCTIONS BELOW WHEN CLONING THIS REPO
# 
# 1. Run this script to generate boilerplate files in a new directory at "../../priv"
# 2. Edit the newly created text files to fill in the relevant strings with your credentials
# 3. Run ./copy-priv.sh to copy the files in ../../priv to the appropriate ignored directories inside this repository
# Note that git operations, like switching to a different branch or pulling from a remote, delete the ignored credentials
# files inside the repo. Don't forget to run ./copy-priv.sh after such operations to copy your private credentials from ../../priv to
# the appropriate ignored paths in this repo.
#========================================================================================

mkdir -p ../../priv
chmod a+w ../../priv/*
cd ../../priv
touch PrivateData.cs
echo 'using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.PrivateData
{
    public class PrivateData
    {
        public static string AzureUsername = "My Azure Username";
        public static string AzurePassword = "My Azure Password";
    }
}' > PrivateData.cs

touch api-keys.json
echo '{
  "type": "service_account",
  "project_id": "codextranslation",
  "private_key_id": "69b55f865606e00a9a9a6dc5d95cc62f2e6c1da8",
  "private_key": "-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDTvFeDpy+4JRVQ\n1Uq4E9BUup8hLxLDW1r4O50zcBBAxTdSB/41NftsCu/qcuD8ixCdXnBsRzLWOGgE\ny4oOWdTLLLe/784Q2zy8qS5dIasWwonikTGmb35oFV4wEoWdwESf/F46DORUgfVG\nWl7weT0VPKV8hj6M6Z9VkyEh5z4wqRsHxtcKabPNa/8darbOPnS2nDyY9UmAEu06\n3UCrw2mV457Nvlgpmtpmm5rC6GtNy5X+ebyT5M/mMX48e8fR/q50079UyEcfLUVU\nQtN5540Uiva6B4y8LLnqnwIo8M4U6IxT+amVhfqS85J4T5rbigitlWUntgPI/E8x\nSlqwMtXPAgMBAAECggEAW8K54pJn+dQIYtms4g2kMbQXFpiB9yv2RrH/NLWSNZ/C\nk8hnAMR+S5qJ0v8qRgg2kRzGeAZ7H5+eZMWY9RM1Rz//+iyBD4kTz2c6cp4OuXRh\nsVDfRZP4Ij9wbED6qx0SIIuMgnfn1D/l/jFYB7tjH3sC9O5w2Ty+EqYKq9i9ePRm\nSRiT9e3+eBNHf9C5QREosXApjs9qz2mW3/4xKqITxn2rpdu034ZoNi+qgqEDOE3Y\nENwHW/hJbPjFUl2pa8O/050A/+vLguNQadKElGYgSjtrwL61I9NGnfqmtU6bPrEF\n5Hotv/wThisTAB7h4YGzwGHvTqgFottg/R6poxPZEQKBgQD4mqtxcRL65EXgRsiW\nqP0BUOt/yIlzZfAvyhs3Qe9zFCcET5GHsodQCy0GiYP8Ee33ijY7Yq5KwH1wjz10\n6Qrb8D5u0c2Rr4u9e94AKTVXJHaM7F77KG3EXgxlsojcTqX9fOEOov/cr79jjb6m\n4nFAMSknQqe9FIYZIGgg1pIWYwKBgQDaCONAZ6gzRtZD37LmIwjd0pnoEed33jXv\noZCvy1MDSGFQY3cK4RS7jBZwcCmPXSt5S1wcAdwH0BwevOsBZcKYNOWZzPj7t38t\ne/YnmcnyJ0fcuSz+zBy/yBYKAyqO0CeDKc2KCWIxjXsEuPmr2AoLOIlNzqLnxHFF\n8XKrMAR4pQKBgA3FE2XvK2v2JOCtSwL9TTY7cy/avbJEnS8odKV043xuS1LzzU3E\n17XZSJ8qEsNtgs7JOwPDDYBadRg89tx44/8WqX6d5FMDdCwEpmE/5xJylJZtqT0k\nGiBcTTd80hPRz2Pg/N5ArQdePK2Y9kxsEnXJw0vHZ18TufG+V8Yp3GGdAoGBAKkd\nta7R9/v12OIClA+/YvQzkx8XLBOkru80wTYpnjTwolKpI7+zf9JMwWbrCMFjggHt\n+v7aTmFdAUKyRBHsbTiD5wcZnpIX7TRJb0+eEK6b2ziQBR+JEE+NEdMoS41EVTF+\nbifASYKn+6BZHYc15ex4KL/KJD9i9n1K3yd68izlAoGBAOEzOn00d+0/xNZOffoK\nWujrAy7JESpr23YTNTYW/p4AXm0ibiQ1nix0r1bjfRMCdqXR90w1pjXdUkSjNAvk\nFjFTMMmeplNq73DRjViEexsCrfAD0Xw7IK7K6YWXJZSsy/BVfBJORgPrmwilJHc8\nqOJTqI3wiQCACtN/HoSsjvaq\n-----END PRIVATE KEY-----\n",
  "client_email": "codexserviceaccount@codextranslation.iam.gserviceaccount.com",
  "client_id": "112178468314082091572",
  "auth_uri": "https://accounts.google.com/o/oauth2/auth",
  "token_uri": "https://oauth2.googleapis.com/token",
  "auth_provider_x509_cert_url": "https://www.googleapis.com/oauth2/v1/certs",
  "client_x509_cert_url": "https://www.googleapis.com/robot/v1/metadata/x509/codexserviceaccount%40codextranslation.iam.gserviceaccount.com"
}' > api-keys.json
echo "Created default priv folder\n ===================================================================="

