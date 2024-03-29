﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using HSC.Common.Extensions;
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
        private readonly IMapper _mapper;

        public GroupService(HSCContext dbContext, IRequestContext requestContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
            _mapper = mapper;
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

        public async Task<GroupDetailsDto> GetGroupDetailsAsync(int groupId)
        {
            var group = await _dbContext.Groups
                .ProjectTo<GroupDetailsDto>(_mapper.ConfigurationProvider)
                .SingleAsync(g => g.Id == groupId);

            var user = await _dbContext.Users.Include(u => u.FriendsOf).Include(u => u.Friends)
                .SingleAsync(u => u.UserName == _requestContext.UserName);
            var friends = user.Friends.Select(f => f.UserName).ToList();
            friends.AddRange(user.FriendsOf.Select(f => f.UserName).ToList());

            group.IsInGroup = group.Users.Any(m => m.UserName == _requestContext.UserName);
            group.Users.ForEach(m => m.IsFriend = friends.Contains(m.UserName));

            return group;
        }

        public async Task<List<GroupDto>> GetGroupsAsync(string searchText)
        {
            // Other groups
            var groups = await _dbContext.Groups
                .Where(g => !g.Users.Any(u => u.UserName == _requestContext.UserName))
                .ProjectTo<GroupDto>(_mapper.ConfigurationProvider)
                .Where(!string.IsNullOrEmpty(searchText), g => g.Name.ToLower().Contains(searchText.ToLower()))
                .Take(20).OrderByDescending(g => g.UserCount)
                .ToListAsync();

            // Own groups
            groups.AddRange(await _dbContext.Groups
                .Where(g => g.Users.Any(u => u.UserName == _requestContext.UserName))
                .ProjectTo<GroupDto>(_mapper.ConfigurationProvider)
                .Where(!string.IsNullOrEmpty(searchText), g => g.Name.ToLower().Contains(searchText.ToLower()))
                .OrderByDescending(g => g.UserCount)
                .ToListAsync());

            var user = await _dbContext.Users.Include(u => u.Groups).SingleAsync(u => u.UserName == _requestContext.UserName);

            groups.ForEach(g =>
            {
                g.IsInGroup = user.Groups.Any(group => g.Name == group.Name);
            });

            return groups;
        }

        public async Task JoinGroupAsync(int groupId)
        {
            var user = await _dbContext.Users.SingleAsync(u => u.UserName == _requestContext.UserName);
            var group = await _dbContext.Groups.SingleAsync(g => g.Id == groupId);

            user.Groups.Add(group);
            await _dbContext.SaveChangesAsync();
        }
    }
}
