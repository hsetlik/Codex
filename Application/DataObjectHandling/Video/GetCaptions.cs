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
using Persistence;

namespace Application.DataObjectHandling.Video
{
    public class GetCaptions
    {
        public class Query : IRequest<Result<List<VideoCaptionElement>>>
        {
            public CaptionsQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<VideoCaptionElement>>>
        {
        private readonly IVideoParser _video;
        private readonly DataContext _context;
            public Handler(IVideoParser video, DataContext context)
            {
            this._context = context;
            this._video = video;
            }

            public async Task<Result<List<VideoCaptionElement>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _video.GetCaptions(request.Dto.VideoId, request.Dto.FromMs, request.Dto.NumCaptions, request.Dto.Language);
            }
        }
    }
}