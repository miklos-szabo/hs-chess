using AutoMapper;
using AutoMapper.QueryableExtensions;
using HSC.Common.RequestContext;
using HSC.Dal;
using HSC.Transfer.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly HSCContext _dbContext;
        private readonly IRequestContext _requestContext;
        private readonly IMapper _mapper;

        public AccountService(HSCContext dbContext, IRequestContext requestContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
            _mapper = mapper;
        }

        public async Task CreateUserIfDoesntExistAsync()
        {
            var hasUser = await _dbContext.Users.AnyAsync(u => u.UserName == _requestContext.UserName);

            if (!hasUser)
            {
                _dbContext.Users.Add(new Dal.Entities.User
                {
                    Balance = 0,
                    Email = _requestContext.Email,
                    Name = _requestContext.Name,
                    PlayMoneyBalance = 500,
                    RatingBlitz = 1000,
                    RatingBullet = 1000,
                    RatingClassical = 1000,
                    RatingRapid = 1000,
                    UserName = _requestContext.UserName,
                });
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<UserFullDetailsDto> GetFullUserData()
        {
            return await _dbContext.Users.ProjectTo<UserFullDetailsDto>(_mapper.ConfigurationProvider)
                .SingleAsync(u => u.UserName == _requestContext.UserName);
        }

        public async Task<UserMenuDto> GetUserMenuData()
        {
            return await _dbContext.Users.ProjectTo<UserMenuDto>(_mapper.ConfigurationProvider)
                .SingleAsync(u => u.UserName == _requestContext.UserName);
        }

        public async Task ChangeRealMoneyAsync(bool toRealMoney)
        {
            var user = await _dbContext.Users.SingleAsync(u => u.UserName == _requestContext.UserName);

            user.IsUsingPlayMoney = !toRealMoney;

            await _dbContext.SaveChangesAsync();
        }
    }
}
