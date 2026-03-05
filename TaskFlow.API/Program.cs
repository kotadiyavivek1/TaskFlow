using Microsoft.EntityFrameworkCore;
using TaskFlow.API.MiddleWare;
using TaskFlow.Services.DependencyInjection;
using TaskFlow.Services.AutoMapper;
using TaskFlow.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TaskFlowContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

DependencyInjection.AddServices(builder.Services);
DependencyInjection.AddInfrastructure(builder.Services);

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
// Add services to the container
builder.Services.AddControllers();

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskFlow API v1");
        options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
