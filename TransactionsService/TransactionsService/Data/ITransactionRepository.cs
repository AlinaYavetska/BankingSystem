using TransactionsService.Models;

namespace TransactionsService.Data
{
    public interface ITransactionRepository
    {
        bool SaveChanges();

        // Customers
        IEnumerable<Customer> GetAllCustomers();
        void CreateCustomer(Customer customer);
        bool CustomerExits(int customerId);
        bool ExternalCustomerExists(int externalCustomerId);

        // Transactions
        IEnumerable<Transaction> GetTransactionsForCustomer(int customerId);
        Transaction GetTransaction(int customerId, int transactionId);
        void CreateTransaction(int customerId, Transaction transaction);
    }
}
