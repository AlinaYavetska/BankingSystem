using CustomerService.Dtos;

namespace CustomerService.SyncDataServices.Http
{
    public interface ITransactionDataClient
    {
        Task SendCustomerToTransaction(CustomerReadDto customer);
    }
}
