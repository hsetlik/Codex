using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Parsing;

namespace Application.Interfaces
{
    public interface IVideoParser
    {
        Task<Result<List<VideoCaptionElement>>> GetNextCaptions(string contentUrl, int fromMs, int numCaptions);
    }
}