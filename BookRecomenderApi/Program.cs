using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using BookRecomenderApi.Models;
using BookRecomenderApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var config = new ServerConfig();
builder.Configuration.Bind(config);

// Register configurations
builder.Services.AddSingleton(config.MongoDB);
builder.Services.AddSingleton(config.RabbitMQ);

// Register MongoDB context and repository
builder.Services.AddSingleton<IRecomendationContext, RecomendationContext>();
builder.Services.AddSingleton<IRecomendationRepository, RecomendationRepository>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Book Recomendation API",
        Version = "v1",
        Description = "API for book recommendations"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Recomendation API V1");
    // Set Swagger UI at the root
    c.RoutePrefix = string.Empty;
});

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
