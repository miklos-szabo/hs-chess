using HSC.Common.RequestContext;
using HSC.Dal;
using HSC.Transfer.Groups;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.GroupService
{
    public class GroupService : IGroupService
    {
        private readonly HSCContext _dbContext;
        private readonly IRequestContext _requestContext;

        public GroupService(HSCContext dbContext, IRequestContext requestContext)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
        }

        public async Task CreateGroupAsync(string groupName, string description)
        {
            var group = new Dal.Entities.Group
            {
                Name = groupName,
                Description = description
            };

            var thisUser = await _dbContext.Users.SingleAsync(u => u.UserName == _requestContext.UserName);
            group.Users.Add(thisUser);

            _dbContext.Groups.Add(group);

            await _dbContext.SaveChangesAsync();
        }

        public Task<GroupDto> GetGroupsAsync()
        {
            throw new NotImplementedException();
        }

        public Task JoinGroupAsync(int groupId)
        {
            throw new NotImplementedException();
        }
    }
}
