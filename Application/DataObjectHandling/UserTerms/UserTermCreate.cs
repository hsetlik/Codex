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
    public class UserTermCreate
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid TermId { get; set; }
            public string FirstTranslation { get; set; }
            public Guid UserTermId { get; set; } //this gets created and supplied on the client side
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
            public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper)
            {
                _mapper = mapper;
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {   
                var user = await _context.Users
                        .Include(u => u.UserLanguageProfiles)
                        .FirstOrDefaultAsync(u => u.UserName == _userAccessor.GetUsername());
                var termExists = await _context.UserTerms.AnyAsync(
                    t => t.TermId == request.TermId && t.Username == user.UserName);

                if (termExists) return Result<Unit>.Failure("Term already exists");

                var term = await _context.Terms.FindAsync(request.TermId);

                var langProfile = user.UserLanguageProfiles.FirstOrDefault(l => l.Language == term.Language);

                var translation = new Translation{TermId = request.TermId, Value = request.FirstTranslation};

                var userTerm = new UserTerm
                {
                    UserTermId = request.UserTermId,
                    TermId = request.TermId,
                    Term = term,
                    Username = user.UserName,
                    UserLanguageProfile = langProfile,
                    Translations = {translation},
                    TimesSeen = 0,
                    KnowledgeLevel = 0,
                    EaseFactor = 2.50f,
                    SrsInterval = 0 
                };

                _context.UserTerms.Add(userTerm);

                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to create UserTerm");
                return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}