using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Phrase
{
    public class UpdatePhrase
    {
        public class Command : IRequest<Result<Unit>>
        {
            public PhraseDetailsDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var phrase = await _context.Phrases.Include(p => p.Translations).FirstOrDefaultAsync(p => p.PhraseId == request.Dto.PhraseId);
                if (phrase == null)
                    return Result<Unit>.Failure($"Could not find phrase with ID {request.Dto.PhraseId} and value {request.Dto.Value}");
                phrase = phrase.UpdatedWith(request.Dto);
                var success = await _context.SaveChangesAsync() > 0;
                if (!success)
                    return Result<Unit>.Failure("Could not save changes");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}