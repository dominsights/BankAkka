using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors.Messages
{
    public class Deposit
    {
        public Deposit(decimal amount)
        {
            Amount = amount;
        }

        public decimal Amount { get; }
    }
}
