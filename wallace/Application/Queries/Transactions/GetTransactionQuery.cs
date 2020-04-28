using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Handlers;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Queries.Transactions
{
    public class GetTransactionQuery : IRequest<TransactionDto>, IDto
    {
        public Guid Id { get; set; }
    }

    public class GetTransactionQueryHandler
        : AuthorizedQueryOneHandler<GetTransactionQuery, TransactionDto>
    {
        public GetTransactionQueryHandler(
            IDbContext dbContext,
            IIdentityAccessor identityAccessor,
            IMapper mapper
        ) : base(dbContext, identityAccessor, mapper) { }
        
        public override async Task<TransactionDto> Handle(
            GetTransactionQuery request,
            CancellationToken cancellationToken
        ) => await QueryOneForCurrentUser<Transaction, TransactionDto>(
            request,
            DbContext.Transactions
        );
    }
}