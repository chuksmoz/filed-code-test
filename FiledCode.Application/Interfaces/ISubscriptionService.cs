using FiledCode.Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FiledCode.Application.Interfaces
{
    public interface ISubscriptionService
    {
        Task<bool> ProcessPayment(ProcessPaymentRequest request);
    }
}
