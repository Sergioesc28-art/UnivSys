namespace UnivSys.API.Models.DTOs
{
    public class LoginResponseDTO
    {
        public string Username { get; set; }
        public string Rol { get; set; }
        public string Token { get; set; } // El JWT
    }
}