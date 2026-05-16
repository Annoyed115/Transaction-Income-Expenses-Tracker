static class TransactionService
{
    public static (decimal Income, decimal Expenses, decimal Total) CalculateBalance(List<Transaction> transactions)
    {
        decimal income = transactions
            .Where(transaction => transaction.Type == TransactionType.Income)
            .Sum(transaction => transaction.Amount);

        decimal expenses = transactions
            .Where(transaction => transaction.Type == TransactionType.Expense)
            .Sum(transaction => transaction.Amount);

        return (income, expenses, income - expenses);
    }

    public static int GetNextTransactionId(List<Transaction> transactions)
    {
        if (transactions.Count == 0)
        {
            return 1;
        }

        return transactions.Max(transaction => transaction.Id) + 1;
    }
}
