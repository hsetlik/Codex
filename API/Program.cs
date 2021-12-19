using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Domain.DataObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DataContext>(); //grabs the DbContext added in ConfigureServices() in Startup.cs
                var userManager = services.GetRequiredService<UserManager<CodexUser>>();
                var parser = services.GetRequiredService<IParserService>();
                var translator = services.GetRequiredService<ITranslator>();
                await context.Database.MigrateAsync(); //appends any pending migrations to the .db file, or creates it if none exists
                await Seed.SeedData(context, userManager, parser, translator);
            }
            catch (Exception ex)
            { 
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Migration Error!");
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
