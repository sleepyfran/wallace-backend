using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Wallace.Application.Common.Interfaces;
using Wallace.Persistence;

namespace Wallace.Tests.Application
{
    public class BaseTest
    {
        protected bool ReloadOnSetUp = true;
        protected IDbContext DbContext;
        
        private void CreateContext()
        {
            var options = new DbContextOptionsBuilder<WallaceDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            
            DbContext = new WallaceDbContext(options);
        }

        [SetUp]
        public void SetUp()
        {
            if (ReloadOnSetUp)
                CreateContext();
        }
    }
}