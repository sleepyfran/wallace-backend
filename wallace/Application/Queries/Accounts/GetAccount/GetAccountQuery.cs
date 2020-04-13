using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Exceptions;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Queries.Accounts.GetAccount
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
        
        public async Task<AccountDto> Handle(
            GetAccountQuery request,
            CancellationToken cancellationToken
        )
        {
            var loggedInUserId = _identityAccessor.Get().Id;
            var account = await _dbContext.Accounts
                .Where(a => a.OwnerId == loggedInUserId)
                .FirstOrDefaultAsync(
                    a => a.Id == request.Id,
                    cancellationToken
                );
            
            if (account == null)
                throw new AccountNotFoundException();
            
            return new AccountDto
            {
                Id = account.Id,
                Balance = account.Balance.Amount,
                Currency = account.Balance.Currency.Code,
                Name = account.Name,
                Owner = account.OwnerId
            };
        }
    }
}