using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Parsing;
using Persistence;

namespace Application.Interfaces
{
    public interface IVideoParser
    {
        Task<Result<List<VideoCaptionElement>>> GetCaptions(string contentUrl, int fromMs, int numCaptions, string language);
    }
}