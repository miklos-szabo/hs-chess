using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.User
{
    public class UserMenuDto
    {
        public string UserName { get; set; }
        public decimal PlayMoneyBalance { get; set; }
        public decimal Balance { get; set; }
        public bool IsUsingPlayMoney { get; set; }
    }
}
