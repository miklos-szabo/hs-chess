using Microsoft.EntityFrameworkCore;

namespace HSC.Dal.Configurations
{
    public class EntityConfigurations
    {
        public static void ConfigureAllEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
