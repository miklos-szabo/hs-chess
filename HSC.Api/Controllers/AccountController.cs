using HSC.Bll.AccountService;
using HSC.Transfer.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSC.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task CreateUserIfDoesntExistAsync()
        {
            await _accountService.CreateUserIfDoesntExistAsync();
        }

        [HttpGet]
        public async Task<UserMenuDto> GetUserMenuData()
        {
            return await _accountService.GetUserMenuData();
        }

        [HttpGet]

        public async Task<UserFullDetailsDto> GetFullUserData()
        {
            return await _accountService.GetFullUserData();
        }

        [HttpPost]
        public async Task ChangeRealMoneyAsync(bool toRealMoney)
        {
            await _accountService.ChangeRealMoneyAsync(toRealMoney);
        }

        [HttpGet]
        public async Task<bool> UsesLightThemeAsync()
        {
            return await _accountService.UsesLightThemeAsync();
        }

        [HttpPost]
        public async Task SetLightTheme(bool isLightTheme)
        {
            await _accountService.SetLightTheme(isLightTheme);
        }
    }
}
