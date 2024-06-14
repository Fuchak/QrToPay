using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;

namespace QrToPay.Api.Data
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTicket>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTickets)
                .HasForeignKey(ut => ut.UserID);
        }
    }
}