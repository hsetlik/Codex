using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Domain.DataObjects;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling
{
    public class TermCreate
    {
        public class Command : IRequest<Result<Unit>>
        {
            public TermDto TermCreateDto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                
                var exists = _context.Terms.Any(x => x.Value == request.TermCreateDto.Value && 
                x.Language == request.TermCreateDto.Language);
                if (exists)
                {  //this just returns success if a term already exists so we don't have to do that logic in the controller
                    return Result<Unit>.Success(Unit.Value);
                }
                var term = new Term
                {
                    Value = request.TermCreateDto.Value,
                    Language = request.TermCreateDto.Language,
                    TermId = Guid.NewGuid()
                };
                _context.Terms.Add(term);
                var result = await _context.SaveChangesAsync() > 0;
                if (result)
                {
                    return Result<Unit>.Success(Unit.Value);
                }
                else
                {
                    return Result<Unit>.Failure("Creation failed");
                }
            }
        }

    }
}