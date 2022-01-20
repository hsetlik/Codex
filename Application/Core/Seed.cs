using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.UserTerms;
using Application.DomainDTOs;
using Application.Extensions;
using Application.Interfaces;
using Domain.DataObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

//initializing w/ dummy data
namespace Domain
{
    public static class WikiUrls
    {
       public static readonly List<string> Urls = new List<string>()
       {

            //Spanish
            "https://es.wikipedia.org/wiki/Fotograf%C3%ADa",
            "https://es.wikipedia.org/wiki/Sudeste_Asi%C3%A1tico",
            "https://es.wikipedia.org/wiki/Ruta_del_Cares",
            //English
            "https://en.wikipedia.org/wiki/Huffman_coding",
            "https://en.wikipedia.org/wiki/Harry_E._Claiborne",
            "https://en.wikipedia.org/wiki/Indian_Air_Force",
            //German 
            "https://de.wikipedia.org/wiki/Ravensbr%C3%BCck-Prozesse",
            "https://de.wikipedia.org/wiki/Heilige_Familie_(Weiler)",
            "https://de.wikipedia.org/wiki/Dieselmotor",
            //Russian
            "https://ru.wikipedia.org/wiki/%D0%90%D0%BB%D1%82%D0%B0%D0%B9%D1%81%D0%BA%D0%B8%D0%B9_%D0%BA%D1%80%D0%B0%D0%B9",
            "https://ru.wikipedia.org/wiki/%D0%97%D0%B0%D0%B0%D0%BB%D0%B8%D1%88%D0%B2%D0%B8%D0%BB%D0%B8,_%D0%9C%D0%B0%D0%BB%D1%85%D0%B0%D0%B7_%D0%9C%D0%B8%D1%85%D0%B0%D0%B9%D0%BB%D0%BE%D0%B2%D0%B8%D1%87",
            "https://ru.wikipedia.org/wiki/%D0%A1%D0%BB%D0%BE%D0%B2%D0%BE_%D0%BE_%D0%BF%D0%BE%D0%BB%D0%BA%D1%83_%D0%98%D0%B3%D0%BE%D1%80%D0%B5%D0%B2%D0%B5",
            "https://meduza.io/feature/2021/09/23/menya-nelzya-bylo-ostanovit"
       };
    }
    
    public static class Seed
    {
        public static async Task SeedData(DataContext context,
            UserManager<CodexUser> userManager,
            IParserService parser,
            ITranslator translator)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            if (!userManager.Users.Any() && !context.UserTerms.Any())
            {
                //make some dummy users w/ whatever info
            var users = new List<CodexUser>
            {
                new CodexUser
                {
                    UserName = "jeff",
                    DisplayName = "Jeff",
                    Email = "jeff@test.com",
                    NativeLanguage = "en",
                    UserLanguageProfiles = new List<UserLanguageProfile>()
                },
                new CodexUser
                {
                    UserName = "hans",
                    DisplayName = "Hans",
                    Email = "hans@test.com",
                    NativeLanguage = "de",
                    UserLanguageProfiles = new List<UserLanguageProfile>()
                },
                new CodexUser
                {
                    UserName = "maria",
                    DisplayName = "Maria",
                    Email = "maria@test.com",
                    NativeLanguage = "es",
                    UserLanguageProfiles = new List<UserLanguageProfile>()
                },
                new CodexUser
                {
                    UserName = "sonya",
                    DisplayName = "Sonya",
                    Email = "sonya@test.com",
                    NativeLanguage = "ru",
                    UserLanguageProfiles = new List<UserLanguageProfile>()
                }
            };
            //create each user on the server
            string[] langs = {"ru", "en", "de", "es"};
            for(int i = 0; i < 4; i++)
            {
                var profile = users[i].CreateProfileFor(langs[i]);
                users[i].UserLanguageProfiles.Add(profile);
                Console.WriteLine($"Creating user {users[i].UserName} with profile language {langs[i]}");
                await userManager.CreateAsync(users[i], "Pa$$w0rd");
            }
            //add contents to Db
            for(int i = 0; i < WikiUrls.Urls.Count; ++i)
            {
                string url = WikiUrls.Urls[i];
                Console.WriteLine($"Seeding content with URL: {url}");
                var metadata = await parser.GetContentMetadata(url);
                var user = users.First(u => u.UserLanguageProfiles.Any(p => p.Language == metadata.Language));
                var profile = user.UserLanguageProfiles.First(p => p.Language == metadata.Language);
                context.Contents.Add(new Content
                {
                    ContentId = metadata.ContentId,
                    ContentUrl = metadata.ContentUrl,
                    VideoId = metadata.VideoId,
                    ContentType = metadata.ContentType,
                    ContentName = metadata.ContentName,
                    Language = metadata.Language,
                    CreatedAt = DateTime.Now,
                    ContentTags = new List<ContentTag>(),
                    NumSections = metadata.NumSections,
                    Description = $"Dummy description number {i}",
                    CreatorUsername = user.UserName,
                    UserLanguageProfile = profile,
                    LanguageProfileId = profile.LanguageProfileId
                });
            }
            await context.SaveChangesAsync();
            var timeString = new List<string>();
            foreach(var lang in langs)
            {
                Console.WriteLine($"Creating terms for language: {lang}");
                var lWatch = System.Diagnostics.Stopwatch.StartNew();
                var user = await context.Users.Include(u => u.UserLanguageProfiles)
                 .FirstOrDefaultAsync(u => u.UserLanguageProfiles.Any(p => p.Language == lang));
                var content = await context.Contents.FirstOrDefaultAsync(c => c.Language == lang);
                var section = await parser.GetSection(content.ContentUrl, 0);
                Console.WriteLine($"Section is: {section.Body}");
                while(section.Body.Length > 2000) 
                {
                    section.TextElements = section.TextElements.Take(section.TextElements.Count - 1).ToList();
                }
                var creators = await section.CreatorsFor(translator, content.Language);
                foreach (var creator in creators)
                {
                    Console.WriteLine($"Creating term for: {creator.TermValue}");
                    var termResult = await context.CreateDummyUserTerm(creator, user.UserName);
                }
                lWatch.Stop();
                Console.WriteLine($"Creating terms for {lang} took {lWatch.ElapsedMilliseconds} ms");
            }
            watch.Stop();
            Console.WriteLine($"Seeding Databse took {watch.ElapsedMilliseconds} ms");
            }
        }
    }
}