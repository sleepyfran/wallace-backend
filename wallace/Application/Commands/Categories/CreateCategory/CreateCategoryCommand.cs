using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Categories.CreateCategory
{
    public class CreateCategoryCommand : CategoryDto, IRequest<Guid> { }

    public class CreateCategoryCommandHandler
        : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly IDbContext _dbContext;
        private readonly IIdentityAccessor _identityAccessor;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(
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
            CreateCategoryCommand request,
            CancellationToken cancellationToken
        )
        {
            var user = await _dbContext.Users
                .FindAsync(_identityAccessor.Get().Id);
            var category = _mapper.Map(request, new Category
            {
                OwnerId = user.Id
            });

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return category.Id;
        }
    }
}