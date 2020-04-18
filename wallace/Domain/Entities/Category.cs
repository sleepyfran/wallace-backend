using System;
using System.Collections.Generic;

namespace Wallace.Domain.Entities
{
    /// <summary>
    /// Represents a category to which a transaction belong.
    /// </summary>
    public class Category : IOwnedEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Emoji { get; set; }
        
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
        
        public List<Transaction> Transactions { get; set; }
            = new List<Transaction>();
    }
}