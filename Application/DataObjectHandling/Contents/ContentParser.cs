using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using MediatR;

namespace Application.DataObjectHandling.Contents
{
    public class ContentParser
    {
        public static async Task<Result<ContentCreateDto>> ParseToContent(string url)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            if (response == null)
                return Result<ContentCreateDto>.Failure("Could not get http response");
            var responseType = response.Content.GetType();
            Console.WriteLine($"Response type is: {responseType.ToString()}");
            return Result<ContentCreateDto>.Success(new ContentCreateDto
            {

            });
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
               return await ParseToContent(request.Url);
            }
        }
    }
}