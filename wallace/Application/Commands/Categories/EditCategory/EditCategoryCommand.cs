using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Handlers;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Commands.Categories.EditCategory
{
    public class EditCategoryCommand : CategoryDto, IRequest<Guid>, IEditDto
    {
        public Guid QueryId { get; set; }
    }

    public class EditCategoryCommandHandler
        : AuthorizedCommandHandler<EditCategoryCommand, Guid>
    {
        public EditCategoryCommandHandler(
            IDbContext dbContext,
            IIdentityAccessor accessor,
            IMapper mapper) : base(dbContext, accessor, mapper) { }

        public override async Task<Guid> Handle(
            EditCategoryCommand request,
            CancellationToken cancellationToken
        ) => await EditForCurrentUser(
            request,
            cancellationToken,
            DbContext.Categories
        );
    }
}