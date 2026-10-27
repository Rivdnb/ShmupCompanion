using Microsoft.AspNetCore.Mvc;
using ShmupCompanion.Contracts;
using System.Net;

namespace ShmupCompanion.Web.Services
{
    public class RunApiService
    {
        private readonly HttpClient _httpClient;

        public RunApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ShmupApi");
        }

        public async Task<List<RunDto>> GetAllForGameAsync(int gameId)
        {
            return await _httpClient.GetFromJsonAsync<List<RunDto>>($"games/{gameId}/runs")
                   ?? [];
        }
        public async Task<RunDto?> GetAsync(int gameId, int id)
        {
            var response = await _httpClient.GetAsync($"games/{gameId}/runs/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<RunDto>();
        }

        public async Task<RunDto?> PostAsync(RunRequest request, int gameId)
        {
            var response = await _httpClient.PostAsJsonAsync($"games/{gameId}/runs", request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<RunDto>();
        }
        public async Task UpdateAsync(RunRequest request, int gameId, int id)
        {
            var response = await _httpClient.PutAsJsonAsync($"games/{gameId}/runs/{id}", request);

            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteAsync(int gameId, int id)
        {
            var response = await _httpClient.DeleteAsync($"games/{gameId}/runs/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}
