using Jeopardy_Backend;
using Microsoft.EntityFrameworkCore;
using System;

namespace Jeopardy_Backend_TestCommon.DataSetup
{
    public class DatabaseFixture : IDisposable
    {
        public JeopardyContext context;
        public DbContextOptions<JeopardyContext> options;

        public DatabaseFixture()
        {
            this.options = new DbContextOptionsBuilder<JeopardyContext>()
                .UseInMemoryDatabase(databaseName: "JeopardyTests")
                .Options;

            this.context = new JeopardyContext(options);
            this.context.Database.EnsureCreated();

            var testDataManager = new TestDataManager(this.context);
            testDataManager.AddTestData();
        }

        public void Dispose()
        {
            this.context.Database.EnsureDeleted();
        }
    }
}
