using System;

namespace Wallace.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity (unknown, T type) could not be found.
    /// </summary>
    public class EntityNotFoundException : Exception { }
}