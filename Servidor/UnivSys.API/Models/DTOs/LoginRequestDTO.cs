using System.ComponentModel.DataAnnotations;
namespace UnivSys.API.Models.DTOs
{
    public class LoginRequestDTO
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}