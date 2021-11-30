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
        public class Query : IRequest<Result<ContentCreateDto>>
        {
            public string Url { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContentCreateDto>>
        {
            public Handler()
            {
            }

            public async Task<Result<ContentCreateDto>> Handle(Query request, CancellationToken cancellationToken)
            {
               return await HtmlContentParser.ParseToContent(request.Url);
            }
        }
    }
}