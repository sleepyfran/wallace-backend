using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Handlers;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Transactions.CreateTransaction
{
    public class CreateTransactionCommand : TransactionDto, IRequest<Guid> { }

    public class CreateTransactionCommandHandler
        : AuthorizedCommandHandler<CreateTransactionCommand, Guid>
    {
        public CreateTransactionCommandHandler(
            IDbContext dbContext,
            IIdentityAccessor accessor,
            IMapper mapper) : base(dbContext, accessor, mapper) { }

        public override async Task<Guid> Handle(
            CreateTransactionCommand request,
            CancellationToken cancellationToken
        ) => await CreateForCurrentUser(
            request,
            cancellationToken,
            DbContext.Transactions
        );
    }
}