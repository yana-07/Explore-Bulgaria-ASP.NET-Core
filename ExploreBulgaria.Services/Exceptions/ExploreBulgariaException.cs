namespace ExploreBulgaria.Services.Exceptions
{
    public class ExploreBulgariaException : ApplicationException
    {
        public ExploreBulgariaException()
        {
        }

        public ExploreBulgariaException(string message)
            : base(message)
        {
        }

        public ExploreBulgariaException(
            string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
