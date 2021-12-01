using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs.Content;
using Application.Interfaces;
using MediatR;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class AbstractTermsForParagraph
    {
        public class Query : IRequest<Result<List<AbstractTermDto>>>
        {
            public ParagraphQueryDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<AbstractTermDto>>>
        {
        private readonly IParserService _parser;
        private readonly DataContext _context;
            public Handler(DataContext context, IParserService parser)
            {
            this._context = context;
            this._parser = parser;
            }

            public Task<Result<List<AbstractTermDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}