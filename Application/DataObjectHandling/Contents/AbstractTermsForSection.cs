using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs.Content;
using Application.Extensions;
using Application.Interfaces;
using Application.Utilities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    public class AbstractTermsForSection
    {
        public class Query : IRequest<Result<SectionAbstractTerms>>
        {
            public SectionQueryDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<SectionAbstractTerms>>
        {
        private readonly IUserAccessor _userAccessor;
        private readonly IParserService _parser;
        private readonly IDataRepository _factory;
            public Handler(IDataRepository factory, IParserService parser, IUserAccessor userAccessor)
            {
            this._factory = factory;
            this._parser = parser;
            this._userAccessor = userAccessor;
            }

            public async Task<Result<SectionAbstractTerms>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _factory.AbstractTermsForSection(request.Dto.ContentUrl, request.Dto.Index, _userAccessor.GetUsername(), _parser);
            }
        }


    }
}