﻿using HSC.Mobile.Data;
using HSC.Mobile.Pages.ChessPage;
using HSC.Mobile.Pages.Settings;
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
            builder.Services.AddLocalization();
            builder.Services.AddLogging();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSingleton<ISettingsService, SettingsService>();

            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<MainPage>();   
            
            builder.Services.AddTransient<ChessPageViewModel>();
            builder.Services.AddTransient<ChessPageView>();

            return builder.Build();
        }
    }
}