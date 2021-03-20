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
        public void Deposit_should_succeed_when_requested_with_correct_values()
        {
            var accountId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            decimal balance = 100m;
            var client = new Client(clientId, "Jonh", "Doe");
            var account = new Account(accountId, balance, client);

            var subject = Sys.ActorOf(Props.Create(() => new AccountActor(account)));
            
            decimal amount = 50m;
            subject.Tell(new Deposit(amount));
            ExpectMsg<DepositResult>(msg => Assert.Equal(Result.Success, msg.Result));
            
            subject.Tell(new CheckBalance());
            ExpectMsg<BalanceStatus>(msg => Assert.Equal(balance + amount, msg.Balance ));
        }

        [Fact]
        public void Withdraw_should_succeed_when_requested_with_correct_values()
        {
            var accountId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            decimal balance = 100m;
            var client = new Client(clientId, "Jonh", "Doe");
            var account = new Account(accountId, balance, client);

            var subject = Sys.ActorOf(Props.Create(() => new AccountActor(account)));

            decimal amount = 50m;
            subject.Tell(new Withdraw(amount));
            ExpectMsg<WithdrawResult>(msg => Assert.True(msg.Result == Result.Success));

            subject.Tell(new CheckBalance());
            ExpectMsg<BalanceStatus>(msg => Assert.Equal(balance - amount, msg.Balance));
        }

        [Fact]
        public void Withdraw_followed_by_deposit_should_work()
        {
            var accountId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            decimal balance = 100m;
            var client = new Client(clientId, "Jonh", "Doe");
            var account = new Account(accountId, balance, client);

            var subject = Sys.ActorOf(Props.Create(() => new AccountActor(account)));

            decimal withdraw = 50m;
            subject.Tell(new Withdraw(withdraw));
            ExpectMsg<WithdrawResult>(msg => Assert.True(msg.Result == Result.Success));

            subject.Tell(new CheckBalance());
            ExpectMsg<BalanceStatus>(msg =>
            {
                decimal expected = balance - withdraw;
                Assert.Equal(expected, msg.Balance);
            });

            decimal deposit = 50m;
            subject.Tell(new Deposit(deposit));
            ExpectMsg<DepositResult>(msg => Assert.Equal(Result.Success, msg.Result));

            subject.Tell(new CheckBalance());
            ExpectMsg<BalanceStatus>(msg =>
            {
                decimal expected = balance + deposit - withdraw;
                Assert.Equal(expected, msg.Balance);
            });
        }
    }
}
