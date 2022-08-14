using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.DomainDTOs.Account;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Account
{
    public class GetUsernameAvailable
    {
        public class Query: IRequest<Result<UsernameAvailableDto>>
        {
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<UsernameAvailableDto>>
        {
            private readonly DataContext context;

            public Handler (DataContext context)
            {
                this.context = context;
            }

            public async Task<Result<UsernameAvailableDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var usernameExists = await context.Users.AnyAsync(u => u.UserName == request.Username);
                    return Result<UsernameAvailableDto>.Success(new UsernameAvailableDto{Username = request.Username, IsAvailable = !usernameExists});
                }
                catch (System.Exception ex)
                {
                    return Result<UsernameAvailableDto>.Failure($"Endpoint failed!: Exception: {ex.Message}");
                }
            }
        }
    }
}