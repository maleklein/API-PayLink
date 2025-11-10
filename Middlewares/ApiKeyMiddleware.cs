//Este archivo define un middleware personalizado para manejar la autenticación mediante API Key en una aplicación ASP.NET Core.
//Es decir, valida que cada solicitud HTTP incluya una API Key válida en los headers antes de permitir el acceso a los endpoints protegidos.
//Menos a Swagger y al endpoint de creación de negocios (POST /api/business), que son públicos.
using System.Net;
using PayLink.Data;

namespace PayLink.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next; //Este objeto no lo creo yo, sino que lo crea el framework cuando se inicializa el middleware, osea cuando se hace una solicitud HTTP.
        private const string ApiKeyHeaderName = "X-API-KEY"; // Nombre del header HTTP donde se espera encontrar la API Key, se mete como constante para evitar errores de tipeo.

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, PayLinkDbContext dbContext) //Metodo que se ejecuta para cada http request
        //HttpContext context: representa la solicitud actual (ruta, headers, body, usuario, etc.).
        //PayLinkDbContext dbContext: se inyecta automáticamente y te da acceso a la base de datos.
        {
            var path = context.Request.Path.Value?.ToLower() ?? ""; //la ruta que el cliente está pidiendo (/api/business, /swagger, etc.)
            // Se pasa a minúsculas para comparar más fácilmente.
            var method = context.Request.Method.ToUpper(); // el método HTTP (GET, POST, PUT, DELETE), en mayúsculas para mantener consistencia.

            // Permitir libre acceso a Swagger y al registro de negocios (POST de business)
            if (path.Contains("swagger") || path.Contains("index.html") || 
                (path.Contains("/api/business") && method == "POST")) 
                //si el usuario está accediendo a Swagger o está creando un nuevo negocio (POST /api/business),
                //no se le exige API Key.
            {
                await _next(context); //Esto quiere decirle al middleware (el codigo entre la solicitud y la respuesta) 
                // que continúe con la siguiente etapa del procesamiento de la solicitud.
                return;
            }

            // Verificar si el header X-API-KEY está presente
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey)) //intenta extraer el valor del header X-API-KEY
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized; // Si no está presente, devuelve 401 Unauthorized al contexto actual osea la solicitud HTTP.
                await context.Response.WriteAsync("Falta el encabezado X-API-KEY."); // Mensaje de error en el cuerpo de la respuesta.
                return;
            }

            // Buscar la ApiKey en la base de datos
            var apiKeyString = extractedApiKey.ToString(); // Convierte el valor del header (StringValues) en un string simple, que EF Core sí puede comparar contra el campo ApiKey en la base.
            var business = dbContext.Businesses.FirstOrDefault(b => b.ApiKey == apiKeyString); // Verifica si la API Key existe en la tabla Businesses, porque cada negocio tiene su propia API Key.
                                                                                               // Si no existe, corta la ejecución y devuelve 401 Unauthorized.

            if (business == null) // Si no se encuentra un negocio con esa API Key 
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("API Key inválida o negocio no autorizado.");
                return;
            }

            //  Si todo está bien, continuar, se puede hacer la solicitud
            await _next(context);
        }
    }
}