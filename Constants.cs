namespace Persistence;

public static class Constants
{
    public static class AppSettings
    {
        public const string JsonFileName = "appsettings.json";
        public const string DBConnName = "SqlConnection";
    }

    public static class CacheSettings
    {
        public const long ExpirationTime = 5;
        public const string Expenses = "ExpensesCache";
        public const string Incomes = "IncomesCache";
        public const string Payments = "PaymentsCache";
        public const string Notifications = "NotificationsCache";
    }

    public static class LoggerSettings
    {
        public const string ExpensesFileName = "expe-webapi";
        public const string IncomesFileName = "inco-webapi";
        public const string PaymentsFileName = "paym-webapi";
        public const string NotificationsFileName = "noti-webapi";
    }

}
