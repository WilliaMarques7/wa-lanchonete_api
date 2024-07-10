using System.Text.Json.Serialization;

namespace Application.Contracts.Response.ResponseOrder
{
    public class QRCodeResponse
    {
        [JsonPropertyName("in_store_order_id")]
        public string? InStoreOrderId { get; set; }

        [JsonPropertyName("qr_data")]
        public string? QrData { get; set; }
    }
}
