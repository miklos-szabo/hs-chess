﻿using System.Net.Http.Headers;
using CommunityToolkit.Maui;
using HSC.Mobile.Handlers;
using HSC.Mobile.Pages.AuthenticationPage;
using HSC.Mobile.Pages.CashierPage;
using HSC.Mobile.Pages.ChessPage;
using HSC.Mobile.Pages.CustomGamesPage;
using HSC.Mobile.Pages.FlyoutPage;
using HSC.Mobile.Pages.FriendsPage;
using HSC.Mobile.Pages.FriendsPage.ChatPage;
using HSC.Mobile.Pages.GroupsPage;
using HSC.Mobile.Pages.GroupsPage.CreateGroupPage;
using HSC.Mobile.Pages.GroupsPage.GroupDetailsPage;
using HSC.Mobile.Pages.HistoryPage;
using HSC.Mobile.Pages.HistoryPage.HistoryChessPage;
using HSC.Mobile.Pages.QuickMatchPage;
using HSC.Mobile.Pages.QuickMatchPage.CreateCustomPage;
using HSC.Mobile.Pages.Settings;
using HSC.Mobile.Pages.TournamentsPage;
using HSC.Mobile.Pages.TournamentsPage.TournamentDetails;
using HSC.Mobile.Services;
using HSCApi;
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
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("materialdesignicons-webfont.ttf", "MaterialDesignIcons");
                });


            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddLocalization();
            builder.Services.AddLogging();

            builder.Services.AddSingleton<SignalrService>();

            builder.Services.AddTransient<TokenHandler>();
            builder.Services.AddHttpClient("Name",
                client =>
                {
                    client.BaseAddress = new Uri("http://hschess.azurewebsites.net");
                }).AddHttpMessageHandler<TokenHandler>();

            builder.Services.AddTransient(
                sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Name")
            );
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<AlertService>();
            builder.Services.AddSingleton<NavigationService>();
            builder.Services.AddSingleton<SettingsService>();
            builder.Services.AddSingleton<CurrentGameService>();
            builder.Services.AddSingleton<EventService>();

            builder.Services.AddTransient<AccountService>();
            builder.Services.AddTransient<AnalysisService>();
            builder.Services.AddTransient<BettingService>();
            builder.Services.AddTransient<ChatService>();
            builder.Services.AddTransient<FriendService>();
            builder.Services.AddTransient<GroupService>();
            builder.Services.AddTransient<HistoryService>();
            builder.Services.AddTransient<MatchService>();
            builder.Services.AddTransient<MatchFinderService>();
            builder.Services.AddTransient<TournamentService>();

            builder.Services.AddTransient<MainPage>();

            builder.Services.AddTransient<AuthenticationPage>();
            builder.Services.AddTransient<AuthenticationViewModel>();

            builder.Services.AddTransient<ChessPageViewModel>();
            builder.Services.AddTransient<ChessPageView>();

            builder.Services.AddTransient<CreateCustomViewModel>();
            builder.Services.AddTransient<CreateCustomPage>();

            builder.Services.AddTransient<QuickMatchPage>();
            builder.Services.AddTransient<QuickMatchViewModel>();

            builder.Services.AddTransient<CashierPage>();
            builder.Services.AddTransient<CashierViewModel>();

            builder.Services.AddTransient<CustomGamesPage>();
            builder.Services.AddTransient<CustomGamesViewModel>();

            builder.Services.AddTransient<HscFlyoutNavigation>();
            builder.Services.AddTransient<HscFlyoutPage>();
            builder.Services.AddTransient<HscFlyoutPageViewModel>();

            builder.Services.AddTransient<FriendsPage>();
            builder.Services.AddTransient<FriendsViewModel>();

            builder.Services.AddTransient<ChatViewModel>();
            builder.Services.AddTransient<ChatPage>();

            builder.Services.AddTransient<GroupsPage>();
            builder.Services.AddTransient<GroupsViewModel>();

            builder.Services.AddTransient<GroupDetailsPage>();
            builder.Services.AddTransient<GroupDetailsViewModel>();

            builder.Services.AddTransient<CreateGroupPage>();
            builder.Services.AddTransient<CreateGroupViewModel>();

            builder.Services.AddTransient<HistoryPage>();
            builder.Services.AddTransient<HistoryViewModel>();

            builder.Services.AddTransient<HistoryChessPage>();
            builder.Services.AddTransient<HistoryChessPageViewModel>();

            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<SettingsViewModel>();

            builder.Services.AddTransient<TournamentsPage>();
            builder.Services.AddTransient<TournamentsViewModel>();

            builder.Services.AddTransient<TournamentDetailsPage>();
            builder.Services.AddTransient<TournamentDetailsViewModel>();

            return builder.Build();
        }
    }
}