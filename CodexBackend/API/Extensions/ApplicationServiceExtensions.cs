using Application.DataObjectHandling;
//using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Persistence;
//using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using FluentValidation.AspNetCore;
using MediatR;
using Application.Interfaces;
using Application.Core;
using Application.DataObjectHandling.UserTerms;
using Application.Parsing;
using EFCore.DbContextFactory.Extensions;
using Application;
using Application.TranslationService;
using Application.ProfileHistoryEngine;
using Application.VideoParsing;
using System;
using API.PrivateData;

//using Application.Interfaces;

//This is just here to move some ugly service configuration code out of Startup.cs

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        private static string Host = "codex-postgres.postgres.database.azure.com";
        // keep credentials in ignored file
        private static string User = PrivateData.PrivateData.AzureUsername;
        private static string DBname = "codex-pgsql";
        private static string Password = PrivateData.PrivateData.AzurePassword;
        private static string Port = "5432";
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers(opt =>
             {
                 var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                 opt.Filters.Add(item: new AuthorizeFilter(policy));
             })
                 .AddFluentValidation(_config =>
             {
                 _config.RegisterValidatorsFromAssemblyContaining<UserTermCreate>();
             });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            string connString = String.Format("Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer",
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);

            Console.WriteLine($"Connection string: {connString}");
            //add the normal, single-instance DB context
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseNpgsql(connString);
            });
            // add the DB factory
            services.AddDbContextFactory<DataContext>(opt =>
            {
                opt.UseNpgsql(connString);
            });
            // add the repository to access the factory
            services.AddScoped<IDataRepository, DataRepository>();

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader(); //TODO: call WithOrigin() with the actual client URL
                });
            });
            services.AddMediatR(typeof(UserTermCreate.Handler).Assembly);
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IParserService, ParserService>();
            services.AddScoped<ITranslator, Translator>();
            services.AddScoped<IProfileHistoryEngine, ProfileHistoryEngine>();
            services.AddScoped<IVideoParser, VideoParser>();
            return services;
        }

    }
}