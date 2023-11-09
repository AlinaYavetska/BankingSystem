using AutoMapper;
using CustomerService.Data;
using Grpc.Core;

namespace CustomerService.SyncDataServices.Grps
{
    public class GrpcCustomerService : GrpcCustomer.GrpcCustomerBase
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public GrpcCustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override Task<CustomerResponse> GetAllCustomers(GetAllRequest request, ServerCallContext context)
        {
            var response = new CustomerResponse();
            var customers = _repository.GetAllCustomers();

            foreach (var customer in customers)
            {
                response.Customer.Add(_mapper.Map<GrpcCustomerModel>(customer));
            }

            return Task.FromResult(response);
        }
    }
}
