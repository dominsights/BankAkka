using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Actors.Messages
{
    public class TransferResult
    {
        public TransferResult(Result result)
        {
            Result = result;
        }

        public Result Result { get; }
    }
}
