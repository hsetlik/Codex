using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Parsing.ContentStorage
{
    public class NewsArticleContentStorage : BaseContentStorage
    {
        public List<VideoCaptionElement> Elements { get; set; }
        
    }
}