using GloboClimaAPI.Interfaces;
using GloboClimaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GloboClimaAPI.Controllers
{
    /// <summary>
    /// Controlador para gerenciar operações relacionadas a clima de cidades.
    /// </summary>
    [Route("api/weather")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherController(
            ILogger<WeatherController> logger,
            IWeatherService weatherService
        )
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        /// <summary>
        /// Busca clima através do nome da cidade.
        /// </summary>
        /// <param name="name">O nome da cidade a ser buscado.</param>
        /// <returns>Retorna o clima da cidade procurada ou um código de status apropriado.</returns>
        [AllowAnonymous]
        [HttpGet("/city/{name}")]
        public async Task<IActionResult> GetWeatherByCityName(string name)
        {
            try 
            {
                var weather = await _weatherService.GetWeatherByCityName(name);

                if (weather == null)
                {
                    _logger.LogWarning($"Clima da cidade não encontrado. Cidade: {name}");
                    return NotFound("Cidade não encontrada.");
                }

                _logger.LogInformation($"Requisição efetuada com sucesso. Cidade: {name}");
                return Ok(weather);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}. Cidade: {name}");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Busca todos os climas favoritos.
        /// </summary>
        [Authorize]
        [HttpGet("/city/favorite")]
        public async Task<IActionResult> GetAllFavoriteWeather()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Busca um clima favorito.
        /// </summary>
        [Authorize]
        [HttpGet("/city/favorite/{id}")]
        public async Task<IActionResult> GetFavoriteWeather(int id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Salva um novo clima favorito.
        /// </summary>
        [Authorize]
        [HttpPost("/city/favorite")]
        public async Task<IActionResult> SaveFavoriteWeather([FromBody] Weather body)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Deleta um clima favorito.
        /// </summary>
        [Authorize]
        [HttpDelete("/city/favorite/{id}")]
        public async Task<IActionResult> DeleteFavoriteWeather(int id)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
