using UnivSys.API.Data;
using UnivSys.API.Services; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UnivSys.API.Controllers;

var builder = WebApplication.CreateBuilder(args);

// --- Configuración de CORS ---
// Define el nombre de la política CORS para usarla en el middleware.
const string MyAllowSpecificOrigins = "_myAngularClientOrigins"; 

// --- 1. Configuración de Servicios ---

// A. Conexión a Base de Datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)); 

// B. Inyección de Dependencias
builder.Services.AddScoped<EstudianteService>(); 
builder.Services.AddScoped<AuthController>();

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

// F. CONFIGURACIÓN DE CORS (Añadido)
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // Orígenes permitidos (necesarios para tu app Angular)
                          policy.WithOrigins("http://localhost:4200",         // Angular Dev Server
                                             "https://gestion-estudiantes.com", // Dominio de Producción
                                             "http://localhost:5000")          // Si la API corre en IIS/Kestrel diferente
                                .AllowAnyHeader()    // Permite cabeceras personalizadas (crucial para el JWT Bearer Token)
                                .AllowAnyMethod();   // Permite verbos HTTP: GET, POST, PUT, DELETE
                      });
});

var app = builder.Build();

// --- 2. Pipeline HTTP ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();

// IMPORTANTE: El Middleware de CORS debe ir después de UseRouting y antes de UseAuthorization/UseEndpoints
app.UseCors(MyAllowSpecificOrigins); 

// Importante: Usar Autenticación y Autorización
app.UseAuthentication(); 
app.UseAuthorization(); 

// Mapeo de Controladores
app.MapControllers();

app.Run();