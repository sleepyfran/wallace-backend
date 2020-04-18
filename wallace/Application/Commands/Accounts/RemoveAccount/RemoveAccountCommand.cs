using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Handlers;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Accounts.RemoveAccount
{
    public class RemoveAccountCommand : IRequest<Guid>, IDto
    {
        /// <summary>
        /// ID of the account to remove.
        /// </summary>
        public Guid Id { get; set; }
    }

    public class RemoveAccountCommandHandler
        : AuthorizedCommandHandler<RemoveAccountCommand, Guid>
    {
        public RemoveAccountCommandHandler(
            IDbContext dbContext,
            IIdentityAccessor identityAccessor,
            IMapper mapper
        ) : base(dbContext, identityAccessor, mapper) { }
        
        public override async Task<Guid> Handle(
            RemoveAccountCommand request,
            CancellationToken cancellationToken
        ) => await RemoveForCurrentUser(
            request,
            cancellationToken,
            DbContext.Accounts
        );
    }
}