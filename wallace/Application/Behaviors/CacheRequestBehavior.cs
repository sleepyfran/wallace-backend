using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wallace.Application.Queries.Currencies;

namespace Wallace.Application.Middleware
{
    public class CacheRequestBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<Type> _cacheableRequests;
        private readonly Dictionary<Type, TResponse> _cache;

        public CacheRequestBehavior(IEnumerable<Type> cacheableRequests)
        {
            _cacheableRequests = cacheableRequests.Any()
                ? cacheableRequests
                : new List<Type> {
                    typeof(GetCurrenciesQuery)
                }; ;
            _cache = new Dictionary<Type, TResponse>();
        }

        public CacheRequestBehavior() : this(new List<Type>()) { }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        )
        {
            var requestType = request.GetType();
            if (_cacheableRequests.Any(r => r == requestType))
            {
                if (_cache.ContainsKey(requestType))
                    return _cache[requestType];

                var response = await next();
                _cache.Add(requestType, response);
                return response;
            }

            return await next();
        }
    }
}
