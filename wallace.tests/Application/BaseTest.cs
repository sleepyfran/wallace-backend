using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Wallace.Application.Common.Interfaces;
using Wallace.Application.Common.Mappings;
using Wallace.Persistence;

namespace Wallace.Tests.Application
{
    public class BaseTest
    {
        protected bool ReloadOnSetUp = true;
        protected IDbContext DbContext;
        protected IMapper Mapper;
        
        private void CreateContext()
        {
            var options = new DbContextOptionsBuilder<WallaceDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            
            DbContext = new WallaceDbContext(options);
            
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = configuration.CreateMapper();
        }

        [SetUp]
        public void SetUp()
        {
            if (ReloadOnSetUp)
                CreateContext();
        }
    }
}