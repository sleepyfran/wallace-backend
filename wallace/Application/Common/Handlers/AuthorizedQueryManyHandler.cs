using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Entities;
using Wallace.Domain.Identity.Interfaces;
using Wallace.Domain.Queries;

namespace Wallace.Application.Common.Handlers
{
    /// <summary>
    /// Base handler for all request handlers that need to query many protected
    /// resources from the database.
    /// </summary>
    public abstract class AuthorizedQueryManyHandler<TRequest, TResult>
        : IRequestHandler<TRequest, TResult>
        where TRequest : IRequest<TResult>
    {
        protected readonly IDbContext DbContext;
        private readonly IIdentityAccessor _accessor;
        private readonly IMapper _mapper;

        protected AuthorizedQueryManyHandler(
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
        /// Queries multiple entities belonging to the current logged in user
        /// from the database.
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
        /// <param name="cancellationToken">
        /// Cancellation token given in the handler.
        /// </param>
        /// <returns>All the entities belonging to the user.</returns>
        protected IQueryable<TEntityDto> QueryManyForCurrentUser
            <TEntity, TEntityDto>
            (
                DbSet<TEntity> dbSet
            ) where TEntity : class, IOwnedEntity, new() =>
            _accessor.WithCurrentIdentityId(userId =>
                dbSet
                    .QueryEntitiesFor(userId)
                    .ProjectTo<TEntityDto>(_mapper.ConfigurationProvider)
            );

        public abstract Task<TResult> Handle(
            TRequest request,
            CancellationToken cancellationToken
        );
    }
}