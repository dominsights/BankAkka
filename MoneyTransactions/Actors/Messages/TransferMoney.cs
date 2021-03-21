using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors.Messages
{
    public class TransferMoney
    {
        public TransferMoney(decimal amount, IActorRef source, IActorRef destination)
        {
            Amount = amount;
            Destination = destination;
            Source = source;
        }

        public decimal Amount { get; }
        public IActorRef Destination { get; }
        public IActorRef Source { get; }
    }
}
