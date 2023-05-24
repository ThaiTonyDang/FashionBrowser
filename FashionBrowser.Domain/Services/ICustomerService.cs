﻿using FashionBrowser.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Domain.Services
{
    public interface ICustomerService
    {
        public Task<bool> CreateCustomerInfor(CustomerItemViewModel customerItemViewModel);
    }
}
