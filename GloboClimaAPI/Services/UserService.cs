using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using GloboClimaAPI.Interfaces;
using GloboClimaAPI.Models;
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
        public async Task<string> LoginAsync(string login, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(login))
                    throw new Exception("Não foi digitado o login.");

                if (string.IsNullOrEmpty(password))
                    throw new Exception("Não foi digitado a senha.");

                var passwordHash = GeneratePasswordHash(password);

                var conditions = new List<ScanCondition>
                {
                    new ScanCondition("Login", ScanOperator.Equal, login),
                    new ScanCondition("Password", ScanOperator.Equal, passwordHash)
                };

                // Executa a operação de scan com os filtros aplicados
                var search = _dbContext.ScanAsync<User>(conditions);
                var users = await search.GetNextSetAsync();

                if (users.Count == 0)
                {
                    throw new Exception("Usuário não encontrado.");
                }

                return GenerateJwtToken(login, passwordHash);
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
        /// Cadastro do usuário.
        /// </summary>
        public async Task<string> SubscribeAsync(string login, string password)
        {
            if (string.IsNullOrEmpty(login))
                throw new Exception("Não foi digitado o login.");

            if (string.IsNullOrEmpty(password))
                throw new Exception("Não foi digitado a senha.");

            var passwordHash = GeneratePasswordHash(password);

            var existingUserCondition = new List<ScanCondition>
            {
                new ScanCondition("Login", ScanOperator.Equal, login)
            };

            var search = _dbContext.ScanAsync<User>(existingUserCondition);
            var existingUsers = await search.GetNextSetAsync();

            if (existingUsers.Count > 0)
            {
                throw new Exception("Login já existe no sistema. Escolha um diferente.");
            }

            // Criação do usuário
            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Login = login,
                Password = passwordHash
            };

            await _dbContext.SaveAsync(newUser);

            return GenerateJwtToken(login, passwordHash);
        }

        /// <summary>
        /// Adiciona hash na senha.
        /// </summary>
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

        private string GenerateJwtToken(string login, string password)
        {
            // Realizar tratamento para gerar o JWT. No momento, só retorna string aleatória.
            return _configuration["Token:PasswordHash"];
        }
    }
}
