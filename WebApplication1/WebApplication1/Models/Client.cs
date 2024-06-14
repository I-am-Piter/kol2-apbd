namespace WebApplication1.Models

{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
    }
}