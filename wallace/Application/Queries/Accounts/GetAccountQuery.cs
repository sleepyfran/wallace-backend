using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wallace.Application.Common.Interfaces;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Handlers;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Queries.Accounts
{
    public class GetAccountQuery : IRequest<AccountDto>, IDto
    {
        public Guid Id { get; set; }
    }
    
    /// <summary>
    /// Retrieves an specific account given its ID only if it belongs to the
    /// current logged in user.
    /// </summary>
    public class GetAccountQueryHandler
        : AuthorizedQueryOneHandler<GetAccountQuery, AccountDto>
    {
        public GetAccountQueryHandler(
            IDbContext dbContext,
            IIdentityAccessor identityAccessor,
            IMapper mapper
        ) : base(dbContext, identityAccessor, mapper) { }

        public override async Task<AccountDto> Handle(
            GetAccountQuery request,
            CancellationToken cancellationToken
        ) => await QueryOneForCurrentUser<Account, AccountDto>(
            request,
            DbContext.Accounts
        );
    }
}