using System.ComponentModel.DataAnnotations;

namespace CashCare.Models
{
    public class DailyExpense
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }
    }
}
