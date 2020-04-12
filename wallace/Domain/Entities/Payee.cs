using System;
using System.Collections.Generic;

namespace Wallace.Domain.Entities
{
    /// <summary>
    /// Represents the person who pays or receieves money in a transaction.
    /// </summary>
    public class Payee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public List<Transaction> Transactions { get; set; }
            = new List<Transaction>();
    }
}