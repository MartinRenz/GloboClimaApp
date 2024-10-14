using GloboClimaAPI.Interfaces;
using GloboClimaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GloboClimaAPI.Controllers
{
    /// <summary>
    /// Controlador para gerenciar operações relacionadas a países.
    /// </summary>
    [Route("api/country")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> _logger;
        private readonly ICountryService _countryService;

        public CountryController(
            ILogger<CountryController> logger,
            ICountryService countryService
        )
        {
            _logger = logger;
            _countryService = countryService;
        }

        /// <summary>
        /// Busca países através do nome.
        /// </summary>
        [HttpGet("/name/{name}")]
        public async Task<IActionResult> GetCountryByName(string name)
        {
            try 
            {
                var country = await _countryService.GetCountryByNameAsync(name);

                if (country == null)
                {
                    _logger.LogWarning($"País não encontrado. Nome: {name}");
                    return NotFound();
                }

                _logger.LogInformation($"Requisição efetuada com sucesso. Nome: {name}");
                return Ok(country);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Busca países através do código.
        /// </summary>
        [HttpGet("/code/{code}")]
        public async Task<IActionResult> GetCountryByCode(string code)
        {
            try {
                var country = await _countryService.GetCountryByCodeAsync(code);

                if (country == null)
                {
                    _logger.LogWarning($"País não encontrado. Código: {code}");
                    return NotFound();
                }

                _logger.LogInformation($"Requisição efetuada com sucesso. Código: {code}");

                return Ok(country);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Busca todos os países favoritos.
        /// </summary>
        [HttpGet("/favorite")]
        [Authorize]
        public async Task<IActionResult> GetAllFavoriteCountries()
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
        /// Busca um país favorito.
        /// </summary>
        [HttpGet("/favorite/{id}")]
        [Authorize]
        public async Task<IActionResult> GetFavoriteCountry(int id)
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
        /// Salva um novo país favorito.
        /// </summary>
        [HttpPost("/favorite")]
        [Authorize]
        public async Task<IActionResult> SaveFavoriteCountry([FromBody] Country body)
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
        /// Deleta um país favorito.
        /// </summary>
        [HttpDelete("/favorite/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFavoriteCountry(int id)
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
