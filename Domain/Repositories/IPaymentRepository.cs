using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task AddPayment(Payment customer);
        Task<Payment> GetPeymentByStatus(string status);
        Task<IEnumerable<Payment>> GetPayments();
    }
}

