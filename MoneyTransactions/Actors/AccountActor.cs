using Akka.Actor;
using MoneyTransactions.Actors.Messages;
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
                Sender.Tell(new DepositConfirmed());
            });

            Receive<Withdraw>(msg =>
            {
                try
                {
                    Account.Withdraw(msg.Amount);
                    Sender.Tell(new WithdrawCompleted());
                }
                catch
                {
                    Sender.Tell(new WithdrawFailed());
                }
            });
        }

        public Account Account { get; }
    }
}
