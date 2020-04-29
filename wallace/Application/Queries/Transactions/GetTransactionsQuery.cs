using System.Collections.Generic;
using System.Linq;
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

namespace Wallace.Application.Queries.Transactions
{
    public class GetTransactionsQuery : IRequest<IEnumerable<TransactionDto>>
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class GetTransactionsQueryHandler
        : AuthorizedQueryManyHandler<GetTransactionsQuery,
            IEnumerable<TransactionDto>>
    {
        public GetTransactionsQueryHandler(
            IDbContext dbContext,
            IIdentityAccessor accessor,
            IMapper mapper) : base(dbContext, accessor, mapper)
        {
        }

        public override async Task<IEnumerable<TransactionDto>> Handle(
            GetTransactionsQuery request,
            CancellationToken cancellationToken
        ) => await QueryManyForCurrentUser<Transaction, TransactionDto>(
            DbContext.Transactions
        )
            .Where(t => 
                t.Date.Month == request.Month && 
                t.Date.Year == request.Year
            )
            .ToListAsync(cancellationToken);
    }
}