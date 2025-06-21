using System.ComponentModel.DataAnnotations;

namespace BiznesApp.Api.Models
{
    public class UserRegisterDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
} 