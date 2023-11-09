using CustomerService.Models;

namespace CustomerService.Data
{
    public interface ICustomerRepository
    {
        bool SaveChanges();

        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);
        void CreateCustomer(Customer customer);
    }
}
