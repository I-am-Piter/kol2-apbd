using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Services

{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;

        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ClientDto> GetClientWithSubscriptionsAsync(int clientId)
        {
            var client = await _context.Clients
                .Include(c => c.Subscriptions)
                .ThenInclude(s => s.Payments)
                .Where(c => c.Id == clientId)
                .FirstOrDefaultAsync();

            if (client == null)
                return null;

            return new ClientDto
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                Phone = client.Phone,
                Subscriptions = client.Subscriptions.Select(s => new SubscriptionDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    TotalPaidAmount = s.Payments.Sum(p => p.Amount)
                }).ToList()
            };
        }

        Task<int> IClientService.AddPaymentAsync(PaymentDto paymentDto)
        {
            return AddPaymentAsync(paymentDto);
        }

        public async Task<int> AddPaymentAsync(PaymentDto paymentDto)
        {
            var client = await _context.Clients.FindAsync(paymentDto.IdClient);
            if (client == null)
                throw new Exception("Client not found");

            var subscription = await _context.Subscriptions.FindAsync(paymentDto.IdSubscription);
            if (subscription == null)
                throw new Exception("Subscription not found");

            if (subscription.EndDate.HasValue && subscription.EndDate < DateTime.Now)
                throw new Exception("Subscription not active");

            var renewalPeriodEndDate = subscription.CreatedAt.AddMonths(subscription.RenewalPeriod);
            var existingPayment = await _context.Payments
                .Where(p => p.ClientId == paymentDto.IdClient && p.SubscriptionId == paymentDto.IdSubscription && p.PaymentDate >= subscription.CreatedAt && p.PaymentDate <= renewalPeriodEndDate)
                .FirstOrDefaultAsync();

            if (existingPayment != null)
                throw new Exception("Payment for this period already exists");

            var activeDiscount = await _context.Discounts
                .Where(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
                .OrderByDescending(d => d.Value)
                .FirstOrDefaultAsync();

            var paymentAmount = paymentDto.Payment;
            if (activeDiscount != null)
            {
                paymentAmount -= (paymentAmount * activeDiscount.Value / 100);
            }

            if (paymentAmount != subscription.Price)
                throw new Exception("Invalid payment amount");

            var payment = new Payment
            {
                ClientId = paymentDto.IdClient,
                SubscriptionId = paymentDto.IdSubscription,
                Amount = paymentAmount,
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment.Id;
        }

        Task<ClientDto> IClientService.GetClientWithSubscriptionsAsync(int clientId)
        {
            return GetClientWithSubscriptionsAsync(clientId);
        }
    }
}