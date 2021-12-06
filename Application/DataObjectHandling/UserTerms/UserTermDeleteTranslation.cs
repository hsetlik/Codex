using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain.DataObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.UserTerms
{
    public class UserTermDeleteTranslation
    {
        public class Command: IRequest<Result<Unit>>
        {
            public ChildTranslationDto Dto { get; set; }
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
                var userTerm = await _context.UserTerms
                .Include(t => t.Translations)
                .FirstOrDefaultAsync(u => u.UserTermId == request.Dto.UserTermId);

                if (userTerm == null)
                    return Result<Unit>.Failure("Could not load userTerm");
                if (userTerm.Translations.Count < 2)
                    return Result<Unit>.Failure("Cannot delete the UserTerm's only translation");
                var translation = userTerm.Translations.FirstOrDefault(t => t.Value == request.Dto.Value);
                if (translation == null)
                    return Result<Unit>.Failure("Could not find matching translation");
                _context.Translations.Remove(translation);

                var success = await _context.SaveChangesAsync() > 0;
                if (!success)
                    return Result<Unit>.Failure("Could not update Db context");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}