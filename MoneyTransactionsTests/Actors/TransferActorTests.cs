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
    public class TransferActorTests : TestKit
    {

        [Fact]
        public void Should_transfer_correct_amount_when_balance_is_enough()
        {
            var accountId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            decimal balance = 100m;
            var client = new Client(clientId, "Jonh", "Doe");
            var account = new Account(accountId, balance, client);

            var subject = Sys.ActorOf(Props.Create(() => new TransferActor()));

            decimal amountToTransfer = 50m;

            var sourceAccountActor = Sys.ActorOf(Props.Create(() => new AccountActor(account)));

            var destinationAccount = new Account(Guid.NewGuid(), balance, new Client(Guid.NewGuid(), "Jane", "Doe"));
            var destinationActor = Sys.ActorOf(Props.Create(() => new AccountActor(destinationAccount)));
            subject.Tell(new TransferMoney(amountToTransfer, sourceAccountActor, destinationActor));

            ExpectMsg<TransferResult>(msg => Assert.True(msg.Result == Result.Success));

            sourceAccountActor.Tell(new CheckBalance());
            ExpectMsg<BalanceStatus>(msg => Assert.Equal(balance - amountToTransfer, msg.Balance));

            destinationActor.Tell(new CheckBalance());
            var currentBalance = ExpectMsg<BalanceStatus>();
            Assert.Equal(balance + amountToTransfer, currentBalance.Balance);
        }


        [Fact]
        public void Transfer_should_fail_when_not_enough_balance()
        {
            var accountId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            decimal balance = 40m;
            var client = new Client(clientId, "Jonh", "Doe");
            var account = new Account(accountId, balance, client);

            var subject = Sys.ActorOf(Props.Create(() => new TransferActor()));

            decimal amountToTransfer = 50m;

            var sourceAccountActor = Sys.ActorOf(Props.Create(() => new AccountActor(account)));

            var destinationAccount = new Account(Guid.NewGuid(), balance, new Client(Guid.NewGuid(), "Jane", "Doe"));
            var destinationActor = Sys.ActorOf(Props.Create(() => new AccountActor(destinationAccount)));
            subject.Tell(new TransferMoney(amountToTransfer, sourceAccountActor, destinationActor));

            ExpectMsg<TransferResult>(msg => Assert.True(msg.Result == Result.Error));

            sourceAccountActor.Tell(new CheckBalance());
            ExpectMsg<BalanceStatus>(msg => Assert.Equal(balance, msg.Balance));

            destinationActor.Tell(new CheckBalance());
            var currentBalance = ExpectMsg<BalanceStatus>();
            Assert.Equal(balance, currentBalance.Balance);
        }
    }
}
