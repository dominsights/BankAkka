using Akka.Actor;
using MoneyTransactions.Actors.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors
{
    public class AccountActor : ReceiveActor, IWithUnboundedStash
    {
        private IActorRef _sender;

        public AccountActor(Account account)
        {
            Account = account;

            Become(Ready);
        }

        private void Ready()
        {
            Receive<TransferMoney>(msg =>
            {
                Account.Withdraw(msg.Amount);
                msg.DestinationActor.Tell(new Deposit(msg.Amount, Account.Id, Account.Client.FullName));
                _sender = Sender;
                Become(WaitingForConfirmation);
            });

            ReceiveCommonMessages();
        }

        private void WaitingForConfirmation()
        {
            ReceiveCommonMessages();

            Receive<TransferMoney>(msg =>
            {
                Stash.Stash();
            });

            Receive<DepositConfirmed>(msg =>
            {
                _sender.Tell(new TransferSucceeded(Account.Balance));
                Become(Ready);
                Stash.UnstashAll();
            });
        }

        private void ReceiveCommonMessages()
        {
            Receive<CheckBalance>(msg =>
            {
                Sender.Tell(new BalanceStatus(Account.Balance));
            });

            Receive<Deposit>(msg =>
            {
                Account.Deposit(msg.Amount);
                Sender.Tell(new DepositConfirmed());
            });
        }

        public Account Account { get; }
        public IStash Stash { get; set; }
    }
}
