using AutoMapper;
using CustomerService.AsyncDataServices;
using CustomerService.Data;
using CustomerService.Dtos;
using CustomerService.Models;
using CustomerService.SyncDataServices.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        private readonly ITransactionDataClient _transactionDataClient;
        private readonly IMessageBusClient _messageBusClient;


        public CustomersController(
            ICustomerRepository repository,
            IMapper mapper,
            ITransactionDataClient transactionDataClient,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _transactionDataClient = transactionDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CustomerReadDto>> GetCustomers()
        {
            Console.WriteLine("--> Getting Customers....");

            var customerItems = _repository.GetAllCustomers();

            return Ok(_mapper.Map<IEnumerable<CustomerReadDto>>(customerItems));
        }

        [HttpGet("{id}", Name = "GetCustomerById")]
        public ActionResult<CustomerReadDto> GetCustomerById(int id)
        {
            var customerItem = _repository.GetCustomerById(id);
            if (customerItem != null)
            {
                return Ok(_mapper.Map<CustomerReadDto>(customerItem));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<CustomerReadDto>> CreateCustomer(CustomerCreateDto customerCreateDto)
        {
            var customerModel = _mapper.Map<Customer>(customerCreateDto);
            _repository.CreateCustomer(customerModel);
            _repository.SaveChanges();

            var customerReadDto = _mapper.Map<CustomerReadDto>(customerModel);

            // Send Sync Message
            try
            {
                await _transactionDataClient.SendCustomerToTransaction(customerReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            //Send Async Message
            try
            {
                var customerPublishedDto = _mapper.Map<CustomerPublishedDto>(customerReadDto);
                customerPublishedDto.Event = "Customer_Published";
                _messageBusClient.PublishNewCustomer(customerPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetCustomerById), new { Id = customerReadDto.Id }, customerReadDto);

        }
    }
}
