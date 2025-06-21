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
        private readonly string _baseAddress;

        public AuthService()
        {
            _httpClient = new HttpClient();
            // Adres API. Używamy 10.0.2.2 dla emulatora Androida, aby uzyskać dostęp do localhost hosta.
            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5136" : "http://localhost:5136";
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            var requestUrl = $"{_baseAddress}/api/auth/register";
            var registerModel = new { Username = username, Password = password };
            var json = JsonSerializer.Serialize(registerModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUrl, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var requestUrl = $"{_baseAddress}/api/auth/login";
            var loginModel = new { Username = username, Password = password };
            var json = JsonSerializer.Serialize(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUrl, content);

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