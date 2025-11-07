using System.ComponentModel.DataAnnotations;
namespace UnivSys.API.Models.DTOs
{
    public class LoginRequestDTO
    {
        [Required] public string username { get; set; }
        [Required] public string password { get; set; }
    }
}