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
            public TermCreateDto TermCreateDto { get; set; }
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