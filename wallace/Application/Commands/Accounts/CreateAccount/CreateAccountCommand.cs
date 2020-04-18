using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NodaMoney;
using Wallace.Application.Common.Interfaces;
using Wallace.Application.Common.Dto;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Accounts.CreateAccount
{
    public class CreateAccountCommand : AccountDto, IRequest<Guid> { }

    public class CreateAccountCommandHandler
        : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly IDbContext _dbContext;
        private readonly IIdentityAccessor _identityAccessor;
        private readonly IMapper _mapper;

        public CreateAccountCommandHandler(
            IDbContext dbContext,
            IIdentityAccessor identityAccessor,
            IMapper mapper
        )
        {
            _dbContext = dbContext;
            _identityAccessor = identityAccessor;
            _mapper = mapper;
        }
    
        public async Task<Guid> Handle(
            CreateAccountCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = await _dbContext.Users
                .FindAsync(_identityAccessor.Get().Id);
            var account = _mapper.Map(request, new Account
            {
                OwnerId = user.Id
            });

            _dbContext.Accounts.Add(account);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return account.Id;
        }
    }
}