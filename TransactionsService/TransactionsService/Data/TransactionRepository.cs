using System;
using System.ComponentModel.Design;
using TransactionsService.Models;

namespace TransactionsService.Data
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }
            _context.Customers.Add(customer);
        }

        public void CreateTransaction(int customerId, Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            transaction.CustomerId = customerId;
            _context.Transactions.Add(transaction);
        }

        public bool CustomerExits(int customerId)
        {
            return _context.Customers.Any(p => p.Id == customerId);
        }

        public bool ExternalCustomerExists(int externalCustomerId)
        {
            return _context.Customers.Any(p => p.ExternalId == externalCustomerId);
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public Transaction GetTransaction(int customerId, int transactionId)
        {
            return _context.Transactions
                .Where(c => c.CustomerId == customerId && c.Id == transactionId).FirstOrDefault();
        }

        public IEnumerable<Transaction> GetTransactionsForCustomer(int customerId)
        {
            return _context.Transactions
                .Where(c => c.CustomerId == customerId)
                .OrderBy(c => c.Customer.Name);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
