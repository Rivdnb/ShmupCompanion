using ShmupCompanion.Contracts.Games;
using System.Net;

namespace ShmupCompanion.Web.Services
{
    public class GameApiService
    {
        private readonly HttpClient _httpClient;

        public GameApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ShmupApi");
        }

        public async Task<List<GameDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<GameDto>>("games")
                   ?? [];
        }
        public async Task<GameDto?> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"games/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<GameDto>();
        }
    }
}
