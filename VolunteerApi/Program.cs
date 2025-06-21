using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VolunteerApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Alas_Chiquitanas.Models; // Asegúrate de que esté el namespace correcto
using Alas_Chiquitanas.Services; // Si tienes el NotificacionService ahí

var builder = WebApplication.CreateBuilder(args);

// Configura la conexión a la base de datos SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura GoogleAI si lo usas
builder.Services.Configure<GoogleAIOptions>(builder.Configuration.GetSection("GoogleAI"));

// ⬇️ Agrega configuración de MongoDB
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<NotificacionService>();

// Agrega servicios de controladores y configuración JSON limpia
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = null;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HttpClient
builder.Services.AddHttpClient();

// Logging
builder.Services.AddLogging();

// 💥 CORS global
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Swagger en Dev y Prod
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirTodo");

app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        await context.Response.CompleteAsync();
    }
    else
    {
        await next();
    }
});

app.UseAuthorization();
app.MapControllers();

app.Run();
