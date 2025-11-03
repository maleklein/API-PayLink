using System.Net;

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

        public async Task InvokeAsync(HttpContext context, IConfiguration config)
        {
            var path = context.Request.Path.Value ?? "";

            // Permitir Swagger sin autenticación
            if (path.Contains("swagger") || path.Contains("index.html"))
            {
                await _next(context);
                return;
            }

            // Verificar si el header X-API-KEY está presente
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Falta el encabezado X-API-KEY.");
                return;
            }

            // Obtener la clave válida desde configuración
            var apiKey = config["ApiKey"];

            if (string.IsNullOrEmpty(apiKey) || !apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("API Key inválida.");
                return;
            }

            // Si todo está bien, continúa
            await _next(context);
        }
    }
}
