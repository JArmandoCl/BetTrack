using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BetTrack.Api
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        string baseUrl = "http://btws.somee.com/api/";
        public ApiClient()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var jsonRequest = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Si la respuesta esperada es string, la retornamos directamente
            if (typeof(TResponse) == typeof(string))
            {
                return (TResponse)(object)jsonResponse;
            }

            // Intentamos deserializar solo si la respuesta parece un JSON
            if (jsonResponse.Trim().StartsWith("{") || jsonResponse.Trim().StartsWith("["))
            {
                return JsonSerializer.Deserialize<TResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            throw new InvalidOperationException("La respuesta no es un JSON válido.");
        }


        public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var jsonRequest = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }
    }

}
