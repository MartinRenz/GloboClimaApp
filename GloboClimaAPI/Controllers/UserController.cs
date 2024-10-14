using Amazon.DynamoDBv2.DataModel;
using GloboClimaAPI.Interfaces;
using GloboClimaAPI.Models;
using GloboClimaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

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
        public async Task<IActionResult> GetUserById([FromBody] UserRequest req)
        {
            try 
            {
                if (string.IsNullOrEmpty(req.Login) || string.IsNullOrEmpty(req.Password))
                    return StatusCode(400, "Erro no envio de login e senha. Tente novemente.");

                var user = await _userService.LoginAsync(req.Login, req.Password);

                if (user == null)
                    return NotFound();

                _logger.LogInformation($"Usuário {req.Login} criado com sucesso.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
