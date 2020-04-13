using System;
using AutoMapper;
using NodaMoney;
using Wallace.Application.Common.Mappings;
using Wallace.Domain.Entities;

namespace Wallace.Application.Queries.Accounts
{
    public class AccountDto : IMapping<Account>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public Guid Owner { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile
                .CreateMap<AccountDto, Account>()
                .ForMember(
                    a => a.Balance,
                    opt => opt.MapFrom(ad => new Money(ad.Balance, ad.Currency))
                )
                .ForMember(
                    a => a.OwnerId,
                    opt => opt.MapFrom(ad => ad.Owner)
                )
                .ForMember(
                    a => a.Owner,
                    opt => opt.Ignore()
                )
                .ForMember(
                    a => a.Transactions,
                    opt => opt.Ignore()
                );

            profile
                .CreateMap<Account, AccountDto>()
                .ForMember(
                    ad => ad.Balance,
                    opt => opt.MapFrom(a => a.Balance.Amount)
                )
                .ForMember(
                    ad => ad.Currency,
                    opt => opt.MapFrom(a => a.Balance.Currency.Code)
                )
                .ForMember(
                    ad => ad.Owner,
                    opt => opt.MapFrom(a => a.OwnerId)
                );
        }
    }
}