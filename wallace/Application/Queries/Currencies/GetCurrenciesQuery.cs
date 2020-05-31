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
    public class GetCurrenciesQuery : IRequest<List<CurrencyDto>> { }

    /**
     * Returns all the currencies available in the RegionInfo of the machine.
     * This might give problems in the long run since a machine might not have
     * all regions available, but it should not be something to worry right now.
     */
    public class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, List<CurrencyDto>>
    {
        public Task<List<CurrencyDto>> Handle(
            GetCurrenciesQuery request,
            CancellationToken cancellationToken
        )
        {
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
                .Where(ri => ri != null && ri.CurrencyEnglishName != string.Empty)
                .GroupBy(ri => ri.CurrencyEnglishName)
                .Select(g => g.First())
                .OrderBy(ri => ri.CurrencyEnglishName)
                .Select(ri => new CurrencyDto
                {
                    Code = ri.ISOCurrencySymbol,
                    Name = ri.CurrencyEnglishName,
                    Symbol = ri.CurrencySymbol
                })
                .ToList();

            return Task.FromResult(currencies);
        }
    }
}
