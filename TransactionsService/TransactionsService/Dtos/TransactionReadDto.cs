using System.ComponentModel.DataAnnotations;

namespace TransactionsService.Dtos
{
    public class TransactionReadDto
    {
        public int Id { get; set; }

        public string TransactionDetails { get; set; }

        public decimal Amount { get; set; }

        public int CustomerId { get; set; }
    }
}
