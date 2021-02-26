using FiledCode.Application.Interfaces;
using FiledCode.Application.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiledCode.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }
        [HttpPost("processPayment")]
        public async Task<ActionResult> ProcessPayment([FromBody] ProcessPaymentRequest request)
        {
            return Ok(await _subscriptionService.ProcessPayment(request));
        }

    }
}
