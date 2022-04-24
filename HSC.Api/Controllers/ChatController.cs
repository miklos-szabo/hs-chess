using HSC.Bll.ChatService;
using HSC.Transfer.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSC.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task SendChatMessage(string toUserName, string message)
        {
            await _chatService.SendChatMessage(toUserName, message);
        }

        [HttpGet]
        public async Task<List<ChatMessageDto>> GetChatMessages(string fromUserName, int pageSize, int page)
        {
            return await _chatService.GetChatMessages(fromUserName, pageSize, page);
        }

        [HttpPost]
        public async Task MessagesRead(string fromUserName)
        {
            await _chatService.MessagesRead(fromUserName);
        }
    }
}
