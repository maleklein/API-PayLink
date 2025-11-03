using System.Net.Http.Json;

namespace PayLink.Services
{
    public class ExternalApiService
    {
        private readonly HttpClient _httpClient;

        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // 🔹 Lógica para consultar la API de un negocio externo y traer una factura
        public async Task<object?> GetInvoiceFromBusinessAsync(string apiUrl, string apiKey, int billId)
        {
            try
            {
                // Construye la URL completa, por ejemplo: https://negocio.com/api/bills/{billId}
                var requestUrl = $"{apiUrl}/bills/{billId}";

                // Limpia headers anteriores y agrega la API Key del negocio
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

                // Realiza la solicitud HTTP GET
                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Error al consultar API externa: {response.StatusCode}");
                    return null;
                }

                // Deserializa el contenido JSON en un objeto genérico
                var data = await response.Content.ReadFromJsonAsync<object>();
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Error en ExternalApiService: {ex.Message}");
                return null;
            }
        }
    }
}