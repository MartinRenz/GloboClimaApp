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

                if (users.Count == 0)
                {
                    throw new Exception("Usuário não encontrado.");
                }

                return GenerateJwtToken(login, passwordHash);
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

                return GenerateJwtToken(login, passwordHash);
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

        private string GenerateJwtToken(string login, string password)
        {
            // Realizar tratamento para gerar o JWT. No momento, só retorna string aleatória.
            return _configuration["Token:PasswordHash"];
        }
    }
}
