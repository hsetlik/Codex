using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.DomainDTOs.Content;
using Application.Extensions;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class GetContentWithId
    {
        public class Query : IRequest<Result<ContentMetadataDto>>
        {
            public ContentIdQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContentMetadataDto>>
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

            public async Task<Result<ContentMetadataDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var contentResult = await _context.GetMetadataFor(_userAccessor.GetUsername(), request.Dto.ContentId);
                if (!contentResult.IsSuccess)
                {
                    // update the lastStudiedLanguage
                    var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == _userAccessor.GetUsername());
                    if (user != null)
                    {
                        user.LastStudiedLanguage = contentResult.Value.Language;
                        var isSuccess = await _context.SaveChangesAsync() > 0;
                        if(!isSuccess)
                        {
                            return Result<ContentMetadataDto>.Failure("Could not update user's last studied language!");
                        }

                    }
                    return Result<ContentMetadataDto>.Failure($"Failed to get metadata! Error message{contentResult.Error}");
                }
                return Result<ContentMetadataDto>.Success(contentResult.Value);
            }
        }
    }
}