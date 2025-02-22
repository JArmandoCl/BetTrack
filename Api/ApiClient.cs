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
        string bearerToken = "";
        public ApiClient(string bearerToken)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            this.bearerToken = bearerToken;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var request = CreateRequestAsync(HttpMethod.Get, endpoint);
            var response = await _httpClient.SendAsync(request);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException($"401-Not Authorized.");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Bad request: {response.StatusCode} - {response.ReasonPhrase}");
            }
            return JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var jsonRequest = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var request = CreateRequestAsync(HttpMethod.Post, endpoint, content);
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException($"401-Not Authorized.");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Bad request: {response.StatusCode} - {response.ReasonPhrase}");
            }

            return JsonSerializer.Deserialize<TResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


        public async Task PutAsync<TRequest>(string endpoint, TRequest data)
        {
            var jsonRequest = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var request = CreateRequestAsync(HttpMethod.Put, endpoint, content);
            var response = await _httpClient.SendAsync(request);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException($"401-Not Authorized.");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Bad request: {response.StatusCode} - {response.ReasonPhrase}");
            }            
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            var request = CreateRequestAsync(HttpMethod.Delete, endpoint);
            var response = await _httpClient.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException($"401-Not Authorized.");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Bad request: {response.StatusCode} - {response.ReasonPhrase}");
            }
            return response.IsSuccessStatusCode;
        }

        private HttpRequestMessage CreateRequestAsync(HttpMethod method, string endpoint, HttpContent content = null)
        {
            var request = new HttpRequestMessage(method, endpoint)
            {
                Content = content
            };

            if (!string.IsNullOrEmpty(bearerToken))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
            }

            return request;
        }
        #region Extras
        public static DateTime ObtenerFechaActual()
        {
            try
            {
                // Definir el ID de zona horaria de México
                string zonaHorariaId = DeviceInfo.Platform == DevicePlatform.Android ? "America/Mexico_City" : "Central Standard Time"; // Android usa "America/Mexico_City", Windows usa "Central Standard Time"

                // Obtener la zona horaria
                TimeZoneInfo zonaHorariaMexico = TimeZoneInfo.FindSystemTimeZoneById(zonaHorariaId);

                // Obtener la fecha y hora UTC actual
                DateTime fechaUtc = DateTime.UtcNow;

                // Convertir la fecha UTC a la zona horaria de México
                return TimeZoneInfo.ConvertTimeFromUtc(fechaUtc, zonaHorariaMexico);
            }
            catch (TimeZoneNotFoundException)
            {
                // Manejar el caso en el que la zona horaria no se encuentra
                return DateTime.UtcNow;
            }
            catch (Exception)
            {
                // Devolver la fecha del sistema si hay otro error
                return DateTime.Now;
            }
        }

        #endregion
    }

}
