using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.Utilities;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Extensions
{
    public static class TermContextExtensions
    {
         public static async Task<Result<Unit>> CreateTerm(this DataContext context, string language, string termValue)
        {
            var normValue = termValue.AsTermValue();
            var term = await context.Terms.FirstOrDefaultAsync(x => x.Language == language && x.NormalizedValue == normValue);
            if (term != null)
            {
                Console.WriteLine("TERM VALUE: " + term.NormalizedValue + " ALREADY EXISTS" );
                return Result<Unit>.Success(Unit.Value);
            } 
            var newTerm = new Term
            {
                Language = language,
                NormalizedValue = normValue
            };
            context.Terms.Add(newTerm);
            var result = await context.SaveChangesAsync() > 0;
            if(!result) return Result<Unit>.Failure("Term could not be found or created");
            return Result<Unit>.Success(Unit.Value);
        }
        public static async Task<Result<Unit>> EnsureTermsForContent(this DataContext context, Guid contentId)
        {  
            var content = await context.Contents.FindAsync(contentId);
            // TODO    
            return Result<Unit>.Success(Unit.Value);
        }

        public static async Task<Result<Unit>> EnsureTerm(this DataContext context, TermDto dto)
        {
            var term = await context.Terms.FirstOrDefaultAsync(t => t.NormalizedValue == dto.Value.AsTermValue() && t.Language == dto.Language);
            if (term == null)
            {
                

            }
            return Result<Unit>.Success(Unit.Value);
        }
    }
}