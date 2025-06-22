using BiznesApp.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BiznesApp.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            var registerModel = new { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register", registerModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var loginModel = new { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                if (tokenResponse.TryGetProperty("token", out var token))
                {
                    return token.GetString();
                }
            }

            return null;
        }
    }
} 