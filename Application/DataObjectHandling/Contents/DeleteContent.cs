using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class DeleteContent
    {
        public class Command : IRequest<Result<Unit>>
        {
            public ContentUrlDto Dto { get; set; }   
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
                var content = await _context.Contents.FirstOrDefaultAsync(c => c.ContentUrl == request.Dto.ContentUrl);
                if (content == null)
                    return Result<Unit>.Failure("No matching URL in database");
                _context.Contents.Remove(content);

                var success = await _context.SaveChangesAsync() > 0;
                if (!success)
                    return Result<Unit>.Failure("Failed to save changes");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}