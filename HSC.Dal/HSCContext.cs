using HSC.Dal.Configurations;
using HSC.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace HSC.Dal
{
    public class HSCContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentMessage> TournamentMessages { get; set; }
        public DbSet<TournamentPlayer> TournamentPlayers { get; set; }

        public HSCContext(DbContextOptions<HSCContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityConfigurations.ConfigureAllEntities(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
