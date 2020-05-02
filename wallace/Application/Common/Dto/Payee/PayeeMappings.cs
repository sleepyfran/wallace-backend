using System;
using AutoMapper;
using Wallace.Application.Common.Mappings;
using Wallace.Domain.Entities;

namespace Wallace.Application.Common.Dto
{
    public class PayeeMappings : IMapping<PayeeDto, Payee>
    {
        public void Mapping(Profile profile)
        {
            profile
                .CreateMap<PayeeDto, Payee>()
                .ForMember(c => c.Owner, opts => opts.Ignore())
                .ForMember(c => c.Transactions, opts => opts.Ignore())
                .ForMember(
                    a => a.OwnerId,
                    opts =>
                    {
                        opts.Condition(cd => cd.Owner != Guid.Empty);
                        opts.MapFrom(cd => cd.Owner);
                    }
                );
            
            profile
                .CreateMap<Payee, PayeeDto>()
                .ForMember(
                    ad => ad.Owner,
                    opt => opt.MapFrom(a => a.OwnerId)
                );;
        }
    }
}