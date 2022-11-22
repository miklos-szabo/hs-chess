using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Keycloak.Net;
using Keycloak.Net.Models.ClientInitialAccess;

namespace HSC.Mobile.Pages.QuickMatchPage
{
    public class QuickMatchViewModel: BaseViewModel
    {
        public ICommand SearchCommand { get; set; }
        public ICommand createCustomCommand { get; set; }

        public QuickMatchViewModel()
        {
            SearchCommand = new Command(async () => await SearchForMatch());
        }

        public async Task SearchForMatch()
        {
            var x = new KeycloakClient("http://hsckeycloak10.c5dzcec2bngmbcds.eastus.azurecontainer.io:8080", "user2", "user2");
            var ahh = await x.CreateInitialAccessTokenAsync("chess", new ClientInitialAccessCreatePresentation());
            Console.WriteLine(ahh.Token);
        }
    }
}
