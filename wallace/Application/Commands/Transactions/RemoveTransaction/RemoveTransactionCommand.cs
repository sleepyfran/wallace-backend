using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Handlers;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Transactions.RemoveTransaction
{
    public class RemoveTransactionCommand : IRequest<Guid>, IDto
    {
        public Guid Id { get; set; }
    }

    public class RemoveTransactionCommandHandler
        : AuthorizedCommandHandler<RemoveTransactionCommand, Guid>
    {
        public RemoveTransactionCommandHandler(
            IDbContext dbContext,
            IIdentityAccessor accessor,
            IMapper mapper) : base(dbContext, accessor, mapper) { }

        public override async Task<Guid> Handle(
            RemoveTransactionCommand request,
            CancellationToken cancellationToken
        ) => await RemoveForCurrentUser(
            request,
            cancellationToken,
            DbContext.Transactions
        );
    }
}