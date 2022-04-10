using HSC.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Reflection.Emit;

namespace HSC.Dal.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
{
            builder.ToTable("User");

            builder.HasKey(u => u.UserName);

            builder.HasMany(c => c.Friends)
               .WithMany(c => c.FriendsOf)
               .UsingEntity<Friend>(
                    e => e.HasOne<User>().WithMany().HasForeignKey(e => e.UserName1),
                    e => e.HasOne<User>().WithMany().HasForeignKey(e => e.UserName2));
        }
    }
}
