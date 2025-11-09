using System.Net;
using PayLink.Data;

namespace PayLink.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ApiKeyHeaderName = "X-API-KEY";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, PayLinkDbContext dbContext)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";
            var method = context.Request.Method.ToUpper();

            // ✅ Permitir libre acceso a Swagger y al registro de negocios (POST de business)
            if (path.Contains("swagger") || path.Contains("index.html") ||
                (path.Contains("/api/business") && method == "POST"))
            {
                await _next(context);
                return;
            }

            // ✅ Verificar si el header X-API-KEY está presente
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Falta el encabezado X-API-KEY.");
                return;
            }

            // ✅ Buscar la ApiKey en la base de datos
            var apiKeyString = extractedApiKey.ToString(); // Convierte el valor del header (StringValues) en un string simple, que EF Core sí puede comparar contra el campo ApiKey en la base.
            var business = dbContext.Businesses.FirstOrDefault(b => b.ApiKey == apiKeyString); // Verifica si la API Key existe en la tabla Businesses.
                                                                                               // Si no existe, corta la ejecución y devuelve 401 Unauthorized.

            if (business == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("API Key inválida o negocio no autorizado.");
                return;
            }

            // ✅ Si todo está bien, continuar
            await _next(context);
        }
    }
}