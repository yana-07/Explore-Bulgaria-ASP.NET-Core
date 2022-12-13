namespace ExploreBulgaria.Services.Exceptions
{
    public class ExploreBulgariaException : ApplicationException
    {
        public ExploreBulgariaException()
        {
        }

        public ExploreBulgariaException(string errorMessage)
            : base(errorMessage)
        {
        }

        public ExploreBulgariaException(
            string errorMessage, Exception innerException)
            : base(errorMessage, innerException)
        {
        }
    }
}
