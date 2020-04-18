using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Handlers;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Queries.Categories
{
    public class GetCategoryQuery : IRequest<CategoryDto>, IDto
    {
        public Guid Id { get; set; }
    }

    public class GetCategoryQueryHandler
        : AuthorizedQueryOneHandler<GetCategoryQuery, CategoryDto>
    {
        public GetCategoryQueryHandler(
            IDbContext dbContext,
            IIdentityAccessor identityAccessor,
            IMapper mapper
        ) : base(dbContext, identityAccessor, mapper) { }
        
        public override async Task<CategoryDto> Handle(
            GetCategoryQuery request,
            CancellationToken cancellationToken
        ) => await QueryOneForCurrentUser<Category, CategoryDto>(
            request,
            DbContext.Categories
        );
    }
}