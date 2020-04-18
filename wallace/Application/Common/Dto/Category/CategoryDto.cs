using System;

namespace Wallace.Application.Common.Dto
{
    public class CategoryDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Emoji { get; set; }
        public Guid Owner { get; set; }
    }
}