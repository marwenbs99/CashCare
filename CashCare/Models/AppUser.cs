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

        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public required string Password { get; set; }
        public DateTime DateOfInscription { get; set; } = DateTime.Now;

        public ICollection<DailyExpense> ExpensesDaily { get; set; }
    }
}
