using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.UserTerms
{
    public class UserTermAddTranslation
    {
        public class Command : IRequest<Result<Unit>>
        {
            public AddTranslationDto AddTranslationDto { get; set; }    
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
            public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper)
            {
            this._mapper = mapper;
            this._userAccessor = userAccessor;
            this._context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var userTerm = await _context.UserTerms
                .FirstOrDefaultAsync(u => u.UserTermId == request.AddTranslationDto.UserTermId);
                if (userTerm == null) return Result<Unit>.Failure("No corresponding UserTerm found");
                var translation = new UserTermTranslation
                {
                    Value = request.AddTranslationDto.NewTranslation,
                    UserTerm = userTerm
                };
                userTerm.Translations.Add(translation);
                var result = await _context.SaveChangesAsync() > 0;
                if (! result) return Result<Unit>.Failure("Could not add translation");
                return Result<Unit>.Success(Unit.Value);
                
            }
        }
    }
}