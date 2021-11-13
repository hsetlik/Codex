using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs
{
        public class TranscriptChunkDto
    {
        public Guid TranscriptChunkId { get; set; }
        public string Language { get; set; }
        public string ChunkText { get; set; }
    }
}