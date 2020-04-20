using System;
using NodaMoney;
using Wallace.Domain.Enums;

namespace Wallace.Domain.Entities
{
    /// <summary>
    /// Represents a transaction of a certain account.
    /// </summary>
    public class Transaction : IOwnedEntity
    {
        public Guid Id { get; set; }
        public TransactionType Type { get; set; }
        public Repetition Repetition { get; set; }
        public Money Amount { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }

        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
        
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        
        public Guid? CategoryId { get; set; }
        public Category Category { get; set; }
        
        public Guid? PayeeId { get; set; }
        public Payee Payee { get; set; }
    }
}