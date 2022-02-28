using HSC.Common.Options;
using HSC.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineAuction.Dal
{
    public static class Wireup
    {
        public static void AddDAL(this IServiceCollection services, ConnectionStringOptions connectionStringOptions)
        {
            services.AddDbContext<HSCContext>(options =>
            {
                options.UseSqlServer(
                    connectionStringOptions.DefaultConnection,
                    x => x.MigrationsAssembly("HSC.Dal"));
            });
        }
    }
}
