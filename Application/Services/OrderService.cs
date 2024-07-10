using Application.Contracts.Request.RequestOrder;
using Application.Contracts.Response.ResponseOrder;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using System.Text;
using System.Text.Json;
using static Application.Contracts.Request.RequestOrder.PaymentRequest;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRepository<Orderstatus> _orderStatusRepository;
        private readonly IRepository<Orderitem> _orderItensRepository;
        private readonly IRepository<Payment> _orderPaymentRepository;
        private readonly HttpClient _httpclient;
        private readonly IRepository<Product> _productRepository;
        private readonly IPaymentRepository paymentRepository;

        public OrderService(IOrderRepository orderRepository, IRepository<Orderstatus> orderStatusRepository, IRepository<Orderitem> orderItensRepository, IPaymentRepository orderPaymentRepository, HttpClient httpclient, IRepository<Product> productRepository)
        {
            _orderRepository = orderRepository;
            _orderStatusRepository = orderStatusRepository;
            _orderItensRepository = orderItensRepository;
            _orderPaymentRepository = orderPaymentRepository;
            _httpclient = httpclient;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetOrderByFilterAsync(o => o.Id == id);
        }

        public async Task<Order> GetOrderByOrderNumber(string orderNumber)
        {
            return await _orderRepository.GetOrderByFilterAsync(o => o.OrderNumber == orderNumber);
        }

        public async Task<IEnumerable<Order>> GetOrderByStatusAsync(string status)
        {
            return await _orderRepository.GetOrderListByFilterAsync(o => o.Status == status);
        }

        public async Task<QRCodeResponse> CreateOrderAsync(Order order)
        {
            order.OrderNumber = await GenerateOrderCodeAsync();
            order.Status = "Recebido";
            order.TotalPrice = order.Orderitems.Sum(x => x.Price);

            var createdOrder = await _orderRepository.CreateAsync(order);

            var orderStatus = new Orderstatus
            {
                OrderId = createdOrder.Id,
                Status = "Recebido"
            };

            await _orderStatusRepository.CreateAsync(orderStatus);

            var qrCode = await this.GerarLinhaDigitavelQrCode(order);

            return qrCode;
        }

        public async Task<OrderPostRequest> PaymentOrderAsync(OrderPostRequest order)
        {
            //await teste(order);
            return order;

        }

        private async Task<QRCodeResponse> GerarLinhaDigitavelQrCode(Order order)
        {
            var product = await _productRepository.GetByIdAsync((int)order.Orderitems.FirstOrDefault().ProductId);

            OrderDto mercado = new OrderDto();
            mercado.total_amount = order.Orderitems.Sum(x => x.Price.Value * x.Quantity.Value);
            mercado.description = "Pedido compra alimentos";
            mercado.title = $"Venda lanchonete - pedido 1";
            mercado.external_reference = $"reference_{order.OrderNumber}";
            mercado.cash_out.amount = 0;
            mercado.notification_url = "https://www.yourserver.com/notifications";

            mercado.items = new List<OrderDto.Item>
            {
                new OrderDto.Item
                {
                    sku_number = "A123K9191938",
                    category = "marketplace",
                    title = product.Name,
                    description = product.Description,
                    unit_price = (decimal)product.Price,
                    quantity = (int)order.Orderitems.ToList().FirstOrDefault().Quantity,
                    unit_measure = "unit",
                    total_amount = order.Orderitems.Sum(x => x.Price.Value * x.Quantity.Value)
                }
            };

            try
            {
                var apiUrl = "https://api.mercadopago.com/instore/orders/qr/seller/collectors/186249149/pos/wa001POS001/qrs";
                var jsonData = JsonSerializer.Serialize(mercado);
                var content = new StringContent(jsonData, Encoding.UTF8, "aplication/json");
                // Adiciona o cabeçalho Authorization
                _httpclient.DefaultRequestHeaders.Add("Authorization", "Bearer TEST-7611799853194381-052816-ced911c8c14e503ce144bc29765a32e8-186249149");

                HttpResponseMessage response = await _httpclient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var qrCodeDtoResponse = JsonSerializer.Deserialize<QRCodeResponse>(responseData);

                    Payment payment = new Payment();
                    payment.PaymentStatus = (int)EnumStatusPayment.PagamentoPendente;
                    payment.OrderId = order.Id;
                    payment.InStoreOrderId = qrCodeDtoResponse.InStoreOrderId;
                    payment.QrData = qrCodeDtoResponse.QrData;
                    payment.PaymentDate = DateTime.Now;
                    payment.PaymentMethod = "Pix";

                    await _orderPaymentRepository.CreateAsync(payment);

                    return qrCodeDtoResponse;
                }
                else
                    throw new HttpRequestException($"{(int)response.StatusCode} - Erro ao acessar a API externa");
            }
            catch (Exception ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }


        public async Task<bool> UpdateOrderAsync(Order order)
        {
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, string status)
        {
            var order = await _orderRepository.GetOrderByFilterAsync(o => o.Id == id);
            order.Status = status;

            var orderStatus = new Orderstatus
            {
                OrderId = order.Id,
                Status = status
            };

            await _orderStatusRepository.CreateAsync(orderStatus);

            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetOrderByFilterAsync(o => o.Id == id);

            if (order == null)
            {
                return false;
            }

            // Exclua os status associados
            foreach (var orderItem in order.Orderstatuses.ToList())
            {
                await _orderStatusRepository.DeleteAsync(orderItem.Id);
            }
            // Exclua os itens associados
            foreach (var orderItem in order.Orderitems.ToList())
            {
                await _orderItensRepository.DeleteAsync(orderItem.Id);
            }
            // Exclua os pagamentos associados
            foreach (var orderItem in order.Payments)
            {
                await _orderPaymentRepository.DeleteAsync(orderItem.Id);
            }

            // Agora exclua o pedido
            return await _orderRepository.DeleteAsync(id);

        }

        private async Task<string> GenerateOrderCodeAsync()
        {
            string orderCode;
            Random random = new Random();
            bool isUnique;

            do
            {
                orderCode = random.Next(10000, 99999).ToString();
                isUnique = !await _orderRepository.GetUnicOrderNumberAsync(orderCode);
            } while (!isUnique);

            return orderCode;
        }
    }
}
