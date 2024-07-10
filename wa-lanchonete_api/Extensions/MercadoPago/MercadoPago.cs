using Microsoft.Extensions.WebEncoders.Testing;
using MercadoPago.Client;
using MercadoPago.Config;
using MercadoPago.Http;
using MercadoPago.Serialization;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using System.Text.Json;
using Domain.Entities;
using System.Drawing.Printing;
namespace wa_lanchonete_api.Extensions.MercadoPago
{
    public class MercadoPago
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string UrlPost { get; set; }
        public string UrlNotification { get; set; }

        public string Token { get; set; }

        public List<Product> Itens { get; set; }
        public double TotalAmout { get; set; }
        public int SponsorId { get; set; }

    }
}
