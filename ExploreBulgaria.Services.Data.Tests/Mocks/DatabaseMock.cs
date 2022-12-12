using ExploreBulgaria.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace ExploreBulgaria.Services.Data.Tests.Mocks
{
    public class DatabaseMock : IDisposable
    {
        private readonly DbConnection connection;
        private readonly DbContextOptions<ApplicationDbContext> dbContextOptions;

        public DatabaseMock()
        {
            connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
        }

        public ApplicationDbContext CreateContext() 
            => new ApplicationDbContext(dbContextOptions);

        public void Dispose() => connection.Dispose();
    }
}
