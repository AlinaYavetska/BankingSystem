using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionsService.Data;
using TransactionsService.Dtos;
using TransactionsService.Models;

namespace TransactionsService.Controllers
{
    [Route("api/t/customers/{customerId}/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;

        public TransactionsController(ITransactionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TransactionReadDto>> GetTransactionsForCustomer(int customerId)
        {
            Console.WriteLine($"--> Hit GetTransactionsForCustomer: {customerId}");

            if (!_repository.CustomerExits(customerId))
            {
                return NotFound();
            }

            var transactions = _repository.GetTransactionsForCustomer(customerId);

            return Ok(_mapper.Map<IEnumerable<TransactionReadDto>>(transactions));
        }

        [HttpGet("{transactionId}", Name = "GetTransactionForCustomer")]
        public ActionResult<TransactionReadDto> GetTransactionForCustomer(int customerId, int transactionId)
        {
            Console.WriteLine($"--> Hit GetTransactionForCustomer: {customerId} / {transactionId}");

            if (!_repository.CustomerExits(customerId))
            {
                return NotFound();
            }

            var transaction = _repository.GetTransaction(customerId, transactionId);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TransactionReadDto>(transaction));
        }

        [HttpPost]
        public ActionResult<TransactionReadDto> CreateTransactionForCustomer(int customerId, TransactionCreateDto transactionDto)
        {
            Console.WriteLine($"--> Hit CreateTransactionForCustomer: {customerId}");

            if (!_repository.CustomerExits(customerId))
            {
                return NotFound();
            }

            var transaction = _mapper.Map<Transaction>(transactionDto);

            _repository.CreateTransaction(customerId, transaction);
            _repository.SaveChanges();

            var transactionReadDto = _mapper.Map<TransactionReadDto>(transaction);

            return CreatedAtRoute(nameof(GetTransactionForCustomer),
                new { customerId = customerId, transactionId = transactionReadDto.Id }, transactionReadDto);
        }
    }
}
