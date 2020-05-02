using System;

namespace Wallace.Application.Common.Dto
{
    public class PayeeDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Owner { get; set; }
    }
}