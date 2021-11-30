using System.Collections.Generic;

namespace Application.Parsing
{
    public class ParserProfile
    {
        public ParserProfile(string name)
        {
            this.Value = name;
        }
        public string Value { get; private set; }
        // The profiles w/Names
       public static ParserProfile Wikipedia { get {return new ParserProfile("Wikipedia");} }
       public static ParserProfile NewsArticle { get {return new ParserProfile("NewsArticle");} }

       //Static accessor for a list of availible profiles
       public static List<ParserProfile> Profiles { get {return new List<ParserProfile>
       {
           ParserProfile.Wikipedia, 
           ParserProfile.NewsArticle
       }; }}
        
    }
}
