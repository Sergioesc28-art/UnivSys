using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UnivSys.API.Data;
using UnivSys.API.Models.DTOs;
using UnivSys.API.Models;
using Microsoft.AspNetCore.Authorization; 

namespace UnivSys.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // ===================================================================
        // ENDPOINT DE REGISTRO (¡NUEVO!)
        // RUTA: POST /api/auth/register
        // ===================================================================
        [HttpPost("register")]
        [AllowAnonymous] // Permitir acceso anónimo (para poder crear el primer usuario)
        public async Task<IActionResult> Register([FromBody] RegistroRequestDTO registroDto)
        {
            // 1. Validar si el rol existe
            var rolExiste = await _context.Roles.AnyAsync(r => r.IDRol == registroDto.IDRol);
            if (!rolExiste)
            {
                return BadRequest(new { Mensaje = "El IDRol proporcionado no es válido." });
            }

            // 2. Validar si el usuario ya existe
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Username == registroDto.Username);
            if (usuarioExiste)
            {
                // 409 Conflict (El recurso ya existe)
                return Conflict(new { Mensaje = "El nombre de usuario ya está en uso." });
            }

            // 3. Hashear la contraseña (¡NUNCA GUARDAR EN TEXTO PLANO!)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registroDto.Password);

            // 4. Crear el nuevo usuario
            var nuevoUsuario = new Usuario
            {
                Username = registroDto.Username,
                PasswordHash = passwordHash,
                IDRol = registroDto.IDRol
            };

            // 5. Guardar en la Base de Datos
            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            // 6. Devolver 201 Created
            return CreatedAtAction(nameof(Register), new { id = nuevoUsuario.IDUsuario }, new { Mensaje = "Usuario creado exitosamente." });
        }


        // ===================================================================
        // ENDPOINT DE LOGIN
        // RUTA: POST /api/auth/login
        // ===================================================================
        [HttpPost("login")]
        [AllowAnonymous] // Permitir acceso anónimo
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            // 1. Buscar al usuario y su rol
            var usuario = await _context.Usuarios
                .Include(u => u.Rol) 
                .FirstOrDefaultAsync(u => u.Username == loginRequest.Username);

            // 2. Validar Usuario y Contraseña (con BCrypt)
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, usuario.PasswordHash))
            {
                return Unauthorized(new { Mensaje = "Credenciales inválidas." });
            }

            // 3. Generar el Token JWT
            var token = GenerarToken(usuario);

            // 4. Devolver 200 OK con el token y los roles
            return Ok(new LoginResponseDTO
            {
                Username = usuario.Username,
                Rol = usuario.Rol.NombreRol,
                Token = token
            });
        }

        // Método Helper para Generar el JWT
        private string GenerarToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IDUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.Role, usuario.Rol.NombreRol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config["Jwt:Key"] ?? "UnaClaveTemporalSeguraDebeEstarAqui"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var now = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(2),
                NotBefore = now,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}