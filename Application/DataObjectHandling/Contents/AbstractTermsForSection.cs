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
        public class Query : IRequest<Result<AbstractTermsFromSection>>
        {
            public SectionQueryDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<AbstractTermsFromSection>>
        {
        private readonly IUserAccessor _userAccessor;
        private readonly IParserService _parser;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
            public Handler(DataContext context, IParserService parser, IUserAccessor userAccessor, IMapper mapper)
            {
            this._mapper = mapper;
            this._context = context;
            this._parser = parser;
            this._userAccessor = userAccessor;
            }

            public async Task<Result<AbstractTermsFromSection>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.AbstractTermsForSection(request.Dto.ContentUrl, request.Dto.Index, _parser, _userAccessor.GetUsername());
            }
        }


    }
}