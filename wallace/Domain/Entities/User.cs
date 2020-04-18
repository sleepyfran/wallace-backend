using System;
using System.Collections.Generic;

namespace Wallace.Domain.Entities
{
    /// <summary>
    /// Representation of the user's credentials and information used across the app.
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<Payee> Payees { get; set; } = new List<Payee>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
