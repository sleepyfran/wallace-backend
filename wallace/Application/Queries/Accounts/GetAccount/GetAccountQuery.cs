using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wallace.Application.Common.Interfaces;
using Wallace.Application.Common.Dto;
using Wallace.Domain.Identity.Interfaces;
using static Wallace.Domain.Queries.UserAccounts;

namespace Wallace.Application.Queries.Accounts
{
    public class GetAccountQuery : IRequest<AccountDto>
    {
        public Guid Id { get; set; }
    }
    
    /// <summary>
    /// Retrieves an specific account given its ID only if it belongs to the
    /// current logged in user.
    /// </summary>
    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, AccountDto>
    {
        private readonly IDbContext _dbContext;
        private readonly IIdentityAccessor _identityAccessor;

        public GetAccountQueryHandler(
            IDbContext dbContext,
            IIdentityAccessor identityAccessor
        )
        {
            _dbContext = dbContext;
            _identityAccessor = identityAccessor;
        }
        
        public Task<AccountDto> Handle(
            GetAccountQuery request,
            CancellationToken cancellationToken
        )
        {
            var loggedInUserId = _identityAccessor.Get().Id;
            var account = _dbContext.Accounts
                .QueryAccountFor(loggedInUserId, request.Id);
            
            return Task.FromResult(new AccountDto
            {
                Id = account.Id,
                Balance = account.Balance.Amount,
                Currency = account.Balance.Currency.Code,
                Name = account.Name,
                Owner = account.OwnerId
            });
        }
    }
}