namespace ExploreBulgaria.Services.Exceptions
{
    public class InvalidImageExtensionException : Exception
    {
        public InvalidImageExtensionException()
        {
        }

        public InvalidImageExtensionException(string message)
            : base(message)
        {
        }
    }
}
