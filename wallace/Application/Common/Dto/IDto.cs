using System;

namespace Wallace.Application.Common.Dto
{
    /// <summary>
    /// Base properties that all DTOs must include.
    /// </summary>
    public interface IDto
    {
        Guid Id { get; set; }
    }
}