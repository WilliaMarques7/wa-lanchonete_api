using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> GetOrderByFilterAsync(Expression<Func<Order, bool>> predicate);
        Task<IEnumerable<Order>> GetOrderListByFilterAsync(Expression<Func<Order, bool>> predicate);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<bool> GetUnicOrderNumberAsync(string orderCode);
    }
}
