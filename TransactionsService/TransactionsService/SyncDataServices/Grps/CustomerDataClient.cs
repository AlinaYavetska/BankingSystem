using AutoMapper;
using CustomerService;
using Grpc.Net.Client;
using TransactionsService.Models;

namespace TransactionsService.SyncDataServices.Grps
{
    public class CustomerDataClient : ICustomerDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CustomerDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<Customer> ReturnAllCustomers()
        {
            Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcCustomer"]}");
            var channel = GrpcChannel.ForAddress(_configuration["GrpcCustomer"]);
            var client = new GrpcCustomer.GrpcCustomerClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllCustomers(request);
                return _mapper.Map<IEnumerable<Customer>>(reply.Customer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not call GRPC Server {ex.Message}");
                return null;
            }
        }
    }
}
