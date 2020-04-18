using System.Collections.Generic;
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
    public class GetAccountsQuery : IRequest<IEnumerable<AccountDto>> { }

    /// <summary>
    /// Retrieves all the accounts associated to the current logged in user.
    /// </summary>
    public class GetAccountsQueryHandler 
        : AuthorizedQueryManyHandler<GetAccountsQuery, IEnumerable<AccountDto>>
    {
        public GetAccountsQueryHandler(
            IDbContext dbContext,
            IIdentityAccessor identityAccessor,
            IMapper mapper
        ) : base(dbContext, identityAccessor, mapper) { }

        public override async Task<IEnumerable<AccountDto>> Handle(
            GetAccountsQuery _,
            CancellationToken cancellationToken
        ) => await QueryManyForCurrentUser<Account, AccountDto>(
            cancellationToken,
            DbContext.Accounts
        );
    }
}