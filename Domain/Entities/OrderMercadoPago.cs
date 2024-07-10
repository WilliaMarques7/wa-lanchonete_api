using Domain.Entities;
namespace wa_lanchonete_api.Extensions.MercadoPago
{
    public class OrderMercadoPago
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? UrlPost { get; set; }
        public string? UrlNotification { get; set; }
        public string? Token { get; set; }
        public decimal? TotalAmout { get; set; }
        public int? SponsorId { get; set; }
        public List<ItensOrderMercadoPago>? itensOrderMercadoPagos { get; set; }
    }
}
