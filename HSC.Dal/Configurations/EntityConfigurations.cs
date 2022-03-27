using Microsoft.EntityFrameworkCore;

namespace HSC.Dal.Configurations
{
    public class EntityConfigurations
    {
        public static void ConfigureAllEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlockConfiguration());
            modelBuilder.ApplyConfiguration(new ChallengeConfiguration());
            modelBuilder.ApplyConfiguration(new FriendRequestConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new MatchConfiguration());
            modelBuilder.ApplyConfiguration(new TournamentConfiguration());
            modelBuilder.ApplyConfiguration(new TournamentMessageConfiguration());
            modelBuilder.ApplyConfiguration(new TournamentPlayerConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new MatchPlayerConfiguration());
            modelBuilder.ApplyConfiguration(new SearchingPlayerConfiguration());
        }
    }
}
