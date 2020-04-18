using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Handlers;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Categories.CreateCategory
{
    public class CreateCategoryCommand : CategoryDto, IRequest<Guid> { }

    public class CreateCategoryCommandHandler
        : AuthorizedCommandHandler<CreateCategoryCommand, Guid>
    {
        public CreateCategoryCommandHandler(
            IDbContext dbContext,
            IIdentityAccessor identityAccessor,
            IMapper mapper
        ) : base(dbContext, identityAccessor, mapper) { }
        
        public override async Task<Guid> Handle(
            CreateCategoryCommand request,
            CancellationToken cancellationToken
        ) => await CreateForCurrentUser(
            request,
            cancellationToken,
            DbContext.Categories
        );
    }
}