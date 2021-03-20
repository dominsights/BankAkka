using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors
{
    #region Messages

    public class TransferConfirmed
    {
    }

    public class TransferMoney
    {
        public TransferMoney(decimal amount, IActorRef source, IActorRef destination)
        {
            Amount = amount;
            Destination = destination;
            Source = source;
        }

        public decimal Amount { get; }
        public IActorRef Destination { get; }
        public IActorRef Source { get; }
    }

    public class TransferFailed
    {
    }

    #endregion

    public class TransferActor : ReceiveActor
    {
        private IActorRef _sender;
        private TransferMoney _transferMoney;

        public TransferActor()
        {
            Become(Ready);
        }

        private void Ready()
        {
            Receive<TransferMoney>(msg =>
            {
                _sender = Sender;
                _transferMoney = msg;

                Become(ExecutingTransfer);
                msg.Source.Tell(new AccountActor.Withdraw(msg.Amount));
            });
        }

        private void ExecutingTransfer()
        {
            Receive<AccountActor.WithdrawCompleted>(msg =>
            {
                _transferMoney.Destination.Tell(new AccountActor.Deposit(_transferMoney.Amount));
            });

            Receive<AccountActor.WithdrawFailed>(msg =>
            {
                _sender.Tell(new TransferFailed());
            });

            Receive<AccountActor.DepositConfirmed>(msg =>
            {
                _sender.Tell(new TransferConfirmed());
                Become(Ready);
            });
        }
    }
}
