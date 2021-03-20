using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors
{
    public class AccountActor : ReceiveActor
    {
        #region Messages

        public class Deposit
        {
            public Deposit(decimal amount)
            {
                Amount = amount;
            }

            public decimal Amount { get; }
        }

        public class DepositConfirmed
        {
        }

        public class Withdraw
        {
            public decimal Amount { get; }

            public Withdraw(decimal amount)
            {
                this.Amount = amount;
            }
        }

        public class WithdrawFailed
        {
        }

        public class WithdrawCompleted
        {
        }

        public class CheckBalance
        {
        }

        public class BalanceStatus
        {
            public BalanceStatus(decimal balance)
            {
                Balance = balance;
            }

            public decimal Balance { get; set; }
        }

        #endregion

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
