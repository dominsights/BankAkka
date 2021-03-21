using Akka.Actor;
using Akka.TestKit.Xunit2;
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
            var subject = Sys.ActorOf(Props.Create(() => new BankActor()));
        }
    }
}
