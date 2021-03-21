using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyTransactions.Foundation
{
    public class Result<T>
    {
        public Status Status { get; }

        public Result(Status status)
        {
            Status = status;
        }
    }
}
