using System;

namespace Wallace.Domain.Entities
{
    /// <summary>
    /// Defines the basic properties of all entities.
    /// </summary>
    public interface IEntity
    {
        public Guid Id { get; set; }
    }
}