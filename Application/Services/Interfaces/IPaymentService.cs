using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IPaymentService
    {
        ValueTask<IActionResult> AddPaymente(Payment payment);
        Task<Payment> GetPaymentByStatus(string status);
    }
}
