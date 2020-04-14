using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NodaMoney;
using Wallace.Application.Common.Interfaces;
using Wallace.Application.Common.Dto;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Accounts.CreateAccount
{
    public class CreateAccountCommand : AccountDto, IRequest<Guid> { }

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
            var user = await _dbContext.Users
                .FindAsync(_identityAccessor.Get().Id);
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