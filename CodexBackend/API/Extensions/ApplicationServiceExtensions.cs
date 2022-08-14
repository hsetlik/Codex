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
using Application;
using Application.TranslationService;
using Application.ProfileHistoryEngine;
using Application.VideoParsing;
using System;
using System.Configuration;

//using Application.Interfaces;

//This is just here to move some ugly service configuration code out of Startup.cs

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        private static bool useConfigServer = false;
        private static bool useVps = true;

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
            string connectionStr = "";

            if (useConfigServer)
                connectionStr = config.GetConnectionString("DefaultConnection");
            else if (useVps)
            {
                Console.WriteLine("Connecting to droplet...");
                string vpsHostname = config["DropletAddress"];
                string vpsDbName = "codexdb";
                string vpsPort = "5432";
                string vpsUserName = config["DropletUser"];
                string vpsPassword = config["DropletPassword"];
                connectionStr = $"Server={vpsHostname};Database={vpsDbName};Port={vpsPort};User Id={vpsUserName};Password={vpsPassword};";

            }

            Console.WriteLine($"Connection string is: {connectionStr}");
            //add the normal, single-instance DB context
            services.AddDbContext<DataContext>(opt => opt.UseNpgsql(connectionStr));

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); //TODO: refine this for secutiry
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