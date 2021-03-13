using Akka.Actor;
using Akka.TestKit.Xunit2;
using MoneyTransactions;
using MoneyTransactions.Actors;
using MoneyTransactions.Actors.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MoneyTransactionsTests.Actors
{
    public class AccountActorTests : TestKit
    {
        [Fact]
        public void Should_create_actor()
        {
            var accountId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            decimal balance = 100m;
            var client = new Client(clientId, "Jonh", "Doe");
            var account = new Account(accountId, balance, client);

            var subject = Sys.ActorOf(Props.Create(() => new AccountActor(account)));

            decimal amount = 50m;
            var destinationAccount = new Account(Guid.NewGuid(), 100m, new Client(Guid.NewGuid(), "Jane", "Doe"));
            var destinationActor = Sys.ActorOf(Props.Create(() => new AccountActor(destinationAccount)));
            subject.Tell(new TransferMoney(amount, destinationActor));

            ExpectMsg<TransferSucceeded>();
        }
    }
}
