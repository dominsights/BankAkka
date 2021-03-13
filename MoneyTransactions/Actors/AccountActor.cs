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

            Receive<TransferMoney>(msg =>
            {
                account.Withdraw(msg.Amount);
                msg.DestinationActor.Tell(new Deposit(msg.Amount, account.Id, account.Client.FullName));
            });

            Receive<CheckBalance>(msg =>
            {
                Sender.Tell(new BalanceStatus(account.Balance));
            });
        }

        public Account Account { get; }
    }
}
