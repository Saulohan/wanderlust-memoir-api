using Microsoft.OpenApi.Models;
using WanderlustMemoir.Application.Extensions;
using WanderlustMemoir.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

try
{
    // Add services to the container.
    builder.Services.AddControllers();

    // Add Application Services
    builder.Services.AddApplicationServices();

    // Add Infrastructure Services
    builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection") ?? "");

    // Configure Swagger/OpenAPI with minimal configuration
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Wanderlust Memoir API",
            Version = "v1",
            Description = "API para gerenciar destinos de viagem e lugares visitados"
        });
    });

    // Add CORS for frontend
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseAuthorization();

    // Health check endpoint
    app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }))
        .WithName("HealthCheck")
        .WithTags("Health");

    app.MapControllers();

    Console.WriteLine("Application configured successfully. Starting...");
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Application failed to start: {ex}");
    throw;
}
