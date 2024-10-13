using GloboClimaAPI.Interfaces;
using GloboClimaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;
using System.Threading.Tasks;

namespace GloboClimaAPI.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        private const string baseUrl = "https://api.openweathermap.org/data/2.5/";
        private const string token = "";

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Busca clima através do nome de cidade.
        /// </summary>
        public async Task<Weather?> GetWeatherByCityName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Não foi digitado nenhuma cidade.");

                var response = await _httpClient.GetAsync($"{baseUrl}weather?q={name}&units=metric&appid={token}");
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var weather = JsonSerializer.Deserialize<Weather>(responseContent);

                if (weather == null)
                {
                    throw new Exception("Não foi encontrado nenhuma cidade e seu clima.");
                }

                return weather;
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception($"Erro na comunicação com a API pública. {httpEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                throw new Exception($"Erro ao tratar o retorno da API pública. {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro inesperado. {ex.Message}");
            }
        }
    }
}
