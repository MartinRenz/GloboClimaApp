using GloboClimaAPI.Models;
using GloboClimaAPI.Interfaces;
using System.Text.Json;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using GloboClimaAPI.Controllers;

namespace GloboClimaAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IDynamoDBContext _dbContext;

        public UserService(
            IDynamoDBContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Login do usuário.
        /// </summary>
        public async Task<User?> LoginAsync(string login, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(login))
                    throw new Exception("Não foi digitado o login.");

                if (string.IsNullOrEmpty(password))
                    throw new Exception("Não foi digitado a senha.");

                var conditions = new List<ScanCondition>
                {
                    new ScanCondition("Login", ScanOperator.Equal, login),
                    new ScanCondition("Password", ScanOperator.Equal, password)
                };

                // Executa a operação de scan com os filtros aplicados
                var search = _dbContext.ScanAsync<User>(conditions);
                var users = await search.GetNextSetAsync();

                if (users.Count == 0)
                {
                    throw new Exception("Usuário não encontrado.");
                }

                // Gerar JWT. E retornar

                return users.FirstOrDefault();
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
        public async Task<User?> SubscribeAsync(string login, string password)
        {
            return new User();
        }
    }
}
