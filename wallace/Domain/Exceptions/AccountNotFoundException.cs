using System;

namespace Wallace.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when an account could not be found in the database.
    /// </summary>
    public class AccountNotFoundException : Exception { }
}