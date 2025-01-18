using Application.Profiles;
using Application.Services;
using AutoMapper;
using Domain.Interfaces;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using Infrastructure;
using Infrastructure.Repositories;
using Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Weather API", Version = "v1" });
});

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configurar DbContext
builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Repositories e Services
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped<IWeatherService, WeatherService>();

// Configurar HttpClient para WeatherService
builder.Services.AddHttpClient<IWeatherService, WeatherService>();

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(WeatherForecastProfile));

// Configurar Hangfire
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        PrepareSchemaIfNecessary = true,
        QueuePollInterval = TimeSpan.FromSeconds(15)
    }));
builder.Services.AddHangfireServer();

// Registrar Jobs
builder.Services.AddScoped<WeatherUpdateJob>();

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configurar o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Configurar Hangfire Dashboard
app.UseHangfireDashboard();

app.Run();