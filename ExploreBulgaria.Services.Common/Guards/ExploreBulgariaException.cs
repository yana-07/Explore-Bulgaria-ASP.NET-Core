namespace ExploreBulgaria.Services.Common.Guards
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
