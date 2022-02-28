using HSC.Dal.Configurations;
using HSC.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace HSC.Dal
{
    public class HSCContext : DbContext
    {
        public DbSet<User> Users { get; set; }

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
