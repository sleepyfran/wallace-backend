using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Queries.Categories
{
    public class GetCategoryQuery : IRequest<CategoryDto>
    {
        public Guid Id { get; set; }
    }

    public class GetCategoryQueryHandler
        : IRequestHandler<GetCategoryQuery, CategoryDto>
    {
        private readonly IDbContext _dbContext;
        private readonly IIdentityAccessor _identityAccessor;
        private readonly IMapper _mapper;

        public GetCategoryQueryHandler(
            IDbContext dbContext,
            IIdentityAccessor identityAccessor,
            IMapper mapper
        )
        {
            _dbContext = dbContext;
            _identityAccessor = identityAccessor;
            _mapper = mapper;
        }
        
        public Task<CategoryDto> Handle(
            GetCategoryQuery request,
            CancellationToken cancellationToken
        )
        {
            var loggedInUserId = _identityAccessor.Get().Id;
            var category = _dbContext.Categories
                .Where(c => c.OwnerId == loggedInUserId)
                .FirstOrDefault(c => c.Id == request.Id);

            return Task.FromResult(_mapper.Map<CategoryDto>(category));
        }
    }
}