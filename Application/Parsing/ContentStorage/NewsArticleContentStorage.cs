using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Parsing.ContentStorage
{
    public class NewsArticleContentStorage : BaseContentStorage
    {
        public List<TextElement> Elements { get; set; }
        
        public List<ContentSection> GetSections(int maxWordsPerSection=400)
        {
            var output = new List<ContentSection>();
            int sectionIndex = 0;
            var current = new ContentSection
            {
                ContentUrl = this.Metadata.ContentUrl,
                Index = sectionIndex,
                SectionHeader = "none",
                TextElements = new List<TextElement>()
            };
            foreach(var element in Elements)
            {

            }
            return output;
        }
    }
}