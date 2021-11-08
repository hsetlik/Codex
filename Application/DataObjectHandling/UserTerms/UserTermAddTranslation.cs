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
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var userTerm = await _context.UserTerms.Include(t => t.Term).FirstOrDefaultAsync(x => x.UserTermId == request.AddTranslationDto.UserTermId);
                if (userTerm == null) return Result<Unit>.Failure("UserTerm not found");
                var translation = new Translation
                {
                    TranslationId = Guid.NewGuid(),
                    Value = request.AddTranslationDto.NewTranslation,
                    Term = userTerm.Term,
                    TermId = userTerm.TermId
                };
                _context.Translations.Add(translation);
                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Translation could not be added");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}