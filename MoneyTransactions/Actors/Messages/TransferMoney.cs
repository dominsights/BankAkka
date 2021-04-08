using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors.Messages
{
    public class TransferMoney
    {
        public TransferMoney(decimal amount, Account source, Account destination)
        {
            Amount = amount;
            Destination = destination;
            Source = source;
        }

        public decimal Amount { get; }
        public Account Destination { get; }
        public Account Source { get; }
    }
}
