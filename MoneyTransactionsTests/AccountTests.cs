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
            var user = new User(userId, "Jonh", "Doe");
            var account = new Account(accountId, balance, user);

            Assert.Equal(accountId, account.Id);
            Assert.Equal(balance, account.Balance);
            Assert.Equal(user, account.User);
        }
    }
}
