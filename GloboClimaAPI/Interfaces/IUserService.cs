using GloboClimaAPI.Models;

namespace GloboClimaAPI.Interfaces
{
    public interface IUserService
    {
        Task<User?> LoginAsync(string login, string password);
        Task<User?> SubscribeAsync(string login, string password);
    }
}
