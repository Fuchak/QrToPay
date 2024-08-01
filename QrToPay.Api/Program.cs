using Microsoft.EntityFrameworkCore;
using FluentValidation;
using QrToPay.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<QrToPayDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Get the current assembly
var assembly = typeof(Program).Assembly;

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(assembly);

// Add MediatR
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "QrToPay API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();