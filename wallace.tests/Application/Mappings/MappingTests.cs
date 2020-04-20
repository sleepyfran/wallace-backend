using System;
using AutoMapper;
using NUnit.Framework;
using Wallace.Application.Commands.Transactions.CreateTransaction;
using Wallace.Application.Common.Mappings;
using Wallace.Application.Common.Dto;
using Wallace.Domain.Entities;

namespace Wallace.Tests.Application.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }
        
        [Test]
        [TestCase(typeof(Account), typeof(AccountDto))]
        [TestCase(typeof(AccountDto), typeof(Account))]
        [TestCase(typeof(Category), typeof(CategoryDto))]
        [TestCase(typeof(CategoryDto), typeof(Category))]
        [TestCase(typeof(Transaction), typeof(TransactionDto))]
        [TestCase(typeof(TransactionDto), typeof(Transaction))]
        [TestCase(typeof(Transaction), typeof(CreateTransactionCommand))]
        [TestCase(typeof(CreateTransactionCommand), typeof(Transaction))]
        public void ShouldSupportMappingFrom(Type from, Type to)
        {
            var instance = Activator.CreateInstance(from);
            _mapper.Map(instance, from, to);
        }
    }
}