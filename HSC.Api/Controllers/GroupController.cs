using HSC.Bll.GroupService;
using HSC.Transfer.Groups;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSC.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public async Task CreateGroupAsync(string groupName, string description)
        {
            await _groupService.CreateGroupAsync(groupName, description);
        }

        [HttpGet]
        public async Task<List<GroupDto>> GetGroupsAsync(string searchText)
        {
            return await _groupService.GetGroupsAsync(searchText);
        }

        [HttpGet]
        public async Task<GroupDetailsDto> GetGroupDetailsAsync(int groupId)
        {
            return await _groupService.GetGroupDetailsAsync(groupId);
        }

        [HttpPost]
        public async Task JoinGroupAsync(int groupId)
        {
            await _groupService.JoinGroupAsync(groupId);
        }
    }
}
