using HSC.Common.RequestContext;
using HSC.Dal;
using HSC.Transfer.Friends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.FriendService
{
    public class FriendService : IFriendService
    {
        private readonly HSCContext _dbContext;
        private readonly IRequestContext _requestContext;

        public FriendService(HSCContext dbContext, IRequestContext requestContext)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
        }
        public Task AcceptFriendRequestAsync(int requestId)
        {
            throw new NotImplementedException();
        }

        public Task<List<FriendDto>> GetFriendsAsync()
        {
            throw new NotImplementedException();
        }

        public Task SendFriendRequestAsync(string toUserName)
        {
            throw new NotImplementedException();
        }
    }
}
