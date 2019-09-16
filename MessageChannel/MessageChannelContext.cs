using MessageChannel.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageChannel
{
    public class MessageChannelContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=MessageChannelDb;Trusted_Connection=True;");
        }
    }
}
