using System.Collections.Generic;

namespace MoneyTransactions
{
    public class CreateAccountResult
    {
        public Status Status { get; private set; }
        public Account Account { get; private set; }

        public CreateAccountResult(Account account, Status status)
        {
            Status = status;
            Account = account;
        }
    }
}