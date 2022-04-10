using AutoMapper;
using HSC.Api.ExceptionHandling;
using HSC.Api.Extensions;
using HSC.Bll.BettingService;
using HSC.Bll.Hubs;
using HSC.Bll.Mappings;
using HSC.Bll.MatchFinderService;
using HSC.Common.Options;
using HSC.Common.RequestContext;
using OnlineAuction.Api.RequestContext;
using OnlineAuction.Dal;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _env;

    private readonly string _debugCorsPolicy = "DebugCorsPolicy";

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        _configuration = configuration;
        _env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionStringOptions = services.ConfigureOption<ConnectionStringOptions>(_configuration);

        services.AddControllers(options =>
        {
            options.Filters.Add<HttpResponseExceptionFilter>();
        });

        services.AddLogging();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerDocument();
        services.AddSignalR();

        services.AddCors(options =>
        {
            options.AddPolicy(_debugCorsPolicy, builder =>
            {
                builder.WithOrigins("https://localhost:5000/")
                .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new Mappings()));
        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddSpaStaticFiles(configuration => configuration.RootPath = "wwwroot");

        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        services.AddScoped<IRequestContext, RequestContext>();
        services.AddScoped<IMatchFinderService, MatchFinderService>();
        services.AddScoped<IBettingService, BettingService>();
        services.AddDAL(connectionStringOptions);
        
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseCors(builder => builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        }

        app.UseHttpsRedirection();
        app.UseCors(_debugCorsPolicy);
        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<ChessHub>("/hubs/chessHub");
        });

        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "../HSC.Web";

            if (env.IsDevelopment())
            {
                spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
            }
        });
    }
}
