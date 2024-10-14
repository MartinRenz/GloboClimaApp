using GloboClimaAPI.Interfaces;
using GloboClimaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GloboClimaAPI.Controllers
{
    /// <summary>
    /// Controlador para gerenciar operações relacionadas a países.
    /// </summary>
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(
            ILogger<UserController> logger,
            IUserService userService
        )
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Login do usuário.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserRequest req)
        {
            try 
            {
                if (string.IsNullOrEmpty(req.Login) || string.IsNullOrEmpty(req.Password))
                    return StatusCode(400, "Erro no envio de login e senha. Tente novemente.");

                var bearerToken = await _userService.LoginAsync(req.Login, req.Password);

                if (bearerToken == null)
                    return NotFound();

                _logger.LogInformation($"Usuário {req.Login} criado com sucesso.");
                return Ok(bearerToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Cadastro do usuário.
        /// </summary>
        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] UserRequest req)
        {
            try
            {
                if (string.IsNullOrEmpty(req.Login) || string.IsNullOrEmpty(req.Password))
                    return StatusCode(400, "Erro no envio de login e senha. Tente novemente.");

                var bearerToken = await _userService.SubscribeAsync(req.Login, req.Password);

                if (bearerToken == null)
                    return NotFound();

                _logger.LogInformation($"Usuário {req.Login} criado com sucesso.");
                return Ok(bearerToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
