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
        /// <param name="name">O nome do país a ser buscado.</param>
        /// <returns>Retorna o país encontrado ou um código de status apropriado.</returns>
        [AllowAnonymous]
        [HttpGet("/name/{name}")]
        public async Task<IActionResult> GetCountryByName(string name)
        {
            try 
            {
                var country = await _countryService.GetCountryByName(name);

                if (country == null)
                {
                    _logger.LogWarning($"País não encontrado. Nome: {name}");
                    return NotFound("País não encontrado.");
                }

                _logger.LogInformation($"Requisição efetuada com sucesso. Nome: {name}");
                return Ok(country);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}. Nome: {name}");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Busca países através do código.
        /// </summary>
        /// <param name="code">O código do país a ser buscado.</param>
        /// <returns>Retorna o país encontrado ou um código de status apropriado.</returns>
        [AllowAnonymous]
        [HttpGet("/code/{code}")]
        public async Task<IActionResult> GetCountryByCode(string code)
        {
            try {
                var country = await _countryService.GetCountryByCode(code);

                if (country == null)
                {
                    _logger.LogWarning($"País não encontrado. Código: {code}");
                    return NotFound("País não encontrado.");
                }

                _logger.LogInformation($"Requisição efetuada com sucesso. Código: {code}");

                return Ok(country);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}. Código: {code}");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Busca todos os países favoritos.
        /// </summary>
        /// <returns>Retorna todos os países favoritos do usuário.</returns>
        [Authorize]
        [HttpGet("/favorite")]
        public async Task<IActionResult> GetAllFavoriteCountries()
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
        /// Busca um país favorito.
        /// </summary>
        /// <returns>Retorna todos os países favoritos do usuário.</returns>
        [Authorize]
        [HttpGet("/favorite/{id}")]
        public async Task<IActionResult> GetFavoriteCountry(int id)
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
        /// Salva um novo país favorito.
        /// </summary>
        [Authorize]
        [HttpPost("/favorite")]
        public async Task<IActionResult> SaveFavoriteCountry([FromBody] Country body)
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
        /// Deleta um país favorito.
        /// </summary>
        [Authorize]
        [HttpDelete("/favorite/{id}")]
        public async Task<IActionResult> DeleteFavoriteCountry(int id)
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
