using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using MediatR;

namespace Application.DataObjectHandling.Terms
{
    public class AbstractUpdateTerm
    {
        public class Command : IRequest<Result<Unit>>
        {

        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            public Handler()
            {
            }

            public Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}