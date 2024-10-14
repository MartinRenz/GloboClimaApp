namespace GloboClimaAPI.Interfaces
{
    public interface IUserService
    {
        Task<string> LoginAsync(string login, string password);
        Task<string> SubscribeAsync(string login, string password);
    }
}
