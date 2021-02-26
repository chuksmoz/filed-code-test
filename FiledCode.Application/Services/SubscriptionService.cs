using FiledCode.Application.Interfaces;
using FiledCode.Application.Models.Request;
using FiledCode.Domain.Entities;
using FiledCode.Domain.Enums;
using FiledCode.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FiledCode.Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IAsyncRepository<Payment> _paymentRepo;
        private readonly IAsyncRepository<PaymentStatus> _paymentStatusRepo;
        private readonly ICheapPaymentGateway _cheapPaymentGateway;
        private readonly IExpensivePaymentGateway _expensivePaymentGateway;
        private readonly IPremiumPaymentService _premiumPaymentService;
        public SubscriptionService(IAsyncRepository<Payment> paymentRepo, IAsyncRepository<PaymentStatus> paymentStatusRepo, ICheapPaymentGateway cheapPaymentGateway, IExpensivePaymentGateway expensivePaymentGateway, IPremiumPaymentService premiumPaymentService)
        {
            _paymentRepo = paymentRepo;
            _paymentStatusRepo = paymentStatusRepo;
            _cheapPaymentGateway = cheapPaymentGateway;
            _expensivePaymentGateway = expensivePaymentGateway;
            _premiumPaymentService = premiumPaymentService;

        }
        public async Task<bool> ProcessPayment(ProcessPaymentRequest request)
        {
            bool isSuccessful;
            var paymentEntity = new Payment()
            {
                SecurityCode = request.SecurityCode,
                Amount = request.Amount,
                CardHolder = request.CardHolder,
                CreditCardNumber = request.CreditCardNumber,
                ExpirationDate = request.ExpirationDate
            };

            _paymentRepo.Add(paymentEntity);
            await _paymentRepo.SaveChangesAsync();

            if (request.Amount < 20)
            {
                isSuccessful = await _cheapPaymentGateway.SendPayment();

                if (isSuccessful)
                {
                    await AddPaymentStatus(paymentEntity.Id, PaymentState.processed);
                    return true;
                }
                await AddPaymentStatus(paymentEntity.Id, PaymentState.failed);
                return false;
            }

            if (request.Amount > 20 && request.Amount <= 500)
            {
                
                var isExpensiveGatewayAvialible = await _expensivePaymentGateway.IsAvailiable();
                if (isExpensiveGatewayAvialible)
                {
                    isSuccessful = await _expensivePaymentGateway.SendPayment();

                    if (isSuccessful)
                    {
                        await AddPaymentStatus(paymentEntity.Id, PaymentState.processed);
                        return true;
                    }
                    await AddPaymentStatus(paymentEntity.Id, PaymentState.failed);
                    return false;
                }
                else
                {
                    isSuccessful = await _cheapPaymentGateway.SendPayment();

                    if (isSuccessful)
                    {
                        await AddPaymentStatus(paymentEntity.Id, PaymentState.processed);
                        return true;
                    }
                    await AddPaymentStatus(paymentEntity.Id, PaymentState.failed);
                    return false;
                }
            }

            if (request.Amount > 500)
            {
                int numberOfTries = 3;
                while (numberOfTries > 0)
                {
                    isSuccessful = await _premiumPaymentService.SendPayment();

                    if (isSuccessful)
                    {
                        await AddPaymentStatus(paymentEntity.Id, PaymentState.processed);
                        return true;
                        
                    }
                    numberOfTries--;
                }
                await AddPaymentStatus(paymentEntity.Id, PaymentState.failed);
                return false;
            }
            return false;
        }

        private async Task AddPaymentStatus(long paymentId, PaymentState paymentState)
        {
            var paymentStatusEntity = new PaymentStatus()
            {
                PaymentId = paymentId,
                PaymentState = paymentState
            };

            _paymentStatusRepo.Add(paymentStatusEntity);
            await _paymentStatusRepo.SaveChangesAsync();
        }
    }
}
