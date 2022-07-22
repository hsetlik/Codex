using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Content.Responses;
using Application.DomainDTOs.UserLanguageProfile;
using Application.Extensions;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class GetSavedContents
    {
        public class Query : IRequest<Result<List<SavedContentDto>>>
        {
            public ProfileIdQuery Dto { get; set; }
        }


        public class Handler : IRequestHandler<Query, Result<List<SavedContentDto>>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<List<SavedContentDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.GetSavedContents(request.Dto.LanguageProfileId);
            }
        }
    }
}