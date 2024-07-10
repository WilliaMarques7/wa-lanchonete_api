using Application.Contracts.Request.RequestOrder;
using Application.Contracts.Response.ResponseOrder;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using static Application.Contracts.Request.RequestOrder.PaymentRequest;

namespace Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> GetOrderByOrderNumber(string orderNumber);
        Task<IEnumerable<Order>> GetOrderByStatusAsync(string status);
        //Task<Order> CreateOrderAsync(Order order);
        Task<QRCodeResponse> CreateOrderAsync(Order order);
        Task<OrderPostRequest> PaymentOrderAsync(OrderPostRequest order);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> UpdateOrderStatusAsync(int id, string status);
        Task<bool> DeleteOrderAsync(int id);
    }
}
