using HSC.Transfer.Friends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.FriendService
{
    public interface IFriendService
    {
        Task SendFriendRequestAsync(string toUserName);
        Task AcceptFriendRequestAsync(int requestId);
        Task<List<FriendDto>> GetFriendsAsync();
    }
}
