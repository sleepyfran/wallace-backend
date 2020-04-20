using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Wallace.Domain.Entities;

namespace Wallace.Application.Common.Validators
{
    public static class IdValidator
    {
        public static IRuleBuilderOptions<T, Guid> MustBeValidIdReference
        <T, TEntity>(
            this IRuleBuilder<T, Guid> ruleBuilder,
            DbSet<TEntity> referenceSet
        ) where TEntity : class, IEntity => ruleBuilder
            .MustAsync(
                (id, cancellationToken) => 
                    BeValidReference(id, cancellationToken, referenceSet)
            )
            .WithMessage("The GUID is not a valid reference or does not exist in the database");

        private static async Task<bool> BeValidReference<T>(
            Guid id,
            CancellationToken cancellationToken,
            DbSet<T> referenceSet
        ) where T : class, IEntity => await referenceSet.AnyAsync(
            e => e.Id == id,
            cancellationToken
        );
    }
}