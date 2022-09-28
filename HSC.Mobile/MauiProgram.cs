using HSC.Mobile.Data;
using HSC.Mobile.Pages.ChessPage;
using Microsoft.AspNetCore.Components.WebView.Maui;

namespace HSC.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();

            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<MainPage>();   
            
            builder.Services.AddTransient<ChessPageViewModel>();
            builder.Services.AddTransient<ChessPageView>();

            return builder.Build();
        }
    }
}