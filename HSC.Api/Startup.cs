using AutoMapper;
using HSC.Api.ExceptionHandling;
using HSC.Api.Extensions;
using HSC.Api.RequestContext;
using HSC.Bll.AccountService;
using HSC.Bll.BettingService;
using HSC.Bll.ChatService;
using HSC.Bll.FriendService;
using HSC.Bll.GroupService;
using HSC.Bll.HistoryService;
using HSC.Bll.Hubs;
using HSC.Bll.Mappings;
using HSC.Bll.Match;
using HSC.Bll.MatchFinderService;
using HSC.Common.Options;
using HSC.Common.RequestContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using OnlineAuction.Dal;
using HSC.Bll.RatingService;
using HSC.Bll.Scheduling;
using HSC.Bll.TournamentService;
using Quartz;
using HSC.Bll.TournamentJobService;
using HSC.Bll.Scheduling.Jobs;

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

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.Authority = "http://hsckeycloak10.c5dzcec2bngmbcds.eastus.azurecontainer.io:8080/auth/realms/chess";
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidIssuer = "http://hsckeycloak10.c5dzcec2bngmbcds.eastus.azurecontainer.ioy:8080/auth/realms/chess",
                ValidAudience = "account",
                AuthenticationType = JwtBearerDefaults.AuthenticationScheme,
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    // If the request is for our hub...
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/hubs/chessHub")))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });

        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.SchedulerId = "HSC-Scheduler";
        });

        services.AddQuartzHostedService(
            q => q.WaitForJobsToComplete = true);

        services.AddLogging();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerDocument();
        services.AddSignalR();

        services.AddCors(options =>
        {
            options.AddPolicy(_debugCorsPolicy, builder =>
            {
                builder.WithOrigins("http://localhost:5212")
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
        services.AddScoped<IMatchService, MatchService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IHistoryService, HistoryService>();
        services.AddScoped<IFriendService, FriendService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddSingleton<IUserIdProvider, PreferredUserNameUserIdProvider>();
        services.AddSingleton<IRatingService, RatingService>();
        services.AddSingleton<HSCJobScheduler>();
        services.AddScoped<ITournamentService, TournamentService>();
        services.AddScoped<ITournamentJobService, TournamentJobService>();
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

        //app.UseHttpsRedirection();
        app.UseCors(_debugCorsPolicy);
        app.UseRouting();

        app.UseAuthentication();
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
