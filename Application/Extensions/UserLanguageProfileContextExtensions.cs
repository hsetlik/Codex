using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain.DataObjects;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class UserLanguageProfileContextExtensions
    {
        static public async Task<Result<UserLanguageProfile>> GetProfileAsync(this DataContext context, string username, string lang)
        {
            var profile = await context.UserLanguageProfiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Language == lang && p.User.UserName == username);
            if (profile == null)
                return Result<UserLanguageProfile>.Failure("Could not load profile!");
            return Result<UserLanguageProfile>.Success(profile);
        }
        
    }
}