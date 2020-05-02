using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Handlers;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Queries.Payees
{
    public class GetPayeesQuery : IRequest<IEnumerable<PayeeDto>>
    {
    }

    public class GetPayeesQueryHandler
        : AuthorizedQueryManyHandler<GetPayeesQuery,
            IEnumerable<PayeeDto>>
    {
        public GetPayeesQueryHandler(
            IDbContext dbContext,
            IIdentityAccessor accessor,
            IMapper mapper) : base(dbContext, accessor, mapper)
        {
        }

        public override async Task<IEnumerable<PayeeDto>> Handle(
            GetPayeesQuery request,
            CancellationToken cancellationToken
        ) => await QueryManyForCurrentUser<Payee, PayeeDto>(
            DbContext.Payees
        ).ToListAsync(cancellationToken);
    }
}