using WebApplication1.Dtos;

namespace WebApplication1.Services
{
    public interface IClientService
    {
        Task<ClientDto> GetClientWithSubscriptionsAsync(int clientId);
        Task<int> AddPaymentAsync(PaymentDto paymentDto);
    }
}