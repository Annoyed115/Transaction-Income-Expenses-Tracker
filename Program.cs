List<Transaction> transactions = TransactionStorage.Load();

while (true)
{
    ClearScreen();
    Console.WriteLine("Transaction Tracker");
    Console.WriteLine("-------------------");
    Console.WriteLine("1. Add income");
    Console.WriteLine("2. Add expense");
    Console.WriteLine("3. List transactions");
    Console.WriteLine("4. Show balance");
    Console.WriteLine("5. Filter by category");
    Console.WriteLine("6. Filter by month");
    Console.WriteLine("7. Show monthly balance");
    Console.WriteLine("8. Delete transaction");
    Console.WriteLine("9. Edit transaction");
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
        case "5":
            FilterByCategory();
            break;
        case "6":
            FilterByMonth();
            break;
        case "7":
            ShowMonthlyBalance();
            break;
        case "8":
            DeleteTransaction();
            break;
        case "9":
            EditTransaction();
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

    DateTime date = DateTime.Today;

    Console.Write("Amount: ");
    string? amountText = Console.ReadLine();

    if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
    {
        Pause("Amount must be a positive number.");
        return;
    }

    Transaction transaction = new(
        TransactionService.GetNextTransactionId(transactions),
        description,
        category,
        amount,
        type,
        date);

    transactions.Add(transaction);
    TransactionStorage.Save(transactions);
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

    PrintTransactions(transactions);

    Pause();
}

void ShowBalance()
{
    (decimal income, decimal expenses, decimal total) = TransactionService.CalculateBalance(transactions);

    ClearScreen();
    Console.WriteLine("Balance");
    Console.WriteLine();
    Console.WriteLine($"Income:   {income:C}");
    Console.WriteLine($"Expenses: {expenses:C}");
    Console.WriteLine($"Total:    {total:C}");

    Pause();
}

void ShowMonthlyBalance()
{
    ClearScreen();
    if (!TryReadYearAndMonth(out int year, out int month))
    {
        return;
    }

    List<Transaction> monthlyTransactions = TransactionService.GetByMonth(transactions, year, month);

    (decimal income, decimal expenses, decimal total) = TransactionService.CalculateBalance(monthlyTransactions);

    ClearScreen();
    Console.WriteLine($"Balance for {year}-{month:00}");
    Console.WriteLine();
    Console.WriteLine($"Income:   {income:C}");
    Console.WriteLine($"Expenses: {expenses:C}");
    Console.WriteLine($"Total:    {total:C}");

    Pause();
}

void FilterByCategory()
{
    ClearScreen();
    TransactionCategory category = ReadCategory();

    List<Transaction> filteredTransactions = TransactionService.GetByCategory(transactions, category);

    ClearScreen();
    Console.WriteLine($"Transactions in {category}");
    Console.WriteLine();

    if (filteredTransactions.Count == 0)
    {
        Pause("No transactions found for this category.");
        return;
    }

    PrintTransactions(filteredTransactions);

    Pause();
}

void FilterByMonth()
{
    ClearScreen();
    if (!TryReadYearAndMonth(out int year, out int month))
    {
        return;
    }

    List<Transaction> filteredTransactions = TransactionService.GetByMonth(transactions, year, month);

    ClearScreen();
    Console.WriteLine($"Transactions for {year}-{month:00}");
    Console.WriteLine();

    if (filteredTransactions.Count == 0)
    {
        Pause("No transactions found for this month.");
        return;
    }

    PrintTransactions(filteredTransactions);

    Pause();
}

void DeleteTransaction()
{
    ClearScreen();
    Console.WriteLine("Delete transaction");
    Console.WriteLine();

    if (transactions.Count == 0)
    {
        Pause("No transactions yet.");
        return;
    }

    PrintTransactions(transactions);
    Console.WriteLine();
    Console.Write("Transaction Id: ");
    string? idText = Console.ReadLine();

    if (!int.TryParse(idText, out int id))
    {
        Pause("Id must be a valid number.");
        return;
    }

    Transaction? transactionToDelete = transactions
        .FirstOrDefault(transaction => transaction.Id == id);

    if (transactionToDelete is null)
    {
        Pause("Transaction not found.");
        return;
    }

    transactions.Remove(transactionToDelete);
    TransactionStorage.Save(transactions);
    Pause("Transaction deleted.");
}

void EditTransaction()
{
    ClearScreen();
    Console.WriteLine("Edit transaction");
    Console.WriteLine();

    if (transactions.Count == 0)
    {
        Pause("No transactions yet.");
        return;
    }

    PrintTransactions(transactions);
    Console.WriteLine();
    Console.Write("Transaction Id: ");
    string? idText = Console.ReadLine();

    if (!int.TryParse(idText, out int id))
    {
        Pause("Id must be a valid number.");
        return;
    }

    int transactionIndex = transactions.FindIndex(transaction => transaction.Id == id);

    if (transactionIndex == -1)
    {
        Pause("Transaction not found.");
        return;
    }

    Transaction currentTransaction = transactions[transactionIndex];

    Console.WriteLine();
    Console.WriteLine("Press Enter to keep the current value.");
    Console.Write($"Description ({currentTransaction.Description}): ");
    string? descriptionText = Console.ReadLine();

    Console.Write($"Amount ({currentTransaction.Amount}): ");
    string? amountText = Console.ReadLine();

    Console.Write($"Change category ({currentTransaction.Category})? (y/n): ");
    string? changeCategoryText = Console.ReadLine();

    Console.Write($"Change type ({currentTransaction.Type})? (y/n): ");
    string? changeTypeText = Console.ReadLine();

    string description = string.IsNullOrWhiteSpace(descriptionText)
        ? currentTransaction.Description
        : descriptionText;

    decimal amount = currentTransaction.Amount;
    if (!string.IsNullOrWhiteSpace(amountText)
        && (!decimal.TryParse(amountText, out amount) || amount <= 0))
    {
        Pause("Amount must be a positive number.");
        return;
    }

    TransactionCategory category = currentTransaction.Category;
    if (changeCategoryText?.Trim().ToLower() == "y")
    {
        category = ReadCategory();
    }

    TransactionType type = currentTransaction.Type;
    if (changeTypeText?.Trim().ToLower() == "y")
    {
        type = currentTransaction.Type == TransactionType.Income
            ? TransactionType.Expense
            : TransactionType.Income;
    }

    Transaction updatedTransaction = currentTransaction with
    {
        Description = description,
        Category = category,
        Amount = amount,
        Type = type,
    };

    transactions[transactionIndex] = updatedTransaction;
    TransactionStorage.Save(transactions);
    Pause("Transaction updated.");
}

void PrintTransactions(List<Transaction> transactionsToPrint)
{
    foreach (Transaction transaction in transactionsToPrint.OrderByDescending(transaction => transaction.Date))
    {
        string sign = transaction.Type == TransactionType.Income ? "+" : "-";
        Console.WriteLine(
            $"{transaction.Id}. {transaction.Date:yyyy-MM-dd} | {transaction.Category} | {transaction.Description} | {sign}{transaction.Amount:C}");
    }
}

bool TryReadYearAndMonth(out int year, out int month)
{
    Console.Write("Year: ");
    string? yearText = Console.ReadLine();

    Console.Write("Month (1-12): ");
    string? monthText = Console.ReadLine();

    if (!int.TryParse(yearText, out year) || year < 1)
    {
        month = 0;
        Pause("Year must be a valid number.");
        return false;
    }

    if (!int.TryParse(monthText, out month) || month < 1 || month > 12)
    {
        Pause("Month must be between 1 and 12.");
        return false;
    }

    return true;
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
