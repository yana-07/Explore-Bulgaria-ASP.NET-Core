namespace ExploreBulgaria.Services.Exceptions
{
    public class DbException : ApplicationException
    {
        public DbException()
        {
        }

        public DbException(string message)
            :base(message)
        {
        }
    }
}
