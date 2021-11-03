using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DataObjects;
using Microsoft.AspNetCore.Identity;

//initializing w/ dummy data
namespace Persistence
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
                    NativeLanguage = "en"
                },
                new CodexUser
                {
                    UserName = "hans",
                    DisplayName = "Hans",
                    Email = "hans@test.com",
                    NativeLanguage = "de"
                },
                new CodexUser
                {
                    UserName = "sonya",
                    DisplayName = "Sonya",
                    Email = "sonya@test.com",
                    NativeLanguage = "ru"
                },
                new CodexUser
                {
                    UserName = "maria",
                    DisplayName = "Maria",
                    Email = "maria@test.com",
                    NativeLanguage = "es"
                }
            };
            //create each user on the server
            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }

            //some dummy words
            var terms = new List<Term>
            {
                new Term
                {
                    Value = "casa",
                    Language = "es"
                },
                new Term
                {
                    Value = "house",
                    Language = "en"

                },
                new Term
                {
                    Value = "дом",
                    Language = "ru"
                },
                new Term
                {
                    Value = "Haus",
                    Language = "de"
                }
            };
            //add words to Db
            await context.Terms.AddRangeAsync(terms);
            await context.SaveChangesAsync();
            }
        }
    }
}