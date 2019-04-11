using System.Data.Common;
using System.Transactions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BankAccount.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId {get; set; }
        [Required]
        public decimal Amount {get; set; }
        [Required]
        public DateTime CreatedAt {get; set; } = DateTime.Now;
        
        [Required]
        public int UserId {get ; set; }
        public User User {get; set; }
    }

}