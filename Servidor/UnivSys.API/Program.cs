using UnivSys.API.Data;
using UnivSys.API.Services; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuración de Servicios ---

// A. Conexión a Base de Datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)); 

// B. Inyección de Dependencias
builder.Services.AddScoped<EstudianteService>(); 

// C. Controladores y Swagger
builder.Services.AddControllers(); 
builder.Services.AddOpenApi();

// D. Configuración de Autenticación JWT 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, 
            ValidateAudience = false, 
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"] ?? "UnaClaveTemporalSeguraDebeEstarAqui"))
        };
    });

// E. Configuración de Autorización (Roles)
builder.Services.AddAuthorization(); 

var app = builder.Build();

// --- 2. Pipeline HTTP ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();

// Importante: Usar Autenticación y Autorización
app.UseAuthentication(); 
app.UseAuthorization(); 

// Mapeo de Controladores
app.MapControllers();

app.Run();