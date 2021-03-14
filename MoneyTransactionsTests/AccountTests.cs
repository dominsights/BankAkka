using MoneyTransactions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MoneyTransactionsTests
{
    public class AccountTests
    {
        [Fact]
        public void Should_create_account()
        {
            var accountId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            decimal balance = 100m;
            var user = new Client(userId, "Jonh", "Doe");
            var account = new Account(accountId, balance, user);

            Assert.Equal(accountId, account.Id);
            Assert.Equal(balance, account.Balance);
            Assert.Equal(user, account.Client);
        }

        [Fact]
        public void Should_withdraw_correct_amount_when_balance_is_enough()
        {
            var accountId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            decimal balance = 100m;
            var user = new Client(userId, "Jonh", "Doe");
            var account = new Account(accountId, balance, user);

            decimal amount = 50m;
            account.Withdraw(amount);

            Assert.Equal(balance - amount, account.Balance);
        }

        [Fact]
        public void Should_not_withdraw_amount_when_balance_is_not_enough()
        {
            var accountId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            decimal balance = 10m;
            var user = new Client(userId, "Jonh", "Doe");
            var account = new Account(accountId, balance, user);

            decimal amount = 50m;
            Assert.Throws<InvalidOperationException>(() => account.Withdraw(amount));
            Assert.Equal(balance, account.Balance);
        }

        [Fact]
        public void Should_deposit_correct_amount_when_positive_value()
        {
            var accountId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            decimal balance = 100m;
            var user = new Client(userId, "Jonh", "Doe");
            var account = new Account(accountId, balance, user);

            decimal amount = 50m;
            account.Deposit(amount);

            Assert.Equal(balance + amount, account.Balance);
        }

        [Fact]
        public void Should_not_deposit_when_negative_value()
        {
            var accountId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            decimal balance = 100m;
            var user = new Client(userId, "Jonh", "Doe");
            var account = new Account(accountId, balance, user);

            decimal amount = -50m;
            Assert.Throws<InvalidOperationException>(() => account.Deposit(amount));
        }

        [Fact]
        public void Should_not_deposit_when_zero()
        {
            var accountId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            decimal balance = 100m;
            var user = new Client(userId, "Jonh", "Doe");
            var account = new Account(accountId, balance, user);

            decimal amount = 0m;
            Assert.Throws<InvalidOperationException>(() => account.Deposit(amount));
        }
    }
}
