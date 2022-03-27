using HSC.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HSC.Dal.Configurations
{
    internal class SearchingPlayerConfiguration : IEntityTypeConfiguration<SearchingPlayer>
    {
        public void Configure(EntityTypeBuilder<SearchingPlayer> builder)
        {
            builder.ToTable("SearchingPlayer");
        }
    }
}
