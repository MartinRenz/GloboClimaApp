using GloboClimaAPI.Models;

namespace GloboClimaAPI.Interfaces
{
    public interface ICountryService
    {
        Task<Country> GetCountryByName(string name);
        Task<Country> GetCountryByCode(string code);
    }
}
