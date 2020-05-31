using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using Wallace.Application.Common.Dto;
using Wallace.Application.Middleware;
using Wallace.Application.Queries.Categories;
using Wallace.Application.Queries.Currencies;

namespace Wallace.Tests.Application.Cache
{
    public class CacheRequestBehaviorsTests : BaseTest
    {
        [Test]
        public async Task ShouldCacheValuesAfterInitialCallIfCacheable()
        {
            var sut = new CacheRequestBehavior<GetCurrenciesQuery, List<CurrencyDto>>();

            var nextMock = new Mock<RequestHandlerDelegate<List<CurrencyDto>>>();
            await sut.Handle(
                new GetCurrenciesQuery(),
                CancellationToken.None,
                nextMock.Object
            );

            await sut.Handle(
                new GetCurrenciesQuery(),
                CancellationToken.None,
                nextMock.Object
            );

            nextMock.Verify(m => m(), Times.Once);
        }

        [Test]
        public async Task ShouldIgnoreRequestsThatAreNotCacheable()
        {
            var sut = new CacheRequestBehavior<GetCategoriesQuery, IEnumerable<CategoryDto>>();

            var nextMock = new Mock<RequestHandlerDelegate<IEnumerable<CategoryDto>>>();
            await sut.Handle(
                new GetCategoriesQuery(),
                CancellationToken.None,
                nextMock.Object
            );

            await sut.Handle(
                new GetCategoriesQuery(),
                CancellationToken.None,
                nextMock.Object
            );

            nextMock.Verify(m => m(), Times.Exactly(2));
        }
    }
}
