using System;
using System.Linq;
using Wallace.Domain.Entities;
using Wallace.Domain.Exceptions;

namespace Wallace.Domain.Queries
{
    /// <summary>
    /// Defines a set of queryable extensions for any entity that implements
    /// the IOwnedEntity interface.
    /// </summary>
    public static class OwnedEntity
    {
        public static T QueryEntityFor<T>(
            this IQueryable<T> entities,
            Guid ownerId,
            Guid entityId
        ) where T : IOwnedEntity
        {
            var entity = entities
                .QueryEntitiesFor(ownerId)
                .FirstOrDefault(e => e.Id == entityId);

            return entity ?? throw new EntityNotFoundException();
        }

        public static IQueryable<T> QueryEntitiesFor<T>(
            this IQueryable<T> entities,
            Guid ownerId
        ) where T : IOwnedEntity
        {
            return entities
                .Where(e => e.OwnerId == ownerId);
        }
    }
}