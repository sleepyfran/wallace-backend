using FluentValidation;
using Wallace.Application.Common.Dto;

namespace Wallace.Application.Common.Validators
{
    public static class QueryIdValidator
    {
        /// <summary>
        /// Checks whether the QueryId and Id of a given IEditDto match.
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IRuleBuilderInitial<T, T> MustHaveMatchingIds<T>(
            this IRuleBuilder<T, T> ruleBuilder
        ) where T : IEditDto => ruleBuilder
            .Custom((c, context) =>
            {
                if (c.QueryId != c.Id)
                    context.AddFailure(
                        "QueryId",
                        "The ID given in the URL should match the one given in the body"
                    );
            });
    }
}