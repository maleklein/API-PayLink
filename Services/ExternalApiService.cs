using System.Net.Http.Json;
//Este servicio se encarga de interactuar con APIs externas para obtener facturas de negocios asociados.
namespace PayLink.Services
{
    public class ExternalApiService
    {
        private readonly HttpClient _httpClient; //clase de .NET para hacer solicitudes HTTP a APIs externas

        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //  Lógica para consultar la API de un negocio externo y traer una factura
        public async Task<object?> GetInvoiceFromBusinessAsync(string apiUrl, int billId)
        {
            try
            {
                // Construye la URL completa. 
                // la apiurl la recibe del cuerpo de un negocio creado por el sistema que maneja las facturas.
                // por ejemplo: https://negocio.com/api/bills/{billId}
                var requestUrl = $"{apiUrl}/bills/{billId}";


                _httpClient.DefaultRequestHeaders.Clear(); 

                // Realiza la solicitud HTTP GET
                var response = await _httpClient.GetAsync(requestUrl); 

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error al consultar API externa: {response.StatusCode}");
                    return null;
                }

                // Deserializa el contenido JSON en un objeto genérico
                var data = await response.Content.ReadFromJsonAsync<object>();
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ExternalApiService: {ex.Message}");
                return null;
            }
        }
    }
}
