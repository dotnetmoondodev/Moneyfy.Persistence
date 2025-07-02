namespace Persistence;

public static class Constants
{
    public const string JsonFileName = "appsettings.json";
    public const string DatabaseName = "MoneyfyDB";

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
        public const string Name = "MongoDB";
        public static readonly IEnumerable<string> Tags = ["ready"];
        public const long TimeoutSeconds = 5;
    }
}
