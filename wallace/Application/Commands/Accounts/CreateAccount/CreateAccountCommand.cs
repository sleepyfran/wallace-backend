using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NodaMoney;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Entities;
using Wallace.Domain.Exceptions;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Accounts.CreateAccount
{
    public class CreateAccountCommand : IRequest<Guid>
    {
        /// <summary>
        /// Name assigned to the account.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Current balance of the account,
        /// </summary>
        public decimal Balance { get; set; }
        
        /// <summary>
        /// Currency code. Ex: EUR, CZK, USD, etc.
        /// </summary>
        public string Currency { get; set; }
    }

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly IDbContext _dbContext;
        private readonly IIdentityAccessor _identityAccessor;

        public CreateAccountCommandHandler(
            IDbContext dbContext,
            IIdentityAccessor identityAccessor
        )
        {
            _dbContext = dbContext;
            _identityAccessor = identityAccessor;
        }
    
        public async Task<Guid> Handle(
            CreateAccountCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = _dbContext.Users.Find(_identityAccessor.Get().Id);
            var account = new Account
            {
                Name = request.Name,
                Balance = new Money(request.Balance, request.Currency),
                OwnerId = user.Id
            };

            _dbContext.Accounts.Add(account);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return account.Id;
        }
    }
}