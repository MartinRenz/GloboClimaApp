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
        public async Task<Country?> GetCountryByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Não foi digitado nenhum código.");

                var response = await _httpClient.GetAsync($"{baseUrl}name/{name}");
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var countries = JsonSerializer.Deserialize<List<Country>>(responseContent);

                if (countries == null || !countries.Any())
                {
                    throw new Exception("Não foi encontrado nenhum país.");
                }

                var country = countries.FirstOrDefault();

                return country;
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

        /// <summary>
        /// Busca países através do código de forma assíncrona.
        /// </summary>
        public async Task<Country?> GetCountryByCodeAsync(string code)
        {
            try
            {
                if(string.IsNullOrEmpty(code))
                    throw new Exception("Não foi digitado nenhum código.");

                var response = await _httpClient.GetAsync($"{baseUrl}alpha/{code}");
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var countries = JsonSerializer.Deserialize<List<Country>>(responseContent);

                if (countries == null || !countries.Any())
                {
                    throw new Exception("Não foi encontrado nenhum país.");
                }

                var country = countries.FirstOrDefault();

                return country;
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
