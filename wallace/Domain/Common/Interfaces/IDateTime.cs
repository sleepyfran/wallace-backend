using System;

namespace Wallace.Domain.Common.Interfaces
{
    /// <summary>
    /// Abstraction over the system's DateTime so we can control the time
    /// in tests.
    /// </summary>
    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }
}