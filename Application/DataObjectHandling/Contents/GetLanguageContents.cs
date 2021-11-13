using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class GetLanguageContents
    {
        public class Query : IRequest<Result<List<ContentHeaderDto>>>
        {
            public LanguageNameDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ContentHeaderDto>>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public async Task<Result<List<ContentHeaderDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var langContents = await _context.Contents.Where(x => x.Language == request.Dto.Language).ToListAsync();
                if (langContents == null || langContents.Count < 1) 
                    return Result<List<ContentHeaderDto>>.Failure("No matching content found!");
                var output = new List<ContentHeaderDto>();
                foreach(var content in langContents)
                {
                    output.Add(content.ToHeader());
                }
                return Result<List<ContentHeaderDto>>.Success(output);
            }
        }
    }
}