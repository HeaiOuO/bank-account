using System.Transactions;
using Microsoft.EntityFrameworkCore;
using BankAccount.Models;

namespace BankAccount.Models
{
    public class logRegContext : DbContext
    {
        public logRegContext(DbContextOptions<logRegContext> options) : base(options) {}

        public DbSet<User> users {get; set;}
        public DbSet<Transaction> transactions {get;set;}
    }
}