using System;

namespace Wallace.Domain.Entities
{
    /// <summary>
    /// Defines an entity that has an owner.
    /// </summary>
    public interface IOwnedEntity : IEntity
    {
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
    }
}