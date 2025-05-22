using System.Text.Json;
using System.Text;

namespace VolunteerApi.Models
{
    public class OpenAIHelperService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAIHelperService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"]; // Asegúrate que esté en appsettings.json
        }

        public async Task<string> ObtenerRecomendacionAsync(string prompt)
        {
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                new { role = "system", content = "Eres un asistente que recomienda voluntarios ideales para eventos sociales." },
                new { role = "user", content = prompt }
            },
                temperature = 0.7
            };

            var requestJson = JsonSerializer.Serialize(requestBody);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");
            request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error desde OpenAI: {error}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseContent);
            var result = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return result?.Trim() ?? "No se pudo obtener una recomendación.";
        }
    }

}
