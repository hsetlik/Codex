using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Parsing.ContentStorage
{
    public class WikiContentStorage : BaseContentStorage
    {
        public List<ContentSection> Sections { get; set; }
    }
}