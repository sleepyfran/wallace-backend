using System.Collections.Generic;
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
    public class GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>> { }

    public class GetCategoriesQueryHandler
        : AuthorizedQueryManyHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
    {
        public GetCategoriesQueryHandler(
            IDbContext dbContext,
            IIdentityAccessor accessor,
            IMapper mapper) : base(dbContext, accessor, mapper) { }

        public override async Task<IEnumerable<CategoryDto>> Handle(
            GetCategoriesQuery request,
            CancellationToken cancellationToken
        ) => await QueryManyForCurrentUser<Category, CategoryDto>(
            cancellationToken,
            DbContext.Categories
        );
    }
}