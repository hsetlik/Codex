using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Parsing.ContentStorage
{
    public enum ArticleElementType
    {
        Headline,
        Subhead,
        Byline,
        BlockQuote,
        BodyParagraph
    }
    public class ArticleElement
    {
      public ArticleElementType ElementType { get; set; } 
      public string Value { get; set; }
    }
    public class NewsArticleContentStorage : BaseContentStorage
    {
        public List<ArticleElement> Elements { get; set; }
        
        public List<ContentSection> GetSections(int maxWordsPerSection=400)
        {
            var output = new List<ContentSection>();
            int sectionIndex = 0;
            var current = new ContentSection
            {
                ContentUrl = this.Metadata.ContentUrl,
                Index = sectionIndex,
                Value = "",
                SectionHeader = "none"
            };
            foreach(var element in Elements)
            {
                if (element.ElementType == ArticleElementType.Headline || 
                element.ElementType == ArticleElementType.Subhead)
                {
                    current.SectionHeader = element.Value;
                }
                else if (element.ElementType == ArticleElementType.BodyParagraph)
                {
                    current.Value += element.Value;
                    var valueLength = current.Value.Split(' ').Count();
                    if (valueLength >= maxWordsPerSection)
                    {
                        Console.WriteLine($"Adding section with header {current.SectionHeader} at index {sectionIndex}");
                        output.Add(current);
                        ++sectionIndex;
                        current = new ContentSection
                        {
                            ContentUrl = this.Metadata.ContentUrl,
                            Index = sectionIndex,
                            Value = "",
                            SectionHeader = "none"
                        };
                    }
                }

                else if(element.ElementType == ArticleElementType.BlockQuote)
                {
                    //TODO
                }

                else if(element.ElementType == ArticleElementType.Byline)
                {
                    //TODO
                }
            }
            return output;
        }
    }
}