using Application.Contracts.Request.RequestOrder;
using Application.Contracts.Response.ResponseOrder;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
using ZXing;
using ZXing.SkiaSharp;
using ZXing.SkiaSharp.Rendering;

namespace wa_lanchonete_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper, ICustomerService customerService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var OrderResponse = _mapper.Map<IEnumerable<OrderResponse>>(orders);
            return Ok(OrderResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            var OrderResponse = _mapper.Map<OrderResponse>(order);
            return Ok(OrderResponse);
        }

        [HttpGet("OrderNumber")]
        public async Task<IActionResult> GetOrderByOrderNumber(string orderNumber)
        {
            var order = await _orderService.GetOrderByOrderNumber(orderNumber);
            if (order == null)
            {
                return NotFound();
            }
            var OrderResponse = _mapper.Map<OrderResponse>(order);
            return Ok(OrderResponse);
        }

        [HttpGet("getStatus")]
        public async Task<IActionResult> GetOrderByStatus(string status)
        {
            var orders = await _orderService.GetOrderByStatusAsync(status);
            var OrderResponse = _mapper.Map<IEnumerable<OrderResponse>>(orders);
            return Ok(OrderResponse);
        }

        [HttpPost]
        [Produces("image/png")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderPostRequest OrderRequest)
        {
            var order = _mapper.Map<Order>(OrderRequest);
            var customerOrder = await _customerService.GetCustomerById(OrderRequest.CustomerId);

            QRCodeResponse createdOrder = await _orderService.CreateOrderAsync(order);
            var qrCodeImage = GenerateQRCodeImage(createdOrder.QrData);

            return File(qrCodeImage, "image/png");
        }

        private byte[] GenerateQRCodeImage(string text)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 300,
                    Width = 300
                },
                Renderer = new SKBitmapRenderer()
            };

            using (var bitmap = writer.Write(text))
            {
                using (var image = SKImage.FromBitmap(bitmap))
                {
                    using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                    {
                        return data.ToArray();
                    }
                }
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> PaymentOrder([FromBody] OrderPostRequest OrderRequest)
        //{
        //    var customerOrder = await _customerService.GetCustomerById(OrderRequest.CustomerId);
        //    if (customerOrder == null)
        //        return NotFound(new { Info = "Customer informado não encontrado" });
        //    var createdOrder = await _orderService.PaymentOrderAsync(OrderRequest);
        //    return Ok();
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderRequest orderRequest)
        {
            if (id != orderRequest.Id)
            {
                return BadRequest();
            }

            var order = _mapper.Map<Order>(orderRequest);
            var result = await _orderService.UpdateOrderAsync(order);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("updateStatus")]
        public async Task<IActionResult> UpdateOrderStatusAsync(int id, string status)
        {
            if (!ValidStatus(status))
            {
                return BadRequest(new { error = "Status informado inválido!" });
            }
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound(new { error = "Pedido não encontrado!" });
            }
            if (status == "Finalizado")
            {
                return BadRequest(new { error = "Não é possível atualizar status de um pedido finalizado!" });
            }

            var result = await _orderService.UpdateOrderStatusAsync(id, status);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        private bool ValidStatus(string status)
        {
            List<string> ValidStatuses = new List<string>
            {
                "Recebido",
                "Em preparação",
                "Pronto",
                "Finalizado"
            };
            return ValidStatuses.Contains(status);
        }
    }
}
