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
using Application.Extensions;
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
                return await _context.CreateTerm(request.TermCreateDto.Language, request.TermCreateDto.Value);
            }
        }

    }
}