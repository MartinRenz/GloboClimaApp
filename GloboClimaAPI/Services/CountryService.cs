using GloboClimaAPI.Interfaces;
using GloboClimaAPI.Models;
using System.Text.Json;

namespace GloboClimaAPI.Services
{
    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;

        private const string baseUrl = "https://restcountries.com/v3.1/";

        public CountryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Busca países através do nome de forma assíncrona.
        /// </summary>
        /// <param name="name">O nome do país a ser buscado.</param>
        /// <returns>Retorna o país encontrado ou exceção.</returns>
        public async Task<Country> GetCountryByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException("Parâmetro Nome não contém valor.");

                var response = await _httpClient.GetAsync($"{baseUrl}name/{name}");

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var countries = JsonSerializer.Deserialize<List<Country>>(responseContent);

                var country = countries?.FirstOrDefault() ?? throw new Exception("Não foi encontrado nenhum país com o nome informado.");

                return country;
            }
            catch (HttpRequestException)
            {
                throw new Exception("Erro na comunicação com a API externa.");
            }
            catch (JsonException)
            {
                throw new Exception("Erro ao processar os dados retornados pela API externa.");
            }
            catch (Exception)
            {
                throw new Exception("Erro inesperado ao buscar país.");
            }
        }

        /// <summary>
        /// Busca países através do código de forma assíncrona.
        /// </summary>
        /// <param name="code">O código do país a ser buscado.</param>
        /// <returns>Retorna o país encontrado ou exceção.</returns>
        public async Task<Country> GetCountryByCode(string code)
        {
            try
            {
                if(string.IsNullOrEmpty(code))
                    throw new Exception("Não foi digitado nenhum código.");

                var response = await _httpClient.GetAsync($"{baseUrl}alpha/{code}");

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var countries = JsonSerializer.Deserialize<List<Country>>(responseContent);

                var country = countries?.FirstOrDefault() ?? throw new Exception("Não foi encontrado nenhum país com o código informado.");

                return country;
            }
            catch (HttpRequestException)
            {
                throw new Exception($"Erro na comunicação com a API pública.");
            }
            catch (JsonException)
            {
                throw new Exception($"Erro ao tratar o retorno da API pública.");
            }
            catch (Exception)
            {
                throw new Exception($"Erro inesperado.");
            }
        }
    }
}
