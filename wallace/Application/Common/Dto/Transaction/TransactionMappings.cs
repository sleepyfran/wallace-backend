using System;
using AutoMapper;
using NodaMoney;
using Wallace.Application.Commands.Transactions.CreateTransaction;
using Wallace.Application.Common.Mappings;
using Wallace.Domain.Entities;

namespace Wallace.Application.Common.Dto
{
    public class TransactionMappings : IMapping<TransactionDto, Transaction>
    {
        public void Mapping(Profile profile)
        {
            profile
                .CreateMap<TransactionDto, Transaction>()
                .Include<CreateTransactionCommand, Transaction>()
                .ForMember(
                    t => t.Amount,
                    opts => opts.MapFrom(
                        td => new Money(td.Amount, td.Currency)
                    )
                )
                .ForMember(
                    a => a.OwnerId,
                    opts => opts.MapFrom(ad => ad.Owner)
                )
                .ForMember(
                    t => t.AccountId,
                    opts => opts.MapFrom(td => td.Account)
                )
                .ForMember(
                    t => t.CategoryId,
                    opts => opts.MapFrom(td => td.Category.NullIfEmpty())
                )
                .ForMember(
                    t => t.PayeeId,
                    opts => opts.MapFrom(td => td.Payee.NullIfEmpty())
                )
                .ForMember(t => t.Owner, opts => opts.Ignore())
                .ForMember(t => t.Account, opts => opts.Ignore())
                .ForMember(t => t.Category, opts => opts.Ignore())
                .ForMember(t => t.Payee, opts => opts.Ignore())
                .ForAllMembers(o => o.Condition(
                    (src, dest, val) => val as Guid? != Guid.Empty)
                );

            profile
                .CreateMap<Transaction, TransactionDto>()
                .Include<Transaction, CreateTransactionCommand>()
                .ForMember(
                    td => td.Amount,
                    opts => opts.MapFrom(t => t.Amount.Amount)
                )
                .ForMember(
                    td => td.Currency,
                    opts => opts.MapFrom(t => t.Amount.Currency.Code)
                )
                .ForMember(
                    td => td.Owner,
                    opts => opts.MapFrom(t => t.OwnerId)
                )
                .ForMember(
                    td => td.Account,
                    opts => opts.MapFrom(t => t.AccountId)
                )
                .ForMember(
                    td => td.CategoryName,
                    opts => opts.MapFrom(t => t.Category.Name)
                )
                .ForMember(
                    td => td.CategoryEmoji,
                    opts => opts.MapFrom(t => t.Category.Emoji)
                )
                .ForMember(
                    td => td.Category,
                    opts => opts.MapFrom(t => t.Category.Id)
                )
                .ForMember(
                    td => td.PayeeName,
                    opts => opts.MapFrom(t => t.Payee.Name)
                )
                .ForMember(
                    td => td.Payee,
                    opts => opts.MapFrom(t => t.Payee.Id)
                );

            profile
                .CreateMap<CreateTransactionCommand, Transaction>()
                .ForMember(
                    t => t.Id,
                    opts => opts.Ignore()
                );
            profile
                .CreateMap<Transaction, CreateTransactionCommand>()
                .ForMember(
                    t => t.Id,
                    opts => opts.Ignore()
                );
        }
    }
}