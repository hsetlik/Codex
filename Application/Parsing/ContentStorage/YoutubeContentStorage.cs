using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode.Videos.ClosedCaptions;

namespace Application.Parsing.ContentStorage
{
    public class CaptionElement : TextElement
    {
        public TimeSpan TimeSpan { get; set; }
        public CaptionElement()
        {

        }
        public CaptionElement(ClosedCaption caption, string contentUrl)
        {
            Tag = "caption";
            ContentUrl = contentUrl;
            TimeSpan = caption.Duration;
            Value = caption.Text;
        }
    }
    public class YoutubeContentStorage : BaseContentStorage
    {
        public List<CaptionElement> CaptionElements { get; set; }
        
    }
}