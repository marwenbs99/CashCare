using System.ComponentModel.DataAnnotations;

namespace CashCare.Models
{
    public class DailyExpense
    {
        [Key]
        public int Id { get; set; }

        public required string Description { get; set; } = "";

        [Required]
        public decimal Amount { get; set; } = 0;

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}
