using Bit.BlazorUI;
using Microsoft.AspNetCore.Mvc;
using ShmupCompanion.Contracts;
using System.Diagnostics;
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
            return await _httpClient.GetFromJsonAsync<List<NoteDto>>($"api/games/{gameId}/runs/{runId}/notes")
                   ?? [];
        }
        public async Task<NoteDto?> GetAsync(int gameId, int runId, int id)
        {
            var response = await _httpClient.GetAsync($"api/games/{gameId}/runs/{runId}/notes/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<NoteDto>();
        }

        public async Task<NoteDto?> PostAsync(NoteRequest request, int gameId, int runId)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/games/{gameId}/runs/{runId}/notes", request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<NoteDto>();
        }
        public async Task UpdateAsync(NoteRequest request, int gameId, int runId, int id)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/games/{gameId}/runs/{runId}/notes/{id}", request);
            //var responseContent = await response.Content.ReadAsStringAsync();
            //Debug.WriteLine($"Status: {response.StatusCode}");
            //Debug.WriteLine($"Response: {responseContent}");
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteAsync(int gameId, int runId, int id)
        {
            var response = await _httpClient.DeleteAsync($"api/games/{gameId}/runs/{runId}/notes/{id}");

            response.EnsureSuccessStatusCode();
        }

        public async Task<string?> UploadImageAsync(int gameId, int runId, 
            int id, BitRichTextEditorImageUpload image)
        {
            using var content = new MultipartFormDataContent();
            using var imageContent = new ByteArrayContent(image.Content);

            imageContent.Headers.ContentType =
                 new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);

            content.Add(imageContent, "image", image.FileName);

            var response = await _httpClient.PostAsync(
                $"/api/games/{gameId}/runs/{runId}/notes/{id}/images", content); //TODO seperate images to other controller/endpoint

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(); // returns URL
        }
    }
}
