namespace ExploreBulgaria.Services.Guards
{
    public class Guard : IGuard
    {
        public void AgainstNull<T>(T value, string? errorMessage = null)
        {
            if (value == null)
            {
                var exception = errorMessage == null ?
                    new ExploreBulgariaException() :
                    new ExploreBulgariaException(errorMessage);

                throw exception;
            }
        }
    }
}
