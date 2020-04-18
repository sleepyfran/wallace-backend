using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Interfaces;
using Wallace.Domain.Queries;

namespace Wallace.Application.Commands.Accounts.EditAccount
{
    public class EditAccountCommand : AccountDto, IRequest<Guid>
    {
        public Guid QueryId { get; set; }
    }

    public class EditAccountCommandHandler
        : IRequestHandler<EditAccountCommand, Guid>
    {
        private readonly IDbContext _dbContext;
        private readonly IIdentityAccessor _identityAccessor;
        private readonly IMapper _mapper;

        public EditAccountCommandHandler(
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
            EditAccountCommand request,
            CancellationToken cancellationToken
        )
        {
            var loggedInUser = _identityAccessor.Get().Id;
            var existingAccount = _dbContext.Accounts
                .QueryEntityFor(loggedInUser, request.Id);
            
            _dbContext.Accounts.Update(
                _mapper.Map(request, existingAccount)
            );
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            return request.Id;
        }
    }
}