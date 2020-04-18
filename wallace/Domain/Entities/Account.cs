using System;
using System.Collections.Generic;
using NodaMoney;

namespace Wallace.Domain.Entities
{
    /// <summary>
    /// Represents a bank account inside the app.
    /// </summary>
    public class Account : IOwnedEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Money Balance { get; set; }
        
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
        
        public List<Transaction> Transactions { get; set; }
            = new List<Transaction>();
    }
}
