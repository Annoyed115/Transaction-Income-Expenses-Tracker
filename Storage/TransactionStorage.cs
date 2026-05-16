using System.Text.Json;
using System.Text.Json.Serialization;

static class TransactionStorage
{
    private const string DataFilePath = "transactions.json";

    private static readonly JsonSerializerOptions JsonOptions = CreateJsonOptions();

    public static List<Transaction> Load()
    {
        if (!File.Exists(DataFilePath))
        {
            return [];
        }

        string json = File.ReadAllText(DataFilePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<Transaction>>(json, JsonOptions) ?? [];
    }

    public static void Save(List<Transaction> transactions)
    {
        string json = JsonSerializer.Serialize(transactions, JsonOptions);
        File.WriteAllText(DataFilePath, json);
    }

    private static JsonSerializerOptions CreateJsonOptions()
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }
}
