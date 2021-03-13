using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors.Messages
{
    public class TransferSucceeded
    {
        public decimal NewBalance { get; set; }
    }
}
