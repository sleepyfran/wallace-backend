using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wallace.Application.Common;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Handlers;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Accounts.EditAccount
{
    public class EditAccountCommand : AccountDto, IRequest<Guid>
    {
        public Guid QueryId { get; set; }
    }

    public class EditAccountCommandHandler
        : AuthorizedCommandHandler<EditAccountCommand, Guid>
    {
        public EditAccountCommandHandler(
            IDbContext dbContext,
            IIdentityAccessor identityAccessor,
            IMapper mapper
        ) : base(dbContext, identityAccessor, mapper) { }

        public override async Task<Guid> Handle(
            EditAccountCommand request,
            CancellationToken cancellationToken
        ) => await EditForCurrentUser(
            request,
            cancellationToken,
            DbContext.Accounts
        );
    }
}