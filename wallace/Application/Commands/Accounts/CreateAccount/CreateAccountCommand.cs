using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wallace.Application.Common.Interfaces;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Handlers;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Accounts.CreateAccount
{
    public class CreateAccountCommand : AccountDto, IRequest<Guid> { }

    public class CreateAccountCommandHandler
        : AuthorizedCommandHandler<CreateAccountCommand, Guid>
    {
        public CreateAccountCommandHandler(
            IDbContext dbContext,
            IIdentityAccessor identityAccessor,
            IMapper mapper
        ) : base(dbContext, identityAccessor, mapper) { }
        
        public override async Task<Guid> Handle(
            CreateAccountCommand request,
            CancellationToken cancellationToken
        ) => await CreateForCurrentUser(
            request,
            cancellationToken,
            DbContext.Accounts
        );
    }
}