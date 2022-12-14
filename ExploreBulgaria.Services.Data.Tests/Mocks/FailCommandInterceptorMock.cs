using ExploreBulgaria.Services.Exceptions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace ExploreBulgaria.Services.Data.Tests.Mocks
{
    public class FailCommandInterceptorMock : DbCommandInterceptor
    {
        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            if (command.CommandText.StartsWith("INSERT"))
            {
                throw new ExploreBulgariaDbException();
            }

            return base.ReaderExecutingAsync(command, eventData,
                result, cancellationToken);
        }
    }
}
