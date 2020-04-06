using System;
using NodaMoney;
using Wallace.Domain.Enums;

namespace Wallace.Domain.Entities
{
    /// <summary>
    /// Represents a transaction of a certain account.
    /// </summary>
    public class Transaction
    {
        public int TransactionId { get; set; }
        public TransactionType Type { get; set; }
        public Repetition Repetition { get; set; }
        public Money Amount { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
        public int PayeeId { get; set; }
        public Payee Payee { get; set; }
    }
}