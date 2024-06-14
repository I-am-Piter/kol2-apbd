using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientWithSubscriptions(int id)
        {
            var client = await _clientService.GetClientWithSubscriptionsAsync(id);
            if (client == null)
                return NotFound();

            return Ok(client);
        }

        [HttpPost("add-payment")]
        public async Task<IActionResult> AddPayment([FromBody] PaymentDto paymentDto)
        {
            try
            {
                var paymentId = await _clientService.AddPaymentAsync(paymentDto);
                return Ok(new { Id = paymentId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}