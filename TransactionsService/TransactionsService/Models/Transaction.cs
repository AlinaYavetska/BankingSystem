using System.ComponentModel.DataAnnotations;

namespace TransactionsService.Models
{
    public class Transaction
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string TransactionDetails { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }
    }
}
