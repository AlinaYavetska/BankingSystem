using TransactionsService.Models;
using TransactionsService.SyncDataServices.Grps;

namespace TransactionsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<ICustomerDataClient>();

                var customers = grpcClient.ReturnAllCustomers();

                SeedData(serviceScope.ServiceProvider.GetService<ITransactionRepository>(), customers);
            }
        }

        private static void SeedData(ITransactionRepository repo, IEnumerable<Customer> customers)
        {
            Console.WriteLine("Seeding new customers...");

            foreach (var customer in customers)
            {
                if (!repo.ExternalCustomerExists(customer.ExternalId))
                {
                    repo.CreateCustomer(customer);
                }
                repo.SaveChanges();
            }
        }
    }
}
