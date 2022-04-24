using HSC.Bll.FriendService;
using HSC.Transfer.Friends;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSC.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;

        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [HttpPost]
        public async Task SendFriendRequestAsync(string toUserName)
        {
            await _friendService.SendFriendRequestAsync(toUserName);
        }

        [HttpPost]
        public async Task AcceptFriendRequestAsync(int requestId)
        {
            await _friendService.AcceptFriendRequestAsync(requestId);
        }

        [HttpPost]
        public async Task DeclineFriendRequestAsync(int requestId)
        {
            await _friendService.DeclineFriendRequestAsync(requestId);
        }

        [HttpGet]
        public async Task<List<FriendDto>> GetFriendsAsync()
        {
            return await _friendService.GetFriendsAsync();
        }

        [HttpGet]
        public async Task<List<FriendRequestDto>> GetFriendRequestsAsync()
        {
            return await _friendService.GetFriendRequestsAsync();
        }

        [HttpGet]
        public async Task<bool> IsNewNotificationShown()
        {
            return await _friendService.IsNewNotificationShown();
        }
    }
}
