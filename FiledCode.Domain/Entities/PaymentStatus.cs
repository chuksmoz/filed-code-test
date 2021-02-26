using FiledCode.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiledCode.Domain.Entities
{
    public class PaymentStatus
    {
        public long Id { get; set; }
        public PaymentState PaymentState { get; set; }
        public long PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
