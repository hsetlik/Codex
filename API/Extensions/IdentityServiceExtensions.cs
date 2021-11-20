using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Services;
using Domain.DataObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        //static function to move ugly setup code out of Startup.cs
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<CodexUser>(opt => 
            {
                opt.Password.RequireNonAlphanumeric = false;

            })
            .AddEntityFrameworkStores<DataContext>()
            .AddSignInManager<SignInManager<CodexUser>>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
    
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt => 
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            /*
            services.AddAuthorization(opt => 
            {
                opt.AddPolicy("IsActivityHost", policy => 
                {
                    //add authorization policies to endpoints here
                });
            });
            */ 
            //wire up authorization handling like so VVVV
            //services.AddTransient<IAuthorizationHandler>();
            services.AddScoped<TokenService>();
            return services;
        }
    }
}