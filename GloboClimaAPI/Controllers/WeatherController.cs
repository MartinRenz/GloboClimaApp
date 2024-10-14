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
        /// Busca clima através do nome de cidade.
        /// </summary>
        [HttpGet("/city/{name}")]
        public async Task<IActionResult> GetWeatherByCityName(string name)
        {
            try 
            {
                var weather = await _weatherService.GetWeatherByCityName(name);

                if (weather == null)
                {
                    _logger.LogWarning($"Clima da cidade não encontrado. Cidade: {name}");
                    return NotFound();
                }

                _logger.LogInformation($"Requisição efetuada com sucesso. Cidade: {name}");
                return Ok(weather);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Busca todos os climas favoritos.
        /// </summary>
        [HttpGet("/city/favorite")]
        [Authorize]
        public async Task<IActionResult> GetAllFavoriteWeather()
        {
            try
            {
                return Ok();
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
        [HttpGet("/city/favorite/{id}")]
        [Authorize]
        public async Task<IActionResult> GetFavoriteWeather(int id)
        {
            try
            {
                return Ok();
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
        [HttpPost("/city/favorite")]
        [Authorize]
        public async Task<IActionResult> SaveFavoriteWeather([FromBody] Weather body)
        {
            try
            {
                return Ok();
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
        [HttpDelete("/city/favorite/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFavoriteWeather(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
