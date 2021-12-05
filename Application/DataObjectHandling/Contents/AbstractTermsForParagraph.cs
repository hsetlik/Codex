using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs.Content;
using Application.Extensions;
using Application.Interfaces;
using Application.Utilities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class AbstractTermsForParagraph
    {
        public class Query : IRequest<Result<AbstractTermsFromParagraph>>
        {
            public ParagraphQueryDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<AbstractTermsFromParagraph>>
        {
        private readonly IUserAccessor _userAccessor;
        private readonly IParserService _parser;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
            public Handler(DataContext context, IParserService parser, IUserAccessor userAccessor, IMapper mapper)
            {
            this._mapper = mapper;
            this._context = context;
            this._parser = parser;
            this._userAccessor = userAccessor;
            }

            public async Task<Result<AbstractTermsFromParagraph>> Handle(Query request, CancellationToken cancellationToken)
            {
                //we need the appropriate LanguageProfileId so we need to get the associated content language,
                //so we can get it from the Content's metadata in the dbContext
                var metadataResult = await _parser.GetContentMetadata(request.Dto.ContentUrl);
                if (metadataResult == null)
                    return Result<AbstractTermsFromParagraph>.Failure($"Could not load content metadata for URL: {request.Dto.ContentUrl}");
                var language = metadataResult.Language;
                var profile = await _context.UserLanguageProfiles.Include(u => u.User).FirstOrDefaultAsync(
                    p => p.User.UserName == _userAccessor.GetUsername() &&
                    p.Language == language);
                if (profile == null)
                    return Result<AbstractTermsFromParagraph>.Failure($"Could not load content metadata for URL: {request.Dto.ContentUrl}");
                var paragraph = await _parser.GetParagraph(request.Dto.ContentUrl, request.Dto.Index);
                var terms = paragraph.Value.Split(' ');
                var abstractTerms = new List<AbstractTermDto>();
                for(int i = 0; i < terms.Count(); ++i)
                {
                    var term = terms[i];
                    var abstractTerm = await _context.AbstractTermFor(term, profile);
                    if (!abstractTerm.IsSuccess)
                        return Result<AbstractTermsFromParagraph>.Failure($"Failed to load term for {term}");
                    abstractTerm.Value.IndexInChunk = i;
                    Console.WriteLine($"Added Term: {abstractTerm.Value.TermValue} at index: {i}");
                    abstractTerms.Add(abstractTerm.Value);
                }
                var output = new AbstractTermsFromParagraph
                {
                    ContentUrl = request.Dto.ContentUrl,
                    Index = request.Dto.Index,
                    AbstractTerms = abstractTerms
                };
                return Result<AbstractTermsFromParagraph>.Success(output);
            }
        }


    }
}