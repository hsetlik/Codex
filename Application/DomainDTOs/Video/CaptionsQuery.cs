using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DomainDTOs.Video
{
    public class CaptionsQuery
    {
        public int FromMs { get; set; }
        public int NumCaptions { get; set; }
        public string ContentUrl { get; set; }
    }

}