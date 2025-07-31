namespace Persistence;

public static class Constants
{
    public const string ThisAssemblyName = "Persistence";
    public const string JsonFileName = "appsettings.json";

    public static class CacheSettings
    {
        public const long ExpirationTime = 5;
        public const string Expenses = "ExpensesCache";
        public const string Incomes = "IncomesCache";
        public const string Payments = "PaymentsCache";
        public const string Notifications = "NotificationsCache";
    }

    public static class HealthCheckSettings
    {
        public const string Query = "select 1;";
        public const string Name = "Sql-Server";
        public static readonly IEnumerable<string> Tags = ["ready"];
        public const long TimeoutSeconds = 5;
    }
}
