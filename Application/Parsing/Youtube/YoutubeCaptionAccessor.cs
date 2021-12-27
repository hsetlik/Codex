using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;

namespace Application.Parsing.Youtube
{
    public class YoutubeCaptionAccessor
    {
        public YoutubeCaptionAccessor(string url)
        {
            var credential = GoogleCredential.FromFile("/Users/hayden/Documents/dev/DotnetProjects/Codex/ApiKeys/key.json");
           
        }
    }
}