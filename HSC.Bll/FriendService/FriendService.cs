using HSC.Bll.Hubs.Clients;
using HSC.Bll.Hubs;
using HSC.Common.RequestContext;
using HSC.Dal;
using HSC.Transfer.Friends;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using HSC.Common.Exceptions;
using HSC.Common.Resources;
using Microsoft.Extensions.Localization;

namespace HSC.Bll.FriendService
{
    public class FriendService : IFriendService
    {
        private readonly HSCContext _dbContext;
        private readonly IRequestContext _requestContext;
        private readonly IHubContext<ChessHub, IChessClient> _chessHub;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<LocalizedStrings> _localizer;

        public FriendService(HSCContext dbContext, IRequestContext requestContext, IHubContext<ChessHub, IChessClient> chessHub, IMapper mapper, IStringLocalizer<LocalizedStrings> localizer)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
            _chessHub = chessHub;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task AcceptFriendRequestAsync(int requestId)
        {
            var request = await _dbContext.FriendRequests.SingleAsync(r => r.Id == requestId);
            var user1 = await _dbContext.Users.SingleAsync(u => u.UserName == request.RequesterUsername);
            var user2 = await _dbContext.Users.SingleAsync(u => u.UserName == request.ReceiverUsername);

            user1.Friends.Add(user2);
            _dbContext.FriendRequests.Remove(request);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeclineFriendRequestAsync(int requestId)
        {
            _dbContext.FriendRequests.Remove(_dbContext.FriendRequests.SingleOrDefault(r => r.Id == requestId));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<FriendRequestDto>> GetFriendRequestsAsync()
        {
            var requests = await _dbContext.FriendRequests
                .Where(r => r.RequesterUsername == _requestContext.UserName || r.ReceiverUsername == _requestContext.UserName)
                .ProjectTo<FriendRequestDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            requests.ForEach(r =>
            {
                r.IsIncoming = r.ReceiverUserName == _requestContext.UserName;
            });

            return requests;
        }

        public async Task<List<FriendDto>> GetFriendsAsync()
        {
            var friends = await _dbContext.Users.Where(u => u.UserName == _requestContext.UserName).SelectMany(u => u.Friends)
                .ProjectTo<FriendDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            friends.AddRange(await _dbContext.Users.Where(u => u.UserName == _requestContext.UserName)
                .SelectMany(u => u.FriendsOf).ProjectTo<FriendDto>(_mapper.ConfigurationProvider)
                .ToListAsync());

            var newMessages = await _dbContext.ChatMessages.Where(m => m.ReceiverUserName == _requestContext.UserName && !m.IsSeen).ToListAsync();

            friends.ForEach(f =>
            {
                f.NewMessagesCount = newMessages.Count(m => m.SenderUserName == f.UserName);
            });

            return friends;
        }

        public async Task<bool> IsNewNotificationShown()
        {
            // Show the notification if there's a friend request or an unseen message
            var anyFriendRequests = await _dbContext.FriendRequests.AnyAsync(r => r.ReceiverUsername == _requestContext.UserName);
            if (!anyFriendRequests)
            {
                return await _dbContext.ChatMessages.AnyAsync(m => m.ReceiverUserName == _requestContext.UserName && !m.IsSeen);
            }
            return anyFriendRequests;
        }

        public async Task SendFriendRequestAsync(string toUserName)
        {
            if (!(await _dbContext.Users.AnyAsync(u => u.UserName == toUserName)))
                throw new BadRequestException($"{_localizer["UserDoesntExist_1"]} {toUserName} {_localizer["UserDoesntExist_2"]}");

            if (await _dbContext.FriendRequests.AnyAsync(r => r.ReceiverUsername == toUserName && r.RequesterUsername == _requestContext.UserName))
                throw new BadRequestException(_localizer["FriendRequestAlreadySent"]);

            _dbContext.FriendRequests.Add(new Dal.Entities.FriendRequest
            {
                ReceiverUsername = toUserName,
                RequesterUsername = _requestContext.UserName,
            });

            await _dbContext.SaveChangesAsync();

            await _chessHub.Clients.User(toUserName).ReceiveFriendRequest(_requestContext.UserName);
        }
    }
}
