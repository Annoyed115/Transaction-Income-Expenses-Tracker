List<Transaction> transactions = [];

while (true)
{
    ClearScreen();
    Console.WriteLine("Transaction Tracker");
    Console.WriteLine("-------------------");
    Console.WriteLine("1. Add income");
    Console.WriteLine("2. Add expense");
    Console.WriteLine("3. List transactions");
    Console.WriteLine("4. Show balance");
    Console.WriteLine("0. Exit");
    Console.WriteLine();
    Console.Write("Choose an option: ");

    string? option = Console.ReadLine();

    switch (option)
    {
        case "1":
            AddTransaction(TransactionType.Income);
            break;
        case "2":
            AddTransaction(TransactionType.Expense);
            break;
        case "3":
            ListTransactions();
            break;
        case "4":
            ShowBalance();
            break;
        case "0":
            return;
        default:
            Pause("Invalid option.");
            break;
    }
}



void AddTransaction(TransactionType type)
{
    ClearScreen();
    Console.WriteLine(type == TransactionType.Income ? "Add income" : "Add expense");
    Console.WriteLine();

    Console.Write("Description: ");
    string description = Console.ReadLine() ?? "";

    TransactionCategory category = ReadCategory();

    Console.Write("Amount: ");
    string? amountText = Console.ReadLine();

    if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
    {
        Pause("Amount must be a positive number.");
        return;
    }

    Transaction transaction = new(
        transactions.Count + 1,
        description,
        category,
        amount,
        type,
        DateTime.Today);

    transactions.Add(transaction);
    Pause("Transaction added.");
}

void ListTransactions()
{
    ClearScreen();
    Console.WriteLine("Transactions");
    Console.WriteLine();

    if (transactions.Count == 0)
    {
        Pause("No transactions yet.");
        return;
    }

    foreach (Transaction transaction in transactions)
    {
        string sign = transaction.Type == TransactionType.Income ? "+" : "-";
        Console.WriteLine(
            $"{transaction.Id}. {transaction.Date:yyyy-MM-dd} | {transaction.Category} | {transaction.Description} | {sign}{transaction.Amount:C}");
    }

    Pause();
}

void ShowBalance()
{
    decimal income = transactions
        .Where(transaction => transaction.Type == TransactionType.Income)
        .Sum(transaction => transaction.Amount);

    decimal expenses = transactions
        .Where(transaction => transaction.Type == TransactionType.Expense)
        .Sum(transaction => transaction.Amount);

    ClearScreen();
    Console.WriteLine("Balance");
    Console.WriteLine();
    Console.WriteLine($"Income:   {income:C}");
    Console.WriteLine($"Expenses: {expenses:C}");
    Console.WriteLine($"Total:    {income - expenses:C}");

    Pause();
}

TransactionCategory ReadCategory()
{
    Console.WriteLine("Category:");
    Console.WriteLine("1. Food");
    Console.WriteLine("2. Transport");
    Console.WriteLine("3. Housing");
    Console.WriteLine("4. Salary");
    Console.WriteLine("5. Entertainment");
    Console.WriteLine("6. Health");
    Console.WriteLine("7. Other");
    Console.Write("Choose a category: ");

    string? option = Console.ReadLine();

    return option switch
    {
        "1" => TransactionCategory.Food,
        "2" => TransactionCategory.Transport,
        "3" => TransactionCategory.Housing,
        "4" => TransactionCategory.Salary,
        "5" => TransactionCategory.Entertainment,
        "6" => TransactionCategory.Health,
        _ => TransactionCategory.Other
    };
}


void Pause(string? message = null)
{
    if (!string.IsNullOrWhiteSpace(message))
    {
        Console.WriteLine();
        Console.WriteLine(message);
    }

    Console.WriteLine();
    Console.Write("Press Enter to continue...");
    Console.ReadLine();
}

void ClearScreen()
{
    if (!Console.IsInputRedirected && !Console.IsOutputRedirected)
    {
        Console.Clear();
    }
}
