namespace WebApplication1.Models

{
    public class Subscription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int RenewalPeriod { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}