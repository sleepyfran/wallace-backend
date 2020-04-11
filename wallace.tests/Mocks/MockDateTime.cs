using System;
using Wallace.Domain.Common.Interfaces;

namespace Wallace.Tests.Mocks
{
    public class MockDateTime : IDateTime
    {
        private readonly DateTime _dateTime;

        public MockDateTime(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public DateTime UtcNow => _dateTime;
    }
}