using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors.Messages
{
    public class TransferMoney
    {
        public TransferMoney(decimal amount, IActorRef destinationActor)
        {
            Amount = amount;
            DestinationActor = destinationActor;
        }

        public decimal Amount { get; }
        public IActorRef DestinationActor { get; }
    }
}
