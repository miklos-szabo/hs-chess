using HSC.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HSC.Dal.Configurations
{
    internal class TournamentMessageConfiguration : IEntityTypeConfiguration<TournamentMessage>
    {
        public void Configure(EntityTypeBuilder<TournamentMessage> builder)
        {
            builder.ToTable("TournamentMessage");

            builder.HasOne(tm => tm.Tournament)
                .WithMany(t => t.Messages)
                .HasForeignKey(tm => tm.TournamentId)
                .HasConstraintName("FK_TournamentMessage_Tournament");
        }
    }
}
