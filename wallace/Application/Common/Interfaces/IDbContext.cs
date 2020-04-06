using System;
using Microsoft.EntityFrameworkCore;
using Wallace.Domain.Entities;

namespace Wallace.Application.Common.Interfaces
{
    /// <summary>
    /// Interface to abstract the use of the database in the application.
    /// </summary>
    public interface IDbContext
    {
        DbSet<User> Users { get; set; }
    }
}
