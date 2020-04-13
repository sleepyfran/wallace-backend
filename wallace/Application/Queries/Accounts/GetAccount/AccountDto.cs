using System;
using NodaMoney;

namespace Wallace.Application.Queries.Accounts.GetAccount
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public Guid Owner { get; set; }
    }
}