using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FiledCode.Application.Interfaces
{
    public interface IExpensivePaymentGateway
    {
        Task<bool> IsAvailiable();
        Task<bool> SendPayment();
    }
}
