using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors.Messages
{
    public class BalanceStatus
    {
        public BalanceStatus(decimal balance)
        {
            Balance = balance;
        }

        public decimal Balance { get; set; }
    }
}
