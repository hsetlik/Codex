using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.Extensions;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.UserTerms
{
    public class AnswerUserTerm
    {
        public class Command : IRequest<Result<Unit>>
        {
            public UserTermAnswerDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
            this._mapper = mapper;
            this._context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var existingTerm = await _context.UserTerms.FindAsync(request.Dto.UserTermId);
                if (existingTerm == null)
                    return Result<Unit>.Failure("Existing term not found");
                var newTerm = existingTerm.AnsweredWith(request.Dto.Answer);
                _mapper.Map(newTerm, existingTerm);
                var success = await _context.SaveChangesAsync() > 0;
                if (!success)
                    return Result<Unit>.Failure("Changes could not be saved in Db context");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}