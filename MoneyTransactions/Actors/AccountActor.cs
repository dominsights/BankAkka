using Akka.Actor;
using MoneyTransactions.Actors.Messages;
using MoneyTransactions.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors
{
    public class AccountActor : ReceiveActor
    {
        public AccountActor(Account account)
        {
            Account = account;

            Receive<CheckBalance>(msg =>
            {
                Sender.Tell(new BalanceStatus(Account.Balance));
            });

            Receive<Deposit>(msg =>
            {
                Account.Deposit(msg.Amount);
                Sender.Tell(new Result<Deposit>(Status.Success));
            });

            Receive<Withdraw>(msg =>
            {
                var result = Account.Withdraw(msg.Amount);
                Sender.Tell(new Result<Withdraw>(result));
            });
        }

        public Account Account { get; }
    }
}
