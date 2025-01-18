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
using WorkerService;

var builder = Host.CreateApplicationBuilder(args);

// Configurar serviços
builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddHttpClient<IWeatherService, WeatherService>();

builder.Services.AddScoped<WeatherUpdateJob>();

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

// Registrar Worker
builder.Services.AddHostedService<Worker>();

// Configurar AutoMapper se necessário
builder.Services.AddAutoMapper(typeof(WeatherForecastProfile));

var app = builder.Build();

// Executar a aplicação
app.Run();
