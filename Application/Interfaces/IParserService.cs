using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataObjectHandling.Terms;

namespace Application.Interfaces
{
    public interface IParserService
    {
        public void PrepareForContent(string url);

        public Task<int> GetNumParagraphs();

        public Task<List<AbstractTermDto>> AbstractTermsForParagraph(string contentUrl, int paragraphIndex);
    }
}