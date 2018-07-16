using Microsoft.EntityFrameworkCore;

namespace GG.Models
{
    public class GGContext : DbContext
    {
        public GGContext(DbContextOptions<GGContext> options) : base(options) {}
        public DbSet<User> Users {get; set;}
        public DbSet<Attendee> Attendees {get; set;}
        public DbSet<Lobby> Lobbies {get; set;}
    }
}