namespace ExploreBulgaria.Services.Guards
{
    public interface IGuard
    {
        void AgainstNull<T>(T value, string? errorMessage = null);
    }
}
