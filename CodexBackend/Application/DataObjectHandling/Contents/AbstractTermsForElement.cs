using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DataObjectHandling.Terms;
using Application.DomainDTOs.Content;
using Application.DomainDTOs.UserTerm.Queries;
using Application.Extensions;
using Application.Interfaces;
using Application.Parsing;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataObjectHandling.Contents
{
    using TaskMap =  Dictionary<int, Task<Result<AbstractTermDto>>>;
    public class AbstractTermsForElement
    {
        public class Query : IRequest<Result<ElementAbstractTerms>>
        {
            public ElementAbstractTermsQuery Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ElementAbstractTerms>>
        {
        private readonly IDataRepository _factory;
        private readonly IUserAccessor _userAccessor;
        private readonly DataContext _context;
            public Handler(IDataRepository factory, IUserAccessor userAccessor, DataContext context)
            {
            this._context = context;
            this._userAccessor = userAccessor;
            this._factory = factory;
            }
            public async Task<Result<ElementAbstractTerms>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.GetAbstractTermsForElement(_userAccessor, _factory, request.Dto);
            }
        }
    }
}