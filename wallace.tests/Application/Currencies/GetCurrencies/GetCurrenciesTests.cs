using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Wallace.Application.Queries.Categories;
using Wallace.Application.Queries.Currencies;

namespace Wallace.Tests.Application.Currencies.GetCurrencies
{
    public class GetCurrenciesTests : BaseTest
    {
        private GetCurrenciesQueryHandler _handler;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();

            _handler = new GetCurrenciesQueryHandler();
        }

        [Test]
        public async Task Handle_ShouldReturnValidCurrencies()
        {
            var currencies = await _handler.Handle(
                new GetCurrenciesQuery(),
                CancellationToken.None
            );

            // Since we won't check every single currency, let's just see that
            // some of them are included.
            var eur = currencies.First(c => c.Code == "EUR");
            var usd = currencies.First(c => c.Code == "USD");
            var czk = currencies.First(c => c.Code == "CZK");

            Assert.AreEqual(eur.Code, "EUR");
            Assert.AreEqual(eur.Symbol, "€");
            Assert.AreEqual(eur.Name, "Euro");

            Assert.AreEqual(usd.Code, "USD");
            Assert.AreEqual(usd.Symbol, "$");
            Assert.AreEqual(usd.Name, "US Dollar");

            Assert.AreEqual(czk.Code, "CZK");
            Assert.AreEqual(czk.Symbol, "Kč");
            Assert.AreEqual(czk.Name, "Czech Koruna");
        }
    }
}
