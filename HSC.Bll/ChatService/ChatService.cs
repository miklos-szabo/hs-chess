using AutoMapper;
using HSC.Bll.Hubs.Clients;
using HSC.Bll.Hubs;
using HSC.Common.RequestContext;
using HSC.Dal;
using HSC.Transfer.SignalR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace HSC.Bll.ChatService
{
    public class ChatService : IChatService
    {
        private readonly HSCContext _dbContext;
        private readonly IRequestContext _requestContext;
        private readonly IHubContext<ChessHub, IChessClient> _chessHub;
        private readonly IMapper _mapper;

        public ChatService(HSCContext dbContext, IRequestContext requestContext, IHubContext<ChessHub, IChessClient> chessHub, IMapper mapper)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
            _chessHub = chessHub;
            _mapper = mapper;
        }

        public async Task<List<ChatMessageDto>> GetChatMessages(string fromUserName, int pageSize, int page)
        {
            return await _dbContext.ChatMessages.Where(m => (m.ReceiverUserName == _requestContext.UserName && m.SenderUserName == fromUserName) ||
            (m.ReceiverUserName == fromUserName && m.SenderUserName == _requestContext.UserName))
                .ProjectTo<ChatMessageDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(m => m.TimeStamp)
                .Skip(page * pageSize).Take(pageSize)
                .ToListAsync();
        }

        public async Task MessagesRead(string fromUserName)
        {
            var messagesReadNow = await _dbContext.ChatMessages
                .Where(m => m.ReceiverUserName == _requestContext.UserName && m.SenderUserName == fromUserName)
                .Where(m => !m.IsSeen)
                .ToListAsync();

            messagesReadNow.ForEach(m =>
            {
                m.IsSeen = true;
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task SendChatMessage(string toUserName, string message)
        {
            _dbContext.ChatMessages.Add(new Dal.Entities.ChatMessage
            {
                ReceiverUserName = toUserName,
                SenderUserName = _requestContext.UserName,
                Message = message,
                TimeStamp = DateTimeOffset.UtcNow,
            });

            await _dbContext.SaveChangesAsync();

            await _chessHub.Clients.User(toUserName).ReceiveMessage(new ChatMessageDto
            {
                Message = message,
                SenderUserName = _requestContext.UserName,
                TimeStamp = DateTimeOffset.UtcNow.ToString("u"),
            });
        }
    }
}
