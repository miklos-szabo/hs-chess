using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using Font = Microsoft.Maui.Font;

namespace HSC.Mobile.Services
{
    public class AlertService
    {
        private Task ShowAlertAsync(string title, string message, string cancel = "OK")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        private Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        public void ShowAlert(string title, string message, string cancel = "OK")
        {
            Application.Current.MainPage.Dispatcher.Dispatch(async () =>
                await ShowAlertAsync(title, message, cancel)
            );
        }

        public void ShowConfirmation(string title, string message, Action<bool> callback,
            string accept = "Yes", string cancel = "No")
        {
            Application.Current.MainPage.Dispatcher.Dispatch(async () =>
            {
                bool answer = await ShowConfirmationAsync(title, message, accept, cancel);
                callback(answer);
            });
        }

        public async Task DisplayInfoNoti(string text)
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.Black,
                CornerRadius = new CornerRadius(10),
                Font = Font.SystemFontOfSize(14),
                ActionButtonFont = Font.SystemFontOfSize(14)
            };

            TimeSpan duration = TimeSpan.FromSeconds(3);

            var snackbar = Snackbar.Make(text, null, "", duration, snackbarOptions);
            await snackbar.Show();
        }

        public async Task DisplaySuccessNoti(string text)
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Colors.LightGreen,
                TextColor = Colors.Black,
                CornerRadius = new CornerRadius(10),
                Font = Font.SystemFontOfSize(14),
                ActionButtonFont = Font.SystemFontOfSize(14)
            };

            TimeSpan duration = TimeSpan.FromSeconds(3);

            var snackbar = Snackbar.Make(text, null, "", duration, snackbarOptions);
            await snackbar.Show();
        }

        public async Task DisplayErrorNoti(string text)
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Colors.Red,
                TextColor = Colors.Black,
                CornerRadius = new CornerRadius(10),
                Font = Font.SystemFontOfSize(14),
                ActionButtonFont = Font.SystemFontOfSize(14)
            };

            TimeSpan duration = TimeSpan.FromSeconds(3);

            var snackbar = Snackbar.Make(text, null, "", duration, snackbarOptions);
            await snackbar.Show();
        }

        public async Task DisplayWarningNoti(string text)
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Colors.Orange,
                TextColor = Colors.White,
                CornerRadius = new CornerRadius(10),
                Font = Font.SystemFontOfSize(14),
                ActionButtonFont = Font.SystemFontOfSize(14)
            };

            TimeSpan duration = TimeSpan.FromSeconds(3);

            var snackbar = Snackbar.Make(text, null, "", duration, snackbarOptions);
            await snackbar.Show();
        }

        public async Task DisplayInfoActionNoti(string text, string buttonText, Action callback)
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.Black,
                ActionButtonTextColor = Colors.Orange,
                CornerRadius = new CornerRadius(10),
                Font = Font.SystemFontOfSize(14),
                ActionButtonFont = Font.SystemFontOfSize(14),
                CharacterSpacing = 0.5
            };

            TimeSpan duration = TimeSpan.FromSeconds(3);
            var snackbar = Snackbar.Make(text, callback, buttonText, duration, snackbarOptions);
            await snackbar.Show();
        }

    }
}
