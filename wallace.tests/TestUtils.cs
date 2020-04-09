using System;
using System.Linq;

namespace Wallace.Tests
{
    public class TestUtils
    {
        /// <summary>
        /// Generates a random string with the given number of characters.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomStringWith(int length)
        {
            var random = new Random();
            const string characters = "abcdefghijklmnopqrstwxyz";
            
            return new string(
                Enumerable
                    .Repeat(characters, length)
                    .Select(c => c[random.Next(c.Length)])
                    .ToArray()
            );
        }
    }
}