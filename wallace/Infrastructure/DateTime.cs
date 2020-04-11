using Wallace.Domain.Common.Interfaces;

namespace Wallace.Infrastructure
{
    public class DateTime : IDateTime
    {
        public System.DateTime UtcNow => System.DateTime.UtcNow;
    }
}