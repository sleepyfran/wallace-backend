using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wallace.Domain.Entities
{
    /// <summary>
    /// Representation of the user's credentials and information used across the app.
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}
