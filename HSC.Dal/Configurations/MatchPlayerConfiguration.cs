using HSC.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HSC.Dal.Configurations
{
    internal class MatchPlayerConfiguration : IEntityTypeConfiguration<MatchPlayer>
    {
        public void Configure(EntityTypeBuilder<MatchPlayer> builder)
        {
            builder.ToTable("MatchPlayer");

            builder.HasOne(m => m.Match)
                .WithMany(t => t.MatchPlayers)
                .HasForeignKey(m => m.MatchId)
                .HasConstraintName("FK_MatchPlayer_Match");
        }
    }
}
