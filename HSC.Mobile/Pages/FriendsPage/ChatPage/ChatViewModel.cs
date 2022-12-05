using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Mobile.Pages.FriendsPage.ChatPage
{
    public class ChatViewModel: BaseViewModel
    {
        private string _otherUserName;

        public string OtherUserName
        {
            get => _otherUserName;
            set => SetField(ref _otherUserName, value);
        }
    }
}
