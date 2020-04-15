using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Identity.Interfaces;
using Wallace.Domain.Queries;

namespace Wallace.Application.Commands.Accounts.RemoveAccount
{
    public class RemoveAccountCommand : IRequest<Guid>
    {
        /// <summary>
        /// ID of the account to remove.
        /// </summary>
        public Guid Id { get; set; }
    }

    public class RemoveAccountCommandHandler
        : IRequestHandler<RemoveAccountCommand, Guid>
    {
        private readonly IDbContext _dbContext;
        private readonly IIdentityAccessor _identityAccessor;

        public RemoveAccountCommandHandler(
            IDbContext dbContext,
            IIdentityAccessor identityAccessor
        )
        {
            _dbContext = dbContext;
            _identityAccessor = identityAccessor;
        }
        
        public async Task<Guid> Handle(
            RemoveAccountCommand request,
            CancellationToken cancellationToken
        )
        {
            var loggedInUserId = _identityAccessor.Get().Id;
            var account = _dbContext.Accounts.QueryAccountFor(
                loggedInUserId,
                request.Id
            );

            _dbContext.Accounts.Remove(account);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return request.Id;
;        }
    }
}