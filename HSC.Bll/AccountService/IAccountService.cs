﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.AccountService
{
    public interface IAccountService
    {
        Task CreateUserIfDoesntExistAsync();
    }
}
