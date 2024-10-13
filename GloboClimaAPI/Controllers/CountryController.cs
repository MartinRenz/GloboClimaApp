using GloboClimaAPI.Interfaces;
using GloboClimaAPI.Models;
using GloboClimaAPI.Services;
using Microsoft.AspNetCore.Http;
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
            var country = await _countryService.GetCountryByCodeAsync(code);

            if (country == null)
            {
                _logger.LogWarning($"País não encontrado. Código: {code}");
                return NotFound();
            }

            _logger.LogInformation($"Requisição efetuada com sucesso. Código: {code}");

            return Ok(country);
        }
    }
}
