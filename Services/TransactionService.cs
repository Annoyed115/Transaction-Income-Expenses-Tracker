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

    public static List<Transaction> GetByCategory(List<Transaction> transactions, TransactionCategory category)
    {
        return transactions
            .Where(transaction => transaction.Category == category)
            .ToList();
    }

    public static List<Transaction> GetByMonth(List<Transaction> transactions, int year, int month)
    {
        return transactions
            .Where(transaction => transaction.Date.Year == year && transaction.Date.Month == month)
            .ToList();
    }

    public static Transaction? FindById(List<Transaction> transactions, int id)
    {
        return transactions.FirstOrDefault(transaction => transaction.Id == id);
    }

    public static int FindIndexById(List<Transaction> transactions, int id)
    {
        return transactions.FindIndex(transaction => transaction.Id == id);
    }
}
