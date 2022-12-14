using ExploreBulgaria.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ExploreBulgaria.Services.Data.Tests.Mocks
{
    public class DatabaseMock : IDisposable
    {
        private readonly DbConnection connection;

        public DatabaseMock()
        {
            connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
        }

        public ApplicationDbContext CreateContext(params IInterceptor[] interceptors)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection);

            if (interceptors.Any())
            {
                optionsBuilder.AddInterceptors(interceptors);
            }

            return new ApplicationDbContext(optionsBuilder.Options);
        }
            
        public void Dispose() => connection.Dispose();
    }
}
