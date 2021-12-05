using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs;
using Application.Extensions;
using Application.Interfaces;
using Domain.DataObjects;
using Microsoft.AspNetCore.Identity;
using Persistence;

//initializing w/ dummy data
namespace Domain
{
    public static class WikiUrls
    {
       public static readonly List<string> Urls = new List<string>()
       {
            "https://es.wikipedia.org/wiki/Sudeste_Asi%C3%A1tico",
            "https://en.wikipedia.org/wiki/Huffman_coding",
            "https://de.wikipedia.org/wiki/Ravensbr%C3%BCck-Prozesse",
            "https://ru.wikipedia.org/wiki/%D0%A1%D0%BB%D0%BE%D0%B2%D0%BE_%D0%BE_%D0%BF%D0%BE%D0%BB%D0%BA%D1%83_%D0%98%D0%B3%D0%BE%D1%80%D0%B5%D0%B2%D0%B5"
       };
    }
    public static class Seed
    {
        public static async Task SeedData(DataContext context,
            UserManager<CodexUser> userManager,
            IParserService parser)
        {
            if (!userManager.Users.Any() && !context.Terms.Any())
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
                    UserName = "sonya",
                    DisplayName = "Sonya",
                    Email = "sonya@test.com",
                    NativeLanguage = "ru",
                    UserLanguageProfiles = new List<UserLanguageProfile>()
                 
                },
                new CodexUser
                {
                    UserName = "maria",
                    DisplayName = "Maria",
                    Email = "maria@test.com",
                    NativeLanguage = "es",
                    UserLanguageProfiles = new List<UserLanguageProfile>()
                }
            };
            //create each user on the server
            string[] langs = {"es", "en", "de", "ru"};
            for(int i = 0; i < 4; i++)
            {
                var profile = new UserLanguageProfile
                {
                    Language = langs[i],
                    UserId = users[i].UserName,
                    User = users[i],
                    KnownWords = 0
                };
                users[i].UserLanguageProfiles.Add(profile);
                await userManager.CreateAsync(users[i], "Pa$$w0rd");
            }
            //add contents to Db
            foreach(var url in WikiUrls.Urls)
            {
                var metadata = await parser.GetContentMetadata(url);
                context.Contents.Add(new Content
                {
                    ContentId = metadata.ContentId,
                    ContentUrl = metadata.ContentUrl,
                    VideoUrl = metadata.VideoUrl,
                    ContentType = metadata.ContentType,
                    ContentName = metadata.ContentName,
                    Language = metadata.Language,
                    DateAdded = DateTime.Now.ToString(),
                    ContentTags = new List<ContentTag>()
                });
            }
            await context.SaveChangesAsync();
            }
        }
    }
}