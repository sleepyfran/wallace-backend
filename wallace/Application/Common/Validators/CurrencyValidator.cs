using System.Collections.Generic;
using FluentValidation;
using NodaMoney;

namespace Wallace.Application.Common.Validators
{
    public static class CurrencyValidator
    {
        /// <summary>
        /// Checks whether the given property is a valid currency or not.
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> MustBeValidCurrency<T>(
            this IRuleBuilder<T, string> ruleBuilder
        ) => ruleBuilder
                .Must(BeValid)
                .WithMessage("The given currency is not valid. Provide a valid currency code. Example: EUR, USD, CZK, etc.");

        private static bool BeValid(string currency)
        {
            try
            {
                Currency.FromCode(currency);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}