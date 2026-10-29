using Microsoft.AspNetCore.Mvc;
using ShmupCompanion.Contracts;
using System.Net;

namespace ShmupCompanion.Web.Services
{
    public class NoteApiService
    {
        private readonly HttpClient _httpClient;

        public NoteApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ShmupApi");
        }

        public async Task<List<NoteDto>> GetAllForRunAsync(int gameId, int runId)
        {
            return await _httpClient.GetFromJsonAsync<List<NoteDto>>($"games/{gameId}/runs/{runId}/notes")
                   ?? [];
        }
        public async Task<NoteDto?> GetAsync(int gameId, int runId, int id)
        {
            var response = await _httpClient.GetAsync($"games/{gameId}/runs/{runId}/notes/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<NoteDto>();
        }

        public async Task<NoteDto?> PostAsync(NoteRequest request, int gameId, int runId)
        {
            var response = await _httpClient.PostAsJsonAsync($"games/{gameId}/runs/{runId}/notes", request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<NoteDto>();
        }
        public async Task UpdateAsync(NoteRequest request, int gameId, int runId, int id)
        {
            var response = await _httpClient.PutAsJsonAsync($"games/{gameId}/runs/{runId}/notes/{id}", request);

            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteAsync(int gameId, int runId, int id)
        {
            var response = await _httpClient.DeleteAsync($"games/{gameId}/runs/{runId}/notes/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}
