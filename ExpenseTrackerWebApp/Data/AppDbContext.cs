using ExpenseTrackerWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerWebApp.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) 
        {
            
        }

        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Expense>().ToTable(nameof(Expense));

            builder.Entity<Expense>()
            .HasOne(e => e.User)
            .WithMany(e => e.Expenses)
            .HasForeignKey(e => e.userId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
