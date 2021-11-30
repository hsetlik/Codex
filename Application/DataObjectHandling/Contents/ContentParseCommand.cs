using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.Parsing;
using HtmlAgilityPack;
using MediatR;

namespace Application.DataObjectHandling.Contents
{
    
    public class ContentParseRequest
    {
        public static class Parser
        {
            
        }
        public class Query : IRequest<Result<ContentMetadataDto>>
        {
            public string Url { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContentMetadataDto>>
        {
            public Handler()
            {
            }

            public async Task<Result<ContentMetadataDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var content = await HtmlContentParser.ParseToContent(request.Url);
                if (content == null)
                    return Result<ContentMetadataDto>.Failure("Could not get parsed content");
                return Result<ContentMetadataDto>.Success(content);
            }
        }
    }
}