using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Exceptions;

namespace Wallace.Application.Queries.Accounts.GetAccount
{
    public class GetAccountQuery : IRequest<AccountDto>
    {
        public Guid Id { get; set; }
    }
    
    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, AccountDto>
    {
        private readonly IDbContext _dbContext;

        public GetAccountQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<AccountDto> Handle(
            GetAccountQuery request,
            CancellationToken cancellationToken
        )
        {
            var account = await _dbContext.Accounts.FindAsync(request.Id);
            
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