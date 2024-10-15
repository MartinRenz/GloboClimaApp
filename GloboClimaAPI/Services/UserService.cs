using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using GloboClimaAPI.Interfaces;
using GloboClimaAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace GloboClimaAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBContext _dbContext;

        public UserService(
            IConfiguration configuration,
            IDynamoDBContext dbContext
        )
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Login do usuário.
        /// </summary>
        /// <param name="login">Login do usuário.</param>
        /// <param name="password">Senha do usuário.</param>
        /// <returns>Retorna um Bearer Token ou exceção.</returns>
        public async Task<string> LoginAsync(
            string login, 
            string password
        )
        {
            try
            {
                if (string.IsNullOrEmpty(login))
                    throw new ArgumentException("Parâmetro Login não contém valor.");

                if (string.IsNullOrEmpty(password))
                    throw new ArgumentException("Parâmetro Password não contém valor.");

                var passwordHash = GeneratePasswordHash(password);

                var conditions = new List<ScanCondition>
                {
                    new ScanCondition("Login", ScanOperator.Equal, login),
                    new ScanCondition("Password", ScanOperator.Equal, passwordHash)
                };

                // Executa a operação de scan com os filtros aplicados
                var search = _dbContext.ScanAsync<UserDbModel>(conditions);
                var users = await search.GetNextSetAsync();

                if (users.Count == 0 || !users.Any())
                {
                    throw new Exception("Usuário não encontrado.");
                }

                var user = users.FirstOrDefault();

                if (string.IsNullOrEmpty(user?.Id) || string.IsNullOrEmpty(user?.Login))
                {
                    throw new Exception("Usuário não encontrado.");
                }

                return GenerateJwtToken(user.Id, user.Login);
            }
            catch (HttpRequestException)
            {
                throw new Exception("Erro na comunicação com a base de dados.");
            }
            catch (JsonException)
            {
                throw new Exception("Erro ao tratar o retorno da base de dados.");
            }
            catch (Exception)
            {
                throw new Exception("Erro inesperado.");
            }
        }

        /// <summary>
        /// Cadastro do usuário.
        /// </summary>
        /// <param name="login">Login do usuário.</param>
        /// <param name="password">Senha do usuário.</param>
        /// <returns>Retorna um Bearer Token ou exceção.</returns>
        public async Task<string> SubscribeAsync(
            string login, 
            string password
        )
        {
            try 
            {
                if (string.IsNullOrEmpty(login))
                    throw new ArgumentException("Parâmetro Login não contém valor.");

                if (string.IsNullOrEmpty(password))
                    throw new ArgumentException("Parâmetro Password não contém valor.");

                var passwordHash = GeneratePasswordHash(password);

                // Validação se usuário já existe.
                var existingUserCondition = new List<ScanCondition>
                {
                    new ScanCondition("Login", ScanOperator.Equal, login)
                };

                var search = _dbContext.ScanAsync<UserDbModel>(existingUserCondition);
                var existingUsers = await search.GetNextSetAsync();

                if (existingUsers.Count > 0)
                {
                    throw new Exception("Login já existe no sistema. Escolha um diferente.");
                }

                // Criação do usuário.
                var newUser = new UserDbModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Login = login,
                    Password = passwordHash
                };

                await _dbContext.SaveAsync(newUser);

                return GenerateJwtToken(newUser.Id, newUser.Login);
            }
            catch (HttpRequestException)
            {
                throw new Exception("Erro na comunicação com a base de dados.");
            }
            catch (JsonException)
            {
                throw new Exception("Erro ao tratar o retorno da base de dados.");
            }
            catch (Exception)
            {
                throw new Exception("Erro inesperado.");
            }
        }

        /// <summary>
        /// Adiciona hash na senha do usuário utilizando SHA256.
        /// </summary>
        /// <param name="password">Senha do usuário.</param>
        /// <returns>Retorna a senha como hash.</returns>
        private string GeneratePasswordHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string GenerateJwtToken(
            string id, 
            string username
        )
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, id)
        };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return tokenHandler.WriteToken(token);
        }
    }
}
