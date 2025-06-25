using System.ComponentModel.DataAnnotations;

namespace SmartHome.Application.DTOs.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(20, ErrorMessage = "First name cannot exceed 20 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(20, ErrorMessage = "Last name cannot exceed 20 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(20, ErrorMessage = "Username cannot exceed 20 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
