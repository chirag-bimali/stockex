using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SE.API.Middlewares;
using SE.API.Services;
using SE.Infrastructure.Persistence.Configurations;
using SE.Infrastructure.Persistence.Seeding;

var builder = WebApplication.CreateBuilder(args);

var jwtKey =
    builder.Configuration["Jwt:SigningKey"]
    ?? throw new InvalidOperationException("JWT signing key is missing.");

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),

            RoleClaimType = "role", // where roles are stored
            NameClaimType = JwtRegisteredClaimNames.Sub, // optional, for User.Identity.Name
        };
        options.MapInboundClaims = false; // keep "sub" and "email" as written
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Auth failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
        };
    });

builder.Services.AddAuthorization();

// Register Infrastructure services
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Database connection string 'DefaultConnection' was not found."
    );

builder.Services.AddControllers();

// Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT access token.",
        }
    );
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>(),
    });
});

builder.Services.AddInfrastructure(connectionString);
builder.Services.AddSeeders();
builder.Services.AddApplication();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

var app = builder.Build();
app.UseExceptionHandler(); // safety net, keep it first
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.ConfigObject.PersistAuthorization = true;
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
