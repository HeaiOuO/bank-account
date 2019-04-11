using System;
using System.Collections.Generic;

namespace BankAccount.Models
{
    public class Info
    {
        public Decimal sum { get; set; }
        public List<Transaction> tran { get; set; }
        public string Firstname{ get; set; }
        public Transaction form {get;set;}
    }

}