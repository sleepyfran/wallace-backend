using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallace.Application.Common.Dto;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Interfaces;
using Wallace.Domain.Queries;

namespace Wallace.Application.Common.Handlers
{
    /// <summary>
    /// Base handler for all request handlers that need to query one protected
    /// resource from the database.
    /// </summary>
    public abstract class AuthorizedQueryOneHandler<TRequest, TResult>
        : IRequestHandler<TRequest, TResult>
    where TRequest : IRequest<TResult>, IDto
    {
        protected readonly IDbContext DbContext;
        private readonly IIdentityAccessor _accessor;
        private readonly IMapper _mapper;

        protected AuthorizedQueryOneHandler(
            IDbContext dbContext,
            IIdentityAccessor accessor,
            IMapper mapper
        )
        {
            DbContext = dbContext;
            _accessor = accessor;
            _mapper = mapper;
        }

        /// <summary>
        /// Queries one entity belonging to the current logged in user from
        /// the database.
        /// </summary>
        /// <remarks>
        /// A mapper from TEntity to TEntityDto must be defined in order for the
        /// method to work.
        /// </remarks>
        /// <param name="request">
        /// Request given to the handler.
        /// </param>
        /// <param name="dbSet">
        /// DbSet of a given IOwnedEntity subclass in which the entity will
        /// be queried.
        /// </param>
        /// <returns>The entity requested, if it exists.</returns>
        protected async Task<TEntityDto> QueryOneForCurrentUser
            <TEntity, TEntityDto>
            (
                TRequest request,
                DbSet<TEntity> dbSet
            ) where TEntity : class, IOwnedEntity, new()
        {
            return await _accessor.WithCurrentIdentityId(userId =>
            {
                var entity = dbSet.QueryEntityFor(userId, request.Id);
                return Task.FromResult(
                    _mapper.Map<TEntityDto>(entity)
                );
            });
        }
        
        public abstract Task<TResult> Handle(
            TRequest request,
            CancellationToken cancellationToken
        );
    }
}