using System.Text;
using BobsCorn.Application.Interfaces;
using BobsCorn.Application.Services;
using BobsCorn.Application.Validations;
using BobsCorn.Domain.Services;
using BobsCorn.Infrastructure.Data;
using BobsCorn.Infrastructure.Repositories;
using BobsCorn.Infrastructure.Services;
using DotNetEnv;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
Env.Load();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
}); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserLoginValidator>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserRegisterValidator>());

builder.Services.AddDbContext<BobCornDbContext>(options =>
    options.UseInMemoryDatabase("BobCornDb"));

builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddScoped<IUserProductLogRepository, UserProductLogRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Env.GetString("JWT_ISSUER") ?? "BobCornIssuer",
        ValidAudience = Env.GetString("JWT_AUDIENCE") ?? "BobCornAudience",
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Env.GetString("JWT_SECRET") ?? "SuperSecretKey12345")),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            builder.WithOrigins("https://localhost:62435")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BobCornDbContext>();
    InMemorySeeder.Seed(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
