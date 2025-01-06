using CarparkAssignment.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<CarparkDbContext>(options =>
    options.UseSqlite("Data Source=carpark.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
        Version = "v1",
        Title = "Carpark API",
        Description = "An API to search and favourite carparks"
    });

    // Include XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
}); // Registers Swagger services to generate API documentation and testing interface

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Carpark API v1");
    options.RoutePrefix = string.Empty; // Swagger at the root
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
