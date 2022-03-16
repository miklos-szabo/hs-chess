using HSC.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HSC.Dal.Configurations
{
    internal class TournamentPlayerConfiguration : IEntityTypeConfiguration<TournamentPlayer>
    {
        public void Configure(EntityTypeBuilder<TournamentPlayer> builder)
        {
            builder.ToTable("TournamentPlayer");

            builder.HasKey(tp => new { tp.TournamentId, tp.UserName });

            builder.HasOne(tp => tp.Tournament)
                .WithMany(t => t.Players)
                .HasForeignKey(d => d.TournamentId)
                .HasConstraintName("FK_TournamentPlayer_Tournament");
        }
    }
}
