using Microsoft.Extensions.Options;
using NetCoreTDD.API.Config;
using NetCoreTDD.API.Models;

namespace NetCoreTDD.API.Services
{
    public interface IUsersService
    {
        public Task<List<User>> GetAllUsers();
    }
    public class UserService : IUsersService
    {
        private readonly HttpClient _httpClient;
        private readonly UsersApiOption _apiConfig;

        public UserService(HttpClient httpClient, IOptions<UsersApiOption> apiConfig)
        {
            _httpClient = httpClient;
            this._apiConfig = apiConfig.Value;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var usersResponse = await _httpClient
                .GetAsync(_apiConfig.EndPoint);

            if(usersResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<User>();
            }
            
            var responseContent = usersResponse.Content;
            var allUsers = await responseContent.ReadFromJsonAsync<List<User>>();
            return allUsers.ToList();
        }
    }
}
