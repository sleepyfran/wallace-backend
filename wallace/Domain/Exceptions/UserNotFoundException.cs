using System;

namespace Wallace.Domain.Exceptions
{
    /// <summary>
    /// Exception for when an user couldn't be found in the database.
    /// </summary>
    public class UserNotFoundException : Exception { }
}