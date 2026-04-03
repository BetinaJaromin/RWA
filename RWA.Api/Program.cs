using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using RWA.Api;
using RWA.Api.Authorization;
using RWA.Api.Authorization.Handlers;
using RWA.Api.Entities;
using RWA.Api.Middleware;
using RWA.Api.Models;
using RWA.Api.Models.Validators;
using RWA.Api.Services;
using RWA.Api.Services.Interfaces;
using System.Data.Common;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RestaurantDbContext>
    (options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));
builder.Services.AddScoped<RestaurantSeeder>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();
builder.Services.AddScoped<IPasswordHasher<User>,PasswordHasher<User>>();

//JWT Authentication
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = authenticationSettings.JwtIssuer,
            ValidAudience = authenticationSettings.JwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", policy =>
        policy.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!));
});

//policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HasNationality", policy => policy.RequireClaim("Nationality", "German"));
    options.AddPolicy("AtLeast20", policy => policy.AddRequirements(new MinimumAgeRequirement(20)));
    options.AddPolicy("CreatedAtleast2Restaurant", policy => policy.AddRequirements(new CreatedMultipleRestaurantsRequirement(2)));

});
builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CreatedMultipleRestaurantsRequirementHandler>();

builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpContextAccessor(); // można wstrzyknąć IHttpContextAccessor i pobrać HttpContext, a z niego User i inne informacje o żądaniu

//FluentValidator
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RestaurantQueryValidator>();

//SWAGGER
builder.Services.AddSwaggerGen();

//MAPSTER
var config = TypeAdapterConfig.GlobalSettings;
config.Scan(Assembly.GetExecutingAssembly()); // automatycznie znajdzie IRegister
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();

// NLog: Register as Logging Provider
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

//SEEDER
using var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
seeder.Seed();

//SWAGGER
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RWA API");
});

//CACHE
app.UseResponseCaching();

//Static files
app.UseStaticFiles(); // umożliwia serwowanie statycznych plików z wwwroot (np. zdjęcia restauracji)

app.UseHttpsRedirection();
//CORS
app.UseCors("FrontEndClient"); 


app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
