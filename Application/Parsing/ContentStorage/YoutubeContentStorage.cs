using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Parsing.ContentStorage
{
    public class YoutubeContentStorage : BaseContentStorage
    {
        public List<TextElement> Subtitles { get; set; }
        
    }
}