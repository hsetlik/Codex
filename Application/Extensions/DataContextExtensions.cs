using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class DataContextExtensions
    {
        public static async Task<Result<Unit>> CreateTerm(this DataContext context, string Language, string TermValue)
        {
            var term = await context.Terms.FirstOrDefaultAsync(x => x.Language == Language && x.Value == TermValue);
            if (term != null)
            {
                Console.WriteLine("TERM VALUE: " + term.Value + " ALREADY EXISTS" );
                return Result<Unit>.Success(Unit.Value);
            } 

            var newTerm = new Term
            {
                Language = Language,
                Value = TermValue
            };
            context.Terms.Add(newTerm);
            var result = await context.SaveChangesAsync() > 0;
            if(!result) return Result<Unit>.Failure("Term could not be found or created");
            return Result<Unit>.Success(Unit.Value);
        }
        
    }
}