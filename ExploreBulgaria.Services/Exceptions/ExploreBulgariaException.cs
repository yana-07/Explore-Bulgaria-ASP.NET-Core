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
    }
}
