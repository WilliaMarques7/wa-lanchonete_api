using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(LanchoneteDbContext context) : base(context) { }

        public Task AddPayment(Payment customer)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Payment>> GetPayments()
        {
            throw new NotImplementedException();
        }

        public Task<Payment> GetPeymentByStatus(string status)
        {
            throw new NotImplementedException();
        }
    }
}
