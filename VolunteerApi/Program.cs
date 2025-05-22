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

// Configura opciones (ejemplo: GoogleAI)
builder.Services.Configure<GoogleAIOptions>(builder.Configuration.GetSection("GoogleAI"));

// Agrega servicios de controladores y configuración JSON limpia (sin referencias circulares)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = null; // sin Preserve
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Registra Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agrega HttpClientFactory
builder.Services.AddHttpClient();

// Logging
builder.Services.AddLogging();

// Configura CORS para permitir todo (útil en desarrollo)
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

// Configuración para entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Deshabilitar la redirección a HTTPS para desarrollo local
// Si no quieres usar HTTPS en desarrollo, puedes eliminar esta línea
// app.UseHttpsRedirection(); // Comenta o elimina esta línea

// Middleware
app.UseCors("AllowAllOrigins");

app.UseRouting();
app.UseAuthorization();

// Mapear los controladores
app.MapControllers();

// Ejecuta la aplicación
app.Run();
