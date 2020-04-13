using System;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Wallace.Tests
{
    public static class ValidatorTestExtensions
    {
        /// <summary>
        /// Tests that the property given by the expression has validation errors
        /// when the input is empty.
        /// </summary>
        public static void ShouldHaveValidationErrorForEmpty<T>(
            this AbstractValidator<T> validator,
            Expression<Func<T, string>> expression
        ) where T : class, new()
        {
            validator.ShouldHaveValidationErrorFor(
                expression,
                null as string
            );

            validator.ShouldHaveValidationErrorFor(
                expression,
                string.Empty
            );
        }

        /// <summary>
        /// Tests that the property given by the expression has validation errors
        /// when the input has less than N characters.
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="expression"></param>
        /// <param name="characters"></param>
        /// <typeparam name="T"></typeparam>
        public static void ShouldHaveValidationErrorForLessThanCharacters<T>(
            this AbstractValidator<T> validator,
            Expression<Func<T, string>> expression,
            int characters
        ) where T : class, new()
        {
            for (var chars = 1; chars < characters; chars++)
            {
                var randomString = TestUtils.GenerateRandomStringWith(chars);

                validator.ShouldHaveValidationErrorFor(
                    expression,
                    randomString
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="expression"></param>
        /// <param name="characters"></param>
        /// <typeparam name="T"></typeparam>
        public static void ShouldHaveValidationErrorForMoreThanCharacters<T>(
            this AbstractValidator<T> validator,
            Expression<Func<T, string>> expression,
            int characters
        ) where T : class, new()
        {
            validator.ShouldNotHaveValidationErrorFor(
                expression,
                TestUtils.GenerateRandomStringWith(characters - 1)
            );
            
            validator.ShouldNotHaveValidationErrorFor(
                expression,
                TestUtils.GenerateRandomStringWith(characters)
            );
            
            validator.ShouldHaveValidationErrorFor(
                expression,
                TestUtils.GenerateRandomStringWith(characters + 1)
            );
        }
    }
}