using HSC.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HSC.Dal.Configurations
{
    internal class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.ToTable("Match");

            builder.HasOne(m => m.Tournament)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.TournamentId)
                .HasConstraintName("FK_Match_Tournament");

            builder.HasOne(m => m.Analysis)
                .WithOne(a => a.Match)
                .HasForeignKey<Analysis>(a => a.MatchId)
                .HasConstraintName("FK_Match_Analysis");
        }
    }
}
