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
    public class BankActorTest : TestKit
    {
        [Fact]
        public void Should_create_new_account()
        {
            var client = new Client(Guid.NewGuid(), "John", "Doe");

            var subject = Sys.ActorOf(Props.Create(() => new BankActor()), "bank");
            subject.Tell(new CreateAccount(client));

            var result = ExpectMsg<CreateAccountResult>();

            var selection = Sys.ActorSelection("/user/bank/" + result.Account.Id);

            selection.Tell(new CheckBalance());

            ExpectMsg<BalanceStatus>();

            Assert.Equal(MoneyTransactions.Status.Success, result.Status);
            Assert.NotNull(result.Account);
        }
    }
}
