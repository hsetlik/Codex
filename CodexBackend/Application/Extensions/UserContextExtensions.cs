using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class UserContextExtensions
    {
        
        public static async Task<Result<Unit>> SetLastStudiedLanguage(this DataContext context, string iso, string username)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) return Result<Unit>.Failure("Invalid username");
            user.LastStudiedLanguage = iso;
            var success = await context.SaveChangesAsync() > 0;
            if (!success)
                return Result<Unit>.Failure("Changes not saved");
            return Result<Unit>.Success(Unit.Value);
        }


    }
}