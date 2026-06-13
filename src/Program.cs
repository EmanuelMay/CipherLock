using System.Text;
using CipherLock.Application.Services;
using CipherLock.Domain.Exceptions;
using CipherLock.Application.Interfaces;
using CipherLock.Application.Interfaces.Repositories;
using CipherLock.Application.Interfaces.Services;
using CipherLock.Infrastructure.Context;
using CipherLock.Infrastructure.Repositories;
using CipherLock.Infrastructure.Services;
using CipherLock.Presentation.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using CipherLock.Application.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(cfg => {}, typeof(VaultProfile), typeof(UserProfile), typeof(CredentialProfile));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IVaultService, VaultService>();
builder.Services.AddScoped<ICredentialService, CredentialService>();
builder.Services.AddScoped<IEncryptService, EncryptionService>();
builder.Services.AddScoped<IHashService, HashService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVaultRepository, VaultRepository>();
builder.Services.AddScoped<ICredentialRepository, CredentialRepository>();

string? mySqlConnection = builder.Configuration["ConnectionStrings:DefaultConnection"]
    ?? throw new ConnectionStringException("connection string is not configured");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        mySqlConnection,
        new MariaDbServerVersion(new Version(11, 4))
    )
);

builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var jwtKey = builder.Configuration["Jwt:PrivateKey"]
    ?? throw new JwtKeyException("jwt key is not configured");
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer   = true,
            ValidIssuer      = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience    = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew        = TimeSpan.Zero,
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler(app.Environment);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
