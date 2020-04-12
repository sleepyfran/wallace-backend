using System;
using System.Collections.Generic;

namespace Wallace.Domain.Entities
{
    /// <summary>
    /// Represents a category to which a transaction belong.
    /// </summary>
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        
        public List<Transaction> Transactions { get; set; }
            = new List<Transaction>();
    }
}