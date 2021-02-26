using FiledCode.Application.Interfaces;
using FiledCode.Application.Models.Request;
using FiledCode.Application.Services;
using FiledCode.Domain.Entities;
using FiledCode.Domain.Interfaces;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FiledCode.Test
{
    public class ProcessPaymentTest
    {
        
        [Fact]
        public async void ProcessPayment_With_Cheap_Payment_Gateway_When_Amount_Range_Less_Than_20_Should_Be_Successful()
        {
            var _paymentRepo = new Mock<IAsyncRepository<Payment>>();
            var _paymentStatusRepo = new Mock<IAsyncRepository<PaymentStatus>>();
            var _cheapPaymentGateway = new Mock<ICheapPaymentGateway>();
            var _expensivePaymentGateway = new Mock<IExpensivePaymentGateway>();
            var _premiumPaymentService = new Mock<IPremiumPaymentService>();


            //_paymentRepo.Setup(x => x.)
            _cheapPaymentGateway.Setup(x => x.SendPayment()).Returns(Task.FromResult(true));
            var sut = new SubscriptionService(_paymentRepo.Object, _paymentStatusRepo.Object, _cheapPaymentGateway.Object, _expensivePaymentGateway.Object, _premiumPaymentService.Object);

            var paymentReq = new ProcessPaymentRequest()
            {
                Amount = 10
            };

             var result = await sut.ProcessPayment(paymentReq);

            _cheapPaymentGateway.Verify(c => c.SendPayment(), Times.Once);

            result.ShouldBe(true);
            


        }

        [Fact]
        public async void ProcessPayment_With_Expensive_Payment_Gateway_When_Amount_Range_Greather_Than_20_Should_Be_Successful_And_Verify_Expensive_Payment_Gateway_Was_Called_Once()
        {
            var _paymentRepo = new Mock<IAsyncRepository<Payment>>();
            var _paymentStatusRepo = new Mock<IAsyncRepository<PaymentStatus>>();
            var _cheapPaymentGateway = new Mock<ICheapPaymentGateway>();
            var _expensivePaymentGateway = new Mock<IExpensivePaymentGateway>();
            var _premiumPaymentService = new Mock<IPremiumPaymentService>();


            //_paymentRepo.Setup(x => x.)
            _expensivePaymentGateway.Setup(x => x.IsAvailiable()).Returns(Task.FromResult(true));
            _expensivePaymentGateway.Setup(x => x.SendPayment()).Returns(Task.FromResult(true));
            var sut = new SubscriptionService(_paymentRepo.Object, _paymentStatusRepo.Object, _cheapPaymentGateway.Object, _expensivePaymentGateway.Object, _premiumPaymentService.Object);

            var paymentReq = new ProcessPaymentRequest()
            {
                Amount = 22
            };

            var result = await sut.ProcessPayment(paymentReq);

            _expensivePaymentGateway.Verify(c => c.SendPayment(), Times.Once);

            result.ShouldBe(true);



        }


        [Fact]
        public async void ProcessPayment_With_Cheap_Payment_Gateway_When_Amount_Range_Greather_Than_20_Should_Be_Successful_And_Verify_Cheap_Payment_Gateway_Was_Called_Once()
        {
            var _paymentRepo = new Mock<IAsyncRepository<Payment>>();
            var _paymentStatusRepo = new Mock<IAsyncRepository<PaymentStatus>>();
            var _cheapPaymentGateway = new Mock<ICheapPaymentGateway>();
            var _expensivePaymentGateway = new Mock<IExpensivePaymentGateway>();
            var _premiumPaymentService = new Mock<IPremiumPaymentService>();


            //_paymentRepo.Setup(x => x.)
            _expensivePaymentGateway.Setup(x => x.IsAvailiable()).Returns(Task.FromResult(false));
            _cheapPaymentGateway.Setup(x => x.SendPayment()).Returns(Task.FromResult(true));
            _expensivePaymentGateway.Setup(x => x.SendPayment()).Returns(Task.FromResult(true));
            var sut = new SubscriptionService(_paymentRepo.Object, _paymentStatusRepo.Object, _cheapPaymentGateway.Object, _expensivePaymentGateway.Object, _premiumPaymentService.Object);

            var paymentReq = new ProcessPaymentRequest()
            {
                Amount = 22
            };

            var result = await sut.ProcessPayment(paymentReq);

            _cheapPaymentGateway.Verify(c => c.SendPayment(), Times.Once);

            result.ShouldBe(true);



        }

        [Fact]
        public async void ProcessPayment_With_Premium__Payment_Service_When_Amount_Range_Greather_Than_500_Should_Be_Successful_And_Verify_Service_Was_Called_Once()
        {
            var _paymentRepo = new Mock<IAsyncRepository<Payment>>();
            var _paymentStatusRepo = new Mock<IAsyncRepository<PaymentStatus>>();
            var _cheapPaymentGateway = new Mock<ICheapPaymentGateway>();
            var _expensivePaymentGateway = new Mock<IExpensivePaymentGateway>();
            var _premiumPaymentService = new Mock<IPremiumPaymentService>();


            //_paymentRepo.Setup(x => x.)
            _premiumPaymentService.Setup(x => x.SendPayment()).Returns(Task.FromResult(true));
            var sut = new SubscriptionService(_paymentRepo.Object, _paymentStatusRepo.Object, _cheapPaymentGateway.Object, _expensivePaymentGateway.Object, _premiumPaymentService.Object);

            var paymentReq = new ProcessPaymentRequest()
            {
                Amount = 510
            };

            var result = await sut.ProcessPayment(paymentReq);

            _premiumPaymentService.Verify(c => c.SendPayment(), Times.Once);

            result.ShouldBe(true);



        }

        [Fact]
        public async void ProcessPayment_With_Premium__Payment_Service_When_Amount_Range_Greather_Than_500_Should_Fail_And_Verify_Service_Was_Called_3_Times()
        {
            var _paymentRepo = new Mock<IAsyncRepository<Payment>>();
            var _paymentStatusRepo = new Mock<IAsyncRepository<PaymentStatus>>();
            var _cheapPaymentGateway = new Mock<ICheapPaymentGateway>();
            var _expensivePaymentGateway = new Mock<IExpensivePaymentGateway>();
            var _premiumPaymentService = new Mock<IPremiumPaymentService>();


            //_paymentRepo.Setup(x => x.)
            _premiumPaymentService.Setup(x => x.SendPayment()).Returns(Task.FromResult(false));
            var sut = new SubscriptionService(_paymentRepo.Object, _paymentStatusRepo.Object, _cheapPaymentGateway.Object, _expensivePaymentGateway.Object, _premiumPaymentService.Object);

            var paymentReq = new ProcessPaymentRequest()
            {
                Amount = 510
            };

            var result = await sut.ProcessPayment(paymentReq);

            _premiumPaymentService.Verify(c => c.SendPayment(), Times.Exactly(3));

            result.ShouldBe(false);



        }
    }
}
