using System;
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
    /// Base handler for all commands that need to perform actions over a
    /// protected resource of the database.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class AuthorizedCommandHandler<TRequest, TResult>
        : IRequestHandler<TRequest, TResult>
    where TRequest : IRequest<TResult>, IDto
    {
        protected readonly IDbContext DbContext;
        private readonly IIdentityAccessor _accessor;
        private readonly IMapper _mapper;

        protected AuthorizedCommandHandler(
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
        /// Creates and saves into the database a new entity for the user
        /// currently logged in.
        /// </summary>
        /// <remarks>
        /// A mapper from TEntity to TRequest must be defined in order for the
        /// method to work.
        /// </remarks>
        /// <param name="request">
        /// Request given to the handler.
        /// </param>
        /// <param name="dbSet">
        /// DbSet of a given IOwnedEntity subclass in which the new entity will
        /// be created.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token given in the handler.
        /// </param>
        /// <returns>ID of the newly created entity</returns>
        protected async Task<Guid> CreateForCurrentUser<TEntity>(
            TRequest request,
            CancellationToken cancellationToken,
            DbSet<TEntity> dbSet
        ) where TEntity : class, IOwnedEntity, new()
        {
            return await _accessor.WithCurrentIdentityId(async userId =>
            {
                var entity = _mapper.Map(request, new TEntity
                {
                    OwnerId = userId
                });

                dbSet.Add(entity);
                await DbContext.SaveChangesAsync(cancellationToken);
            
                return entity.Id;
            });
        }

        /// <summary>
        /// Edits and saves into the database an entity that belongs to the
        /// current logged in user with the changes given.
        /// </summary>
        /// <remarks>
        /// A mapper from TEntity to TRequest must be defined in order for the
        /// method to work.
        /// </remarks>
        /// <param name="request">
        /// Request given to the handler.
        /// </param>
        /// <param name="dbSet">
        /// DbSet of a given IOwnedEntity subclass in which the entity will
        /// be edited.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token given in the handler.
        /// </param>
        /// <returns>ID of the edited entity</returns>
        protected async Task<Guid> EditForCurrentUser<TEntity>(
            TRequest request,
            CancellationToken cancellationToken,
            DbSet<TEntity> dbSet
        ) where TEntity : class, IOwnedEntity, new()
        {
            return await _accessor.WithCurrentIdentityId(async userId =>
            {
                var existingEntity = dbSet
                    .QueryEntityFor(userId, request.Id);

                dbSet.Update(
                    _mapper.Map(request, existingEntity)
                );
                await DbContext.SaveChangesAsync(cancellationToken);
                
                return request.Id;
            });
        }

        /// <summary>
        /// Removes an entity that belongs to the current logged in user from
        /// the database.
        /// </summary>
        /// <param name="request">
        /// Request given to the handler.
        /// </param>
        /// <param name="dbSet">
        /// DbSet of a given IOwnedEntity subclass in which the entity will
        /// be removed.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token given in the handler.
        /// </param>
        /// <returns>ID of the removed entity</returns>
        protected async Task<Guid> RemoveForCurrentUser<TEntity>(
            TRequest request,
            CancellationToken cancellationToken,
            DbSet<TEntity> dbSet
        ) where TEntity : class, IOwnedEntity, new()
        {
            return await _accessor.WithCurrentIdentityId(async userId =>
            {
                var entity = dbSet
                    .QueryEntityFor(userId, request.Id);

                dbSet.Remove(entity);
                await DbContext.SaveChangesAsync(cancellationToken);

                return request.Id;
            });
        }

        public abstract Task<TResult> Handle(
            TRequest request,
            CancellationToken cancellationToken
        );
    }
}