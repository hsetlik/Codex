using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain.DataObjects;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class ContentHeaderDto
    {
        public bool HasVideo { get; set; }
        public bool HasAudio { get; set; }
        public string ContentType { get; set; }
        public string ContentName { get; set; }
        public string Language { get; set; }
        public string DateAdded { get; set; }
    }
    public static class ContentHeaderDtoFactory
    {
        public static ContentHeaderDto ToHeader(this Content content)
        {
            var hasVideo = !(content.VideoUrl == "none");
            var hasAudio = !(content.AudioUrl == "none");
             return new ContentHeaderDto {
                 HasVideo = hasVideo,
                 HasAudio = hasAudio,
                 ContentType = content.ContentType,
                 ContentName = content.ContentName,
                 Language = content.Language,
                 DateAdded = content.DateAdded
             };
        }
    }
    public class GetContentHeader
    {
        public class GetContentHeaderDto
        {
            public Guid ContentId { get; set; }
        }
        public class Query : IRequest<Result<ContentHeaderDto>>
        {
            public GetContentHeaderDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContentHeaderDto>>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

            public  async Task<Result<ContentHeaderDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var content = await _context.Contents.FindAsync(request.Dto.ContentId);
                if (content == null) return Result<ContentHeaderDto>.Failure("Content not found");
                return Result<ContentHeaderDto>.Success(content.ToHeader());
            }
        }
    }
}