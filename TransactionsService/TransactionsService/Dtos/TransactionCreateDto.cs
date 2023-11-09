using System.ComponentModel.DataAnnotations;

namespace TransactionsService.Dtos
{
    public class TransactionCreateDto
    {
        [Required]
        public string TransactionDetails { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
