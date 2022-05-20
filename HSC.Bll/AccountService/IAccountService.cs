using HSC.Transfer.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.AccountService
{
    public interface IAccountService
    {
        Task CreateUserIfDoesntExistAsync();
        Task<UserMenuDto> GetUserMenuData();
        Task<UserFullDetailsDto> GetFullUserData();
        Task ChangeRealMoneyAsync(bool toRealMoney);
        Task<bool> UsesLightThemeAsync();
        Task SetLightTheme(bool isLightTheme);
    }
}
