using Microsoft.EntityFrameworkCore;
using FluentValidation;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Filters;
using QrToPay.Api.Common.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container and add global filters
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.AddDbContext<QrToPayDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });

// Get the current assembly
var assembly = typeof(Program).Assembly;

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(assembly);

// Add MediatR
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

builder.Services.AddSingleton<VerificationStorageService>();

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