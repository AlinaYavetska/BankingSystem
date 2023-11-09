using System.Text.Json;
using System.Text;
using CustomerService.Dtos;

namespace CustomerService.SyncDataServices.Http
{
    public class HttpTransactionDataClient : ITransactionDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpTransactionDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }


        public async Task SendCustomerToTransaction(CustomerReadDto customer)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(customer),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["TransactionService"]}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to TransactionService was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to TransactionService was NOT OK!");
            }
        }
    }
}
