namespace MoneyTransactions
{

    public class WithdrawResult
    {
        public Result Result { get; }

        public WithdrawResult(Result result)
        {
            Result = result;
        }
    }
}