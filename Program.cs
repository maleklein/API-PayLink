using Microsoft.EntityFrameworkCore;
using PayLink.Data;
using PayLink.Services;
using System.Net;
using DotNetEnv; // Para leer variables del archivo .env
using Microsoft.OpenApi.Models;
using PayLink.Middlewares;

// Carga de variables desde el archivo .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// 🧩 Servicios base
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PayLink API", Version = "v1" });

    // Definición de seguridad tipo API Key
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Ingrese su API Key en el campo: X-API-KEY",
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
        //NBasicamente agrega un campo Authorize arriba en swagger, para poder agregar la X-API-KEY 
    });

    // Mostrar el compo API Key en todas las operaciones desde el swagger
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                Scheme = "ApiKeyScheme",
                Name = "X-API-KEY",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


//  SQL Server (lee la conexión desde .env o appsettings.json)
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                      ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PayLinkDbContext>(options =>
    options.UseSqlServer(connectionString));

//  Servicio HTTP para comunicarse con APIs externas
builder.Services.AddHttpClient<ExternalApiService>(); //Crea e inyecta un HttpClient configurado para el servicio ExternalApiService
// (el que usa tu API para llamar a negocios externos y traer facturas).

//  Inyección de dependencias (servicios)
builder.Services.AddScoped<ExternalApiService>();
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

var app = builder.Build();

//  Middleware de autenticación por API Key
//  Middleware de autenticación por API Key
app.UseMiddleware<ApiKeyMiddleware>();

//  Swagger y rutas
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
