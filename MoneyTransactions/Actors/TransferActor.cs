using Akka.Actor;
using MoneyTransactions.Actors.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors
{
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
                msg.Source.Tell(new Withdraw(msg.Amount));
            });
        }

        private void ExecutingTransfer()
        {
            Receive<WithdrawCompleted>(msg =>
            {
                _transferMoney.Destination.Tell(new Deposit(_transferMoney.Amount));
            });

            Receive<WithdrawFailed>(msg =>
            {
                _sender.Tell(new TransferFailed());
            });

            Receive<DepositConfirmed>(msg =>
            {
                _sender.Tell(new TransferConfirmed());
                Become(Ready);
            });
        }
    }
}
