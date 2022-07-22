using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode.Videos.ClosedCaptions;

namespace Application.Parsing.ContentStorage
{
    public class YoutubeSection : ContentSection
    {
        public new List<VideoCaptionElement> TextElements { get; set; }
    }
    public class YoutubeContentStorage : BaseContentStorage
    {
        public new List<YoutubeSection> Sections { get; set; }
        
    }
}