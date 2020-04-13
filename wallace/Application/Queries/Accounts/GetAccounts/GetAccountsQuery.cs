using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Wallace.Application.Common.Interfaces;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Application.Queries.Accounts.GetAccounts
{
    public class GetAccountsQuery : IRequest<IEnumerable<AccountDto>> { }

    public class GetAccountsQueryHandler 
        : IRequestHandler<GetAccountsQuery, IEnumerable<AccountDto>>
    {
        private readonly IDbContext _dbContext;
        private readonly IIdentityAccessor _identityAccessor;
        private readonly IMapper _mapper;

        public GetAccountsQueryHandler(
            IDbContext dbContext,
            IIdentityAccessor identityAccessor,
            IMapper mapper
        )
        {
            _dbContext = dbContext;
            _identityAccessor = identityAccessor;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<AccountDto>> Handle(
            GetAccountsQuery _,
            CancellationToken cancellationToken
        )
        {
            var userId = _identityAccessor.Get().Id;

            return _dbContext.Accounts
                .Where(a => a.OwnerId == userId)
                .ProjectTo<AccountDto>(_mapper.ConfigurationProvider);
        }
    }
}