using TransactionsService.Models;

namespace TransactionsService.SyncDataServices.Grps
{
    public interface ICustomerDataClient
    {
        IEnumerable<Customer> ReturnAllCustomers();
    }
}
