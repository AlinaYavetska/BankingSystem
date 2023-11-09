using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionsService.Data;
using TransactionsService.Dtos;

namespace TransactionsService.Controllers
{
    [Route("api/t/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;

        public CustomersController(ITransactionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CustomerReadDto>> GetCustomers()
        {
            Console.WriteLine("--> Getting Customers from TransactionsService");

            var customerItems = _repository.GetAllCustomers();

            return Ok(_mapper.Map<IEnumerable<CustomerReadDto>>(customerItems));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Transactions Service");

            return Ok("Inbound test of from Customers Controller");
        }
    }
}
