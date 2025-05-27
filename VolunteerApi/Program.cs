using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VolunteerApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configura la conexión a la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura opciones (por ejemplo, si usás opciones personalizadas como GoogleAI)
builder.Services.Configure<GoogleAIOptions>(builder.Configuration.GetSection("GoogleAI"));

// Agrega servicios de controladores con configuración JSON limpia
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = null;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Registra Swagger (documentación de la API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agrega HttpClientFactory
builder.Services.AddHttpClient();

// Logging
builder.Services.AddLogging();

// Configura CORS para permitir todo (útil para frontend desplegado en otro dominio)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Activa Swagger tanto en desarrollo como en producción
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS (¡antes de routing!)
app.UseCors("AllowAllOrigins");

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
