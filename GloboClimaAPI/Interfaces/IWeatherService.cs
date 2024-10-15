using GloboClimaAPI.Models;

namespace GloboClimaAPI.Interfaces
{
    public interface IWeatherService
    {
        Task<Weather> GetWeatherByCityName(string name);
    }
}
