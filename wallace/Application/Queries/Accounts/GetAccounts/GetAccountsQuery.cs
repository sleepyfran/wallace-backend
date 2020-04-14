using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallace.Application.Common.Interfaces;
using Wallace.Application.Common.Dto;
using Wallace.Domain.Identity.Interfaces;
using static Wallace.Domain.Queries.UserAccounts;

namespace Wallace.Application.Queries.Accounts
{
    public class GetAccountsQuery : IRequest<IEnumerable<AccountDto>> { }

    /// <summary>
    /// Retrieves all the accounts associated to the current logged in user.
    /// </summary>
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
            var loggedInUser = _identityAccessor.Get().Id;

            return await _dbContext.Accounts
                .QueryAccountsFor(loggedInUser)
                .ProjectTo<AccountDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}