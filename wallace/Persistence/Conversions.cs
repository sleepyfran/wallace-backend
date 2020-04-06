using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaMoney;

namespace Wallace.Persistence
{
    public static class Conversions
    {
        /// <summary>
        /// Conversion from the Money data type to a string and back using its
        /// utility methods.
        /// </summary>
        public static ValueConverter<Money, string> MoneyToString =
            new ValueConverter<Money, string>(
                m => m.ToString(),
                s => Money.Parse(s)
            );
    }
}