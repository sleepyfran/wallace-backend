using System;
using AutoMapper;
using NodaMoney;
using Wallace.Application.Common.Mappings;
using Wallace.Domain.Entities;

namespace Wallace.Application.Common.Dto
{
    public class AccountMappings : IMapping<AccountDto, Account>
    {
        public void Mapping(Profile profile)
        {
            profile
                .CreateMap<AccountDto, Account>()
                .ForMember(
                    a => a.Balance,
                    opt => opt.MapFrom(ad => new Money(ad.Balance, ad.Currency))
                )
                .ForMember(a => a.Owner, opts => opts.Ignore())
                .ForMember(a => a.Transactions, opts => opts.Ignore())
                .ForMember(
                    a => a.OwnerId,
                    opts =>
                    {
                        opts.Condition(ad => ad.Owner != Guid.Empty);
                        opts.MapFrom(ad => ad.Owner);
                    }
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