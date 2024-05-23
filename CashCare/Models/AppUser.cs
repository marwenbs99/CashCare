using System.ComponentModel.DataAnnotations;

namespace CashCare.Models
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required string Email { get; set; }

        public int PhoneNumber { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
