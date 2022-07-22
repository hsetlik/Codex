using System.Collections.Generic;

namespace Application.Parsing
{
    public class ScraperProfile
    {
        public ScraperProfile(string name)
        {
            this.Value = name;
        }
        public string Value { get; private set; }
        // The profiles w/Names
       public static ScraperProfile Wikipedia { get {return new ScraperProfile("Wikipedia");} }
       public static ScraperProfile NewsArticle { get {return new ScraperProfile("NewsArticle");} }
       public static ScraperProfile Youtube { get {return new ScraperProfile("Youtube");} } 

       //Static accessor for a list of availible profiles
       public static List<ScraperProfile> Profiles { get {return new List<ScraperProfile>
       {
           ScraperProfile.Wikipedia, 
           ScraperProfile.NewsArticle,
           ScraperProfile.Youtube
       }; }}
        
    }
}
