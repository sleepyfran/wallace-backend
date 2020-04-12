using NUnit.Framework;
using Wallace.Domain.ValueObjects;

namespace Wallace.Tests.Domain.ValueObjects
{
    public class DaysTests
    {
        [Test]
        public void Seconds_ShouldReturnCorrectValue(
            [Values(15, 305, 200, 1, 10)]
            int input
        )
        {
            var days = new Days(input);
            Assert.AreEqual(input * 24 * 60, days.Seconds);
        }

        [Test]
        public void Minutes_ShouldReturnCorrectValue(
            [Values(15, 305, 200, 1, 10)]
            int input
        )
        {
            var days = new Days(input);
            Assert.AreEqual(input * 24, (int)days.Minutes);
        }
    }
}