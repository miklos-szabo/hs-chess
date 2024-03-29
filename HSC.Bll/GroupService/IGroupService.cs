﻿using HSC.Transfer.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.GroupService
{
    public interface IGroupService
    {
        Task CreateGroupAsync(string groupName, string description);
        Task<List<GroupDto>> GetGroupsAsync(string searchText);
        Task<GroupDetailsDto> GetGroupDetailsAsync(int groupId);
        Task JoinGroupAsync(int groupId);
    }
}
