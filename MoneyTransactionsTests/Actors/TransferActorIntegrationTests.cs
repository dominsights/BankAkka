using Akka.Actor;
using Akka.TestKit.Xunit2;
using MoneyTransactions;
using MoneyTransactions.Actors;
using MoneyTransactions.Actors.Messages;
using MoneyTransactions.Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MoneyTransactionsTests.Actors
{
    public class TransferActorIntegrationTests : TestKit
    {
        [Fact]
        public void Should_transfer_correct_amount_when_balance_is_enough()
        {
            var client1 = new Client(Guid.NewGuid(), "John", "Doe");
            var client2 = new Client(Guid.NewGuid(), "Jane", "Doe");

            var bank = Sys.ActorOf(Props.Create(() => new BankActor()), "bank");
            bank.Tell(new CreateAccount(client1));
            var resultClient1 = ExpectMsg<CreateAccountResult>();
            bank.Tell(new CreateAccount(client2));
            var resultClient2 = ExpectMsg<CreateAccountResult>();

            var accountClient1 = Sys.ActorSelection("/user/bank/" + resultClient1.Account.Id);
            var accountClient2 = Sys.ActorSelection("/user/bank/" + resultClient2.Account.Id);

            accountClient1.Tell(new Deposit(100m));
            accountClient2.Tell(new Deposit(50m));

            ExpectMsg<Result<Deposit>>();
            ExpectMsg<Result<Deposit>>();

            var transferActor = Sys.ActorOf(Props.Create(() => new TransferActor()));
            var transfer = new TransferMoney(60m, resultClient1.Account, resultClient2.Account);
            transferActor.Tell(transfer);
            
            var transferResult = ExpectMsg<Result<TransferMoney>>();
            Assert.Equal(MoneyTransactions.Status.Success, transferResult.Status);

            accountClient1.Tell(new CheckBalance());
            ExpectMsg<BalanceStatus>(msg => Assert.Equal(40m, msg.Balance));

            accountClient2.Tell(new CheckBalance());
            ExpectMsg<BalanceStatus>(msg => Assert.Equal(110m, msg.Balance));
        }

        [Fact]
        public void Transfer_should_fail_when_not_enough_balance()
        {
            var client1 = new Client(Guid.NewGuid(), "John", "Doe");
            var client2 = new Client(Guid.NewGuid(), "Jane", "Doe");

            var bank = Sys.ActorOf(Props.Create(() => new BankActor()), "bank");
            bank.Tell(new CreateAccount(client1));
            var resultClient1 = ExpectMsg<CreateAccountResult>();
            bank.Tell(new CreateAccount(client2));
            var resultClient2 = ExpectMsg<CreateAccountResult>();

            var accountClient1 = Sys.ActorSelection("/user/bank/" + resultClient1.Account.Id);
            var accountClient2 = Sys.ActorSelection("/user/bank/" + resultClient2.Account.Id);

            accountClient1.Tell(new Deposit(50m));
            accountClient2.Tell(new Deposit(50m));

            ExpectMsg<Result<Deposit>>();
            ExpectMsg<Result<Deposit>>();

            var transferActor = Sys.ActorOf(Props.Create(() => new TransferActor()));
            var transfer = new TransferMoney(60m, resultClient1.Account, resultClient2.Account);
            transferActor.Tell(transfer);

            var transferResult = ExpectMsg<Result<TransferMoney>>();
            Assert.Equal(MoneyTransactions.Status.Error, transferResult.Status);

            accountClient1.Tell(new CheckBalance());
            ExpectMsg<BalanceStatus>(msg => Assert.Equal(50m, msg.Balance));

            accountClient2.Tell(new CheckBalance());
            ExpectMsg<BalanceStatus>(msg => Assert.Equal(50m, msg.Balance));
        }
    }
}
