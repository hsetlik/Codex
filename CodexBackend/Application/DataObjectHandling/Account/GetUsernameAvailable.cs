using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Account
{
    public class GetUsernameAvailable
    {
       public class Query : IRequest<Result<UsernameAvailableDto>>
       {
            public UsernameDto Dto { get; set; }
       }

        public class Handler : IRequestHandler<Query, Result<UsernameAvailableDto>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
                this._context = context;

            }
            public async Task<Result<UsernameAvailableDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var existing = await _context.Users.AnyAsync(u => u.UserName == request.Dto.Username);
                    return Result<UsernameAvailableDto>.Success(new UsernameAvailableDto {Username = request.Dto.Username, IsAvailable = !existing});
                }
                catch (Exception ex)
                {
                    return Result<UsernameAvailableDto>.Failure($"Failed to check existing username {request.Dto.Username}! Exception: {ex.Message}");
                }
            }
        }

    }
}