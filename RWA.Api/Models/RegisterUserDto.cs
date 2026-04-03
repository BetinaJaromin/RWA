using System.ComponentModel.DataAnnotations;

namespace RWA.Api.Models
{
    public class RegisterUserDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
        public int? RoleId { get; set; }
    }
}
