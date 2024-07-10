namespace Application.Contracts.Request.RequestOrder
{
    public class PaymentRequest
    {        
        public class OrderDto
        {
            public OrderDto()
            {
                cash_out = new CashOutDto();
                items = new List<Item>();
            }
            public CashOutDto cash_out { get; set; }
            public string description { get; set; }
            public string external_reference { get; set; }
            public List<Item> items { get; set; }
            public string notification_url { get; set; }
            public string title { get; set; }
            public decimal total_amount { get; set; }

            public class CashOutDto
            {
                public decimal amount { get; set; }
            }

            public class Item
            {
                public string sku_number { get; set; }
                public string category { get; set; }
                public string title { get; set; }
                public string description { get; set; }
                public decimal unit_price { get; set; }
                public int quantity { get; set; }
                public string unit_measure { get; set; }
                public decimal total_amount { get; set; }
            }
        }
    }
}
