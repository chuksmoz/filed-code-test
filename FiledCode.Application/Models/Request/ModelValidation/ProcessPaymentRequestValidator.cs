using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiledCode.Application.Models.Request.ModelValidation
{
    public class ProcessPaymentRequestValidator: AbstractValidator<ProcessPaymentRequest>
    {
        public ProcessPaymentRequestValidator()
        {
            RuleFor(p => p.CreditCardNumber).NotEmpty().CreditCard();
            RuleFor(p => p.Amount).NotEmpty().GreaterThan(0);
            RuleFor(p => p.ExpirationDate).NotEmpty().Must(IsGreaterThanToday);
            RuleFor(p => p.CardHolder).NotEmpty();
            RuleFor(p => p.SecurityCode).Must(IsNullOrNotGreaterThan3Chars).WithMessage("Security code must be equal to 3 charaters or empty");
        }

        private bool IsGreaterThanToday(DateTime dateTime)
        {
            return DateTime.Today < dateTime;
        }

        private bool IsNullOrNotGreaterThan3Chars(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }
            else if(value.Length == 3)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
