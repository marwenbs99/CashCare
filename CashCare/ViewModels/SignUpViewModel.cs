using System.ComponentModel.DataAnnotations;

namespace CashCare.ViewModels
{
    public class SignUpViewModel
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string RepeatedPassword { get; set; }
    }
}
