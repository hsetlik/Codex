using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Configuration;
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
            Console.WriteLine("Initializing codex...");
            Console.WriteLine("Preparing to create host. . .");
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            Console.WriteLine("Got service provider");
            try
            {
                var context = services.GetRequiredService<DataContext>(); //grabs the DbContext added in ConfigureServices() in Startup.cs
                var userManager = services.GetRequiredService<UserManager<CodexUser>>();
                var parser = services.GetRequiredService<IParserService>();
                var translator = services.GetRequiredService<ITranslator>();
                var creds = services.GetRequiredService<ConfigCredentials>();
                await context.Database.MigrateAsync(); //appends any pending migrations to the .db file, or creates it if none exists
                await Seed.SeedData(context, userManager, parser, translator, creds);
                Console.WriteLine("Initialized successfully");
            }
            catch (Npgsql.NpgsqlException exc)
            {
                Console.WriteLine("Hit Npgsql Exception!");
                Console.WriteLine($"SqlState: {exc.SqlState}");
                // Console.WriteLine($"Batch Command: {exc.BatchCommand.CommandText}");
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(exc, "Npgsql error!");
            }
            catch (Exception ex)
            { 
                var logger = services.GetRequiredService<ILogger<Program>>();
                Exception innerExc = null;
                if (ex.InnerException != null)
                {
                    innerExc = ex.InnerException;
                    Console.WriteLine($"Inner exception with message: {innerExc.Message}");
                }
                else
                    Console.WriteLine("No inner exception");
                if (ex.Data.Count > 0)
                {
                    int i = 0;
                    foreach(var pair in ex.Data)
                    {
                        Console.WriteLine($"Data entry #{i} string: {pair.ToString()}");
                    }
                }
                else
                    Console.WriteLine("No exception data!");
                logger.LogError(ex, "Migration Error!");
            }
           
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    if (context.HostingEnvironment.IsProduction())
                    {
                        ConfigCredentials.ConfigureKeyVault(context, config);
                    }
                });
    }
}
