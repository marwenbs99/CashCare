using CashCare.Models;
using Microsoft.EntityFrameworkCore;

namespace CashCare.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<AppUser> AppUsers { get; set; }
    }
}
