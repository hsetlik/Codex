using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs.Video;
using Application.Interfaces;
using Application.Parsing;
using Application.VideoParsing;
using MediatR;

namespace Application.DataObjectHandling.Video
{
    public class GetNextCaptions
    {
        public class Query : IRequest<Result<List<VideoCaptionElement>>>
        {
            public CaptionsQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<VideoCaptionElement>>>
        {
        private readonly IVideoParser _video;
            public Handler(IVideoParser video)
            {
            this._video = video;
            }

            public async Task<Result<List<VideoCaptionElement>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _video.GetNextCaptions(request.Dto.ContentUrl, request.Dto.FromMs, request.Dto.NumCaptions);
            }
        }
    }
}