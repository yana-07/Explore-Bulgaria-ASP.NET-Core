namespace ExploreBulgaria.Services.Exceptions
{
    public class ExploreBulgariaDbException : Exception
    {
        public ExploreBulgariaDbException()
        {
        }

        public ExploreBulgariaDbException(string message)
            :base(message)
        {
        }

        public ExploreBulgariaDbException(
            string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
