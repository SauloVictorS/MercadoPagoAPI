namespace MercadoPagoAPI.Models.Order
{
    public class RefaundRequest
    {
        public string OrderId { get; set; }
        public long PixId { get; set; }
        public decimal? Amount { get; set; }

    }
}
