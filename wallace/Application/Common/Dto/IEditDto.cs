using System;

namespace Wallace.Application.Common.Dto
{
    /// <summary>
    /// Base properties that all DTOs allowing edition of entities must include.
    /// </summary>
    public interface IEditDto : IDto
    {
        public Guid QueryId { get; set; }
    }
}