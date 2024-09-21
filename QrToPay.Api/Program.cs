using Microsoft.EntityFrameworkCore;
using FluentValidation;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Filters;
using QrToPay.Api.Common.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using QrToPay.Api.Extensions;
using QrToPay.Api.Common.Settings;
using QrToPay.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppAuthSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.AddScoped<UserContextMiddleware>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddDbContext<QrToPayDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth();

var assembly = typeof(Program).Assembly;

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

builder.Services.AddSingleton<VerificationStorageService>();

builder.Services.AddSingleton<TokenProviderService>();

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "QrToPay API V1");
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<UserContextMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();