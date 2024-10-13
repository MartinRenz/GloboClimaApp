using GloboClimaAPI.Models;

namespace GloboClimaAPI.Interfaces
{
    public interface ICountryService
    {
        Task<Country?> GetCountryByNameAsync(string name);
        Task<Country?> GetCountryByCodeAsync(string code);
    }
}
