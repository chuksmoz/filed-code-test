﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FiledCode.Application.Interfaces
{
    public interface IPremiumPaymentService
    {
        Task<bool> SendPayment();
    }
}
