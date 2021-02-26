using FiledCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiledCode.Infrastructure.Data.Context
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }

    }
}
