namespace WebApplication1.Dtos
{
    public class PaymentDto
    {
        public int IdClient { get; set; }
        public int IdSubscription { get; set; }
        public decimal Payment { get; set; }
    }
}