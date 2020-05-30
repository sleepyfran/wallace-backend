using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wallace.Application.Common.Dto;

namespace Wallace.Application.Queries.Currencies
{
    public class GetCurrenciesQuery : IRequest<IEnumerable<CurrencyDto>> { }

    /**
     * Returns all the currencies available in the RegionInfo of the machine.
     * This might give problems in the long run since a machine might not have
     * all regions available, but it should not be something to worry right now.
     */
    public class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, IEnumerable<CurrencyDto>>
    {
        public Task<IEnumerable<CurrencyDto>> Handle(
            GetCurrenciesQuery request,
            CancellationToken cancellationToken
        )
        {
            // TODO: Cache values.
            var currencies = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Select(ci => ci.LCID)
                .Distinct()
                .Select(cid =>
                {
                    try
                    {
                        return new RegionInfo(cid);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null)
                .GroupBy(ri => ri.CurrencyEnglishName)
                .Select(g => g.First())
                .Select(ri => new CurrencyDto
                {
                    Code = ri.ISOCurrencySymbol,
                    Name = ri.CurrencyEnglishName,
                    Symbol = ri.CurrencySymbol
                });

            return Task.FromResult(currencies);
        }
    }
}
