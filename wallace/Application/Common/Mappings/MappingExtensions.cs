using System;

namespace Wallace.Application.Common.Mappings
{
    public static class MappingExtensions
    {
        /// <summary>
        /// Returns null if the given Guid is Guid.Empty. Useful for when
        /// mapping into a database that does not accept Guid.Empty as a valid
        /// Guid but is nullable.
        /// </summary>
        public static Guid? NullIfEmpty(this Guid guid) => guid == Guid.Empty
            ? null as Guid?
            : guid;
    }
}