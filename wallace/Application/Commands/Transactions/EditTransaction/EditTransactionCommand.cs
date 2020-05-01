using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Handlers;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Transactions.EditTransaction
{
    public class EditTransactionCommand : TransactionDto, IRequest<Guid>,
        IEditDto
    {
        public Guid QueryId { get; set; }
    }

    public class EditTransactionCommandHandler
        : AuthorizedCommandHandler<EditTransactionCommand, Guid>
    {
        public EditTransactionCommandHandler(
            IDbContext dbContext,
            IIdentityAccessor accessor,
            IMapper mapper) : base(dbContext, accessor, mapper)
        {
        }

        public override async Task<Guid> Handle(
            EditTransactionCommand request,
            CancellationToken cancellationToken
        ) => await EditForCurrentUser(
            request,
            cancellationToken,
            DbContext.Transactions
        );
    }
}