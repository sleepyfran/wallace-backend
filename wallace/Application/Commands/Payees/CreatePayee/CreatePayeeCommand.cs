using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Handlers;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Payees
{
    public class CreatePayeeCommand : PayeeDto, IRequest<Guid> { }

    public class CreatePayeeCommandHandler
        : AuthorizedCommandHandler<CreatePayeeCommand, Guid>
    {
        public CreatePayeeCommandHandler(
            IDbContext dbContext,
            IIdentityAccessor identityAccessor,
            IMapper mapper
        ) : base(dbContext, identityAccessor, mapper) { }
        
        public override async Task<Guid> Handle(
            CreatePayeeCommand request,
            CancellationToken cancellationToken
        ) => await CreateForCurrentUser(
            request,
            cancellationToken,
            DbContext.Payees
        );
    }
}