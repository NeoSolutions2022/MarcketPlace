using MarcketPlace.Api.Configuration;
using MarcketPlace.Application;
using MarcketPlace.Infra;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var  myAllowSpecificOrigins = "_myAllowSpecificOrigins";

//Melhorar os logs
builder
    .Host
    .UseSystemd()
    .UseSerilog((_, lc) =>
    {
        lc.WriteTo.Console();
        lc.WriteTo.Debug();
    });

builder
    .Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder
    .Services
    .AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
    });

builder.Services.ConfigureApplication(builder.Configuration);
builder.Services.AddApiConfiguration();
builder.Services.ConfigureServices();
builder.Services.AddVersioning();
builder.Services.AddSwagger();
builder.Services.AddAuthenticationConfig(builder.Configuration);
builder.Services.AddOpenTelemetryTracingConfig(builder.Configuration);

// add azure

// Add CORs
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy  =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myAllowSpecificOrigins);

app.UseMigrations(app.Services);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

// app.UseStaticFileConfiguration(app.Configuration);

app.MapControllers();

app.Run();