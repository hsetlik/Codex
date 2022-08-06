using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Application.Core;
using Application.DataObjectHandling;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Application;
using Persistence;
using API.Middleware;
using FluentValidation.AspNetCore;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices(_configuration);
            services.AddIdentityServices(_configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine("Configuring Codex");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
                Console.WriteLine("Running in dev environment");
            }

            app.UseHttpsRedirection();
            Console.WriteLine("Set up https redirection");

            app.UseRouting();
            Console.WriteLine("Set up routing");

            app.UseDefaultFiles();
            Console.WriteLine("Set up default files");

            app.UseStaticFiles();
            Console.WriteLine("Set up static files");

            app.UseCors("CorsPolicy");
            Console.WriteLine("Set up CORS policy");

            app.UseAuthentication();
            Console.WriteLine("Set up authentication");

            app.UseAuthorization();
            Console.WriteLine("Set up authorization");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback");
            });
            Console.WriteLine("Set up endpoints");

            Console.WriteLine("=========================STARTUP FINISHED======================================");
        }
    }
}
