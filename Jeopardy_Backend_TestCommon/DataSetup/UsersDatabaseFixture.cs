using Jeopardy_Backend;
using Microsoft.EntityFrameworkCore;
using System;

namespace Jeopardy_Backend_UnitTests
{
    public class UserDatabaseFixture : IDisposable
    {
        public UserContext context;
        public DbContextOptions<UserContext> options;

        public UserDatabaseFixture()
        {
            this.options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: "UsersTests")
                .Options;

            this.context = new UserContext(options);
            this.context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            this.context.Database.EnsureDeleted();
        }
    }
}
