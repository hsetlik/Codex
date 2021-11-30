using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DomainDTOs;
using Application.Extensions;
using Domain.DataObjects;
using Microsoft.AspNetCore.Identity;
using Persistence;

//initializing w/ dummy data
namespace Domain
{
    public static class Seed
    {
        public static async Task SeedData(DataContext context,
            UserManager<CodexUser> userManager)
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
            await context.SaveChangesAsync();
            //add contents to Db
            }
        }
    }
}