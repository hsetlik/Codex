using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.UserTerms
{
    public class AddTranslationDto
    {
        public Guid UserTermId { get; set; }
        public string NewTranslation { get; set; }
    }
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
                throw new NotImplementedException();
                var userTerm = await _context.UserTerms
                .Include(x => x.Translations)
                .ThenInclude(t => t.Term)
                .FirstOrDefaultAsync( x => x.UserTermId == request.AddTranslationDto.UserTermId);

                

                
            }
        }
    }
}