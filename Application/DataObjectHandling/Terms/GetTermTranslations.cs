using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.DomainDTOs;
using Application.DomainDTOs.Term;
using MediatR;

namespace Application.DataObjectHandling.Terms
{
    public class GetTermTranslations
    {
       public class Query : IRequest<Result<List<DictionaryEntryDto>>>
       {
           public TranslationRequestDto Dto { get; set; }
       }

        public class Handler : IRequestHandler<Query, Result<List<DictionaryEntryDto>>>
        {
            public Handler()
            {
                
            }

            public Task<Result<List<DictionaryEntryDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}