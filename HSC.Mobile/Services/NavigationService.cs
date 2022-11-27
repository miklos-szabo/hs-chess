using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Mobile.Pages.FlyoutPage;

namespace HSC.Mobile.Services
{
    public class NavigationService
    {
        public void ChangeMainPage(Page page)
        {
            Application.Current.MainPage = page;
        }

        public void ChangeDetailPage(Page page)
        {
            if (MainThread.IsMainThread)
            {
                (Application.Current.MainPage as FlyoutPage).Detail = new NavigationPage(page);
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    (Application.Current.MainPage as FlyoutPage).Detail = new NavigationPage(page);
                });
            }
        }

        public async Task PushAsync(Page page)
        {
            await ((Application.Current.MainPage as FlyoutPage).Detail as NavigationPage).PushAsync(new NavigationPage(page));
        }

        public async Task PopAsync()
        {
            await ((Application.Current.MainPage as FlyoutPage).Detail as NavigationPage).PopAsync();
        }
    }
}
