using FiledCode.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FiledCode.Application.Services
{
    public class CheapPaymentGateway : ICheapPaymentGateway
    {
        public Task<bool> IsAvailiable()
        {
            return Task.FromResult(true);
        }

        public Task<bool> SendPayment()
        {
            return Task.FromResult(true);
        }
    }
}
