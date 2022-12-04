using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HSCApi;

namespace HSC.Mobile.Pages.CashierPage
{
    public class CashierViewModel: BaseViewModel
    {
        private readonly AccountService _accountService;
        private decimal _balance;

        public ICommand Add100Command { get; set; }

        public decimal Balance
        {
            get => _balance;
            set => SetField(ref _balance, value);
        }

        public CashierViewModel(AccountService accountService)
        {
            _accountService = accountService;

            Add100Command = new Command(async () => await Add100());

            Task.Run(async () =>
            {
                var data = await _accountService.GetUserMenuDataAsync();
                Balance = data.Balance;
            });
        }

        public async Task Add100()
        {
            await _accountService.AddMoneyAsync(100);
            var data = await _accountService.GetUserMenuDataAsync();
            Balance = data.Balance;
        }


    }
}
