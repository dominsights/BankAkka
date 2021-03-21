using Akka.Actor;
using Akka.TestKit.Xunit2;
using MoneyTransactions;
using MoneyTransactions.Actors;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MoneyTransactionsTests.Actors
{
    public class BankActorTest : TestKit
    {
        [Fact]
        public void Should_create_new_account()
        {
            var client = new Client(Guid.NewGuid(), "John", "Doe");

            var subject = Sys.ActorOf(Props.Create(() => new BankActor()));
            subject.Tell(new CreateAccount(client));

            var result = ExpectMsg<CreateAccountResult>();

            Assert.Equal(MoneyTransactions.Status.Success, result.Status);
            Assert.NotNull(result.Account);
        }
    }
}
