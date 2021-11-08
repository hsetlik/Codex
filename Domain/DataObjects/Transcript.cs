using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DataObjects
{
    public class TranscriptChunk
    {
        [Key]
        public Guid TranscriptChunkId { get; set; }
        public Transcript Transcript { get; set; }
        public Guid TranscriptId { get; set; } //Nav property
        public string Language { get; set; }
        public string ChunkText { get; set; }
    }
    public class Transcript
    {
        [Key]
        public Guid TranscriptId { get; set; }
        public string Language { get; set; }
        public List<TranscriptChunk> TranscriptChunks { get; set; }
    }

}